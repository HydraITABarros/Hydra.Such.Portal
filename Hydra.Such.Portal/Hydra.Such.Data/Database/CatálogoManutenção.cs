using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class CatálogoManutenção
    {
        public CatálogoManutenção()
        {
            MãoDeObraFolhaDeHoras = new HashSet<MãoDeObraFolhaDeHoras>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }

        public ICollection<MãoDeObraFolhaDeHoras> MãoDeObraFolhaDeHoras { get; set; }
    }
}
