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
    public class ElementosChecklist
    {
        public string ProcedimentoID { get; set; }
        public int Estado { get; set; }
        public DateTime DataChecklist { get; set; }
        public TimeSpan HoraChecklist { get; set; }

        public ElementosChecklistArea ChecklistArea { get; set; }
        public ElementosChecklistImobilizadoContabilidade ChecklistImobilizadoContabilidade { get; set; }
        public ElementosChecklistImobilizadoArea CkecklistImobilizadoArea { get; set; }
        public ElementosChecklistImobilizadoCA ChecklistImobilizadoCA { get; set; }
        public ElementosChecklistFundamentoCompras ChecklistFundamentoCompras { get; set; }
        public ElementosChecklistFundamentoFinanceiros ChecklistFundamentoFinanceiros { get; set; }
        public ElementosChecklistJuridico ChecklistJuridico { get; set; }

    }

    public class ElementosGeneric
    {
        public string Comentario { get; set; }
        public string Responsavel { get; set; }
        public string NomeResponsavel { get; set; }
        public DateTime Data { get; set; }

    }
    public class ElementosChecklistArea
    {
        public string ComentarioArea { get; set; }
        public string ResponsavelArea { get; set; }
        public string NomeResponsavelArea { get; set; } 
        public DateTime DataResponsavel { get; set; }

        public ElementosChecklistArea(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioArea = fluxo.Comentario;
            ResponsavelArea = fluxo.User;
            NomeResponsavelArea = fluxo.NomeUser;
            DataResponsavel = fluxo.Data;
        }
    }

    public class ElementosChecklistImobilizadoContabilidade
    {
        public string ComentarioImobContabilidade { get; set; }
        public string ComentarioImobContabilidade2 { get; set; }
        public bool ImobilizadoSimNao { get; set; }
        public string ResponsavelImobContabilidade { get; set; }
        public string NomeResponsavelImobContabilidade { get; set; }
        public DateTime DataImobContabilidade { get; set; }

        public ElementosChecklistImobilizadoContabilidade(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioImobContabilidade = fluxo.Comentario;
            ComentarioImobContabilidade2 = fluxo.Comentario2;
            if (fluxo.ImobSimNao.HasValue)
            {
                ImobilizadoSimNao = fluxo.ImobSimNao.Value;
            }
            else
            {
                ImobilizadoSimNao = false;
            }
            ResponsavelImobContabilidade = fluxo.User;
            NomeResponsavelImobContabilidade = fluxo.NomeUser;
            DataImobContabilidade = fluxo.Data;
        }
    }

    public class ElementosChecklistImobilizadoArea
    {
        public string ComentarioImobArea { get; set; }
        public string ResponsavelImobArea { get; set; }
        public string NomeResponsavelImobArea { get; set; }
        public DateTime DataImobArea { get; set; }
        public string EmailDestinoCA { get; set; }

        public ElementosChecklistImobilizadoArea(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioImobArea = fluxo.Comentario;
            ResponsavelImobArea = fluxo.User;
            NomeResponsavelImobArea = fluxo.NomeUser;
            DataImobArea = fluxo.Data;
        }
    }

    public class ElementosChecklistImobilizadoCA
    {
        public string ComentarioImobCA { get; set; }
        public string ResponsavelImobCA { get; set; }
        public string NomeResponsavelImobCA { get; set; }
        public DateTime DataImobAprovisionamentoCA { get; set; }

        public ElementosChecklistImobilizadoCA(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioImobCA = fluxo.Comentario;
            ResponsavelImobCA = fluxo.User;
            NomeResponsavelImobCA = fluxo.NomeUser;
            DataImobAprovisionamentoCA = fluxo.Data;
        }
    }

    public class ElementosChecklistFundamentoCompras
    {
        public string ComentarioFundamentoCompras { get; set; }
        public string ResponsavelFundamentoCompras { get; set; }    // zpgm. NAV label: "Responsável pelo envio para a área"
        public string NomeResponsavelFundamentoCompras { get; set; }
        public DateTime DataEnvio { get; set; }

        public ElementosChecklistFundamentoCompras(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioFundamentoCompras = fluxo.Comentario;
            ResponsavelFundamentoCompras = fluxo.User;
            NomeResponsavelFundamentoCompras = fluxo.NomeUser;
            DataEnvio = fluxo.Data;
        }
    }

    public class ElementosChecklistFundamentoFinanceiros
    {
        public string ComentarioFundamentoFinanceiros { get; set; }
        public string ComentarioFundamentoFinanceiros2 { get; set; }
        public string ResponsavelFundamentoFinanceiros { get; set; }    // zpgm. NAV label: "Responsável dos serviços financeiros"
        public string NomeResponsavelFundamentoFinanceiros { get; set; }
        public DateTime DataFinanceiros { get; set; }

        public ElementosChecklistFundamentoFinanceiros(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioFundamentoFinanceiros = fluxo.Comentario;
            ComentarioFundamentoFinanceiros2 = fluxo.Comentario2;
            ResponsavelFundamentoFinanceiros = fluxo.User;
            NomeResponsavelFundamentoFinanceiros = fluxo.NomeUser;
            DataFinanceiros = fluxo.Data;
        }
    }

    public class ElementosChecklistJuridico
    {
        public string ComentarioJuridico { get; set; }
        public string ResponsavelJuridico { get; set; }    
        public string NomeResponsavelJuridico { get; set; }
        public DateTime DataJuridico { get; set; }

        public ElementosChecklistJuridico(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioJuridico = fluxo.Comentario;
            ResponsavelJuridico = fluxo.User;
            NomeResponsavelJuridico = fluxo.NomeUser;
            DataJuridico = fluxo.Data;
        }
    }

}
