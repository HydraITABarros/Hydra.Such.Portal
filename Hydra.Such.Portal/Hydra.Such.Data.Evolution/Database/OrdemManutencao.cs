using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class OrdemManutencao
    {
        public OrdemManutencao()
        {
            OrdemManutencaoEquipamentos = new HashSet<OrdemManutencaoEquipamentos>();
            OrdemManutencaoMateriais = new HashSet<OrdemManutencaoMateriais>();
        }

        public int IdOm { get; set; }
        public int NumOm { get; set; }
        public int IdTipoObra { get; set; }
        public int IdEstadoObra { get; set; }
        public int IdOrigemAvaria { get; set; }
        public int IdTipoContacto { get; set; }
        public int RegistadoPor { get; set; }
        public DateTime DataRegisto { get; set; }
        public DateTime? DataEncerramento { get; set; }
        public int Cliente { get; set; }
        public int Instituicao { get; set; }
        public int Servico { get; set; }
        public string Contrato { get; set; }
        public DateTime DataPedido { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; }
        public int Trimestre { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public string NumReqCliente { get; set; }
        public string Contacto { get; set; }
        public TimeSpan? HoraAvaria { get; set; }
        public string DescAvaria { get; set; }
        public string Relatorio { get; set; }

        public Cliente ClienteNavigation { get; set; }
        public Contrato ContratoNavigation { get; set; }
        public EstadoObra IdEstadoObraNavigation { get; set; }
        public OrigemAvaria IdOrigemAvariaNavigation { get; set; }
        public TipoContacto IdTipoContactoNavigation { get; set; }
        public TipoObra IdTipoObraNavigation { get; set; }
        public Instituicao InstituicaoNavigation { get; set; }
        public Utilizador RegistadoPorNavigation { get; set; }
        public Servico ServicoNavigation { get; set; }
        public ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
        public ICollection<OrdemManutencaoMateriais> OrdemManutencaoMateriais { get; set; }
    }
}
