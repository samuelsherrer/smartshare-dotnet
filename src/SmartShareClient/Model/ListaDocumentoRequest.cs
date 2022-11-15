using System.Collections.Generic;

namespace SmartShareClient.Model
{
    public class ListaDocumentoRequest
    {
        public string dsDocumento { get; set; }
        public int cdTipoDocumento { get; set; }
        public int inicio { get; set; }
        public int quantidade { get; set; }
        public bool stApenasDocumentosAprovados { get; set; }
        public List<Indice> lstIndices { get; set; }
        public string dtEfetivacaoInicial { get; set; }
        public string dtEfetivacaoFinal { get; set; }
        public string dtVencimentoInicial { get; set; }
        public string dtVencimentoFinal { get; set; }
    }
}
