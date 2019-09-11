using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Hydra.Such.Data
{
    public class Enumerations
    {
        public enum RequisitionTypes
        {
            [Description("Normal")]
            Normal = 0,
            [Description("Compras Dinheiro")]
            ComprasDinheiro = 1
        }

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
            [Description("Rejeitado")]
            Rejected,
            [Description("Disponibilizado")]
            Available,
            [Description("Arquivado")]
            Archived,

            //Criação de 2 Novos Estados para o Workflow
            [Description("Consulta Mercado")]
            Consulta,
            [Description("Encomenda")]
            Encomenda
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
            [Description("Telemóveis")]
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
            [Description("Autorização Faturação")]
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
            [Description("Existências")]
            Existencias = 39,
            [Description("Necessidade de Compras")]
            NecessidadeCompras = 41,
            [Description("Histórico de Requisições")]
            HistóricoRequisições = 43,
            [Description("Necessidade de Compras Direta")]
            NecessidadeComprasDireta = 44,
            [Description("Modelos de Requisição")]
            ModelosRequisicao = 45,
            [Description("Registo Nº Refeições")]
            RegistoRefeicoes = 46,
            [Description("Clientes")]
            Clientes = 47,
            [Description("Pré-Registos")]
            PreRegistos = 48,
            [Description("Preços Serviços Cliente")]
            PreçoServCliente = 49,
            [Description("Mercado Local")]
            MercadoLocal = 50,
            [Description("Pré-Compra")]
            PréEncomendas = 51,
            [Description("Localizações")]
            Localizações = 52,
            [Description("Pedido Cotação")]
            PedidoCotacao = 53,
            [Description("Linhas de Acordos de Preços")]
            LinhasAcordosPrecos = 54,
            [Description("Requisições por Dimensões")]
            RequisicoesPorDimensoes = 55,
            [Description("Pré-Requisições Compras a Dinheiro")]
            PréRequisiçõesComprasDinheiro = 56,
            // zpgm. impressão guias transporte do NAV
            [Description("Impressão Guias Transporte NAV")]
            ImpressaoGuiaTransporteNAV = 57,
            [Description("Requisições Compras a Dinheiro")]
            RequisiçõesComprasDinheiro = 58,
            [Description("Faturação de Projetos")]
            FaturacaoDeProjetos = 59,
            [Description("Pedidos de Pagamento")]
            PedidosPagamento = 60,
            [Description("Contratos Internos")]
            ContratosInternos = 61,
            [Description("Contratos Quotas")]
            ContratosQuotas = 62,
            [Description("Lista Compras a Dinheiro")]
            ListaComprasDinheiro = 63,
            [Description("Contratos Avença Fixa")]
            ContratosAvencaFixa = 64,
            [Description("Folha Horas Report Aj Custo RH")]
            FHReportAjCustoRH = 65,
            [Description("Contrato Criar Fatura")]
            ContratosCriarFatura = 66,
            [Description("Linhas de Contratos")]
            LinhasContratos = 67,
            [Description("Ordens de Manutenção")]
            MaintenanceOrders = 68,
            [Description("Linhas de Contratos Internos")]
            LinhasContratosInternos = 69,
            [Description("Contabilidade Mapas")]
            ContabilidadeMapas = 70,
            [Description("Orçamentos")]
            Orcamentos = 71,



            [Description("Administração Geral")]
            AdminGeral = 100,
            [Description("Administração Aprovações")]
            AdminAprovacoes = 101,
            [Description("Administração Projetos")]
            AdminProjetos = 102,
            [Description("Administração Vendas")]
            AdminVendas = 103,
            [Description("Administração Nutrição")]
            AdminNutricao = 104,
            [Description("Administração Requisições")]
            AdminRequisicoes = 105,
            [Description("Administração Folha de Horas")]
            AdminFolhaHoras = 106,
            [Description("Administração Viaturas e Telemóveis")]
            AdminViaturasTelemoveis = 107,
            [Description("Administração Existências")]
            AdminExistencias = 108,
            [Description("Administração Receção Faturação")]
            AdminReceçãoFaturação = 109,
            [Description("Taxa Resíduos")]
            AdminTaxaResiduos = 110,
            [Description("Administração Acordos de Preços")]
            AdminAcordosPrecos = 111,
            [Description("Encomendas")]
            Encomendas = 112,
            [Description("Administração Serviços")]
            AdminServicos = 113,
            [Description("Administração Pedidos Desenvolvimento")]
            AdminPedidosDEV = 114,
            [Description("Administração Config Linhas Enc Fornecedor")]
            AdminConfigLinhasEncFornecedor = 115,
            [Description("Administração Contactos")]
            AdminContactos = 116,
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

        public enum BillingReceptionStates
        {
            [Description("Receção / Conferência")]
            Rececao,
            [Description("Pendente")]
            Pendente,
            [Description("Contabilizado")]
            Contabilizado,
            [Description("Devolvido")]
            Devolvido,
            [Description("Sem Efeito")]
            SemEfeito,
            [Description("Resolvido")]
            Resolvido
        }

        public enum BillingDocumentTypes
        {
            Fatura = NAVBaseDocumentTypes.Fatura,
            [Description("Nota de Crédito")]
            NotaCredito = NAVBaseDocumentTypes.NotaCredito
        }

        public enum NAVBaseDocumentTypes
        {
            Proposta,
            Encomenda,
            Fatura,
            [Description("Nota de Crédito")]
            NotaCredito,
            [Description("Encomenda Aberta")]
            EncomendaAberta,
            [Description("Devolução")]
            Devolucao
        }

        public enum BillingReceptionAreas
        {
            Contabilidade,
            Aprovisionamento,
            [Description("Unidades Produtivas")]
            UnidadesProdutivas,
            [Description("Unidades de Apoio e Suporte")]
            UnidadesApoioESuporte,
        }

        public enum BillingReceptionUserProfiles
        {
            Perfil,
            Utilizador,
            Tudo
        }

        public enum ContractLineTypes
        {
            [Description("")]
            NaoDefinido,
            Recurso,
            Produto,
            [Description("Conta CG")]
            ContaCG,
        }

        public enum ProjectDiaryTypes
        {
            [Description("")]
            NaoDefinido,
            Produto,
            Recurso,            
            [Description("Conta CG")]
            ContaCG,
        }

        public enum ProjectDiaryMovementTypes
        {
            [Description("")]
            NaoDefinido,
            Consumo,
            Venda,
            [Description("Devolução")]
            Devolucao
        }

        public enum ResourceTypes
        {
            [Description("Mão-de-Obra")]
            MaoDeObra,
            Custo,
            [Description("Serviço Externo")]
            ServicoExterno,
            Ferramenta,
            Invoicing
        }

        public enum EstadoProjecto
        {
            [Description("Pendente")]
            Pendente = 0,
            [Description("Encomenda")]
            Encomenda = 1,
            [Description("Terminado")]
            Terminado = 2  
        }
    }
}
