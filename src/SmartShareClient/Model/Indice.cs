using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class Indice
    {
        public Indice(int cdTipoIndice, string vlCondicao, string dsValor)
        {
            this.cdTipoIndice = cdTipoIndice;
            this.vlCondicao = vlCondicao;
            this.dsValor = dsValor;
        }

        public Indice()
        {

        }

        public int cdTipoIndice { get; set; }
        public string vlCondicao { get; set; }
        public string dsValor { get; set; }
    }
}
