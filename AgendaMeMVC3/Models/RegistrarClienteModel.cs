using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaMeMVC3.Models
{
    public class RegistrarClienteModel
    {
        public String DsCpfCnpj { get; set; }
        public String DsNome { get; set; }
        public String DsSobrenome { get; set; }
        public DateTime DtNascimento { get; set; }
        public String DsFone1 { get; set; }
        public String DsFone2 { get; set; }
        public String DsEmail { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
        public Boolean BooLembrar { get; set; }
        public Boolean ErrouSenha { get; set; }
    }
}