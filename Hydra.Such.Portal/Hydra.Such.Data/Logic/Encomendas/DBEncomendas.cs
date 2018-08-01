using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Encomendas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Hydra.Such.Data.NAV;

namespace Hydra.Such.Data.Logic.Encomendas
{
    public class DBEncomendas
    {
        #region CRUD
        /// <summary>
        /// Lista de todos os registos (Linhas)
        /// </summary>
        /// <returns></returns>
        public static List<LinhasPreEncomenda> GetAllLinhasPreEncomendaToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasPreEncomenda.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        /// <summary>
        /// Devolve os dados de um registo da tabela
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static LinhasPreEncomenda GetLinhasPreEncomenda(int numLinhaPreEncomenda)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasPreEncomenda.Where(p => p.NºLinhaPreEncomenda == numLinhaPreEncomenda).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static LinhasPreEncomenda Update(LinhasPreEncomenda ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasPreEncomenda.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion


        public static LinhasPreEncomendaView CastLinhasPreEncomendaToView(LinhasPreEncomenda ObjectToTransform)
        {
            string _fornecedor = string.Empty;
            
            LinhasPreEncomendaView view = new LinhasPreEncomendaView()
            {
                CodigoAreaFuncional = ObjectToTransform.CódigoÁreaFuncional,
                CodigoCentroResponsabilidade = ObjectToTransform.CódigoCentroResponsabilidade,
                CodigoLocalizacao = ObjectToTransform.CódigoLocalização,
                CodigoProduto = ObjectToTransform.CódigoProduto,
                CodigoRegiao = ObjectToTransform.CódigoRegião,
                CodigoUnidadeMedida = ObjectToTransform.CódigoUnidadeMedida,
                CustoUnitario = ObjectToTransform.CustoUnitário,
                DataHoraCriacao = ObjectToTransform.DataHoraCriação,
                DataHoraModificacao = ObjectToTransform.DataHoraModificação,
                DescricaoProduto = ObjectToTransform.DescriçãoProduto,
                NumFornecedor = ObjectToTransform.NºFornecedor,
                NumLinhaPreEncomenda = ObjectToTransform.NºLinhaPreEncomenda,
                NumLinhaRequisicao = ObjectToTransform.NºLinhaRequisição,
                NumPreEncomenda = ObjectToTransform.NºPreEncomenda,
                NumProjeto = ObjectToTransform.NºProjeto,
                NumRequisicao = ObjectToTransform.NºRequisição,
                QuantidadeDisponibilizada = ObjectToTransform.QuantidadeDisponibilizada,
                UtilizadorCriacao = ObjectToTransform.UtilizadorCriação,
                UtilizadorModificacao = ObjectToTransform.UtilizadorModificação,
                DocumentoACriar = ObjectToTransform.DocumentoACriar,
                //CriarDocumento = ObjectToTransform.CriarDocumento,
                NumEncomendaAberto = ObjectToTransform.NºEncomendaAberto,
                NumLinhaEncomendaAberto = ObjectToTransform.NºLinhaEncomendaAberto,
               // Tratada = ObjectToTransform.Tratada,
                DataHoraCriacao_Show = ObjectToTransform.DataHoraCriação == null ? "" : ObjectToTransform.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                DataHoraModificacao_Show = ObjectToTransform.DataHoraModificação == null ? "" : ObjectToTransform.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                NomeFornecedor_Show = _fornecedor,
                DocumentoACriar_Show = ObjectToTransform.DocumentoACriar == null ? "" : ObjectToTransform.DocumentoACriar == 0 ? "Consulta Mercado" : "Encomenda"
            };

            return view;
        }

        public static LinhasPreEncomenda CastLinhasPreEncomendaToDB(LinhasPreEncomendaView ObjectToTransform)
        {
            string _fornecedor = string.Empty;

            LinhasPreEncomenda linha = new LinhasPreEncomenda()
            {
                CódigoÁreaFuncional = ObjectToTransform.CodigoAreaFuncional,
                CódigoCentroResponsabilidade = ObjectToTransform.CodigoCentroResponsabilidade,
                CódigoLocalização = ObjectToTransform.CodigoLocalizacao,
                CódigoProduto = ObjectToTransform.CodigoProduto,
                CódigoRegião = ObjectToTransform.CodigoRegiao,
                CódigoUnidadeMedida = ObjectToTransform.CodigoUnidadeMedida,
                CustoUnitário = ObjectToTransform.CustoUnitario,
                DataHoraCriação = ObjectToTransform.DataHoraCriacao,
                DataHoraModificação = ObjectToTransform.DataHoraModificacao,
                DescriçãoProduto = ObjectToTransform.DescricaoProduto,
                NºFornecedor = ObjectToTransform.NumFornecedor,
                NºLinhaPreEncomenda = ObjectToTransform.NumLinhaPreEncomenda,
                NºLinhaRequisição = ObjectToTransform.NumLinhaRequisicao,
                NºPreEncomenda = ObjectToTransform.NumPreEncomenda,
                NºProjeto = ObjectToTransform.NumProjeto,
                NºRequisição = ObjectToTransform.NumRequisicao,
                QuantidadeDisponibilizada = ObjectToTransform.QuantidadeDisponibilizada,
                UtilizadorCriação = ObjectToTransform.UtilizadorCriacao,
                UtilizadorModificação = ObjectToTransform.UtilizadorModificacao,
                DocumentoACriar = ObjectToTransform.DocumentoACriar,
                CriarDocumento = ObjectToTransform.CriarDocumento,
                NºEncomendaAberto = ObjectToTransform.NumEncomendaAberto,
                NºLinhaEncomendaAberto = ObjectToTransform.NumLinhaEncomendaAberto,
                Tratada = ObjectToTransform.Tratada
            };

            return linha;
        }

    }
}
