using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Localizações
    {
        public string Código { get; set; }
        public string Nome { get; set; }
        public string Endereço { get; set; }
        public string Cidade { get; set; }
        public string Telefone { get; set; }
        public string NºFax { get; set; }
        public string Contato { get; set; }
        public string CódPostal { get; set; }
        public string Email { get; set; }
        public bool? Bloqueado { get; set; }
        public string Região { get; set; }
        public string Área { get; set; }
        public string CentroResponsabilidade { get; set; }
        public string LocalFornecedor { get; set; }
        public int? CódigoLocalEntrega { get; set; }
        public string ResponsávelArmazém { get; set; }
        public bool? ArmazémAmbiente { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Locais CódigoLocalEntregaNavigation { get; set; }
    }
}
