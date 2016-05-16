using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;

namespace AgendaMeMVC3.Models
{
    [Serializable()]
    [DataContractAttribute(IsReference = true)]
    public class AgendamentoModel : EntityObject
    {
        protected List<Profissional> Fprofissionais;
        protected List<Agenda> Fagendamentos;
        protected List<Servico> Fservicos;
        protected AgendaLivreLista FAgendaLivreOntemHojeAmanha;
        public List<Cliente> Clientes { get; set; }
        public DateTime DataFiltro { get; set; }
        public String ServicoFiltro { get; set; }
        public String Action { get; set; }

        public List<Profissional> Profissionais
        {
            get
            {
                return this.Fprofissionais;
            }
        }

        public AgendaLivreLista AgendaLivreLista
        {
            get
            {
                return this.FAgendaLivreOntemHojeAmanha;
            }
        }

        public List<Agenda> Agendamentos
        {
            get
            {
                return this.Fagendamentos;
            }
        }

        public List<Servico> Servicos
        {
            get
            {
                return this.Fservicos;
            }
        }

        public AgendamentoModel(List<Profissional> prProfissionais, List<Agenda> prAgendas, List<Servico> prServicos, AgendaLivreLista prAgendaLivreOntemHojeAmanha)
        {
            this.Fprofissionais = prProfissionais;
            this.Fagendamentos = prAgendas;
            this.Fservicos = prServicos;
            this.FAgendaLivreOntemHojeAmanha = prAgendaLivreOntemHojeAmanha;
        }
    }
}