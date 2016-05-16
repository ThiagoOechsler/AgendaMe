using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaMeMVC3.Models
{
    public partial class Pessoa
    {
        public String DsNomeCompleto
        {
            get
            {
                return (this.DsNome + " " + this.DsSobrenome);
            }
        }

    }
}