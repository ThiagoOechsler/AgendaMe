using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using AgendaMeMVC3.Models;
using System;
using System.Web.Security;
using System.Web.UI;
using System.Linq.Expressions;

namespace AgendaMeMVC3.Controllers
{
    public class AgendaActionResponseModel
    {
        public String Status;
        public Int64 Source_id;
        public Int64 Target_id;

        public AgendaActionResponseModel(String status, Int64 source_id, Int64 target_id)
        {
            Status = status;
            Source_id = source_id;
            Target_id = target_id;
        }
    }

    public class AgendaController : Controller
    {
        public SelectList ListaClientes { get; set; }
        public string SelectedItem { get; set; }

        AgendaMeEntities contexto = new AgendaMeEntities();
        
        #region Index
        public ActionResult Index()
        {
            List<AgendaLivreDia> agendaLivreOntem = new List<AgendaLivreDia>();
            List<AgendaLivreDia> agendaLivreHoje = new List<AgendaLivreDia>();
            List<AgendaLivreDia> agendaLivreAmanha = new List<AgendaLivreDia>();

            AgendaLivreLista livresOntemHojeAmanha = new AgendaLivreLista(agendaLivreOntem, agendaLivreHoje, agendaLivreAmanha);

            ViewData["ListaServicos"] = new SelectList(contexto.Servico.ToList(), "CdServico", "DsNome");
            //Usando uma nova classe modelo AgendamentoModel, temos a lista de todos os agendamentos do sistema e também uma lista de profissionais.
            // Desta forma podemos ter dois "Model" em uma view
            // -A lista de agendamentos serve para escrever os agendamentos na grid (abaixo do componente Scheduler)
            // -A lista de profissionais é usada na montagem do componente Scheduler

            //Monta uma lista de objetos AgendamentoModel, porém na prática sempre teremos apenas um elemento nesta lista
            //  (tem que ser passado em lista pois a View espera uma lista)
            List<AgendamentoModel> lst = new List<AgendamentoModel>();
            lst.Add(new AgendamentoModel(contexto.CreateObjectSet<Profissional>().ToList<Profissional>(), contexto.CreateObjectSet<Agenda>().ToList<Agenda>(), contexto.CreateObjectSet<Servico>().ToList<Servico>(), livresOntemHojeAmanha));
            
            lst[0].Action = "Index";
            lst[0].ServicoFiltro = "";
            lst[0].DataFiltro = DateTime.MinValue;

            return View(lst);
        }

        public ActionResult IndexAdmin()
        {
            List<AgendaLivreDia> agendaLivreOntem = new List<AgendaLivreDia>();
            List<AgendaLivreDia> agendaLivreHoje = new List<AgendaLivreDia>();
            List<AgendaLivreDia> agendaLivreAmanha = new List<AgendaLivreDia>();

            AgendaLivreLista livresOntemHojeAmanha = new AgendaLivreLista(agendaLivreOntem, agendaLivreHoje, agendaLivreAmanha);

            ViewData["ListaServicos"] = new SelectList(contexto.Servico.ToList(), "CdServico", "DsNome");
            //Usando uma nova classe modelo AgendamentoModel, temos a lista de todos os agendamentos do sistema e também uma lista de profissionais.
            // Desta forma podemos ter dois "Model" em uma view
            // -A lista de agendamentos serve para escrever os agendamentos na grid (abaixo do componente Scheduler)
            // -A lista de profissionais é usada na montagem do componente Scheduler

            //Monta uma lista de objetos AgendamentoModel, porém na prática sempre teremos apenas um elemento nesta lista
            //  (tem que ser passado em lista pois a View espera uma lista)
            List<AgendamentoModel> lst = new List<AgendamentoModel>();
            lst.Add(new AgendamentoModel(contexto.CreateObjectSet<Profissional>().ToList<Profissional>(), contexto.CreateObjectSet<Agenda>().ToList<Agenda>(), contexto.CreateObjectSet<Servico>().ToList<Servico>(), livresOntemHojeAmanha));
            lst[0].Clientes = contexto.CreateObjectSet<Cliente>().ToList<Cliente>();

            lst[0].Action = "IndexAdmin";
            lst[0].ServicoFiltro = "";
            lst[0].DataFiltro = DateTime.Today;

            return View(lst);
        }


        #endregion

