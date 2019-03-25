using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBProjects
    {
        #region CRUD
        public static Projetos GetById(string NProjeto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Where(x => x.NºProjeto == NProjeto).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<Projetos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Projetos Create(Projetos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Projetos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Projetos Update(Projetos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Projetos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string ProjectNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Projetos.RemoveRange(ctx.Projetos.Where(x => x.NºProjeto == ProjectNo));
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

        public static List<Projetos> GetAllByArea(int AreaId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Where(x => x.Área == AreaId && x.Estado != EstadoProjecto.Encomenda).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Projetos GetAllByProjectNumber(string ProjectNumber)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Where(x => x.NºProjeto == ProjectNumber).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ProjectListItemViewModel> GetAllByAreaToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Select(x => new ProjectListItemViewModel()
                    {
                        ProjectNo = x.NºProjeto,
                        Date = x.Data,
                        DateText = x.Data.HasValue ? x.Data.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        Status = x.Estado,
                        Description = x.Descrição,
                        ClientNo = x.NºCliente,
                        RegionCode = x.CódigoRegião,
                        FunctionalAreaCode = x.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                        ContractoNo = x.NºContrato,
                        ProjectTypeCode = x.CódTipoProjeto,
                        ProjectTypeDescription = x.CódTipoProjetoNavigation.Descrição
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ProjectListItemViewModel> GetAllByEstado(EstadoProjecto Estado)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Select(x => new ProjectListItemViewModel()
                    {
                        ProjectNo = x.NºProjeto,
                        Date = x.Data,
                        DateText = x.Data.HasValue ? x.Data.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        Status = x.Estado,
                        Description = x.Descrição,
                        ClientNo = x.NºCliente,
                        RegionCode = x.CódigoRegião,
                        FunctionalAreaCode = x.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                        ContractoNo = x.NºContrato,
                        ProjectTypeCode = x.CódTipoProjeto,
                        ProjectTypeDescription = x.CódTipoProjetoNavigation.Descrição
                    }).Where(y => y.Status == Estado).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<ProjectListItemViewModel> GetByContract(string contractId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Projetos.Where(x => x.NºContrato == contractId).Select(x => new ProjectListItemViewModel()
                    {
                        ProjectNo = x.NºProjeto,
                        Date = x.Data,
                        DateText = x.Data.HasValue ? x.Data.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        Status = x.Estado,
                        Description = x.Descrição,
                        ClientNo = x.NºCliente,
                        RegionCode = x.CódigoRegião,
                        FunctionalAreaCode = x.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                        ContractoNo = x.NºContrato,
                        ProjectTypeCode = x.CódTipoProjeto,
                        ProjectTypeDescription = x.CódTipoProjetoNavigation.Descrição
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region Parse Utilities
        public static ProjectDetailsViewModel ParseToViewModel(this Projetos item)
        {
            if (item != null)
            {
                return new ProjectDetailsViewModel()
                {
                    ProjectNo = item.NºProjeto,
                    Area = item.Área,
                    Description = item.Descrição,
                    ClientNo = item.NºCliente,
                    Date = item.Data.HasValue ? item.Data.Value.ToString("yyyy-MM-dd") : "",
                    Status = item.Estado,
                    RegionCode = item.CódigoRegião,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = item.CódigoCentroResponsabilidade,
                    Billable = item.Faturável,
                    ContractNo = item.NºContrato,
                    ShippingAddressCode = item.CódEndereçoEnvio,
                    ShippingName = item.EnvioANome,
                    ShippingAddress = item.EnvioAEndereço,
                    ShippingPostalCode = item.EnvioACódPostal,
                    ShippingLocality = item.EnvioALocalidade,
                    ShippingContact = item.EnvioAContato,
                    ProjectTypeCode = item.CódTipoProjeto,
                    ProjectTypeDescription = item.CódTipoProjetoNavigation?.Descrição,
                    OurProposal = item.NossaProposta,
                    ServiceObjectCode = item.CódObjetoServiço,
                    CommitmentCode = item.NºCompromisso,
                    AccountWorkGroup = item.GrupoContabObra,
                    GroupContabProjectType = item.TipoGrupoContabProjeto,
                    GroupContabOMProjectType = item.TipoGrupoContabOmProjeto,
                    ClientRequest = item.PedidoDoCliente,
                    RequestDate = item.DataDoPedido.HasValue ? item.DataDoPedido.Value.ToString("yyyy-MM-dd") : "",
                    RequestValidity = item.ValidadeDoPedido,
                    DetailedDescription = item.DescriçãoDetalhada,
                    ProjectCategory = item.CategoriaProjeto,
                    BudgetContractNo = item.NºContratoOrçamento,
                    InternalProject = item.ProjetoInterno,
                    ProjectLeader = item.ChefeProjeto,
                    ProjectResponsible = item.ResponsávelProjeto,
                    CreateUser = item.UtilizadorCriação,
                    CreateDate = item.DataHoraCriação,
                    UpdateUser = item.UtilizadorModificação,
                    UpdateDate = item.DataHoraModificação,
                    FaturaPrecosIvaIncluido = item.FaturaPrecosIvaIncluido
                };
            }
            return null;
        }

        public static List<ProjectDetailsViewModel> ParseToViewModel(this List<Projetos> items)
        {
            List<ProjectDetailsViewModel> parsedItems = new List<ProjectDetailsViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static Projetos ParseToDB(this ProjectDetailsViewModel item)
        {
            if (item != null)
            {
                return new Projetos()
                {
                    NºProjeto = item.ProjectNo,
                    Área = item.Area,
                    Descrição = item.Description,
                    NºCliente = item.ClientNo,
                    Data = item.Date != "" && item.Date != null ? DateTime.Parse(item.Date) : (DateTime?)null,
                    Estado = item.Status,
                    CódigoRegião = item.RegionCode,
                    CódigoÁreaFuncional = item.FunctionalAreaCode,
                    CódigoCentroResponsabilidade = item.ResponsabilityCenterCode,
                    Faturável = item.Billable,
                    NºContrato = item.ContractNo,
                    CódEndereçoEnvio = item.ShippingAddressCode,
                    EnvioANome = item.ShippingName,
                    EnvioAEndereço = item.ShippingAddress,
                    EnvioACódPostal = item.ShippingPostalCode,
                    EnvioALocalidade = item.ShippingLocality,
                    EnvioAContato = item.ShippingContact,
                    CódTipoProjeto = item.ProjectTypeCode,
                    NossaProposta = item.OurProposal,
                    CódObjetoServiço = item.ServiceObjectCode,
                    NºCompromisso = item.CommitmentCode,
                    GrupoContabObra = item.AccountWorkGroup,
                    TipoGrupoContabProjeto = item.GroupContabProjectType,
                    TipoGrupoContabOmProjeto = item.GroupContabOMProjectType,
                    PedidoDoCliente = item.ClientRequest,
                    DataDoPedido = item.RequestDate != "" && item.RequestDate != null ? DateTime.Parse(item.RequestDate) : (DateTime?)null,
                    ValidadeDoPedido = item.RequestValidity,
                    DescriçãoDetalhada = item.DetailedDescription,
                    CategoriaProjeto = item.ProjectCategory,
                    NºContratoOrçamento = item.BudgetContractNo,
                    ProjetoInterno = item.InternalProject,
                    ChefeProjeto = item.ProjectLeader,
                    ResponsávelProjeto = item.ProjectResponsible,
                    UtilizadorCriação = item.CreateUser,
                    DataHoraCriação = item.CreateDate,
                    UtilizadorModificação = item.UpdateUser,
                    DataHoraModificação = item.UpdateDate,
                    FaturaPrecosIvaIncluido = item.FaturaPrecosIvaIncluido
                };
            }
            return null;
        }

        public static List<Projetos> ParseToDB(this List<ProjectDetailsViewModel> items)
        {
            List<Projetos> parsedItems = new List<Projetos>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
