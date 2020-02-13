using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Abate
    {
        public static List<Viaturas2Abate> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Abate.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Abate GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Abate.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Abate> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Abate.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Abate Create(Viaturas2Abate ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Abate.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Abate ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Abate.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Abate Update(Viaturas2Abate ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Abate.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Abate ParseToDB(Viaturas2AbateViewModel x)
        {
            Viaturas2Abate viatura = new Viaturas2Abate()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDTipoAtoAdministrativo = x.IDTipoAtoAdministrativo,
                NoRegisto = x.NoRegisto,
                IDDescricaoAto = x.IDDescricaoAto,
                Fundamentacao = x.Fundamentacao,
                Autor = x.Autor,
                Data = x.Data,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataTexto)) viatura.Data = Convert.ToDateTime(x.DataTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2Abate> ParseListToViewModel(List<Viaturas2AbateViewModel> x)
        {
            List<Viaturas2Abate> Viaturas2Abate = new List<Viaturas2Abate>();

            x.ForEach(y => Viaturas2Abate.Add(ParseToDB(y)));

            return Viaturas2Abate;
        }

        public static Viaturas2AbateViewModel ParseToViewModel(Viaturas2Abate x)
        {
            Viaturas2AbateViewModel viatura = new Viaturas2AbateViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDTipoAtoAdministrativo = x.IDTipoAtoAdministrativo,
                NoRegisto = x.NoRegisto,
                IDDescricaoAto = x.IDDescricaoAto,
                Fundamentacao = x.Fundamentacao,
                Autor = x.Autor,
                Data = x.Data,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.Data != null) viatura.DataTexto = x.Data.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2AbateViewModel> ParseListToViewModel(List<Viaturas2Abate> x)
        {
            List<Viaturas2AbateViewModel> Viaturas2Abate = new List<Viaturas2AbateViewModel>();

            x.ForEach(y => Viaturas2Abate.Add(ParseToViewModel(y)));

            return Viaturas2Abate;
        }
    }
}
