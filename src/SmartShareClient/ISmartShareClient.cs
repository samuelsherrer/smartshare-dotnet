using SmartShareClient.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartShareClient
{
    public interface ISmartShareClient
    {
        Task<ValidarLoginResponse> ValidarLogin();
        Task<ListaDocumentosResponse> ObterDocumento(int idDocumento, string tokenSmartshare);
        Task<bool> ExcluirDocumento(int cdDocumento, int cdVersao, string token);
        Task<IList<ListaDocumentosResponse>> ListarDocumentos(ListaDocumentoRequest documentoRequest, string token);
        Task<UploadDocumentoResponse> UploadArquivo(UploadDocumentoRequest documentoRequest, string token);
    }
}
