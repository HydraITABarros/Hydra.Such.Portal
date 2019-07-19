using Hydra.Such.Data.Evolution.DatabaseReference;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Data.Evolution.Repositories
{
    public class EquipamentoRepository : ConfigurationBasedRepository<Equipamento, int?>
    {
        public EquipamentoRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {
        }

    }
}
