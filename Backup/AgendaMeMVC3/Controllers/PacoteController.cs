using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{
    public class PacoteController : Controller
    {
        AgendaMeEntities contexto = new AgendaMeEntities();
        // GET: /Pacote/

        public ActionResult Index()
        {
            List<Pacote> lst = contexto.CreateObjectSet<Pacote>().ToList<Pacote>();
            return View(lst);
        }

        //
        // GET: /Pacote/Details/5

        public ActionResult Details(int id)
        {
            var pacote = (from pct in contexto.Pacote where pct.CdPacote == id select pct).First();
            return View(pacote);
        }

        //
        // GET: /Pacote/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Pacote/Create

        [HttpPost]
        public ActionResult Create(Pacote pacote)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(pacote);
                }
                // TODO: Add insert logic here
                contexto.AddToPacote(pacote);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Pacote/Edit/5
 
        public ActionResult Edit(int id)
        {
            var pacote = (from pct in contexto.Pacote where pct.CdPacote == id select pct).First();
            return View(pacote);
        }

        // POST: /Pacote/Edit/5

        [HttpPost]
        public ActionResult Edit(Pacote pacote)
        {
            try
            {
                // TODO: Add update logic here
                var original = (from pct in contexto.Pacote where pct.CdPacote == pacote.CdPacote select pct).First();

                if (!ModelState.IsValid)
                    return View();

                contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, pacote);
                contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Pacote/Delete/5
 
        public ActionResult Delete(int id)
        {
            var pacote = (from pct in contexto.Pacote where pct.CdPacote == id select pct).First();
            return View(pacote);
        }

        //
        // POST: /Pacote/Delete/5

        [HttpPost]
        public ActionResult Delete(Pacote pacote)
        {
            try
            {
                // obtem o bairro a excluir
                var delPacote = (from agd in contexto.Pacote where agd.CdPacote == pacote.CdPacote select agd).First();

                //excluir
                contexto.DeleteObject(delPacote);
                contexto.SaveChanges();

                //exibe a view index
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
