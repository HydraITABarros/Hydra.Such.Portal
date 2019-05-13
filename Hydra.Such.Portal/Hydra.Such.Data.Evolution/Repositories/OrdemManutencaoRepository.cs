using Hydra.Such.Data.Evolution.Database;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Data.Evolution.Repositories
{
    public class OrdemManutencaoRepository : ConfigurationBasedRepository<OrdemManutencao, string>
    {
        public OrdemManutencaoRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {
        }
    }
}
