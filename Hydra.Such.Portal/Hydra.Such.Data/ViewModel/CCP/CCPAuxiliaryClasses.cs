using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.CCP;
using Hydra.Such.Data.ViewModel.CCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hydra.Such.Data.ViewModel.CCP
{
    //public class ElementosChecklist
    //{
    //    public string ProcedimentoID { get; set; }
    //    public int Estado { get; set; }
    //    public DateTime DataChecklist { get; set; }
    //    public TimeSpan HoraChecklist { get; set; }

    //}

    //public class ElementosChecklistArea : ElementosChecklist
    //{
    //    public string ComentarioArea { get; set; }
    //    public string ResponsavelArea { get; set; }
    //    public string NomeResponsavelArea { get; set; } 
    //    public DateTime DataResponsavel { get; set; }

    //    public ElementosChecklistArea()
    //    {

    //    }
    //    public ElementosChecklistArea(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ProcedimentoID = fluxo.No;
    //        Estado = fluxo.Estado;
    //        DataChecklist = fluxo.Data;
    //        HoraChecklist = fluxo.Hora;

    //        ComentarioArea = fluxo.Comentario;
    //        ResponsavelArea = fluxo.User;
    //        NomeResponsavelArea = fluxo.NomeUser;
    //        DataResponsavel = fluxo.Data;
    //    }

    //    public ElementosChecklistArea(string ProcId, int estado, string comentario)
    //    {
    //        ProcedimentoID = ProcId;
    //        DataChecklist = DateTime.Now;
    //        HoraChecklist = DateTime.Now.TimeOfDay;
    //        Estado = estado;
    //        ComentarioArea = comentario;
    //    }
    //}

    //public class ElementosChecklistImobilizadoContabilidade
    //{
    //    public string ComentarioImobContabilidade { get; set; }
    //    public string ComentarioImobContabilidade2 { get; set; }
    //    public bool ImobilizadoSimNao { get; set; }
    //    public string ResponsavelImobContabilidade { get; set; }
    //    public string NomeResponsavelImobContabilidade { get; set; }
    //    public DateTime DataImobContabilidade { get; set; }

    //    public ElementosChecklistImobilizadoContabilidade(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioImobContabilidade = fluxo.Comentario;
    //        ComentarioImobContabilidade2 = fluxo.Comentario2;
    //        if (fluxo.ImobSimNao.HasValue)
    //        {
    //            ImobilizadoSimNao = fluxo.ImobSimNao.Value;
    //        }
    //        else
    //        {
    //            ImobilizadoSimNao = false;
    //        }
    //        ResponsavelImobContabilidade = fluxo.User;
    //        NomeResponsavelImobContabilidade = fluxo.NomeUser;
    //        DataImobContabilidade = fluxo.Data;
    //    }
    //}

    //public class ElementosChecklistImobilizadoArea
    //{
    //    public string ComentarioImobArea { get; set; }
    //    public string ResponsavelImobArea { get; set; }
    //    public string NomeResponsavelImobArea { get; set; }
    //    public DateTime DataImobArea { get; set; }
    //    public string EmailDestinoCA { get; set; }

    //    public ElementosChecklistImobilizadoArea(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioImobArea = fluxo.Comentario;
    //        ResponsavelImobArea = fluxo.User;
    //        NomeResponsavelImobArea = fluxo.NomeUser;
    //        DataImobArea = fluxo.Data;
    //    }
    //}

    //public class ElementosChecklistImobilizadoCA
    //{
    //    public string ComentarioImobCA { get; set; }
    //    public string ResponsavelImobCA { get; set; }
    //    public string NomeResponsavelImobCA { get; set; }
    //    public DateTime DataImobAprovisionamentoCA { get; set; }

    //    public ElementosChecklistImobilizadoCA(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioImobCA = fluxo.Comentario;
    //        ResponsavelImobCA = fluxo.User;
    //        NomeResponsavelImobCA = fluxo.NomeUser;
    //        DataImobAprovisionamentoCA = fluxo.Data;
    //    }
    //}

    //public class ElementosChecklistFundamentoCompras
    //{
    //    public string ComentarioFundamentoCompras { get; set; }
    //    public string ResponsavelFundamentoCompras { get; set; }    // zpgm. NAV label: "Responsável pelo envio para a área"
    //    public string NomeResponsavelFundamentoCompras { get; set; }
    //    public DateTime DataEnvio { get; set; }

    //    public ElementosChecklistFundamentoCompras(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioFundamentoCompras = fluxo.Comentario;
    //        ResponsavelFundamentoCompras = fluxo.User;
    //        NomeResponsavelFundamentoCompras = fluxo.NomeUser;
    //        DataEnvio = fluxo.Data;
    //    }
    //}

    //public class ElementosChecklistFundamentoFinanceiros
    //{
    //    public string ComentarioFundamentoFinanceiros { get; set; }
    //    public string ComentarioFundamentoFinanceiros2 { get; set; }
    //    public string ResponsavelFundamentoFinanceiros { get; set; }    // zpgm. NAV label: "Responsável dos serviços financeiros"
    //    public string NomeResponsavelFundamentoFinanceiros { get; set; }
    //    public DateTime DataFinanceiros { get; set; }

    //    public ElementosChecklistFundamentoFinanceiros(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioFundamentoFinanceiros = fluxo.Comentario;
    //        ComentarioFundamentoFinanceiros2 = fluxo.Comentario2;
    //        ResponsavelFundamentoFinanceiros = fluxo.User;
    //        NomeResponsavelFundamentoFinanceiros = fluxo.NomeUser;
    //        DataFinanceiros = fluxo.Data;
    //    }
    //}

    //public class ElementosChecklistJuridico
    //{
    //    public string ComentarioJuridico { get; set; }
    //    public string ResponsavelJuridico { get; set; }    
    //    public string NomeResponsavelJuridico { get; set; }
    //    public DateTime DataJuridico { get; set; }

    //    public ElementosChecklistJuridico(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioJuridico = fluxo.Comentario;
    //        ResponsavelJuridico = fluxo.User;
    //        NomeResponsavelJuridico = fluxo.NomeUser;
    //        DataJuridico = fluxo.Data;
    //    }
    //}

    //public class ElementosChecklistAberturaCA
    //{
    //    public string ComentarioCA { get; set; }
    //    public string ResponsavelCA { get; set; }
    //    public string NomeResponsavelCA { get; set; }
    //    public DateTime DataAberturaCA { get; set; }

    //    public ElementosChecklistAberturaCA(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioCA = fluxo.Comentario;
    //        ResponsavelCA = fluxo.User;
    //        NomeResponsavelCA = fluxo.NomeUser;
    //        DataAberturaCA = fluxo.Data;
    //    }

    //}

    //public class ElementosChecklistAdjudicacaoCompras
    //{
    //    public string ComentarioAdjudicacao { get; set; }
    //    public string ResponsavelAdjudicacao { get; set; }
    //    public string NomeResponsavelAdjudicacao { get; set; }
    //    public DateTime DataAdjudicacao { get; set; }

    //    public ElementosChecklistAdjudicacaoCompras(FluxoTrabalhoListaControlo fluxo)
    //    {
    //        ComentarioAdjudicacao = fluxo.Comentario;
    //        ResponsavelAdjudicacao = fluxo.User;
    //        NomeResponsavelAdjudicacao = fluxo.NomeUser;
    //        DataAdjudicacao = fluxo.Data;
    //    }
    //}

    public static class ReturnHandlers
    {
        /*
         * Return Handlers categories:
         * -9 - Generic/Unkmown failure
         * -2 - Record Already exists
         * -1 - Object is empty or null
         * 0 - Success
         * 1xx - Create errors
         * 2xx - Update Errors
         * 3xx - Read Erros
         * 4xx - Email related errors
         * 5xx - User Permissons
         * 6xx - Procedimento status errors
         * 7xx - Unknown data
         * 
         */

        public static ErrorHandler Error = new ErrorHandler
        {
            eReasonCode = -9,
            eMessage = "Erro"
        };
        public static ErrorHandler ExistsData = new ErrorHandler
        {
            eReasonCode = -2,
            eMessage = "Já existe um registo"
        };
        public static ErrorHandler NoData = new ErrorHandler
        {
            eReasonCode = -1,
            eMessage = "Sem dados"
        };
        public static ErrorHandler Success = new ErrorHandler
        {
            eReasonCode = 0,
            eMessage = "Sucesso"
        };

        public static ErrorHandler UnableToCreateEmailProcedimento = new ErrorHandler
        {
            eReasonCode = 100,
            eMessage = "Não foi possível criar Email Procedimento"
        };
        public static ErrorHandler UnableToCreateFluxo = new ErrorHandler
        {
            eReasonCode = 101,
            eMessage = "Não foi possível criar o Fluxo de Trabalho"
        };
        public static ErrorHandler UnableToCreateTemposPA = new ErrorHandler
        {
            eReasonCode = 102,
            eMessage = "Não foi possível criar os Tempos PA"
        };

        public static ErrorHandler UnableToUpdateProcedimento = new ErrorHandler
        {
            eReasonCode = 200,
            eMessage = "Não foi possível actualizar o Procedimento"
        };
        public static ErrorHandler UnableToUpdateTemposPA = new ErrorHandler
        {
            eReasonCode = 201,
            eMessage = "Não foi possível actualizar os Tempos PA"
        };
        public static ErrorHandler UnableToUpdateFluxo = new ErrorHandler
        {
            eReasonCode = 202,
            eMessage = "Não foi possível actualizar o Fluxo"
        };
        //NR 20180314
        public static ErrorHandler ProcedimentoNotPublished = new ErrorHandler
        {
            eReasonCode = 203,
            eMessage = "O Procedimento ainda não foi publicado"
        };
        //NR 20180315
        public static ErrorHandler ProcedimentoPlatformNotGathering = new ErrorHandler
        {
            eReasonCode = 204,
            eMessage = "Ainda não foi registada a recolha da plataforma"
        };
        //NR 20180315
        public static ErrorHandler ProcedimentoPreliminaryReportNotValidated = new ErrorHandler
        {
            eReasonCode = 205,
            eMessage = "Ainda não foi validado o relatório preliminar"
        };
        //NR 20180315
        public static ErrorHandler ProcedimentoPriorHearingNotRegistered = new ErrorHandler
        {
            eReasonCode = 206,
            eMessage = "Ainda não foi registada a Audiência Prévia"
        };

        public static ErrorHandler InvalidEmailAddress = new ErrorHandler
        {
            eReasonCode = 400,
            eMessage = "Endereço de email inválido"
        };
        public static ErrorHandler AddressListIsEmpty = new ErrorHandler
        {
            eReasonCode = 401,
            eMessage = "Lista de endereços inválida"
        };
        //NR 20180308
        public static ErrorHandler EmptyCAEmailAddress = new ErrorHandler
        {
            eReasonCode = 402,
            eMessage = "Tem que indicar o email do destinatário do CA!"
        };

        public static ErrorHandler UserNotAllowed = new ErrorHandler
        {
            eReasonCode = 500,
            eMessage = "Utilizador não tem permissões para efectuar esta acção"
        };

        public static ErrorHandler StateNotAllowed = new ErrorHandler
        {
            eReasonCode = 600,
            eMessage = "Estado actual não permite esta opção"
        };
        public static ErrorHandler ProcedimentoAlreadySubmitted = new ErrorHandler
        {
            eReasonCode = 601,
            eMessage = "Procedimento já submetido"
        };
        //NR 20180319
        public static ErrorHandler ProcedimentoAlreadyConfirmed = new ErrorHandler
        {
            eReasonCode = 602,
            eMessage = "Procedimento já foi Confirmado"
        };
        //NR 20180320
        public static ErrorHandler ProcedimentoAlreadyTreated = new ErrorHandler
        {
            eReasonCode = 603,
            eMessage = "Este Procedimento já foi tratado. Verifique o WorkFlow"
        };
        //NR 20180323
        public static ErrorHandler ProcedimentoNotImmobilized = new ErrorHandler
        {
            eReasonCode = 604,
            eMessage = "Este Procedimento não é de imobilizado!"
        };
        public static ErrorHandler ProcedimentoAlreadyAuthorized = new ErrorHandler
        {
            eReasonCode = 605,
            eMessage = "Este Procedimento já foi Autorizado!"
        };
        //NR 20180326
        public static ErrorHandler ProcedimentoNotPossibleAuth = new ErrorHandler
        {
            eReasonCode = 606,
            eMessage = "Neste estado não pode autorizar o processo!"
        };
        //NR 20180328
        public static ErrorHandler ProcedimentoNotPossibleChangeToUpper = new ErrorHandler
        {
            eReasonCode = 607,
            eMessage = "Não pode Avançar para um estado seguinte ou igual!"
        };
        public static ErrorHandler ProcedimentoNotPossibleChangeToImob = new ErrorHandler
        {
            eReasonCode = 608,
            eMessage = "Não pode alterar para este estado! A Aquisição não é de Imobilizado"
        };

        //NR 20180329
        public static ErrorHandler ProcedimentoAtaNumberExists = new ErrorHandler
        {
            eReasonCode = 609,
            eMessage = "Já existe este Nº de Ata!"
        };

        // ALT_CCP_#001.y2019.b
        public static ErrorHandler BatchAlreadyAwarded = new ErrorHandler
        {
            eReasonCode = 680,
            eMessage = "Lote já adjudicado!"
        };

        public static ErrorHandler UnableToUpdateBatch = new ErrorHandler
        {
            eReasonCode = 681,
            eMessage = "Não foi possivel actualizar o Lote!"
        };
        // ALT_CCP_#001.y2019.e

        public static ErrorHandler UnknownArea = new ErrorHandler
        {
            eReasonCode = 700,
            eMessage = "Área Funcional sem correspondência para as Áreas do Portal!"
        };
        public static ErrorHandler ProcessNameNotSet = new ErrorHandler
        {
            eReasonCode = 701,
            eMessage = "Tem que definir o Nome do Procedimento"
        };

        //NR 20180308
        public static ErrorHandler UnknownTipoProcedimento = new ErrorHandler
        {
            eReasonCode = 702,
            eMessage = "Antes de prosseguir, tem que definir o tipo de procedimento"
        };

        //NR 20180314
        public static ErrorHandler UnknownDatePublicacao = new ErrorHandler
        {
            eReasonCode = 703,
            eMessage = "Antes de prosseguir, tem que indicar uma data!"
        };

        //NR 20180319
        public static ErrorHandler UncompletedDecisionProposal = new ErrorHandler
        {
            eReasonCode = 704,
            eMessage = "Proposta de decisão tem que estar preenchida"
        };

    }

}
