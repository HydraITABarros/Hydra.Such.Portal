using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Services
{
    public class ProposalsService
    {
        private SuchDBContext ctx;
        private readonly string changedByUserName;

        public ProposalsService(string logChangesAsUserName)
        {
            ctx = new SuchDBContext();
            changedByUserName = logChangesAsUserName;
        }

        public ContractViewModel SetStatus(ContractViewModel contractToUpdate)
        {
            if (contractToUpdate != null)
            {
                if (contractToUpdate.ContractType == 2)
                {
                    Contratos updatedContract = null;
                    Contratos contractWithoutChanges = DBContracts.GetByIdAndVersion(contractToUpdate.ContractNo, contractToUpdate.VersionNo);

                    if (contractWithoutChanges != null)
                    {
                        if (contractWithoutChanges.Estado != contractToUpdate.Status)
                        {
                            //handle status change
                            switch (contractToUpdate.Status)
                            {
                                case 1: //Alterar para Aberta
                                        /*
                                         * Fazer arquivo
                                         * Preencher estado com ‘Aberta’
                                         */
                                case 2: //Alterada para enviada
                                        /*
                                         * Fazer arquivo
                                         * Preencher estado com ‘Enviada’
                                         * Preencher ‘Data Envio’ com data indicada pelo utilizador.Deve ser criado um mecanismo tipo ‘PopUp’ para indicação dessa data.
                                         */
                                    contractToUpdate = ArchiveContract(contractToUpdate);
                                    break;
                                case 3: //Alterada para Revista
                                        /*
                                         * Preencher estado com ‘Revista’
                                         * Fazer arquivo
                                         * Preencher estado com ‘Aberta’
                                         * Incrementar 1 na versão da Proposta
                                         * Limpar Data de Estado
                                         * Preencher Data alteração com data do sistema
                                         * Alterar versão nas Linhas da Proposta para versão colocada em cabeçalho
                                         */
                                    updatedContract = DBContracts.Update(DBContracts.ParseToDB(contractToUpdate));
                                    updatedContract.Estado = 1;
                                    updatedContract.DataEstadoProposta = null;
                                    contractToUpdate = ArchiveContract(DBContracts.ParseToViewModel(updatedContract, string.Empty, string.Empty));
                                    break;
                                case 4: //Alterada para Perdida
                                        /*
                                         * Fazer arquivo
                                         * Preencher a razão do Arquivo. Deve ser criado um mecanismo tipo ‘PopUp’ para indicação desta informação.
                                         * Preencher estado com ‘Perdida’
                                         * Preencher Data Estado com data do sistema
                                         * Passar p/ Histórico
                                         */
                                case 5: //Alterar para Cancelada
                                        /*
                                         * Fazer arquivo
                                         * Preencher a razão do Arquivo.Deve ser criado um mecanismo tipo ‘PopUp’ para indicação desta informação.
                                         * Preencher estado com ‘Cancelada’
                                         * Preencher Data Estado com data do sistema
                                         * Passar p/ Histórico
                                         */
                                case 10: //Alterar para Não Respondida
                                         /*
                                          * Fazer arquivo
                                          * Preencher a razão do Arquivo.Deve ser criado um mecanismo tipo ‘PopUp’ para indicação desta informação.
                                          * Preencher estado com ‘Oportunidade Não Respondida’
                                          * Preencher Data Estado com data do sistema
                                          * Passar p/ Histórico
                                          */
                                    contractToUpdate.DateProposedState = DateTime.Now.ToString();
                                    contractToUpdate = ArchiveContract(contractToUpdate);
                                    break;
                                default:
                                    updatedContract = DBContracts.Update(DBContracts.ParseToDB(contractToUpdate));
                                    contractToUpdate = DBContracts.ParseToViewModel(updatedContract, string.Empty, string.Empty);
                                    break;

                            }
                        }
                        //else
                        //{
                        //    updatedContract = DBContracts.Update(DBContracts.ParseToDB(contractToUpdate));
                        //    contractToUpdate = DBContracts.ParseToViewModel(updatedContract, string.Empty, string.Empty);
                        //}
                        contractToUpdate.eReasonCode = 1;
                    }
                    else
                    {
                        contractToUpdate.eReasonCode = 2;
                        contractToUpdate.eMessage = "Ocorreu um erro ao atualizar o contrato.";
                    }
                }
                else
                {
                    contractToUpdate.eReasonCode = 2;
                    contractToUpdate.eMessage = "O tipo de contrato é inválido.";
                }
        }
            return contractToUpdate;
        }

        public ContractViewModel ArchiveContract(ContractViewModel data)
        {
            if (data != null)
            {
                if (data.ContractType == 2)
                {
                    Contratos contract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);

                    if (contract != null)
                    {
                        try
                        {
                            //Create new contract and update old
                            contract.UtilizadorModificação = changedByUserName;
                            contract.Arquivado = true;
                            DBContracts.Update(contract);

                            contract = DBContracts.ParseToDB(data);

                            contract.NºVersão = contract.NºVersão + 1;
                            contract.UtilizadorCriação = changedByUserName;
                            contract.UtilizadorModificação = "";
                            if (contract.TipoContrato == 1)
                            {
                                contract.NºProposta = "";
                            }
                            else if (contract.TipoContrato == 2)
                            {
                                contract.NºContrato = "";
                            }

                            contract.DataHoraModificação = null;
                            List<int> finishingStates = new List<int>() { 4, 5, 10 };
                            if (contract.Estado.HasValue)
                            {
                                if (finishingStates.Contains(contract.Estado.Value))
                                    contract.Arquivado = true;
                                else
                                    contract.Arquivado = false;
                            }
                            else
                            {
                                contract.Arquivado = false;
                            }

                            //if (data.ActionCode.HasValue && data.ActionCode.Value == 2)
                            //{
                            //    contract.Estado = 1;
                            //    contract.DataHoraModificação = DateTime.Now;
                            //    contract.UtilizadorModificação = changedByUserName;
                            //}

                            DBContracts.Create(contract);

                            //Duplicate Contract Lines
                            List<LinhasContratos> contractLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);

                            contractLines.ForEach(x =>
                            {
                                x.NºLinha = 0;
                                x.NºVersão = contract.NºVersão;
                                DBContractLines.Create(x);
                            });

                            //data.VersionNo = contract.NºVersão;
                            data = DBContracts.ParseToViewModel(contract, string.Empty, string.Empty);
                            data.eReasonCode = 1;
                            data.eMessage = "Arquivado com sucesso.";
                        }
                        catch (Exception)
                        {
                            data.eReasonCode = 2;
                            data.eMessage = "Ocorreu um erro ao arquivar.";
                        }
                    }
                }
                else
                {
                    data.eReasonCode = 2;
                    data.eMessage = "O tipo de contrato é inválido.";
                }
            }
            else
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao arquivar.";
            }
            return data;
        }
    }
}
