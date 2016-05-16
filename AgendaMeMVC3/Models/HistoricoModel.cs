using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaMeMVC3.Models
{
    public class HistoricoModel
    {
        public String DsNomeServico { get; set; } // nome do serviço
        public String DsNomeProfissional { get; set; } // nome do profissional
        public DateTime DtInicio { get; set; }
        public DateTime DtFim { get; set; }
        public DateTime? DtCancelamento { get; set; }
        public String DsMotivoCancel { get; set; }
    }
}