﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Configurations
{
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

        public static readonly List<EnumData> FolhaDeHoraCodeTypeKms = new List<EnumData>()
        {
            new EnumData()
            {
                Id = 0,
                Value = "Kilometros"
            },
            new EnumData()
            {
                Id = 1,
                Value = "Kilometros (Coimbra)"
            },
            new EnumData()
            {
                Id = 2,
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
    }
    public class EnumData
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
