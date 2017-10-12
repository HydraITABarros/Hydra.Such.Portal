using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ClassificaçãoFichasTécnicas
    {
        public ClassificaçãoFichasTécnicas()
        {
            FichasTécnicasPratosClassFt1Navigation = new HashSet<FichasTécnicasPratos>();
            FichasTécnicasPratosClassFt2Navigation = new HashSet<FichasTécnicasPratos>();
            InverseGrupoNavigation = new HashSet<ClassificaçãoFichasTécnicas>();
        }

        public int Código { get; set; }
        public int? Tipo { get; set; }
        public int? Grupo { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ClassificaçãoFichasTécnicas GrupoNavigation { get; set; }
        public ICollection<FichasTécnicasPratos> FichasTécnicasPratosClassFt1Navigation { get; set; }
        public ICollection<FichasTécnicasPratos> FichasTécnicasPratosClassFt2Navigation { get; set; }
        public ICollection<ClassificaçãoFichasTécnicas> InverseGrupoNavigation { get; set; }
    }
}
