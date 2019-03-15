using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Extensions;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
namespace Hydra.Such.Portal.Services
{
    public class ContractsService
    {
        private SuchDBContext ctx;
        private readonly string changedByUserName;

        public ContractsService(string logChangesAsUserName)
        {
            ctx = new SuchDBContext();
            changedByUserName = logChangesAsUserName;
        }
        
        public ContractViewModel ArchiveContract(ContractViewModel contractToArchive)
        {
            if (contractToArchive != null)
            {
                Contratos cContract = DBContracts.GetByIdAndVersion(contractToArchive.ContractNo, contractToArchive.VersionNo);
                var lastVersionContract = DBContracts.GetByIdLastVersion(contractToArchive.ContractNo);
                int lastVersionNumber = 0;

                if (lastVersionContract != null)
                    lastVersionNumber = lastVersionContract.NºVersão + 1;
                else
                    lastVersionNumber = contractToArchive.VersionNo + 1;

                if (cContract != null)
                {
                    try
                    {
                        //Create new contract and update old
                        cContract.Notas = cContract.Notas + Environment.NewLine + contractToArchive.ArchiveReason;
                        cContract.UtilizadorModificação = changedByUserName;
                        cContract.Arquivado = true;
                        DBContracts.Update(cContract);

                        //NR20181116 - Só faz se não for Oportunidade
                        if (cContract.TipoContrato == (int)ContractType.Oportunity)
                        {
                            contractToArchive.eReasonCode = 1;
                            contractToArchive.eMessage = "Arquivado com sucesso.";
                            return contractToArchive;
                        }

                        cContract.NºVersão = lastVersionNumber;// cContract.NºVersão + 1;
                        cContract.UtilizadorCriação = changedByUserName;
                        cContract.UtilizadorModificação = "";
                        if (cContract.TipoContrato == (int)ContractType.Oportunity)
                        {
                            cContract.NºProposta = "";
                        }
                        else if (cContract.TipoContrato == (int)ContractType.Proposal)
                        {
                            cContract.NºContrato = "";
                        }

                        cContract.DataHoraModificação = null;
                        cContract.Arquivado = false;

                        if (contractToArchive.ActionCode.HasValue && contractToArchive.ActionCode.Value == 2)
                        {
                            cContract.Estado = 1;
                            cContract.DataHoraModificação = DateTime.Now;
                            cContract.UtilizadorModificação = changedByUserName;
                        }

                        DBContracts.Create(cContract);

                        //Duplicate Contract Lines
                        List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(contractToArchive.ContractNo, contractToArchive.VersionNo);

                        ContractLines.ForEach(x =>
                        {
                            x.NºVersão = lastVersionNumber;// cContract.NºVersão;
                            DBContractLines.Create(x);
                        });

                        contractToArchive.VersionNo = lastVersionNumber;// cContract.NºVersão;
                        contractToArchive.eReasonCode = 1;
                        contractToArchive.eMessage = "Arquivado com sucesso.";
                        return contractToArchive;
                    }
                    catch (Exception)
                    {
                        contractToArchive.eReasonCode = 2;
                        contractToArchive.eMessage = "Ocorreu um erro ao arquivar.";
                    }
                }
            }
            else
            {
                contractToArchive.eReasonCode = 2;
                contractToArchive.eMessage = "Ocorreu um erro ao arquivar.";
            }
            return contractToArchive;
        }
        
        public ContractViewModel UpdatePrices(UpdateContractPricesRequest updPriceRequest)
        {
            ContractViewModel contract;

            var tmpContract = DBContracts.GetByIdAndVersion(updPriceRequest.ContractNo, updPriceRequest.VersionNo);
            if (tmpContract == null)
            {
                contract = new ContractViewModel();
                contract.eReasonCode = 2;
                contract.eMessage = "Não foi possivel obter os dados do contrato.";
                return contract;
            }

            contract = ArchiveContract(DBContracts.ParseToViewModel(tmpContract, string.Empty, string.Empty));
            if (contract.eReasonCode == 1)
            {
                contract.LoadLines();

                contract.DueDate = updPriceRequest.DueDate;
                contract.ClientRequisitionNo = updPriceRequest.ClientRequisitionNo;
                contract.NextInvoiceDate = updPriceRequest.NextInvoiceDate;
                contract.ReceiptDateRequisition = updPriceRequest.RequisitionReceiveDate;
                contract.StartData = updPriceRequest.StartDate;

                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var updatedContract = ctx.Contratos.Update(DBContracts.ParseToDB(contract));

                        contract.Lines.ForEach(x =>
                        {
                            if (updPriceRequest.percentageToApllyInLines > (-100))
                            {
                                x.UnitPrice = x.UnitPrice + ((updPriceRequest.percentageToApllyInLines * x.UnitPrice) / 100);
                                ctx.LinhasContratos.Update(DBContractLines.ParseToDB(x));
                            }
                                
                        });
                        ctx.SaveChanges();
                        transaction.Commit();

                        contract.eReasonCode = 1;
                        contract.eMessage = "Preços atualizados com sucesso.";
                    }
                    catch
                    {
                        contract.eReasonCode = 2;
                        contract.eMessage = "Ocorreu um erro ao atualizar o contrato.";
                    }
                }
            }
            return contract;
        }
    }
}
