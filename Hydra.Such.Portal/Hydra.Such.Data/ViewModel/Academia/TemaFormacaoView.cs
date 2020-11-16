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
        public bool TemaActivo { get; set; }
        public ICollection<AttachmentsViewModel> ImagensTema { get; set; }
        public ICollection<AccaoFormacaoView> Accoes;

        public TemaFormacaoView()
        {

        }

        public TemaFormacaoView(string idTema)
        {
            if (!string.IsNullOrEmpty(idTema))
            {
                TemaFormacao tema = DBAcademia.__GetDetailsTema(idTema);
                if (tema != null)
                {
                    IdTema = tema.IdTema;
                    CodigoInterno = tema.CodigoInterno;
                    DescricaoTema = tema.DescricaoTema;
                    Activo = tema.Activo;
                    UrlImagem = tema.UrlImagem;
                    AccoesTema = tema.AccoesTema;

                    Accoes = CastToAccaoView(AccoesTema.Where(a => a.Activa.Value == 1).ToList());
                    ImagensTema = new List<AttachmentsViewModel>();
                }

            }
            else
            {
                return;
            }
        }

        public TemaFormacaoView(TemaFormacao tema)
        {
            IdTema = tema.IdTema;
            CodigoInterno = tema.CodigoInterno;
            DescricaoTema = tema.DescricaoTema;
            Activo = tema.Activo == null ? 0 : tema.Activo.Value;
            TemaActivo = tema.Activo == null ? false : tema.Activo.Value == 1;
            UrlImagem = tema.UrlImagem;
            AccoesTema = tema.AccoesTema;

            Accoes = CastToAccaoView(AccoesTema);

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
                Activo = TemaActivo ? 1 : 0
            };

            return tema;
        }

        public TemaFormacaoView ParseToView(TemaFormacao tema, bool onlyActives)
        {
            if (onlyActives)
            {
                return new TemaFormacaoView()
                {
                    IdTema = tema.IdTema,
                    CodigoInterno = tema.CodigoInterno,
                    DescricaoTema = tema.DescricaoTema,
                    Activo = tema.Activo == null ? 0 : tema.Activo.Value,
                    TemaActivo = tema.Activo == null ? false : tema.Activo.Value == 1,
                    UrlImagem = tema.UrlImagem

                };
            }
            else
            {
                return new TemaFormacaoView(tema);
            }
        }

        private List<AccaoFormacaoView> CastToAccaoView(ICollection<AccaoFormacao> accoes)
        {
            List<AccaoFormacaoView> accoesView = new List<AccaoFormacaoView>();
            foreach (var item in accoes)
            {
                accoesView.Add(new AccaoFormacaoView(item));
            }

            return accoesView;
        }
    }
}
