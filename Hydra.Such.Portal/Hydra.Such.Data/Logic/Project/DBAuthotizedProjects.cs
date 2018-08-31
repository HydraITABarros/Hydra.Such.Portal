using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBAuthotizedProjects
    {
        #region Parse Utilities
        public static AuthorizedProjectViewModel ParseToViewModel(this ProjectosAutorizados item)
        {
            if (item != null)
            {
                AuthorizedProjectViewModel proj = new AuthorizedProjectViewModel();

                proj.CodProjeto = item.CodProjeto;
                proj.GrupoFactura = item.GrupoFactura;
                proj.Descricao = item.Descricao;
                proj.CodCliente = item.CodCliente;
                proj.CodRegiao = item.CodRegiao;
                proj.CodAreaFuncional = item.CodAreaFuncional;
                proj.CodCentroResponsabilidade = item.CodCentroResponsabilidade;
                proj.CodContrato = item.CodContrato;
                proj.CodEnderecoEnvio = item.CodEnderecoEnvio;
                proj.GrupoContabilisticoObra = item.GrupoContabilisticoObra;
                proj.GrupoContabilisticoProjeto = item.GrupoContabilisticoProjeto;
                proj.NumSerie = item.NumSerie;
                proj.Utilizador = item.Utilizador;
                proj.DataAutorizacao = item.DataAutorizacao.HasValue ? item.DataAutorizacao.Value.ToString("yyyy-MM-dd") : "";
                proj.DataServPrestado = item.DataServPrestado;
                proj.DataPrestacaoServico = item.DataPrestacaoServico.HasValue ? item.DataPrestacaoServico.Value.ToString("yyyy-MM-dd") : "";
                proj.Observacoes = item.Observacoes;
                proj.Observacoes1 = item.Observacoes1;
                proj.PedidoCliente = item.PedidoCliente;
                proj.Opcao = item.Opcao;
                proj.DataPedido = item.DataPedido;
                proj.DescricaoGrupo = item.DescricaoGrupo;
                proj.CodTermosPagamento = item.CodTermosPagamento;
                proj.Diversos = item.Diversos;
                proj.NumCompromisso = item.NumCompromisso;
                proj.SituacoesPendentes = item.SituacoesPendentes;
                proj.CodMetodoPagamento = item.CodMetodoPagamento;
                proj.Faturado = item.Faturado;
                proj.ValorAutorizado = 0;//item.ValorAutorizado;

                return proj;
            }
            return null;
        }

        public static List<AuthorizedProjectViewModel> ParseToViewModel(this List<ProjectosAutorizados> items)
        {
            List<AuthorizedProjectViewModel> parsedItems = new List<AuthorizedProjectViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static ProjectosAutorizados ParseToDB(this AuthorizedProjectViewModel item)
        {
            if (item != null)
            {
                ProjectosAutorizados proj = new ProjectosAutorizados();

                proj.CodProjeto = item.CodProjeto;
                proj.GrupoFactura = item.GrupoFactura;
                proj.Descricao = item.Descricao;
                proj.CodCliente = item.CodCliente;
                proj.CodRegiao = item.CodRegiao;
                proj.CodAreaFuncional = item.CodAreaFuncional;
                proj.CodCentroResponsabilidade = item.CodCentroResponsabilidade;
                proj.CodContrato = item.CodContrato;
                proj.CodEnderecoEnvio = item.CodEnderecoEnvio;
                proj.GrupoContabilisticoObra = item.GrupoContabilisticoObra;
                proj.GrupoContabilisticoProjeto = item.GrupoContabilisticoProjeto;
                proj.NumSerie = item.NumSerie;
                proj.Utilizador = item.Utilizador;
                proj.DataAutorizacao = string.IsNullOrEmpty(item.DataAutorizacao) ? (DateTime?)null : DateTime.Parse(item.DataAutorizacao);
                proj.DataServPrestado = item.DataServPrestado;
                proj.DataPrestacaoServico = string.IsNullOrEmpty(item.DataPrestacaoServico) ? (DateTime?)null : DateTime.Parse(item.DataPrestacaoServico);
                proj.Observacoes = item.Observacoes;
                proj.Observacoes1 = item.Observacoes1;
                proj.PedidoCliente = item.PedidoCliente;
                proj.Opcao = item.Opcao;
                proj.DataPedido = item.DataPedido;
                proj.DescricaoGrupo = item.DescricaoGrupo;
                proj.CodTermosPagamento = item.CodTermosPagamento;
                proj.Diversos = item.Diversos;
                proj.NumCompromisso = item.NumCompromisso;
                proj.SituacoesPendentes = item.SituacoesPendentes;
                proj.CodMetodoPagamento = item.CodMetodoPagamento;
                proj.Faturado = item.Faturado;

                return proj;
            }
            return null;
        }

        public static List<ProjectosAutorizados> ParseToDB(this List<AuthorizedProjectViewModel> items)
        {
            List<ProjectosAutorizados> parsedItems = new List<ProjectosAutorizados>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
