using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{
    public class PromocaoController : Controller
    {
        AgendaMeEntities contexto = new AgendaMeEntities();
        // GET: /Promocao/

        public ActionResult Index()
        {
            List<Promocao> lst = contexto.CreateObjectSet<Promocao>().ToList<Promocao>();
            foreach (Promocao b in lst)
            {
                b.DsEstabelecimento = b.BuscaNomeEstabelecimento();
            }
            return View(lst);
        }

        //
        // GET: /Promocao/Details/5

        public ActionResult Details(int id)
        {
            var promocao = (from prom in contexto.Promocao where prom.CdPromocao == id select prom).First();
            return View(promocao);
        }

        //
        // GET: /Promocao/Create

        public ActionResult Create()
        {
            ViewData["LstEstabelecimento"] = new SelectList(contexto.Estabelecimento.ToList(), "CdEstabelecimento", "DsNome");
            return View();
        } 

        //
        // POST: /Promocao/Create

        [HttpPost]
        public ActionResult Create(Promocao promocao)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(promocao);
                }
                // TODO: Add insert logic here
                contexto.AddToPromocao(promocao);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Promocao/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewData["LstEstabelecimento"] = new SelectList(contexto.Estabelecimento.ToList(), "CdEstabelecimento", "DsNome");
            var promocao = (from prom in contexto.Promocao where prom.CdPromocao == id select prom).First();
            return View(promocao);
        }

        //
        // POST: /Promocao/Edit/5

        [HttpPost]
        public ActionResult Edit(Promocao promocao)
        {
            try
            {
                // TODO: Add update logic here
                var original = (from prom in contexto.Promocao where prom.CdPromocao == promocao.CdPromocao select prom).First();

                if (!ModelState.IsValid)
                    return View();

                contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, promocao);
                contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Promocao/Delete/5
 
        public ActionResult Delete(int id)
        {
            var promocao = (from prom in contexto.Promocao where prom.CdPromocao == id select prom).First();
            return View(promocao);
        }

        //
        // POST: /Promocao/Delete/5

        [HttpPost]
        public ActionResult Delete(Promocao promocao)
        {
            try
            {
                // obtem o bairro a excluir
                var delPromocao = (from prom in contexto.Promocao where prom.CdPromocao == promocao.CdPromocao select prom).First();

                //excluir
                contexto.DeleteObject(delPromocao);
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
