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
    public class ServicoController : Controller
    {
        private AgendaMeEntities db = new AgendaMeEntities();

        //
        // GET: /Servico/

        public ViewResult Index()
        {
            var servicos = db.Servico.Include("TipoServico").Include("Profissional");

            foreach (Servico s in servicos)
            {
                s.DsProfissional = s.BuscaProfissional();
                s.DsTipoServico = s.BuscaTipoServico();
            }
            return View(servicos.ToList());
        }

        //
        // GET: /Servico/Details/5

        public ViewResult Details(int id)
        {
            Servico servico = db.Servico.Single(s => s.CdServico == id);
            return View(servico);
        }

        //
        // GET: /Servico/Create

        public ActionResult Create()
        {
            var profissionais = from p in db.Profissional
                                join e in db.Pessoa on p.CdPessoa equals e.CdPessoa
                                select new { p.CdProfissional, e.DsNome };

            ViewBag.CdProfissional = new SelectList(profissionais, "CdProfissional", "DsNome");
            ViewData["LstProfissional"] = new SelectList(profissionais, "CdProfissional", "DsNome");
            ViewBag.CdTipoServico = new SelectList(db.TipoServico, "CdTipoServico", "DsNome");
            return View();
        } 

        //
        // POST: /Servico/Create

        [HttpPost]
        public ActionResult Create(Servico servico)
        {
            if (ModelState.IsValid)
            {
                db.Servico.AddObject(servico);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }
            var profissionais = from p in db.Profissional
                                join e in db.Pessoa on p.CdPessoa equals e.CdPessoa
                                select new { p.CdProfissional, e.DsNome };
            ViewBag.CdTipoServico = new SelectList(db.TipoServico, "CdTipoServico", "DsNome", servico.CdTipoServico);
            ViewBag.CdProfissional = new SelectList(profissionais, "CdProfissional", "DsNome", servico.CdProfissional);
            ViewData["LstProfissional"] = new SelectList(profissionais, "CdProfissional", "DsNome");
            return View(servico);
        }
        
        //
        // GET: /Servico/Edit/5
 
        public ActionResult Edit(int id)
        {
            Servico servico = db.Servico.Single(s => s.CdServico == id);
            ViewBag.CdTipoServico = new SelectList(db.TipoServico, "CdTipoServico", "DsNome", servico.CdTipoServico);
            var profissionais = from p in db.Profissional
                                join e in db.Pessoa on p.CdPessoa equals e.CdPessoa
                                select new { p.CdProfissional, e.DsNome };
            ViewData["LstProfissional"] = new SelectList(profissionais, "CdProfissional", "DsNome");
            ViewBag.CdProfissional = new SelectList(profissionais, "CdProfissional", "DsNome", servico.CdProfissional);

            return View(servico);
        }

        //
        // POST: /Servico/Edit/5

        [HttpPost]
        public ActionResult Edit(Servico servico)
        {
            if (ModelState.IsValid)
            {
                db.Servico.Attach(servico);
                db.ObjectStateManager.ChangeObjectState(servico, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CdTipoServico = new SelectList(db.TipoServico, "CdTipoServico", "DsNome", servico.CdTipoServico);
            var profissionais = from p in db.Profissional
                                join e in db.Pessoa on p.CdPessoa equals e.CdPessoa
                                select new { p.CdProfissional, e.DsNome };
            ViewData["LstProfissional"] = new SelectList(profissionais, "CdProfissional", "DsNome");
            ViewBag.CdProfissional = new SelectList(profissionais, "CdProfissional", "DsNome", servico.CdProfissional);
            return View(servico);
        }

        //
        // GET: /Servico/Delete/5
 
        public ActionResult Delete(int id)
        {
            Servico servico = db.Servico.Single(s => s.CdServico == id);
            return View(servico);
        }

        //
        // POST: /Servico/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Servico servico = db.Servico.Single(s => s.CdServico == id);
            db.Servico.DeleteObject(servico);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}