using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace AgendaMeMVC3.Models
{
    public partial class Bairro
    {
        public String DsCidade;

        public String BuscaNomeCidade()
        {            
            SQLProvider banco = new SQLProvider();

            return banco.RetornaValorString("Select DsNome from Cidade where cdCidade = " + this.CdCidade);

        }
    }
}