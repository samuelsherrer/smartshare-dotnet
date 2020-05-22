using Newtonsoft.Json;
using SmartShareClient.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using SmartShareClient.Serializers;

namespace SmartShareClient
{
    public class SmartShareClient : ISmartShareClient
    {
        private readonly SmartShareOptions _options;
        private readonly RestClient _client;

        public SmartShareClient(SmartShareOptions options) =>
            _options = options;

        private RestRequest ConfigureRequestAuthentication(string path, Method method, string token = null)
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

        public async Task<GenerateTokenResponse> GenerateToken()
        {
            var path = $"v3/Usuario/ValidarLogin";
            var request = ConfigureRequestAuthentication(path, Method.POST);

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

        public async Task<ListaDocumentosResponse> GetDocument(int idDocumento)
        {
            var authentication = await GenerateToken();

            string path = $"v2/Documento/{idDocumento}";
            var request = ConfigureRequestAuthentication(path, Method.GET, authentication.tokenUsuario);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            var documentoResponseHeader = JsonConvert.DeserializeObject<ObterDocumentoResponse>(response.Content).versaoDocumentoInfo;

            return documentoResponseHeader;
        }

        public async Task<IList<ListaDocumentosResponse>> ListDocuments(ListaDocumentoRequest listaDocumentoRequest)
        {
            var authentication = await GenerateToken();

            string path = "v2/Documento/ListaDocumentos";
            var request = ConfigureRequestAuthentication(path, Method.POST, authentication.tokenUsuario);

            request.AddJsonBody(listaDocumentoRequest);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return JsonConvert.DeserializeObject<IList<ListaDocumentosResponse>>(response.Content);
        }

        public async Task<UploadDocumentoResponse> UploadDocument(UploadDocumentoRequest documentoRequest)
        {
            var authentication = await GenerateToken();

            string path = "v2/Documento";
            var request = ConfigureRequestAuthentication(path, Method.POST, authentication.tokenUsuario);

            request.AddJsonBody(documentoRequest);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return JsonConvert.DeserializeObject<UploadDocumentoResponse>(response.Content);
        }

        public async Task<ErroResponse> DeleteDocument(int cdDocumento, int cdVersao)
        {
            var authentication = await GenerateToken();

            string path = "v2/Documento";
            var request = ConfigureRequestAuthentication(path, Method.DELETE, authentication.tokenUsuario);

            request.AddHeader("cdDocumento", cdDocumento.ToString());
            request.AddHeader("cdVersao", cdVersao.ToString());

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return JsonConvert.DeserializeObject<ErroResponse>(response.Content);
        }
    }
}