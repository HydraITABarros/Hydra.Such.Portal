using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class LinhaFolhaHorasMãoObra
    {
        public byte[] Timestamp { get; set; }
        public long NºFolhaHoras { get; set; }
        public int NºLinha { get; set; }
        public int TipoObra { get; set; }
        public string NºObra { get; set; }
        public string CódFaseProjecto { get; set; }
        public string CódSubfaseProjecto { get; set; }
        public string CódTarefaProjecto { get; set; }
        public int NºLinhaOrdemManut { get; set; }
        public int NºLinhaTarefaOrdemManut { get; set; }
        public DateTime Data { get; set; }
        public DateTime HoraDeInício { get; set; }
        public DateTime HoraDeFim { get; set; }
        public DateTime NºHoras { get; set; }
        public decimal NºHorasAux { get; set; }
        public string NºRecurso { get; set; }
        public string NºFamíliaRecurso { get; set; }
        public string CódTipoTrabalho { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal PreçoCusto { get; set; }
        public decimal CustoUnitDirecto { get; set; }
        public decimal ValorCusto { get; set; }
        public decimal PreçoVenda { get; set; }
        public decimal PreçoTotal { get; set; }
        public decimal Margem { get; set; }
        public DateTime InicioTrabSup1 { get; set; }
        public DateTime FimTrabSup1 { get; set; }
        public DateTime InicioTrabSup2 { get; set; }
        public DateTime FimTrabSup2 { get; set; }
        public DateTime NºHorasTrabSup1 { get; set; }
        public decimal NumHorasTrabSup1Aux { get; set; }
        public DateTime NºHorasTrabSup2 { get; set; }
        public decimal NumHorasTrabSup2Aux { get; set; }
        public byte RegistarTrabalhoSuplementar { get; set; }
        public string TipoAusênciaPresença { get; set; }
        public string Observação { get; set; }
        public string NºEmpregado { get; set; }
        public byte HorarioAlmoco { get; set; }
        public byte HorarioJantar { get; set; }
        public int Mês { get; set; }
        public int Ano { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public string OmOrderType { get; set; }
        public long Nfh { get; set; }
        public byte Validada { get; set; }
        public string Descrição3 { get; set; }
        public string Descrição1 { get; set; }
        public string Descrição2 { get; set; }
    }
}
