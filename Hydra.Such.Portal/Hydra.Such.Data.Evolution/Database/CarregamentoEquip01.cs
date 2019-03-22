using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class CarregamentoEquip01
    {
        public int Id { get; set; }
        public string Equipamento { get; set; }
        public string IdEquip { get; set; }
        public string Contrato { get; set; }
        public string DescContrato { get; set; }
        public string Cliente { get; set; }
        public string DescCliente { get; set; }
        public string Instituicao { get; set; }
        public string Localizacao { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public string NumeroInventario { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public string CodFichaManutencao { get; set; }
        public string DescFichaManutencao { get; set; }
        public string Regiao { get; set; }
        public string Area { get; set; }
        public string Equipa { get; set; }
        public string Cresp { get; set; }
        public int? IdTipo { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdCliente { get; set; }
        public int? IdInstituicao { get; set; }
        public int? IdServico { get; set; }
        public int? IdRegiao { get; set; }
        public int? IdArea { get; set; }
        public int? IdEquipa { get; set; }
        public int? IdAreaop { get; set; }
        public int? IdMarca { get; set; }
        public int? IdModelo { get; set; }
        public int? IdModelos { get; set; }
        public int? IdFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public int? IdPeriodicidade { get; set; }
    }
}
