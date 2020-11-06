using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Hydra.Such.Data.ViewModel.Academia
{
    public class TemaFormacaoView : TemaFormacao
    {
        public int NoAccoesTema { get; set; }
        public ICollection<AttachmentsViewModel> ImagensTema { get; set; }

        public TemaFormacaoView()
        {

        }

        public TemaFormacaoView(TemaFormacao tema)
        {
            IdTema = tema.IdTema;
            CodigoInterno = tema.CodigoInterno;
            DescricaoTema = tema.DescricaoTema;
            Activo = tema.Activo;
            UrlImagem = tema.UrlImagem;
            AccoesTema = tema.AccoesTema;

            NoAccoesTema = AccoesTema == null ? 0 : AccoesTema.Count();

            List<Anexos> imagens = DBAttachments.GetById(TipoOrigemAnexos.TemaFormacao, tema.IdTema);
            ImagensTema = DBAttachments.ParseToViewModel(imagens);
            foreach(var i in ImagensTema)
            {
                i.Visivel = i.Visivel == null ? false : i.Visivel.Value;
            }
        }

        public TemaFormacao ParseToDb()
        {
            TemaFormacao tema = new TemaFormacao()
            {
                IdTema = IdTema,
                CodigoInterno = CodigoInterno,
                DescricaoTema = DescricaoTema,
                UrlImagem = UrlImagem,
                Activo = Activo
            };

            return tema;
        }
    }
}
