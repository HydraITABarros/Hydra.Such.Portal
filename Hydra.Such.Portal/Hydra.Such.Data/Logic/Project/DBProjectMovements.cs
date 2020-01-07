using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using static Hydra.Such.Data.Enumerations;

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

        public static List<MovimentosDeProjeto> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.ToList();
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
                    return ctx.MovimentosDeProjeto.Where(x => x.Utilizador == user && x.Registado != true && x.NºProjetoNavigation.Estado != EstadoProjecto.Terminado).ToList();
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

        public static List<MovimentosDeProjeto> GetProjMovementsById(string projectNo, int? ProjGroup, bool Faturada = false)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                        return ctx.MovimentosDeProjeto
                            .Where(x => x.NºProjeto == projectNo &&
                                    (x.Faturada == Faturada) && //Consumo
                                    x.Faturável == true &&
                                    x.GrupoFatura == ProjGroup &&
                                    x.FaturaçãoAutorizada == true)
                            .ToList();
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

        public static List<MovimentosDeProjeto> GetMovementProjectByGroupProj(int grupo, string nProjecto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.Faturável == true && x.FaturaçãoAutorizada == true && x.GrupoFatura == grupo && x.NºProjeto== nProjecto).ToList();
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

        public static bool Create(List<MovimentosDeProjeto> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (items != null)
                    {
                        items.ForEach(x => x.DataHoraCriação = DateTime.Now);
                    }
                    ctx.MovimentosDeProjeto.AddRange(items);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        public static List<MovimentosDeProjeto> GetRegisteredDiaryByDate(string ProjectNo, DateTime date)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Data >= date).ToList();
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
                        item.MealType= temp.TipoRefeicao.Equals(DBNull.Value) ? null : (int?)temp.TipoRefeicao;
                        item.InvoiceGroup = temp.GrupoFatura.Equals(DBNull.Value) ? null : (int?)temp.GrupoFatura;
                        item.InvoiceGroupDescription= temp.GrupoFaturaDescricao.Equals(DBNull.Value) ? "" : (string)temp.GrupoFaturaDescricao;
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
                ProjectMovementViewModel projMovement = new ProjectMovementViewModel();

                projMovement.LineNo = item.NºLinha;
                projMovement.ProjectNo = item.NºProjeto;
                projMovement.Date = item.Data == null ? String.Empty : item.Data.Value.ToString("yyyy-MM-dd");
                projMovement.MovementType = item.TipoMovimento;
                projMovement.DocumentNo = item.NºDocumento;
                projMovement.Type = item.Tipo;
                //TypeDescription
                projMovement.Code = item.Código;
                projMovement.Description = item.Descrição;
                projMovement.CodigoTipoTrabalho = item.CodigoTipoTrabalho;
                projMovement.Quantity = item.Quantidade;
                projMovement.MeasurementUnitCode = item.CódUnidadeMedida;
                projMovement.LocationCode = item.CódLocalização;
                projMovement.ProjectContabGroup = item.GrupoContabProjeto;
                projMovement.RegionCode = item.CódigoRegião;
                projMovement.FunctionalAreaCode = item.CódigoÁreaFuncional;
                projMovement.ResponsabilityCenterCode = item.CódigoCentroResponsabilidade;
                projMovement.User = item.Utilizador;
                projMovement.UnitCost = item.CustoUnitário;
                projMovement.TotalCost = item.CustoTotal;
                projMovement.UnitPrice = item.PreçoUnitário;
                projMovement.TotalPrice = item.PreçoTotal;
                projMovement.UnitValueToInvoice = item.ValorUnitárioAFaturar;
                projMovement.Currency = item.Moeda;
                projMovement.Billable = item.Faturável.HasValue ? item.Faturável.Value : false;
                projMovement.Billed = item.Faturada.HasValue ? item.Faturada.Value : false;
                projMovement.Registered = item.Registado.HasValue ? item.Registado.Value : false;
                projMovement.ResourceType = item.TipoRecurso;
                projMovement.ServiceClientCode = item.CódServiçoCliente;
                //ServiceClientDescription
                projMovement.ServiceGroupCode = item.CódGrupoServiço;
                projMovement.ExternalGuideNo = item.NºGuiaExterna;
                projMovement.ConsumptionDate = item.DataConsumo?.ToString("yyyy-MM-dd");
                projMovement.ResidueGuideNo = item.NºGuiaResíduos;
                projMovement.AdjustedDocument = item.DocumentoCorrigido;
                projMovement.AdjustedDocumentDate = item.DataDocumentoCorrigido?.ToString("yyyy-MM-dd");
                projMovement.ResidueFinalDestinyCode = item.CódDestinoFinalResíduos;
                projMovement.MealType = item.TipoRefeição;
                //MealTypeDescription
                projMovement.InvoiceToClientNo = item.FaturaANºCliente;
                projMovement.CreateUser = item.UtilizadorCriação;
                projMovement.CreateDate = item.DataHoraCriação;
                projMovement.UpdateUser = item.UtilizadorModificação;
                projMovement.UpdateDate = item.DataHoraModificação;
                //ServiceData = item;
                //ClientRequest = item;
                projMovement.RequestNo = item.NºRequisição;
                projMovement.RequestLineNo = item.NºLinhaRequisição;
                projMovement.Driver = item.Motorista;
                projMovement.OriginalDocument = item.DocumentoOriginal;
                projMovement.AdjustedPrice = item.AcertoDePreços;
                projMovement.AutorizatedInvoice = item.FaturaçãoAutorizada;
                projMovement.AutorizatedInvoice2 = item.FaturaçãoAutorizada2;
                projMovement.AutorizatedInvoiceDate = item.DataAutorizaçãoFaturação?.ToString("yyyy-MM-dd");
                projMovement.AuthorizedBy = item.AutorizadoPor;
                projMovement.TimesheetNo = item.NºFolhaHoras;
                projMovement.InternalRequest = item.RequisiçãoInterna;
                projMovement.EmployeeNo = item.NºFuncionário;
                projMovement.QuantityReturned = item.QuantidadeDevolvida;
                projMovement.CustomerNo = item.CodCliente;
                projMovement.LicensePlate = item.Matricula;
                projMovement.ReadingCode = item.CodigoLer;
                projMovement.Group = item.Grupo;
                projMovement.Operation = item.Operacao;
                projMovement.InvoiceGroup = item.GrupoFatura;
                projMovement.InvoiceGroupDescription = item.GrupoFaturaDescricao;
                //CommitmentNumber = Project.DBProjects.GetAllByProjectNumber(item.NºProjeto).NºCompromisso,
                projMovement.ClientName = DBNAV2017Clients.GetClientNameByNo(item.FaturaANºCliente, navDatabaseName, navCompanyName);
                projMovement.ClientVATReg = DBNAV2017Clients.GetClientVATByNo(item.FaturaANºCliente, navDatabaseName, navCompanyName);
                projMovement.CriarMovNav2017 = item.CriarMovNav2017;
                projMovement.Selecionada = item.Selecionada;
                projMovement.Fatura = item.Fatura;

                return projMovement;
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
                MovimentosDeProjeto projMovement = new MovimentosDeProjeto();

                projMovement.NºLinha = item.LineNo;
                projMovement.NºProjeto = item.ProjectNo;
                projMovement.Data = string.IsNullOrEmpty(item.Date) ? (DateTime?)null : DateTime.Parse(item.Date);
                projMovement.TipoMovimento = item.MovementType;
                projMovement.NºDocumento = item.DocumentNo;
                projMovement.Tipo = item.Type;
                //TypeDescription
                projMovement.Código = item.Code;
                projMovement.Descrição = item.Description;
                projMovement.CodigoTipoTrabalho = item.CodigoTipoTrabalho;
                projMovement.Quantidade = item.Quantity;
                projMovement.CódUnidadeMedida = item.MeasurementUnitCode;
                projMovement.CódLocalização = item.LocationCode;
                projMovement.GrupoContabProjeto = item.ProjectContabGroup;
                projMovement.CódigoRegião = item.RegionCode;
                projMovement.CódigoÁreaFuncional = item.FunctionalAreaCode;
                projMovement.CódigoCentroResponsabilidade = item.ResponsabilityCenterCode;
                projMovement.Utilizador = item.User;
                projMovement.CustoUnitário = item.UnitCost;
                projMovement.CustoTotal = item.TotalCost;
                projMovement.PreçoUnitário = item.UnitPrice;
                projMovement.PreçoTotal = item.TotalPrice;
                projMovement.ValorUnitárioAFaturar = item.UnitValueToInvoice;
                projMovement.Moeda = item.Currency;
                projMovement.Faturável = item.Billable.HasValue ? item.Billable.Value : false;
                projMovement.Faturada = item.Billed;
                projMovement.Registado = item.Registered.HasValue ? item.Registered.Value : false;
                projMovement.TipoRecurso = item.ResourceType;
                projMovement.CódServiçoCliente = item.ServiceClientCode;
                //ServiceClientDescription
                projMovement.CódGrupoServiço = item.ServiceGroupCode;
                projMovement.NºGuiaExterna = item.ExternalGuideNo;
                projMovement.DataConsumo = string.IsNullOrEmpty(item.ConsumptionDate) ? (DateTime?)null : DateTime.Parse(item.ConsumptionDate);
                projMovement.NºGuiaResíduos = item.ResidueGuideNo;
                projMovement.DocumentoCorrigido = item.AdjustedDocument;
                projMovement.DataDocumentoCorrigido = string.IsNullOrEmpty(item.AdjustedDocumentDate) ? (DateTime?)null : DateTime.Parse(item.AdjustedDocumentDate);
                projMovement.CódDestinoFinalResíduos = item.ResidueFinalDestinyCode;
                projMovement.TipoRefeição = item.MealType;
                //MealTypeDescription
                projMovement.FaturaANºCliente = item.InvoiceToClientNo;
                projMovement.UtilizadorCriação = item.CreateUser;
                projMovement.DataHoraCriação = item.CreateDate;
                projMovement.UtilizadorModificação = item.UpdateUser;
                projMovement.DataHoraModificação = item.UpdateDate;
                //ServiceData = item;
                //ClientRequest = item;
                projMovement.NºRequisição = item.RequestNo;
                projMovement.NºLinhaRequisição = item.RequestLineNo;
                projMovement.Motorista = item.Driver;
                projMovement.DocumentoOriginal = item.OriginalDocument;
                projMovement.AcertoDePreços = item.AdjustedPrice;
                projMovement.FaturaçãoAutorizada = item.AutorizatedInvoice;
                projMovement.FaturaçãoAutorizada2 = item.AutorizatedInvoice2;
                projMovement.DataAutorizaçãoFaturação = string.IsNullOrEmpty(item.AutorizatedInvoiceDate) ? (DateTime?)null : DateTime.Parse(item.AutorizatedInvoiceDate);
                projMovement.AutorizadoPor = item.AuthorizedBy;
                projMovement.NºFolhaHoras = item.TimesheetNo;
                projMovement.RequisiçãoInterna = item.InternalRequest;
                projMovement.NºFuncionário = item.EmployeeNo;
                projMovement.QuantidadeDevolvida = item.QuantityReturned;
                projMovement.CodCliente = item.CustomerNo;
                projMovement.Matricula = item.LicensePlate;
                projMovement.CodigoLer = item.ReadingCode;
                projMovement.Grupo = item.Group;
                projMovement.Operacao = item.Operation;
                projMovement.GrupoFatura = item.InvoiceGroup;
                projMovement.GrupoFaturaDescricao = item.InvoiceGroupDescription;
                projMovement.CriarMovNav2017 = item.CriarMovNav2017;
                projMovement.Selecionada = item.Selecionada;
                projMovement.Fatura = item.Fatura;

                return projMovement;
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
