using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBOrigemDestinoFh
    {
        #region CRUD

        public static OrigemDestinoFh GetById(string id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.OrigemDestinoFh.FirstOrDefault(x => x.Código == id);
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<OrigemDestinoFh> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.OrigemDestinoFh.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static OrigemDestinoFh Create(OrigemDestinoFh ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.OrigemDestinoFh.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static OrigemDestinoFh Update(OrigemDestinoFh ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraÚltimaAlteração = DateTime.Now;
                    ctx.OrigemDestinoFh.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(OrigemDestinoFh ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.OrigemDestinoFh.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static string GetOrigemDestinoDescricao(string Origem)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    OrigemDestinoFh OrigemDestinhoFH;

                    OrigemDestinhoFH = ctx.OrigemDestinoFh.FirstOrDefault(x => x.Código == Origem);
                    return OrigemDestinhoFH.Descrição;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static OrigemDestinoFh ParseToDB(OrigemDestinoFHViewModel x)
        {
            try
            {
                return new OrigemDestinoFh()
                {
                    Código = x.Codigo,
                    Descrição = x.Descricao,
                    CriadoPor = x.CriadoPor,
                    DataHoraCriação = x.DataHoraCriacao,
                    AlteradoPor = x.AlteradoPor,
                    DataHoraÚltimaAlteração = x.DataHoraUltimaAlteracao
                };
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static OrigemDestinoFHViewModel ParseToViewModel(OrigemDestinoFh x)
        {
            return new OrigemDestinoFHViewModel()
            {
                Codigo = x.Código,
                Descricao = x.Descrição,
                CriadoPor = x.CriadoPor,
                DataHoraCriacao = x.DataHoraCriação,
                AlteradoPor = x.AlteradoPor,
                DataHoraUltimaAlteracao = x.DataHoraÚltimaAlteração
            };
        }

        public static List<OrigemDestinoFHViewModel> ParseListToViewModel(List<OrigemDestinoFh> x)
        {
            List<OrigemDestinoFHViewModel> result = new List<OrigemDestinoFHViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }
        #endregion
    }
}
