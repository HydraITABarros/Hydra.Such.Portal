using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class RegistoAlteracoesPedidoFormacao
    {
        public string IdPedidoFormacao { get; set; }
        public int? IdRegisto { get; set; }
        public string TipoAlteracao { get; set; }
        public string DescricaoAlteracao { get; set; }
        public string UtilizadorAlteracao { get; set; }
        public DateTime? DataHoraAlteracao { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public PedidoParticipacaoFormacao PedidoNavigation { get; set; }
    }
}
