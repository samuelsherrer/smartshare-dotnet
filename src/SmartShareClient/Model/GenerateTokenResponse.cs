using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class GenerateTokenResponse
    {
        public int CdUsuario { get; set; }
        public string TokenUsuario { get; set; }
    }
}
