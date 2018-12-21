using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.ProjectDiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.ProjectDiary
{
    public class DBClientServices
    {
        public static ServiçosCliente GetByServiceCode(string Codigo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ServiçosCliente.Where(x => x.CódServiço == Codigo).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static ServiçosCliente GetByClientNumber(string NCliente)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ServiçosCliente.Where(x => x.NºCliente == NCliente).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static List<ServiçosCliente> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ServiçosCliente.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ServiçosCliente Create(ServiçosCliente ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ServiçosCliente.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static ServiçosCliente Update(ServiçosCliente ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.ServiçosCliente.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string ServCode, string ClientCode)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<ServiçosCliente> ProfileAccessesToDelete = ctx.ServiçosCliente.Where(x => x.CódServiço == ServCode && x.NºCliente == ClientCode).ToList();
                    ctx.ServiçosCliente.RemoveRange(ProfileAccessesToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<ClientServicesViewModel> GetAllFromClientWithDescription(string ClientNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.ServiçosCliente.Where(x => x.NºCliente == ClientNo).Select(x => new ClientServicesViewModel() {
                        ServiceCode = x.CódServiço,
                        ServiceDescription = x.CódServiçoNavigation.Descrição,
                        ServiceGroup = x.GrupoServiços,
                        CodGrupoServico = x.CodGrupoServico,
                        ClientNumber = x.NºCliente
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        
        public static List<ClientServicesViewModel> GetAllServiceGroup(string NCliente, bool AllProjs)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (AllProjs)
                    {
                        return ctx.ServiçosCliente.Where(x => x.GrupoServiços == true).Select(x => new ClientServicesViewModel()
                        {
                            ServiceCode = x.CódServiço,
                            ServiceDescription = x.CódServiçoNavigation.Descrição,
                            ServiceGroup = x.GrupoServiços,
                            CodGrupoServico = x.CodGrupoServico,
                            ClientNumber = x.NºCliente
                        }).ToList();
                    }
                    else
                    {
                        return ctx.ServiçosCliente.Where(x => x.NºCliente == NCliente && x.GrupoServiços == true).Select(x => new ClientServicesViewModel()
                        {
                            ServiceCode = x.CódServiço,
                            ServiceDescription = x.CódServiçoNavigation.Descrição,
                            ServiceGroup = x.GrupoServiços,
                            CodGrupoServico = x.CodGrupoServico,
                            ClientNumber = x.NºCliente
                        }).ToList();
                    }                                
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static List<ClientServicesViewModel> GetAllClientService(string NCliente, bool AllProjs)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (AllProjs)
                    {
                        
                        return ctx.ServiçosCliente.Where(x => x.GrupoServiços == false).Select(x => new ClientServicesViewModel()
                        {
                            ServiceCode = x.CódServiço,
                            ServiceDescription = x.CódServiçoNavigation.Descrição,
                            ServiceGroup = x.GrupoServiços,
                            CodGrupoServico = x.CodGrupoServico,
                            ClientNumber = x.NºCliente
                        }).ToList();
                    }
                    else
                    {
                        return ctx.ServiçosCliente.Where(x => x.NºCliente == NCliente && x.GrupoServiços == false).Select(x => new ClientServicesViewModel()
                        {
                            ServiceCode = x.CódServiço,
                            ServiceDescription = x.CódServiçoNavigation.Descrição,
                            ServiceGroup = x.GrupoServiços,
                            CodGrupoServico = x.CodGrupoServico,
                            ClientNumber = x.NºCliente                            
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
