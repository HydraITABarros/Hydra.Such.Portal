﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;

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
                        return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado != true).ToList();
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
                    return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Registado != true && x.NºProjetoNavigation.Estado != 4 && x.NºProjetoNavigation.Estado != 5).ToList();
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
                    return ctx.DiárioDeProjeto.Where(x => x.Utilizador == user && x.Código == code && x.Registado != true).FirstOrDefault();
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
                    return ctx.DiárioDeProjeto.Where(x => x.NºProjeto == ProjectNo && x.Utilizador == user && x.Registado != true).ToList();
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
                        return ctx.DiárioDeProjeto.Where(x => x.NºLinha == LineNo).ToList();

                    else
                        return ctx.DiárioDeProjeto.Where(x => x.NºLinha == LineNo && x.Utilizador == user).ToList();
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
                ServiceClientCode = x.CódServiçoCliente
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
