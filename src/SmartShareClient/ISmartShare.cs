using SmartShareClient.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartShareClient
{
    public interface ISmartShare
    {
        Task<GenerateTokenResponse> GenerateTokenAsync();
        Task<ListaDocumentosResponse> GetDocumentAsync(int idDocumento);
        Task<ErroResponse> DeleteDocumentAsync(int cdDocumento, int cdVersao);
        Task<IList<ListaDocumentosResponse>> ListDocumentsAsync(ListaDocumentoRequest documentoRequest);
        Task<UploadDocumentoResponse> UploadDocumentAsync(UploadDocumentoRequest documentoRequest);
    }
}
