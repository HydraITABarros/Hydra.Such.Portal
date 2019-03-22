using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class ClientePimp
    {
        public int IdClientePimp { get; set; }
        public string IdContrato { get; set; }
        public int IdContratoLinha { get; set; }
        public int IdRegiao { get; set; }
        public int IdArea { get; set; }
        public int IdEquipa { get; set; }
        public int IdAreaOp { get; set; }
        public DateTime DataPlano { get; set; }
        public int Dia { get; set; }
        public int Semana { get; set; }
        public int Mes { get; set; }
        public int Trimestre { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        public string Observacoes { get; set; }
        public int? Tecnico { get; set; }
        public int IdUtilizadorInsercao { get; set; }
        public DateTime DataInsercao { get; set; }
        public int? IdUtilizadorAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? PlanoExecutado { get; set; }
        public bool? Replicado { get; set; }
        public int? IdCliente { get; set; }
        public int? IdInstituicao { get; set; }
        public int? IdServico { get; set; }

        public Area IdAreaNavigation { get; set; }
        public AreaOp IdAreaOpNavigation { get; set; }
        public ContratoLinha IdContratoLinhaNavigation { get; set; }
        public Contrato IdContratoNavigation { get; set; }
        public Equipa IdEquipaNavigation { get; set; }
        public Regiao IdRegiaoNavigation { get; set; }
        public Utilizador IdUtilizadorAlteracaoNavigation { get; set; }
        public Utilizador IdUtilizadorInsercaoNavigation { get; set; }
        public Utilizador TecnicoNavigation { get; set; }
    }
}
