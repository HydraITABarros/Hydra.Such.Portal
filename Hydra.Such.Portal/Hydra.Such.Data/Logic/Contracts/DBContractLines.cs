using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Contracts
{
    public static class DBContractLines
    {
        public static List<LinhasContratos> GetAllByActiveContract(string contractNo, int versionNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasContratos.Where(x => x.NºContrato == contractNo && x.NºVersão == versionNo).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
