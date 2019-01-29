using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public static class DBDistribuicaoCustoFolhaDeHoras
    {
        #region CRUD

        public static DistribuiçãoCustoFolhaDeHoras GetDistribuiçãoByID(string NoFolhasDeHoras, int NoLinhaPercursosAjudasCustoDespesas, int NoLinha)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DistribuiçãoCustoFolhaDeHoras.Where(x => x.NºFolhasDeHoras == NoFolhasDeHoras && x.NºLinhaPercursosEAjudasCustoDespesas == NoLinhaPercursosAjudasCustoDespesas && x.NºLinha == NoLinha).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DistribuiçãoCustoFolhaDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DistribuiçãoCustoFolhaDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DistribuiçãoCustoFolhaDeHoras Create(DistribuiçãoCustoFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.DistribuiçãoCustoFolhaDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static DistribuiçãoCustoFolhaDeHoras Update(DistribuiçãoCustoFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.DistribuiçãoCustoFolhaDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Update(List<DistribuiçãoCustoFolhaDeHoras> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (items != null)
                    {
                        items.ForEach(item => item.DataHoraModificação = DateTime.Now);
                    }
                    ctx.DistribuiçãoCustoFolhaDeHoras.UpdateRange(items);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public static void Update(List<DistribuiçãoCustoFolhaDeHoras> items, SuchDBContext ctx)
        {
            if (items != null)
            {
                items.ForEach(item => item.DataHoraModificação = DateTime.Now);
                ctx.DistribuiçãoCustoFolhaDeHoras.UpdateRange(items);
            }
        }

        public static bool Delete(DistribuiçãoCustoFolhaDeHoras ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DistribuiçãoCustoFolhaDeHoras.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        #region Parse Utilities
        public static DistribuicaoCustoFolhaDeHorasViewModel ParseToViewModel(this DistribuiçãoCustoFolhaDeHoras item)
        {
            if (item != null)
            {
                return new DistribuicaoCustoFolhaDeHorasViewModel()
                {
                    NoFolhasDeHoras = item.NºFolhasDeHoras,
                    NoLinhaPercursosEAjudasCustoDespesas = item.NºLinhaPercursosEAjudasCustoDespesas,
                    NoLinha = item.NºLinha,
                    TipoObra = item.TipoObra,
                    NoObra = item.NºObra,
                    PercentagemValor = item.PercentagemValor,
                    Valor = item.Valor,
                    TotalValor = item.TotalValor,
                    TotalPercentagemValor = item.TotalPercentagemValor,
                    KmTotais = item.KmTotais,
                    KmDistancia = item.KmDistancia,
                    Quantidade = item.Quantidade,
                    CodigoRegiao = item.CódigoRegião,
                    CodigoAreaFuncional = item.CódigoÁreaFuncional,
                    CodigoCentroResponsabilidade = item.CódigoCentroResponsabilidade,
                    DataHoraCriacao = item.DataHoraCriação,
                    DataHoraModificacao = item.DataHoraModificação,
                    UtilizadorCriacao = item.UtilizadorCriação,
                    UtilizadorModificacao = item.UtilizadorModificação
                };
            }
            return null;
        }

        public static List<DistribuicaoCustoFolhaDeHorasViewModel> ParseToViewModel(this List<DistribuiçãoCustoFolhaDeHoras> items)
        {
            List<DistribuicaoCustoFolhaDeHorasViewModel> parsedItems = new List<DistribuicaoCustoFolhaDeHorasViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static DistribuiçãoCustoFolhaDeHoras ParseToDB(this DistribuicaoCustoFolhaDeHorasViewModel item)
        {
            if (item != null)
            {
                return new DistribuiçãoCustoFolhaDeHoras()
                {
                    NºFolhasDeHoras = item.NoFolhasDeHoras,
                    NºLinhaPercursosEAjudasCustoDespesas = item.NoLinhaPercursosEAjudasCustoDespesas,
                    NºLinha = item.NoLinha,
                    TipoObra = item.TipoObra,
                    NºObra = item.NoObra,
                    PercentagemValor = item.PercentagemValor,
                    Valor = item.Valor,
                    TotalValor = item.TotalValor,
                    TotalPercentagemValor = item.TotalPercentagemValor,
                    KmTotais = item.KmTotais,
                    KmDistancia = item.KmDistancia,
                    Quantidade = item.Quantidade,
                    CódigoRegião = item.CodigoRegiao,
                    CódigoÁreaFuncional = item.CodigoAreaFuncional,
                    CódigoCentroResponsabilidade = item.CodigoCentroResponsabilidade,
                    DataHoraCriação = item.DataHoraCriacao,
                    DataHoraModificação = item.DataHoraModificacao,
                    UtilizadorCriação = item.UtilizadorCriacao,
                    UtilizadorModificação = item.UtilizadorModificacao
                };
            }
            return null;
        }

        public static List<DistribuiçãoCustoFolhaDeHoras> ParseToDB(this List<DistribuicaoCustoFolhaDeHorasViewModel> items)
        {
            List<DistribuiçãoCustoFolhaDeHoras> parsedItems = new List<DistribuiçãoCustoFolhaDeHoras>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
