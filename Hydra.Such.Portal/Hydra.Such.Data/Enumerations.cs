using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Hydra.Such.Data
{
    public class Enumerations
    {
        public enum RequisitionStates
        {
            [Description("Pendente")]
            Pending,
            [Description("Recebido")]
            Received,
            [Description("Tratado")]
            Treated,
            [Description("Validado")]
            Validated,
            [Description("Aprovado")]
            Approved,
            [Description("Disponibilizado")]
            Available,
            [Description("Arquivado")]
            Archived
        }

        public enum Dimensions
        {
            Region = 1,
            FunctionalArea = 2,
            ResponsabilityCenter = 3
        }

        public enum Features
        {
            [Description("Projetos")]
            Projetos = 1,
            [Description("Contratos")]
            Contratos = 2,
            [Description("Pré-Requisições")]
            PréRequisições = 3,
            [Description("Requisições")]
            Requisições = 4,
            [Description("Requisições Simplificadas")]
            RequisiçõesSimplificadas = 5,
            [Description("Folhas de Horas")]
            FolhasHoras = 6,
            [Description("Unidades Produtivas")]
            UnidadesProdutivas = 7,
            [Description("Fichas Técnicas de Pratos")]
            FichasTécnicasPratos = 8,
            [Description("Pedido de Aquisição")]
            PedidoAquisição = 9,
            [Description("Pedido Simplificado")]
            PedidoSimplificado = 10,
            [Description("Viaturas")]
            Viaturas = 11,
            [Description("Telemoveis")]
            Telemoveis = 12,
            [Description("Telefones")]
            Telefones = 13,
            [Description("Processos Disciplinares")]
            ProcessosDisciplinares = 14,
            [Description("Processos de Inquérito")]
            ProcessosInquérito = 15,
            [Description("Receção de Compras")]
            ReceçãoCompras = 16,
            [Description("Receção de Faturação")]
            ReceçãoFaturação = 17,
            [Description("Administração")]
            Administração = 18,
            [Description("Diário de Projeto")]
            DiárioProjeto = 19,
            [Description("Oportunidades")]
            Oportunidades = 20,
            [Description("Propostas")]
            Propostas = 21,
            [Description("Autorizaçãp Faturação")]
            AutorizaçãoFaturação = 22,
            [Description("Elemento Juri CCP")]
            ElementoJuriCCP = 23,
            [Description("Contactos")]
            Contactos = 24,
            [Description("Checklist - Elemento Pre-Area 0")]
            Checklist_ElementoPreArea_0 = 25,
            [Description("Checklist - Elemento Pre-Area")]
            Checklist_ElementoPreArea = 26,
            [Description("Checklist - Elemento Compras")]
            Checklist_ElementoCompras = 27,
            [Description("Checklist - Elemento Juri")]
            Checklist_ElementoJuri = 28,
            [Description("Checklist - Elemento Contabilidade")]
            Checklist_ElementoContabilidade = 29,
            [Description("Checklist - Elemento Juridico")]
            Checklist_ElementoJuridico = 30,
            [Description("Checklist - Elemento CA")]
            Checklist_ElementoCA = 31,
            [Description("Checklist - Gestor Processo")]
            Checklist_GestorProcesso = 32,
            [Description("Checklist - Secretariado CA")]
            Checklist_SecretariadoCA = 33,
            [Description("Checklist - Fecho de Processo")]
            Checklist_FechoProcesso = 34,
            [Description("Cafetarias/Refeitórios")]
            Cafetarias_Refeitórios = 35,
            [Description("Diário Cafetarias/Refeitórios")]
            Diário_Cafetarias_Refeitórios = 36,
            [Description("Checklist - Elemento Area")]
            Checklist_ElementoArea = 37,
            [Description("Modelos Requisições Simplificadas")]
            ModelosRequisiçõesSimplificadas = 38,
            [Description("Localizações")]
            Localizações = 39,
            [Description("Necessidade de Compras")]
            NecessidadeCompras = 41,
            [Description("Histórico de Requisições")]
            HistóricoRequisições = 43,
            [Description("Necessidade de Compras Direta")]
            NecessidadeComprasDireta = 44,
            [Description("Modelos de Requisição")]
            ModelosRequisicao = 45,
            [Description("Registo Nº Refeições")]
            RegistoRefeicoes = 46
        }

        public enum Areas
        {
            [Description("Engenharia")]
            Engenharia = 1,
            [Description("Ambiente")]
            Ambiente = 2,
            [Description("Nutrição")]
            Nutrição = 3,
            [Description("Vendas")]
            Vendas = 4,
            [Description("Apoio")]
            Apoio = 5,
            [Description("P&O")]
            PO = 6,
            [Description("Novas Áreas")]
            NovasÁreas = 7,
            [Description("Internacional")]
            Internacional = 8,
            [Description("Jurídico")]
            Jurídico = 9,
            [Description("Compras")]
            Compras = 10,
            [Description("Administração")]
            Administração = 11,
            [Description("Genérica")]
            Genérica = 99
        }
    }
}
