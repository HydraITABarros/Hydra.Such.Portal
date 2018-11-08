using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hydra.Such.Data.ViewModel.GuiaTransporte
{
    public class LinhaGuiaTransporteNavViewModel
    {
        public string NoGuiaTransporte { get; set; }
        public int NoLinha { get; set; }

        public int Tipo { get; set; }
        public string TipoDescription { get; set; }
        public string No { get; set; }
        public string Descricao { get; set; }
        public string CodUnidadeMedida { get; set; }
        public string NoProjecto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeEnviar { get; set; }
        public string RefDocOrigem { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string FunctionalLocationNo { get; set; }

        public int EstadoEquipamento { get; set; }
        public string EstadoEquipamentoDescription { get; set; }
        public string InventoryNo { get; set; }

        public GuiaTransporteNavViewModel ParentNo { get; set; }

    }
}
