using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorioAssinatura
    {
        public string Om { get; set; }
        public int IdEquipamento { get; set; }
        public string Codigo { get; set; }
        public string Versao { get; set; }
        public int IdUtilizador { get; set; }
        public DateTime Data { get; set; }
        public byte[] Assinatura { get; set; }
        public int Id { get; set; }
    }
}
