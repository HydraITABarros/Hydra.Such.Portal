using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class UserAcessosLocalizacoesViewModel
    {
        public string ID_Utilizador { get; set; }
        public string Localizacao { get; set; }
        public DateTime? DataHora_Criacao { get; set; }
        public DateTime? DataHora_Modificacao { get; set; }
        public string Utilizador_Criacao { get; set; }
        public string Utilizador_Modificacao { get; set; }
    }
}
