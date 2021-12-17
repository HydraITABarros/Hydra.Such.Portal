
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static Hydra.Such.Data.Enumerations;
using static Hydra.Such.Data.Logic.DBNAV2009Procedimentos;

namespace Hydra.Such.Data.Logic
{
    public static class DBLinhasAcordoPrecos
    {
        #region CRUD
        public static LinhasAcordoPrecos GetById(string NoProcedimento, string NoFornecedor, string CodProduto, DateTime DtValidadeInicio, string Cresp, string Localizacao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor &&
                                                        x.CodProduto == CodProduto && x.DtValidadeInicio == DtValidadeInicio &&
                                                        x.Cresp == Cresp && x.Localizacao == Localizacao).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasAcordoPrecos> GetAll()
        {
            

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasAcordoPrecos> GetAllByDimensionsUser(string _user)
        {
            List<LinhasAcordoPrecos> _result = new List<LinhasAcordoPrecos>();

            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(_user);

            try
            {
                using (var ctx = new SuchDBContext())
                {
                    _result = ctx.LinhasAcordoPrecos.ToList();
                }
            }
            catch (Exception ex)
            {
               
            }

            if (_result.Count > 0)
            {
                //Regions
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                    _result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.Regiao));
                //FunctionalAreas
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                    _result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.Area));
                //ResponsabilityCenter
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    _result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.Cresp));
            }

            return _result;
        }


        public static List<LinhasAcordoPrecos> GetAllByNoProcedimento(string NoProcedimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasAcordoPrecos> GetAllByDateArea(string Area, DateTime Data)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos.Where(x => x.Area == Area && (x.DtValidadeInicio <= Data && x.DtValidadeFim >= Data)).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static LinhasAcordoPrecos Create(LinhasAcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataCriacao = DateTime.Now;
                    ctx.LinhasAcordoPrecos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasAcordoPrecos Update(LinhasAcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasAcordoPrecos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string NoProcedimento, string NoFornecedor, string CodProduto, DateTime DtValidadeInicio, string Cresp, string Localizacao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    LinhasAcordoPrecos userLinhasAcordoPrecos = ctx.LinhasAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento && x.NoFornecedor == NoFornecedor &&
                                                        x.CodProduto == CodProduto && x.DtValidadeInicio == DtValidadeInicio &&
                                                        x.Cresp == Cresp && x.Localizacao == Localizacao).FirstOrDefault();
                    if (userLinhasAcordoPrecos != null)
                    {
                        ctx.LinhasAcordoPrecos.Remove(userLinhasAcordoPrecos);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool DeleteByProcedimento(string NoProcedimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    LinhasAcordoPrecos userLinhasAcordoPrecos = ctx.LinhasAcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).FirstOrDefault();

                    if (userLinhasAcordoPrecos != null)
                    {
                        ctx.LinhasAcordoPrecos.Remove(userLinhasAcordoPrecos);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static List<LinhasAcordoPrecos> GetForDimensions(DateTime date, string respCenterId, string regionId, string funcAreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos
                        .Where(x => x.Cresp == respCenterId 
                                 && x.Area == funcAreaId
                                 && x.Regiao == regionId
                                 && (x.DtValidadeInicio <= date && x.DtValidadeFim >= date))
                        .ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasAcordoPrecos> GetMateriaPrima(DateTime date, string armazem)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos
                        .Where(x => x.Localizacao == armazem
                                 && (x.DtValidadeInicio <= date && x.DtValidadeFim >= date))
                        .ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<LinhasAcordoPrecos> GetMateriaSubsidiaria(DateTime date, string armazem, string cresp)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasAcordoPrecos
                        .Where(x => x.Localizacao == armazem
                                && x.Cresp == cresp
                                 && (x.DtValidadeInicio <= date && x.DtValidadeFim >= date))
                        .ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<NAV2009FornecedoresContratos> AcordoPrecoGetContratos(string NAVServerName, string NAVDatabaseName, string NAVCompanyName, string FornecedorNo = null)
        {
            try
            {
                List<NAV2009FornecedoresContratos> result = new List<NAV2009FornecedoresContratos>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@ServerName", NAVServerName),
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@FornecedorNo", FornecedorNo )
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009AcordoPrecoGetContratos @ServerName, @DBName, @CompanyName, @FornecedorNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAV2009FornecedoresContratos()
                        {
                            ContratoNo = temp.No.Equals(DBNull.Value) ? "" : (string)temp.No,
                            FornecedorNo = temp.FornecedorNo.Equals(DBNull.Value) ? "" : (string)temp.FornecedorNo,
                            Descricao = temp.Descricao.Equals(DBNull.Value) ? "" : (string)temp.Descricao
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
