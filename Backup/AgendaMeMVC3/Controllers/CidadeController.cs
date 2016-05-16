using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{
    public class CidadeController : Controller
    {
        AgendaMeEntities contexto = new AgendaMeEntities();
        //
        // GET: /Cidade/

        public ActionResult Index()
        {
            List<Cidade> lst = contexto.CreateObjectSet<Cidade>().ToList<Cidade>();
            return View(lst);
        }

        //
        // GET: /Cidade/Details/5

        public ActionResult Details(int id)
        {
            var cidade = (from cid in contexto.Cidade where cid.CdCidade == id select cid).First();
            return View(cidade);
        }

        //
        // GET: /Cidade/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Cidade/Create

        [HttpPost]
        public ActionResult Create(Cidade cidade)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(cidade);
                }
                // TODO: Add insert logic here
                contexto.AddToCidade(cidade);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Cidade/Edit/5
 
        public ActionResult Edit(int id)
        {
            var cidade = (from cid in contexto.Cidade where cid.CdCidade == id select cid).First();
            return View(cidade);
        }

        //
        // POST: /Cidade/Edit/5

        [HttpPost]
        public ActionResult Edit(Cidade ecidade)
        {
            try
            {
                // TODO: Add update logic here
                var original = (from cid in contexto.Cidade where cid.CdCidade == ecidade.CdCidade select cid).First();

                if (!ModelState.IsValid)
                    return View();

                contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, ecidade);
                contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Cidade/Delete/5
 
        public ActionResult Delete(int id)
        {
            var cidade = (from cid in contexto.Cidade where cid.CdCidade == id select cid).First();
            return View(cidade);
        }

        //
        // POST: /Cidade/Delete/5

        [HttpPost]
        public ActionResult Delete(Cidade cidade)
        {
            try
            {
                // obtem o bairro a excluir
                var delcidade = (from cid in contexto.Cidade where cid.CdCidade == cidade.CdCidade select cid).First();

                //excluir
                contexto.DeleteObject(delcidade);
                contexto.SaveChanges();

                //exibe a view index
                return RedirectToAction("Index");
            }
            catch 
            {
                AgendaMeEntities contexto = new AgendaMeEntities();                
               return View();
            }
        }
    }
}
