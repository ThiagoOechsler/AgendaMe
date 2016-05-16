using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{
    public class EstabelecimentoController : Controller
    {
        AgendaMeEntities contexto = new AgendaMeEntities();


        public ActionResult Index()
        {
            List<Estabelecimento> lst = contexto.CreateObjectSet<Estabelecimento>().ToList<Estabelecimento>();

            foreach (Estabelecimento b in lst)
            {
                b.DsBairro = b.BuscaNomeBairro();
                b.TrataHoraApresentacao();
            }
            return View(lst);
        }

        // GET: /Estabelecimento/Details/5

        public ActionResult Details(int id)
        {
            var estabelecimento = (from est in contexto.Estabelecimento where est.CdEstabelecimento == id select est).First();
            return View(estabelecimento);
        }

        // GET: /Estabelecimento/Create

        public ActionResult Create()
        {

            ViewData["LstEmpresa"] = new SelectList(contexto.Empresa.ToList(), "CdEmpresa", "DsNome");
            ViewData["LstBairro"] = new SelectList(contexto.Bairro.ToList(), "CdBairro", "DsBairro");
            return View();
        } 

        // POST: /Estabelecimento/Create

        [HttpPost]
        public ActionResult Create(Estabelecimento estabelecimento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(estabelecimento);
                }
                // TODO: Add insert logic here
                contexto.AddToEstabelecimento(estabelecimento);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewData["LstEmpresa"] = new SelectList(contexto.Empresa.ToList(), "CdEmpresa", "DsNome");
                ViewData["LstBairro"] = new SelectList(contexto.Bairro.ToList(), "CdBairro", "DsBairro");
                ViewData["Erro"] = ex.Message;
                return View();
            }
        }
        
        // GET: /Estabelecimento/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewData["LstEmpresa"] = new SelectList(contexto.Empresa.ToList(), "CdEmpresa", "DsNome");
            ViewData["LstBairro"] = new SelectList(contexto.Bairro.ToList(), "CdBairro", "DsBairro");
            var estabelecimento = (from est in contexto.Estabelecimento where est.CdEstabelecimento == id select est).First();
            return View(estabelecimento);
        }

        //
        // POST: /Estabelecimento/Edit/5

        [HttpPost]
        public ActionResult Edit(Estabelecimento estabelecimento)
        {
            try
            {
                // TODO: Add update logic here
                var original = (from est in contexto.Estabelecimento where est.CdEstabelecimento == estabelecimento.CdEstabelecimento select est).First();

                if (!ModelState.IsValid)
                    return View();

                contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, estabelecimento);
                contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        // GET: /Estabelecimento/Delete/5
 
        public ActionResult Delete(int id)
        {
            var estabelecimento = (from est in contexto.Estabelecimento where est.CdEstabelecimento == id select est).First();
            return View(estabelecimento);
        }

        // POST: /Estabelecimento/Delete/5

        [HttpPost]
        public ActionResult Delete(Estabelecimento estabelecimento)
        {
            try
            {
                // obtem o bairro a excluir
                var delEstabelecimento = (from est in contexto.Estabelecimento where est.CdEstabelecimento == estabelecimento.CdEstabelecimento select est).First();

                //excluir
                contexto.DeleteObject(delEstabelecimento);
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
