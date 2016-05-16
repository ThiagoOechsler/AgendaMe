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
    public class ClienteController : Controller
    {
        private AgendaMeEntities db = new AgendaMeEntities();

        //
        // GET: /Cliente/

        public ViewResult Index()
        {
            var cliente = db.Cliente.Include("Pessoa");
            return View(cliente.ToList());
        }

        //
        // GET: /Cliente/Details/5

        public ViewResult Details(int id)
        {
            Cliente cliente = db.Cliente.Single(c => c.CdCliente == id);
            return View(cliente);
        }

        //
        // GET: /Cliente/Create

        public ActionResult Create()
        {
            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome");
            return View();
        } 

        //
        // POST: /Cliente/Create

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.AddObject(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome", cliente.CdPessoa);
            return View(cliente);
        }
        
        //
        // GET: /Cliente/Edit/5
 
        public ActionResult Edit(int id)
        {
            Cliente cliente = db.Cliente.Single(c => c.CdCliente == id);
            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome", cliente.CdPessoa);
            return View(cliente);
        }

        //
        // POST: /Cliente/Edit/5

        [HttpPost]
        public ActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Pessoa.CdPessoa = Convert.ToInt32(cliente.CdPessoa);
                db.Cliente.Attach(cliente);
                db.ObjectStateManager.ChangeObjectState(cliente, EntityState.Modified);
                db.ObjectStateManager.ChangeObjectState(cliente.Pessoa, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CdPessoa = new SelectList(db.Pessoa, "CdPessoa", "DsNome", cliente.CdPessoa);
            return View(cliente);
        }

        //
        // GET: /Cliente/Delete/5
 
        public ActionResult Delete(int id)
        {
            Cliente cliente = db.Cliente.Single(c => c.CdCliente == id);
            return View(cliente);
        }

        //
        // POST: /Cliente/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Cliente cliente = db.Cliente.Single(c => c.CdCliente == id);
            db.Cliente.DeleteObject(cliente);
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