using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class TelemoveisOcorrencias
    {
        public int NumOcorrencia { get; set; }
        public string NumCartao { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string Ocorrencia { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
