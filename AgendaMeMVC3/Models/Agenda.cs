using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaMeMVC3.Models
{
    public partial class Agenda
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public String text { get; set; }
        public int section_id { get; set; }
        public int servico_id { get; set; }
        public int cliente_id { get; set; }
        public int id { get; set; }
    }
}