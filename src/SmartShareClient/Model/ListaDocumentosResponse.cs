using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartShareClient.Model
{
    public class ListaDocumentosResponse
    {
        public int cdVersaoDocumento { get; set; }
        public int cdDocumento { get; set; }
        public int cdVersao { get; set; }
        public DateTime dtEfetivacao { get; set; }
        public DateTime dtVencimento { get; set; }
        public string dsTitulo { get; set; }
        public int cdTipoDocumento { get; set; }
        public string dsTipoDocumento { get; set; }
        public string dsArquivoOriginal { get; set; }

        [JsonPropertyName("lstIndices")]
        public List<Indice> ListaIndices { get; set; }
        public int idStatusDocumento { get; set; }

        [JsonPropertyName("file")]
        private string _file { get; set; }
    }
}
