using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class SmartShareOptions
    {
        public string Endpoint { get; set; }

        /// <summary>
        /// dsCliente
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// dsChaveAutenticacao
        /// </summary>
        public string ClientKey { get; set; }

        /// <summary>
        /// dsUsuario
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// dsSenha
        /// </summary>
        public string Password { get; set; }
    }
}
