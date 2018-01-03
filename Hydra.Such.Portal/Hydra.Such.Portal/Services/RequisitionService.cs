using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Compras;

namespace Hydra.Such.Portal.Services
{
    public class RequisitionService
    {
        private readonly NAVWSConfigurations _configws;

        public RequisitionService(NAVWSConfigurations NAVWSConfigs)
        {
            _configws = NAVWSConfigs;
        }

        public ErrorHandler CreatePrePurchaseOrderFor(RequisitionViewModel requisition)
        {
            ErrorHandler status = new ErrorHandler();

            if (requisition != null && requisition.Lines != null && requisition.Lines.Count > 0 && requisition.State == RequisitionStates.Approved)
            {
                /*
                    Filtrar as linhas da requisição cujos campos ‘Mercado Local’ seja = true, ‘Validado Compras’=false e ‘Quandidade Requerida’ > 0;
                    18-12-2017: Indicação para agrupar por fornecedor para criação de cabeçalhos e linhas na tab. Compras do NAV.
                */
                //use for database update later
                var linesToValidate = requisition.Lines
                    .Where(x =>
                        x.LocalMarket != null
                        && x.PurchaseValidated != null
                        && x.QuantityRequired != null
                        && x.LocalMarket.Value
                        && !x.PurchaseValidated.Value
                        && x.QuantityRequired.Value > 0)
                    .ToList();

                var supplierProducts = linesToValidate.GroupBy(x =>
                            x.SupplierNo,
                            x => x,
                            (key, items) => new PurchOrderToSupplierDTO
                            {
                                SupplierId = key,
                                RequisitionId = requisition.RequisitionNo,
                                CenterResponsibilityCode = requisition.CenterResponsibilityCode,
                                FunctionalAreaCode = requisition.FunctionalAreaCode,
                                RegionCode = requisition.RegionCode,
                                Lines = items.Select(line => new PurchToSupplierLineDTO()
                                {
                                    LineId = line.LineNo.Value,
                                    Type = line.Type,
                                    Code = line.Code,
                                    Description = line.Description,
                                    ProjectNo = line.ProjectNo,
                                    QuantityRequired = line.QuantityRequired,
                                    UnitCost = line.UnitCost,
                                    LocationCode = line.LocalCode,
                                    OpenOrderNo = line.OpenOrderNo,
                                    OpenOrderLineNo = line.OpenOrderLineNo
                                })
                                .ToList()
                            })
                    .ToList();

                if (supplierProducts.Count() > 0)
                {
                    string executionReport = "Relatório de validação de mercado local: ";
                    bool hasErros = false;
                    supplierProducts.ForEach(purchFromSupplier =>
                    {
                        Task<WSPurchaseInvHeader.Create_Result> createPurchaseHeaderTask = NAVPurchaseHeaderService.CreateAsync(purchFromSupplier, _configws);
                        try
                        {
                            createPurchaseHeaderTask.Wait();
                            if (createPurchaseHeaderTask.IsCompletedSuccessfully)
                            {
                                purchFromSupplier.NAVPrePurchOrderId = createPurchaseHeaderTask.Result.WSPurchInvHeaderInterm.No;

                                executionReport += string.Format("Criada a pré-compra {0}.", purchFromSupplier.NAVPrePurchOrderId);
                                Task<WSPurchaseInvLine.CreateMultiple_Result> createPurchaseLinesTask = NAVPurchaseLineService.CreateMultipleAsync(purchFromSupplier, _configws);
                                try
                                {
                                    createPurchaseLinesTask.Wait();
                                    if (createPurchaseLinesTask.IsCompletedSuccessfully)
                                    {
                                        executionReport += string.Format(" Criadas linhas de pré-compra com sucesso.");
                                    }
                                    else
                                    {
                                        executionReport += string.Format(" Não foi possivel criar as linhas de pré-compra.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    hasErros = true;
                                    executionReport += string.Format(" Ocorreu um erro ao criar as linhas de pré-compra no NAV.");
                                }
                            }
                            else
                            {
                                executionReport += string.Format(" Ocorreu um erro ao criar a pré-compra para o fornecedor com o ID:{0}.", purchFromSupplier.SupplierId);
                            }
                        }
                        catch (Exception ex)
                        {
                            hasErros = true;
                            executionReport += string.Format(" Ocorreu um erro ao criar a pré-compra no NAV.");
                        }
                    });
                    status.eReasonCode = hasErros ? 2 : 1;
                    status.eMessage = executionReport;
                }
                else
                {
                    status.eReasonCode = 3;
                    status.eMessage = " Não existem linhas que cumpram os requisitos de validação.";
                }
            }
            else
            {
                status.eReasonCode = 3;
                status.eMessage = " O estado da requisição e / ou linhas não cumprem os requisitos de validação.";
            }
            return status;
        }
        
        public ErrorHandler CreateMarketConsultFor(RequisitionViewModel requisition)
        {
            throw new NotImplementedException("CreateMarketConsultFor");
        }

        public ErrorHandler CreatePurchaseOrderFor(RequisitionViewModel requisition)
        {
            ErrorHandler status = new ErrorHandler();

            if (requisition != null && requisition.Lines != null && requisition.Lines.Count > 0)
            {
                //use for database update later
                var linesToCreateFrom = requisition.Lines;

                var supplierProducts = linesToCreateFrom.GroupBy(x =>
                            x.SupplierNo,
                            x => x,
                            (key, items) => new PurchOrderToSupplierDTO
                            {
                                SupplierId = key,
                                RequisitionId = requisition.RequisitionNo,
                                CenterResponsibilityCode = requisition.CenterResponsibilityCode,
                                FunctionalAreaCode = requisition.FunctionalAreaCode,
                                RegionCode = requisition.RegionCode,
                                Lines = items.Select(line => new PurchToSupplierLineDTO()
                                {
                                    LineId = line.LineNo.Value,
                                    Type = line.Type,
                                    Code = line.Code,
                                    Description = line.Description,
                                    ProjectNo = line.ProjectNo,
                                    QuantityRequired = line.QuantityRequired,
                                    UnitCost = line.UnitCost,
                                    LocationCode = line.LocalCode,
                                    OpenOrderNo = line.OpenOrderNo,
                                    OpenOrderLineNo = line.OpenOrderLineNo
                                })
                                .ToList()
                            })
                    .ToList();

                if (supplierProducts.Count() > 0)
                {
                    string executionReport = "Relatório: ";
                    bool hasErros = false;
                    supplierProducts.ForEach(purchFromSupplier =>
                    {
                        try
                        {
                            Task<WSPurchaseInvHeader.Create_Result> createPurchaseHeaderTask = NAVPurchaseHeaderService.CreateAsync(purchFromSupplier, _configws);
                            createPurchaseHeaderTask.Wait();
                            if (createPurchaseHeaderTask.IsCompletedSuccessfully)
                            {
                                purchFromSupplier.NAVPrePurchOrderId = createPurchaseHeaderTask.Result.WSPurchInvHeaderInterm.No;

                                try
                                {
                                    Task<WSPurchaseInvLine.CreateMultiple_Result> createPurchaseLinesTask = NAVPurchaseLineService.CreateMultipleAsync(purchFromSupplier, _configws);
                                    createPurchaseLinesTask.Wait();
                                    if (createPurchaseLinesTask.IsCompletedSuccessfully)
                                    {
                                        try
                                        {
                                            Task<WSGenericCodeUnit.FxCabimento_Result> createPurchOrderTask = WSGeneric.CreatePurchaseOrderFitting(purchFromSupplier.NAVPrePurchOrderId, _configws);
                                            createPurchOrderTask.Wait();
                                            if (createPurchOrderTask.IsCompletedSuccessfully)
                                            {
                                                
                                                //TODO: As linhas da requisição devem ficar com a informação do nº da encomeda compromisso e nº encomenda cabimento; O Nº da requisição e linha deverão passar para a linha da encomenda compromisso
                                                //Get id's from NAV. createPurchOrderTask.Result...
                                                string tempPurchOrderFitId = "";
                                                //string tempPurchOrderCommitmentId = "";
                                                //var linesToUpdate = linesToCreateFrom.Where(x => purchFromSupplier.Lines.Select(y => y.LineId).ToArray().Contains(x.LineNo.Value)).ToList();

                                                //purchFromSupplier.Lines.ForEach(line =>
                                                //{
                                                //    var lineToUpdate = linesToUpdate.FirstOrDefault(x => x.LineNo == line.LineId);
                                                //    if (lineToUpdate != null)
                                                //    {
                                                //        lineToUpdate.PurchOrderFitId = tempPurchOrderFitId;
                                                //        lineToUpdate.PurchOrderCommitmentId = tempPurchOrderCommitmentId;
                                                //    }
                                                //});
                                                //UpdateLines(linesToUpdate);
                                            }
                                            else
                                            {

                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    else
                                    {
                                        executionReport += string.Format(" Não foi possivel criar as linhas de pré-compra.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    hasErros = true;
                                    executionReport += string.Format(" Ocorreu um erro ao criar as linhas de pré-compra no NAV.");
                                }
                            }
                            else
                            {
                                executionReport += string.Format("Ocorreu um erro ao criar a pré-compra para o fornecedor com o ID:{0}.", purchFromSupplier.SupplierId);
                            }
                        }
                        catch (Exception ex)
                        {
                            hasErros = true;
                            executionReport += string.Format(" Ocorreu um erro ao criar a pré-compra no NAV.");
                        }
                    });
                    status.eReasonCode = hasErros ? 2 : 1;
                    status.eMessage = executionReport;
                }
                else
                {
                    status.eReasonCode = 3;
                    status.eMessage = "Não existem linhas que cumpram os requisitos de validação do mercado local.";
                }
            }
            return status;
        }

        public ErrorHandler CreateTransportationGuideFor(RequisitionViewModel requisition)
        {
            throw new NotImplementedException("CreatePurchaseOrderCommitmentFrom");
        }

        public ErrorHandler SendPrePurchaseFor(RequisitionViewModel requisition)
        {
            throw new NotImplementedException("CreatePurchaseOrderCommitmentFrom");
        }

        private bool UpdateLines(List<RequisitionLineViewModel> linesToUpdate)
        {
            return DBRequestLine.Update(linesToUpdate.ParseToDB());

        }

        public class CreatePrePurchOrderResult
        {
            public bool IsCompletedSuccessfully { get; set; }
            public string NAVPrePurchOrderId { get; set; }
            public string ErrorMessage { get; set; }

            public CreatePrePurchOrderResult()
            {
                this.IsCompletedSuccessfully = false;
            }
        }

        private CreatePrePurchOrderResult CreatePrePurchaseOrderFrom(PurchOrderToSupplierDTO purchOrderToSupplier)
        {
            CreatePrePurchOrderResult result = new CreatePrePurchOrderResult();
            Task<WSPurchaseInvHeader.Create_Result> createPurchaseHeaderTask = NAVPurchaseHeaderService.CreateAsync(purchOrderToSupplier, _configws);
            try
            {
                createPurchaseHeaderTask.Wait();
                if (createPurchaseHeaderTask.IsCompletedSuccessfully)
                {
                    result.NAVPrePurchOrderId = createPurchaseHeaderTask.Result.WSPurchInvHeaderInterm.No;
                    
                    //TODO: Create lines



                    result.IsCompletedSuccessfully = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        
    }
}
