using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Hydra.Such.Data.Logic.Project;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel.ProjectView;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel;

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
                    return ctx.FolhasDeHoras.Where(x => x.NºFolhaDeHoras == NFolhaDeHora && x.Eliminada == false).FirstOrDefault();
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
                    return ctx.FolhasDeHoras.Where(x => x.Eliminada == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhasDeHoras> GetAllByViatura(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(x => x.Matrícula == Matricula).ToList();
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

        public static FolhasDeHoras UpdateDetalhes(string NoFolhaHoras, string NAV2009DatabaseName, string NAV2009CompanyName)
        {
            try
            {
                decimal CustoTotalAjudaCusto = 0;
                decimal CustoTotalHoras = 0;
                decimal CustoTotalKm = 0;
                decimal NumTotalKm = 0;
                FolhasDeHoras FolhaDeHora = DBFolhasDeHoras.GetById(NoFolhaHoras);

                CustoTotalAjudaCusto = DBLinhasFolhaHoras.GetCustoTotalAjudaCustoByFolhaHoraNo(NoFolhaHoras);
                CustoTotalHoras = DBMaoDeObraFolhaDeHoras.GetCustoTotalHorasByFolhaHoraNo(NoFolhaHoras);
                NumTotalKm = DBLinhasFolhaHoras.GetNoTotalKmByFolhaHoraNo(NoFolhaHoras);
                CustoTotalKm = DBLinhasFolhaHoras.GetCustoTotalKMByFolhaHoraNo(NoFolhaHoras);

                using (var ctx = new SuchDBContext())
                {
                    FolhaDeHora.CustoTotalAjudaCusto = CustoTotalAjudaCusto;
                    FolhaDeHora.CustoTotalHoras = CustoTotalHoras;
                    FolhaDeHora.CustoTotalKm = CustoTotalKm;
                    FolhaDeHora.NumTotalKm = NumTotalKm;
                    FolhaDeHora.DataHoraModificação = DateTime.Now;

                    ctx.FolhasDeHoras.Update(FolhaDeHora);
                    ctx.SaveChanges();
                }

                FolhaDeHorasViewModel FH = GetListaValidadoresIntegradores(NoFolhaHoras, FolhaDeHora.NºEmpregado, NAV2009DatabaseName, NAV2009CompanyName);
                using (var ctx = new SuchDBContext())
                {
                    FolhaDeHora.CódigoRegião = FH.CodigoRegiao;
                    FolhaDeHora.CódigoÁreaFuncional = FH.CodigoAreaFuncional;
                    FolhaDeHora.CódigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade;

                    FolhaDeHora.NºResponsável1 = FH.Responsavel1No;
                    FolhaDeHora.NºResponsável2 = FH.Responsavel2No;
                    FolhaDeHora.NºResponsável3 = FH.Responsavel3No;

                    FolhaDeHora.Validadores = FH.Validadores;
                    FolhaDeHora.IntegradoresEmRh = FH.IntegradoresEmRH;
                    FolhaDeHora.IntegradoresEmRhkm = FH.IntegradoresEmRHKM;

                    FH.Intervenientes = FH.Intervenientes;

                    FolhaDeHora.DataHoraModificação = DateTime.Now;

                    ctx.FolhasDeHoras.Update(FolhaDeHora);
                    ctx.SaveChanges();
                }

                return FolhaDeHora;
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

        public static List<FolhasDeHoras> GetAllByArea(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(x => x.Área == AreaId && x.Eliminada == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public static List<FolhaDeHorasViewModel> GetAllByAreaToList(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.FolhasDeHoras.Where(FH => FH.Área == AreaId && FH.Eliminada == false).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetDimensionRegiao(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            string regiao = "";

            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(user);
            if (CUserDimensions != null)
            {
                CUserDimensions.ForEach(X =>
                {
                    if (X.Dimensão == 1)
                        if (regiao == "")
                            regiao = X.ValorDimensão;
                        else
                            regiao = regiao + "," + X.ValorDimensão;
                });
            }

            if (regiao == "")
            {
                List<NAVDimValueViewModel> list_regiao = DBNAV2017DimensionValues.GetByDimType(NAVDatabaseName, NAVCompanyName, 1);
                if (list_regiao != null)
                {
                    list_regiao.ForEach(x =>
                    {
                        if (regiao == "")
                            regiao = x.Code;
                        else
                            regiao = regiao + "," + x.Code;
                    });
                }
            }

            return regiao;
        }

        public static string GetDimensionArea(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            string area = "";

            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(user);

            if (CUserDimensions != null)
            {
                CUserDimensions.ForEach(X =>
                {
                    if (X.Dimensão == 2)
                        if (area == "")
                            area = X.ValorDimensão;
                        else
                            area = area + "," + X.ValorDimensão;
                });
            }

            if (area == "")
            {
                List<NAVDimValueViewModel> list_area = DBNAV2017DimensionValues.GetByDimType(NAVDatabaseName, NAVCompanyName, 2);
                if (list_area != null)
                {
                    list_area.ForEach(x =>
                    {
                        if (area == "")
                            area = x.Code;
                        else
                            area = area + "," + x.Code;
                    });
                }
            }

            return area;
        }

        public static string GetDimensionCresp(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            string cresp = "";

            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(user);

            if (CUserDimensions != null)
            {
                CUserDimensions.ForEach(X =>
                {
                    if (X.Dimensão == 3)
                        if (cresp == "")
                            cresp = X.ValorDimensão;
                        else
                            cresp = cresp + "," + X.ValorDimensão;
                });
            }

            if (cresp == "")
            {
                List<NAVDimValueViewModel> list_cresp = DBNAV2017DimensionValues.GetByDimType(NAVDatabaseName, NAVCompanyName, 3);
                if (list_cresp != null)
                {
                    list_cresp.ForEach(x =>
                    {
                        if (cresp == "")
                            cresp = x.Code;
                        else
                            cresp = cresp + "," + x.Code;
                    });
                }
            }

            return cresp;
        }

        public static List<FolhaDeHorasViewModel> GetAllByAdministrator(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            try
            {
                //string regiao = "";
                //string area = "";
                //string cresp = "";
                //string userName = "";

                //regiao = GetDimensionRegiao(NAVDatabaseName, NAVCompanyName, user);
                //area = GetDimensionArea(NAVDatabaseName, NAVCompanyName, user);
                //cresp = GetDimensionCresp(NAVDatabaseName, NAVCompanyName, user);
                //userName = DBUserConfigurations.GetById(user).Nome;

                using (var ctx = new SuchDBContext())
                {
                    List<FolhaDeHorasViewModel> AllFH = ctx.FolhasDeHoras.Where(x =>
                        //(x.Intervenientes.ToLower().Contains(user.ToLower())) &&
                        //(regiao.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                        //(area.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                        //(cresp.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null) &&

                        (x.Eliminada == false)
                    ).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();

                    //ConfigUtilizadores ConfigUser = DBUserConfigurations.GetById(user);
                    //if (ConfigUser != null && ConfigUser.Administrador != true)
                    //{
                    //    AllFH.RemoveAll(x => !x.Intervenientes.ToLower().Contains(user.ToLower()));
                    //}
                    return AllFH;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHorasViewModel> GetAllByTodas(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            try
            {
                //string regiao = "";
                //string area = "";
                //string cresp = "";
                //string userName = "";

                ConfigUtilizadores CUser = DBUserConfigurations.GetById(user);
                //regiao = GetDimensionRegiao(NAVDatabaseName, NAVCompanyName, user);
                //area = GetDimensionArea(NAVDatabaseName, NAVCompanyName, user);
                //cresp = GetDimensionCresp(NAVDatabaseName, NAVCompanyName, user);
                //userName = DBUserConfigurations.GetById(user).Nome;

                using (var ctx = new SuchDBContext())
                {
                    List<FolhaDeHorasViewModel> AllFH = ctx.FolhasDeHoras.Where(x =>
                        //(x.Intervenientes.ToLower().Contains(user.ToLower())) &&
                        //(regiao.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                        //(area.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                        //(cresp.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null) &&

                        (x.NºEmpregado == CUser.EmployeeNo ||
                        x.CriadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.TerminadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Validador.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRh.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRhKm.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.UtilizadorModificação.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Intervenientes.ToUpper().Contains(" " + CUser.IdUtilizador.ToUpper())) &&

                        (x.Eliminada == false) &&
                        (x.Estado != 2)
                    ).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();

                    //ConfigUtilizadores ConfigUser = DBUserConfigurations.GetById(user);
                    //if (ConfigUser != null && ConfigUser.Administrador != true)
                    //{
                    //    AllFH.RemoveAll(x => !(x.Intervenientes.ToLower().Contains(user.ToLower()) || x.CriadoPor.ToLower().Contains(user.ToLower())));
                    //}
                    return AllFH;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHorasViewModel> GetAllByPendentes(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            try
            {
                //string regiao = "";
                //string area = "";
                //string cresp = "";
                //string userName = "";

                ConfigUtilizadores CUser = DBUserConfigurations.GetById(user);
                //regiao = GetDimensionRegiao(NAVDatabaseName, NAVCompanyName, user);
                //area = GetDimensionArea(NAVDatabaseName, NAVCompanyName, user);
                //cresp = GetDimensionCresp(NAVDatabaseName, NAVCompanyName, user);
                //userName = DBUserConfigurations.GetById(user).Nome;

                using (var ctx = new SuchDBContext())
                {
                    List<FolhaDeHorasViewModel> AllFH = ctx.FolhasDeHoras.Where(x =>
                        //(x.Intervenientes.ToLower().Contains(user.ToLower())) &&
                        //(regiao.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                        //(area.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                        //(cresp.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null) &&

                        (x.NºEmpregado == CUser.EmployeeNo ||
                        x.CriadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.TerminadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Validador.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRh.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRhKm.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.UtilizadorModificação.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Intervenientes.ToUpper().Contains(" " + CUser.IdUtilizador.ToUpper())) &&

                        (x.Eliminada == false) &&
                        (x.Estado == 0) &&
                        (x.Terminada == null || x.Terminada == false)
                    ).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();

                    //ConfigUtilizadores ConfigUser = DBUserConfigurations.GetById(user);
                    //if (ConfigUser != null && ConfigUser.Administrador != true)
                    //{
                    //    AllFH.RemoveAll(x => !x.Intervenientes.ToLower().Contains(user.ToLower()));
                    //}
                    return AllFH;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHorasViewModel> GetAllByValidacao(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            try
            {
                //string regiao = "";
                //string area = "";
                //string cresp = "";
                //string userName = "";

                ConfigUtilizadores CUser = DBUserConfigurations.GetById(user);
                //regiao = GetDimensionRegiao(NAVDatabaseName, NAVCompanyName, user);
                //area = GetDimensionArea(NAVDatabaseName, NAVCompanyName, user);
                //cresp = GetDimensionCresp(NAVDatabaseName, NAVCompanyName, user);
                //userName = DBUserConfigurations.GetById(user).Nome;

                using (var ctx = new SuchDBContext())
                {
                    List<FolhaDeHorasViewModel> AllFH = ctx.FolhasDeHoras.Where(x =>
                        //(x.Intervenientes.ToLower().Contains(user.ToLower())) &&
                        //(regiao.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                        //(area.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                        //(cresp.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null) &&
                        //(x.Validadores.ToLower().Contains(user.ToLower())) &&

                        (x.NºEmpregado == CUser.EmployeeNo ||
                        x.CriadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.TerminadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Validador.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRh.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRhKm.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.UtilizadorModificação.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Intervenientes.ToUpper().Contains(" " + CUser.IdUtilizador.ToUpper())) &&

                        (x.Eliminada == false) &&
                        (x.Estado == 0) &&
                        (x.Terminada == true)
                    ).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();

                    //ConfigUtilizadores ConfigUser = DBUserConfigurations.GetById(user);
                    //if (ConfigUser != null && ConfigUser.Administrador != true)
                    //{
                    //    AllFH.RemoveAll(x => !x.Intervenientes.ToLower().Contains(user.ToLower()));
                    //}
                    return AllFH;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHorasViewModel> GetAllByIntegracaoAjuda(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            try
            {
                //string regiao = "";
                //string area = "";
                //string cresp = "";
                //string userName = "";

                ConfigUtilizadores CUser = DBUserConfigurations.GetById(user);
                //regiao = GetDimensionRegiao(NAVDatabaseName, NAVCompanyName, user);
                //area = GetDimensionArea(NAVDatabaseName, NAVCompanyName, user);
                //cresp = GetDimensionCresp(NAVDatabaseName, NAVCompanyName, user);
                //userName = DBUserConfigurations.GetById(user).Nome;

                using (var ctx = new SuchDBContext())
                {
                    List<FolhaDeHorasViewModel> AllFH = ctx.FolhasDeHoras.Where(x =>
                        //(x.Intervenientes.ToLower().Contains(user.ToLower())) &&
                        //(regiao.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                        //(area.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                        //(cresp.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null) &&
                        //(x.IntegradoresEmRh.ToLower().Contains(user.ToLower())) &&

                        (x.NºEmpregado == CUser.EmployeeNo ||
                        x.CriadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.TerminadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Validador.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRh.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRhKm.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.UtilizadorModificação.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Intervenientes.ToUpper().Contains(" " + CUser.IdUtilizador.ToUpper())) &&

                        (x.Eliminada == false) &&
                        (x.IntegradoEmRh == false || x.IntegradoEmRh == null) &&
                        (x.Estado == 1) //1 = VALIDADO
                    ).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();

                    //ConfigUtilizadores ConfigUser = DBUserConfigurations.GetById(user);
                    //if (ConfigUser != null && ConfigUser.Administrador != true)
                    //{
                    //    AllFH.RemoveAll(x => !x.Intervenientes.ToLower().Contains(user.ToLower()));
                    //}
                    return AllFH;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHorasViewModel> GetAllByIntegracaoKMS(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            try
            {
                //string regiao = "";
                //string area = "";
                //string cresp = "";
                //string userName = "";

                ConfigUtilizadores CUser = DBUserConfigurations.GetById(user);
                //regiao = GetDimensionRegiao(NAVDatabaseName, NAVCompanyName, user);
                //area = GetDimensionArea(NAVDatabaseName, NAVCompanyName, user);
                //cresp = GetDimensionCresp(NAVDatabaseName, NAVCompanyName, user);
                //userName = DBUserConfigurations.GetById(user).Nome;

                using (var ctx = new SuchDBContext())
                {
                    List<FolhaDeHorasViewModel> AllFH = ctx.FolhasDeHoras.Where(x =>
                        //(x.Intervenientes.ToLower().Contains(user.ToLower())) &&
                        //(regiao.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                        //(area.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                        //(cresp.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null) &&
                        //x.IntegradoresEmRhkm.ToLower().Contains(user.ToLower()) &&

                        (x.NºEmpregado == CUser.EmployeeNo ||
                        x.CriadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.TerminadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Validador.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRh.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRhKm.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.UtilizadorModificação.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Intervenientes.ToUpper().Contains(" " + CUser.IdUtilizador.ToUpper())) &&

                        (x.Eliminada == false) &&
                        (x.IntegradoEmRhkm == false || x.IntegradoEmRhkm == null) &&
                        (x.Estado == 1) && // 1 == VALIDADO
                        (x.TipoDeslocação == 2) // 2 == "Viatura Própria"
                    ).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();

                    //ConfigUtilizadores ConfigUser = DBUserConfigurations.GetById(user);
                    //if (ConfigUser != null && ConfigUser.Administrador != true)
                    //{
                    //    AllFH.RemoveAll(x => !x.Intervenientes.ToLower().Contains(user.ToLower()));
                    //}
                    return AllFH;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<FolhaDeHorasViewModel> GetAllByHistorico(string NAVDatabaseName, string NAVCompanyName, string user)
        {
            try
            {
                //string regiao = "";
                //string area = "";
                //string cresp = "";

                ConfigUtilizadores CUser =DBUserConfigurations.GetById(user);
                //regiao = GetDimensionRegiao(NAVDatabaseName, NAVCompanyName, user);
                //area = GetDimensionArea(NAVDatabaseName, NAVCompanyName, user);
                //cresp = GetDimensionCresp(NAVDatabaseName, NAVCompanyName, user);
                //userName = DBUserConfigurations.GetById(user).Nome;

                using (var ctx = new SuchDBContext())
                {
                    List<FolhaDeHorasViewModel> AllFH = ctx.FolhasDeHoras.Where(x =>
                        //(x.Intervenientes.ToLower().Contains(user.ToLower())) &&
                        //(regiao.ToLower().Contains(x.CódigoRegião.ToLower()) || x.CódigoRegião == null) &&
                        //(area.ToLower().Contains(x.CódigoÁreaFuncional.ToLower()) || x.CódigoÁreaFuncional == null) &&
                        //(cresp.ToLower().Contains(x.CódigoCentroResponsabilidade.ToLower()) || x.CódigoCentroResponsabilidade == null) &&

                        (x.NºEmpregado == CUser.EmployeeNo ||
                        x.CriadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.TerminadoPor.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Validador.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRh.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.IntegradorEmRhKm.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.UtilizadorModificação.ToUpper() == CUser.IdUtilizador.ToUpper() ||
                        x.Intervenientes.ToUpper().Contains(" " + CUser.IdUtilizador.ToUpper())) &&

                        (x.Eliminada == false) &&
                        (x.Estado == 2) // 2 == REGISTADO
                    ).Select(FH => new FolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = FH.NºFolhaDeHoras,
                        Area = FH.Área,
                        AreaTexto = FH.Área == null ? "" : FH.Área.ToString(),
                        ProjetoNo = FH.NºProjeto,
                        ProjetoDescricao = FH.ProjetoDescricao,
                        EmpregadoNo = FH.NºEmpregado,
                        EmpregadoNome = FH.NomeEmpregado,
                        DataHoraPartida = FH.DataHoraPartida,
                        DataPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("yyyy-MM-dd"),
                        HoraPartidaTexto = FH.DataHoraPartida == null ? "" : FH.DataHoraPartida.Value.ToString("HH:mm:ss"),
                        DataHoraChegada = FH.DataHoraChegada,
                        DataChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("yyyy-MM-dd"),
                        HoraChegadaTexto = FH.DataHoraChegada == null ? "" : FH.DataHoraChegada.Value.ToString("HH:mm:ss"),
                        TipoDeslocacao = FH.TipoDeslocação,
                        TipoDeslocacaoTexto = FH.TipoDeslocação == null ? "" : FH.TipoDeslocação == null ? "" : FH.TipoDeslocação.ToString(),
                        CodigoTipoKms = FH.CódigoTipoKmS,
                        Matricula = FH.Matrícula,
                        DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                        DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho == null ? "" : FH.DeslocaçãoForaConcelho.ToString(),
                        DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                        DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada == null ? "" : FH.DeslocaçãoPlaneada.ToString(),
                        Terminada = FH.Terminada,
                        TerminadaTexto = FH.Terminada == null ? "" : FH.Terminada.ToString(),
                        Estado = FH.Estado,
                        Estadotexto = FH.Estado == null ? "" : FH.Estado.ToString(),
                        CriadoPor = FH.CriadoPor,
                        DataHoraCriacao = FH.DataHoraCriação,
                        DataCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        HoraCriacaoTexto = FH.DataHoraCriação == null ? "" : FH.DataHoraCriação.Value.ToString("HH:mm:ss"),
                        CodigoRegiao = FH.CódigoRegião,
                        CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                        CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                        TerminadoPor = FH.TerminadoPor,
                        DataHoraTerminado = FH.DataHoraTerminado,
                        DataTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("yyyy-MM-dd"),
                        HoraTerminadoTexto = FH.DataHoraTerminado == null ? "" : FH.DataHoraTerminado.Value.ToString("HH:mm:ss"),
                        Validado = FH.Validado,
                        ValidadoTexto = FH.Validado == null ? "" : FH.Validado.ToString(),
                        Validadores = FH.Validadores == null ? "" : FH.Validadores,
                        Validador = FH.Validador,
                        DataHoraValidacao = FH.DataHoraValidação,
                        DataValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("yyyy-MM-dd"),
                        HoraValidacaoTexto = FH.DataHoraValidação == null ? "" : FH.DataHoraValidação.Value.ToString("HH:mm:ss"),
                        IntegradoEmRh = FH.IntegradoEmRh,
                        IntegradorEmRH = FH.IntegradorEmRh,
                        IntegradoresEmRH = FH.IntegradoresEmRh,
                        DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                        DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh == null ? "" : FH.DataIntegraçãoEmRh.Value.ToString("HH:mm:ss"),
                        IntegradoEmRhKm = FH.IntegradoEmRhkm,
                        IntegradorEmRHKM = FH.IntegradorEmRhKm,
                        IntegradoresEmRHKM = FH.IntegradoresEmRhkm,
                        DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                        DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("yyyy-MM-dd"),
                        HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm == null ? "" : FH.DataIntegraçãoEmRhKm.Value.ToString("HH:mm:ss"),
                        CustoTotalAjudaCusto = Convert.ToDecimal(FH.CustoTotalAjudaCusto),
                        CustoTotalHoras = Convert.ToDecimal(FH.CustoTotalHoras),
                        CustoTotalKM = Convert.ToDecimal(FH.CustoTotalKm),
                        NumTotalKM = Convert.ToDecimal(FH.NumTotalKm),
                        Observacoes = FH.Observações,
                        Responsavel1No = FH.NºResponsável1,
                        Responsavel2No = FH.NºResponsável2,
                        Responsavel3No = FH.NºResponsável3,
                        ValidadoresRHKM = FH.ValidadoresRhKm,
                        DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                        DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("yyyy-MM-dd"),
                        HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado == null ? "" : FH.DataHoraÚltimoEstado.Value.ToString("HH:mm:ss"),
                        UtilizadorModificacao = FH.UtilizadorModificação,
                        DataHoraModificacao = FH.DataHoraModificação,
                        DataModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("yyyy-MM-dd"),
                        HoraModificacaoTexto = FH.DataHoraModificação == null ? "" : FH.DataHoraModificação.Value.ToString("HH:mm:ss"),
                        Eliminada = FH.Eliminada,
                        Intervenientes = FH.Intervenientes
                    }).ToList();

                    //ConfigUtilizadores ConfigUser = DBUserConfigurations.GetById(user);
                    //if (ConfigUser != null && ConfigUser.Administrador != true)
                    //{
                    //    AllFH.RemoveAll(x => !x.Intervenientes.ToLower().Contains(user.ToLower()));
                    //}
                    return AllFH;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static FolhaDeHorasViewModel GetListaValidadoresIntegradores(string FolhaHorasNo, string idEmployee, string NAV2009DatabaseName, string NAV2009CompanyName)
        {
            FolhasDeHoras FolhaHoras = new FolhasDeHoras();
            FolhaDeHorasViewModel FH = new FolhaDeHorasViewModel();

            if (!string.IsNullOrEmpty(FolhaHorasNo))
            {
                string idEmployeePortal = null;
                FolhaHoras = DBFolhasDeHoras.GetById(FolhaHorasNo);

                decimal CustoTotalHoras = FolhaHoras.CustoTotalHoras == null ? 0 : (decimal)FolhaHoras.CustoTotalHoras;
                decimal CustoTotalAjudaCusto = FolhaHoras.CustoTotalAjudaCusto == null ? 0 : (decimal)FolhaHoras.CustoTotalAjudaCusto;
                decimal CustoTotalKm = FolhaHoras.CustoTotalKm == null ? 0 : (decimal)FolhaHoras.CustoTotalKm;
                //decimal CustoTotal = CustoTotalHoras + CustoTotalAjudaCusto + CustoTotalKm;

                if (idEmployee != null && idEmployee != "")
                {
                    ConfigUtilizadores ConfUtilizadores = DBUserConfigurations.GetAll().Where(x => x.EmployeeNo == null ? "" == idEmployee.ToLower() : x.EmployeeNo.ToLower() == idEmployee.ToLower()).FirstOrDefault();

                    if (ConfUtilizadores == null)
                    {
                        ConfUtilizadores = DBUserConfigurations.GetAll().Where(x => x.IdUtilizador == null ? "" == idEmployee.ToLower() : x.IdUtilizador.ToLower() == idEmployee.ToLower()).FirstOrDefault();
                        if (ConfUtilizadores != null)
                        {
                            idEmployee = ConfUtilizadores.EmployeeNo == null ? "" : ConfUtilizadores.EmployeeNo;
                        }
                    }

                    if (ConfUtilizadores != null)
                    {
                        idEmployeePortal = ConfUtilizadores.IdUtilizador == null ? "" : ConfUtilizadores.IdUtilizador;
                    }
                    else
                    {
                        idEmployeePortal = "";
                    }

                    if (idEmployeePortal != null)
                    {
                        if (idEmployeePortal != "")
                        {
                            FH.CodigoRegiao = DBUserConfigurations.GetByEmployeeNo(idEmployee) == null ? "" : DBUserConfigurations.GetByEmployeeNo(idEmployee).RegiãoPorDefeito;
                            FH.CodigoAreaFuncional = DBUserConfigurations.GetByEmployeeNo(idEmployee) == null ? "" : DBUserConfigurations.GetByEmployeeNo(idEmployee).AreaPorDefeito;
                            FH.CodigoCentroResponsabilidade = DBUserConfigurations.GetByEmployeeNo(idEmployee) == null ? "" : DBUserConfigurations.GetByEmployeeNo(idEmployee).CentroRespPorDefeito;
                        }
                        else
                        {
                            NAVEmployeeViewModel Employee = DBNAV2009Employees.GetAll(idEmployee, NAV2009DatabaseName, NAV2009CompanyName).FirstOrDefault();
                            if (Employee != null)
                            {
                                FH.CodigoRegiao = Employee.Regiao;
                                FH.CodigoAreaFuncional = Employee.Area;
                                FH.CodigoCentroResponsabilidade = Employee.Cresp;
                            }
                        }

                        string Superior;

                        //GET LIST VALIDADORES
                        List<ConfiguraçãoAprovações> ApprovalConfigurationsValidadores = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensionsAndNivel(3, FH.CodigoAreaFuncional, FH.CodigoCentroResponsabilidade, FH.CodigoRegiao, CustoTotalAjudaCusto, DateTime.Now, 1);
                        ApprovalConfigurationsValidadores.RemoveAll(x => !x.NívelAprovação.HasValue || x.NívelAprovação < 1);

                        if (ApprovalConfigurationsValidadores.Count > 0)
                        {
                            //Create User ApprovalMovements
                            List<string> UsersToNotify = new List<string>();
                            var approvalConfiguration = ApprovalConfigurationsValidadores[0];
                            if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                            {
                                if (approvalConfiguration.UtilizadorAprovação.ToLower() == idEmployeePortal.ToLower())
                                {
                                    Superior = DBUserConfigurations.GetById(idEmployeePortal).SuperiorHierarquico;
                                    if (!string.IsNullOrEmpty(Superior))
                                        UsersToNotify.Add(Superior);
                                }
                                else
                                    UsersToNotify.Add(approvalConfiguration.UtilizadorAprovação);
                            }
                            else if (approvalConfiguration.GrupoAprovação.HasValue)
                            {
                                List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                                GUsers.ForEach(y =>
                                {
                                    if (y.ToLower() == idEmployeePortal.ToLower())
                                    {
                                        Superior = DBUserConfigurations.GetById(idEmployeePortal).SuperiorHierarquico;
                                        if (!string.IsNullOrEmpty(Superior))
                                            UsersToNotify.Add(Superior);
                                    }
                                    else
                                        UsersToNotify.Add(y);
                                });
                            }

                            //Notify Users
                            int indice = 1;
                            UsersToNotify = UsersToNotify.Distinct().ToList();
                            UsersToNotify.ForEach(user =>
                            {
                                if (indice == 1 && user != "")
                                    FH.Responsavel1No = user;
                                if (indice == 2 && user != "")
                                    FH.Responsavel2No = user;
                                if (indice == 3 && user != "")
                                    FH.Responsavel3No = user;
                                indice = indice + 1;

                                FH.Validadores = FH.Validadores + user + " - ";
                            });
                        }

                        //GET LIST INTEGRADORES EM RH
                        List<ConfiguraçãoAprovações> ApprovalConfigurationsIntegradoresEmRH = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensionsAndNivel(3, FH.CodigoAreaFuncional, FH.CodigoCentroResponsabilidade, FH.CodigoRegiao, CustoTotalAjudaCusto, DateTime.Now, 2);
                        ApprovalConfigurationsIntegradoresEmRH.RemoveAll(x => !x.NívelAprovação.HasValue || x.NívelAprovação < 2);

                        if (ApprovalConfigurationsIntegradoresEmRH.Count > 0)
                        {
                            //Create User ApprovalMovements
                            List<string> UsersToNotify = new List<string>();
                            var approvalConfiguration = ApprovalConfigurationsIntegradoresEmRH[0];
                            if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                            {
                                if (approvalConfiguration.UtilizadorAprovação.ToLower() == idEmployeePortal.ToLower())
                                {
                                    Superior = DBUserConfigurations.GetById(idEmployeePortal).SuperiorHierarquico;
                                    if (!string.IsNullOrEmpty(Superior))
                                        UsersToNotify.Add(Superior);
                                }
                                else
                                    UsersToNotify.Add(approvalConfiguration.UtilizadorAprovação);
                            }
                            else if (approvalConfiguration.GrupoAprovação.HasValue)
                            {
                                List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                                GUsers.ForEach(y =>
                                {
                                    if (y.ToLower() == idEmployeePortal.ToLower())
                                    {
                                        Superior = DBUserConfigurations.GetById(idEmployeePortal).SuperiorHierarquico;
                                        if (!string.IsNullOrEmpty(Superior))
                                            UsersToNotify.Add(Superior);
                                    }
                                    else
                                        UsersToNotify.Add(y);
                                });
                            }

                            //Notify Users
                            UsersToNotify = UsersToNotify.Distinct().ToList();
                            UsersToNotify.ForEach(user =>
                            {
                                FH.IntegradoresEmRH = FH.IntegradoresEmRH + user + " - ";
                            });
                        }

                        //GET LIST INTEGRADORES EM RH KM
                        List<ConfiguraçãoAprovações> ApprovalConfigurationsIntegradoresEmRHKM = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensionsAndNivel(3, FH.CodigoAreaFuncional, FH.CodigoCentroResponsabilidade, FH.CodigoRegiao, CustoTotalKm, DateTime.Now, 3);
                        ApprovalConfigurationsIntegradoresEmRHKM.RemoveAll(x => !x.NívelAprovação.HasValue || x.NívelAprovação < 3);

                        if (ApprovalConfigurationsIntegradoresEmRHKM.Count > 0)
                        {
                            //Create User ApprovalMovements
                            List<string> UsersToNotify = new List<string>();
                            var approvalConfiguration = ApprovalConfigurationsIntegradoresEmRHKM[0];
                            if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                            {
                                if (approvalConfiguration.UtilizadorAprovação.ToLower() == idEmployeePortal.ToLower())
                                {
                                    Superior = DBUserConfigurations.GetById(idEmployeePortal).SuperiorHierarquico;
                                    if (!string.IsNullOrEmpty(Superior))
                                        UsersToNotify.Add(Superior);
                                }
                                else
                                    UsersToNotify.Add(approvalConfiguration.UtilizadorAprovação);
                            }
                            else if (approvalConfiguration.GrupoAprovação.HasValue)
                            {
                                List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);

                                GUsers.ForEach(y =>
                                {
                                    if (y.ToLower() == idEmployeePortal.ToLower())
                                    {
                                        Superior = DBUserConfigurations.GetById(idEmployeePortal).SuperiorHierarquico;
                                        if (!string.IsNullOrEmpty(Superior))
                                            UsersToNotify.Add(Superior);
                                    }
                                    else
                                        UsersToNotify.Add(y);
                                });
                            }

                            //Notify Users
                            UsersToNotify = UsersToNotify.Distinct().ToList();
                            UsersToNotify.ForEach(user =>
                            {
                                FH.IntegradoresEmRHKM = FH.IntegradoresEmRHKM + user + " - ";
                            });
                        }
                    }
                }

                FH.Intervenientes = " CRIADOPOR: ";
                if (!string.IsNullOrEmpty(FH.CriadoPor))
                    FH.Intervenientes = FH.Intervenientes + FH.CriadoPor;

                FH.Intervenientes = FH.Intervenientes + " EMPREGADO: ";
                if (!string.IsNullOrEmpty(idEmployeePortal))
                    FH.Intervenientes = FH.Intervenientes + idEmployeePortal;

                FH.Intervenientes = FH.Intervenientes + " VALIDADORES: ";
                if (!string.IsNullOrEmpty(FH.Validadores))
                    FH.Intervenientes = FH.Intervenientes + FH.Validadores;

                FH.Intervenientes = FH.Intervenientes + " INTEGRADORESEMRH: ";
                if (!string.IsNullOrEmpty(FH.IntegradoresEmRH))
                    FH.Intervenientes = FH.Intervenientes + FH.IntegradoresEmRH;

                FH.Intervenientes = FH.Intervenientes + " INTEGRADORESEMRHKM: ";
                if (!string.IsNullOrEmpty(FH.IntegradoresEmRHKM))
                    FH.Intervenientes = FH.Intervenientes + FH.IntegradoresEmRHKM;
            }

            return FH;
        }

        public static FolhasDeHoras ParseToFolhaHoras(FolhaDeHorasViewModel FHViewModel)
        {
            try
            {
                if (FHViewModel != null)
                {
                    FolhasDeHoras FH = new FolhasDeHoras();

                    FH.NºFolhaDeHoras = FHViewModel.FolhaDeHorasNo;
                    FH.Área = FHViewModel.Area;
                    FH.NºProjeto = FHViewModel.ProjetoNo;
                    FH.ProjetoDescricao = FHViewModel.ProjetoDescricao;
                    FH.NºEmpregado = FHViewModel.EmpregadoNo;
                    FH.NomeEmpregado = FHViewModel.EmpregadoNome;
                    FH.DataHoraPartida = FHViewModel.DataHoraPartida;
                    FH.DataHoraChegada = FHViewModel.DataHoraChegada;
                    FH.TipoDeslocação = FHViewModel.TipoDeslocacao;
                    FH.CódigoTipoKmS = FHViewModel.CodigoTipoKms;
                    FH.Matrícula = FHViewModel.Matricula;
                    FH.DeslocaçãoForaConcelho = FHViewModel.DeslocacaoForaConcelho;
                    FH.DeslocaçãoPlaneada = FHViewModel.DeslocacaoPlaneada;
                    FH.Terminada = FHViewModel.Terminada;
                    FH.Estado = FHViewModel.Estado;
                    FH.CriadoPor = FHViewModel.CriadoPor;
                    FH.DataHoraCriação = FHViewModel.DataHoraCriacao;
                    FH.CódigoRegião = FHViewModel.CodigoRegiao;
                    FH.CódigoÁreaFuncional = FHViewModel.CodigoAreaFuncional;
                    FH.CódigoCentroResponsabilidade = FHViewModel.CodigoCentroResponsabilidade;
                    FH.TerminadoPor = FHViewModel.TerminadoPor;
                    FH.DataHoraTerminado = FHViewModel.DataHoraTerminado;
                    FH.Validado = FHViewModel.Validado;
                    FH.Validadores = FHViewModel.Validadores;
                    FH.Validador = FHViewModel.Validador;
                    FH.DataHoraValidação = FHViewModel.DataHoraValidacao;
                    FH.IntegradoEmRh = FHViewModel.IntegradoEmRh;
                    FH.IntegradoresEmRh = FHViewModel.IntegradoresEmRH;
                    FH.IntegradorEmRh = FHViewModel.IntegradorEmRH;
                    FH.DataIntegraçãoEmRh = FHViewModel.DataIntegracaoEmRH;
                    FH.IntegradoEmRhkm = FHViewModel.IntegradoEmRhKm;
                    FH.IntegradoresEmRhkm = FHViewModel.IntegradoresEmRHKM;
                    FH.IntegradorEmRhKm = FHViewModel.IntegradorEmRHKM;
                    FH.DataIntegraçãoEmRhKm = FHViewModel.DataIntegracaoEmRHKM;
                    FH.CustoTotalAjudaCusto = FHViewModel.CustoTotalAjudaCusto;
                    FH.CustoTotalHoras = FHViewModel.CustoTotalHoras;
                    FH.CustoTotalKm = FHViewModel.CustoTotalKM;
                    FH.NumTotalKm = FHViewModel.NumTotalKM;
                    FH.Observações = FHViewModel.Observacoes;
                    FH.NºResponsável1 = FHViewModel.Responsavel1No;
                    FH.NºResponsável2 = FHViewModel.Responsavel2No;
                    FH.NºResponsável3 = FHViewModel.Responsavel3No;
                    FH.ValidadoresRhKm = FHViewModel.ValidadoresRHKM;
                    FH.DataHoraÚltimoEstado = FHViewModel.DataHoraUltimoEstado;
                    FH.UtilizadorModificação = FHViewModel.UtilizadorModificacao;
                    FH.DataHoraModificação = FHViewModel.DataHoraModificacao;
                    FH.Eliminada = FHViewModel.Eliminada;
                    FH.Intervenientes = FHViewModel.Intervenientes;

                    return FH;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
