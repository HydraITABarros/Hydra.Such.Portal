using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.CCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public static class CCPFunctions
    {
        #region functions to map the Entity Framework objects to Json-friendly objects
        /*
            The next functions are used to map the EF objects to Json compatible objects and vice-versa 
        */

        // this function receives an ProcedimentosCcpView object and maps it to a ProcedimentosCcp object
        public static ProcedimentosCcp CastProcedimentoCcpViewToProcedimentoCcp(ProcedimentoCCPView ProcedimentoView)
        {
            return (new ProcedimentosCcp
            {
                Nº = ProcedimentoView.No,
                Tipo = ProcedimentoView.Tipo,
                Ano = ProcedimentoView.Ano,
                Referência = ProcedimentoView.Referencia,
                CódigoRegião = ProcedimentoView.CodigoRegiao,
                CódigoÁreaFuncional = ProcedimentoView.CodigoAreaFuncional,
                CódigoCentroResponsabilidade = ProcedimentoView.CodigoCentroResponsabilidade,
                Estado = ProcedimentoView.Estado,
                DataCriação = ProcedimentoView.DataCriacao,
                Imobilizado = ProcedimentoView.Imobilizado,
                ComentárioEstado = ProcedimentoView.ComentarioEstado,
                Anexos = ProcedimentoView.Anexos,
                DataHoraEstado = ProcedimentoView.DataHoraEstado,
                UtilizadorEstado = ProcedimentoView.UtilizadorEstado,
                CondiçõesDePagamento = ProcedimentoView.CondicoesDePagamento,
                NomeProcesso = ProcedimentoView.NomeProcesso,
                GestorProcesso = ProcedimentoView.GestorProcesso,
                TipoProcedimento = ProcedimentoView.TipoProcedimento,
                InformaçãoTécnica = ProcedimentoView.InformacaoTecnica,
                FundamentaçãoAquisição = ProcedimentoView.FundamentacaoAquisicao,
                PreçoBase = ProcedimentoView.PrecoBase,
                ValorPreçoBase = ProcedimentoView.ValorPrecoBase,
                Negociação = ProcedimentoView.Negociacao,
                CritériosAdjudicação = ProcedimentoView.CriteriosAdjudicacao,
                Prazo = ProcedimentoView.Prazo,
                PreçoMaisBaixo = ProcedimentoView.PrecoMaisBaixo,
                PropostaEconMaisVantajosa = ProcedimentoView.PropostaEconMaisVantajosa,
                PropostaVariante = ProcedimentoView.PropostaVariante,
                AbertoFechadoAoMercado = ProcedimentoView.AbertoFechadoAoMercado,
                PrazoEntrega = ProcedimentoView.PrazoEntrega,
                LocaisEntrega = ProcedimentoView.LocaisEntrega,
                ObservaçõesAdicionais = ProcedimentoView.ObservacoesAdicionais,
                EstimativaPreço = ProcedimentoView.EstimativaPreco,
                FornecedoresSugeridos = ProcedimentoView.FornecedoresSugeridos,
                FornecedorExclusivo = ProcedimentoView.FornecedorExclusivo,
                Interlocutor = ProcedimentoView.Interlocutor,
                DescPreçoMaisBaixo = ProcedimentoView.DescPrecoMaisBaixo,
                DescPropostaEconMaisVantajosa = ProcedimentoView.DescPropostaEconMaisVantajosa,
                DescPropostaVariante = ProcedimentoView.DescPropostaVariante,
                DescAbertoFechadoAoMercado = ProcedimentoView.DescAbertoFechadoAoMercado,
                DescFornecedorExclusivo = ProcedimentoView.DescFornecedorExclusivo,
                CritérioEscolhaProcedimento = ProcedimentoView.CriterioEscolhaProcedimento,
                DescEscolhaProcedimento = ProcedimentoView.DescEscolhaProcedimento,
                Júri = ProcedimentoView.Juri,
                ObjetoDoContrato = ProcedimentoView.ObjetoDoContrato,
                PréÁrea = ProcedimentoView.PreArea,
                SubmeterPréÁrea = ProcedimentoView.SubmeterPreArea,
                ValorDecisãoContratar = ProcedimentoView.ValorDecisaoContratar,
                ValorAdjudicaçãoAnteriro = ProcedimentoView.ValorAdjudicacaoAnteriro,
                ValorAdjudicaçãoAtual = ProcedimentoView.ValorAdjudicacaoAtual,
                DiferençaEuros = ProcedimentoView.DiferencaEuros,
                DiferençaPercent = ProcedimentoView.DiferencaPercent,
                WorkflowFinanceiros = ProcedimentoView.WorkflowFinanceiros,
                WorkflowJurídicos = ProcedimentoView.WorkflowJuridicos,
                WorkflowFinanceirosConfirm = ProcedimentoView.WorkflowFinanceirosConfirm,
                WorkflowJurídicosConfirm = ProcedimentoView.WorkflowJuridicosConfirm,
                AutorizaçãoImobCa = ProcedimentoView.AutorizacaoImobCa,
                AutorizaçãoAberturaCa = ProcedimentoView.AutorizacaoAberturaCa,
                AutorizaçãoAquisiçãoCa = ProcedimentoView.AutorizacaoAquisicaoCa,
                RejeiçãoImobCa = ProcedimentoView.RejeicaoImobCa,
                RejeiçãoAberturaCa = ProcedimentoView.RejeicaoAberturaCa,
                RejeiçãoAquisiçãoCa = ProcedimentoView.RejeicaoAquisicaoCa,
                DataAutorizaçãoImobCa = ProcedimentoView.DataAutorizacaoImobCa,
                DataAutorizaçãoAbertCa = ProcedimentoView.DataAutorizacaoAbertCa,
                DataAutorizaçãoAquisiCa = ProcedimentoView.DataAutorizacaoAquisiCa,
                RatificarCaAbertura = ProcedimentoView.RatificarCaAbertura,
                RatificarCaAdjudicação = ProcedimentoView.RatificarCaAdjudicacao,
                CaRatificar = ProcedimentoView.CaRatificar,
                CaDataRatificaçãoAbert = ProcedimentoView.CaDataRatificacaoAbert,
                CaDataRatificaçãoAdjudic = ProcedimentoView.CaDataRatificacaoAdjudic,
                NºAta = ProcedimentoView.No_Ata,
                DataAta = ProcedimentoView.DataAta,
                ComentárioPublicação = ProcedimentoView.ComentarioPublicacao,
                DataPublicação = ProcedimentoView.DataPublicacao,
                UtilizadorPublicação = ProcedimentoView.UtilizadorPublicacao,
                DataSistemaPublicação = ProcedimentoView.DataSistemaPublicacao,
                RecolhaComentário = ProcedimentoView.RecolhaComentario,
                DataRecolha = ProcedimentoView.DataRecolha,
                UtilizadorRecolha = ProcedimentoView.UtilizadorRecolha,
                DataSistemaRecolha = ProcedimentoView.DataSistemaRecolha,
                ComentárioRelatórioPreliminar = ProcedimentoView.ComentarioRelatorioPreliminar,
                DataValidRelatórioPreliminar = ProcedimentoView.DataValidRelatorioPreliminar,
                UtilizadorValidRelatórioPreliminar = ProcedimentoView.UtilizadorValidRelatorioPreliminar,
                DataSistemaValidRelatórioPreliminar = ProcedimentoView.DataSistemaValidRelatorioPreliminar,
                ComentárioAudiênciaPrévia = ProcedimentoView.ComentarioAudienciaPrevia,
                DataAudiênciaPrévia = ProcedimentoView.DataAudienciaPrevia,
                UtilizadorAudiênciaPrévia = ProcedimentoView.UtilizadorAudienciaPrevia,
                DataSistemaAudiênciaPrévia = ProcedimentoView.DataSistemaAudienciaPrevia,
                ComentárioRelatórioFinal = ProcedimentoView.ComentarioRelatorioFinal,
                DataRelatórioFinal = ProcedimentoView.DataRelatorioFinal,
                UtilizadorRelatórioFinal = ProcedimentoView.UtilizadorRelatorioFinal,
                DataSistemaRelatórioFinal = ProcedimentoView.DataSistemaRelatorioFinal,
                ComentárioNotificação = ProcedimentoView.ComentarioNotificacao,
                DataNotificação = ProcedimentoView.DataNotificacao,
                UtilizadorNotificação = ProcedimentoView.UtilizadorNotificacao,
                DataSistemaNotificação = ProcedimentoView.DataSistemaNotificacao,
                PrazoNotificaçãoDias = ProcedimentoView.PrazoNotificacaoDias,
                PercentExecução = ProcedimentoView.PercentExecucao,
                Arquivado = ProcedimentoView.Arquivado,
                DataFechoInicial = ProcedimentoView.DataFechoInicial,
                DataFechoPrevista = ProcedimentoView.DataFechoPrevista,
                NºDiasProcesso = ProcedimentoView.No_DiasProcesso,
                NºDiasAtraso = ProcedimentoView.No_DiasAtraso,
                Tratado = ProcedimentoView.Tratado,
                Fornecedor = ProcedimentoView.Fornecedor,
                Comentário = ProcedimentoView.Comentario,
                CaSuspenso = ProcedimentoView.CaSuspenso,
                CríticoAbertura = ProcedimentoView.CriticoAbertura,
                CríticoAdjudicação = ProcedimentoView.CriticoAdjudicacao,
                ObjetoDecisão = ProcedimentoView.ObjetoDecisao,
                RazãoNecessidade = ProcedimentoView.RazaoNecessidade,
                ProtocoloContrato = ProcedimentoView.ProtocoloContrato,
                AutorizaçãoAdjudicação = ProcedimentoView.AutorizacaoAdjudicacao,
                NãoAdjudicaçãoEEncerramento = ProcedimentoView.NaoAdjudicacaoEEncerramento,
                NãoAdjudicaçãoESuspensão = ProcedimentoView.NaoAdjudicacaoESuspensao,
                DataHoraCriação = ProcedimentoView.DataHoraCriacao,
                UtilizadorCriação = ProcedimentoView.UtilizadorCriacao,
                DataHoraModificação = ProcedimentoView.DataHoraModificacao,
                UtilizadorModificação = ProcedimentoView.UtilizadorModificacao,
                ElementosJuri = ProcedimentoView.ElementosJuri

            });
        }
        // this function receives an ProcedimentosCcp objet and maps it to a ProcedimentosCccpView object
        public static ProcedimentoCCPView CastProcedimentoCcpToProcedimentoCcpView(ProcedimentosCcp Procedimento)
        {
            return (new ProcedimentoCCPView
            {
                No = Procedimento.Nº,
                Tipo = Procedimento.Tipo,
                Ano = Procedimento.Ano,
                Referencia = Procedimento.Referência,
                CodigoRegiao = Procedimento.CódigoRegião,
                CodigoAreaFuncional = Procedimento.CódigoÁreaFuncional,
                CodigoCentroResponsabilidade = Procedimento.CódigoCentroResponsabilidade,
                Estado = Procedimento.Estado,
                DataCriacao = Procedimento.DataCriação,
                Imobilizado = Procedimento.Imobilizado,
                ComentarioEstado = Procedimento.ComentárioEstado,
                Anexos = Procedimento.Anexos,
                DataHoraEstado = Procedimento.DataHoraEstado,
                UtilizadorEstado = Procedimento.UtilizadorEstado,
                CondicoesDePagamento = Procedimento.CondiçõesDePagamento,
                NomeProcesso = Procedimento.NomeProcesso,
                GestorProcesso = Procedimento.GestorProcesso,
                TipoProcedimento = Procedimento.TipoProcedimento,
                InformacaoTecnica = Procedimento.InformaçãoTécnica,
                FundamentacaoAquisicao = Procedimento.FundamentaçãoAquisição,
                PrecoBase = Procedimento.PreçoBase,
                ValorPrecoBase = Procedimento.ValorPreçoBase,
                Negociacao = Procedimento.Negociação,
                CriteriosAdjudicacao = Procedimento.CritériosAdjudicação,
                Prazo = Procedimento.Prazo,
                PrecoMaisBaixo = Procedimento.PreçoMaisBaixo,
                PropostaEconMaisVantajosa = Procedimento.PropostaEconMaisVantajosa,
                PropostaVariante = Procedimento.PropostaVariante,
                AbertoFechadoAoMercado = Procedimento.AbertoFechadoAoMercado,
                PrazoEntrega = Procedimento.PrazoEntrega,
                LocaisEntrega = Procedimento.LocaisEntrega,
                ObservacoesAdicionais = Procedimento.ObservaçõesAdicionais,
                EstimativaPreco = Procedimento.EstimativaPreço,
                FornecedoresSugeridos = Procedimento.FornecedoresSugeridos,
                FornecedorExclusivo = Procedimento.FornecedorExclusivo,
                Interlocutor = Procedimento.Interlocutor,
                DescPrecoMaisBaixo = Procedimento.DescPreçoMaisBaixo,
                DescPropostaEconMaisVantajosa = Procedimento.DescPropostaEconMaisVantajosa,
                DescPropostaVariante = Procedimento.DescPropostaVariante,
                DescAbertoFechadoAoMercado = Procedimento.DescAbertoFechadoAoMercado,
                DescFornecedorExclusivo = Procedimento.DescFornecedorExclusivo,
                CriterioEscolhaProcedimento = Procedimento.CritérioEscolhaProcedimento,
                DescEscolhaProcedimento = Procedimento.DescEscolhaProcedimento,
                Juri = Procedimento.Júri,
                ObjetoDoContrato = Procedimento.ObjetoDoContrato,
                PreArea = Procedimento.PréÁrea,
                SubmeterPreArea = Procedimento.SubmeterPréÁrea,
                ValorDecisaoContratar = Procedimento.ValorDecisãoContratar,
                ValorAdjudicacaoAnteriro = Procedimento.ValorAdjudicaçãoAnteriro,
                ValorAdjudicacaoAtual = Procedimento.ValorAdjudicaçãoAtual,
                DiferencaEuros = Procedimento.DiferençaEuros,
                DiferencaPercent = Procedimento.DiferençaPercent,
                WorkflowFinanceiros = Procedimento.WorkflowFinanceiros,
                WorkflowJuridicos = Procedimento.WorkflowJurídicos,
                WorkflowFinanceirosConfirm = Procedimento.WorkflowFinanceirosConfirm,
                WorkflowJuridicosConfirm = Procedimento.WorkflowJurídicosConfirm,
                AutorizacaoImobCa = Procedimento.AutorizaçãoImobCa,
                AutorizacaoAberturaCa = Procedimento.AutorizaçãoAberturaCa,
                AutorizacaoAquisicaoCa = Procedimento.AutorizaçãoAquisiçãoCa,
                RejeicaoImobCa = Procedimento.RejeiçãoImobCa,
                RejeicaoAberturaCa = Procedimento.RejeiçãoAberturaCa,
                RejeicaoAquisicaoCa = Procedimento.RejeiçãoAquisiçãoCa,
                DataAutorizacaoImobCa = Procedimento.DataAutorizaçãoImobCa,
                DataAutorizacaoAbertCa = Procedimento.DataAutorizaçãoAbertCa,
                DataAutorizacaoAquisiCa = Procedimento.DataAutorizaçãoAquisiCa,
                RatificarCaAbertura = Procedimento.RatificarCaAbertura,
                RatificarCaAdjudicacao = Procedimento.RatificarCaAdjudicação,
                CaRatificar = Procedimento.CaRatificar,
                CaDataRatificacaoAbert = Procedimento.CaDataRatificaçãoAbert,
                CaDataRatificacaoAdjudic = Procedimento.CaDataRatificaçãoAdjudic,
                No_Ata = Procedimento.NºAta,
                DataAta = Procedimento.DataAta,
                ComentarioPublicacao = Procedimento.ComentárioPublicação,
                DataPublicacao = Procedimento.DataPublicação,
                UtilizadorPublicacao = Procedimento.UtilizadorPublicação,
                DataSistemaPublicacao = Procedimento.DataSistemaPublicação,
                RecolhaComentario = Procedimento.RecolhaComentário,
                DataRecolha = Procedimento.DataRecolha,
                UtilizadorRecolha = Procedimento.UtilizadorRecolha,
                DataSistemaRecolha = Procedimento.DataSistemaRecolha,
                ComentarioRelatorioPreliminar = Procedimento.ComentárioRelatórioPreliminar,
                DataValidRelatorioPreliminar = Procedimento.DataValidRelatórioPreliminar,
                UtilizadorValidRelatorioPreliminar = Procedimento.UtilizadorValidRelatórioPreliminar,
                DataSistemaValidRelatorioPreliminar = Procedimento.DataSistemaValidRelatórioPreliminar,
                ComentarioAudienciaPrevia = Procedimento.ComentárioAudiênciaPrévia,
                DataAudienciaPrevia = Procedimento.DataAudiênciaPrévia,
                UtilizadorAudienciaPrevia = Procedimento.UtilizadorAudiênciaPrévia,
                DataSistemaAudienciaPrevia = Procedimento.DataSistemaAudiênciaPrévia,
                ComentarioRelatorioFinal = Procedimento.ComentárioRelatórioFinal,
                DataRelatorioFinal = Procedimento.DataRelatórioFinal,
                UtilizadorRelatorioFinal = Procedimento.UtilizadorRelatórioFinal,
                DataSistemaRelatorioFinal = Procedimento.DataSistemaRelatórioFinal,
                ComentarioNotificacao = Procedimento.ComentárioNotificação,
                DataNotificacao = Procedimento.DataNotificação,
                UtilizadorNotificacao = Procedimento.UtilizadorNotificação,
                DataSistemaNotificacao = Procedimento.DataSistemaNotificação,
                PrazoNotificacaoDias = Procedimento.PrazoNotificaçãoDias,
                PercentExecucao = Procedimento.PercentExecução,
                Arquivado = Procedimento.Arquivado,
                DataFechoInicial = Procedimento.DataFechoInicial,
                DataFechoPrevista = Procedimento.DataFechoPrevista,
                No_DiasProcesso = Procedimento.NºDiasProcesso,
                No_DiasAtraso = Procedimento.NºDiasAtraso,
                Tratado = Procedimento.Tratado,
                Fornecedor = Procedimento.Fornecedor,
                Comentario = Procedimento.Comentário,
                CaSuspenso = Procedimento.CaSuspenso,
                CriticoAbertura = Procedimento.CríticoAbertura,
                CriticoAdjudicacao = Procedimento.CríticoAdjudicação,
                ObjetoDecisao = Procedimento.ObjetoDecisão,
                RazaoNecessidade = Procedimento.RazãoNecessidade,
                ProtocoloContrato = Procedimento.ProtocoloContrato,
                AutorizacaoAdjudicacao = Procedimento.AutorizaçãoAdjudicação,
                NaoAdjudicacaoEEncerramento = Procedimento.NãoAdjudicaçãoEEncerramento,
                NaoAdjudicacaoESuspensao = Procedimento.NãoAdjudicaçãoESuspensão,
                DataHoraCriacao = Procedimento.DataHoraCriação,
                UtilizadorCriacao = Procedimento.UtilizadorCriação,
                DataHoraModificacao = Procedimento.DataHoraModificação,
                UtilizadorModificacao = Procedimento.UtilizadorModificação
            });
        }

        public static RegistoDeAtas CastRegistoActasViewToRegistoActas(RegistoActasView ActaView)
        {
            return (new RegistoDeAtas
            {
                NºProcedimento = ActaView.NumProcedimento,
                NºAta = ActaView.NumActa,
                DataDaAta = ActaView.DataDaActa,
                Observações = ActaView.Observacoes,
                DataHoraCriação = ActaView.DataHoraCriacao,
                UtilizadorCriação = ActaView.UtilizadorCriacao,
                DataHoraModificação = ActaView.DataHoraModificacao,
                UtilizadorModificação = ActaView.UtilizadorModificacao
            });
        }
        public static RegistoActasView CastRegistoActasToRegistoActasView(RegistoDeAtas Acta)
        {
            return (new RegistoActasView
            {
                NumProcedimento = Acta.NºProcedimento,
                NumActa = Acta.NºAta,
                DataDaActa = Acta.DataDaAta,
                Observacoes = Acta.Observações,
                DataHoraCriacao = Acta.DataHoraCriação,
                UtilizadorCriacao = Acta.UtilizadorCriação,
                DataHoraModificacao = Acta.DataHoraModificação,
                UtilizadorModificacao = Acta.UtilizadorModificação
            });
        }

        public static TemposPaCcp CastTemposCCPViewToTemposPaCcp(TemposPACCPView TemposView)
        {
            return (new TemposPaCcp()
            {
                NºProcedimento = TemposView.NumProcedimento,
                Estado0 = TemposView.Estado0,
                Estado1 = TemposView.Estado1,
                Estado2 = TemposView.Estado2,
                Estado3 = TemposView.Estado3,
                Estado4 = TemposView.Estado4,
                Estado5 = TemposView.Estado5,
                Estado6 = TemposView.Estado6,
                Estado7 = TemposView.Estado7,
                Estado8 = TemposView.Estado8,
                Estado9 = TemposView.Estado9,
                Estado10 = TemposView.Estado10,
                Estado11 = TemposView.Estado11,
                Estado12 = TemposView.Estado12,
                Estado13 = TemposView.Estado13,
                Estado14 = TemposView.Estado14,
                Estado15 = TemposView.Estado15,
                Estado16 = TemposView.Estado16,
                Estado17 = TemposView.Estado17,
                Estado18 = TemposView.Estado18,
                Estado19 = TemposView.Estado19,
                Estado20 = TemposView.Estado20,
                Estado0Tg = TemposView.Estado0Tg,
                Estado1Tg = TemposView.Estado1Tg,
                Estado2Tg = TemposView.Estado2Tg,
                Estado3Tg = TemposView.Estado3Tg,
                Estado4Tg = TemposView.Estado4Tg,
                Estado5Tg = TemposView.Estado5Tg,
                Estado6Tg = TemposView.Estado6Tg,
                Estado7Tg = TemposView.Estado7Tg,
                Estado8Tg = TemposView.Estado8Tg,
                Estado9Tg = TemposView.Estado9Tg,
                Estado10Tg = TemposView.Estado10Tg,
                Estado11Tg = TemposView.Estado11Tg,
                Estado12Tg = TemposView.Estado12Tg,
                Estado13Tg = TemposView.Estado13Tg,
                Estado14Tg = TemposView.Estado14Tg,
                Estado15Tg = TemposView.Estado15Tg,
                Estado16Tg = TemposView.Estado16Tg,
                Estado17Tg = TemposView.Estado17Tg,
                Estado18Tg = TemposView.Estado18Tg,
                Estado19Tg = TemposView.Estado19Tg,
                Estado20Tg = TemposView.Estado20Tg,
                DataHoraCriação = TemposView.DataHoraCriacao,
                UtilizadorCriação = TemposView.UtilizadorCriacao,
                DataHoraModificação = TemposView.DataHoraModificacao,
                UtilizadorModificação = TemposView.UtilizadorModificacao
            });
        }
        public static TemposPACCPView CastTemposPaCcpToTemposCCPView(TemposPaCcp Tempos)
        {
            return (new TemposPACCPView()
            {
                NumProcedimento = Tempos.NºProcedimento,
                Estado0 = Tempos.Estado0,
                Estado1 = Tempos.Estado1,
                Estado2 = Tempos.Estado2,
                Estado3 = Tempos.Estado3,
                Estado4 = Tempos.Estado4,
                Estado5 = Tempos.Estado5,
                Estado6 = Tempos.Estado6,
                Estado7 = Tempos.Estado7,
                Estado8 = Tempos.Estado8,
                Estado9 = Tempos.Estado9,
                Estado10 = Tempos.Estado10,
                Estado11 = Tempos.Estado11,
                Estado12 = Tempos.Estado12,
                Estado13 = Tempos.Estado13,
                Estado14 = Tempos.Estado14,
                Estado15 = Tempos.Estado15,
                Estado16 = Tempos.Estado16,
                Estado17 = Tempos.Estado17,
                Estado18 = Tempos.Estado18,
                Estado19 = Tempos.Estado19,
                Estado20 = Tempos.Estado20,
                Estado0Tg = Tempos.Estado0Tg,
                Estado1Tg = Tempos.Estado1Tg,
                Estado2Tg = Tempos.Estado2Tg,
                Estado3Tg = Tempos.Estado3Tg,
                Estado4Tg = Tempos.Estado4Tg,
                Estado5Tg = Tempos.Estado5Tg,
                Estado6Tg = Tempos.Estado6Tg,
                Estado7Tg = Tempos.Estado7Tg,
                Estado8Tg = Tempos.Estado8Tg,
                Estado9Tg = Tempos.Estado9Tg,
                Estado10Tg = Tempos.Estado10Tg,
                Estado11Tg = Tempos.Estado11Tg,
                Estado12Tg = Tempos.Estado12Tg,
                Estado13Tg = Tempos.Estado13Tg,
                Estado14Tg = Tempos.Estado14Tg,
                Estado15Tg = Tempos.Estado15Tg,
                Estado16Tg = Tempos.Estado16Tg,
                Estado17Tg = Tempos.Estado17Tg,
                Estado18Tg = Tempos.Estado18Tg,
                Estado19Tg = Tempos.Estado19Tg,
                Estado20Tg = Tempos.Estado20Tg,
                DataHoraCriacao = Tempos.DataHoraCriação,
                UtilizadorCriacao = Tempos.UtilizadorCriação,
                DataHoraModificacao = Tempos.DataHoraModificação,
                UtilizadorModificacao = Tempos.UtilizadorModificação
            });
        }

        public static ElementosJuri CastElementosJuriViewToElementosJuri(ElementosJuriView ElementosView)
        {
            return (new ElementosJuri()
            {
                NºProcedimento = ElementosView.NoProcedimento,
                NºLinha = ElementosView.NoLinha,
                Utilizador = ElementosView.Utilizador,
                NºEmpregado = ElementosView.NoEmpregado,
                Presidente = ElementosView.Presidente,
                Vogal = ElementosView.Vogal,
                Suplente = ElementosView.Suplente,
                Email = ElementosView.Email,
                EnviarEmail = ElementosView.EnviarEmail,
                DataHoraCriação = ElementosView.DataHoraCriacao,
                DataHoraModificação = ElementosView.DataHoraModificacao,
                UtilizadorCriação = ElementosView.UtilizadorCriacao,
                UtilizadorModificação = ElementosView.UtilizadorModificacao
            });
        }
        public static ElementosJuriView CastElementosJuriToElementosJuriView(ElementosJuri Elementos)
        {
            return (new ElementosJuriView()
            {
                NoProcedimento = Elementos.NºProcedimento,
                NoLinha = Elementos.NºLinha,
                Utilizador = Elementos.Utilizador,
                NoEmpregado = Elementos.NºEmpregado,
                Presidente = Elementos.Presidente,
                Vogal = Elementos.Vogal,
                Suplente = Elementos.Suplente,
                Email = Elementos.Email,
                EnviarEmail = Elementos.EnviarEmail,
                DataHoraCriacao = Elementos.DataHoraCriação,
                DataHoraModificacao = Elementos.DataHoraModificação,
                UtilizadorCriacao = Elementos.UtilizadorCriação,
                UtilizadorModificacao = Elementos.UtilizadorModificação
            });
        }

        public static NotasProcedimentosCcp CastNotaProcedimentoViewToNotaProcedimento(NotasProcedimentoCCPView NotaView)
        {
            return (new NotasProcedimentosCcp()
            {
                NºProcedimento = NotaView.NoProcedimento,
                NºLinha = NotaView.NoLinha,
                DataHora = NotaView.DataHora,
                Nota = NotaView.Nota,
                Utilizador = NotaView.Utilizador,
                DataHoraCriação = NotaView.DataHoraCriacao,
                UtilizadorCriação = NotaView.UtilizadorCriação,
                DataHoraModificação = NotaView.DataHoraModificacao,
                UtilizadorModificação = NotaView.UtilizadorModificacao
            });
        }
        public static NotasProcedimentoCCPView CastNotaProcedimentoToNotaProcedimentoView(NotasProcedimentosCcp Nota)
        {
            return (new NotasProcedimentoCCPView()
            {
                NoProcedimento = Nota.NºProcedimento,
                NoLinha = Nota.NºLinha,
                DataHora = Nota.DataHora,
                Nota = Nota.Nota,
                Utilizador = Nota.Utilizador,
                DataHoraCriacao = Nota.DataHoraCriação,
                UtilizadorCriação = Nota.UtilizadorCriação,
                DataHoraModificacao = Nota.DataHoraModificação,
                UtilizadorModificacao = Nota.UtilizadorModificação
            });
        }

        public static WorkflowProcedimentosCcp CastWorkflowProcedimentoToWorkflowProcedimentoView(WorkflowProcedimentosCCPView WorkflowView)
        {
            return (new WorkflowProcedimentosCcp()
            {
                NºProcedimento = WorkflowView.NoProcedimento,
                Estado = WorkflowView.Estado,
                DataHora = WorkflowView.DataHora,
                TipoEstado = WorkflowView.TipoEstado,
                Comentário = WorkflowView.Comentario,
                Resposta = WorkflowView.Resposta,
                TipoResposta = WorkflowView.TipoResposta,
                DataResposta = WorkflowView.DataResposta,
                Utilizador = WorkflowView.Utilizador,
                Imobilizado = WorkflowView.Imobilizado,
                EstadoAnterior = WorkflowView.EstadoAnterior,
                EstadoSeguinte = WorkflowView.EstadoSeguinte,
                DataHoraCriação = WorkflowView.DataHoraCriacao,
                UtilizadorCriação = WorkflowView.UtilizadorCriacao,
                DataHoraModificação = WorkflowView.DataHoraModificacao,
                UtilizadorModificação = WorkflowView.UtilizadorModificacao
            });
        }
        public static WorkflowProcedimentosCCPView CastWorkflowProcedimentoViewToWorkflowProcedimento(WorkflowProcedimentosCcp Workflow)
        {
            return (new WorkflowProcedimentosCCPView()
            {
                NoProcedimento = Workflow.NºProcedimento,
                Estado = Workflow.Estado,
                DataHora = Workflow.DataHora,
                TipoEstado = Workflow.TipoEstado,
                Comentario = Workflow.Comentário,
                Resposta = Workflow.Resposta,
                TipoResposta = Workflow.TipoResposta,
                DataResposta = Workflow.DataResposta,
                Utilizador = Workflow.Utilizador,
                Imobilizado = Workflow.Imobilizado,
                EstadoAnterior = Workflow.EstadoAnterior,
                EstadoSeguinte = Workflow.EstadoSeguinte,
                DataHoraCriacao = Workflow.DataHoraCriação,
                UtilizadorCriacao = Workflow.UtilizadorCriação,
                DataHoraModificacao = Workflow.DataHoraModificação,
                UtilizadorModificacao = Workflow.UtilizadorModificação
            });
        }

        public static EmailsProcedimentosCcp CastEmailProcedimentoViewToEmailProcedimento(EmailsProcedimentoCCPView EmailView)
        {
            return (new EmailsProcedimentosCcp()
            {
                NºProcedimento = EmailView.NoProcedimento,
                NºLinha = EmailView.NoLinha,
                Esclarecimento = EmailView.Esclarecimento,
                Resposta = EmailView.Resposta,
                DataHoraPedido = EmailView.DataHoraPedido,
                UtilizadorPedidoEscl = EmailView.UtilizadorPedidoEscl,
                DataHoraResposta = EmailView.DataHoraResposta,
                UtilizadorResposta = EmailView.UtilizadorResposta,
                Anexo = EmailView.Anexo,
                Anexo1 = EmailView.Anexo1,
                Email = EmailView.Email,
                Destinatário = EmailView.Destinatario,
                Assunto = EmailView.Assunto,
                DataHoraEmail = EmailView.DataHoraEmail,
                TextoEmail = EmailView.TextoEmail,
                EmailDestinatário = EmailView.EmailDestinatario,
                UtilizadorEmail = EmailView.UtilizadorEmail,
                DataHoraCriação = EmailView.DataHoraCriacao,
                DataHoraModificação = EmailView.DataHoraModificacao,
                UtilizadorCriação = EmailView.UtilizadorCriacao,
                UtilizadorModificação = EmailView.UtilizadorModificacao,

            });
        }
        public static EmailsProcedimentoCCPView CastEmailProcedimentoToEmailProcedimentoView(EmailsProcedimentosCcp Email)
        {
            return (new EmailsProcedimentoCCPView()
            {
                NoProcedimento = Email.NºProcedimento,
                NoLinha = Email.NºLinha,
                Esclarecimento = Email.Esclarecimento,
                Resposta = Email.Resposta,
                DataHoraPedido = Email.DataHoraPedido,
                UtilizadorPedidoEscl = Email.UtilizadorPedidoEscl,
                DataHoraResposta = Email.DataHoraResposta,
                UtilizadorResposta = Email.UtilizadorResposta,
                Anexo = Email.Anexo,
                Anexo1 = Email.Anexo1,
                Email = Email.Email,
                Destinatario = Email.Destinatário,
                Assunto = Email.Assunto,
                DataHoraEmail = Email.DataHoraEmail,
                TextoEmail = Email.TextoEmail,
                EmailDestinatario = Email.EmailDestinatário,
                UtilizadorEmail = Email.UtilizadorEmail,
                DataHoraCriacao = Email.DataHoraCriação,
                DataHoraModificacao = Email.DataHoraModificação,
                UtilizadorCriacao = Email.UtilizadorCriação,
                UtilizadorModificacao = Email.UtilizadorModificação
            });
        }

        public static LinhasPEncomendaProcedimentosCcp CastLinhaParaEncomendaProcedimentoViewToLinhaEncomendaProcedimento(LinhasParaEncomendaCCPView LinhaEncView)
        {
            return (new LinhasPEncomendaProcedimentosCcp()
            {
                NºProcedimento = LinhaEncView.NoProcedimento,
                NºLinha = LinhaEncView.NoLinha,
                Tipo = LinhaEncView.Tipo,
                Código = LinhaEncView.Codigo,
                CódLocalização = LinhaEncView.CodLocalizacao,
                Descrição = LinhaEncView.Descricao,
                CódUnidadeMedida = LinhaEncView.CodUnidadeMedida,
                CustoUnitário = LinhaEncView.CustoUnitário,
                QuantARequerer = LinhaEncView.QuantARequerer,
                CódigoRegião = LinhaEncView.CodigoRegiao,
                CódigoÁreaFuncional = LinhaEncView.CodigoAreaFuncional,
                CódigoCentroResponsabilidade = LinhaEncView.CodigoCentroResponsabilidade,
                NºProjeto = LinhaEncView.NoProjeto,
                NºRequisição = LinhaEncView.NoRequisicao,
                NºLinhaRequisição = LinhaEncView.NoLinhaRequisicao,
                DataHoraCriação = LinhaEncView.DataHoraCriacao,
                UtilizadorCriação = LinhaEncView.UtilizadorCriacao,
                DataHoraModificação = LinhaEncView.DataHoraModificacao,
                UtilizadorModificação = LinhaEncView.UtilizadorModificacao
            });
        }
        public static LinhasParaEncomendaCCPView CastLinhaParaEncomendaProcediementoToLinhaEncomendaCCPView(LinhasPEncomendaProcedimentosCcp LinhaEnc)
        {
            return (new LinhasParaEncomendaCCPView()
            {
                NoProcedimento = LinhaEnc.NºProcedimento,
                NoLinha = LinhaEnc.NºLinha,
                Tipo = LinhaEnc.Tipo,
                Codigo = LinhaEnc.Código,
                CodLocalizacao = LinhaEnc.CódLocalização,
                Descricao = LinhaEnc.Descrição,
                CodUnidadeMedida = LinhaEnc.CódUnidadeMedida,
                CustoUnitário = LinhaEnc.CustoUnitário,
                QuantARequerer = LinhaEnc.QuantARequerer,
                CodigoRegiao = LinhaEnc.CódigoRegião,
                CodigoAreaFuncional = LinhaEnc.CódigoÁreaFuncional,
                CodigoCentroResponsabilidade = LinhaEnc.CódigoCentroResponsabilidade,
                NoProjeto = LinhaEnc.NºProjeto,
                NoRequisicao = LinhaEnc.NºRequisição,
                NoLinhaRequisicao = LinhaEnc.NºLinhaRequisição,
                DataHoraCriacao = LinhaEnc.DataHoraCriação,
                UtilizadorCriacao = LinhaEnc.UtilizadorCriação,
                DataHoraModificacao = LinhaEnc.DataHoraModificação,
                UtilizadorModificacao = LinhaEnc.UtilizadorModificação
            });
        }
        #endregion
    }
}