        #region Save
        public ActionResult Save(Agenda agendamento, FormCollection actionValues)
        {
            String action_type = actionValues["!nativeeditor_status"];
            Int64 source_id = Int64.Parse(actionValues["id"]);
            Int64 target_id = source_id;

            if (agendamento.start_date > DateTime.Now)
            {
                try
                {
                    if (agendamento.cliente_id <= 0)
                    {
                        String login = Membership.GetUser().UserName.ToString();
                        var cliente = (from cli in contexto.Cliente where cli.Pessoa.DsLogin == login select cli).First();
                        agendamento.CdCliente = cliente.CdCliente;
                    }
                    else
                    {
                        agendamento.CdCliente = agendamento.cliente_id;
                    }                
                    agendamento.DtInicio = agendamento.start_date;
                    agendamento.DtFim = agendamento.end_date;
                    agendamento.CdProfissional = agendamento.section_id;
                    agendamento.CdServico = agendamento.servico_id;

                    switch (action_type)
                    {
                        case "inserted":
                            agendamento.DtCadastro = System.DateTime.Now;
                            contexto.AddToAgenda(agendamento);
                            break;
                        case "deleted":
                            agendamento.CdAgenda = Convert.ToInt16(source_id);
                            contexto.DeleteObject(agendamento);
                            break;
                        default: // "updated"                          
                            var original = (from agd in contexto.Agenda where agd.CdAgenda == source_id select agd).First();
                            agendamento.CdAgenda = Convert.ToInt16(source_id);
                            agendamento.DtCadastro = original.DtCadastro;
                            contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, agendamento);
                            break;
                    }
                    contexto.SaveChanges();
                    target_id = agendamento.CdAgenda;
                }
                catch (Exception a)
                {
                    action_type = "Erro ao gravar alterações do banco";
                    return View();
                }
            }
            else
            {
                action_type = "deleted";

                return View(new AgendaActionResponseModel(action_type, source_id, target_id));
            }
            return View(new AgendaActionResponseModel(action_type, source_id, target_id));
            //List<AgendamentoModel> lst = new List<AgendamentoModel>();
            //lst.Add(new AgendamentoModel(contexto.CreateObjectSet<Profissional>().ToList<Profissional>(), contexto.CreateObjectSet<Agenda>().ToList<Agenda>(), contexto.CreateObjectSet<Servico>().ToList<Servico>()));
            //return View(lst);
        }
        #endregion

        public ActionResult Salvar(int id, int cdProfissional, DateTime dtInicio, DateTime dtFim)
        {
            if ((!(Roles.IsUserInRole("CLIENTE"))) && (!(Roles.IsUserInRole("ADMIN"))))
            {
                LogOnModelVisit logon = new LogOnModelVisit();
                logon.id = id;
                logon.cdProfissional = cdProfissional;
                logon.dtInicio = dtInicio;
                logon.dtFim = dtFim;
                return View("LogOnVisit",logon);
            }
            else
            {
                int cdServico = id;

                try
                {
                    String login = Membership.GetUser().UserName.ToString();
                    var cliente = (from cli in contexto.Cliente where cli.Pessoa.DsLogin == login select cli).First();

                    Agenda agendamento = new Agenda();

                    agendamento.CdCliente = cliente.CdCliente;
                    agendamento.DtInicio = dtInicio;
                    agendamento.DtFim = dtFim;
                    agendamento.CdProfissional = cdProfissional;
                    agendamento.CdServico = cdServico;
                    agendamento.DtCadastro = System.DateTime.Now;

                    contexto.AddToAgenda(agendamento);

                    contexto.SaveChanges();
                }
                catch (Exception a)
                {
                    return View();
                }

                List<AgendaLivreDia> agendaLivreOntem = new List<AgendaLivreDia>();
                List<AgendaLivreDia> agendaLivreHoje = new List<AgendaLivreDia>();
                List<AgendaLivreDia> agendaLivreAmanha = new List<AgendaLivreDia>();

                AgendaLivreLista livresOntemHojeAmanha = new AgendaLivreLista(agendaLivreOntem, agendaLivreHoje, agendaLivreAmanha);

                ViewData["ListaServicos"] = new SelectList(contexto.Servico.ToList(), "CdServico", "DsNome");
                //Monta uma lista de objetos AgendamentoModel, porém na prática sempre teremos apenas um elemento nesta lista
                //  (tem que ser passado em lista pois a View espera uma lista)
                List<AgendamentoModel> lst = new List<AgendamentoModel>();
                lst.Add(new AgendamentoModel(contexto.CreateObjectSet<Profissional>().ToList<Profissional>(),  //Lista de profissionais usado pelo componente scheduler
                                             contexto.CreateObjectSet<Agenda>().ToList<Agenda>(),
                                             contexto.CreateObjectSet<Servico>().ToList<Servico>(), //Todos os serviços usado pelo componente scheduler
                                             livresOntemHojeAmanha));
                lst[0].Action = "Salvar";
                lst[0].ServicoFiltro = "!!Cadastrado!!";
                lst[0].DataFiltro = DateTime.Today;

                //passa a lista de agendamentos para a View
                return View("Index", lst);
            }
        }

