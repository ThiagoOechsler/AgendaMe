using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaMeMVC3.Models
{
    public class AgendaLivreLista
    {
        public List<AgendaLivreDia> HorariosLivresOntem { get; set; }
        public List<AgendaLivreDia> HorariosLivresHoje { get; set; }
        public List<AgendaLivreDia> HorariosLivresAmanha { get; set; }

        public AgendaLivreLista(List<AgendaLivreDia> prHorariosLivresOntem, List<AgendaLivreDia> prHorariosLivresHoje, List<AgendaLivreDia> prHorariosLivresAmanha)
        {
            this.HorariosLivresOntem = prHorariosLivresOntem;
            this.HorariosLivresHoje = prHorariosLivresHoje;
            this.HorariosLivresAmanha = prHorariosLivresAmanha;
        }
    }
}