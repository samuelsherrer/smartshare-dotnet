using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class GenerateTokenRequest
    {
        public string dsUsuario { get; set; }

        public string dsSenha { get; set; }
    }
}
