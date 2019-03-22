using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class LinhaFolhaHorasDistribCus
    {
        public byte[] Timestamp { get; set; }
        public int NºLinhaFolhaHoras { get; set; }
        public int TipoCusto { get; set; }
        public int NºLinha { get; set; }
        public int TipoObra { get; set; }
        public string NºObra { get; set; }
        public string CódFaseProjecto { get; set; }
        public string CódSubfaseProjecto { get; set; }
        public string CódTarefaProjecto { get; set; }
        public int NºLinhaOrdemManut { get; set; }
        public int NºLinhaTarefaOrdemManut { get; set; }
        public decimal Valor { get; set; }
        public decimal Valor1 { get; set; }
        public decimal KmTotais { get; set; }
        public decimal KmDistancia { get; set; }
        public decimal Quantity { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public long NºFolhaHoras { get; set; }
    }
}
