using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmEquipamentos
    {
        public int Id { get; set; }
        public int NumEmm { get; set; }
        public int IdGrupo { get; set; }
        public int? IdTipo { get; set; }
        public string TipoDescricao { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdMarca { get; set; }
        public int? IdModelo { get; set; }
        public string NumSerie { get; set; }
        public int? IdEquipamento { get; set; }
        public string IdRegiao { get; set; }
        public string IdArea { get; set; }
        public string IdAreaUp { get; set; }
        public string IdCresp { get; set; }
        public string IdSupervisor { get; set; }
        public string IdResponsavel { get; set; }
        public int? IdPeriocidade { get; set; }
        public int? IdEstado { get; set; }
        public DateTime? DataCompra { get; set; }
        public DateTime? DataAbate { get; set; }
        public DateTime? DataUltimaInspecao { get; set; }
        public DateTime? DataProximaInspecao { get; set; }
        public int? IdFornecedor { get; set; }
        public int? IdCalibracao { get; set; }
        public int? IdServico { get; set; }
        public string UsadoProcesso { get; set; }
        public string Informacao { get; set; }
        public string NumPatrimonio { get; set; }
        public bool? Activo { get; set; }
    }
}
