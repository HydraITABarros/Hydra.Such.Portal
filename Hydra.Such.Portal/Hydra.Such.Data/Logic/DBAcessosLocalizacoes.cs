using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public static class DBAcessosLocalizacoes
    {
        #region CRUD
        public static AcessosLocalizacoes GetById(string ID_Utilizador, string Localizacao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosLocalizacoes.Where(x => x.IdUtilizador == ID_Utilizador && x.Localizacao == Localizacao).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<AcessosLocalizacoes> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosLocalizacoes.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static AcessosLocalizacoes Create(AcessosLocalizacoes item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraCriacao = DateTime.Now;
                    ctx.AcessosLocalizacoes.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<AcessosLocalizacoes> Create(List<AcessosLocalizacoes> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHoraCriacao = DateTime.Now;
                        ctx.AcessosLocalizacoes.Add(x);
                    });
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<AcessosLocalizacoes> Create(string ID_Utilizador, List<AcessosLocalizacoes> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.IdUtilizador = ID_Utilizador;
                        x.DataHoraCriacao = DateTime.Now;
                        ctx.AcessosLocalizacoes.Add(x);
                    });
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static AcessosLocalizacoes Update(AcessosLocalizacoes item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHoraModificacao = DateTime.Now;
                    ctx.AcessosLocalizacoes.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string ID_Utilizador, string Localizacao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    AcessosLocalizacoes userAcessoLocalizacao = ctx.AcessosLocalizacoes.Where(x => x.IdUtilizador == ID_Utilizador && x.Localizacao == Localizacao).FirstOrDefault();
                    if (userAcessoLocalizacao != null)
                    {
                        ctx.AcessosLocalizacoes.Remove(userAcessoLocalizacao);
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool Delete(AcessosLocalizacoes item)
        {
            return Delete(new List<AcessosLocalizacoes> { item });
        }

        public static bool Delete(List<AcessosLocalizacoes> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.AcessosLocalizacoes.RemoveRange(items);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteAllFromUser(string ID_Utilizador)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<AcessosLocalizacoes> userAccessesToDelete = ctx.AcessosLocalizacoes.Where(x => x.IdUtilizador == ID_Utilizador).ToList();
                    ctx.AcessosLocalizacoes.RemoveRange(userAccessesToDelete);
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

        public static List<AcessosLocalizacoes> GetByUserId(string ID_Utilizador)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.AcessosLocalizacoes.Where(x => x.IdUtilizador == ID_Utilizador)
                        .ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return new List<AcessosLocalizacoes>();
        }

        public static UserAcessosLocalizacoesViewModel ParseToViewModel(this AcessosLocalizacoes item)
        {
            if (item != null)
            {
                return new UserAcessosLocalizacoesViewModel()
                {
                    ID_Utilizador = item.IdUtilizador,
                    Localizacao = item.Localizacao,
                    DataHora_Criacao = item.DataHoraCriacao.HasValue ? item.DataHoraCriacao : (DateTime?)null,
                    DataHora_Modificacao = item.DataHoraModificacao.HasValue ? item.DataHoraModificacao : (DateTime?)null,
                    Utilizador_Criacao = item.UtilizadorCriacao,
                    Utilizador_Modificacao = item.UtilizadorModificacao
                };
            }
            return null;
        }

        public static List<UserAcessosLocalizacoesViewModel> ParseToViewModel(this List<AcessosLocalizacoes> items)
        {
            List<UserAcessosLocalizacoesViewModel> userAcessosLocalizacoes = new List<UserAcessosLocalizacoesViewModel>();
            if (items != null)
                items.ForEach(x =>
                    userAcessosLocalizacoes.Add(x.ParseToViewModel()));
            return userAcessosLocalizacoes;
        }

        public static AcessosLocalizacoes ParseToDB(this AcessosLocalizacoes item)
        {
            if (item != null)
            {
                return new AcessosLocalizacoes()
                {
                    IdUtilizador = item.IdUtilizador,
                    Localizacao = item.Localizacao,
                    UtilizadorCriacao = item.UtilizadorCriacao,
                    DataHoraCriacao = item.DataHoraCriacao.HasValue ? item.DataHoraCriacao : (DateTime?)null,
                    UtilizadorModificacao = item.UtilizadorModificacao,
                    DataHoraModificacao = item.DataHoraModificacao.HasValue ? item.DataHoraModificacao : (DateTime?)null
                };
            }
            return null;
        }

        public static List<AcessosLocalizacoes> ParseToDB(this List<AcessosLocalizacoes> items)
        {
            List<AcessosLocalizacoes> AcessosLocalizacoes = new List<AcessosLocalizacoes>();
            if (items != null)
                items.ForEach(x =>
                    AcessosLocalizacoes.Add(x.ParseToDB()));
            return AcessosLocalizacoes;
        }
    }
}
