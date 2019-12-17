using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class ValidarLoginResponse
    {
        public int cdUsuario { get; set; }
        public string tokenUsuario { get; set; }
    }
}
