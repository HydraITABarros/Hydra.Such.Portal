using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipamentoBdProdNorte2018jun07
    {
        public int IdEquipamento { get; set; }
        public string Nome { get; set; }
        public int Marca { get; set; }
        public int Modelo { get; set; }
        public int Categoria { get; set; }
        public string NumSerie { get; set; }
        public string NumInventario { get; set; }
        public string NumEquipamento { get; set; }
        public string Observacao { get; set; }
        public string IdFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public int IdCliente { get; set; }
        public int IdServico { get; set; }
        public bool? Activo { get; set; }
        public DateTime? DataAquisicao { get; set; }
        public DateTime? DataInstalacao { get; set; }
        public int? AnoFabrico { get; set; }
        public int? IdRegiao { get; set; }
        public int? IdArea { get; set; }
        public int? IdEquipa { get; set; }
        public int? IdAreaOp { get; set; }
        public string IdContrato { get; set; }
        public byte[] CodigoBarras { get; set; }
        public byte[] CodigoBarrasCliente { get; set; }
        public decimal? PrecoAquisicao { get; set; }
        public string Localizacao { get; set; }
        public int? IdPeriodicidade { get; set; }
        public bool AssociadoContrato { get; set; }
        public int InseridoPor { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool? PorValidar { get; set; }
        public int? ValidadoPor { get; set; }
        public DateTime? DataValidacao { get; set; }
        public DateTime? DataEntradaContrato { get; set; }
        public DateTime? DataSaidaContrato { get; set; }
        public string DesignacaoComplementar { get; set; }
        public byte[] Foto { get; set; }
        public int? AlteradoPor { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int ToleranciaEquipamento { get; set; }
        public bool? Garantia { get; set; }
        public DateTime? DataFimGarantia { get; set; }
        public bool? IncluiMc { get; set; }
        public int? MpPlaneadas { get; set; }
        public DateTime? DataInactivacao { get; set; }
        public bool Instalacao { get; set; }
        public int? Criticidade { get; set; }
        public bool? Abatido { get; set; }
        public string Sala { get; set; }
        public string DesignacaoComplementar2 { get; set; }
        public bool? SubContratar { get; set; }
        public string NomeCategoria { get; set; }
        public string NomeMarca { get; set; }
        public string NomeModelo { get; set; }
        public string NomeServico { get; set; }
        public string DescricaoTreePath { get; set; }
        public string NomeEquipa { get; set; }
        public string NomeAreaOp { get; set; }
    }
}
