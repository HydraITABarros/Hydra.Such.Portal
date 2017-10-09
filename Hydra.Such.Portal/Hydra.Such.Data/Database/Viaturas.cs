using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas
    {
        public Viaturas()
        {
            DiárioMovimentosViaturas = new HashSet<DiárioMovimentosViaturas>();
            LinhasRequisição = new HashSet<LinhasRequisição>();
            MovimentosViaturas = new HashSet<MovimentosViaturas>();
            Requisição = new HashSet<Requisição>();
        }

        public string Matrícula { get; set; }
        public DateTime? DataMatrícula { get; set; }
        public string NºQuadro { get; set; }
        public int? Estado { get; set; }
        public int? CódigoTipoViatura { get; set; }
        public int? CódigoMarca { get; set; }
        public int? CódigoModelo { get; set; }
        public string CartãoCombustível { get; set; }
        public string Apólice { get; set; }
        public int? PesoBruto { get; set; }
        public int? Tara { get; set; }
        public int? Cilindrada { get; set; }
        public int? TipoCombustível { get; set; }
        public int? NºLugares { get; set; }
        public string Cor { get; set; }
        public string AtribuídaA { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public int? TipoPropriedade { get; set; }
        public string NºImobilizado { get; set; }

        public Modelos CódigoM { get; set; }
        public Marcas CódigoMarcaNavigation { get; set; }
        public TiposViatura CódigoTipoViaturaNavigation { get; set; }
        public ICollection<DiárioMovimentosViaturas> DiárioMovimentosViaturas { get; set; }
        public ICollection<LinhasRequisição> LinhasRequisição { get; set; }
        public ICollection<MovimentosViaturas> MovimentosViaturas { get; set; }
        public ICollection<Requisição> Requisição { get; set; }
    }
}
