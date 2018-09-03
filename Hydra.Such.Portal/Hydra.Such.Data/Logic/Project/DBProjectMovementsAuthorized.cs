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
    public static class DBProjectMovementsAuthorized
    {
        public static List<MovimentosProjectoAutorizados> GetAll(string user)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosProjectoAutorizados.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public static MovimentosProjectoAutorizados GetAllByNum(int numMov)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentosProjectoAutorizados.Where(x => x.NumMovimento == numMov).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public static MovimentosProjectoAutorizados Create(MovimentosProjectoAutorizados ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosProjectoAutorizados.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static MovimentosProjectoAutorizados Update(MovimentosProjectoAutorizados ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosProjectoAutorizados.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<MovimentosProjectoAutorizados> Update(List<MovimentosProjectoAutorizados> projectMovements)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosProjectoAutorizados.UpdateRange(projectMovements);
                    ctx.SaveChanges();
                }
                return projectMovements;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(MovimentosProjectoAutorizados ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentosProjectoAutorizados.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    

        #region Parse Utilities
        public static MovementAuthorizedProjectViewModel ParseToViewModel(this MovimentosProjectoAutorizados item, string navDatabaseName, string navCompanyName)
        {
            if (item != null)
            {
                MovementAuthorizedProjectViewModel projMovement = new MovementAuthorizedProjectViewModel();
                projMovement.NoMovement = item.NumMovimento;
                projMovement.Date = item.DataRegisto;
                projMovement.Type = item.Tipo;
                projMovement.Code = item.Codigo;
                projMovement.Description = item.Descricao;
                projMovement.Quantity = item.Quantidade;
                projMovement.UnitCode = item.CodUnidadeMedida;
                projMovement.SalesPrice = item.PrecoVenda;
                projMovement.TotalPrice = item.PrecoTotal;
                projMovement.CodProject = item.CodProjeto;
                projMovement.RegionCode = item.CodRegiao;
                projMovement.FunctionalAreaCode = item.CodAreaFuncional;
                projMovement.ResponsabilityCenterCode = item.CodCentroResponsabilidade;
                projMovement.CodContract = item.CodContrato;
                projMovement.CodServiceGroup = item.CodGrupoServico ??0;
                projMovement.CodServClient = item.CodServCliente;
                projMovement.DescServClient = item.DescServCliente;
                projMovement.NumGuideResiduesGar = item.NumGuiaResiduosGar;
                projMovement.NumGuideExternal = item.NumGuiaExterna;
                projMovement.DateConsume = item.DataConsumo;
                projMovement.TypeMeal = item.TipoRefeicao;
                projMovement.TypeResourse = item.TipoRecurso;
                projMovement.NumDocument = item.NumDocumento;
                projMovement.CostPrice = item.PrecoCusto;
                projMovement.CostTotal = item.CustoTotal;
                projMovement.CodClient = item.CodCliente;
                projMovement.InvoiceGroup = item.GrupoFactura ?? 0;


            }
            return null;
        }

        public static List<MovementAuthorizedProjectViewModel> ParseToViewModel(this List<MovimentosProjectoAutorizados> items, string navDatabaseName, string navCompanyName)
        {
            List<MovementAuthorizedProjectViewModel> parsedItems = new List<MovementAuthorizedProjectViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel(navDatabaseName, navCompanyName)));
            return parsedItems;
        }

        public static MovimentosProjectoAutorizados ParseToDB(this MovementAuthorizedProjectViewModel item)
        {
            if (item != null)
            {
                MovimentosProjectoAutorizados projMovement = new MovimentosProjectoAutorizados();
                projMovement.NumMovimento = item.NoMovement;
                projMovement.DataRegisto = item.Date;
                projMovement.Tipo = item.Type;
                projMovement.Codigo = item.Code;
                projMovement.Descricao = item.Description;
                projMovement.Quantidade = item.Quantity;
                projMovement.CodUnidadeMedida = item.UnitCode;
                projMovement.PrecoVenda = item.SalesPrice;
                projMovement.PrecoTotal = item.TotalPrice;
                projMovement.CodProjeto = item.CodProject;
                projMovement.CodRegiao = item.RegionCode;
                projMovement.CodAreaFuncional = item.FunctionalAreaCode;
                projMovement.CodCentroResponsabilidade = item.ResponsabilityCenterCode;
                projMovement.CodContrato = item.CodContract;
                projMovement.CodGrupoServico = item.CodServiceGroup;
                projMovement.CodServCliente = item.CodServClient;
                projMovement.DescServCliente = item.DescServClient;
                projMovement.NumGuiaResiduosGar = item.NumGuideResiduesGar;
                projMovement.NumGuiaExterna = item.NumGuideExternal;
                projMovement.DataConsumo = item.DateConsume;
                projMovement.TipoRefeicao = item.TypeMeal;
                projMovement.TipoRecurso = item.TypeResourse;
                projMovement.NumDocumento = item.NumDocument;
                projMovement.PrecoCusto = item.CostPrice;
                projMovement.CustoTotal = item.CostTotal;
                projMovement.CodCliente = item.CodClient;
                projMovement.GrupoFactura = item.InvoiceGroup;

                return projMovement;
            }
            return null;
        }

        public static List<MovimentosProjectoAutorizados> ParseToDB(this List<MovementAuthorizedProjectViewModel> items)
        {
            List<MovimentosProjectoAutorizados> parsedItems = new List<MovimentosProjectoAutorizados>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
