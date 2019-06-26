using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    //[JsonSchemaProcessor(typeof(ReactSchemaProcessor))]
 
    [ModelMetadataType(typeof(IEquipamento))]
    //[Select(SelectType = SelectExpandType.Automatic)]
    //[Select("CodigoBarras", SelectType = SelectExpandType.Disabled)]
    //[Select("CodigoBarrasCliente", SelectType = SelectExpandType.Disabled)]
    //[Select("Foto", SelectType = SelectExpandType.Disabled)]
    public partial class Equipamento
    {
        [NotMapped]
        public string CategoriaText { get; set; }

        [NotMapped]
        public string MarcaText { get; set; }

        [NotMapped]
        public string ServicoText { get; set; }

        [NotMapped]
        public string Estado { get; set; }
    }

    
    public interface IEquipamento
    {
        
        int IdEquipamento { get; set; }

        string Nome { get; set; }

        int Marca { get; set; }

        int Modelo { get; set; }

        int Categoria { get; set; }


        string NumSerie { get; set; }

        string NumInventario { get; set; }

        string NumEquipamento { get; set; }

        string Observacao { get; set; }
        string IdFornecedor { get; set; }
        string NomeFornecedor { get; set; }

        int IdCliente { get; set; }

        int IdServico { get; set; }

        bool? Activo { get; set; }
        DateTime? DataAquisicao { get; set; }
        DateTime? DataInstalacao { get; set; }
        int? AnoFabrico { get; set; }
        int? IdRegiao { get; set; }
        int? IdArea { get; set; }

        int? IdEquipa { get; set; }
        int? IdAreaOp { get; set; }

        string IdContrato { get; set; }

        byte[] CodigoBarras { get; set; }

        byte[] CodigoBarrasCliente { get; set; }

        decimal? PrecoAquisicao { get; set; }
        string Localizacao { get; set; }
        int? IdPeriodicidade { get; set; }
        bool AssociadoContrato { get; set; }
        int InseridoPor { get; set; }
        DateTime DataInsercao { get; set; }
        bool? PorValidar { get; set; }
        int? ValidadoPor { get; set; }
        DateTime? DataValidacao { get; set; }
        DateTime? DataEntradaContrato { get; set; }
        DateTime? DataSaidaContrato { get; set; }
        string DesignacaoComplementar { get; set; }

        byte[] Foto { get; set; }

        int? AlteradoPor { get; set; }
        DateTime? DataAlteracao { get; set; }
        int ToleranciaEquipamento { get; set; }
        bool? Garantia { get; set; }
        DateTime? DataFimGarantia { get; set; }
        bool? IncluiMc { get; set; }
        int? MpPlaneadas { get; set; }
        DateTime? DataInactivacao { get; set; }
        bool Instalacao { get; set; }
        int? Criticidade { get; set; }
        bool? Abatido { get; set; }
        string Sala { get; set; }
        string DesignacaoComplementar2 { get; set; }
        bool? SubContratar { get; set; }

        EquipCategoria CategoriaNavigation { get; set; }
        Area IdAreaNavigation { get; set; }
        AreaOp IdAreaOpNavigation { get; set; }
        Cliente IdClienteNavigation { get; set; }
        Contrato IdContratoNavigation { get; set; }
        Equipa IdEquipaNavigation { get; set; }
        Fornecedor IdFornecedorNavigation { get; set; }
        Regiao IdRegiaoNavigation { get; set; }
        Servico IdServicoNavigation { get; set; }
        EquipMarca MarcaNavigation { get; set; }
        EquipModelo ModeloNavigation { get; set; }
        ICollection<EquipDadosTecnicos> EquipDadosTecnicos { get; set; }
        ICollection<EquipDependente> EquipDependenteIdEquipPrincipalNavigation { get; set; }
        ICollection<EquipDependente> EquipDependenteIdEquipSecundarioNavigation { get; set; }
        ICollection<EquipPimp> EquipPimp { get; set; }
        ICollection<EquipamentoAcessorio> EquipamentoAcessorio { get; set; }
        ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
        ICollection<OrdemManutencaoLinha> OrdemManutencaoLinha { get; set; }
        ICollection<SolicitacoesLinha> SolicitacoesLinha { get; set; }

    }

}
