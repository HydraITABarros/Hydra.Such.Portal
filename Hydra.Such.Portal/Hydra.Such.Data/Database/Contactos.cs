using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Contactos
    {
        public string Nº { get; set; }
        public string Nome { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public string Endereço { get; set; }
        public string Cp { get; set; }
        public string Cidade { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Nif { get; set; }
        public string CódigoRegião { get; set; }
        public string PessoaContato { get; set; }
        public string TelefoneContato { get; set; }
        public string FunçãoContato { get; set; }
        public string TelemovelContato { get; set; }
        public string EmailContato { get; set; }
        public string Notas { get; set; }
        public string ClientNo { get; set; }
    }
}
