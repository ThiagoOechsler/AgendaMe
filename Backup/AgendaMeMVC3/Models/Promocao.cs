using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaMeMVC3.Models
{
    public partial class Promocao
    {
        public String DsEstabelecimento;

        public String BuscaNomeEstabelecimento()
        {
            SQLProvider banco = new SQLProvider();

            return banco.RetornaValorString("Select DsNome from Estabelecimento where cdEstabelecimento = " + this.CdEstabelecimento);

        }
    }
}