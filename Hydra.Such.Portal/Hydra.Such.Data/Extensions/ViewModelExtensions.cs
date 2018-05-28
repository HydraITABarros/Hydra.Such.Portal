using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.ViewModel.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Extensions
{
    public static class ViewModelExtensions
    {
        public static void LoadLines(this ContractViewModel contract)
        {
            if (contract == null)
                return;

            var contractLines = DBContractLines.GetAllByActiveContract(contract.ContractNo, contract.VersionNo);
            if (contractLines != null)
            {
                contract.Lines = DBContractLines.ParseToViewModel(contractLines);
            }
        }
    }
}
