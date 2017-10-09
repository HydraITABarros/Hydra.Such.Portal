using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class FichasTécnicasPratos
    {
        public FichasTécnicasPratos()
        {
            LinhasFichasTécnicasPratosCódigoNavigation = new HashSet<LinhasFichasTécnicasPratos>();
            LinhasFichasTécnicasPratosNºPratoNavigation = new HashSet<LinhasFichasTécnicasPratos>();
            ProcedimentosDeConfeção = new HashSet<ProcedimentosDeConfeção>();
        }

        public string NºPrato { get; set; }
        public string Descrição { get; set; }
        public string CódUnidadeMedida { get; set; }
        public int? Estado { get; set; }
        public string NomeFichaTécnica { get; set; }
        public string CódLocalização { get; set; }
        public int? TempoPreparação { get; set; }
        public int? TécnicaCulinária { get; set; }
        public string Grupo { get; set; }
        public string Época { get; set; }
        public int? NºDeDoses { get; set; }
        public int? TemperaturaPreparação { get; set; }
        public int? TemperaturaFinalConfeção { get; set; }
        public int? TemperaturaAServir { get; set; }
        public byte[] Image { get; set; }
        public int? VariaçãoPreçoCusto { get; set; }
        public int? ClassFt1 { get; set; }
        public int? ClassFt2 { get; set; }
        public string ClassFt3 { get; set; }
        public string ClassFt4 { get; set; }
        public string ClassFt5 { get; set; }
        public string ClassFt6 { get; set; }
        public string ClassFt7 { get; set; }
        public string ClassFt8 { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string Observações { get; set; }

        public ClassificaçãoFichasTécnicas ClassFt1Navigation { get; set; }
        public ClassificaçãoFichasTécnicas ClassFt2Navigation { get; set; }
        public ICollection<LinhasFichasTécnicasPratos> LinhasFichasTécnicasPratosCódigoNavigation { get; set; }
        public ICollection<LinhasFichasTécnicasPratos> LinhasFichasTécnicasPratosNºPratoNavigation { get; set; }
        public ICollection<ProcedimentosDeConfeção> ProcedimentosDeConfeção { get; set; }
    }
}
