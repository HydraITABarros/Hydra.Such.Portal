using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Chat
    {
        public int IdMensagem { get; set; }
        public int IdUserEnvio { get; set; }
        public int IdUserDestino { get; set; }
        public DateTime DataMensagem { get; set; }
        public string Mensagem { get; set; }
        public bool Lida { get; set; }

        public virtual Utilizador IdUserDestinoNavigation { get; set; }
        public virtual Utilizador IdUserEnvioNavigation { get; set; }
    }
}
