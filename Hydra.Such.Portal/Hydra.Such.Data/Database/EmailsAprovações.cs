using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class EmailsAprovações
    {
        public int Id { get; set; }
        public int NºMovimento { get; set; }
        public string EmailDestinatário { get; set; }
        public string NomeDestinatário { get; set; }
        public string Assunto { get; set; }
        public DateTime DataHoraEmail { get; set; }
        public string TextoEmail { get; set; }
        public bool Enviado { get; set; }
        public string ObservaçõesEnvio { get; set; }
    }
}
