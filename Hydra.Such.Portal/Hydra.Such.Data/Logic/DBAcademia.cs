using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Academia;
using Hydra.Such.Data.Logic.Approvals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;
//using System.Diagnostics;

namespace Hydra.Such.Data.Logic
{
    public class Formando : NAVEmployeeViewModel//, IEquatable<Formando>
    {
        public string RegiaoNav2017 { get; set; }
        public string DescRegiaoNav2017 { get; set; }
        public string AreaNav2017 { get; set; } 
        public string DescAreaNav2017 { get; set; }
        public string CrespNav2017 { get; set; }
        public string DescCrespNav2017 { get; set; }
        public string Projecto { get; set; }
        public string Funcao { get; set; }
        public string CodEstabelecimento { get; set; }
        public string DescricaoEstabelecimento { get; set; }

        public List<ConfiguracaoAprovacaoAcademia> HierarquiaFormando { get; set; }
        
        public ICollection<PedidoParticipacaoFormacao> PedidosFormacao { get; set; }    


        public Formando()
        {

        }

        public Formando(string employeeNo)
        {
            Formando f = DBAcademia.__GetDetailsFormando(employeeNo);
            No = f.No;
            Name = f.Name;
            Regiao = f.Regiao;
            Area = f.Area;
            Cresp = f.Cresp;
            RegiaoNav2017 = f.RegiaoNav2017;
            DescRegiaoNav2017 = f.DescRegiaoNav2017;
            AreaNav2017 = f.AreaNav2017;
            DescAreaNav2017 = f.DescAreaNav2017;
            CrespNav2017 = f.CrespNav2017;
            DescCrespNav2017 = f.DescCrespNav2017;
            Projecto = f.Projecto;
            Funcao = f.Funcao;
            CodEstabelecimento = f.CodEstabelecimento;
            DescricaoEstabelecimento = f.DescricaoEstabelecimento;
        }

