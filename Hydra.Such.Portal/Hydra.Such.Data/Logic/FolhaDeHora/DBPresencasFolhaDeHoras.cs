using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBPresencasFolhaDeHoras
    {
        #region CRUD
        public static List<PresençasFolhaDeHoras> GetByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PresençasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PresençasFolhaDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PresençasFolhaDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PresençasFolhaDeHoras Create(PresençasFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.PresençasFolhaDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PresençasFolhaDeHoras Update(PresençasFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.PresençasFolhaDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string FolhaHoraNo, string Data)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PresençasFolhaDeHoras.RemoveRange(ctx.PresençasFolhaDeHoras.Where(x => (x.NºFolhaDeHoras == FolhaHoraNo) && (x.Data == Convert.ToDateTime(Data))));
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

        public static List<PresencasFolhaDeHorasViewModel> GetAllByPresencaToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PresençasFolhaDeHoras.Where(Presenca => Presenca.NºFolhaDeHoras == FolhaHoraNo).Select(Presenca => new PresencasFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = Presenca.NºFolhaDeHoras,
                        Data = Presenca.Data,
                        DataTexto = Presenca.Data.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Hora1Entrada = Presenca.Hora1ªEntrada.Value.ToString(),
                        Hora1Saida = Presenca.Hora1ªSaída.Value.ToString(),
                        Hora2Entrada = Presenca.Hora2ªEntrada.Value.ToString(),
                        Hora2Saida = Presenca.Hora2ªSaída.Value.ToString(),
                        Observacoes = Presenca.Observacoes,
                        UtilizadorCriacao = Presenca.UtilizadorCriação,
                        DataHoraCriacao = Presenca.DataHoraCriação,
                        DataHoraCriacaoTexto = Presenca.DataHoraCriação.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        UtilizadorModificacao = Presenca.UtilizadorModificação,
                        DataHoraModificacao = Presenca.DataHoraModificação,
                        DataHoraModificacaoTexto = Presenca.DataHoraModificação.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
