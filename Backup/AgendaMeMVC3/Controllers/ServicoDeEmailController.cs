using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net;
using ActionMailer.Net.Mvc;
using AgendaMeMVC3.Models;

namespace AgendaMeMVC3.Controllers
{
    //MailerBase classe da Bilioteca ActionMailer que contém toda a lógica de envio de e-mail
    public class ServicoDeEmailController : MailerBase
    {
        public EmailResult Lembrete(AgendaMeMVC3.Models.Cliente model)
        {
            //Remetente- salão de beleza
            From = "Cabeloseciatcc@gmail.com";
            //Busca e adiciona o e-mail do cliente passado no parâmetro model
            To.Add(model.Pessoa.DsEmail);
            //Assunto do e-mail
            Subject = "Lembrete de Horário Agendado";
            return Email("Lembrete", model);
        }
    }
}
