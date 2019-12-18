using SmartShareClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmartShareClient.Examples
{
    class Program
    {
        static ISmartShareClient _smartShareClient;
        static SmartShareOptions _options = new SmartShareOptions()
        {
            Endpoint = "",
            ClientId = "",
            ClientKey = "",
            User = "",
            Password = ""
        };

        static void Main(string[] args)
        {
            _smartShareClient = new SmartShareClient(_options);
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var validarLogin = await _smartShareClient.ValidarLogin();

            //await UploadoDocumento(validarLogin.tokenUsuario);
            //await ListarDocumentos(validarLogin.tokenUsuario);
            //await ObterDocumento(validarLogin.tokenUsuario);
            //await DeletarDocumento(validarLogin.tokenUsuario);
        }

        static async Task UploadoDocumento(string token)
        {
            // Enviar arquivo.
            var arquivo = new UploadDocumentoRequest()
            {
                dsTitulo = "FILENAME.pdf",
                dsArquivoOriginal = "FILENAME.pdf",
                cdTipoDocumento = 1,
                dtEfetivacao = DateTime.Now.ToString("yyyy/MM/dd"),
                dtVencimento = new DateTime(2099, 10, 10).ToString("yyyy/MM/dd"),
                idTipoRevisao = 1,
                cdPasta = 1,
                lstIndicesArray = new List<Indice>()
                {
                    new Indice(){
                        cdTipoIndice = 1,
                        dsValor = ""
                    }
                },
                lstPerfilArray = new List<Perfil>()
                {
                    new Perfil()
                    {
                        cdPerfil = 1,
                        stRevisao = 1
                    }
                },
                stGerarAprovacao = false,
                file = Convert.ToBase64String(null)
            };

            var result = await _smartShareClient.UploadDocumento(arquivo, token);
        }

        static async Task ListarDocumentos(string token)
        {
            var listaParametros = new ListaDocumentoRequest()
            {
                quantidade = 30,
                stApenasDocumentosAprovados = true,
                lstIndices = new List<Indice>()
                {
                    new Indice()
                    {
                        cdTipoIndice = 1,
                        vlCondicao = "=",
                        dsValor = ""
                    }
                }
            };

            var retorno = await _smartShareClient.ListarDocumentos(listaParametros, token);
        }

        static async Task ObterDocumento(string token)
        {
            var retorno = await _smartShareClient.ObterDocumento(0, token);
        }

        static async Task DeletarDocumento(string token)
        {
            var retorno = await _smartShareClient.ExcluirDocumento(0, 1, token);
        }
    }
}
