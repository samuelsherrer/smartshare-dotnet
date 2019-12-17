using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SmartShareClient.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartShareClient
{
    public class SmartShareClient : ISmartShareClient
    {
        private readonly SmartShareOptions _options;

        public SmartShareClient(SmartShareOptions options) =>
            _options = options;

        public async Task<ValidarLoginResponse> ValidarLogin()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("dsCliente", _options.ClientId);
                    client.DefaultRequestHeaders.Add("dsChaveAutenticacao", _options.ClientKey);

                    var validaLogin = new ValidarLoginRequest()
                    {
                        dsUsuario = _options.User,
                        dsSenha = _options.Password
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(validaLogin), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{_options.Endpoint}/api/v3/Usuario/ValidarLogin", content);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new SmartShareException("Cliente não autorizado.");

                    string resultado = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(JsonConvert.DeserializeObject<ErroResponse>(resultado).Message);

                    return JsonConvert.DeserializeObject<ValidarLoginResponse>(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new SmartShareException(ex.Message);
            }
        }

        public async Task<ListaDocumentosResponse> ObterDocumento(int idDocumento, string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            try
            {
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("tokenUsuario", token);
                    client.DefaultRequestHeaders.Add("stArquivo", "true");

                    var response = await client.GetAsync($"{_options.Endpoint}/api/v2/Documento/{idDocumento}");

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new SmartShareException("Cliente não autorizado.");

                    string resultado = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(JsonConvert.DeserializeObject<ErroResponse>(resultado).Message);

                    var documentoResponseHeader = JsonConvert.DeserializeObject<ObterDocumentoResponse>(resultado).versaoDocumentoInfo;

                    return documentoResponseHeader;
                }
            }
            catch (Exception ex)
            {
                throw new SmartShareException(ex.Message);
            }
        }

        public async Task<IList<ListaDocumentosResponse>> ListarDocumentos(ListaDocumentoRequest listaDocumentoRequest, string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            try
            {
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("tokenUsuario", token);

                    var content = new StringContent(JsonConvert.SerializeObject(listaDocumentoRequest), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{_options.Endpoint}/api/v2/Documento/ListaDocumentos", content);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new SmartShareException("Cliente não autorizado.");

                    string resultado = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(JsonConvert.DeserializeObject<ErroResponse>(resultado).Message);

                    return JsonConvert.DeserializeObject<IList<ListaDocumentosResponse>>(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new SmartShareException(ex.Message);
            }
        }

        public async Task<UploadDocumentoResponse> UploadArquivo(UploadDocumentoRequest documentoRequest, string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("tokenUsuario", token);

                    var content = new StringContent(JsonConvert.SerializeObject(documentoRequest), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{_options.Endpoint}/api/v2/Documento", content);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new SmartShareException("Cliente não autorizado.");

                    string resultado = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(JsonConvert.DeserializeObject<ErroResponse>(resultado).Message);

                    return JsonConvert.DeserializeObject<UploadDocumentoResponse>(resultado);
                }
            }
            catch (Exception ex)
            {
                throw new SmartShareException(ex.Message);
            }
        }

        public async Task<bool> ExcluirDocumento(int cdDocumento, int cdVersao, string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("tokenUsuario", token);
                    client.DefaultRequestHeaders.Add("cdDocumento", cdDocumento.ToString());
                    client.DefaultRequestHeaders.Add("cdVersao", cdVersao.ToString());

                    HttpResponseMessage response = await client.DeleteAsync($"{_options.Endpoint}/api/v2/Documento/");

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new Exception("Cliente não autenticado!");

                    string resultado = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(JsonConvert.DeserializeObject<ErroResponse>(resultado).Message);

                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                throw new SmartShareException(ex.Message);
            }
        }
    }
}