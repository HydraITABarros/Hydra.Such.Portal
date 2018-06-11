using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.CCP;
using Hydra.Such.Data.ViewModel.CCP;
using System;
using System.Collections.Generic;
using System.Linq;

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
            DateTime? Data_Publicacao_Show = ProcedimentoView.DataPublicacao;
            DateTime? Data_Recolha_Show = ProcedimentoView.DataRecolha;
            DateTime? Data_Valid_Relatorio_Preliminar_Show = ProcedimentoView.DataValidRelatorioPreliminar;
            DateTime? Data_Audiencia_Previa_Show = ProcedimentoView.DataAudienciaPrevia;
            DateTime? Data_Relatorio_Final_Show = ProcedimentoView.DataRelatorioFinal;
            DateTime? Data_Notificacao_Show = ProcedimentoView.DataNotificacao;

            if (ProcedimentoView.DataPublicacao.ToString() == "")
            {
                Data_Publicacao_Show = ProcedimentoView.DataPublicacao_Show == "" ? ProcedimentoView.DataPublicacao : Convert.ToDateTime(ProcedimentoView.DataPublicacao_Show);
            }
            if (ProcedimentoView.DataRecolha.ToString() == "")
            {
                Data_Recolha_Show = ProcedimentoView.DataRecolha_Show == "" ? ProcedimentoView.DataRecolha : Convert.ToDateTime(ProcedimentoView.DataRecolha_Show);
            }
            if (ProcedimentoView.DataValidRelatorioPreliminar.ToString() == "")
            {
                Data_Valid_Relatorio_Preliminar_Show = ProcedimentoView.DataValidRelatorioPreliminar_Show == "" ? ProcedimentoView.DataValidRelatorioPreliminar : Convert.ToDateTime(ProcedimentoView.DataValidRelatorioPreliminar_Show);
            }
            if (ProcedimentoView.DataAudienciaPrevia.ToString() == "")
            {
                Data_Audiencia_Previa_Show = ProcedimentoView.DataAudienciaPrevia_Show == "" ? ProcedimentoView.DataAudienciaPrevia : Convert.ToDateTime(ProcedimentoView.DataAudienciaPrevia_Show);
            }
            if (ProcedimentoView.DataRelatorioFinal.ToString() == "")
            {
                Data_Relatorio_Final_Show = ProcedimentoView.DataRelatorioFinal_Show == "" ? ProcedimentoView.DataRelatorioFinal : Convert.ToDateTime(ProcedimentoView.DataRelatorioFinal_Show);
            }
            if (ProcedimentoView.DataNotificacao.ToString() == "")
            {
                Data_Notificacao_Show = ProcedimentoView.DataNotificacao_Show == "" ? ProcedimentoView.DataNotificacao : Convert.ToDateTime(ProcedimentoView.DataNotificacao_Show);
            }

            ProcedimentosCcp Procedimento = new ProcedimentosCcp
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
                ValorDecisãoContratar = ProcedimentoView.ValorDecisaoContratar == null ? 0 : ProcedimentoView.ValorDecisaoContratar,
                ValorAdjudicaçãoAnteriro = ProcedimentoView.ValorAdjudicacaoAnteriro == null ? 0 : ProcedimentoView.ValorAdjudicacaoAnteriro,
                ValorAdjudicaçãoAtual = ProcedimentoView.ValorAdjudicacaoAtual == null ? 0 : ProcedimentoView.ValorAdjudicacaoAtual,
                DiferençaEuros = ProcedimentoView.DiferencaEuros == null ? 0 : ProcedimentoView.DiferencaEuros,
                DiferençaPercent = ProcedimentoView.DiferencaPercent == null ? 0 : ProcedimentoView.DiferencaPercent,
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

                //NR 20180314
                //DataPublicação = ProcedimentoView.DataPublicacao,
                DataPublicação = Data_Publicacao_Show,

                UtilizadorPublicação = ProcedimentoView.UtilizadorPublicacao,
                DataSistemaPublicação = ProcedimentoView.DataSistemaPublicacao,
                RecolhaComentário = ProcedimentoView.RecolhaComentario,

                //NR 20180314
                //DataRecolha = ProcedimentoView.DataRecolha,
                DataRecolha = Data_Recolha_Show,

                UtilizadorRecolha = ProcedimentoView.UtilizadorRecolha,
                DataSistemaRecolha = ProcedimentoView.DataSistemaRecolha,
                ComentárioRelatórioPreliminar = ProcedimentoView.ComentarioRelatorioPreliminar,

                //NR 20180315
                //DataValidRelatórioPreliminar = ProcedimentoView.DataValidRelatorioPreliminar,
                DataValidRelatórioPreliminar = Data_Valid_Relatorio_Preliminar_Show,

                UtilizadorValidRelatórioPreliminar = ProcedimentoView.UtilizadorValidRelatorioPreliminar,
                DataSistemaValidRelatórioPreliminar = ProcedimentoView.DataSistemaValidRelatorioPreliminar,
                ComentárioAudiênciaPrévia = ProcedimentoView.ComentarioAudienciaPrevia,

                //NR 20180315
                //DataAudiênciaPrévia = ProcedimentoView.DataAudienciaPrevia,
                DataAudiênciaPrévia = Data_Audiencia_Previa_Show,

                UtilizadorAudiênciaPrévia = ProcedimentoView.UtilizadorAudienciaPrevia,
                DataSistemaAudiênciaPrévia = ProcedimentoView.DataSistemaAudienciaPrevia,
                ComentárioRelatórioFinal = ProcedimentoView.ComentarioRelatorioFinal,

                //NR 20180315
                //DataRelatórioFinal = ProcedimentoView.DataRelatorioFinal,
                DataRelatórioFinal = Data_Relatorio_Final_Show,

                UtilizadorRelatórioFinal = ProcedimentoView.UtilizadorRelatorioFinal,
                DataSistemaRelatórioFinal = ProcedimentoView.DataSistemaRelatorioFinal,
                ComentárioNotificação = ProcedimentoView.ComentarioNotificacao,

                //NR 20180327
                //DataNotificação = ProcedimentoView.DataNotificacao,
                DataNotificação = Data_Notificacao_Show,

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
                UtilizadorModificação = ProcedimentoView.UtilizadorModificacao
            };

            if (ProcedimentoView.TemposPaCcp != null)
            {
                Procedimento.TemposPaCcp = CCPFunctions.CastTemposCCPViewToTemposPaCcp(ProcedimentoView.TemposPaCcp);
            }

            if(ProcedimentoView.RegistoDeAtas != null && ProcedimentoView.RegistoDeAtas.Count > 0)
            {
                List<RegistoDeAtas> Actas = new List<RegistoDeAtas>();
                foreach(var ra in ProcedimentoView.RegistoDeAtas)
                {
                    Actas.Add(CastRegistoActasViewToRegistoActas(ra));
                }

                Procedimento.RegistoDeAtas = Actas;
            }

            if(ProcedimentoView.ElementosJuri != null && ProcedimentoView.ElementosJuri.Count > 0)
            {
                List<ElementosJuri> Elementos = new List<ElementosJuri>();
                foreach(var ej in ProcedimentoView.ElementosJuri)
                {
                    Elementos.Add(CastElementosJuriViewToElementosJuri(ej));
                }

                Procedimento.ElementosJuri = Elementos;
            }

            if(ProcedimentoView.EmailsProcedimentosCcp != null && ProcedimentoView.EmailsProcedimentosCcp.Count > 0)
            {
                List<EmailsProcedimentosCcp> Emails = new List<EmailsProcedimentosCcp>();
                foreach(var em in ProcedimentoView.EmailsProcedimentosCcp)
                {
                    Emails.Add(CastEmailProcedimentoViewToEmailProcedimento(em));
                }

                Procedimento.EmailsProcedimentosCcp = Emails;
            }

            if(ProcedimentoView.LinhasPEncomendaProcedimentosCcp != null && ProcedimentoView.LinhasPEncomendaProcedimentosCcp.Count > 0)
            {
                List<LinhasPEncomendaProcedimentosCcp> LinhasParaEncomenda = new List<LinhasPEncomendaProcedimentosCcp>();
                foreach(var le in ProcedimentoView.LinhasPEncomendaProcedimentosCcp)
                {
                    LinhasParaEncomenda.Add(CastLinhaParaEncomendaProcedimentoViewToLinhaEncomendaProcedimento(le));
                }

                Procedimento.LinhasPEncomendaProcedimentosCcp = LinhasParaEncomenda;
            }

            if(ProcedimentoView.NotasProcedimentosCcp != null && ProcedimentoView.NotasProcedimentosCcp.Count > 0)
            {
                List<NotasProcedimentosCcp> Notas = new List<NotasProcedimentosCcp>();
                foreach(var np in ProcedimentoView.NotasProcedimentosCcp)
                {
                    Notas.Add(CastNotaProcedimentoViewToNotaProcedimento(np));
                }

                Procedimento.NotasProcedimentosCcp = Notas;
            }

            if(ProcedimentoView.WorkflowProcedimentosCcp != null && ProcedimentoView.WorkflowProcedimentosCcp.Count > 0)
            {
                List<WorkflowProcedimentosCcp> Workflows = new List<WorkflowProcedimentosCcp>();
                foreach(var wf in ProcedimentoView.WorkflowProcedimentosCcp)
                {
                    Workflows.Add(CastWorkflowProcedimentoViewToWorkflowProcedimento(wf));
                }

                Procedimento.WorkflowProcedimentosCcp = Workflows;
            }

            if(ProcedimentoView.FluxoTrabalhoListaControlo != null && ProcedimentoView.FluxoTrabalhoListaControlo.Count > 0)
            {
                Procedimento.FluxoTrabalhoListaControlo = ProcedimentoView.FluxoTrabalhoListaControlo;
            }

            return Procedimento;

        }
        // this function receives an ProcedimentosCcp objet and maps it to a ProcedimentosCccpView object
        public static ProcedimentoCCPView CastProcedimentoCcpToProcedimentoCcpView(ProcedimentosCcp Procedimento)
        {
            string temp = string.Empty;

            temp = Procedimento.CódigoÁreaFuncional != null ? Procedimento.CódigoÁreaFuncional.StartsWith("0") ? "orange" : Procedimento.CódigoÁreaFuncional.StartsWith("1") ? "red" : Procedimento.CódigoÁreaFuncional.StartsWith("22") ? "blue" : Procedimento.CódigoÁreaFuncional.StartsWith("5") ? "tomato" : Procedimento.CódigoÁreaFuncional.StartsWith("23") ? "green" : Procedimento.CódigoÁreaFuncional.StartsWith("27") ? "purple" : "Transparent" : "Transparent";


            ProcedimentoCCPView ProcedimentoView = new ProcedimentoCCPView()
            {
                No = Procedimento.Nº,
                Tipo = Procedimento.Tipo,
                
                Tipo_Show = Procedimento.Tipo == 0 ? "" : Procedimento.Tipo == 1 ? "AD" : Procedimento.Tipo == 2 ? "CP" : Procedimento.Tipo == 3 ? "CLPQ" : Procedimento.Tipo == 4 ? "PN" : Procedimento.Tipo == 5 ? "DC" : Procedimento.Tipo == 6 ? "CPI" : "",

                Cor_Folder = Procedimento.CódigoÁreaFuncional != null ? Procedimento.CódigoÁreaFuncional.StartsWith("0") ? "orange" : Procedimento.CódigoÁreaFuncional.StartsWith("1") ? "red" : Procedimento.CódigoÁreaFuncional.StartsWith("22") ? "blue" : Procedimento.CódigoÁreaFuncional.StartsWith("5") ? "tomato" : Procedimento.CódigoÁreaFuncional.StartsWith("23") ? "green" : Procedimento.CódigoÁreaFuncional.StartsWith("27") ? "purple" : "Transparent" : "Transparent",

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

                PrecoBase_Show = Procedimento.PreçoBase == null ? "Não" : Procedimento.PreçoBase == false ? "Não" : "Sim",

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
                ValorDecisaoContratar = Procedimento.ValorDecisãoContratar == null ? 0 : Procedimento.ValorDecisãoContratar,
                ValorAdjudicacaoAnteriro = Procedimento.ValorAdjudicaçãoAnteriro == null ? 0 : Procedimento.ValorAdjudicaçãoAnteriro,
                ValorAdjudicacaoAtual = Procedimento.ValorAdjudicaçãoAtual == null ? 0 : Procedimento.ValorAdjudicaçãoAtual,
                DiferencaEuros = Procedimento.DiferençaEuros == null ? 0 : Procedimento.DiferençaEuros,
                DiferencaPercent = Procedimento.DiferençaPercent == null ? 0 : Procedimento.DiferençaPercent,
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

                //NR 20180314
                DataPublicacao_Show = Procedimento.DataPublicação.ToString(),

                UtilizadorPublicacao = Procedimento.UtilizadorPublicação,
                DataSistemaPublicacao = Procedimento.DataSistemaPublicação,
                RecolhaComentario = Procedimento.RecolhaComentário,
                DataRecolha = Procedimento.DataRecolha,

                //NR 20180314
                DataRecolha_Show = Procedimento.DataRecolha.ToString(),

                UtilizadorRecolha = Procedimento.UtilizadorRecolha,
                DataSistemaRecolha = Procedimento.DataSistemaRecolha,
                ComentarioRelatorioPreliminar = Procedimento.ComentárioRelatórioPreliminar,
                DataValidRelatorioPreliminar = Procedimento.DataValidRelatórioPreliminar,

                //NR 20180315
                DataValidRelatorioPreliminar_Show = Procedimento.DataValidRelatórioPreliminar.ToString(),
                
                UtilizadorValidRelatorioPreliminar = Procedimento.UtilizadorValidRelatórioPreliminar,
                DataSistemaValidRelatorioPreliminar = Procedimento.DataSistemaValidRelatórioPreliminar,
                ComentarioAudienciaPrevia = Procedimento.ComentárioAudiênciaPrévia,
                DataAudienciaPrevia = Procedimento.DataAudiênciaPrévia,

                //NR 20180315
                DataAudienciaPrevia_Show = Procedimento.DataAudiênciaPrévia.ToString(),

                UtilizadorAudienciaPrevia = Procedimento.UtilizadorAudiênciaPrévia,
                DataSistemaAudienciaPrevia = Procedimento.DataSistemaAudiênciaPrévia,
                ComentarioRelatorioFinal = Procedimento.ComentárioRelatórioFinal,
                DataRelatorioFinal = Procedimento.DataRelatórioFinal,

                //NR 20180315
                DataRelatorioFinal_Show = Procedimento.DataRelatórioFinal.ToString(),

                UtilizadorRelatorioFinal = Procedimento.UtilizadorRelatórioFinal,
                DataSistemaRelatorioFinal = Procedimento.DataSistemaRelatórioFinal,
                ComentarioNotificacao = Procedimento.ComentárioNotificação,
                DataNotificacao = Procedimento.DataNotificação,
                
                //NR 20180327
                DataNotificacao_Show = Procedimento.DataNotificação.ToString(),

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
            };

            if (Procedimento.TemposPaCcp != null)
            {
                ProcedimentoView.TemposPaCcp = CCPFunctions.CastTemposPaCcpToTemposCCPView(Procedimento.TemposPaCcp);
            }

            if(Procedimento.RegistoDeAtas != null && Procedimento.RegistoDeAtas.Count > 0)
            {
                List<RegistoActasView> ActasList = new List<RegistoActasView>();
                foreach(var ra in Procedimento.RegistoDeAtas)
                {
                    ActasList.Add(CastRegistoActasToRegistoActasView(ra));
                }

                ProcedimentoView.RegistoDeAtas = ActasList;
            }

            if(Procedimento.ElementosJuri != null && Procedimento.ElementosJuri.Count > 0)
            {
                List<ElementosJuriView> ElementosList = new List<ElementosJuriView>();
                foreach(var ej in Procedimento.ElementosJuri)
                {
                    ElementosList.Add(CastElementosJuriToElementosJuriView(ej));
                }
                ProcedimentoView.ElementosJuri = ElementosList;
            }
            
            if(Procedimento.EmailsProcedimentosCcp != null && Procedimento.EmailsProcedimentosCcp.Count > 0)
            {
                
                ProcedimentoView.EmailsProcedimentosCcp = DBProcedimentosCCP.GetAllEmailsView(Procedimento);
            }

            if(Procedimento.LinhasPEncomendaProcedimentosCcp != null && Procedimento.LinhasPEncomendaProcedimentosCcp.Count > 0)
            {
                ProcedimentoView.LinhasPEncomendaProcedimentosCcp = DBProcedimentosCCP.GetAllLinhasParaEncomendaView(Procedimento);
            }

            if(Procedimento.NotasProcedimentosCcp != null && Procedimento.NotasProcedimentosCcp.Count > 0)
            {
                ProcedimentoView.NotasProcedimentosCcp = DBProcedimentosCCP.GetAllNotasProcedimentoView(Procedimento);
            }

            if(Procedimento.WorkflowProcedimentosCcp != null && Procedimento.WorkflowProcedimentosCcp.Count > 0)
            {
                ProcedimentoView.WorkflowProcedimentosCcp = DBProcedimentosCCP.GetAllWorkflowsView(Procedimento);
            }

            if(Procedimento.FluxoTrabalhoListaControlo != null && Procedimento.FluxoTrabalhoListaControlo.Count > 0)
            {
                ProcedimentoView.FluxoTrabalhoListaControlo = Procedimento.FluxoTrabalhoListaControlo;

                ProcedimentoView.FluxoTrabalhoListaControlo_Show = DBProcedimentosCCP.GetAllFluxoTrabalhoListaControloCCPView(Procedimento);
            }
            return ProcedimentoView;
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
                UtilizadorModificacao = Acta.UtilizadorModificação,
                DataDaActa_Show = Acta.DataDaAta.HasValue ? Acta.DataDaAta.Value.ToShortDateString() : string.Empty
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
            int sum_estados_atribuidos = (Tempos.Estado0.HasValue ? Tempos.Estado0.Value : 0) + (Tempos.Estado1.HasValue ? Tempos.Estado1.Value : 0) + (Tempos.Estado2.HasValue ? Tempos.Estado2.Value : 0) + (Tempos.Estado3.HasValue ? Tempos.Estado3.Value : 0) + (Tempos.Estado4.HasValue ? Tempos.Estado4.Value : 0) + (Tempos.Estado5.HasValue ? Tempos.Estado5.Value : 0) + (Tempos.Estado6.HasValue ? Tempos.Estado6.Value : 0) + (Tempos.Estado7.HasValue ? Tempos.Estado7.Value : 0) + (Tempos.Estado8.HasValue ? Tempos.Estado8.Value : 0) + (Tempos.Estado9.HasValue ? Tempos.Estado9.Value : 0) + (Tempos.Estado10.HasValue ? Tempos.Estado10.Value : 0) + (Tempos.Estado11.HasValue ? Tempos.Estado11.Value : 0) + (Tempos.Estado12.HasValue ? Tempos.Estado12.Value : 0) + (Tempos.Estado13.HasValue ? Tempos.Estado13.Value : 0) + (Tempos.Estado14.HasValue ? Tempos.Estado14.Value : 0) + (Tempos.Estado15.HasValue ? Tempos.Estado15.Value : 0) + (Tempos.Estado16.HasValue ? Tempos.Estado16.Value : 0) + (Tempos.Estado17.HasValue ? Tempos.Estado17.Value : 0) + (Tempos.Estado18.HasValue ? Tempos.Estado18.Value : 0) + (Tempos.Estado19.HasValue ? Tempos.Estado19.Value : 0) + (Tempos.Estado20.HasValue ? Tempos.Estado20.Value : 0);
            int sum_estados_gastos = (Tempos.Estado0Tg.HasValue ? Tempos.Estado0Tg.Value : 0) + (Tempos.Estado1Tg.HasValue ? Tempos.Estado1Tg.Value : 0) + (Tempos.Estado2Tg.HasValue ? Tempos.Estado2Tg.Value : 0) + (Tempos.Estado3Tg.HasValue ? Tempos.Estado3Tg.Value : 0) + (Tempos.Estado4Tg.HasValue ? Tempos.Estado4Tg.Value : 0) + (Tempos.Estado5Tg.HasValue ? Tempos.Estado5Tg.Value : 0) + (Tempos.Estado6Tg.HasValue ? Tempos.Estado6Tg.Value : 0) + (Tempos.Estado7Tg.HasValue ? Tempos.Estado7Tg.Value : 0) + (Tempos.Estado8Tg.HasValue ? Tempos.Estado8Tg.Value : 0) + (Tempos.Estado9Tg.HasValue ? Tempos.Estado9Tg.Value : 0) + (Tempos.Estado10Tg.HasValue ? Tempos.Estado10Tg.Value : 0) + (Tempos.Estado11Tg.HasValue ? Tempos.Estado11Tg.Value : 0) + (Tempos.Estado12Tg.HasValue ? Tempos.Estado12Tg.Value : 0) + (Tempos.Estado13Tg.HasValue ? Tempos.Estado13Tg.Value : 0) + (Tempos.Estado14Tg.HasValue ? Tempos.Estado14Tg.Value : 0) + (Tempos.Estado15Tg.HasValue ? Tempos.Estado15Tg.Value : 0) + (Tempos.Estado16Tg.HasValue ? Tempos.Estado16Tg.Value : 0) + (Tempos.Estado17Tg.HasValue ? Tempos.Estado17Tg.Value : 0) + (Tempos.Estado18Tg.HasValue ? Tempos.Estado18Tg.Value : 0) + (Tempos.Estado19Tg.HasValue ? Tempos.Estado19Tg.Value : 0) + (Tempos.Estado20Tg.HasValue ? Tempos.Estado20Tg.Value : 0);

            int sum_estados_atribuidos_percentagem = 0;
            int sum_estados_gastos_percentagem = 0;

            if (sum_estados_atribuidos >= sum_estados_gastos)
            {
                if (sum_estados_atribuidos >= 100)
                {
                    sum_estados_atribuidos_percentagem = 100;
                    sum_estados_gastos_percentagem = Convert.ToInt32((100 * sum_estados_gastos) / sum_estados_atribuidos);
                }
                else
                {
                    sum_estados_atribuidos_percentagem = sum_estados_atribuidos;
                    sum_estados_gastos_percentagem = sum_estados_gastos;
                }
            }
            else
            {
                if (sum_estados_gastos >= 100)
                {
                    sum_estados_gastos_percentagem = 100;
                    sum_estados_atribuidos_percentagem = Convert.ToInt32((100 * sum_estados_atribuidos) / sum_estados_gastos);
                }
                else
                {
                    sum_estados_gastos_percentagem = sum_estados_gastos;
                    sum_estados_atribuidos_percentagem = sum_estados_atribuidos;
                }
            }


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
                UtilizadorModificacao = Tempos.UtilizadorModificação,
                
                //NR 20180403
                SomaEstadosAtribuidos = sum_estados_atribuidos,
                SomaEstadosGastos = sum_estados_gastos,
                SomaEstadosAtribuidos_percentagem = sum_estados_atribuidos_percentagem,
                SomaEstadosGastos_percentagem = sum_estados_gastos_percentagem
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
            ElementosJuriView ElementoJuriV = new ElementosJuriView()
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
            };

            ElementoJuriV.NomeEmpregado = DBProcedimentosCCP.GetUserName(ElementoJuriV.Utilizador);

            return ElementoJuriV;
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
                UtilizadorModificacao = Nota.UtilizadorModificação,
                DataHora_Show = Nota.DataHora.HasValue ? Nota.DataHora.Value.ToShortDateString() : string.Empty
            });
        }

        public static WorkflowProcedimentosCcp CastWorkflowProcedimentoViewToWorkflowProcedimento(WorkflowProcedimentosCCPView WorkflowView)
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
        public static WorkflowProcedimentosCCPView CastWorkflowProcedimentoToWorkflowProcedimentoView(WorkflowProcedimentosCcp Workflow)
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

        //NR 20180327
        public static FluxoTrabalhoListaControloCCPView CastFluxoTrabalhoListaControloToFluxoTrabalhoListaControlo_Show(FluxoTrabalhoListaControlo Fluxo)
        {
            return (new FluxoTrabalhoListaControloCCPView()
            {
                No = Fluxo.No,
                Estado = Fluxo.Estado,
                Data = Fluxo.Data,
                Hora = Fluxo.Hora,
                Data_Show = Fluxo.Data.ToShortDateString(),
                //Hora_Show = Fluxo.Hora.Hours.ToString() + ":" + Fluxo.Hora.Minutes.ToString() + ":" + Fluxo.Hora.Seconds.ToString(),
                Hora_Show = Fluxo.Hora.ToString(@"hh\:mm\:ss"),
                TipoEstado = Fluxo.TipoEstado,
                Comentario = Fluxo.Comentario,
                Resposta = Fluxo.Resposta,
                TipoResposta = Fluxo.TipoResposta,
                DataResposta = Fluxo.DataResposta,
                User = Fluxo.User,
                NomeUser = Fluxo.NomeUser,
                ImobSimNao = Fluxo.ImobSimNao,
                EstadoAnterior = Fluxo.EstadoAnterior,
                EstadoSeguinte = Fluxo.EstadoSeguinte,
                Comentario2 = Fluxo.Comentario2,
                UtilizadorCriacao = Fluxo.UtilizadorCriacao,
                DataHoraCriacao = Fluxo.DataHoraCriacao,
                UtilizadorModificacao = Fluxo.UtilizadorModificacao,
                DataHoraModificacao = Fluxo.DataHoraModificacao,
                Nr_Workflow = Fluxo.Estado > Fluxo.EstadoSeguinte ? "Vermelho" : "Verde"
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
            TimeSpan _datahoraemail = Email.DataHoraEmail.HasValue ? new TimeSpan(0, Email.DataHoraEmail.Value.Hour, Email.DataHoraEmail.Value.Minute, Email.DataHoraEmail.Value.Second, Email.DataHoraEmail.Value.Millisecond) : TimeSpan.Zero;

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
                UtilizadorModificacao = Email.UtilizadorModificação,
                DataEmail = Email.DataHoraEmail.HasValue ? Email.DataHoraEmail.Value.ToShortDateString() : string.Empty,
                HoraEmail = Email.DataHoraEmail.HasValue ? _datahoraemail.ToString(@"hh\:mm\:ss") : string.Empty
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
                CustoUnitário = LinhaEncView.CustoUnitario,
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
                TipoText = LinhaEnc.Tipo == 0 ? "-" : "Produto",
                Codigo = LinhaEnc.Código,
                CodLocalizacao = LinhaEnc.CódLocalização,
                Descricao = LinhaEnc.Descrição,
                CodUnidadeMedida = LinhaEnc.CódUnidadeMedida,
                CustoUnitario = LinhaEnc.CustoUnitário,
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


        #region Procedures used in the email automation
        public static string MakeEmailBodyContent(string BodyText, string SenderName)
        {
            string Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:600px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Exmos (as) Senhores (as)," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                BodyText +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            return Body;
        } 
        #endregion
    }
}
