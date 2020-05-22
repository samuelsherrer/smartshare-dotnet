using SmartShareClient.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartShareClient
{
    public interface ISmartShareClient
    {
        Task<GenerateTokenResponse> GenerateToken();
        Task<ListaDocumentosResponse> GetDocument(int idDocumento);
        Task<ErroResponse> DeleteDocument(int cdDocumento, int cdVersao);
        Task<IList<ListaDocumentosResponse>> ListDocuments(ListaDocumentoRequest documentoRequest);
        Task<UploadDocumentoResponse> UploadDocument(UploadDocumentoRequest documentoRequest);
    }
}
