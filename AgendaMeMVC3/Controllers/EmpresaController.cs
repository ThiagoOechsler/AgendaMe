
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{
    public class EmpresaController : Controller
    {
        AgendaMeEntities contexto = new AgendaMeEntities();
        // GET: /Empresa/ 
        public ActionResult Index()
        {
            //Busca do modelo de dados como uma lista de objetos, todos os registros de empresa 
            List<Empresa> lst = contexto.CreateObjectSet<Empresa>().ToList<Empresa>();
            return View(lst);
        }

        // GET: Empresa/Details
        public ActionResult Details(int id)
        {
            //Busca do modelo de dados a empresa com id oriundo do parâmmetro
            var empresa = (from emp in contexto.Empresa where emp.CdEmpresa == id select emp).First();
            return View(empresa);
        }

        // GET: /Empresa/Create
        public ActionResult Create()
        {
            return View();
        } 

        // POST: /Empresa/Create
        [HttpPost]
        public ActionResult Create(Empresa empresa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(empresa);
                }
                // TODO: Add insert logic here
                contexto.AddToEmpresa(empresa);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        // GET: /Empresa/Edit 
        public ActionResult Edit(int id)
        {
            var empresa = (from emp in contexto.Empresa where emp.CdEmpresa == id select emp).First();
            return View(empresa);
        }

        // POST: /Empresa/Edit
        [HttpPost]
        public ActionResult Edit(Empresa empresa)
        {
            try
            {
                // TODO: Add update logic here
                var original = (from emp in contexto.Empresa where emp.CdEmpresa == empresa.CdEmpresa select emp).First();

                if (!ModelState.IsValid)
                    return View();

                contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, empresa);
                contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: /Empresa/Delete 
        public ActionResult Delete(int id)
        {
            var empresa = (from emp in contexto.Empresa where emp.CdEmpresa == id select emp).First();
            return View(empresa);
        }

        // POST: /Empresa/Delete
        [HttpPost]
        public ActionResult Delete(Empresa empresa)
        {
            try
            {
                // obtem o bairro a excluir
                var delEmpresa = (from emp in contexto.Empresa where emp.CdEmpresa == empresa.CdEmpresa select emp).First();

                //excluir
                contexto.DeleteObject(delEmpresa);
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
