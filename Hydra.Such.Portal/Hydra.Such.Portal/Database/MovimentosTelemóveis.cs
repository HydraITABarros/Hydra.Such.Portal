using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class MovimentosTelemóveis
    {
        public int Id { get; set; }
        public string NúmeroTelemóvel { get; set; }
        public DateTime? Data { get; set; }
        public string NúmeroFatura { get; set; }
        public decimal? ValorComIva { get; set; }
        public decimal? ValorSemIva { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string NºFaturaNav { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraModificaçao { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }

        public CartõesTelemóveis NúmeroTelemóvelNavigation { get; set; }
    }
}
