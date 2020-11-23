using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Hydra.Such.Data.ViewModel.Academia
{
    public class AccaoFormacaoView : AccaoFormacao
    {
        public int NoSessoes { get; set; }
        public bool AccaoActiva { get; set; }
        public Boolean TemSessoes { get; set; }
        public ICollection<AttachmentsViewModel> ImagensAccao { get; set; }

        public AccaoFormacaoView()
        {

        }

        public AccaoFormacaoView(AccaoFormacao accao)
        {
            IdAccao = accao.IdAccao;
            CodigoInterno = accao.CodigoInterno;
            DesignacaoAccao = accao.DesignacaoAccao;
            IdTema = accao.IdTema;
            Activa = accao.Activa == null ? 0 : accao.Activa.Value;
            AccaoActiva = Activa == 1;
            DataInicio = accao.DataInicio;
            //Activa = DataInicio != null && DataInicio.Value.Date < DateTime.Now.Date ? 0 : Activa.Value;
            DataFim = accao.DataFim;
            IdEntidadeFormadora = accao.IdEntidadeFormadora;
            NumeroTotalHoras = accao.NumeroTotalHoras == null ? 0 : accao.NumeroTotalHoras.Value;
            UrlImagem = accao.UrlImagem;

            SessoesFormacao = accao.SessoesFormacao;

            NoSessoes = accao.SessoesFormacao == null ? 0 : accao.SessoesFormacao.Count();
            TemSessoes = NoSessoes > 0;

            Entidade = DBAcademia.__GetDetailsEntidade(IdEntidadeFormadora);

            List<Anexos> imagens = DBAttachments.GetById(TipoOrigemAnexos.AccaoFormacao, accao.IdAccao);
            ImagensAccao = DBAttachments.ParseToViewModel(imagens);
            foreach (var item in ImagensAccao)
            {
                item.Visivel = item.Visivel == null ? false : item.Visivel.Value;
            }
        }

        public AccaoFormacao ParseToDb()
        {
            AccaoFormacao accao = new AccaoFormacao()
            {
                IdAccao = IdAccao,
                CodigoInterno = CodigoInterno,
                DesignacaoAccao = DesignacaoAccao,
                IdTema = IdTema,
                Activa = AccaoActiva ? 1 : 0,
                DataInicio = DataInicio,
                DataFim = DataFim,
                IdEntidadeFormadora = IdEntidadeFormadora,
                NumeroTotalHoras = NumeroTotalHoras,
                UrlImagem = UrlImagem
            };

            return accao;
        }
    }
}
