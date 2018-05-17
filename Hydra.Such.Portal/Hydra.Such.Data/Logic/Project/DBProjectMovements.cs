using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
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

        public static List<MovimentosDeProjeto> GetAllTableByAreaProjectNo(string user, int areaId, string projectNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //return ctx.MovimentosDeProjeto.Where(x => x.Faturada == false && x.Faturável == true && x.Registado == true && x.Utilizador == user && x.NºProjetoNavigation.Área == areaId && x.NºProjeto == projectNo && x.FaturaçãoAutorizada == false).ToList();
                    return ctx.MovimentosDeProjeto.Where(x => x.Faturada == false && x.Faturável == true && x.Registado == true && x.NºProjeto == projectNo && x.FaturaçãoAutorizada == false).ToList();
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
                        result.Add(new SPInvoiceListViewModel()
                        {
                            ClientRequest = temp.PedidodoCliente.Equals(DBNull.Value) ? "" : (string)temp.PedidodoCliente,
                            InvoiceToClientNo = temp.FaturaNoCliente.Equals(DBNull.Value) ? "" : (string)temp.FaturaNoCliente,
                            CommitmentNumber = temp.NoCompromisso.Equals(DBNull.Value) ? "" : (string)temp.NoCompromisso,
                            ProjectNo = temp.NoProjeto.Equals(DBNull.Value) ? "" : (string)temp.NoProjeto,
                            Date = temp.Data.Equals(DBNull.Value) ? "" : (string)temp.Data.ToString("yyyy-MM-dd"),
                            LineNo = (int)temp.NoLinha,
                            MovementType = (int?)temp.TipoMovimento,
                            //DocumentNo =  temp.NoDocumento.Equals(DBNull.Value) ? "" : (string)temp.NoDocumento,
                            Type = (int?)temp.Tipo,
                            Code = temp.Código.Equals(DBNull.Value) ? "" : (string)temp.Código,
                            Description = temp.Descrição.Equals(DBNull.Value) ? "" : (string)temp.Descrição,
                            MeasurementUnitCode = temp.CodUnidadeMedida.Equals(DBNull.Value) ? "" : (string)temp.CodUnidadeMedida,
                            Quantity = (decimal?)temp.Quantidade,
                            LocationCode = temp.CodLocalizacao.Equals(DBNull.Value) ? "" : (string)temp.CodLocalizacao,
                            ProjectContabGroup = temp.GrupoContabProjeto.Equals(DBNull.Value) ? "" : (string)temp.GrupoContabProjeto,
                            RegionCode = temp.CodigoRegiao.Equals(DBNull.Value) ? "" : (string)temp.CodigoRegiao,
                            FunctionalAreaCode = temp.CodAreaFuncional.Equals(DBNull.Value) ? "" : (string)temp.CodAreaFuncional,
                            ResponsabilityCenterCode = temp.CodCentroResponsabilidade.Equals(DBNull.Value) ? "" : (string)temp.CodCentroResponsabilidade,
                            User = temp.Utilizador.Equals(DBNull.Value) ? "" : (string)temp.Utilizador,
                            UnitCost = (decimal?)temp.CustoUnitario,
                            TotalCost = (decimal?)temp.CustoTotal,
                            UnitPrice = (decimal?)temp.PrecoUnitario,
                            TotalPrice = (decimal?)temp.PrecoTotal,
                            Billable = (bool?)temp.Faturável,
                            //ResidueGuideNo = temp.NoGuiaResiduos.Equals(DBNull.Value) ? "" : (string)temp.NoGuiaResiduos,
                            //ExternalGuideNo = temp.NoGuiaExterna.Equals(DBNull.Value) ? "" : (string)temp.NoGuiaExterna,
                            //RequestNo = temp.NoRequisicao.Equals(DBNull.Value) ? "" : (string)temp.NoRequisicao,
                            //RequestLineNo = temp.NoLinhaRequisicao == DBNull.Value ?? (int?)temp.NoLinhaRequisicao,
                            //Driver = temp.Motorista.Equals(DBNull.Value) ? "" : (string)temp.Motorista,
                            //MealType = temp.TipoRefeicao == DBNull.Value ?? (int?)temp.TipoRefeicao,
                            //ResidueFinalDestinyCode = temp.CodDestinoFinalResiduos == DBNull.Value ?? (int?)temp.CodDestinoFinalResiduos,
                            //OriginalDocument = temp.DocumentoOriginal.Equals(DBNull.Value) ? "" : (string)temp.DocumentoOriginal,
                            //AdjustedDocument = temp.DocumentoCorrigido.Equals(DBNull.Value) ? "" : (string)temp.DocumentoCorrigido,
                            //AdjustedPrice = temp.AcertoPrecos == DBNull.Value ?? (bool?)temp.AcertoPrecos,
                            //AdjustedDocumentData = temp.DataDocumentoCorrigido.Equals(DBNull.Value) ? "" : (string)temp.DataDocumentoCorrigido.ToString("yyyy-MM-dd"),
                            AutorizatedInvoice = (bool?)temp.FaturacaoAutorizada,
                            AutorizatedInvoiceData = temp.DataAutorizacaoFaturacao.Equals(DBNull.Value) ? "" : (string)temp.DataAutorizacaoFaturacao.ToString("yyyy-MM-dd"),
                            //ServiceGroupCode = temp.CodGrupoServico == DBNull.Value ?? (int?)temp.CodGrupoServico,
                            //ResourceType = temp.TipoRecurso == DBNull.Value ?? (int?)temp.TipoRecurso,
                            //TimesheetNo = temp.NoFolhaHoras.Equals(DBNull.Value) ? "" : (string)temp.NoFolhaHoras,
                            //InternalRequest = temp.RequisicaoInterna.Equals(DBNull.Value) ? "" : (string)temp.RequisicaoInterna,
                            //EmployeeNo = temp.NoFuncionario.Equals(DBNull.Value) ? "" : (string)temp.NoFuncionario,
                            //QuantityReturned = temp.QuantidadeDevolvida == DBNull.Value ?? (decimal?)temp.QuantidadeDevolvida,
                            ConsumptionDate = temp.DataConsumo.Equals(DBNull.Value) ? "" : (string)temp.DataConsumo.ToString("yyyy-MM-dd"),
                            CreateDate = (DateTime)temp.DataHoraCriacao,
                            //UpdateDate = temp.DataHoraModificacao == DBNull.Value ?? (DateTime)temp.DataHoraModificacao,
                            CreateUser = temp.UtilizadorCriacao.Equals(DBNull.Value) ? "" : (string)temp.UtilizadorCriacao,
                            //UpdateUser = temp.UtilizadorModificacao.Equals(DBNull.Value) ? "" : (string)temp.UtilizadorModificacao,
                            Registered = (bool?)temp.Registado,
                            Billed = (bool?)temp.Faturada,
                            //Currency = temp.Moeda.Equals(DBNull.Value) ? "" : (string)temp.Moeda,
                            //UnitValueToInvoice = temp.ValorUnitarioaFaturar == DBNull.Value ?? (decimal?)temp.ValorUnitarioaFaturar,
                            //ServiceClientCode = temp.CodServicoCliente == DBNull.Value ?? (int?)temp.CodServicoCliente,
                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
