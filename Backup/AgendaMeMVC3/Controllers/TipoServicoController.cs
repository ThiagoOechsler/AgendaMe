using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{ 
    public class TipoServicoController : Controller
    {
        private AgendaMeEntities contexto = new AgendaMeEntities();

        //
        // GET: /TipoServico/

        public ViewResult Index()
        {
            var tiposervico = contexto.TipoServico.Include("Estabelecimento");
            return View(tiposervico.ToList());
        }

        //
        // GET: /TipoServico/Details/5

        public ViewResult Details(int id)
        {
            TipoServico tiposervico = contexto.TipoServico.Single(t => t.CdTipoServico == id);
            return View(tiposervico);
        }

        //
        // GET: /TipoServico/Create

        public ActionResult Create()
        {
            ViewBag.CdEstabelecimento = new SelectList(contexto.Estabelecimento, "CdEstabelecimento", "DsNome");
            return View();
        } 

        //
        // POST: /TipoServico/Create

        [HttpPost]
        public ActionResult Create(TipoServico tiposervico)
        {
            if (ModelState.IsValid)
            {
                contexto.TipoServico.AddObject(tiposervico);
                contexto.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CdEstabelecimento = new SelectList(contexto.Estabelecimento, "CdEstabelecimento", "DsNome", tiposervico.CdEstabelecimento);
            return View(tiposervico);
        }
        
        //
        // GET: /TipoServico/Edit/5
 
        public ActionResult Edit(int id)
        {
            TipoServico tiposervico = contexto.TipoServico.Single(t => t.CdTipoServico == id);
            ViewBag.CdEstabelecimento = new SelectList(contexto.Estabelecimento, "CdEstabelecimento", "DsNome", tiposervico.CdEstabelecimento);
            return View(tiposervico);
        }

        //
        // POST: /TipoServico/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoServico tiposervico)
        {
            if (ModelState.IsValid)
            {
                contexto.TipoServico.Attach(tiposervico);
                contexto.ObjectStateManager.ChangeObjectState(tiposervico, EntityState.Modified);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CdEstabelecimento = new SelectList(contexto.Estabelecimento, "CdEstabelecimento", "DsNome", tiposervico.CdEstabelecimento);
            return View(tiposervico);
        }

        //
        // GET: /TipoServico/Delete/5
 
        public ActionResult Delete(int id)
        {
            TipoServico tiposervico = contexto.TipoServico.Single(t => t.CdTipoServico == id);
            return View(tiposervico);
        }

        //
        // POST: /TipoServico/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            TipoServico tiposervico = contexto.TipoServico.Single(t => t.CdTipoServico == id);
            contexto.TipoServico.DeleteObject(tiposervico);
            contexto.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            contexto.Dispose();
            base.Dispose(disposing);
        }
    }
}