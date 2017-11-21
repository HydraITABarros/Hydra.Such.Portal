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
}
