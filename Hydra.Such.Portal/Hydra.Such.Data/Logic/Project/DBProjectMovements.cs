using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Such.Data.Logic.ProjectMovements
{
    public static class DBProjectMovements
    {
        public static List<MovimentosDeProjeto> GetAll(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.Utilizador == user && x.Registado != true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MovimentosDeProjeto> GetAllOpen(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.Utilizador == user && x.Registado != true && x.NºProjetoNavigation.Estado != 4 && x.NºProjetoNavigation.Estado != 5).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MovimentosDeProjeto GetAllByCode(string user, string code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.Utilizador == user && x.Código == code && x.Registado != true).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MovimentosDeProjeto> GetAllTable(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.Faturada == false /*|| x.Faturada == null*/ && x.Faturável == true && x.Registado == true && x.Utilizador == user).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MovimentosDeProjeto> GetProjectMovementsFor(string projectNo, bool? billable)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (billable.HasValue)
                    {
                        return ctx.MovimentosDeProjeto
                            .Where(x => x.NºProjeto == projectNo &&
                                    x.TipoMovimento == 1 && //Consumo
                                    x.Faturável == billable &&
                                    x.FaturaçãoAutorizada == false)
                            .ToList();
                    }
                    else
                    {
                        return ctx.MovimentosDeProjeto
                           .Where(x => x.NºProjeto == projectNo &&
                                    x.TipoMovimento == 1 && //Consumo
                                    x.FaturaçãoAutorizada == false)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MovimentosDeProjeto> GetNonInvoiced()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.Faturável == true && x.FaturaçãoAutorizada == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static MovimentosDeProjeto Create(MovimentosDeProjeto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.MovimentosDeProjeto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static MovimentosDeProjeto Update(MovimentosDeProjeto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosDeProjeto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<MovimentosDeProjeto> Update(List<MovimentosDeProjeto> projectMovements)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosDeProjeto.UpdateRange(projectMovements);
                    ctx.SaveChanges();
                }
                return projectMovements;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(MovimentosDeProjeto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosDeProjeto.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<MovimentosDeProjeto> GetByProjectNo(string ProjectNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.NºProjeto == ProjectNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MovimentosDeProjeto> GetByProjectNo(string ProjectNo, string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Utilizador == user && x.Registado != true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static List<MovimentosDeProjeto> GetByLineNo(int LineNo, string user = "")
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (user == "")
                        return ctx.MovimentosDeProjeto.Where(x => x.NºLinha == LineNo).ToList();

                    else
                        return ctx.MovimentosDeProjeto.Where(x => x.NºLinha == LineNo && x.Utilizador == user).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static List<MovimentosDeProjeto> GetRegisteredDiary(string ProjectNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.NºProjeto == ProjectNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<MovimentosDeProjeto> GetRegisteredDiaryDp(string ProjectNo, string user, bool AllProjs)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (AllProjs)
                    {
                        return ctx.MovimentosDeProjeto.ToList();//.Where(x => x.Utilizador == user && x.Registado == true)
                    }
                    else
                    {
                        return ctx.MovimentosDeProjeto.Where(x => x.NºProjeto == ProjectNo).ToList();// && x.Utilizador == user && x.Registado == true
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static decimal GetProjectTotaConsumption(string projectNo)
        {
            decimal? totalConsumption = null;
            if (!string.IsNullOrEmpty(projectNo))
            {
                try
                {
                    using (var ctx = new SuchDBContext())
                    {
                        totalConsumption = ctx.MovimentosDeProjeto.Where(proj => proj.NºProjeto == projectNo &&
                                                                                proj.TipoMovimento == 1 &&
                                                                                proj.Registado.Value)
                                                              .Sum(total => total.CustoTotal);
                    }
                }
                catch { }
            }
            return totalConsumption.HasValue ? totalConsumption.Value : 0;
        }


        public static List<SPInvoiceListViewModel> GetAllAutorized()
        {
            try
            {
                List<SPInvoiceListViewModel> result = new List<SPInvoiceListViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {

                    var parameters = new[]{
                       new SqlParameter("@Autorization", 1),
                       new SqlParameter("@Invoiced", 0)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ProjectMovemmentsInvoiceGrid @Autorization, @Invoiced", parameters);

                    foreach (dynamic temp in data)
                    {
                        SPInvoiceListViewModel item = new SPInvoiceListViewModel();
                        item.ClientRequest = temp.PedidodoCliente.Equals(DBNull.Value) ? "" : (string)temp.PedidodoCliente;
                        item.InvoiceToClientNo = temp.FaturaNoCliente.Equals(DBNull.Value) ? "" : (string)temp.FaturaNoCliente;
                        item.CommitmentNumber = temp.NoCompromisso.Equals(DBNull.Value) ? "" : (string)temp.NoCompromisso;
                        item.ProjectNo = temp.NoProjeto.Equals(DBNull.Value) ? "" : (string)temp.NoProjeto;
                        item.Date = temp.Data.Equals(DBNull.Value) ? "" : (string)temp.Data.ToString("yyyy-MM-dd");
                        item.LineNo = (int)temp.NoLinha;
                        item.MovementType = temp.TipoMovimento.Equals(DBNull.Value) ? null : (int?)temp.TipoMovimento;
                        item.Type = temp.Tipo.Equals(DBNull.Value) ? null : (int?)temp.Tipo;
                        item.Code = temp.Código.Equals(DBNull.Value) ? "" : (string)temp.Código;
                        item.Description = temp.Descrição.Equals(DBNull.Value) ? "" : (string)temp.Descrição;
                        item.MeasurementUnitCode = temp.CodUnidadeMedida.Equals(DBNull.Value) ? "" : (string)temp.CodUnidadeMedida;
                        item.Quantity = temp.Quantidade.Equals(DBNull.Value) ? null : (decimal?)temp.Quantidade;
                        item.LocationCode = temp.CodLocalizacao.Equals(DBNull.Value) ? "" : (string)temp.CodLocalizacao;
                        item.ProjectContabGroup = temp.GrupoContabProjeto.Equals(DBNull.Value) ? "" : (string)temp.GrupoContabProjeto;
                        item.RegionCode = temp.CodigoRegiao.Equals(DBNull.Value) ? "" : (string)temp.CodigoRegiao;
                        item.FunctionalAreaCode = temp.CodAreaFuncional.Equals(DBNull.Value) ? "" : (string)temp.CodAreaFuncional;
                        item.ResponsabilityCenterCode = temp.CodCentroResponsabilidade.Equals(DBNull.Value) ? "" : (string)temp.CodCentroResponsabilidade;
                        item.User = temp.Utilizador.Equals(DBNull.Value) ? "" : (string)temp.Utilizador;
                        item.UnitCost = temp.CustoUnitario.Equals(DBNull.Value) ? null : (decimal?)temp.CustoUnitario;
                        item.TotalCost = temp.CustoTotal.Equals(DBNull.Value) ? null : (decimal?)temp.CustoTotal;
                        item.UnitPrice = temp.PrecoUnitario.Equals(DBNull.Value) ? null : (decimal?)temp.PrecoUnitario;
                        item.TotalPrice = temp.PrecoTotal.Equals(DBNull.Value) ? null : (decimal?)temp.PrecoTotal;
                        item.Billable = temp.Faturável.Equals(DBNull.Value) ? null : (bool?)temp.Faturável;
                        item.AutorizatedInvoice = temp.FaturacaoAutorizada.Equals(DBNull.Value) ? null : (bool?)temp.FaturacaoAutorizada;
                        item.AutorizatedInvoiceData = temp.DataAutorizacaoFaturacao.Equals(DBNull.Value) ? "" : (string)temp.DataAutorizacaoFaturacao.ToString("yyyy-MM-dd");
                        item.ConsumptionDate = temp.DataConsumo.Equals(DBNull.Value) ? "" : (string)temp.DataConsumo.ToString("yyyy-MM-dd");
                        item.CreateDate = temp.DataHoraCriacao.Equals(DBNull.Value) ? null : (DateTime?)temp.DataHoraCriacao;
                        item.CreateUser = temp.UtilizadorCriacao.Equals(DBNull.Value) ? "" : (string)temp.UtilizadorCriacao;
                        item.Registered = temp.Registado.Equals(DBNull.Value) ? null : (bool?)temp.Registado;
                        item.Billed = temp.Faturada.Equals(DBNull.Value) ? null : (bool?)temp.Faturada;

                        result.Add(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Parse Utilities
        public static ProjectMovementViewModel ParseToViewModel(this MovimentosDeProjeto item, string navDatabaseName, string navCompanyName)
        {
            if (item != null)
            {
                return new ProjectMovementViewModel()
                {
                    LineNo = item.NºLinha,
                    ProjectNo = item.NºProjeto,
                    Date = item.Data == null ? String.Empty : item.Data.Value.ToString("yyyy-MM-dd"),
                    MovementType = item.TipoMovimento,
                    DocumentNo = item.NºDocumento,
                    Type = item.Tipo,
                    //TypeDescription
                    Code = item.Código,
                    Description = item.Descrição,
                    Quantity = item.Quantidade,
                    MeasurementUnitCode = item.CódUnidadeMedida,
                    LocationCode = item.CódLocalização,
                    ProjectContabGroup = item.GrupoContabProjeto,
                    RegionCode = item.CódigoRegião,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = item.CódigoCentroResponsabilidade,
                    User = item.Utilizador,
                    UnitCost = item.CustoUnitário,
                    TotalCost = item.CustoTotal,
                    UnitPrice = item.PreçoUnitário,
                    TotalPrice = item.PreçoTotal,
                    UnitValueToInvoice = item.ValorUnitárioAFaturar,
                    Currency = item.Moeda,
                    Billable = item.Faturável.HasValue ? item.Faturável.Value : false,
                    Billed = item.Faturada.HasValue ? item.Faturada.Value : false,
                    Registered = item.Registado.HasValue ? item.Registado.Value : false,
                    ResourceType = item.TipoRecurso,
                    ServiceClientCode = item.CódServiçoCliente,
                    //ServiceClientDescription
                    ServiceGroupCode = item.CódGrupoServiço,
                    ExternalGuideNo = item.NºGuiaExterna,
                    ConsumptionDate = item.DataConsumo?.ToString("yyyy-MM-dd"),
                    ResidueGuideNo = item.NºGuiaResíduos,
                    AdjustedDocument = item.DocumentoCorrigido,
                    AdjustedDocumentDate = item.DataDocumentoCorrigido?.ToString("yyyy-MM-dd"),
                    ResidueFinalDestinyCode = item.CódDestinoFinalResíduos,
                    MealType = item.TipoRefeição,
                    //MealTypeDescription
                    InvoiceToClientNo = item.FaturaANºCliente,
                    CreateUser = item.UtilizadorCriação,
                    CreateDate = item.DataHoraCriação,
                    UpdateUser = item.UtilizadorModificação,
                    UpdateDate = item.DataHoraModificação,
                    //ServiceData = item,
                    //ClientRequest = item,
                    RequestNo = item.NºRequisição,
                    RequestLineNo = item.NºLinhaRequisição,
                    Driver = item.Motorista,
                    OriginalDocument = item.DocumentoOriginal,
                    AdjustedPrice = item.AcertoDePreços,
                    AutorizatedInvoice = item.FaturaçãoAutorizada,
                    AutorizatedInvoice2 = item.FaturaçãoAutorizada2,
                    AutorizatedInvoiceDate = item.DataAutorizaçãoFaturação?.ToString("yyyy-MM-dd"),
                    TimesheetNo = item.NºFolhaHoras,
                    InternalRequest = item.RequisiçãoInterna,
                    EmployeeNo = item.NºFuncionário,
                    QuantityReturned = item.QuantidadeDevolvida,
                    CustomerNo = item.CodCliente,
                    LicensePlate = item.Matricula,
                    ReadingCode = item.CodigoLer,
                    Group = item.Grupo,
                    Operation = item.Operacao,
                    InvoiceGroup = item.GrupoFatura,
                    InvoiceGroupDescription = item.GrupoFaturaDescricao,
                    CommitmentNumber = Project.DBProjects.GetAllByProjectNumber(item.NºProjeto).NºCompromisso,
                    ClientName = DBNAV2017Clients.GetClientNameByNo(item.FaturaANºCliente, navDatabaseName, navCompanyName),
                    ClientVATReg = DBNAV2017Clients.GetClientVATByNo(item.FaturaANºCliente, navDatabaseName, navCompanyName)
                };
            }
            return null;
        }

        public static List<ProjectMovementViewModel> ParseToViewModel(this List<MovimentosDeProjeto> items, string navDatabaseName, string navCompanyName)
        {
            List<ProjectMovementViewModel> parsedItems = new List<ProjectMovementViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel(navDatabaseName, navCompanyName)));
            return parsedItems;
        }

        public static MovimentosDeProjeto ParseToDB(this ProjectMovementViewModel item)
        {
            if (item != null)
            {
                return new MovimentosDeProjeto()
                {
                    NºLinha = item.LineNo,
                    NºProjeto = item.ProjectNo,
                    Data = string.IsNullOrEmpty(item.Date) ? (DateTime?)null : DateTime.Parse(item.Date),
                    TipoMovimento = item.MovementType,
                    NºDocumento = item.DocumentNo,
                    Tipo = item.Type,
                    //TypeDescription
                    Código = item.Code,
                    Descrição = item.Description,
                    Quantidade = item.Quantity,
                    CódUnidadeMedida = item.MeasurementUnitCode,
                    CódLocalização = item.LocationCode,
                    GrupoContabProjeto = item.ProjectContabGroup,
                    CódigoRegião = item.RegionCode,
                    CódigoÁreaFuncional = item.FunctionalAreaCode,
                    CódigoCentroResponsabilidade = item.ResponsabilityCenterCode,
                    Utilizador = item.User,
                    CustoUnitário = item.UnitCost,
                    CustoTotal = item.TotalCost,
                    PreçoUnitário = item.UnitPrice,
                    PreçoTotal = item.TotalPrice ,
                    ValorUnitárioAFaturar = item.UnitValueToInvoice,
                    Moeda = item.Currency ,
                    Faturável = item.Billable.HasValue ? item.Billable.Value : false,
                    Faturada = item.Billed,
                    Registado = item.Registered.HasValue ? item.Registered.Value : false,
                    TipoRecurso = item.ResourceType,
                    CódServiçoCliente = item.ServiceClientCode,
                    //ServiceClientDescription
                    CódGrupoServiço = item.ServiceGroupCode,
                    NºGuiaExterna = item.ExternalGuideNo,
                    DataConsumo = string.IsNullOrEmpty(item.ConsumptionDate) ? (DateTime?)null : DateTime.Parse(item.ConsumptionDate),
                    NºGuiaResíduos = item.ResidueGuideNo,
                    DocumentoCorrigido = item.AdjustedDocument,
                    DataDocumentoCorrigido = string.IsNullOrEmpty(item.AdjustedDocumentDate) ? (DateTime?)null : DateTime.Parse(item.AdjustedDocumentDate),
                    CódDestinoFinalResíduos = item.ResidueFinalDestinyCode,
                    TipoRefeição = item.MealType,
                    //MealTypeDescription
                    FaturaANºCliente = item.InvoiceToClientNo,
                    UtilizadorCriação = item.CreateUser,
                    DataHoraCriação = item.CreateDate,
                    UtilizadorModificação = item.UpdateUser,
                    DataHoraModificação = item.UpdateDate,
                    //ServiceData = item,
                    //ClientRequest = item,
                    NºRequisição = item.RequestNo,
                    NºLinhaRequisição = item.RequestLineNo,
                    Motorista = item.Driver,
                    DocumentoOriginal = item.OriginalDocument,
                    AcertoDePreços = item.AdjustedPrice,
                    FaturaçãoAutorizada = item.AutorizatedInvoice,
                    FaturaçãoAutorizada2 = item.AutorizatedInvoice2,
                    DataAutorizaçãoFaturação = string.IsNullOrEmpty(item.AutorizatedInvoiceDate) ? (DateTime?)null : DateTime.Parse(item.AutorizatedInvoiceDate),
                    NºFolhaHoras = item.TimesheetNo,
                    RequisiçãoInterna = item.InternalRequest,
                    NºFuncionário = item.EmployeeNo,
                    QuantidadeDevolvida = item.QuantityReturned,
                    CodCliente = item.CustomerNo,
                    Matricula = item.LicensePlate,
                    CodigoLer = item.ReadingCode,
                    Grupo = item.Group,
                    Operacao = item.Operation,
                    GrupoFatura = item.InvoiceGroup,
                    GrupoFaturaDescricao = item.InvoiceGroupDescription,
                };
            }
            return null;
        }

        public static List<MovimentosDeProjeto> ParseToDB(this List<ProjectMovementViewModel> items)
        {
            List<MovimentosDeProjeto> parsedItems = new List<MovimentosDeProjeto>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
