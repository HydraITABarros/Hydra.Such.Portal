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
    }
    public class ElementosChecklistArea
    {
        public string ComentarioAreaApresentar { get; set; }
        public string ResponsavelAreaApresentar { get; set; }
        public string NomeResponsavelAreaApresentar { get; set; } 
        public DateTime DataResponsavelApresentar { get; set; }

        public ElementosChecklistArea(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioAreaApresentar = fluxo.Comentario;
            ResponsavelAreaApresentar = fluxo.User;
            NomeResponsavelAreaApresentar = fluxo.NomeUser;
            DataResponsavelApresentar = fluxo.Data;
        }
    }

    public class ElementosChecklistImobilizado
    {
        public string ComentarioContabilidade { get; set; }
        public string ComentarioContabilidade2 { get; set; }
        public bool ImobilizadoSimNao { get; set; }
        public string ResponsavelContabilidade { get; set; }
        public string NomeResponsavelContabilidade { get; set; }
        public DateTime DataContabilidade { get; set; }

        public ElementosChecklistImobilizado(FluxoTrabalhoListaControlo fluxo)
        {
            ComentarioContabilidade = fluxo.Comentario;
            ComentarioContabilidade2 = fluxo.Comentario2;
            if (fluxo.ImobSimNao.HasValue)
            {
                ImobilizadoSimNao = fluxo.ImobSimNao.Value;
            }
            else
            {
                ImobilizadoSimNao = false;
            }
            ResponsavelContabilidade = fluxo.User;
            NomeResponsavelContabilidade = fluxo.NomeUser;
            DataContabilidade = fluxo.Data;
        }
    }
}
