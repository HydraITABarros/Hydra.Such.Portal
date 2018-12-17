using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBProjectDiary
    {
        #region CRUD

        public static List<DiárioDeProjeto> GetAll(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                        return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado != true && x.PréRegisto ==false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
            public static List<DiárioDeProjeto> GetAllPreRegist(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                        return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado != true && x.PréRegisto ==true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DiárioDeProjeto> GetAllOpen(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado != true && x.NºProjetoNavigation.Estado != EstadoProjecto.Terminado && x.PréRegisto == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         public static List<DiárioDeProjeto> GetAllOpenPreRegist(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado != true && x.NºProjetoNavigation.Estado != EstadoProjecto.Terminado && x.PréRegisto == true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DiárioDeProjeto GetAllByCode(string user, string code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Código == code && x.Registado != true  && x.PréRegisto == false).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         public static DiárioDeProjeto GetAllPreRegByCode(string user, string code)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Código == code && x.Registado != true && x.PréRegisto == true).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DiárioDeProjeto> GetAllTable(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Faturada == false /*|| x.Faturada == null*/ && x.Faturável == true && x.Registado == true && x.Utilizador == user).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DiárioDeProjeto> GetAllTableByArea(string user, int areaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Faturada == false /*|| x.Faturada == null*/ && x.Faturável == true && x.Registado == true && x.Utilizador == user && x.NºProjetoNavigation.Área == areaId).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DiárioDeProjeto> GetNonInvoiced()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.Faturável == true && x.FaturaçãoAutorizada == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static DiárioDeProjeto Create(DiárioDeProjeto ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    if (!ObjectToCreate.Faturável.HasValue)
                        ObjectToCreate.Faturável = false;
                    ctx.DiárioDeProjeto.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static DiárioDeProjeto Update(DiárioDeProjeto ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioDeProjeto.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(DiárioDeProjeto ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.DiárioDeProjeto.Remove(ObjectToDelete);
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

        public static List<DiárioDeProjeto> GetByProjectNo(string ProjectNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<DiárioDeProjeto> GetByProjectNo(string ProjectNo, string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Utilizador == user && x.Registado != true && x.PréRegisto == false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         public static List<DiárioDeProjeto> GetPreRegistByProjectNo(string ProjectNo, string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Utilizador == user && x.Registado != true && x.PréRegisto == true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static List<DiárioDeProjeto> GetByLineNo(int LineNo, string user = "")
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (user == "")
                        return ctx.DiárioDeProjeto.Where(x => x.NºLinha == LineNo && x.PréRegisto ==false).ToList();

                    else
                        return ctx.DiárioDeProjeto.Where(x => x.NºLinha == LineNo && x.Utilizador == user && x.PréRegisto ==false).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<DiárioDeProjeto> GetPreRegistByLineNo(int LineNo, string user = "")
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (user == "")
                        return ctx.DiárioDeProjeto.Where(x => x.NºLinha == LineNo && x.PréRegisto ==true).ToList();

                    else
                        return ctx.DiárioDeProjeto.Where(x => x.NºLinha == LineNo && x.Utilizador == user && x.PréRegisto ==true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static List<DiárioDeProjeto> GetRegisteredDiary(string ProjectNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Registado == true).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<DiárioDeProjeto> GetRegisteredDiaryDp(string ProjectNo, string user, bool AllProjs)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (AllProjs)
                    {
                        return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado == true).ToList();
                    }
                    else
                    {
                       return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Utilizador == user && x.Registado == true).ToList();
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
                        totalConsumption = ctx.DiárioDeProjeto.Where(proj => proj.NºProjeto == projectNo &&
                                                                                proj.TipoMovimento == 1 &&
                                                                                proj.Registado.Value)
                                                              .Sum(total => total.CustoTotal);
                    }
                }
                catch { }
            }
            return totalConsumption.HasValue ? totalConsumption.Value : 0;
        }

        public static ProjectDiaryViewModel ParseToViewModel(DiárioDeProjeto x)
        {
            return new ProjectDiaryViewModel()
            {
                LineNo = x.NºLinha,
                ProjectNo = x.NºProjeto,
                Date = !x.Data.HasValue ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                MovementType = x.TipoMovimento,
                Type = x.Tipo,
                Code = x.Código,
                Description = x.Descrição,
                Quantity = x.Quantidade,
                MeasurementUnitCode = x.CódUnidadeMedida,
                LocationCode = x.CódLocalização,
                ProjectContabGroup = x.GrupoContabProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                User = x.Utilizador,
                UnitCost = x.CustoUnitário,
                TotalCost = x.CustoTotal,
                UnitPrice = x.PreçoUnitário,
                TotalPrice = x.PreçoTotal,
                Billable = x.Faturável,
                Registered = x.Registado,
                Billed = x.Faturada.HasValue ? x.Faturada.Value : false,
                Currency = x.Moeda,
                UnitValueToInvoice = x.ValorUnitárioAFaturar,
                MealType = x.TipoRefeição,
                ServiceGroupCode = x.CódGrupoServiço,
                ResidueGuideNo = x.NºGuiaResíduos,
                ExternalGuideNo = x.NºGuiaExterna,
                ConsumptionDate = !x.DataConsumo.HasValue ? "" : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                InvoiceToClientNo = x.FaturaANºCliente,
                ServiceClientCode = x.CódServiçoCliente,

                AdjustedPrice = x.AcertoDePreços,
                ResidueFinalDestinyCode = x.CódDestinoFinalResíduos,
                AutorizatedInvoiceData = x.DataAutorizaçãoFaturação.HasValue ? "" : x.DataAutorizaçãoFaturação.Value.ToString("yyyy-MM-dd"),
                AdjustedDocumentData = x.DataDocumentoCorrigido.HasValue ? "" : x.DataDocumentoCorrigido.Value.ToString("yyyy-MM-dd"),
                CreateDate = x.DataHoraCriação,
                AdjustedDocument = x.DocumentoCorrigido,
                OriginalDocument = x.DocumentoOriginal,
                AutorizatedInvoice = x.FaturaçãoAutorizada,
                Driver = x.Motorista,
                DocumentNo = x.NºDocumento,
                FolhaHoras = x.NºFolhaHoras,
                EmployeeNo = x.NºFuncionário,
                RequestLineNo = x.NºLinhaRequisição,
                RequestNo = x.NºRequisição,
                QuantityReturned = (decimal)x.QuantidadeDevolvida,
                InternalRequest = x.RequisiçãoInterna,
                ResourceType = x.TipoRecurso,
                CreateUser = x.UtilizadorCriação
            };
        }

        public static List<ProjectDiaryViewModel> ParseToViewModel(this List<DiárioDeProjeto> items)
        {
            List<ProjectDiaryViewModel> projectDiary = new List<ProjectDiaryViewModel>();
            if (items != null)
                items.ForEach(x =>
                    projectDiary.Add(ParseToViewModel(x)));
            return projectDiary;
        }

        public static DiárioDeProjeto ParseToDatabase(ProjectDiaryViewModel x)
        {
            return new DiárioDeProjeto()
            {
                NºLinha = x.LineNo,
                NºProjeto = x.ProjectNo,
                Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date),
                TipoMovimento = x.MovementType,
                Tipo = x.Type,
                Código = x.Code,
                Descrição = x.Description,
                Quantidade = x.Quantity,
                CódUnidadeMedida = x.MeasurementUnitCode,
                CódLocalização = x.LocationCode,
                GrupoContabProjeto = x.ProjectContabGroup,
                CódigoRegião = x.RegionCode,
                CódigoÁreaFuncional = x.FunctionalAreaCode,
                CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                Utilizador = x.User,
                CustoUnitário = x.UnitCost,
                CustoTotal = x.TotalCost,
                PreçoUnitário = x.UnitPrice,
                PreçoTotal = x.TotalPrice,
                Faturável = x.Billable,
                Registado = x.Registered,
                FaturaANºCliente = x.InvoiceToClientNo,
                Moeda = x.Currency,
                ValorUnitárioAFaturar = x.UnitValueToInvoice,
                TipoRefeição = x.MealType,
                CódGrupoServiço = x.ServiceGroupCode,
                NºGuiaResíduos = x.ResidueGuideNo,
                NºGuiaExterna = x.ExternalGuideNo,
                DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                CódServiçoCliente = x.ServiceClientCode,
                Faturada = x.Billed,
                DataHoraModificação = x.UpdateDate,
                UtilizadorModificação = x.UpdateUser,
                PréRegisto = x.PreRegistered,

                AcertoDePreços = x.AdjustedPrice,
                CódDestinoFinalResíduos = x.ResidueFinalDestinyCode,
                DataAutorizaçãoFaturação = x.AutorizatedInvoiceData == "" || x.AutorizatedInvoiceData == null ? (DateTime?)null : DateTime.Parse(x.AutorizatedInvoiceData),
                DataDocumentoCorrigido = x.AdjustedDocumentData == "" || x.AdjustedDocumentData == null ? (DateTime?)null : DateTime.Parse(x.AdjustedDocumentData),
                DataHoraCriação = x.CreateDate,
                DocumentoCorrigido = x.AdjustedDocument,
                DocumentoOriginal = x.OriginalDocument,
                FaturaçãoAutorizada = x.AutorizatedInvoice,
                Motorista = x.Driver,
                NºDocumento = x.DocumentNo,
                NºFolhaHoras = x.FolhaHoras,
                NºFuncionário = x.EmployeeNo,
                NºLinhaRequisição = x.RequestLineNo,
                NºRequisição = x.RequestNo,
                QuantidadeDevolvida = x.QuantityReturned,
                RequisiçãoInterna = x.InternalRequest,
                TipoRecurso = x.ResourceType,
                UtilizadorCriação = x.CreateUser
            };
        }

        public static List<DiárioDeProjeto> ParseToDatabase(this List<ProjectDiaryViewModel> items)
        {
            List<DiárioDeProjeto> projectDiary = new List<DiárioDeProjeto>();
            if (items != null)
                items.ForEach(x =>
                    projectDiary.Add(ParseToDatabase(x)));
            return projectDiary;
        }

    }
}
