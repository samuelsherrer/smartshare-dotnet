using Newtonsoft.Json;
using SmartShareClient.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using SmartShareClient.Serializers;
using Microsoft.Extensions.Configuration;

namespace SmartShareClient
{
    public class SmartShareClient : ISmartShareClient
    {
        private readonly SmartShareOptions _options;
        private readonly RestClient _client;

        public SmartShareClient(SmartShareOptions options) : this()
            => _options = options;

        public SmartShareClient(IConfiguration configuration) : this()
            => _options = JsonConvert.DeserializeObject<SmartShareOptions>(configuration.GetSection("SmartShare").Value);

        public SmartShareClient()
            => this._client = new RestClient(_options.Endpoint);

        private RestRequest ConfigureRequest(string path, Method method, string token = null)
        {
            string endpoint = $"{_options.Endpoint}/{path}";
            RestRequest request = new RestRequest(endpoint, method, DataFormat.Json);

            request.AddHeader("dsCliente", _options.ClientId);
            request.AddHeader("dsChaveAutenticacao", _options.ClientKey);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("tokenUsuario", token);

            request.JsonSerializer = new NewtonsoftRestSharpSerializer();
            return request;
        }

        public async Task<GenerateTokenResponse> GenerateTokenAsync()
        {
            var path = $"v3/Usuario/ValidarLogin";
            var request = ConfigureRequest(path, Method.POST);

            var validaLogin = new GenerateTokenRequest()
            {
                dsUsuario = _options.User,
                dsSenha = _options.Password
            };

            request.AddJsonBody(validaLogin);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return JsonConvert.DeserializeObject<GenerateTokenResponse>(response.Content);
        }

        public async Task<ListaDocumentosResponse> GetDocumentAsync(int idDocumento)
        {
            var authentication = await GenerateTokenAsync();

            string path = $"v2/Documento/{idDocumento}";
            var request = ConfigureRequest(path, Method.GET, authentication.TokenUsuario);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            var documentoResponseHeader = JsonConvert.DeserializeObject<ObterDocumentoResponse>(response.Content).versaoDocumentoInfo;

            return documentoResponseHeader;
        }

        public async Task<IList<ListaDocumentosResponse>> ListDocumentsAsync(ListaDocumentoRequest listaDocumentoRequest)
        {
            var authentication = await GenerateTokenAsync();

            string path = "v2/Documento/ListaDocumentos";
            var request = ConfigureRequest(path, Method.POST, authentication.TokenUsuario);

            request.AddJsonBody(listaDocumentoRequest);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return JsonConvert.DeserializeObject<IList<ListaDocumentosResponse>>(response.Content);
        }

        public async Task<UploadDocumentoResponse> UploadDocumentAsync(UploadDocumentoRequest documentoRequest)
        {
            var authentication = await GenerateTokenAsync();

            string path = "v2/Documento";
            var request = ConfigureRequest(path, Method.POST, authentication.TokenUsuario);

            request.AddJsonBody(documentoRequest);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return JsonConvert.DeserializeObject<UploadDocumentoResponse>(response.Content);
        }

        public async Task<ErroResponse> DeleteDocumentAsync(int cdDocumento, int cdVersao)
        {
            var authentication = await GenerateTokenAsync();

            string path = "v2/Documento";
            var request = ConfigureRequest(path, Method.DELETE, authentication.TokenUsuario);

            request.AddHeader("cdDocumento", cdDocumento.ToString());
            request.AddHeader("cdVersao", cdVersao.ToString());

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return JsonConvert.DeserializeObject<ErroResponse>(response.Content);
        }
    }
}