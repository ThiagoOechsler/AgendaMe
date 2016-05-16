using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaMeMVC3.Models
{
    public partial class Servico
    {
        public String DsTipoServico;
        public String DsProfissional;

        public String BuscaTipoServico()
        {
            SQLProvider banco = new SQLProvider();

            return banco.RetornaValorString("Select DsNome from TipoServico where cdTipoServico = " + this.CdTipoServico);

        }    
   
        public String BuscaProfissional()
        {
            SQLProvider banco = new SQLProvider();

            return banco.RetornaValorString("Select Pessoa.DsNome from Pessoa, Profissional where Pessoa.CdPessoa = Profissional.CdPessoa AND Profissional.CdProfissional = " + this.CdProfissional);

        }    
    }
}