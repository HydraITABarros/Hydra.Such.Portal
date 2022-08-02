﻿using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Extensions
{
    public static class DBModelExtensions
    {
        public static ProjectDiaryViewModel ParseToProjectDiary(this LinhasContratos x, string projectNo, string userName, string date, string customerServiceId, string serviceGroupId)
        {
            ProjectDiaryViewModel item = new ProjectDiaryViewModel();
            item.ProjectNo = projectNo;
            item.Code = x.Código;
            item.Description = x.Descrição;
            item.Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2) : Math.Round((decimal)0, 2); //A pedido Marco Marcelo 03/07/2019 = 0;
            item.MeasurementUnitCode = x.CódUnidadeMedida;
            item.RegionCode = x.CódigoRegião;
            item.FunctionalAreaCode = x.CódigoÁreaFuncional;
            item.ResponsabilityCenterCode = x.CódigoCentroResponsabilidade;
            item.User = userName;
            item.UnitPrice = Math.Round((decimal)x.PreçoUnitário, 4);
            item.TotalPrice = x.Quantidade.HasValue ? Math.Round(Math.Round((decimal)x.Quantidade, 2) * Math.Round((decimal)x.PreçoUnitário, 4), 2) : Math.Round((decimal)0, 2);
            item.Billable = x.Faturável;
            item.Registered = false;
            item.Date = string.IsNullOrEmpty(date) ? "" : date;
            item.ServiceClientCode = (x.CódServiçoCliente != "" && x.CódServiçoCliente != null) ? x.CódServiçoCliente : customerServiceId;
            item.ServiceGroupCode = serviceGroupId;
            item.PreRegistered = false;
            item.MovementType = 1;
            //switch (x.Tipo.Value)
            //{
            //    case 1: item.Type = 2; break;
            //    case 2: item.Type = 1; break;
            //    default: item.Type = x.Tipo; break;
            //}
            item.Type = x.Tipo;
            return item;
        }

        public static List<ProjectDiaryViewModel> ParseToViewModel(this List<LinhasContratos> items, string projectNo, string userName, string date, string customerServiceId, string serviceGroupId)
        {
            List<ProjectDiaryViewModel> projectDiary = new List<ProjectDiaryViewModel>();
            if (items != null)
                items.ForEach(x =>
                    projectDiary.Add(x.ParseToProjectDiary(projectNo, userName, date, customerServiceId, serviceGroupId)));
            return projectDiary;
        }
    }
}
