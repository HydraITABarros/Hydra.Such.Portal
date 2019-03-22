using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class SistemaConfig
    {
        public int IdConfig { get; set; }
        public int IdCliente { get; set; }
        public string NomeInstituicaoMae { get; set; }
        public string NomeInstituicao { get; set; }
        public string NomeServico { get; set; }
    }
}
