using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2ViaVerde
    {
        public static List<Viaturas2ViaVerde> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ViaVerde.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2ViaVerde GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ViaVerde.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2ViaVerde> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2ViaVerde.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2ViaVerde Create(Viaturas2ViaVerde ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2ViaVerde.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2ViaVerde ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2ViaVerde.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2ViaVerde Update(Viaturas2ViaVerde ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2ViaVerde.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2ViaVerde ParseToDB(Viaturas2ViaVerdeViewModel x)
        {
            Viaturas2ViaVerde viatura = new Viaturas2ViaVerde()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEmpresa = x.IDEmpresa,
                NoIdentificador = x.NoIdentificador,
                NoContrato = x.NoContrato,
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

        public static List<Viaturas2ViaVerde> ParseListToViewModel(List<Viaturas2ViaVerdeViewModel> x)
        {
            List<Viaturas2ViaVerde> Viaturas2ViaVerde = new List<Viaturas2ViaVerde>();

            x.ForEach(y => Viaturas2ViaVerde.Add(ParseToDB(y)));

            return Viaturas2ViaVerde;
        }

        public static Viaturas2ViaVerdeViewModel ParseToViewModel(Viaturas2ViaVerde x)
        {
            Viaturas2ViaVerdeViewModel viatura = new Viaturas2ViaVerdeViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                IDEmpresa = x.IDEmpresa,
                NoIdentificador = x.NoIdentificador,
                NoContrato = x.NoContrato,
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

        public static List<Viaturas2ViaVerdeViewModel> ParseListToViewModel(List<Viaturas2ViaVerde> x)
        {
            List<Viaturas2ViaVerdeViewModel> Viaturas2ViaVerde = new List<Viaturas2ViaVerdeViewModel>();

            x.ForEach(y => Viaturas2ViaVerde.Add(ParseToViewModel(y)));

            return Viaturas2ViaVerde;
        }
    }
}