        public bool AlreadyRegisteredForCourse(string courseId, ref string requestId)
        {
            requestId = string.Empty;

            if (!string.IsNullOrEmpty(courseId))
            {
                try
                {
                    using (var _ctx = new SuchDBContext())
                    {
                        requestId = _ctx.PedidoParticipacaoFormacao.LastOrDefault(
                                p => (p.IdEmpregado == No) && 
                                (p.IdAccaoFormacao == courseId) && 
                                (p.Estado >= (int)Enumerations.EstadoPedidoFormacao.PedidoSubmetido &&
                                 p.Estado <= (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado)).IdPedido;

                        return !string.IsNullOrEmpty(requestId);

                    }
                }
                catch (Exception ex)
                {

                    return false;
                }
                
            }

            return false;
        }

        public void GetAllTrainingRequestsForTrainne()
        {
            PedidosFormacao = DBAcademia.__GetAllPedidosFormacao(No);
        }

        public void GetTraineeManagers(int type)
        {
            HierarquiaFormando = new List<ConfiguracaoAprovacaoAcademia>();
          

            // obter todas as configurações de aprovações para Pedidos de Participação em Formação referentes à Área do Formando
            // o "Nível de Aprovação" é que distinguirá quem é Director e quem é Chefia
            List<ConfiguraçãoAprovações> configAprovadores = DBApprovalConfigurations.GetAllByType(type)
                .Where(a => a.CódigoÁreaFuncional == AreaNav2017 && a.CódigoCentroResponsabilidade == CrespNav2017).ToList();

            List<string> utilizadoresAprovacao = new List<string>();

            foreach(var c in configAprovadores)
            {
                if (!string.IsNullOrEmpty(c.UtilizadorAprovação) && c.NívelAprovação.Value == 3)
                {
                    ConfigUtilizadores userConfig = DBUserConfigurations.GetById(c.UtilizadorAprovação);
                    ConfiguracaoAprovacaoAcademia u = new ConfiguracaoAprovacaoAcademia(userConfig.IdUtilizador, userConfig.Nome, c);

                    if (u != null && !string.IsNullOrEmpty(u.IdUtilizador))
                    {
                        HierarquiaFormando.Add(u);
                    }
                }
                else
                {
                    if (c.GrupoAprovação.HasValue)
                    {
                        utilizadoresAprovacao = DBApprovalUserGroup.GetAllFromGroup(c.GrupoAprovação.Value);
                        foreach (var ua in utilizadoresAprovacao)
                        {
                            ConfigUtilizadores userConfig = DBUserConfigurations.GetById(ua);
                            ConfiguracaoAprovacaoAcademia u = new ConfiguracaoAprovacaoAcademia(userConfig.IdUtilizador, userConfig.Nome, c);
                            if (u != null && !string.IsNullOrEmpty(u.IdUtilizador) && !CheckIfManagerExists(u))
                            {
                                HierarquiaFormando.Add(u);
                            }
                        }
                    }                    
                }
            }
        }

        public List<string> GetManagersEmailAddresses(int approvalLevel)
        {
            List<string> addresses = new List<string>();

            foreach (var c in HierarquiaFormando)
            {
                if (c.NívelAprovação == approvalLevel)
                {
                    addresses.Add(c.IdUtilizador);
                }
            }
            return addresses;
        }

        private bool CheckIfManagerExists(ConfiguracaoAprovacaoAcademia config)
        {
            foreach (var c in HierarquiaFormando)
            {
                if (c.IdUtilizador == config.IdUtilizador && c.NívelAprovação == config.NívelAprovação)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class ConfiguracaoAprovacaoAcademia : ConfiguraçãoAprovações
    {
        public string IdUtilizador { get; set; }
        public string NomeAprovador { get; set; }

        public Enumerations.TipoUtilizadorFluxoPedidoFormacao TipoUtilizadorConfiguracao { get; set; }

        public ConfiguracaoAprovacaoAcademia(string userId, string userName, ConfiguraçãoAprovações config)
        {
            IdUtilizador = userId;
            NomeAprovador = userName;
            if (config.NívelAprovação == 1)
                TipoUtilizadorConfiguracao = Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorChefia;

            if (config.NívelAprovação == 2)
                TipoUtilizadorConfiguracao = Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorDireccao;

            if (config.NívelAprovação == 3)
                TipoUtilizadorConfiguracao = Enumerations.TipoUtilizadorFluxoPedidoFormacao.ConselhoAdministracao;

            if (config.NívelAprovação != 1 && config.NívelAprovação != 2)
                TipoUtilizadorConfiguracao = Enumerations.TipoUtilizadorFluxoPedidoFormacao.Formando;

            Id = config.Id;
            Tipo = config.Tipo;
            Área = config.Área;
            CódigoÁreaFuncional = config.CódigoÁreaFuncional;
            CódigoRegião = config.CódigoRegião;
            CódigoCentroResponsabilidade = config.CódigoCentroResponsabilidade;
            NívelAprovação = config.NívelAprovação;
            UtilizadorAprovação = config.UtilizadorAprovação;
            GrupoAprovação = config.GrupoAprovação;
        }

        
    }

    public class ConfiguracaoAprovacaoUtilizador
    {
        public string IdUtilizador { get; set; }
        public string EmployeeNo { get; set; }
        public Enumerations.TipoUtilizadorFluxoPedidoFormacao TipoUtilizadorGlobal { get; set; }
        public List<string> AreasChefia { get; set; }
        public List<string> CRespChefia { get; set; }
        public List<string> AreasDirige { get; set; }
        public List<string> PelourosConselho { get; set; }
        public List<ConfiguracaoAprovacaoAcademia> ConfiguracaoAprovAcademia { get; set; }

        public ConfiguracaoAprovacaoUtilizador(ConfigUtilizadores userConfig, int type)
        {
            IdUtilizador = userConfig.IdUtilizador;
            EmployeeNo = userConfig.EmployeeNo;

            try
            {
                TipoUtilizadorGlobal = (Enumerations.TipoUtilizadorFluxoPedidoFormacao)userConfig.TipoUtilizadorFormacao.Value;
            }
            catch (Exception)
            {

                TipoUtilizadorGlobal = Enumerations.TipoUtilizadorFluxoPedidoFormacao.Formando;
            }            

            ConfiguracaoAprovAcademia = new List<ConfiguracaoAprovacaoAcademia>();

            AreasChefia = new List<string>();
            CRespChefia = new List<string>();
            AreasDirige = new List<string>();
            PelourosConselho = new List<string>();

            List<UtilizadoresGruposAprovação> grupos = DBApprovalUserGroup.GetByUser(userConfig.IdUtilizador);
            List<ConfiguraçãoAprovações> configAprovadores = DBApprovalConfigurations.GetAllByType(type);

           if(configAprovadores != null && configAprovadores.Count > 0)
            {
                foreach(var c in configAprovadores)
                {
                    if(grupos.FirstOrDefault(g => g.GrupoAprovação == c.GrupoAprovação) != null)
                    {
                        ConfiguracaoAprovAcademia.Add(new ConfiguracaoAprovacaoAcademia(userConfig.IdUtilizador, userConfig.Nome, c));

                        switch (c.NívelAprovação)
                        {
                            case 1: // Chefia
                                {
                                    if (!string.IsNullOrWhiteSpace(c.CódigoÁreaFuncional))
                                        AreasChefia.Add(c.CódigoÁreaFuncional);

                                    if (!string.IsNullOrWhiteSpace(c.CódigoCentroResponsabilidade))
                                        CRespChefia.Add(c.CódigoCentroResponsabilidade);
                                }
                                break;
                            case 2: // Director
                                {
                                    if (!string.IsNullOrWhiteSpace(c.CódigoÁreaFuncional))
                                        AreasDirige.Add(c.CódigoÁreaFuncional);
                                }
                                break;
                            case 3: // CA
                                {
                                    if (c.UtilizadorAprovação == userConfig.IdUtilizador && 
                                        (Enumerations.TipoUtilizadorFluxoPedidoFormacao)userConfig.TipoUtilizadorFormacao.Value == Enumerations.TipoUtilizadorFluxoPedidoFormacao.ConselhoAdministracao)
                                    {
                                        if (!string.IsNullOrWhiteSpace(c.CódigoÁreaFuncional))
                                        {
                                            PelourosConselho.Add(c.CódigoÁreaFuncional);
                                        }
                                    }
                                    
                                }
                                break;
                        }                                              
                        
                    }
                }

                AreasChefia = AreasChefia != null && AreasChefia.Count > 0 ? AreasChefia.Distinct().ToList() : null;
                CRespChefia = CRespChefia != null && CRespChefia.Count > 0 ? CRespChefia.Distinct().ToList() : null;
                AreasDirige = AreasDirige != null && AreasDirige.Count > 0 ? AreasDirige.Distinct().ToList() : null;
                PelourosConselho = PelourosConselho != null && PelourosConselho.Count > 0 ? PelourosConselho.Distinct().ToList() : null;
            }
        }

        public bool IsChief()
        {
            // só é Chefia se tiver Área(s) e CResp(s) preenchido
            return ConfiguracaoAprovAcademia.FirstOrDefault(c => c.TipoUtilizadorConfiguracao == Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorChefia) == null ||
                    (AreasChefia == null && CRespChefia == null) ? false : true;
        }

        public bool IsDirector()
        {
            return ConfiguracaoAprovAcademia.FirstOrDefault(c => c.TipoUtilizadorConfiguracao == Enumerations.TipoUtilizadorFluxoPedidoFormacao.AprovadorDireccao) == null ||
                    AreasDirige == null ? false : true;
        }

        public bool IsBoardMember()
        {
            return TipoUtilizadorGlobal == Enumerations.TipoUtilizadorFluxoPedidoFormacao.ConselhoAdministracao;
        }

    }

    #region Envio de emails
    public class EmailAcademia
    {
        public string IdPedido { get; set; }
        public string BodyText { get; set; }
        public string SubjectText { get; set; }
        public string SenderAddress { get; set; }
        public string SenderName { get; set; }
        public List<string> ToAddresses { get; set; }
        public List<string> CcAddresses { get; set; }
        public List<string> BccAddresses { get; set; }
        public bool IsHtml { get; set; }

    }
    public class SendEmailsAcademia : EmailAutomation
    {
        public EmailAcademia NotificationEmail { get; set; }
        public int ReturnCode { get; set; }

        private bool MailSent = false;


        public SendEmailsAcademia(EmailAcademia email)
            : base(email.SenderAddress, email.SenderName, email.SubjectText, email.BodyText, email.IsHtml)
        {
            NotificationEmail = email;

            To = email.ToAddresses;
            CC = email.CcAddresses;
            BCC = email.BccAddresses;

        }

        public void MakeBodyContent()
        {
            if (!IsBodyHtml)
            {
                Body = NotificationEmail.BodyText;

            }
            else
            {
                Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:600px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Exmos (as) Senhores (as)," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                NotificationEmail.BodyText +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                NotificationEmail.SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";
            }
        }

        public void MakeEmailToChiefForApproval(Formando f, PedidoParticipacaoFormacao p, string url)
        {
            if (!IsBodyHtml)
            {
                Body = NotificationEmail.BodyText;

            }
            else
            {
                if (f.HierarquiaFormando == null || f.HierarquiaFormando.Count == 0)
                {
                    return;
                }

                List<string> nomesChefias = new List<string>();
                string chefias;
                string bodyTxt;

                foreach (var item in f.HierarquiaFormando)
                {
                    if (item.NívelAprovação == 1)
                    {
                        nomesChefias.Add(item.NomeAprovador);
                    }
                }

                if (nomesChefias == null || nomesChefias.Count == 0)
                {
                    return;
                }

                if (nomesChefias.Count == 1)
                {
                    chefias = "Caro(a) " + string.Join(", ", nomesChefias) + ",";
                }
                else
                {
                    chefias = "Caros(as)<br />" + string.Join(",<br />", nomesChefias) + ",";
                }

                bodyTxt = "O Colaborador " + f.Name + " (" + f.No + "), " +
                    "pertencente ao Centro de Responsabilidade" + f.CrespNav2017 + " - " + f.DescCrespNav2017 + ", realizou um pedido de participação em formação externa " +
                    "para a seguinte acção de formação:<br />" +
                    "Acção: " + p.DesignacaoAccao + "<br />" +
                    "Data Inicio: " + p.DataInicio.Value.Date.ToString("dd-MM-yyyy") + "<br /><br />" +
                    "Para tratar o pedido, por favor, aceda a: <a href=\"" + url + "\">" + p.IdPedido + "</a><br />";

                Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:800px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                chefias +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                bodyTxt +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                NotificationEmail.SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";
            }
        }

        public void MakeEmailToDirectorForApproval(Formando f, PedidoParticipacaoFormacao p, string url)
        {
            if (!IsBodyHtml)
            {
                Body = NotificationEmail.BodyText;

            }
            else
            {
                if (f.HierarquiaFormando == null || f.HierarquiaFormando.Count == 0)
                {
                    return;
                }

                List<string> nomesDirectores = new List<string>();
                string directores;
                string bodyTxt;

                foreach (var item in f.HierarquiaFormando)
                {
                    if (item.NívelAprovação == 2)
                    {
                        nomesDirectores.Add(item.NomeAprovador);
                    }
                }

                if (nomesDirectores == null || nomesDirectores.Count == 0)
                {
                    return;
                }

                if (nomesDirectores.Count == 1)
                {
                    directores = "Caro(a) " + string.Join(", ", nomesDirectores) + ",";
                }
                else
                {
                    directores = "Caros(as)<br />" + string.Join(",<br />", nomesDirectores) + ",";
                }

                bodyTxt = "O Colaborador " + f.Name + " (" + f.No + "), " +
                    "pertencente ao " + f.CrespNav2017 + " - " + f.DescCrespNav2017 + ", realizou um pedido de participação em formação externa " +
                    "para a seguinte acção de formação:<br />" +
                    "Acção: " + p.DesignacaoAccao + "<br />" +
                    "Data Inicio: " + p.DataInicio.Value.Date.ToString("dd-MM-yyyy") + "<br /><br />" +
                    "O pedido foi aprovado pela chefia directa.<br /><br />" +
                    "Para tratar o pedido, por favor, aceda a: <a href=\"" + url + "\">" + p.IdPedido + "</a><br />";

                Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:800px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                directores +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                bodyTxt +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                NotificationEmail.SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";
            }
        }

        public void MakeEmailToDirectorRequestDenial(Formando f, PedidoParticipacaoFormacao p, string url)
        {
            if (!IsBodyHtml)
            {
                Body = NotificationEmail.BodyText;
            }
            else
            {
                if (f.HierarquiaFormando == null || f.HierarquiaFormando.Count == 0)
                {
                    return;
                }

                List<string> nomesDirectores = new List<string>();
                string directores;
                string bodyTxt;

                foreach (var item in f.HierarquiaFormando)
                {
                    if (item.NívelAprovação == 2)
                    {
                        nomesDirectores.Add(item.NomeAprovador);
                    }
                }

                if (nomesDirectores == null || nomesDirectores.Count == 0)
                {
                    return;
                }

                if (nomesDirectores.Count == 1)
                {
                    directores = "Caro(a) " + string.Join(", ", nomesDirectores) + ",";
                }
                else
                {
                    directores = "Caros(as)<br />" + string.Join(",<br />", nomesDirectores) + ",";
                }

                bodyTxt = "O pedido de participação em formação externa nº" + p.IdPedido + "," +
                    "do colaborador " + f.Name + "(" + f.No + "), pertencente ao Centro de Responsabilidade " + f.CrespNav2017 + " - " + f.DescCrespNav2017 +
                    ", para a acção de formação:<br />" +
                    "Acção: " + p.DesignacaoAccao + "<br />" +
                    "Data Inicio: " + p.DataInicio.Value.Date.ToString("dd-MM-yyyy") + "<br />" +
                    "Foi rejeitado com a seguinte informação:<br />" + p.ParecerDotacaoAcademia + "<br />" +
                    "Por favor corrija e reenvie o pedido à Academia: <a href=\"" + url + "\">" + p.IdPedido + "</a><br />";

                Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:800px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                directores +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                bodyTxt +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                NotificationEmail.SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";
            }
        }
        public void MakeEmailToBoardForApproval(Formando f, PedidoParticipacaoFormacao p, string url)
        {
            if (!IsBodyHtml)
            {
                Body = NotificationEmail.BodyText;
            }
            else
            {
                if (f.HierarquiaFormando == null || f.HierarquiaFormando.Count == 0)
                {
                    return;
                }

                List<string> nomesCA = new List<string>();
                string nomes;
                string bodyTxt;

                foreach (var item in f.HierarquiaFormando)
                {
                    if (item.NívelAprovação.Value == 3)
                    {
                        nomesCA.Add(item.NomeAprovador);
                    }
                }
                if (nomesCA == null || nomesCA.Count == 0)
                {
                    return;
                }

                if (nomesCA.Count == 1)
                {
                    nomes = "Exmo(a) Membro do Conselho de Administração" + string.Join(", ", nomesCA) + ",";
                }
                else
                {
                    nomes = "Exmos(as) Membros do Conselho de Administração<br />" + string.Join(",<br />", nomesCA) + ",";
                }

                bodyTxt = "Remete-se para aprovação de V.Ex.ª o pedido de participação em formação externa, do colaborador(a) " + f.Name + "(" + f.No + "), " +
                    "pertencente ao Centro de Responsabilidade" + f.CrespNav2017 + " - " + f.DescCrespNav2017 + ".<br /><br />" +
                    "Para tratar, por favor siga a ligação: <a href=\"" + url + "\">" + p.IdPedido + "</a><br />";

                Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:800px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                nomes +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                bodyTxt +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                NotificationEmail.SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            }
        }

        public void MakeEmailToAcademy(PedidoParticipacaoFormacao p, string url)
        {
            if (!IsBodyHtml)
            {
                Body = NotificationEmail.BodyText;
            }
            else
            {
                string bodyTxt = "<p>" + NotificationEmail.BodyText + "<a href=\"" + url + "\">" + p.IdPedido + "</a></p>";
            }
        }
        

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            MailSent = false;
            string Token = (string)e.UserState;
            if (!e.Cancelled && e.Error == null)
            {
                MailSent = true;
                ReturnCode = 0;
            }

            if (e.Error != null)
            {
                ReturnCode = -1;
            }
        }

        public void SendEmail()
        {
            if (string.IsNullOrEmpty(From) || !IsValidEmail(From))
            {
                ReturnCode = -10;
                return;
            }

            foreach (var t in To)
            {
                if (!IsValidEmail(t))
                {
                    To.Remove(t);
                }
            }

            if (To == null || To.Count <= 0)
            {
                ReturnCode = -20;
                return;
            }

            SmtpClient Client = new SmtpClient(Config.Host, Config.Port);
            NetworkCredential Credentials = new NetworkCredential(Config.Username, Config.Password);

            Client.UseDefaultCredentials = true;
            Client.Credentials = Credentials;
            Client.EnableSsl = Config.SSL;

            MailMessage MMessage = new MailMessage
            {
                From = new MailAddress(From, DisplayName)
            };

            foreach (var t in To)
            {
                MMessage.To.Add(new MailAddress(t));
            }

            foreach (var cc in CC)
            {
                if (IsValidEmail(cc))
                    MMessage.CC.Add(cc);
            }

            foreach (var bcc in BCC)
            {
                if (IsValidEmail(bcc))
                    MMessage.Bcc.Add(bcc);
            }

            MMessage.Subject = Subject;
            MMessage.Body = Body;
            MMessage.IsBodyHtml = IsBodyHtml;

            Client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            string UserState = "EmailAcademia";
            Client.SendAsync(MMessage, UserState);
        }
    }
    #endregion
    /// <summary>
    /// Obter os pedidos que a Academia deverá gerir
    /// 1. Pedidos a Tratar: Estado 3-AprovadoDireccao
    /// 2. Pedidos em Curso: Estado 5-Analisado
    /// 3. Pedidos Autorizados pelo CA: Estado 6-AutorizadoCA
    /// 4. Pedidos Rejeitados pelo CA: Estado 7-RejeitadoCA
    /// 5. Pedidos Finalizados(Histórico): Estado 8-Finalizado
    /// </summary>
    public class GestaoPedidos
    {
        public List<PedidoParticipacaoFormacaoView> Tratar { get; set; } 
        public List<PedidoParticipacaoFormacaoView> EmCurso { get; set; }
        public List<PedidoParticipacaoFormacaoView> Autorizados { get; set; }
        public List<PedidoParticipacaoFormacaoView> Rejeitados { get; set; }
        public List<PedidoParticipacaoFormacaoView> Encerrados { get; set; } 

        public GestaoPedidos()
        {
            Tratar = new List<PedidoParticipacaoFormacaoView>();
            EmCurso = new List<PedidoParticipacaoFormacaoView>();
            Autorizados = new List<PedidoParticipacaoFormacaoView>();
            Rejeitados = new List<PedidoParticipacaoFormacaoView>();
            Encerrados = new List<PedidoParticipacaoFormacaoView>();

            Tratar = ToView(DBAcademia.__GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao, 3));
            EmCurso = ToView(DBAcademia.__GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao, 5));
            Autorizados = ToView(DBAcademia.__GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao, 6));
            Rejeitados = ToView(DBAcademia.__GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao, 7));
            Encerrados = ToView(DBAcademia.__GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao, 8));

        }

        private List<PedidoParticipacaoFormacaoView> ToView(List<PedidoParticipacaoFormacao> pedidos)
        {
            try
            {
                List<PedidoParticipacaoFormacaoView> returnList = new List<PedidoParticipacaoFormacaoView>();
                foreach (var p in pedidos)
                {
                    returnList.Add(new PedidoParticipacaoFormacaoView(p));
                }
                return returnList;
            }
            catch (Exception ex)
            {

                return new List<PedidoParticipacaoFormacaoView>();
            }
        }
        
    }

    /// <summary>
    /// 14-07-2020
    /// Classe responsável pelos CRUD relacionados com Pedidos de Participação Em Formação (PPF)
    /// 
    /// /!\/!\/!\/!\
    ///     Os métodos deverão ser chamados com os campos que permitem avaliar os dados de criação/modificação, dos objectos, já preenchidos.
    /// /!\/!\/!\/!\
    /// </summary>
    public static class DBAcademia
    {
        public const int NoMesesMostrarAccoesPorDefeito = 2;

        #region Creates
        public static PedidoParticipacaoFormacao __CriarPedidoFormacao(AccaoFormacao accao, Formando formando, ConfigUtilizadores user)
        {
            if (accao == null)
            {
                return null;
            }

            Configuração config = DBConfigurations.GetById(1);
            int numeracaoPedidos = config.NumeracaoPedidoFormacao == null ? 0 : config.NumeracaoPedidoFormacao.Value;

            if (numeracaoPedidos > 0)
            {
                try
                {
                    //Formando formando = __GetDetailsFormando(user.EmployeeNo);
                    //Formando formando = new Formando(user.EmployeeNo);

                    PedidoParticipacaoFormacao pedido = new PedidoParticipacaoFormacao()
                    {
                        IdPedido = DBNumerationConfigurations.GetNextNumeration(numeracaoPedidos, true, false),
                        Estado = (int)Enumerations.EstadoPedidoFormacao.PedidoCriado,
                        IdEmpregado = formando.No,
                        NomeEmpregado = formando.Name,
                        FuncaoEmpregado = formando.Funcao,
                        ProjectoEmpregado = formando.Projecto,
                        IdAreaFuncional = formando.AreaNav2017,
                        AreaFuncionalEmpregado = formando.DescAreaNav2017,
                        IdCentroResponsabilidade = formando.CrespNav2017,
                        CentroResponsabilidadeEmpregado = formando.DescCrespNav2017,
                        IdEstabelecimento = formando.CodEstabelecimento,
                        DescricaoEstabelecimento = formando.DescricaoEstabelecimento,
                        IdAccaoFormacao = accao.IdAccao,
                        DesignacaoAccao = accao.DesignacaoAccao,
                        DataInicio = accao.DataInicio,
                        DataFim = accao.DataFim,
                        NumeroTotalHoras = accao.NumeroTotalHoras,
                        LocalRealizacao = accao.LocalRealizacao,
                        UtilizadorCriacao = user.IdUtilizador,
                        DataHoraCriacao = DateTime.Now                        
                    };

                    if (accao.Entidade != null)
                    {
                        pedido.IdEntidadeFormadora = accao.Entidade.IdEntidade;
                        pedido.DescricaoEntidadeFormadora = accao.Entidade.DescricaoEntidade;
                    }

                    using (var _ctx = new SuchDBContext())
                    {
                        _ctx.PedidoParticipacaoFormacao.Add(pedido);
                        _ctx.SaveChanges();

                        return pedido;
                    }
                }
                catch (Exception ex)
                {

                    return null;
                }                
            }

            return null;
        }
        public static PedidoParticipacaoFormacao __CriarPedidoFormacao(ConfigUtilizadores user)
        {
            try
            {
                Configuração config = DBConfigurations.GetById(1);
                int numeracaoPedidos = config.NumeracaoPedidoFormacao == null ? 0 : config.NumeracaoPedidoFormacao.Value;

                if (numeracaoPedidos > 0)
                {
                    Formando formando = __GetDetailsFormando(user.EmployeeNo);

                    PedidoParticipacaoFormacao pedido = new PedidoParticipacaoFormacao()
                    {
                        IdPedido = DBNumerationConfigurations.GetNextNumeration(numeracaoPedidos, true, false),
                        Estado = (int)Enumerations.EstadoPedidoFormacao.PedidoCriado,
                        IdEmpregado = formando.No,
                        NomeEmpregado = formando.Name,
                        FuncaoEmpregado = formando.Funcao,
                        ProjectoEmpregado = formando.Projecto,
                        IdAreaFuncional = formando.AreaNav2017,
                        AreaFuncionalEmpregado = formando.DescAreaNav2017,
                        IdCentroResponsabilidade = formando.CrespNav2017,
                        CentroResponsabilidadeEmpregado = formando.DescCrespNav2017,
                        IdEstabelecimento = formando.CodEstabelecimento,
                        DescricaoEstabelecimento = formando.DescricaoEstabelecimento,
                        UtilizadorCriacao = user.IdUtilizador,
                        DataHoraCriacao = DateTime.Now
                    };

                    using (var _ctx = new SuchDBContext())
                    {
                        _ctx.PedidoParticipacaoFormacao.Add(pedido);
                        _ctx.SaveChanges();

                        return pedido;
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }

        public static bool __CriarRegistoAlteracaoPedidoFormacao(PedidoParticipacaoFormacao pedidoFormacao, int tipoAlteracao, string descricao, string userName)
        {
            int lineNo = 10;

            if (pedidoFormacao == null)
                return false;

            if (pedidoFormacao.RegistosAlteracoes == null && pedidoFormacao.RegistosAlteracoes.Count() == 0)
            {
                pedidoFormacao.RegistosAlteracoes = __GetRegistosAlteracaoPedido(pedidoFormacao.IdPedido);
            }

            if (pedidoFormacao.RegistosAlteracoes != null && pedidoFormacao.RegistosAlteracoes.Count() > 0)
            {
                RegistoAlteracoesPedidoFormacao log = pedidoFormacao.RegistosAlteracoes.OrderBy(r => r.IdPedidoFormacao).ThenBy(r => r.IdRegisto).LastOrDefault();
                lineNo += log.IdRegisto.Value;
            }

            RegistoAlteracoesPedidoFormacao changeLog = new RegistoAlteracoesPedidoFormacao()
            {
                IdPedidoFormacao = pedidoFormacao.IdPedido,
                IdRegisto = lineNo,
                TipoAlteracao = tipoAlteracao,
                DescricaoAlteracao = descricao,
                UtilizadorAlteracao = userName,
                DataHoraAlteracao = DateTime.Now
            };


            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.RegistoAlteracoesPedidoFormacao.Add(changeLog);
                    _ctx.SaveChanges();
                }

                
                pedidoFormacao.RegistosAlteracoes.Add(changeLog);
               

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }   

        public static bool __CriarComentario(Comentario comment)
        {
            if (comment == null || string.IsNullOrEmpty(comment.NoDocumento) || string.IsNullOrEmpty(comment.UtilizadorCriacao))
            {
                return false;
            }
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    _ctx.Comentario.Add(comment);
                    _ctx.SaveChanges();
                    return true;

                }
                
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        #region Reads
        public static List<AccaoFormacao> __GetAllAccoesFormacao(DateTime aposData)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.AccaoFormacao.Where(a => a.DataInicio > aposData).ToList(); 
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AccaoFormacao> __GetAllAccoesFormacao(string idTema)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.AccaoFormacao.Where(a => a.IdTema == idTema).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AccaoFormacao> __GetAllAccoesFormacao(string idTema, DateTime aposData)
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    return _ctx.AccaoFormacao.Where(a => a.IdTema == idTema && a.DataInicio > aposData).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<EntidadeFormadora> __GetAllEntidades()
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    return _ctx.EntidadeFormadora.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<SessaoAccaoFormacao> __GetSessoesFormacao(string idAccao)
        {
            if (string.IsNullOrEmpty(idAccao))
            {
                return null;
            }
            using(var _ctx = new SuchDBContext())
            {
                try
                {
                    return _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == idAccao)
                        .OrderByDescending(a => a.DataSessao)
                        .ThenByDescending(a => a.HoraInicioSessao).ToList();
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
        }
        public static AccaoFormacao __GetDetailsAccaoFormacao(string accaoId)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    AccaoFormacao accao = _ctx.AccaoFormacao.LastOrDefault(a => a.IdAccao == accaoId);

                    accao.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == accaoId)
                        .OrderByDescending(a => a.DataSessao)
                        .ThenByDescending(a => a.HoraInicioSessao)
                        .ToList(); ;

                    if (!string.IsNullOrEmpty(accao.IdEntidadeFormadora))
                    {
                        accao.Entidade = _ctx.EntidadeFormadora.Where(e => e.IdEntidade == accao.IdEntidadeFormadora).LastOrDefault();
                    }

                    return accao;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TemaFormacao> __GetCatalogo(bool onlyActives = false)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    if (onlyActives)
                    {
                        List<TemaFormacao> temas = _ctx.TemaFormacao.Where(t => t.Activo == 1).ToList();
                        foreach (var item in temas)
                        {

                            item.AccoesTema = _ctx.AccaoFormacao.Where(a => a.IdTema == item.IdTema && a.DataInicio > DateTime.Now && a.Activa.Value == 1).ToList();                                

                            if (item.AccoesTema == null || (item.AccoesTema != null && item.AccoesTema.Count == 0))
                            {
                                item.Activo = 0;
                                __UpdateTemaFormacao(item);
                            }
                        }
                        return temas.Where(t => t.Activo.Value == 1).ToList();
                        
                    }
                    else
                    {
                        return _ctx.TemaFormacao.ToList();
                    }

                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<TemaFormacao> __GetCatalogo()
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    //int ano = DateTime.Now.Year - 1;

                    //DateTime inicio = DateTime.Parse(ano.ToString()  + "-01-01");
                    
                    List<TemaFormacao> temas = _ctx.TemaFormacao.ToList();
                    foreach (var item in temas)
                    {
                        int noMesesAnteriores = item.NoMesesAnterioresAccoes.HasValue && item.NoMesesAnterioresAccoes.Value > 0 ? item.NoMesesAnterioresAccoes.Value : NoMesesMostrarAccoesPorDefeito;
                        DateTime inicio = DateTime.Today.AddMonths((noMesesAnteriores * -1));

                        item.AccoesTema = _ctx.AccaoFormacao.Where(a => a.IdTema == item.IdTema && a.DataInicio >= inicio)
                            .OrderBy(a => a.DataInicio).ToList();

                        if (item.AccoesTema == null || (item.AccoesTema != null && item.AccoesTema.Count == 0))
                        {
                            item.Activo = 0;
                        }

                        if (item.AccoesTema != null && item.AccoesTema.Count > 0)
                        {
                            item.Activo = 1;
                        }
                    }
                    return temas.Where(t => t.Activo.Value == 1).ToList();

                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TemaFormacao __GetDetailsTema(string idTema, DateTime? filtroData = null)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    TemaFormacao tema = _ctx.TemaFormacao.FirstOrDefault(t => t.IdTema == idTema);
                    int noMesesAnteriores = tema.NoMesesAnterioresAccoes.HasValue && tema.NoMesesAnterioresAccoes.Value > 0 ? tema.NoMesesAnterioresAccoes.Value : NoMesesMostrarAccoesPorDefeito;
                    DateTime inicio;
                    if (filtroData != null)
                    {
                        inicio = filtroData.Value;
                    }
                    else
                    {
                        inicio = DateTime.Today.AddMonths((noMesesAnteriores * -1));
                    }
                    
                    tema.AccoesTema = _ctx.AccaoFormacao
                        .Where(a => a.IdTema == idTema && (a.DataInicio >= inicio))
                        .OrderByDescending(a => a.DataInicio)
                        .ThenByDescending(a => a.CodigoInterno)
                        .ToList();

                    foreach (var item in tema.AccoesTema)
                    {
                        item.Activa = item.Activa ?? 0;
                        
                        item.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == item.IdAccao).ToList();
                    }
                    return tema;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static TemaFormacaoView __GetDetailsTemaView(string idTema)
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    //DateTime inicio = DateTime.Now.AddYears(-1);
                    TemaFormacao tema = _ctx.TemaFormacao.FirstOrDefault(t => t.IdTema == idTema);
                    int noMesesAnteriores = tema.NoMesesAnterioresAccoes.HasValue && tema.NoMesesAnterioresAccoes.Value > 0 ? tema.NoMesesAnterioresAccoes.Value : NoMesesMostrarAccoesPorDefeito;
                    DateTime inicio = DateTime.Today.AddMonths((noMesesAnteriores * -1));
                    tema.AccoesTema = _ctx.AccaoFormacao
                        .Where(a => a.IdTema == idTema && (a.DataInicio >= inicio))
                        .OrderByDescending(a => a.DataInicio)
                        .ThenByDescending(a => a.CodigoInterno)
                        .ToList();

                    foreach (var item in tema.AccoesTema)
                    {
                        item.SessoesFormacao = _ctx.SessaoAccaoFormacao.Where(s => s.IdAccao == item.IdAccao).ToList();
                    }

                    TemaFormacaoView temaV = new TemaFormacaoView(tema);
                    temaV.AccoesDoTema(tema);
                    return temaV;
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public static EntidadeFormadora __GetDetailsEntidade(string idEntidade)
        {
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    return _ctx.EntidadeFormadora.FirstOrDefault(e => e.IdEntidade == idEntidade);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Comentario> __GetCometariosPedido(string idPedido)
        {
            if (string.IsNullOrEmpty(idPedido))
            {
                return null;
            }

            using (var _ctx = new SuchDBContext())
            {
                try
                {
                    return _ctx.Comentario.Where(c => c.NoDocumento == idPedido).ToList();
                }
                catch (Exception ex)
                {

                    return null;
                }                
            }
        }

        /// <summary>
        /// Este método deverá ser utilizado para obter todos os pedidos do utilizador
        /// </summary>
        /// <param name="userName">O nome do utilizdor</param>
        /// <param name="onlyActives">Apenas os pedidos que não estejam no estado Finalizado, ou em Curso</param>
        /// <returns></returns>
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(string userName, string employeeNo, bool? onlyActives)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            try
            {
                if (onlyActives.HasValue)
                {
                    if (onlyActives.Value)
                    {
                        // são considerados activos todos os pedidos que não estejam finalizados ou cancelados
                        using (var _ctx = new SuchDBContext())
                        {
                            return _ctx.PedidoParticipacaoFormacao.Where(p => (p.UtilizadorCriacao == userName || p.IdEmpregado == employeeNo) && p.Estado.Value < (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                        }
                    }
                    else
                    {
                        // todos os pedidos do utilizador que estejam finalizados ou cancelados
                        using (var _ctx = new SuchDBContext())
                        {
                            return _ctx.PedidoParticipacaoFormacao.Where(p => (p.UtilizadorCriacao == userName || p.IdEmpregado == employeeNo) && p.Estado.Value >= (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                        }
                    }
                }
                else
                {
                    // todos os pedidos do utilizador, independentemente do estado
                    using (var _ctx = new SuchDBContext())
                    {
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.UtilizadorCriacao == userName).ToList();
                    }
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(string employeeNo)
        {
            if (!string.IsNullOrEmpty(employeeNo))
            {
                try
                {
                    using (var _ctx = new SuchDBContext())
                    {
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.IdEmpregado == employeeNo).ToList();
                    }
                }
                catch (Exception ex)
                {

                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Este método deverá ser utilizado nos Fluxos de Aprovação: 
        ///     Chefia, 
        ///     Director 
        ///     CA
        /// </summary>
        /// <param name="cfgUser">Detalhes da configuração de aprovação do utilizador, relativamente à Formação</param>
        /// <param name="origin">A origem do pedido do utilizador</param>
        /// <param name="onlyCompleted">Pedidos terminados ou em curso</param>
        /// <returns></returns>
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(ConfiguracaoAprovacaoUtilizador cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade origin, bool onlyCompleted)
        {
            try
            {
                string areasDireccao = string.Join(",", cfgUser.AreasDirige);
                string areasChefia = string.Join(",", cfgUser.AreasChefia);
                string cresps = string.Join(",", cfgUser.CRespChefia);

                using(var _ctx = new SuchDBContext())
                {
                    if (cfgUser.IsChief() && cfgUser.IsDirector())
                    {
                        if (onlyCompleted)
                        {
                            List<PedidoParticipacaoFormacao> pedidosAprovadosDirector =
                                _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado &&
                                                                areasDireccao.Contains(p.IdAreaFuncional)
                                                            ).ToList();

                            List<PedidoParticipacaoFormacao> pedidosAprovadosChefia =                                
                                _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado &&
                                                                areasChefia.Contains(p.IdAreaFuncional) &&
                                                                cresps.Contains(p.IdCentroResponsabilidade)
                                                            ).ToList();

                            return pedidosAprovadosDirector.Union(pedidosAprovadosChefia).ToList(); 
                        }
                        else
                        {
                            List<PedidoParticipacaoFormacao> pedidosAprovarDirector =
                                _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoSubmetido &&
                                                                areasDireccao.Contains(p.IdAreaFuncional)
                                                            ).ToList();

                            List<PedidoParticipacaoFormacao> pedidosAprovarChefia =
                                _ctx.PedidoParticipacaoFormacao.Where(
                                                                p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoSubmetido &&
                                                                areasChefia.Contains(p.IdAreaFuncional) &&
                                                                cresps.Contains(p.IdCentroResponsabilidade)
                                                            ).ToList();

                            return pedidosAprovarDirector.Union(pedidosAprovarChefia).ToList();
                        }
                    }
                    else
                    {
                        if (origin == Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia && cfgUser.IsChief())
                        {
                            if (onlyCompleted)
                            {
                                // pedidos Finalizados
                                return _ctx.PedidoParticipacaoFormacao.Where(
                                                                    p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                    p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado &&
                                                                    areasChefia.Contains(p.IdAreaFuncional) &&
                                                                    cresps.Contains(p.IdCentroResponsabilidade)
                                                                ).ToList();
                            }
                            else
                            {
                                // pedidos submetidos
                                return _ctx.PedidoParticipacaoFormacao.Where(
                                                                    p => p.IdEmpregado != cfgUser.EmployeeNo &&
                                                                    p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoSubmetido &&
                                                                    areasChefia.Contains(p.IdAreaFuncional) &&
                                                                    cresps.Contains(p.IdCentroResponsabilidade)
                                                                ).ToList();
                            }

                        }

                        if (origin == Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector && cfgUser.IsDirector())
                        {
                            if (onlyCompleted)
                            {
                                // pedidos Finalizados
                                return _ctx.PedidoParticipacaoFormacao.Where(
                                                                    p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado &&
                                                                    areasDireccao.Contains(p.IdAreaFuncional)
                                                                ).ToList();
                            }
                            else
                            {
                                // pedidos submetidos ou devolvidos pela Academia para completar
                                return _ctx.PedidoParticipacaoFormacao.Where(
                                                                    p => (p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoAprovadoChefia ||
                                                                     p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoRejeitadoAcademia) &&
                                                                    areasDireccao.Contains(p.IdAreaFuncional)
                                                                ).ToList();
                            }

                        }
                    }

                    

                    if(origin == Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuCA && cfgUser.TipoUtilizadorGlobal == Enumerations.TipoUtilizadorFluxoPedidoFormacao.ConselhoAdministracao)
                    {
                        if (onlyCompleted)
                        {
                            // pedidos Finalizados
                            return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                        }
                        else
                        {
                            // pedidos Analisados
                            return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoAnalisadoAcademia).ToList();
                        }
                    }
                }

                return null;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// Este método deverá ser utilizado para obter os Pedidos que estão no Fluxo de tratamento pela Academia: 
        ///     Aprovado Direcção, 
        ///     Devolvido Direcção, 
        ///     Analisado,
        ///     Autorizado,
        ///     Rejeitado CA
        /// </summary>
        /// <param name="origin">A origem do pedido do utilizador</param>
        /// <param name="state">O estado dos pedidos que se pretendem obter</param>
        /// <param name="onlyCompleted">Apenas Pedidos terminados</param>
        /// <returns></returns>
        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade origin, int state, bool onlyCompleted)
        {
            if (origin != Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao)
                return null;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    if (onlyCompleted)
                    {
                        // pedidos Finalizados
                        return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                    }
                    else
                    {
                        // Os pedidos finalizados são tratados no ciclo if acima
                        if (state < (int)Enumerations.EstadoPedidoFormacao.PedidoAprovadoDireccao && state > (int)Enumerations.EstadoPedidoFormacao.PedidoRejeitadoCA)
                            return null;

                        return _ctx.PedidoParticipacaoFormacao.Where(
                            p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoAprovadoDireccao && 
                            p.Estado.Value < (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();


                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<PedidoParticipacaoFormacao> __GetAllPedidosFormacao(Enumerations.AcademiaOrigemAcessoFuncionalidade origin, int state)
        {
            if (origin != Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuGestao)
                return null;

            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    //if (onlyCompleted)
                    //{
                    //    // pedidos Finalizados
                    //    return _ctx.PedidoParticipacaoFormacao.Where(p => p.Estado.Value == (int)Enumerations.EstadoPedidoFormacao.PedidoFinalizado).ToList();
                    //}
                    //else
                    //{
                        // Os pedidos finalizados são tratados no ciclo if acima
                        if (state < (int)Enumerations.EstadoPedidoFormacao.PedidoAprovadoDireccao && state > (int)Enumerations.EstadoPedidoFormacao.PedidoRejeitadoCA)
                            return null;

                        return _ctx.PedidoParticipacaoFormacao.Where(
                            p => p.Estado.Value == state).ToList();


                    //}
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static PedidoParticipacaoFormacao __GetDetailsPedidoFormacao(string pedidoId)
        {
            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    PedidoParticipacaoFormacao pedido = _ctx.PedidoParticipacaoFormacao.Where(p => p.IdPedido == pedidoId).LastOrDefault();

                    pedido.RegistosAlteracoes = _ctx.RegistoAlteracoesPedidoFormacao.Where(r => r.IdPedidoFormacao == pedidoId).ToList();

                    if (!string.IsNullOrEmpty(pedido.IdAccaoFormacao))
                    {
                        pedido.Accao = _ctx.AccaoFormacao.FirstOrDefault(a => a.IdAccao == pedido.IdAccaoFormacao);
                    }
                    
                    return pedido;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        public static List<RegistoAlteracoesPedidoFormacao> __GetRegistosAlteracaoPedido(string pedidoId)
        {
            if (string.IsNullOrEmpty(pedidoId))
                return null;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    return _ctx.RegistoAlteracoesPedidoFormacao.Where(r => r.IdPedidoFormacao == pedidoId).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Formando> __GetAllFormandos(ConfiguracaoAprovacaoUtilizador cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade origin)
        {
            Enumerations.AcademiaOrigemAcessoFuncionalidade chief = Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuChefia;
            Enumerations.AcademiaOrigemAcessoFuncionalidade director = Enumerations.AcademiaOrigemAcessoFuncionalidade.MenuDirector;

            if ((origin == chief && (cfgUser.AreasChefia == null || cfgUser.AreasChefia.Count() == 0)) ||
                (origin == chief && (cfgUser.CRespChefia == null || cfgUser.CRespChefia.Count() == 0)))
            {
                return null;
            }

            if(origin == director && (cfgUser.AreasDirige == null || cfgUser.AreasDirige.Count() == 0))
            {
                return null;
            }

            try
            {
                using(var _ctxExt = new SuchDBContextExtention())
                {
                    List<Formando> result = new List<Formando>();

                    var parameters = new[]
                    {
                            new SqlParameter("@EmployeeNo", null)
                    };

                    IEnumerable<dynamic> data = _ctxExt.execStoredProcedure("exec NAV2009Formandos @EmployeeNo", parameters);

                    foreach (var tmp in data)
                    {
                        if (!tmp.NoMecanografico.Equals(DBNull.Value))
                        {
                            Formando formando = new Formando()
                            {
                                No = (string)tmp.NoMecanografico,
                                Name = tmp.NomeCompleto.Equals(DBNull.Value) ? string.Empty : (string)tmp.NomeCompleto,
                                Regiao = tmp.Regiao.Equals(DBNull.Value) ? string.Empty : (string)tmp.Regiao,
                                Area = tmp.Area.Equals(DBNull.Value) ? string.Empty : (string)tmp.Area,
                                Cresp = tmp.CResp.Equals(DBNull.Value) ? string.Empty : (string)tmp.CResp,
                                RegiaoNav2017 = tmp.RegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.RegiaoNav2017,
                                DescRegiaoNav2017 = tmp.DescRegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.DescRegiaoNav2017,
                                AreaNav2017 = tmp.AreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.AreaNav2017,
                                DescAreaNav2017 = tmp.DescAreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.DescAreaNav2017,
                                CrespNav2017 = tmp.CRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.CRespNav2017,
                                DescCrespNav2017 = tmp.DescCRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)tmp.DescCRespNav2017,
                                Projecto = tmp.NoProjecto.Equals(DBNull.Value) ? string.Empty : (string)tmp.NoProjecto,
                                Funcao = tmp.Funcao.Equals(DBNull.Value) ? string.Empty : (string)tmp.Funcao,
                                CodEstabelecimento = tmp.CodEstabelecimento.Equals(DBNull.Value) ? string.Empty : (string)tmp.CodEstabelecimento,
                                DescricaoEstabelecimento = tmp.DescEstabelecimento.Equals(DBNull.Value)? string.Empty : (string)tmp.DescEstabelecimento
                            };

                            result.Add(formando);
                        }
                    }

                    if (cfgUser.IsChief() && cfgUser.IsDirector())
                    {
                        List<Formando> isChiefOfFormandos = result.Where(r => cfgUser.AreasChefia.Contains(r.AreaNav2017)).ToList()
                            .Where(r => cfgUser.CRespChefia.Contains(r.CrespNav2017)).ToList();

                        List<Formando> isDirectorOfFormandos = result.Where(r => cfgUser.AreasDirige.Contains(r.AreaNav2017)).ToList();

                        result = null;

                        result = isDirectorOfFormandos.Union(isChiefOfFormandos)
                            .OrderBy(r => r.AreaNav2017).ThenBy(r => r.CrespNav2017).ThenBy(r => r.No)
                            .ToList();
                    }
                    else
                    {
                        if (origin == chief)
                        {
                            result = result.OrderBy(r => r.AreaNav2017).ThenBy(r => r.CrespNav2017).ThenBy(r => r.No)
                                .Where(r => cfgUser.AreasChefia.Contains(r.AreaNav2017)).ToList()
                                .Where(r => cfgUser.CRespChefia.Contains(r.CrespNav2017)).ToList();
                        }

                        if (origin == director)
                        {
                            result = result.OrderBy(r => r.AreaNav2017).ThenBy(r => r.CrespNav2017).ThenBy(r => r.No)
                                .Where(r => cfgUser.AreasDirige.Contains(r.AreaNav2017)).ToList();
                        }
                    }

                    

                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Formando __GetDetailsFormando(string employeeNo)
        {
            if (string.IsNullOrWhiteSpace(employeeNo))
                return null;
            try
            {
                using(var _ctxExt = new SuchDBContextExtention())
                {
                    
                    var parameters = new[]
                    {
                            new SqlParameter("@EmployeeNo", employeeNo)
                    };

                    dynamic data = _ctxExt.execStoredProcedure("exec NAV2009Formandos @EmployeeNo", parameters).FirstOrDefault();

                    if (data != null)
                    {
                        if (!data.NoMecanografico.Equals(DBNull.Value))
                        {
                            Formando formando = new Formando()
                            {
                                No = (string)data.NoMecanografico,
                                Name = data.NomeCompleto.Equals(DBNull.Value) ? string.Empty : (string)data.NomeCompleto,
                                Regiao = data.Regiao.Equals(DBNull.Value) ? string.Empty : (string)data.Regiao,
                                Area = data.Area.Equals(DBNull.Value) ? string.Empty : (string)data.Area,
                                Cresp = data.CResp.Equals(DBNull.Value) ? string.Empty : (string)data.CResp,
                                RegiaoNav2017 = data.RegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.RegiaoNav2017,
                                DescRegiaoNav2017 = data.DescRegiaoNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.DescRegiaoNav2017,
                                AreaNav2017 = data.AreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.AreaNav2017,
                                DescAreaNav2017 = data.DescAreaNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.DescAreaNav2017,
                                CrespNav2017 = data.CRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.CRespNav2017,
                                DescCrespNav2017 = data.DescCRespNav2017.Equals(DBNull.Value) ? string.Empty : (string)data.DescCRespNav2017,
                                Projecto = data.NoProjecto.Equals(DBNull.Value) ? string.Empty : (string)data.NoProjecto,
                                Funcao = data.Funcao.Equals(DBNull.Value) ? string.Empty : (string)data.Funcao,
                                CodEstabelecimento = data.CodEstabelecimento.Equals(DBNull.Value) ? string.Empty : (string)data.CodEstabelecimento,
                                DescricaoEstabelecimento = data.DescEstabelecimento.Equals(DBNull.Value) ? string.Empty : (string)data.DescEstabelecimento
                            };

                            return formando;
                        } 
                    }

                }
            }
            catch (Exception ex)
            {

                return null;
            }

            return null;
        }
        #endregion

        #region Updates
        public static bool __UpdatePedidoFormacao(PedidoParticipacaoFormacao pedido)
        {
            if (pedido == null)
                return false;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.PedidoParticipacaoFormacao.Update(pedido);
                    _ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            
        }


        public static bool __UpdateAccaoFormacao(AccaoFormacao accao)
        {
            if (accao == null)
                return false;

            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    _ctx.AccaoFormacao.Update(accao);
                    _ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool __UpdateAccaoFormacao(AccaoFormacaoView accaoV)
        {
            try
            {
                foreach (var item in accaoV.ImagensAccao)
                {
                    DBAttachments.Update(DBAttachments.ParseToDB(item));
                }
                return __UpdateAccaoFormacao(accaoV.ParseToDb());
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool __UpdateTemaFormacao(TemaFormacao tema)
        {
            if (tema == null)
                return false;

            try
            {
                using(var _ctx = new SuchDBContext())
                {
                    _ctx.TemaFormacao.Update(tema);
                    _ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool __UpdateTemaFormacao(TemaFormacaoView temaV)
        {
            try
            {
                TemaFormacao tema = temaV.ParseToDb();               

                foreach(var t in temaV.ImagensTema)
                {
                    DBAttachments.Update(DBAttachments.ParseToDB(t));
                }

                if (temaV.Accoes != null && temaV.Accoes.Count > 0)
                {
                    foreach (var item in temaV.Accoes)
                    {
                        __UpdateAccaoFormacao(item.ParseToDb());
                    }
                }
                
                return __UpdateTemaFormacao(tema);
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool __UpdateComentario(Comentario comment)
        {
            if (comment == null || string.IsNullOrEmpty(comment.NoDocumento))
            {
                return false;
            }
            try
            {
                using (var _ctx = new SuchDBContext())
                {
                    _ctx.Comentario.Update(comment);
                    _ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region Deletes
        public static bool __DeletePedidoFormacao(string pedidoId)
        {
            using(var _ctx = new SuchDBContext())
            {
                PedidoParticipacaoFormacao pedido = _ctx.PedidoParticipacaoFormacao.Where(p => p.IdPedido == pedidoId).LastOrDefault();
                if(pedido != null)
                {
                    pedido.RegistosAlteracoes = _ctx.RegistoAlteracoesPedidoFormacao.Where(r => r.IdPedidoFormacao == pedidoId).ToList();

                    if(pedido.RegistosAlteracoes != null)
                    {
                        _ctx.RegistoAlteracoesPedidoFormacao.RemoveRange(pedido.RegistosAlteracoes);
                    }

                    _ctx.PedidoParticipacaoFormacao.Remove(pedido);
                    _ctx.SaveChanges();

                }
            }
            return false;
        }
        #endregion

    }
}
