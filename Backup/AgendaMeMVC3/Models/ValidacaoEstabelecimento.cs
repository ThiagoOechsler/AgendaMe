using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AgendaMeMVC3.Models
{
    [MetadataType(typeof(EstabelecimentoMetadata))]
    public partial class Estabelecimento
    {
    }

    [Bind(Exclude = "CdEstabelecimento")]
    public class EstabelecimentoMetadata
    {
        [Required(ErrorMessage = "Por favor, informe o CPF/CNPJ.")]
        public string DsCpfCnpj { get; set; }

        [Required(ErrorMessage = "Por favor, informe o Nome.")]
        public string DsNome { get; set; }

        [Required(ErrorMessage = "Por favor, informe o horário de atendimento")]
        public string HrAtendInicioManha { get; set; }

        [Required(ErrorMessage = "Por favor, informe o horário de atendimento")]
        public string HrAtendFimManha { get; set; }

        [Required(ErrorMessage = "Por favor, informe o horário de atendimento")]
        public string HrAtendInicioTarde { get; set; }

        [Required(ErrorMessage = "Por favor, informe o horário de atendimento")]
        public string HrAtendFimTarde { get; set; }
    }
}