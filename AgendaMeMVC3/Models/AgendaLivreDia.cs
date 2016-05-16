using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using System.Data;

namespace AgendaMeMVC3.Models
{
    public class AgendaLivreDia
    {
        public DateTime DtInicio { get; set; }
        public DateTime DtFim { get; set; }
        public int CdProfissional { get; set; }
        public int CdEstabelecimento { get; set; }
        public int CdServico { get; set; }
        public String NomeProfissional { get; set; }
        public String NomeEstabelecimento { get; set; }
        public String Data { get; set; }
        public String Inicio { get; set; }
        public String Fim { get; set; }
        public String InicioFim { get; set; }

        public static AgendaLivreDia GeraInicioFim(AgendaLivreDia horario)
        {
            horario.InicioFim = horario.Data+" "+horario.Inicio+"|"+horario.Data+" "+horario.Fim+"|"+horario.CdProfissional+"|"+horario.CdServico;
            return horario;
        }

        public static List<AgendaLivreDia> GetAgendaLivre(int prCdProfissional, int prCdBairro, DateTime prDia, int prCdServico)
        {
            AgendaMeEntities contexto = new AgendaMeEntities();
            SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AgendaMe;Integrated Security=SSPI");

            var salao = (from i in contexto.Estabelecimento select i);
            if (prCdBairro > 0) { salao = salao.Where(i => i.CdBairro.Equals(prCdBairro)); }

            var pessoa = (from i in contexto.Profissional select i);
            if (prCdProfissional > 0) { pessoa = pessoa.Where(i => i.CdProfissional.Equals(prCdProfissional)); }

            var servico = (from i in contexto.Servico select i);
            if (prCdServico > 0) { servico = servico.Where(i => i.CdServico.Equals(prCdServico)); }

            Servico serv = servico.First();

            List<AgendaLivreDia> result = new List<AgendaLivreDia>();

            foreach (Estabelecimento s in salao)
            {

                DateTime inimanha = new DateTime(prDia.Year, prDia.Month, prDia.Day, s.HrAtendInicioManha.Hour, s.HrAtendInicioManha.Minute, 0);
                DateTime fimmanha = new DateTime(prDia.Year, prDia.Month, prDia.Day, s.HrAtendFimManha.Hour, s.HrAtendFimManha.Minute, 0);
                DateTime initarde = new DateTime(prDia.Year, prDia.Month, prDia.Day, s.HrAtendInicioTarde.Hour, s.HrAtendInicioTarde.Minute, 0);
                DateTime fimtarde = new DateTime(prDia.Year, prDia.Month, prDia.Day, s.HrAtendFimTarde.Hour, s.HrAtendFimTarde.Minute, 0);

                foreach (Profissional p in pessoa)
                {
                    StringBuilder sql = new StringBuilder("select A.DtInicio, A.DtFim, A.CdProfissional, C.DsNome from Agenda A, Profissional B, Pessoa C  ");
                    sql.Append(" where (A.DtInicio >= @DiaIni and A.DtInicio < @DiaFim) and (B.CdPessoa = C.CdPessoa)  ");
                    sql.Append(" and A.CdProfissional = B.CdProfissional and B.CdEstabelecimento = " + s.CdEstabelecimento + " and B.CdProfissional = " + p.CdProfissional);
                    sql.Append(" order by A.DtInicio ");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql.ToString());
                    cmd.Parameters.Add("@DiaIni", SqlDbType.DateTime);
                    cmd.Parameters["@DiaIni"].Value = new DateTime(prDia.Year, prDia.Month, prDia.Day, 0, 0, 0);
                    cmd.Parameters.Add("@DiaFim", SqlDbType.DateTime);
                    cmd.Parameters["@DiaFim"].Value = new DateTime(prDia.Year, prDia.Month, prDia.Day, 23, 59, 59);
                    cmd.Connection = conn;

                    SqlDataReader rdr = cmd.ExecuteReader();
                    DateTime ini = DateTime.MinValue; //Início do primeiro horário livre
                    DateTime ult = DateTime.MinValue; //Ultimo horario agendado no dia

                    #region Gera Horarios Livres entre os Agendados
                    while (rdr.Read())
                    {
                        DateTime agIni = Convert.ToDateTime(rdr["DtInicio"]); //Inicio da hora marcada
                        DateTime agFim = Convert.ToDateTime(rdr["DtFim"]); //Fim da hora marcada

                        if (!((agIni.Day == inimanha.Day) && (agIni.Month == inimanha.Month) && (agIni.Hour == inimanha.Hour) && (agIni.Minute == inimanha.Minute)))
                        {
                            //Se não está definido um início horário livre, então este é o primeiro horário livre
                            if (ini == DateTime.MinValue)
                            {
                                //Monta o primeiro horário livre
                                if ((prDia.Day == DateTime.Today.Day) && (prDia.Month == DateTime.Today.Month))
                                {
                                    ini = new DateTime(prDia.Year, prDia.Month, prDia.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                                }
                                else
                                {
                                    ini = new DateTime(prDia.Year, prDia.Month, prDia.Day, s.HrAtendInicioManha.Hour, s.HrAtendInicioManha.Minute, 0);
                                }
                            }
                        
                            //Pega a diferença entre o horário livre e o início do agendamento
                            TimeSpan ts = agIni - ini;

                            if (ts.TotalMinutes > 0)
                            {
                                //Condicao para validar se o servico cabe no espaço determinado
                                double totalminutos = Convert.ToInt64((serv.NrDuracaoHoras * 60) + serv.NrDuracaoMinutos);
                                if (ts.TotalMinutes > totalminutos)
                                {
                                    //divide o tempo livre pelo tempo do serviço
                                    double calculo = ts.TotalMinutes / totalminutos;
                                    //obtem a quantidade redonda de serviços que cabe no tempo livre
                                    int qtdCabeManha = Convert.ToInt16(Math.Floor(calculo));
                                    //para cada quantidade livre, gera um espaço de tempo
                                    for (int i = 0; i <= qtdCabeManha - 1; i++)
                                    {
                                        //Cria um registro de horário livre
                                        AgendaLivreDia livre = new AgendaLivreDia();
                                        livre.DtInicio = ini.AddMinutes(totalminutos * i);
                                        livre.DtFim = ini.AddMinutes(totalminutos * (i + 1));
                                        livre.CdProfissional = p.CdProfissional;
                                        livre.CdServico = prCdServico;
                                        livre.NomeProfissional = p.Pessoa.DsNomeCompleto;
                                        livre.NomeEstabelecimento = s.DsNome;
                                        livre.Data = livre.DtInicio.ToString("d");
                                        //Prepara para mostrar horas conforme padrão Alemão
                                        CultureInfo ci = new CultureInfo("de-DE");
                                        livre.Inicio = livre.DtInicio.ToString("t", ci);
                                        livre.Fim = livre.DtFim.ToString("t", ci);
                                        //ADICIONA HORARIO LIVRE
                                        result.Add(GeraInicioFim(livre));
                                    }
                                }
                            }
                            //O início do proximo horário livre é o fim da hora marcada atual
                        }
                        ini = agFim;
                        //Guarda a última hora marcada
                        ult = agFim;

                    }
                    rdr.Close(); //fecha o reader
                    #endregion

                    #region Gera o último Horário Livre ou se nenhum horário agendado gera um de manhã e outro a tarde
                    //Verifica se o ultimo horario foi definido
                    if (!(ult == DateTime.MinValue))
                    {
                        if ((ult.Day == DateTime.Today.Day) && (ult.Month == DateTime.Today.Month))
                        {
                            if (ult <= DateTime.Now)
                            {
                                ult = DateTime.Now.AddMinutes(1);
                            }
                        }
                        //Pega o intervalo entre o ultimo horario marcado e o final do dia
                        DateTime finaldia = new DateTime(prDia.Year, prDia.Month, prDia.Day, s.HrAtendFimTarde.Hour, s.HrAtendFimTarde.Minute, 0);
                        TimeSpan ts = finaldia - ult;
                        if (ts.TotalMinutes > 0)
                        {
                            //Condicao para validar se o servico cabe no ultimo espaço determinado
                            double totalminutos = Convert.ToInt64((serv.NrDuracaoHoras * 60) + serv.NrDuracaoMinutos);
                            if (ts.TotalMinutes > totalminutos)
                            {
                                //divide o tempo do ultimo espaço pelo tempo do serviço
                                double calculo = ts.TotalMinutes / totalminutos;
                                //obtem a quantidade redonda de serviços que cabe no ultimo tempo livre
                                int qtdCabeManha = Convert.ToInt16(Math.Floor(calculo));
                                //para cada quantidade livre, gera um espaço de tempo
                                for (int i = 0; i <= qtdCabeManha - 1; i++)
                                {
                                    //Cria um registro de horário livre
                                    AgendaLivreDia livre = new AgendaLivreDia();
                                    livre.DtInicio = ult.AddMinutes(totalminutos * i);
                                    livre.DtFim = ult.AddMinutes(totalminutos * (i + 1));
                                    livre.CdProfissional = p.CdProfissional;
                                    livre.CdServico = prCdServico;
                                    livre.NomeProfissional = p.Pessoa.DsNomeCompleto;
                                    livre.NomeEstabelecimento = s.DsNome;
                                    livre.Data = livre.DtInicio.ToString("d");
                                    //Prepara para mostrar horas conforme padrão Alemão
                                    CultureInfo ci = new CultureInfo("de-DE");
                                    livre.Inicio = livre.DtInicio.ToString("t", ci);
                                    livre.Fim = livre.DtFim.ToString("t", ci);
                                    //ADICIONA HORARIO LIVRE
                                    result.Add(GeraInicioFim(livre));
                                }
                            }
                        }
                    }
                    else
                    {
                        //Não teve nenhum agendamento
                        // Criar dois horarios: de manha e de tarde
                        double totalminutos = Convert.ToInt64((serv.NrDuracaoHoras * 60) + serv.NrDuracaoMinutos);

                        bool gerarManha = true;
                        TimeSpan ts;
                        double calculo = 0;

                        CultureInfo ci = new CultureInfo("de-DE");

                        if ((prDia.Day == DateTime.Today.Day) && (prDia.Month == DateTime.Today.Month))
                        {
                            if (DateTime.Now > fimmanha)
                            {
                                gerarManha = false;
                            }
                            else
                            {
                                inimanha = new DateTime(prDia.Year, prDia.Month, prDia.Day, DateTime.Now.Hour, (DateTime.Now.Minute + 1), 0);
                            }
                        }

                        if (gerarManha)
                        {
                            //Calcula o total de minutos da parte da manhã
                            ts = fimmanha - inimanha;
                            //Divide o total de minutos da manhã pelos minutos do serviço
                            calculo = ts.TotalMinutes / totalminutos;
                            //Obtem quantos serviços cabe na manha
                            int qtdCabeManha = Convert.ToInt16(Math.Floor(calculo));
                            //lança os horarios livres
                            for (int i = 0; i <= qtdCabeManha - 1; i++)
                            {
                                AgendaLivreDia livreManha = new AgendaLivreDia();
                                livreManha.DtInicio = inimanha.AddMinutes(totalminutos * i);
                                livreManha.DtFim = inimanha.AddMinutes(totalminutos * (i + 1));

                                livreManha.CdProfissional = p.CdProfissional;
                                livreManha.CdServico = prCdServico;
                                livreManha.NomeProfissional = p.Pessoa.DsNomeCompleto;
                                livreManha.NomeEstabelecimento = s.DsNome;
                                livreManha.Data = livreManha.DtInicio.ToString("d");
                                //Prepara para mostrar horas conforme padrão Alemão
                                livreManha.Inicio = livreManha.DtInicio.ToString("t", ci);
                                livreManha.Fim = livreManha.DtFim.ToString("t", ci);
                                //ADICIONA HORARIO LIVRE
                                result.Add(GeraInicioFim(livreManha));
                            }
                        }

                        bool gerarTarde = true;

                        if ((prDia.Day == DateTime.Today.Day) && (prDia.Month == DateTime.Today.Month))
                        {
                            if (DateTime.Now > fimtarde)
                            {
                                gerarTarde = false;
                            }
                            else
                            {
                                initarde = new DateTime(prDia.Year, prDia.Month, prDia.Day, DateTime.Now.Hour, (DateTime.Now.Minute + 1), 0);
                            }
                        }

                        if (gerarTarde)
                        {
                            //Calculo o total de minutos da parte da tarde
                            ts = fimtarde - initarde;
                            //Divide o total de minutos da tarde pelos minutos do serviço
                            calculo = ts.TotalMinutes / totalminutos;
                            //Obtem quantos serviços cabe na parte da tarde
                            int qtdCabeTarde = Convert.ToInt16(Math.Floor(calculo));
                            //lança os serviços
                            for (int i = 0; i <= qtdCabeTarde - 1; i++)
                            {
                                AgendaLivreDia livreTarde = new AgendaLivreDia();
                                livreTarde.DtInicio = initarde.AddMinutes(totalminutos * i);
                                livreTarde.DtFim = initarde.AddMinutes(totalminutos * (i + 1));

                                livreTarde.CdProfissional = p.CdProfissional;
                                livreTarde.CdServico = prCdServico;
                                livreTarde.NomeProfissional = p.Pessoa.DsNomeCompleto;
                                livreTarde.NomeEstabelecimento = s.DsNome;
                                livreTarde.Data = livreTarde.DtInicio.ToString("d");
                                //Prepara para mostrar horas conforme padrão Alemão
                                livreTarde.Inicio = livreTarde.DtInicio.ToString("t", ci);
                                livreTarde.Fim = livreTarde.DtFim.ToString("t", ci);
                                //ADICIONA HORARIO LIVRE
                                result.Add(GeraInicioFim(livreTarde));
                            }
                        }
                    }
                    #endregion

                    conn.Close();
                }
            }

            return result;
        }
    }


}