using Hydra.Such.Data.Evolution.DatabaseReference;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.ViewModels
{

    public class EquipamentoViewModel
    {
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
    }

}
