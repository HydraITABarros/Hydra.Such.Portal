using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TelemoveisEquipamentos
    {
        public int Tipo { get; set; }
        public string Imei { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Estado { get; set; }
        public string Cor { get; set; }
        public string Observacoes { get; set; }
        public DateTime? DataRecepcao { get; set; }
        public string Documento { get; set; }
        public string DocumentoRecepcao { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public byte DevolvidoBk { get; set; }
        public string NumEmpregadoComprador { get; set; }
        public string NomeComprador { get; set; }
        public int? Devolvido { get; set; }
    }
}
