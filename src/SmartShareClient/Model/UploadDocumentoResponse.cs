using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class UploadDocumentoResponse
    {
        public int cdVersaoDocumento { get; set; }
        public int cdDocumento { get; set; }
        public int cdVersao { get; set; }
        public string message { get; set; }
    }
}
