using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBFolhasDeHoras
    {
        #region CRUD
        public static FolhasDeHoras GetById(string NFolhaDeHora)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(x => x.NºFolhaDeHoras == NFolhaDeHora).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhasDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FolhasDeHoras Create(FolhasDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.FolhasDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FolhasDeHoras Update(FolhasDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.FolhasDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(string FolhaDeHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.FolhasDeHoras.RemoveRange(ctx.FolhasDeHoras.Where(x => x.NºFolhaDeHoras == FolhaDeHoraNo));
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

        public static List<FolhasDeHoras> GetAllByArea(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(x => x.Área == AreaId).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHorasViewModel> GetAllByAreaToList(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(FH => FH.Área == AreaId).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = "",
                        EmpregadoNo = FH.NºEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida.Value.ToShortDateString(),
                        HoraPartidaTexto = FH.DataHoraPartida.Value.ToShortTimeString(),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada.Value.ToShortDateString(),
                        HoraChegadaTexto = FH.DataHoraChegada.Value.ToShortTimeString(),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho.ToString(),
                        Validadores = FH.Validadores,
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação.Value.ToShortDateString(),
                        HoraCriacaoTexto = FH.DataHoraCriação.Value.ToShortTimeString(),
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToShortDateString(),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToShortTimeString(),
                        //abarros_
                        //UtilizadorCriacao = FH.UtilizadorCriação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação.Value.ToShortDateString(),
                        HoraModificacaoTexto = FH.DataHoraModificação.Value.ToShortTimeString(),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        EmpregadoNome = FH.NomeEmpregado,
                        Matricula = FH.Matrícula,
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada.ToString(),
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado.Value.ToShortDateString(),
                        HoraTerminadoTexto = FH.DataHoraTerminado.Value.ToShortTimeString(),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada.ToString(),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação.Value.ToShortDateString(),
                        HoraValidacaoTexto = FH.DataHoraValidação.Value.ToShortTimeString(),
                        IntegradorEmRH = FH.IntegradorEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToShortDateString(),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToShortTimeString(),
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToShortDateString(),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToShortTimeString()
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region PERCURSO
        #endregion
    }
}
