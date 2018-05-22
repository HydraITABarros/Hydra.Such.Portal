using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBAcordoPrecos
    {
        #region CRUD
        public static AcordoPrecos GetById(string NoProcedimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AcordoPrecosModelView> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcordoPrecos.Select(AcordoPreco => new AcordoPrecosModelView()
                    {
                        NoProcedimento = AcordoPreco.NoProcedimento,
                        DtInicioTexto = AcordoPreco.DtInicio == null ? "" : Convert.ToDateTime(AcordoPreco.DtInicio).ToShortDateString(),
                        DtFimTexto = AcordoPreco.DtFim == null ? "" : Convert.ToDateTime(AcordoPreco.DtFim).ToShortDateString(),
                        ValorTotal = AcordoPreco.ValorTotal
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            //try
            //{
            //    using (var ctx = new SuchDBContext())
            //    {
            //        return ctx.AcordoPrecos.ToList();
            //    }
            //}
            //catch (Exception ex)
            //{

            //    return null;
            //}
        }

        public static AcordoPrecos Create(AcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcordoPrecos.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AcordoPrecos Update(AcordoPrecos item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcordoPrecos.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string NoProcedimento)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    AcordoPrecos userAcordoPrecos = ctx.AcordoPrecos.Where(x => x.NoProcedimento == NoProcedimento).FirstOrDefault();
                    if (userAcordoPrecos != null)
                    {
                        ctx.AcordoPrecos.Remove(userAcordoPrecos);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        #endregion

        //public static List<AcessosLocalizacoes> GetByUserId(string ID_Utilizador)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.AcessosLocalizacoes.Where(x => x.ID_Utilizador == ID_Utilizador)
        //                .ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return new List<AcessosLocalizacoes>();
        //}

        //public static UserAcessosLocalizacoesViewModel ParseToViewModel(this AcessosLocalizacoes item)
        //{
        //    if (item != null)
        //    {
        //        return new UserAcessosLocalizacoesViewModel()
        //        {
        //            ID_Utilizador = item.ID_Utilizador,
        //            Localizacao = item.Localizacao,
        //            DataHora_Criacao = item.DataHora_Criacao.HasValue ? item.DataHora_Criacao : (DateTime?)null,
        //            DataHora_Modificacao = item.DataHora_Modificacao.HasValue ? item.DataHora_Modificacao : (DateTime?)null,
        //            Utilizador_Criacao = item.Utilizador_Criacao,
        //            Utilizador_Modificacao = item.Utilizador_Modificacao
        //        };
        //    }
        //    return null;
        //}

        //public static List<UserAcessosLocalizacoesViewModel> ParseToViewModel(this List<AcessosLocalizacoes> items)
        //{
        //    List<UserAcessosLocalizacoesViewModel> userAcessosLocalizacoes = new List<UserAcessosLocalizacoesViewModel>();
        //    if (items != null)
        //        items.ForEach(x =>
        //            userAcessosLocalizacoes.Add(x.ParseToViewModel()));
        //    return userAcessosLocalizacoes;
        //}

        //public static AcessosLocalizacoes ParseToDB(this AcessosLocalizacoes item)
        //{
        //    if (item != null)
        //    {
        //        return new AcessosLocalizacoes()
        //        {
        //            ID_Utilizador = item.ID_Utilizador,
        //            Localizacao = item.Localizacao,
        //            Utilizador_Criacao = item.Utilizador_Criacao,
        //            DataHora_Criacao = item.DataHora_Criacao.HasValue ? item.DataHora_Criacao : (DateTime?)null,
        //            Utilizador_Modificacao = item.Utilizador_Modificacao,
        //            DataHora_Modificacao = item.DataHora_Modificacao.HasValue ? item.DataHora_Modificacao : (DateTime?)null
        //        };
        //    }
        //    return null;
        //}

        //public static List<AcessosLocalizacoes> ParseToDB(this List<AcessosLocalizacoes> items)
        //{
        //    List<AcessosLocalizacoes> AcessosLocalizacoes = new List<AcessosLocalizacoes>();
        //    if (items != null)
        //        items.ForEach(x =>
        //            AcessosLocalizacoes.Add(x.ParseToDB()));
        //    return AcessosLocalizacoes;
        //}
    }
}
