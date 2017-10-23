using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class Telefones
    {
        public Telefones()
        {
            MovimentosTelefones = new HashSet<MovimentosTelefones>();
        }

        public string NºTelefone { get; set; }
        public int? Estado { get; set; }
        public string LocalDaInstalação { get; set; }
        public string Morada { get; set; }
        public string Cidade { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string Fornecedor { get; set; }
        public string NºTelefoneÚltimo { get; set; }
        public int? NºExtensões { get; set; }
        public int? NºFaxes { get; set; }
        public bool? DébitoDireto { get; set; }
        public bool? ValorAcrescentado { get; set; }
        public decimal? ValorAssinaturaMensal { get; set; }
        public string Observações { get; set; }
        public int? TipoDeCircuito { get; set; }
        public bool? RedeDeDados { get; set; }
        public string Site { get; set; }
        public string LarguraDeBanda { get; set; }
        public bool? Redundância { get; set; }
        public string IpRouter { get; set; }
        public string IpServidor { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }

        public ICollection<MovimentosTelefones> MovimentosTelefones { get; set; }
    }
}
