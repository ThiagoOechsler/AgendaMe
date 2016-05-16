using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AgendaMeMVC3.Models
{
    public partial class Estabelecimento
    {
        public String DsBairro;

        public String DsHoraFimManha;
        public String DsHoraFimTarde;
        public String DsHoraIniManha;
        public String DsHoraIniTarde;

        public void TrataHoraApresentacao()
        {
            if (this.HrAtendFimManha != null)
            {
                this.DsHoraFimManha = Convert.ToString(this.HrAtendFimManha).Substring(11, 5);
            }

            if (this.HrAtendFimTarde != null)
            {
                this.DsHoraFimTarde = Convert.ToString(this.HrAtendFimTarde).Substring(11, 5);
            }

            if (this.HrAtendInicioManha != null)
            {
                this.DsHoraIniManha = Convert.ToString(this.HrAtendInicioManha).Substring(11, 5);
            }

            if (this.HrAtendInicioTarde != null)
            {
                this.DsHoraIniTarde = Convert.ToString(this.HrAtendInicioTarde).Substring(11, 5);
            }
        }

        public String BuscaNomeBairro()
        {
            SQLProvider banco = new SQLProvider();

            return banco.RetornaValorString("Select DsBairro from Bairro where cdBairro = " + this.CdBairro);

        }

        public String DsEmpresa;

        public String BuscaNomeEmpresa()
        {
            SQLProvider banco = new SQLProvider();

            return banco.RetornaValorString("Select DsNome from Empresa where cdEmpresa = " + this.CdEmpresa);

        }
    }
}