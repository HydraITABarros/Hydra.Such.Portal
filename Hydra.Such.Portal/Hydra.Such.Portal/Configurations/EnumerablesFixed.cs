using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Configurations
{
    public class EnumData
    {
        public EnumData()
        { }

        public EnumData(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class EnumerablesFixed
    {
        public static readonly List<EnumData> Areas = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Engenharia"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Ambiente"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Nutrição"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Vendas"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Apoio"
            },
            new EnumData()
            {
                Id = 6,
                Value = "P&O"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Novas Áreas"
            },
            new EnumData()
            {
                Id = 8,
                Value = "Internacionalizações"
            },
            new EnumData()
            {
                Id = 9,
                Value = "Jurídico"
            },
            new EnumData()
            {
                Id = 10,
                Value = "Compras"
            },
            new EnumData()
            {
                Id = 11,
                Value = "Administração"
            },

            // zpgm. Generic Area to allow features to be applied to all Areas
            new EnumData()
            {
                Id = 99,
                Value = "Genérica"
            }
        };

        public static readonly List<EnumData> Features = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Projetos"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Contratos"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Pré-Requisições"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Requisições"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Requisições Simplificadas"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Folhas de Horas"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Unidades Produtivas"
            },
            new EnumData()
            {
                Id = 8,
                Value = "Fichas Técnicas de Pratos"
            },
            new EnumData()
            {
                Id = 9,
                Value = "Pedido de Aquisição"
            },
            new EnumData()
            {
                Id = 10,
                Value = "Pedido Simplificado"
            },
            new EnumData()
            {
                Id = 11,
                Value = "Viaturas"
            },
            new EnumData()
            {
                Id = 12,
                Value = "Telemoveis"
            },
            new EnumData()
            {
                Id = 13,
                Value = "Telefones"
            },
            new EnumData()
            {
                Id = 14,
                Value = "Processos Disciplinares"
            },
            new EnumData()
            {
                Id = 15,
                Value = "Processos de Inquérito"
            },
            new EnumData()
            {
                Id = 16,
                Value = "Receção de Compras"
            },
            new EnumData()
            {
                Id = 17,
                Value = "Receção de Faturação"
            },
            new EnumData()
            {
                Id = 18,
                Value = "Administração"
            },
            new EnumData()
            {
                Id = 19,
                Value = "Diário de Projeto"
            },
            new EnumData()
            {
                Id = 20,
                Value = "Oportunidades"
            },
             new EnumData()
            {
                Id = 21,
                Value = "Propostas"
            },
             new EnumData()
            {
                Id = 22,
                Value = "Autorizaçãp Faturação"
            },

             // zpgm. identify users that can be appointed Elementos Juri in ProcedimentosCcp
             new EnumData()
             {
                 Id = 23,
                 Value = "Elemento Juri CCP"
             }
        };

        public static readonly List<EnumData> ProposalStatus = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Orçamento"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Proposta"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Encomenda"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Terminado"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Fechado"
            }
        };

        public static readonly List<EnumData> RequestOrigin = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Pedidio do Cliente"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Ajuste Direto"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Concurso Público"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Concurso Limitado por Previa Qualificação"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Procedimento de Negociação"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Diálogo Concorrencial"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Consulta ao Mercado"
            },
            new EnumData()
            {
                Id = 8,
                Value = "Renovação de Contrato"
            },
            new EnumData()
            {
                Id = 9,
                Value = "Iniciativa SUCH"
            },
            new EnumData()
            {
                Id = 10,
                Value = "Adenda"
            },
            new EnumData()
            {
                Id = 11,
                Value = "Outros"
            }
        };

        public static readonly List<EnumData> ProposalsStatus = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Enviada"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Revista"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Perdida"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Cancelada"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Renovada"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Adjudicada"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Parcialmente Aceite, "
            },
            new EnumData()
            {
                Id = 8,
                Value = "Oportunidade"
            },
            new EnumData()
            {
                Id = 9,
                Value = "Oportunidade Não Respondida"
            }
        };

        public static readonly List<EnumData> ProjectStatus = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Orçamento"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Proposta"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Encomenda"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Terminado"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Fechado"
            } };

        public static readonly List<EnumData> ProjectCategories = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Contrato"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Serviço Ocasional"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Comum"
            }};

        public static readonly List<EnumData> ProjectDiaryMovements = new List<EnumData>()
        {
             new EnumData()
            {
                Id = 1,
                Value = "Consumo"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Venda"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Devolução"
            }
        };

        public static readonly List<EnumData> ProjectDiaryTypes = new List<EnumData>()
        {
             new EnumData()
            {
                Id = 1,
                Value = "Recurso"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Produto"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Conta CG"
            }
        };

        public static readonly List<EnumData> FeeUnits = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Nutrição"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Tratamento de roupa"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Tratamento de resíduos"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Limpeza Hospitalar"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Manutenção"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Segurança e controlo Técnico"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Projeto e Obras"
            },
            new EnumData()
            {
                Id = 8,
                Value = "Energia"
            },
            new EnumData()
            {
                Id = 9,
                Value = "Manutenção e Segurança e Controlo Técnico"
            },
            new EnumData()
            {
                Id = 10,
                Value = "Manutenção"
            },
            new EnumData()
            {
                Id = 11,
                Value = "Segurança e Controlo Técnico e Energia"
            },
            new EnumData()
            {
                Id = 12,
                Value = "Manutenção e Energia"
            },
            new EnumData()
            {
                Id = 13,
                Value = "Segurança e Controlo Técnico e Energia"
            },
            new EnumData()
            {
                Id = 14,
                Value = "Tratamento de Roupa e Tratamento de Resíduos"
            },
            new EnumData()
            {
                Id = 15,
                Value = "Tratamento de Resíduos e Limpeza Hospitalar"
            },
            new EnumData()
            {
                Id = 16,
                Value = "Outra"
            }};

        public static readonly List<EnumData> ContractStatus = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "A enviar"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Enviado"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Pendente"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Assinado"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Cancelado"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Perdido"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Em vigor"
            }
        };

        public static readonly List<EnumData> ContractChangeStatus = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Aberto"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Bloqueado"
            }
        };

        public static readonly List<EnumData> ContractBillingTypes = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Mensal"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Consumo"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Fase"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Mensal+Consumo"
            }
        };

        public static readonly List<EnumData> ContractMaintenanceTypes = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "MP"
            }
        };

        public static readonly List<EnumData> ContractInvoicePeriods = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Mensal"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Bimensal"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Trimestral"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Semestral"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Anual"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Nenhum"
            }
        };

        public static readonly List<EnumData> ContractInvoiceGroups = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "1"
            },
            new EnumData()
            {
                Id = 2,
                Value = "2"
            },
            new EnumData()
            {
                Id = 3,
                Value = "3"
            },
            new EnumData()
            {
                Id = 4,
                Value = "4"
            },
            new EnumData()
            {
                Id = 5,
                Value = "5"
            },
            new EnumData()
            {
                Id = 6,
                Value = "6"
            },
            new EnumData()
            {
                Id = 7,
                Value = "7"
            },
            new EnumData()
            {
                Id = 8,
                Value = "8"
            },
            new EnumData()
            {
                Id = 9,
                Value = "9"
            },
            new EnumData()
            {
                Id = 10,
                Value = "10"
            },
            new EnumData()
            {
                Id = 11,
                Value = "11"
            },
            new EnumData()
            {
                Id = 12,
                Value = "12"
            },
            new EnumData()
            {
                Id = 13,
                Value = "13"
            },
            new EnumData()
            {
                Id = 14,
                Value = "14"
            },
            new EnumData()
            {
                Id = 15,
                Value = "15"
            },
            new EnumData()
            {
                Id = 16,
                Value = "16"
            },
            new EnumData()
            {
                Id = 17,
                Value = "17"
            },
            new EnumData()
            {
                Id = 18,
                Value = "18"
            },
            new EnumData()
            {
                Id = 19,
                Value = "19"
            },
            new EnumData()
            {
                Id = 20,
                Value = "20"
            }
        };

        public static readonly List<EnumData> ContractLineTypes = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Produto"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Recurso"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Conta CG"
            }
        };


        public static readonly List<EnumData> ContabGroupTypesOM_Type = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Estado Operacional"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Disponibilidade do Sistema"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Tipo de Ordem"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Efeito"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Razão Falha"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Motivo Ordem Pendente"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Tipo Plano"
            }
        };

        public static readonly List<EnumData> ContabGroupTypesOM_FailType = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Todos"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Solicitação"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Nota de Avaria"
            }
        };

        public static readonly List<EnumData> FolhaDeHoraStatus = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = "Criado"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Registado"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Validado"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Invalidado"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Validado RH"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Validado S/RH"
            }
        };

        public static readonly List<EnumData> FolhaDeHoraTypeDeslocation = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = ""
            },
            new EnumData()
            {
                Id = 1,
                Value = "Viatura SUCH"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Viatura Própria"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Outros Meios Transporte"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Sem Deslocação"
            }
        };

        public static readonly List<EnumDataString> FolhaDeHoraCodeTypeKms = new List<EnumDataString>()
        {
            new EnumDataString()
            {
                Id = "KM",
                Value = "Kilometros"
            },
            new EnumDataString()
            {
                Id = "KMC",
                Value = "Kilometros (Coimbra)"
            },
            new EnumDataString()
            {
                Id = "KMP",
                Value = "Kilometros (Porto)"
            }
        };

        public static readonly List<EnumData> FolhaDeHoraDisplacementOutsideCity = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = "Não"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Sim"
            }
        };

        public static readonly List<EnumData> FolhaDeHoraAjudaTipoCusto = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 2,
                Value = "Ajuda de Custo"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Despesa"
            }
        };

        // zpgm.< Enumerable types for Procedimentos CCP
        public static readonly List<EnumData> ProcedimentosCcpProcedimentoType = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 1,
                Value = "Procedimento Aquisição"
            },

            new EnumData()
            {
                Id = 2,
                Value = "Procedimento Simplificado"
            }
        };

        public static readonly List<EnumData> ProcedimentosCcpType = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = ""
            },

            new EnumData()
            {
                Id = 1,
                Value = "AD"
            },

            new EnumData()
            {
                Id = 2,
                Value = "CP"
            },

            new EnumData()
            {
                Id = 3,
                Value = "CLPQ"
            },

            new EnumData()
            {
                Id = 4,
                Value = "PN"
            },

            new EnumData()
            {
                Id = 5,
                Value = "PN"
            },

            new EnumData()
            {
                Id = 6,
                Value = "DC"
            },

            new EnumData()
            {
                Id = 7,
                Value = "CPI"
            }
        };

        public static readonly List<EnumData> ProcedimentosCcpStates = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = "0 - Inicial"
            },

            new EnumData()
            {
                Id = 1,
                Value = "1 - Cabimento"
            },

            new EnumData()
            {
                Id = 2,
                Value = "2 - Avaliação Imobilizado"
            },

            new EnumData()
            {
                Id = 3,
                Value = "3 - Autorização CA Imobilizado"
            },

            new EnumData()
            {
                Id = 4,
                Value = "4 - Fundamento Processual"
            },

            new EnumData()
            {
                Id = 5,
                Value = "5 - Fundamento Financeiro"
            },

            new EnumData()
            {
                Id = 6,
                Value = "6 - Fundamento Jurídico"
            },

            new EnumData()
            {
                Id = 7,
                Value = "7 - Pedido Aberto"
            },

            new EnumData()
            {
                Id = 8,
                Value = "8 - Autorização em Aberto"
            },

            new EnumData()
            {
                Id = 9,
                Value = "9 - Publicação Plataforma"
            },

            new EnumData()
            {
                Id = 10,
                Value = "10 - Recolha Proposta"
            },

            new EnumData()
            {
                Id = 11,
                Value = "11 - Relatório Preliminar Júri"
            },

            new EnumData()
            {
                Id = 12,
                Value = "12 - Audição Prévia"
            },

            new EnumData()
            {
                Id = 13,
                Value = "13 - Relatório Final"
            },

            new EnumData()
            {
                Id = 14,
                Value = "14 - Avaliação Jurídico Contrato"
            },

            new EnumData()
            {
                Id = 15,
                Value = "15 - Valor Adjudicado"
            },

            new EnumData()
            {
                Id = 16,
                Value = "16 - Pedido Adjudicado"
            },

            new EnumData()
            {
                Id = 17,
                Value = "17 - Autorização Adjudicação"
            },

            new EnumData()
            {
                Id = 18,
                Value = "18 - Adjudicação Fornecedor"
            },

            new EnumData()
            {
                Id = 19,
                Value = "19 - Final Processo"
            },

            new EnumData()
            {
                Id = 20,
                Value = "20 - Fechado"
            }
        };

        public static readonly List<EnumBoolValues> BoolValues = new List<EnumBoolValues>()
        {
            new EnumBoolValues
            {
                BooleanValue = true,
                StringValue = "Sim"
            },

            new EnumBoolValues
            {
                BooleanValue = false,
                StringValue = "Não"
            },

            new EnumBoolValues
            {
                BooleanValue = null,
                StringValue =""
            }
        };
        // zpgm.>

        public static readonly List<EnumData> Dimension = new List<EnumData>() {
            new EnumData(1, "Região"),
            new EnumData(2, "Área Funcional"),
            new EnumData(3, "Centro Responsabilidade")
        };

        public static readonly List<EnumData> NutritionCoffeShopTypes = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Cafetaria"
            },
            new EnumData
            {
                Id = 2,
                Value = "Refeitório"
            }
        };




    }

    public class EnumDataString
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public class EnumBoolValues
    {
        public bool? BooleanValue { get; set; }
        public string StringValue { get; set; }
    }
}
