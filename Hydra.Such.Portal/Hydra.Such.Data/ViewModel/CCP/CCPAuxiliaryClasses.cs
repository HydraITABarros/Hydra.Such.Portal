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
            eMessage = "Não foipossível actualizar o Fluxo"
        };

        public static ErrorHandler InvalidEmailAddres = new ErrorHandler
        {
            eReasonCode = 400,
            eMessage = "Endereço de email inválido"
        };
        public static ErrorHandler AddressListIsEmpty = new ErrorHandler
        {
            eReasonCode = 401,
            eMessage = "Lista de endereços inválida"
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



    }

}
