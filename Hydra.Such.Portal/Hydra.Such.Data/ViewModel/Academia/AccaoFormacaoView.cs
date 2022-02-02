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
        public int NoPedidosParaAprovacao { get; set; }
        public int NoPedidosAprovados { get; set; }
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
            Activa = accao.Activa.HasValue ? accao.Activa.Value : 0;
            AccaoActiva = Activa == 1;
            DataInicio = accao.DataInicio;
            //Activa = DataInicio != null && DataInicio.Value.Date < DateTime.Now.Date ? 0 : Activa.Value;
            DataFim = accao.DataFim;
            IdEntidadeFormadora = accao.IdEntidadeFormadora;
            NumeroTotalHoras = accao.NumeroTotalHoras ?? 0;
            LocalRealizacao = accao.LocalRealizacao;
            UrlImagem = accao.UrlImagem;
            CustoInscricao = accao.CustoInscricao ?? 0;

        }

        public void ImagensDaAccao()
        {
            List<Anexos> imagens = DBAttachments.GetById(TipoOrigemAnexos.AccaoFormacao, IdAccao);
            ImagensAccao = DBAttachments.ParseToViewModel(imagens);
            foreach (var item in ImagensAccao)
            {
                item.Visivel = item.Visivel ?? false;
            }
        }

        public void SessoesDaAccao(AccaoFormacao accao)
        {
            SessoesFormacao = accao.SessoesFormacao;

            NoSessoes = accao.SessoesFormacao == null ? 0 : accao.SessoesFormacao.Count();
            TemSessoes = NoSessoes > 0;
        }

        public void DetalhesEntidade()
        {
            Entidade = DBAcademia.__GetDetailsEntidade(IdEntidadeFormadora);
        }

       public void CarregaPedidos(ConfiguracaoAprovacaoUtilizador cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade origin, bool onlyCompleted)
        {
            PedidosParticipacao = DBAcademia.__GetAllPedidosFormacao(IdAccao, cfgUser, origin, onlyCompleted);
        }

        public void ContaPedidos(ConfiguracaoAprovacaoUtilizador cfgUser, Enumerations.AcademiaOrigemAcessoFuncionalidade origin)
        {
            
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
                LocalRealizacao = LocalRealizacao,
                UrlImagem = UrlImagem,
                CustoInscricao = CustoInscricao
            };

            if (Entidade != null)
            {
                accao.Entidade = Entidade;
            }
            return accao;
        }
    }
}
