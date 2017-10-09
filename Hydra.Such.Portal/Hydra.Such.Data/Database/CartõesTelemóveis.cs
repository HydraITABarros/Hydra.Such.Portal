using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class CartõesTelemóveis
    {
        public CartõesTelemóveis()
        {
            MovimentosTelemóveis = new HashSet<MovimentosTelemóveis>();
        }

        public string NúmeroTelemóvel { get; set; }
        public int? Estado { get; set; }
        public DateTime? DataFidelização { get; set; }
        public int? TipoDeServiço { get; set; }
        public string NúmeroEmpregado { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string ContaSuch { get; set; }
        public bool? Plafond100Utilizador { get; set; }
        public int? PlafondFr { get; set; }
        public int? ExtraPlafond { get; set; }
        public bool? ChamadasInternacionais { get; set; }
        public bool? Roaming { get; set; }
        public int? BarramentoDeVoz { get; set; }
        public int? TarifárioDeDados { get; set; }
        public decimal? ValorMensalidade { get; set; }
        public int? Plafond { get; set; }
        public int? Gprs { get; set; }
        public string Observações { get; set; }
        public string Imei { get; set; }
        public DateTime? DataAtribuição { get; set; }
        public bool? EquipamentoNãoDevolvido { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraModificação { get; set; }

        public BarramentosDeVoz BarramentoDeVozNavigation { get; set; }
        public Tarifários TarifárioDeDadosNavigation { get; set; }
        public ICollection<MovimentosTelemóveis> MovimentosTelemóveis { get; set; }
    }
}
