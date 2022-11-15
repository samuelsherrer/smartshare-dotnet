using SmartShareClient.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SmartShareClient
{
    public class SmartShare : ISmartShare
    {
        private readonly SmartShareOptions _options;
        private readonly RestClient _client;

        public SmartShare(IOptions<SmartShareOptions> options, RestClient client = null)
            : this(options.Value, client)
        {
        }

        public SmartShare(IConfiguration configuration, RestClient client = null)
            : this(configuration.GetSection("SmartShare").Get<SmartShareOptions>(), client)
        {
        }

        public SmartShare(SmartShareOptions smartShareOptions, RestClient client = null)
        {
            this._options = smartShareOptions;
            this._client = client ?? new RestClient(smartShareOptions.Endpoint);
        }

        private RestRequest ConfigureRequest(string path, Method method, string token = null)
        {
            var request = new RestRequest(path, method);

            request.AddHeader("dsCliente", _options.ClientId);
            request.AddHeader("dsChaveAutenticacao", _options.ClientKey);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("tokenUsuario", token);

            return request;
        }

        public async Task<GenerateTokenResponse> GenerateTokenAsync()
        {
            var path = $"v3/Usuario/ValidarLogin";
            var request = ConfigureRequest(path, Method.Post);

            var validaLogin = new GenerateTokenRequest()
            {
                dsUsuario = _options.User,
                dsSenha = _options.Password
            };

            request.AddJsonBody(validaLogin);

            var response = await _client.ExecuteAsync<GenerateTokenResponse>(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return response.Data;
        }

        public async Task<ListaDocumentosResponse> GetDocumentAsync(int idDocumento)
        {
            var authentication = await GenerateTokenAsync();

            string path = $"v2/Documento/{idDocumento}";
            var request = ConfigureRequest(path, Method.Get, authentication.TokenUsuario);

            var response = await _client.ExecuteAsync<ObterDocumentoResponse>(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return response.Data.versaoDocumentoInfo;
        }

        public async Task<IList<ListaDocumentosResponse>> ListDocumentsAsync(ListaDocumentoRequest listaDocumentoRequest)
        {
            var authentication = await GenerateTokenAsync();

            string path = "v2/Documento/ListaDocumentos";
            var request = ConfigureRequest(path, Method.Post, authentication.TokenUsuario);

            request.AddJsonBody(listaDocumentoRequest);

            var response = await _client.ExecuteAsync<IList<ListaDocumentosResponse>>(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return response.Data;
        }

        public async Task<UploadDocumentoResponse> UploadDocumentAsync(UploadDocumentoRequest documentoRequest)
        {
            var authentication = await GenerateTokenAsync();

            string path = "v2/Documento";
            var request = ConfigureRequest(path, Method.Post, authentication.TokenUsuario);

            request.AddJsonBody(documentoRequest);

            var response = await _client.ExecuteAsync<UploadDocumentoResponse>(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return response.Data;
        }

        public async Task<ErroResponse> DeleteDocumentAsync(int cdDocumento, int cdVersao)
        {
            var authentication = await GenerateTokenAsync();

            string path = "v2/Documento";
            var request = ConfigureRequest(path, Method.Delete, authentication.TokenUsuario);

            request.AddHeader("cdDocumento", cdDocumento.ToString());
            request.AddHeader("cdVersao", cdVersao.ToString());

            var response = await _client.ExecuteAsync<ErroResponse>(request);

            if (!response.IsSuccessful)
                throw new SmartShareException("Error from SmartShare.", response.Content);

            return response.Data;
        }
    }
}