        #region Pesquisar
        public ActionResult IndexPesquisa(string servico, string profissional, string cidade, string bairro, string horario)
        {
            try
            {
                #region Trata data

                DateTime data = DateTime.Today;
                try
                {
                    //Se informou um horario
                    if (!string.IsNullOrEmpty(horario))
                    {
                        //Testamos se o horário informado é valido
                        data = Convert.ToDateTime(horario);
                    }
                }
                catch
                {
                    //Caso a data informada é invalida
                    data = DateTime.MinValue;
                }

                #endregion

                #region Monta os horarios livres de acordo com os filtros
                
                //Prepara a lista de agendamentos livres
                List<AgendaLivreDia> agendaLivreOntem = null;
                List<AgendaLivreDia> agendaLivreHoje = null;
                List<AgendaLivreDia> agendaLivreAmanha = null;

                int cdServico = 0;
                var servicoslista = (from ser in contexto.Servico select ser);
                servicoslista = servicoslista.Where(ser => ser.DsNome.ToUpper().StartsWith(servico.ToUpper()));
                if (servicoslista.Count() > 0)
                {
                    cdServico = servicoslista.First().CdServico;
                }
 
                //Caso a data informada seja válida (Maior que hoje)
                if ((data != DateTime.MinValue) && (data >= DateTime.Today) && (!(cdServico <= 0)))
                {
                    int cdProfissional = 0;
                    int cdBairro = 0;
                    int cdCidade = 0;

                    //Busca o registro da cidade filtrado
                    var cidadefiltro = (from cid in contexto.Cidade select cid);
                    if (!string.IsNullOrEmpty(cidade))
                    {
                        cidadefiltro = cidadefiltro.Where(cid => cid.DsNome.ToUpper().StartsWith(cidade.ToUpper()));
                        if (cidadefiltro.Count() > 0)
                        {
                            cdCidade = cidadefiltro.First().CdCidade;
                        }
                    }

                    //Busca o registro do Profissional filtrado
                    var profissionallista = (from pro in contexto.Profissional select pro);
                    if (!string.IsNullOrEmpty(profissional))
                    {
                        profissionallista = profissionallista.Where(pro => pro.Pessoa.DsNome.ToUpper().StartsWith(profissional.ToUpper()));
                        if (profissionallista.Count() > 0)
                        {
                            cdProfissional = profissionallista.First().CdProfissional;
                        }
                    }

                    //Busca o registro do Bairro filtrado
                    var bairrofiltro = (from bai in contexto.Bairro select bai);
                    if (!string.IsNullOrEmpty(bairro))
                    {
                        bairrofiltro = bairrofiltro.Where(bai => bai.DsBairro.ToUpper().StartsWith(bairro.ToUpper()));
                        if (bairrofiltro.Count() > 0)
                        {
                            cdBairro = bairrofiltro.First().CdBairro;
                        }
                    }
                    
                    //Monta os agendamentos livres de acordo com os filtros informados
                    if ((data.Day == DateTime.Today.Day) && (data.Month == DateTime.Today.Month))
                    {
                        //Se a data é de hoje, não montar horários para Ontem
                        agendaLivreOntem = new List<AgendaLivreDia>();
                    }
                    else
                    {
                        agendaLivreOntem = AgendaLivreDia.GetAgendaLivre(cdProfissional, cdBairro, data.AddDays(-1), cdServico);
                    }
                    agendaLivreHoje = AgendaLivreDia.GetAgendaLivre(cdProfissional, cdBairro, data, cdServico);
                    agendaLivreAmanha = AgendaLivreDia.GetAgendaLivre(cdProfissional, cdBairro, data.AddDays(1), cdServico);
                }
                else
                {
                    agendaLivreOntem  = new List<AgendaLivreDia>();
                    agendaLivreHoje   = new List<AgendaLivreDia>();
                    agendaLivreAmanha = new List<AgendaLivreDia>();
                }
                //CARREGA O MODELO COM OS HORARIOS DE ONTEM, HOJE e AMANHA
                AgendaLivreLista livresOntemHojeAmanha = new AgendaLivreLista(agendaLivreOntem, agendaLivreHoje, agendaLivreAmanha);
                #endregion

                ViewData["ListaServicos"] = new SelectList(contexto.Servico.ToList(), "CdServico", "DsNome");
                //Monta uma lista de objetos AgendamentoModel, porém na prática sempre teremos apenas um elemento nesta lista
                //  (tem que ser passado em lista pois a View espera uma lista)
                List<AgendamentoModel> lst = new List<AgendamentoModel>();
                lst.Add(new AgendamentoModel(contexto.CreateObjectSet<Profissional>().ToList<Profissional>(),  //Lista de profissionais usado pelo componente scheduler
                                             contexto.CreateObjectSet<Agenda>().ToList<Agenda>(),
                                             contexto.CreateObjectSet<Servico>().ToList<Servico>(), //Todos os serviços usado pelo componente scheduler
                                             livresOntemHojeAmanha));
                lst[0].Action = "Pesquisa";
                lst[0].ServicoFiltro = servico;
                lst[0].DataFiltro = data;

                //passa a lista de agendamentos para a View
                return View("Index", lst);
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
        #endregion

        public ActionResult Data()
        {
            List<Agenda> lst = contexto.CreateObjectSet<Agenda>().ToList<Agenda>();
            return View(lst);
        }

        public ActionResult DataAdmin()
        {
            List<Agenda> lst = contexto.CreateObjectSet<Agenda>().ToList<Agenda>();
            return View(lst);
        }

        public ActionResult Details(int id)
        {
            var agenda = (from agd in contexto.Agenda where agd.CdAgenda == id select agd).First();
            return View(agenda);
        }

        public ActionResult Create()
        {
            return View();
        }
                
        public ActionResult LogOnVisit(LogOnModelVisit m)
        {
            MembershipUser usuario = Membership.GetUser(m.UserName);

            if (Membership.ValidateUser(usuario.UserName, usuario.GetPassword()))
            {
                FormsAuthentication.SetAuthCookie(usuario.UserName, false);

                return RedirectToAction("Index", "Agenda");    
            }
            else
            {
                ModelState.AddModelError("", "O usuário ou senha informados estão incorretos.");
            }
            

            // If we got this far, something failed, redisplay form
            LogOnModelVisit logon = new LogOnModelVisit();
            return View("LogOnVisit", logon);
        }


        [HttpPost]
        public ActionResult Create(Agenda agenda)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(agenda);
                }
                // TODO: Add insert logic here
                contexto.AddToAgenda(agenda);
                contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
 
        public ActionResult Edit(int id)
        {
            var agenda = (from agd in contexto.Agenda where agd.CdAgenda == id select agd).First();
            return View(agenda);
        }

        [HttpPost]
        public ActionResult Edit(Agenda agenda)
        {
            try
            {
                // TODO: Add update logic here
                var original = (from agd in contexto.Agenda where agd.CdAgenda == agenda.CdAgenda select agd).First();

                if (!ModelState.IsValid)
                    return View();

                contexto.ApplyCurrentValues(original.EntityKey.EntitySetName, agenda);
                contexto.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
 
        public ActionResult Delete(int id)
        {
            var agenda = (from agd in contexto.Agenda where agd.CdAgenda == id select agd).First();
            return View(agenda);
        }

        [HttpPost]
        public ActionResult Delete(Agenda agenda)
        {
            try
            {
                // obtem o bairro a excluir
                var delAgenda = (from agd in contexto.Agenda where agd.CdAgenda == agenda.CdAgenda select agd).First();

                //excluir
                contexto.DeleteObject(delAgenda);
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


////Serviço é obrigatório
//if (!(string.IsNullOrEmpty(servico)))
//{
//    //Se informou serviço nos filtros:
//    if (!string.IsNullOrEmpty(servico))
//    {
//        //Dos agendamentos buscados, pega os registros somente do serviço filtrado
//        query = query.Where(i => i.Servico.DsNome.ToUpper().StartsWith(servico.ToUpper()));
//    }
//    //Se informou profissional nos filtros
//    if (!string.IsNullOrEmpty(profissional))
//    {
//        //Dos agendamentos buscados, pega os registros somente do profissional filtrado
//        query = query.Where(i => i.Profissional.Pessoa.DsNome.ToUpper().StartsWith(profissional.ToUpper()));
//    }
//    //Se informou cidade nos filtros
//    if (!string.IsNullOrEmpty(cidade))
//    {
//        //Dos agendamentos buscados, pega os registros somente da cidade filtrada
//        //Observe que usamos o LINQ: vamos do profissional para estabelecimento para bairro e para cidade
//        query = query.Where(i => i.Profissional.Estabelecimento.Bairro.Cidade.DsNome.ToUpper().StartsWith(cidade.ToUpper()));
//    }

//    //Se informou bairro nos filtros
//    if (!string.IsNullOrEmpty(bairro))
//    {
//        //Dos agendamentos buscados, pega os registros somente do bairro filtrado
//        //Observe que usamos o LINQ: vamos do profissional para estabelecimento e para bairro
//        query = query.Where(i => i.Profissional.Estabelecimento.Bairro.DsBairro.ToUpper().StartsWith(bairro.ToUpper()));
//    }
//}