using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturaImagem
    {

        public static List<ViaturasImagens> GetAllToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ViaturasImagens.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ViaturasImagens GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ViaturasImagens.Where(p => p.Matricula == Matricula).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ViaturasImagens Create(ViaturasImagens ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.ViaturasImagens.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(ViaturasImagens ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ViaturasImagens.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static ViaturasImagens Update(ViaturasImagens ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.ViaturasImagens.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ViaturasImagens ParseToDB(ViaturasImagensViewModel x)
        {
            ViaturasImagens viatura = new ViaturasImagens()
            {
                Matricula = x.Matricula,
                Imagem = x.Imagem,
                DataHoraCriacao = x.DataHoraCriacao != "" && x.DataHoraCriacao != null ? DateTime.Parse(x.DataHoraCriacao) : (DateTime?)null,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraModificacao = x.DataHoraModificacao != "" && x.DataHoraModificacao != null ? DateTime.Parse(x.DataHoraModificacao) : (DateTime?)null,
                UtilizadorModificacao = x.UtilizadorModificacao
            };

            return viatura;
        }

        public static ViaturasImagensViewModel ParseToViewModel(ViaturasImagens x)
        {
            ViaturasImagensViewModel viatura = new ViaturasImagensViewModel()
            {
                Matricula = x.Matricula,
                Imagem = x.Imagem,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataHoraCriacao = x.DataHoraCriacao.Value.ToString("yyyy-MM-dd hh:mm:ss"),
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataHoraModificacao = x.DataHoraModificacao.Value.ToString("yyyy-MM-dd hh:mm:ss")
            };

            return viatura;
        }

        public static List<ViaturasImagensViewModel> ParseListToViewModel(List<ViaturasImagens> x)
        {
            List<ViaturasImagensViewModel> viatura = new List<ViaturasImagensViewModel>();

            x.ForEach(y => viatura.Add(ParseToViewModel(y)));

            return viatura;
        }

    }
}
