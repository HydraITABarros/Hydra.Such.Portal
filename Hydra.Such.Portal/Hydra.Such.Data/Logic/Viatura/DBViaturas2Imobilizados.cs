using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Imobilizados
    {
        public static List<Viaturas2Imobilizados> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Imobilizados.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Imobilizados GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Imobilizados.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Imobilizados> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Imobilizados.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2ImobilizadosViewModel> GetListPai(string NAVDatabaseName, string NAVCompanyName, string NoImobilizado)
        {
            try
            {
                List<Viaturas2ImobilizadosViewModel> result = new List<Viaturas2ImobilizadosViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@Code", NoImobilizado),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec Viaturas2_ImobilizadoNoList @DBName, @CompanyName, @Code", parameters);

                    foreach (dynamic temp in data)
                    {
                        var item = new Viaturas2ImobilizadosViewModel();

                        item.NoImobilizado = temp.ImobilizadoNo;
                        item.Descricao = temp.Descricao;


                        result.Add(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Viaturas2ImobilizadosViewModel> GetAllImobilizados(string NAVDatabaseName, string NAVCompanyName, string NoImobilizado)
        {
            try
            {
                List<Viaturas2ImobilizadosViewModel> result = new List<Viaturas2ImobilizadosViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@Code", NoImobilizado),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec Viaturas2_ImobilizadoList @DBName, @CompanyName, @Code", parameters);

                    foreach (dynamic temp in data)
                    {
                        var item = new Viaturas2ImobilizadosViewModel();

                        item.NoImobilizado = temp.NoImbilizado.Equals(DBNull.Value) ? "" : Convert.ToString(temp.NoImbilizado);
                        item.Descricao = temp.Descricao.Equals(DBNull.Value) ? "" : Convert.ToString(temp.Descricao);
                        item.DataCompraTexto = temp.DataCompra.Equals(DBNull.Value) ? "" : Convert.ToString(temp.DataCompra);
                        item.DocumentoCompra = temp.DocumentoCompra.Equals(DBNull.Value) ? "" : Convert.ToString(temp.DocumentoCompra);
                        item.ValorCompra = temp.ValorCompra.Equals(DBNull.Value) ? "" : Convert.ToString(temp.ValorCompra);
                        item.DataIncioAmortizacaoTexto = temp.DataIncioAmortizacao.Equals(DBNull.Value) ? "" : Convert.ToString(temp.DataIncioAmortizacao);
                        item.DataFinalAmortizacaoTexto = temp.DataFinalAmortizacao.Equals(DBNull.Value) ? "" : Convert.ToString(temp.DataFinalAmortizacao);
                        item.ValorAmortizado = temp.ValorAmortizado.Equals(DBNull.Value) ? "" : Convert.ToString(temp.ValorAmortizado);
                        item.VendaAbate = temp.VendaAbate.Equals(DBNull.Value) ? "" : Convert.ToString(temp.VendaAbate);
                        item.DataVendaAbateTexto = temp.DataVendaAbate.Equals(DBNull.Value) ? "" : Convert.ToString(temp.DataVendaAbate);
                        item.DocumentoVendaAbate = temp.DocumentoVendaAbate.Equals(DBNull.Value) ? "" : Convert.ToString(temp.DocumentoVendaAbate);
                        item.ValorVendaAbate = temp.ValorVendaAbate.Equals(DBNull.Value) ? "" : Convert.ToString(temp.ValorVendaAbate);
                        item.EstadoImobilizado = temp.EstadoImobilizado.Equals(DBNull.Value) ? "" : Convert.ToString(temp.EstadoImobilizado);
                        item.Bloqueado = temp.Bloqueado.Equals(DBNull.Value) ? "" : Convert.ToString(temp.Bloqueado);
                        item.Pai = temp.Bloqueado.Equals(DBNull.Value) ? "" : Convert.ToString(temp.Pai);
                        item.PaiFilho = temp.Bloqueado.Equals(DBNull.Value) ? "" : Convert.ToString(temp.PaiFilho);

                        result.Add(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Viaturas2Imobilizados Create(Viaturas2Imobilizados ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Imobilizados.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Imobilizados ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Imobilizados.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Imobilizados Update(Viaturas2Imobilizados ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Imobilizados.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Imobilizados ParseToDB(Viaturas2ImobilizadosViewModel x)
        {
            Viaturas2Imobilizados viatura = new Viaturas2Imobilizados()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                NoImobilizado = x.NoImobilizado,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2Imobilizados> ParseListToViewModel(List<Viaturas2ImobilizadosViewModel> x)
        {
            List<Viaturas2Imobilizados> Viaturas2Imobilizados = new List<Viaturas2Imobilizados>();

            x.ForEach(y => Viaturas2Imobilizados.Add(ParseToDB(y)));

            return Viaturas2Imobilizados;
        }

        public static Viaturas2ImobilizadosViewModel ParseToViewModel(Viaturas2Imobilizados x)
        {
            Viaturas2ImobilizadosViewModel viatura = new Viaturas2ImobilizadosViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                NoImobilizado = x.NoImobilizado,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2ImobilizadosViewModel> ParseListToViewModel(List<Viaturas2Imobilizados> x)
        {
            List<Viaturas2ImobilizadosViewModel> Viaturas2Imobilizados = new List<Viaturas2ImobilizadosViewModel>();

            x.ForEach(y => Viaturas2Imobilizados.Add(ParseToViewModel(y)));

            return Viaturas2Imobilizados;
        }
    }
}
