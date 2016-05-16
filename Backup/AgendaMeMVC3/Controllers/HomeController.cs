using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{
    public class HomeController : Controller
    {
        AgendaMeEntities contexto = new AgendaMeEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult EnviarEmailLembrete()
        {
            var cliente = (from cli in contexto.Cliente where cli.CdCliente == 1 select cli).First();
            new ServicoDeEmailController().Lembrete(cliente).Deliver();
            return View();
        }
    }
}
