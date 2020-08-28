using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.PedidoCotacao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Hydra.Such.Data.Logic.PedidoCotacao
{
    public class DBSeleccaoEntidades
    {
        public static List<SeleccaoEntidadesView> GetConsultasPorFornecedor(int Ano, string CodFornecedor, bool Historico)
        {
            try
            {
                List<SeleccaoEntidadesView> result = new List<SeleccaoEntidadesView>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@Ano", Ano),
                        new SqlParameter("@CodFornecedor", CodFornecedor),
                        new SqlParameter("@Historico", Historico)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec eSUCHConsultasPorFornecedor @Ano, @CodFornecedor, @Historico", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new SeleccaoEntidadesView()
                        {
                            IdSeleccaoEntidades = (int)temp.IdSeleccaoEntidades,
                            NumConsultaMercado = temp.NumConsultaMercado.Equals(DBNull.Value) ? "" : (string)temp.NumConsultaMercado,
                            CodFornecedor = temp.CodFornecedor.Equals(DBNull.Value) ? "" : (string)temp.CodFornecedor,
                            NomeFornecedor = temp.NomeFornecedor.Equals(DBNull.Value) ? "" : (string)temp.NomeFornecedor,
                            CodActividade = temp.CodActividade.Equals(DBNull.Value) ? "" : (string)temp.CodActividade,
                            CidadeFornecedor = temp.CidadeFornecedor.Equals(DBNull.Value) ? "" : (string)temp.CidadeFornecedor,
                            CodTermosPagamento = temp.CodTermosPagamento.Equals(DBNull.Value) ? "" : (string)temp.CodTermosPagamento,
                            CodFormaPagamento = temp.CodFormaPagamento.Equals(DBNull.Value) ? "" : (string)temp.CodFormaPagamento,
                            Selecionado = temp.Selecionado.Equals(DBNull.Value) ? false : (bool?)temp.Selecionado,
                            Preferencial = temp.Preferencial.Equals(DBNull.Value) ? false : (bool?)temp.Preferencial,
                            EmailFornecedor = temp.EmailFornecedor.Equals(DBNull.Value) ? "" : (string)temp.EmailFornecedor,
                            DataEnvioAoFornecedor = (DateTime?)temp.DataEnvioAoFornecedor,
                            DataRecepcaoProposta = (DateTime?)temp.DataRecepcaoProposta,
                            UtilizadorEnvio = temp.UtilizadorEnvio.Equals(DBNull.Value) ? "" : (string)temp.UtilizadorEnvio,
                            UtilizadorRecepcaoProposta = temp.UtilizadorRecepcaoProposta.Equals(DBNull.Value) ? "" : (string)temp.UtilizadorRecepcaoProposta,
                            Fase = (int)temp.Fase,
                            PrazoResposta = (int?)temp.PrazoResposta,
                            DataRespostaEsperada = (DateTime?)temp.DataRespostaEsperada,
                            DataPedidoEsclarecimento = (DateTime?)temp.DataPedidoEsclarecimento,
                            DataRespostaEsclarecimento = (DateTime?)temp.DataRespostaEsclarecimento,
                            DataRespostaDoFornecedor = (DateTime?)temp.DataRespostaDoFornecedor,
                            NaoRespostaDoFornecedor = temp.NaoRespostaDoFornecedor.Equals(DBNull.Value) ? false : (bool?)temp.NaoRespostaDoFornecedor,
                            DataEnvioPropostaArea = (DateTime?)temp.DataEnvioPropostaArea,
                            DataRespostaArea = (DateTime?)temp.DataRespostaArea,
                            PedidoCotacaoCriadoEm = (DateTime?)temp.PedidoCotacaoCriadoEm,
                            NoRequisicao = temp.NoRequisicao.Equals(DBNull.Value) ? "" : (string)temp.NoRequisicao,
                            CodRegiao = temp.CodRegiao.Equals(DBNull.Value) ? "" : (string)temp.CodRegiao,
                            CodArea = temp.CodArea.Equals(DBNull.Value) ? "" : (string)temp.CodArea,
                            CodCresp = temp.CodCresp.Equals(DBNull.Value) ? "" : (string)temp.CodCresp,
                            Historico = temp.Historico.Equals(DBNull.Value) ? false : (bool?)temp.Historico,
                            NotaEncomenda_Show = temp.NotaEncomenda.Equals(DBNull.Value) ? "" : (string)temp.NotaEncomenda,
                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
