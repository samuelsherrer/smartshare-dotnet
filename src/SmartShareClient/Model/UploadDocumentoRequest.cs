using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class UploadDocumentoRequest
    {
        public string dsTitulo { get; set; }
        public string dsArquivoOriginal { get; set; }
        public int cdTipoDocumento { get; set; }
        public string dtEfetivacao { get; set; }
        public string dtVencimento { get; set; }
        public int idTipoRevisao { get; set; }
        public int cdPasta { get; set; }
        public List<Indice> lstIndicesArray { get; set; }
        public List<Perfil> lstPerfilArray { get; set; }
        public bool stGerarAprovacao { get; set; }
        public string file { get; set; }
    }
}
