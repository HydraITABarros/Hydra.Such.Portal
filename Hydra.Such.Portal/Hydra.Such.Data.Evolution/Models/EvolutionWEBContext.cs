using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EvolutionWEBContext : DbContext
    {
        public EvolutionWEBContext(DbContextOptions<EvolutionWEBContext> options) : base(options)
        {
        }
    }
}
