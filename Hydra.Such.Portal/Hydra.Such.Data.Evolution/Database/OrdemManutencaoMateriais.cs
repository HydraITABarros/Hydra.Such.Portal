using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class OrdemManutencaoMateriais
    {
        public int IdOmMaterias { get; set; }
        public int IdOm { get; set; }
        public int IdMaterial { get; set; }
        public string ReferenciaMaterial { get; set; }
        public int IdFornecedor { get; set; }
        public int QtdMaterial { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
        public int IdEstadoMaterial { get; set; }
        public DateTime? DataAplicacao { get; set; }
        public int? Ano { get; set; }
        public int? Semestre { get; set; }
        public int? Trimestre { get; set; }
        public int? Mes { get; set; }
        public int? Dia { get; set; }

        public OrdemManutencaoEstadoMaterial IdEstadoMaterialNavigation { get; set; }
        public OrdemManutencao IdOmNavigation { get; set; }
    }
}
