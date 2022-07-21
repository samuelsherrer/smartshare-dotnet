using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class SmartShareOptions
    {
        public string Endpoint { get; set; }

        /// <summary>
        /// Represents dsCliente
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Represents dsChaveAutenticacao
        /// </summary>
        public string ClientKey { get; set; }

        /// <summary>
        /// Represents dsUsuario
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Represents dsSenha
        /// </summary>
        public string Password { get; set; }
    }
}
