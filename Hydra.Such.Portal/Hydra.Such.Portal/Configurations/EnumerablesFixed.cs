using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using static Hydra.Such.Data.Enumerations;

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
        private static List<EnumData> areasEnumData
        {
            get
            {
                List<EnumData> enumData = new List<EnumData>();
                var items = Data.EnumHelper.GetItemsFor(typeof(Data.Enumerations.Areas));
                items.ForEach(x =>
                    enumData.Add(new EnumData(x.Key, x.Value))
                );
                return enumData;
            }
        }

        private static List<EnumData> featuresEnumData
        {
            get
            {
                List<EnumData> enumData = new List<EnumData>();
                var items = Data.EnumHelper.GetItemsFor(typeof(Features));
                items.ForEach(x =>
                    enumData.Add(new EnumData(x.Key, x.Value))
                );
                return enumData;
            }
        }

        public static readonly List<EnumData> Areas = areasEnumData;
        //new List<EnumData>(){
        //    new EnumData()
        //    {
        //        Id = 1,
        //        Value = "Engenharia"
        //    },
        //    new EnumData()
        //    {
        //        Id = 2,
        //        Value = "Ambiente"
        //    },
        //    new EnumData()
        //    {
        //        Id = 3,
        //        Value = "Nutrição"
        //    },
        //    new EnumData()
        //    {
        //        Id = 4,
        //        Value = "Vendas"
        //    },
        //    new EnumData()
        //    {
        //        Id = 5,
        //        Value = "Apoio"
        //    },
        //    new EnumData()
        //    {
        //        Id = 6,
        //        Value = "P&O"
        //    },
        //    new EnumData()
        //    {
        //        Id = 7,
        //        Value = "Novas Áreas"
        //    },
        //    new EnumData()
        //    {
        //        Id = 8,
        //        Value = "Internacional"
        //    },
        //    new EnumData()
        //    {
        //        Id = 9,
        //        Value = "Jurídico"
        //    },
        //    new EnumData()
        //    {
        //        Id = 10,
        //        Value = "Compras"
        //    },
        //    new EnumData()
        //    {
        //        Id = 11,
        //        Value = "Administração"
        //    },

        //    // zpgm. Generic Area to allow features to be applied to all Areas
        //    new EnumData()
        //    {
        //        Id = 99,
        //        Value = "Genérica"
        //    }
        //};

        public static readonly List<EnumData> Features = featuresEnumData;
       //     new List<EnumData>(){
       //     new EnumData()
       //     {
       //         Id = 1,
       //         Value = "Projetos"
       //     },
       //     new EnumData()
       //     {
       //         Id = 2,
       //         Value = "Contratos"
       //     },
       //     new EnumData()
       //     {
       //         Id = 3,
       //         Value = "Pré-Requisições"
       //     },
       //     new EnumData()
       //     {
       //         Id = 4,
       //         Value = "Requisições"
       //     },
       //     new EnumData()
       //     {
       //         Id = 5,
       //         Value = "Requisições Simplificadas"
       //     },
       //     new EnumData()
       //     {
       //         Id = 6,
       //         Value = "Folhas de Horas"
       //     },
       //     new EnumData()
       //     {
       //         Id = 7,
       //         Value = "Unidades Produtivas"
       //     },
       //     new EnumData()
       //     {
       //         Id = 8,
       //         Value = "Fichas Técnicas de Pratos"
       //     },
       //     new EnumData()
       //     {
       //         Id = 9,
       //         Value = "Pedido de Aquisição"
       //     },
       //     new EnumData()
       //     {
       //         Id = 10,
       //         Value = "Pedido Simplificado"
       //     },
       //     new EnumData()
       //     {
       //         Id = 11,
       //         Value = "Viaturas"
       //     },
       //     new EnumData()
       //     {
       //         Id = 12,
       //         Value = "Telemoveis"
       //     },
       //     new EnumData()
       //     {
       //         Id = 13,
       //         Value = "Telefones"
       //     },
       //     new EnumData()
       //     {
       //         Id = 14,
       //         Value = "Processos Disciplinares"
       //     },
       //     new EnumData()
       //     {
       //         Id = 15,
       //         Value = "Processos de Inquérito"
       //     },
       //     new EnumData()
       //     {
       //         Id = 16,
       //         Value = "Receção de Compras"
       //     },
       //     new EnumData()
       //     {
       //         Id = 17,
       //         Value = "Receção de Faturação"
       //     },
       //     new EnumData()
       //     {
       //         Id = 18,
       //         Value = "Administração"
       //     },
       //     new EnumData()
       //     {
       //         Id = 19,
       //         Value = "Diário de Projeto"
       //     },
       //     new EnumData()
       //     {
       //         Id = 20,
       //         Value = "Oportunidades"
       //     },
       //      new EnumData()
       //     {
       //         Id = 21,
       //         Value = "Propostas"
       //     },
       //      new EnumData()
       //     {
       //         Id = 22,
       //         Value = "Autorizaçãp Faturação"
       //     },

        //      // zpgm. identify users that can be appointed Elementos Juri in ProcedimentosCcp
        //      new EnumData()
        //      {
        //          Id = 23,
        //          Value = "Elemento Juri CCP"
        //      },

        //      new EnumData()
        //      {
        //          Id = 24,
        //          Value = "Contactos"
        //      },


        //     #region 1. identify user roles related to CCP
        //new EnumData()
        //      {
        //          Id = 25,
        //          Value = "Checklist - Elemento Pre-Area 0"
        //      },

        //      new EnumData()
        //      {
        //          Id  = 26,
        //          Value = "Checklist - Elemento Pre-Area"
        //      },

        //      new EnumData()
        //      {
        //          Id = 27,
        //          Value = "Checklist - Elemento Compras"
        //      },

        //      new EnumData()
        //      {
        //          Id  = 28,
        //          Value = "Checklist - Elemento Juri"
        //      },

        //      new EnumData()
        //      {
        //          Id = 29,
        //          Value = "Checklist - Elemento Contabilidade"
        //      },

        //      new EnumData()
        //      {
        //          Id = 30,
        //          Value = "Checklist - Elemento Juridico"
        //      },

        //      new EnumData()
        //      {
        //          Id = 31,
        //          Value = "Checklist - Elemento CA"
        //      },

        //      new EnumData()
        //      {
        //          Id = 32,
        //          Value = "Checklist - Gestor Processo"
        //      },

        //      new EnumData()
        //      {
        //          Id = 33,
        //          Value = "Checklist - Secretariado CA"
        //      },

        //      new EnumData()
        //      {
        //          Id = 34,
        //          Value = "Checklist - Fecho de Processo"
        //      },
        //  #endregion

        //      new EnumData()
        //      {
        //          Id = 35,
        //          Value = "Cafetarias/Refeitórios"
        //      },

        //      new EnumData()
        //      {
        //          Id = 36,
        //          Value = "Diário Cafetarias/Refeitórios"
        //      },

        //     #region 2. identify user roles related to CCP
        //      new EnumData
        //      {
        //          Id = 37,
        //          Value = "Checklist - Elemento Area"
        //      },
        //  #endregion

        //      new EnumData()
        //     {
        //         Id = 38,
        //         Value = "Modelos Requisições Simplificadas"
        //     },

        //       new EnumData()
        //     {
        //         Id = 39,
        //         Value = "Localizações"
        //     },
        //       new EnumData()
        //     {
        //         Id = 40,
        //         Value = "Requisições Simplificadas"
        //     },
        //     new EnumData()
        //     {
        //         Id = 41,
        //         Value = "Necessidade de Compras"
        //     },
        //     new EnumData()
        //     {
        //         Id = 42,
        //         Value = "Fichas Tecnicas de Pratos"
        //     },
        //     new EnumData()
        //     {
        //         Id = 43,
        //         Value = "Histórico de Requisições"
        //     },
        //     new EnumData()
        //     {
        //         Id = 44,
        //         Value = "Necessidade de Compras Direta"
        //     }
        // };

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

        public static readonly List<EnumData> project_diaryPrice = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Contrato"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Projeto"
            }
        };

        public static readonly List<EnumData> RequestOrigin = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Pedido do Cliente"
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
                Value = "Concurso Limitado por Prévia Qualificação"
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
            },
            new EnumData()
            {
                Id = 12,
                Value = "Consulta Prévia"
            },
            new EnumData()
            {
                Id = 13,
                Value = "Contratação Excluída"
            },
            new EnumData()
            {
                Id = 14,
                Value = "Consulta Preliminar"
            }
        };

        public static readonly List<EnumData> ProposalsStatus = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Aberta"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Enviada"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Revista"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Perdida"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Cancelada"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Renovada"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Adjudicada"
            },
            new EnumData()
            {
                Id = 8,
                Value = "Parcialmente Aceite"
            },
            new EnumData()
            {
                Id = 9,
                Value = "Oportunidade"
            },
            new EnumData()
            {
                Id = 10,
                Value = "Oportunidade Não Respondida"
            }
        };

        public static readonly List<EnumData> ProposalsFetchUnit = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Nutrição"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Tratamento de Roupa"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Tratamento de Resíduos"
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
                Value = "Segurança e Controlo Técnico"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Projetos e Obras"
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
                Value = "Manutenção, Segurança e Controlo Técnico e Energia"
            },
            new EnumData()
            {
                Id = 11,
                Value = "Manutenção e Energia"
            },
            new EnumData()
            {
                Id = 12,
                Value = "Segurança e Controlo Técnico e Energia"
            },
            new EnumData()
            {
                Id = 13,
                Value = "Tratamento de Roupa e Tratamento de Resíduos"
            },
            new EnumData()
            {
                Id = 13,
                Value = "Tratamento de Resíduos e Limpeza Hospitalar"
            },
            new EnumData()
            {
                Id = 13,
                Value = "Outra"
            },
        };

        /*
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
                Value = "Fechado"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Terminado"
            } 
        };
        */

        public static readonly List<EnumData> ContractType = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Contratos"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Contratos Internos"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Quotas"
            }};

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
        public static readonly List<EnumData> ProjectPreMovements = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 1,
                Value = "Consumo"
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
        public static readonly List<EnumData> requestTypes = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 2,
                Value = "Produto"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Recurso"
            }
        };

        public static readonly List<EnumData> LPlatesTechnicalFiles_Type = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 3,
                Value = "Prato"
            },
            new EnumData()
            {
                Id = 2,
                Value = "LM Produção"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Produto"
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
            },
            new EnumData()
            {
                Id = 2,
                Value = "MP+MC"
            },
            new EnumData()
            {
                Id = 2,
                Value = "MP+(x)% MC"
            }
        };

        public static readonly List<EnumData> InvoiceType = new List<EnumData>(){
            new EnumData()
            {
                Id = 3,
                Value = "Faturas"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Notas de Débito"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Notas de Crédito"
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
                Value = "Validado"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Registado"
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
                Value = "quilómetros"
            },
            new EnumDataString()
            {
                Id = "KMC",
                Value = "quilómetros (Coimbra)"
            },
            new EnumDataString()
            {
                Id = "KMP",
                Value = "quilómetros (Porto)"
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

        public static readonly List<EnumData> ProcedimentosAbertoFechado = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Aberto"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Fechado"
            }
        };

        public static readonly List<EnumData> RequisitionsStatus = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Pendente"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Aprovado"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Registado"
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
            
            //NR 20180403 - Tipo repetido. Comentado e alterado ID dos 2 estados seguintes, DC e CPI
            //new EnumData()
            //{
            //    Id = 5,
            //    Value = "PN"
            //},

            new EnumData()
            {
                Id = 5,
                Value = "DC"
            },

            new EnumData()
            {
                Id = 6,
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

        public static readonly List<EnumData> ViaturasEstado = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Activo"
            },
            new EnumData
            {
                Id = 2,
                Value = "Bloqueado"
            },
            new EnumData
            {
                Id = 3,
                Value = "Abatido"
            },
            new EnumData
            {
                Id = 4,
                Value = "Cedido"
            }
        };

        public static readonly List<EnumData> ViaturasTipoCombustivel = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Gasolina"
            },
            new EnumData
            {
                Id = 2,
                Value = "Gasoleo"
            },
            new EnumData
            {
                Id = 3,
                Value = "Gas"
            },
            new EnumData
            {
                Id = 4,
                Value = "Eletrico"
            },
            new EnumData
            {
                Id = 5,
                Value = "Hibrido"
            }
        };

        public static readonly List<EnumData> ViaturasTipoPropriedade = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "SUCH"
            },
            new EnumData
            {
                Id = 2,
                Value = "Renting"
            },
            new EnumData
            {
                Id = 3,
                Value = "Leasing"
            }
        };

        public static readonly List<EnumData> TipoCartoesEApolices = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Apólices"
            },
            new EnumData
            {
                Id = 2,
                Value = "Cartões"
            }
        };

        public static readonly List<EnumData> AjudaCustoTipoCusto = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "km"
            },
            new EnumData
            {
                Id = 2,
                Value = "Ajuda de Custo"
            },
            new EnumData
            {
                Id = 3,
                Value = "Despesa"
            }
        };

        public static readonly List<EnumData> AjudaCustoRefCusto = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Almoço"
            },
            new EnumData
            {
                Id = 2,
                Value = "Jantar"
            },
            new EnumData
            {
                Id = 1,
                Value = "Estadia"
            },
            new EnumData
            {
                Id = 1,
                Value = "Alomço + Jantar"
            },
            new EnumData
            {
                Id = 3,
                Value = "quilómetros"
            }
        };

        public static readonly List<EnumData> AjudaCustoPartidaChegada = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "="
            },
            new EnumData
            {
                Id = 2,
                Value = ">"
            },
            new EnumData
            {
                Id = 1,
                Value = ">="
            },
            new EnumData
            {
                Id = 1,
                Value = "<"
            },
            new EnumData
            {
                Id = 1,
                Value = "<="
            }
        };

        public static readonly List<EnumData> GetTipoHoraFH = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Contrato"
            },
            new EnumData
            {
                Id = 1,
                Value = "Avulso"
            },
            new EnumData
            {
                Id = 2,
                Value = "Viagem"
            },
            new EnumData
            {
                Id = 3,
                Value = "Interna"
            }
        };

        public static readonly List<EnumData> TipoMovimento = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Receita"
            },
            new EnumData
            {
                Id = 2,
                Value = "Despesa"
            },
             new EnumData
            {
                Id = 3,
                Value = "Depósito"
            }
        };

        public static readonly List<EnumData> ContractTerminationDeadlineNotice = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "30 dias"
            },
            new EnumData
            {
                Id = 2,
                Value = "60 dias"
            },
             new EnumData
            {
                Id = 3,
                Value = "180 dias"
            },
             new EnumData
            {
                Id = 4,
                Value = "1 ano"
            },
             new EnumData
            {
                Id = 5,
                Value = "2 anos"
            },
             new EnumData
            {
                Id = 6,
                Value = "3 anos"
            },
             new EnumData
            {
                Id = 7,
                Value = "5 anos"
            },
             new EnumData
            {
                Id = 8,
                Value = "Outra"
            },
             new EnumData
            {
                Id = 9,
                Value = "Nada Referido"
            }
        };

        public static readonly List<EnumData> ContractTerminationTerms = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Tx Inflação INE"
            },
            new EnumData
            {
                Id = 2,
                Value = "Tx Infl INE e factor act salarial CCT"
            },
             new EnumData
            {
                Id = 3,
                Value = "Factor act salarial CCT"
            },
             new EnumData
            {
                Id = 4,
                Value = "Outra"
            },
             new EnumData
            {
                Id = 5,
                Value = "Nada Referido"
            }
        };

        public static readonly List<EnumData> ContractPaymentTerms = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "3% p/pag 30D"
            },
            new EnumData
            {
                Id = 2,
                Value = "1"
            },
             new EnumData
            {
                Id = 3,
                Value = "5% p/pag 60D"
            },
             new EnumData
            {
                Id = 4,
                Value = "Outra"
            },
             new EnumData
            {
                Id = 5,
                Value = "Nada Referido"
            }
        };

        public static readonly List<EnumDataString> LocalMarketRegions = new List<EnumDataString>()
        {
            new EnumDataString
            {
                Id = "1",
                Value = string.Empty
            },
            new EnumDataString
            {
                Id = "2",
                Value = "Lisboa"
            },
             new EnumDataString
            {
                Id = "3",
                Value = "Porto"
            },
             new EnumDataString
            {
                Id = "4",
                Value = "Coimbra"
            },
        };


        public static readonly List<EnumData> ApprovalTypes = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Requisições"
            },
            new EnumData
            {
                Id = 2,
                Value = "Requisições Simplificadas"
            },
            new EnumData
            {
                Id = 3,
                Value = "Folhas de Horas"
            },
            new EnumData
            {
                Id = 4,
                Value = "Compras Dinheiro"
            },
        };

        public static readonly List<EnumData> CookingTechniqueTypes = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Assado"
            },
            new EnumData
            {
                Id = 2,
                Value = "Cozido"
            },
            new EnumData
            {
                Id = 3,
                Value = "Estufado"
            },
            new EnumData
            {
                Id = 4,
                Value = "Frito"
            },
            new EnumData
            {
                Id = 5,
                Value = "Grelhado"
            },
            new EnumData
            {
                Id = 6,
                Value = "Guisado"
            },
            new EnumData
            {
                Id = 7,
                Value = "Gratinado"
            },
            new EnumData
            {
                Id = 8,
                Value = "Salteado"
            },
            new EnumData
            {
                Id = 9,
                Value = "Misto"
            },
            new EnumData
            {
                Id = 10,
                Value = "Ao natural"
            }
        };

        public static readonly List<EnumData> LinesRecTechnicPlastesType = new List<EnumData>()
        {
            new EnumData
            {
                Id = 1,
                Value = "Prato"
            },
            new EnumData
            {
                Id = 2,
                Value = "Produto"
            }
        };

        public static List<EnumData> RequisitionStatesEnumData
        {
            get
            {
                List<EnumData> enumData = new List<EnumData>();
                var items = Data.EnumHelper.GetItemsFor(typeof(RequisitionStates));
                items.ForEach(x =>
                    enumData.Add(new EnumData(x.Key, x.Value))
                );
                return enumData;
            }
        }



        // NR 20180223 Procedimentos CCP
        public static readonly List<EnumData> TipoLinhasProdutosCCP = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = "-"
            },

            new EnumData()
            {
                Id = 1,
                Value = "Produto"
            }
        };

        // NR 20180228 Procedimentos CCP
        public static readonly List<EnumData> ObjectoDeContratoCCP = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = "-"
            },

            new EnumData()
            {
                Id = 1,
                Value = "Equipamento Informático"
            },

            new EnumData()
            {
                Id = 2,
                Value = "Reparações"
            },

            new EnumData()
            {
                Id = 3,
                Value = "Renovação Contratos"
            },

            new EnumData()
            {
                Id = 4,
                Value = "Matéria Prima Alimentar"
            },

            new EnumData()
            {
                Id = 5,
                Value = "Papel"
            },

            new EnumData()
            {
                Id = 6,
                Value = "Serviços"
            },

            new EnumData()
            {
                Id = 7,
                Value = "Detergentes"
            },

            new EnumData()
            {
                Id = 8,
                Value = "Imobilizado"
            },

            new EnumData()
            {
                Id = 9,
                Value = "Licenças"
            },

            new EnumData()
            {
                Id = 10,
                Value = "Seguros"
            },

            new EnumData()
            {
                Id = 11,
                Value = "Equipamento Proteção"
            },

            new EnumData()
            {
                Id = 12,
                Value = "Sacos e Manga Plástico"
            },

            new EnumData()
            {
                Id = 13,
                Value = "Descartáveis Nutrição"
            },

            new EnumData()
            {
                Id = 14,
                Value = "Telecomunicações"
            },

            new EnumData()
            {
                Id = 15,
                Value = "Combustiveis"
            },

            new EnumData()
            {
                Id = 16,
                Value = "Frota"
            },

            new EnumData()
            {
                Id = 17,
                Value = "Fardamento SUCH"
            },

            new EnumData()
            {
                Id = 18,
                Value = "Roupa Hospitalar"
            },

            new EnumData()
            {
                Id = 19,
                Value = "Transportes"
            },

            new EnumData()
            {
                Id = 20,
                Value = "Obras"
            },

            new EnumData()
            {
                Id = 21,
                Value = "Contentores Plástico"
            }
        };

        //ACORDO DE PREÇOS
        public static readonly List<EnumData> AP_FormaEntrega = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Entrega pelo Fornecedor"
            },
            new EnumData
            {
                Id = 1,
                Value = "Entrega pelo SUCH"
            }
        };

        //ACORDO DE PREÇOS
        public static readonly List<EnumData> AP_TipoPreco = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Fornecedor Principal"
            },
            new EnumData
            {
                Id = 1,
                Value = "Fornecedor Secundário"
            }
        };
        
        //CLIENTE
        public static List<EnumData> Tipo_Cliente
        {
            get
            {
                List<EnumData> enumData = new List<EnumData>();
                var items = Data.EnumHelper.GetItemsFor(typeof(Data.ViewModel.Clients.Tipo_Cliente));
                items.ForEach(x =>
                    enumData.Add(new EnumData(x.Key, x.Value))
                );
                return enumData;
            }
        }

        public static List<EnumData> Natureza_Cliente
        {
            get
            {
                List<EnumData> enumData = new List<EnumData>();
                var items = Data.EnumHelper.GetItemsFor(typeof(Data.ViewModel.Clients.Natureza_Cliente));
                items.ForEach(x =>
                    enumData.Add(new EnumData(x.Key, x.Value))
                );
                return enumData;
            }
        }

        //ANEXOS ERROS
        public static readonly List<EnumData> AE_Origem = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = ""
            },
            new EnumData
            {
                Id = 1,
                Value = "Acordo de Preços"
            },
            new EnumData
            {
                Id = 2,
                Value = "FH Empregado Recursos"
            },
            new EnumData
            {
                Id = 3,
                Value = "FH Preço Venda Recursos"
            }
        };

        //ANEXOS ERROS
        public static readonly List<EnumData> AE_Tipo = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = ""
            },
            new EnumData
            {
                Id = 1,
                Value = "Sucesso"
            },
            new EnumData
            {
                Id = 2,
                Value = "Insucesso"
            }
        };

        //Tipo de Equipamento, nos telemóveis
        public static readonly List<EnumData> Telemoveis_Tipo = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Equipamento"
            },
            new EnumData
            {
                Id = 1,
                Value = "Placa de Rede"
            }
        };

        //Estado do Equipamento, nos telemóveis
        public static readonly List<EnumData> Telemoveis_Estado = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Novo"
            },
            new EnumData
            {
                Id = 1,
                Value = "Usado"
            }
        };

        public static readonly List<EnumData> Telemoveis_Devolvido = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = ""
            },
            new EnumData
            {
                Id = 1,
                Value = "Devolvido"
            },
            new EnumData
            {
                Id = 2,
                Value = "Abate TMN"
            },
            new EnumData
            {
                Id = 3,
                Value = "Vendido"
            },
            new EnumData
            {
                Id = 4,
                Value = "Perdido"
            },
            new EnumData
            {
                Id = 5,
                Value = "Roubado"
            },
            new EnumData
            {
                Id = 6,
                Value = "Empréstimo"
            },
            new EnumData
            {
                Id = 7,
                Value = "Não Devolvido"
            }
        };

        //Tipo de Serviço, nos cartões
        public static readonly List<EnumData> Cartoes_TipoServico = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Voz"
            },
            new EnumData
            {
                Id = 1,
                Value = "Dados"
            }
        };

        //Estado, nos cartões
        public static readonly List<EnumData> Cartoes_Estado = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Activo"
            },
            new EnumData
            {
                Id = 1,
                Value = "Bloqueado"
            },
            new EnumData
            {
                Id = 2,
                Value = "Cancelado"
            },
            new EnumData
            {
                Id = 3,
                Value = "Alteração de Titular"
            },
            new EnumData
            {
                Id = 4,
                Value = "Por Activar"
            }
        };

        //GPRS, nos cartões
        public static readonly List<EnumData> Cartoes_GPRS = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "Activo"
            },
            new EnumData
            {
                Id = 1,
                Value = "Desactivo"
            }
        };

        //Barramento de Voz, nos cartões
        public static readonly List<EnumData> Cartoes_BarramentoVoz = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "OFFNET"
            },
            new EnumData
            {
                Id = 1,
                Value = "Sem Restrições"
            }
        };

        //Tarifarios de Dados, nos cartões
        public static readonly List<EnumData> Cartoes_TarifariosDados = new List<EnumData>()
        {
            new EnumData
            {
                Id = 0,
                Value = "*BLOQUEADO"
            },
            new EnumData
            {
                Id = 1,
                Value = "Banda Larga"
            },
            new EnumData
            {
                Id = 2,
                Value = "Internet no Telemóvel"
            },
            new EnumData
            {
                Id = 3,
                Value = "IPAD"
            }
        };

        public static readonly List<EnumData> ComprasEstado = new List<EnumData>(){
            new EnumData()
            {
                Id = 0,
                Value = ""
            },
            new EnumData()
            {
                Id = 1,
                Value = "Aprovado"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Validado"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Recusado"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Tratado"
            }
        };

        public static readonly List<EnumData> LinhasPreEncomenda_Documento_a_Criar = new List<EnumData>(){
            new EnumData()
            {
                Id = 0,
                Value = "Consulta Mercado"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Encomenda"
            }
        };

        public static readonly List<EnumData> ProdutosTipo = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Alimento"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Prato"
            }
        };

        //Pedidos de Cotação - Destino
        public static readonly List<EnumData> Destino = new List<EnumData>(){
            new EnumData()
            {
                Id = 0,
                Value = ""
            },
            new EnumData()
            {
                Id = 1,
                Value = "Armazém"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Projeto"
            }
        };

        //Pedidos de Cotação - Estado
        public static readonly List<EnumData> Estado = new List<EnumData>(){
            new EnumData()
            {
                Id = 0,
                Value = "Aberto"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Liberto"
            }
        };

        //Pedidos de Cotação - Fase
        public static readonly List<EnumData> Fase = new List<EnumData>(){
            new EnumData()
            {
                Id = 0,
                Value = "Abertura"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Consulta"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Negociação e Contratação"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Adjudicação"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Fecho"
            }
        };

        //Pedidos de Cotação - Modalidade
        public static readonly List<EnumData> Modalidade = new List<EnumData>(){
            new EnumData()
            {
                Id = 0,
                Value = "Consulta Alargada"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Ajuste Direto"
            }
        };

        //Pedidos de Cotação - Estado Resposta
        public static readonly List<EnumData> EstadoResposta = new List<EnumData>(){
            new EnumData()
            {
                Id = 0,
                Value = "Pendente"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Aprovado"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Rejeitado"
            }
        };

        //Pedidos de Cotação - Fase_Entidades
        public static readonly List<EnumData> Fase_Entidades = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "FASE 1 - Pendente Proposta Fornecedor"
            },
            new EnumData()
            {
                Id = 2,
                Value = "FASE 2 - Pendente Esclarecimentos"
            },
            new EnumData()
            {
                Id = 3,
                Value = "FASE 3 - Pendente Aceitação Área"
            },
            new EnumData()
            {
                Id = 4,
                Value = "FASE 4 - Pendente Compras"
            }
        };

        public static readonly List<EnumData> TipoPedidoPagamento = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Transferência Bancária"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Cheque"
            }
        };

        public static readonly List<EnumData> EstadoPedidoPagamento = new List<EnumData>(){
            new EnumData()
            {
                Id = 1,
                Value = "Inicial"
            },
            new EnumData()
            {
                Id = 2,
                Value = "Em Aprovação"
            },
            new EnumData()
            {
                Id = 3,
                Value = "Aprovado"
            },
            new EnumData()
            {
                Id = 4,
                Value = "Validado"
            },
            new EnumData()
            {
                Id = 5,
                Value = "Anulado"
            },
            new EnumData()
            {
                Id = 6,
                Value = "Disponível"
            },
            new EnumData()
            {
                Id = 7,
                Value = "Arquivado"
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
