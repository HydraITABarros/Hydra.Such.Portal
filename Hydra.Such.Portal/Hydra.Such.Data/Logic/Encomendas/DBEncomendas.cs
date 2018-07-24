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
        #endregion


        public static LinhasPreEncomendaView CastLinhasPreEncomendaToView(LinhasPreEncomenda ObjectToTransform)
        {
            string _fornecedor = string.Empty;

            if (ObjectToTransform != null)
            {
                SuchDBContext _context = new SuchDBContext();
                try
                {

                    _fornecedor = ObjectToTransform.NºFornecedor == null ? "" DBNAV2017Supplier.GetAll(_config.NAVDatabaseName, _config.NAVCompanyName, Compras.NoFornecedor).FirstOrDefault().Name,


                }
                catch
                {

                }
            }

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
                DataHoraCriacao_Show = ObjectToTransform.DataHoraCriação == null ? "" : ObjectToTransform.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                DataHoraModificacao_Show = ObjectToTransform.DataHoraModificação == null ? "" : ObjectToTransform.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                NomeFornecedor_Show = _fornecedor
            };

            return view;
        }

    }
}
