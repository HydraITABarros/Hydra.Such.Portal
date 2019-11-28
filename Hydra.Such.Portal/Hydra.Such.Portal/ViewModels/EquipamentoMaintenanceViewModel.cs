using Hydra.Such.Data.Evolution.DatabaseReference;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.ViewModels
{

    public class EquipamentMaintenancePlanViewModel
    {
        public int Id { get; set; }               
        public int Categoria { get; set; }               
        public string EquipmentCresp { get; set; }
        public string CategoriaText { get; set; }
        public int Marca { get; set; }
        public string MarcaText { get; set; }
        public int IdServico { get; set; }
        public string ServicoText { get; set; }
        public int IdModelo { get; set; }
        public string ModeloText { get; set; }
        public string Estado { get; set; }
        public string haveCurative { get; set; }
        public int IdEquipamento { get; set; }
        public string Nome { get; set; }                      
        public string NumSerie { get; set; }
        public string NumInventario { get; set; }
        public string NumEquipamento { get; set; }        
        public string Sala { get; set; }
        public int IdRegiao { get; set; }
        public int Modelo { get; set; }
        public int IdCliente { get; set; }
        public int? IdManutencao { get; set; }
        public string Codigo { get; set; }
        public string Versao { get; set; }
        public List<FichaManutencaoRelatorioManutencaoViewModel> PlanMaintenance { get; set; }
        public List<FichaManutencaoTestesQualitativosViewModel> PlanQuality { get; set; }
        public List<FichaManutencaoTestesQuantitativosViewModel> PlanQuantity { get; set; }
        public List<FichaManutencaoEmmEquipamentosViewModel> Emms { get; set; }
        public List<FichaManutencaoRelatorioMaterialViewModel> Materials { get; set; }
        public string Observacao { get; set; }
        public int? EstadoFinal { get; set; }
        public string AssinaturaTecnico { get; set; }
        public string AssinaturaCliente { get; set; }
        public string AssinaturaSie { get; set; }
        public string Rotina { get; set; }
        public DateTime? DataRelatorio { get; set; }
        public Utilizador UtilizadorAssinaturaTecnico { get; set; }
    }

}
