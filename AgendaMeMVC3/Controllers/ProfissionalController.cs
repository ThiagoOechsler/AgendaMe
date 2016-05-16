using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;
using System.Collections;

namespace AgendaMeMVC3.Controllers
{ 
    public class ProfissionalController : Controller
    {
        private AgendaMeEntities db = new AgendaMeEntities();

        //
        // GET: /Profissional/

        public ViewResult Index()
        {
            //var profissional = db.Profissional.Include("Estabelecimento").Include("Pessoa");
            System.Collections.Generic.IEnumerable<Profissional> lst = db.CreateObjectSet<Profissional>().ToList<Profissional>();
            return View(lst);
        }

        //
        // GET: /Profissional/Details/5

        public ViewResult Details(int id)
        {
            Profissional profissional = db.Profissional.Single(p => p.CdProfissional == id);
            return View(profissional);
        }

        //
        // GET: /Profissional/Create

        public ActionResult Create()
        {
            ViewBag.CdEstabelecimento = new SelectList(db.Estabelecimento, "CdEstabelecimento", "DsNome");
            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome");
            return View();
        } 

        //
        // POST: /Profissional/Create

        [HttpPost]
        public ActionResult Create(Profissional profissional)
        {
            if (ModelState.IsValid)
            {
                db.Profissional.AddObject(profissional);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CdEstabelecimento = new SelectList(db.Estabelecimento, "CdEstabelecimento", "DsNome", profissional.CdEstabelecimento);
            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome", profissional.CdPessoa);
            return View(profissional);
        }
        
        //
        // GET: /Profissional/Edit/5
 
        public ActionResult Edit(int id)
        {
            Profissional profissional = db.Profissional.Single(p => p.CdProfissional == id);
            ViewBag.CdEstabelecimento = new SelectList(db.Estabelecimento, "CdEstabelecimento", "DsNome", profissional.CdEstabelecimento);
            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome", profissional.CdPessoa);
            return View(profissional);
        }

        //
        // POST: /Profissional/Edit/5

        [HttpPost]
        public ActionResult Edit(Profissional profissional)
        {
            if (ModelState.IsValid)
            {
                profissional.Pessoa.CdPessoa = Convert.ToInt32(profissional.CdPessoa);
                db.Profissional.Attach(profissional);
                db.ObjectStateManager.ChangeObjectState(profissional, EntityState.Modified);
                db.ObjectStateManager.ChangeObjectState(profissional.Pessoa, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CdEstabelecimento = new SelectList(db.Estabelecimento, "CdEstabelecimento", "DsNome", profissional.CdEstabelecimento);
            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome", profissional.CdPessoa);
            return View(profissional);
        }

        //
        // GET: /Profissional/Delete/5
 
        public ActionResult Delete(int id)
        {
            Profissional profissional = db.Profissional.Single(p => p.CdProfissional == id);
            return View(profissional);
        }

        //
        // POST: /Profissional/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Profissional profissional = db.Profissional.Single(p => p.CdProfissional == id);
            db.Profissional.DeleteObject(profissional);
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