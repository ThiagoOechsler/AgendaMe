using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{

    public class BairroController : Controller
    {
        AgendaMeEntities contexto = new AgendaMeEntities();
        
        public ActionResult Index()
        {
            List<Bairro> lst = contexto.CreateObjectSet<Bairro>().ToList<Bairro>();

            foreach (Bairro b in lst)
            {
                b.DsCidade = b.BuscaNomeCidade();
            }
            return View(lst);
        }

        // GET: /Bairro/Details/5

        public ActionResult Details(int id)
        {
            var bairro = (from bro in contexto.Bairro where bro.CdBairro == id select bro).First();
            return View(bairro);
        }

        // GET: /Bairro/Create

        public ActionResult Create()
        {
            ViewData["LstCidade"] = new SelectList(contexto.Cidade.ToList(), "CdCidade", "DsNome");
            return View();
        } 

        // POST: /Bairro/Create

        [HttpPost]
        public ActionResult Create(Bairro bairro)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(bairro);
                }
                // TODO: Add insert logic here
                contexto.AddToBairro(bairro);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        // GET: /Bairro/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewData["LstCidade"] = new SelectList(contexto.Cidade.ToList(), "CdCidade", "DsNome");
            var bairro = (from bro in contexto.Bairro where bro.CdBairro == id select bro).First();
            return View(bairro);
            
        }

        // POST: /Bairro/Edit/5

        [HttpPost]
        public ActionResult Edit(Bairro ebairro)
        {
            try
            {
                // TODO: Add update logic here
                var original = (from bro in contexto.Bairro where bro.CdBairro == ebairro.CdBairro select bro).First();
                
                if (!ModelState.IsValid)
                    return View();

                contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, ebairro);
                contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: /Bairro/Delete/5
 
        public ActionResult Delete(int id)
        {
            //ViewData["LstBairro"] = new SelectList(contexto.Bairro.ToList(), "CdBairro", "DsNome");
            var bairro = (from bro in contexto.Bairro where bro.CdBairro == id select bro).First();
            return View(bairro);
        }

        // POST: /Bairro/Delete/5

        [HttpPost]
        public ActionResult Delete(Bairro bairro)
        {
            try
            {
                // obtem o bairro a excluir
                var delbairro = (from bro in contexto.Bairro where bro.CdBairro == bairro.CdBairro select bro).First();

                //excluir
                contexto.DeleteObject(delbairro);
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
