using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hydra.Such.Data.ViewModel.FH
{
    public static class FolhaDeHorasFunctions
    {
        public static FolhaDeHorasViewModel CastFolhaDeHorasToFolhaDeHorasViewModel(FolhasDeHoras FH)
        {
            return (new FolhaDeHorasViewModel
            {
                FolhaDeHorasNo = FH.NºFolhaDeHoras,
                Area = FH.Área,
                AreaTexto = FH.Área.ToString(),
                ProjetoNo = FH.NºProjeto,
                ProjetoDescricao = "",
                EmpregadoNo = FH.NºEmpregado,
                DataHoraPartida = FH.DataHoraPartida,
                DataPartidaTexto = FH.DataHoraPartida.Value.ToShortDateString(),
                HoraPartidaTexto = FH.DataHoraPartida.Value.ToShortTimeString(),
                DataHoraChegada = FH.DataHoraChegada,
                DataChegadaTexto = FH.DataHoraChegada.Value.ToShortDateString(),
                HoraChegadaTexto = FH.DataHoraChegada.Value.ToShortTimeString(),
                TipoDeslocacao = FH.TipoDeslocação,
                TipoDeslocacaoTexto = FH.TipoDeslocação.ToString(),
                CodigoTipoKms = FH.CódigoTipoKmS,
                DeslocacaoForaConcelho = FH.DeslocaçãoForaConcelho,
                DeslocacaoForaConcelhoTexto = FH.DeslocaçãoForaConcelho.ToString(),
                Validadores = FH.Validadores,
                Estado = FH.Estado,
                Estadotexto = FH.Estado.ToString(),
                CriadoPor = FH.CriadoPor,
                DataHoraCriacao = FH.DataHoraCriação,
                DataCriacaoTexto = FH.DataHoraCriação.Value.ToShortDateString(),
                HoraCriacaoTexto = FH.DataHoraCriação.Value.ToShortTimeString(),
                DataHoraUltimoEstado = FH.DataHoraÚltimoEstado,
                DataUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToShortDateString(),
                HoraUltimoEstadoTexto = FH.DataHoraÚltimoEstado.Value.ToShortTimeString(),
                //abarros_
                //UtilizadorCriacao = FH.UtilizadorCriação,
                DataHoraModificacao = FH.DataHoraModificação,
                DataModificacaoTexto = FH.DataHoraModificação.Value.ToShortDateString(),
                HoraModificacaoTexto = FH.DataHoraModificação.Value.ToShortTimeString(),
                UtilizadorModificacao = FH.UtilizadorModificação,
                EmpregadoNome = FH.NomeEmpregado,
                Matricula = FH.Matrícula,
                Terminada = FH.Terminada,
                TerminadaTexto = FH.Terminada.ToString(),
                TerminadoPor = FH.TerminadoPor,
                DataHoraTerminado = FH.DataHoraTerminado,
                DataTerminadoTexto = FH.DataHoraTerminado.Value.ToShortDateString(),
                HoraTerminadoTexto = FH.DataHoraTerminado.Value.ToShortTimeString(),
                Validado = FH.Validado,
                ValidadoTexto = FH.Validado.ToString(),
                DeslocacaoPlaneada = FH.DeslocaçãoPlaneada,
                DeslocacaoPlaneadaTexto = FH.DeslocaçãoPlaneada.ToString(),
                Observacoes = FH.Observações,
                Responsavel1No = FH.NºResponsável1,
                Responsavel2No = FH.NºResponsável2,
                Responsavel3No = FH.NºResponsável3,
                ValidadoresRHKM = FH.ValidadoresRhKm,
                CodigoRegiao = FH.CódigoRegião,
                CodigoAreaFuncional = FH.CódigoÁreaFuncional,
                CodigoCentroResponsabilidade = FH.CódigoCentroResponsabilidade,
                Validador = FH.Validador,
                DataHoraValidacao = FH.DataHoraValidação,
                DataValidacaoTexto = FH.DataHoraValidação.Value.ToShortDateString(),
                HoraValidacaoTexto = FH.DataHoraValidação.Value.ToShortTimeString(),
                IntegradorEmRH = FH.IntegradorEmRh,
                DataIntegracaoEmRH = FH.DataIntegraçãoEmRh,
                DataIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToShortDateString(),
                HoraIntegracaoEmRHTexto = FH.DataIntegraçãoEmRh.Value.ToShortTimeString(),
                IntegradorEmRHKM = FH.IntegradorEmRhKm,
                DataIntegracaoEmRHKM = FH.DataIntegraçãoEmRhKm,
                DataIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToShortDateString(),
                HoraIntegracaoEmRHKMTexto = FH.DataIntegraçãoEmRhKm.Value.ToShortTimeString()
            });
        }

        public static FolhasDeHoras CastFolhaDeHorasViewModelToFolhaDeHoras(FolhaDeHorasViewModel FH)
        {
            return (new FolhasDeHoras
            {
                NºFolhaDeHoras = FH.FolhaDeHorasNo,
                Área = Int32.Parse(FH.AreaTexto),
                NºProjeto = FH.ProjetoNo,
                //ProjetoDescricao = "",
                NºEmpregado = FH.EmpregadoNo,
                DataHoraPartida = DateTime.Parse(FH.DataPartidaTexto + " " + FH.HoraPartidaTexto),
                DataHoraChegada = DateTime.Parse(FH.DataChegadaTexto + " " + FH.HoraChegadaTexto),
                TipoDeslocação = Int32.Parse(FH.TipoDeslocacaoTexto),
                CódigoTipoKmS = FH.CodigoTipoKms,
                DeslocaçãoForaConcelho = Boolean.Parse(FH.DeslocacaoForaConcelhoTexto),
                Validadores = FH.Validadores,
                Estado = Int32.Parse(FH.Estadotexto),
                CriadoPor = FH.CriadoPor,
                DataHoraCriação = DateTime.Parse(FH.DataCriacaoTexto + " " + FH.HoraCriacaoTexto),
                DataHoraÚltimoEstado = DateTime.Parse(FH.DataUltimoEstadoTexto + " " + FH.HoraUltimoEstadoTexto),
                //abarros_
                //UtilizadorCriação = FH.UtilizadorCriacao,
                DataHoraModificação = DateTime.Parse(FH.DataModificacaoTexto + " " + FH.HoraModificacaoTexto),
                UtilizadorModificação = FH.UtilizadorModificacao,
                NomeEmpregado = FH.EmpregadoNome,
                Matrícula = FH.Matricula,
                Terminada = Boolean.Parse(FH.TerminadaTexto),
                TerminadoPor = FH.TerminadoPor,
                DataHoraTerminado = DateTime.Parse(FH.DataTerminadoTexto + " " + FH.HoraTerminadoTexto),
                Validado = Boolean.Parse(FH.ValidadoTexto),
                DeslocaçãoPlaneada = Boolean.Parse(FH.DeslocacaoPlaneadaTexto),
                Observações = FH.Observacoes,
                NºResponsável1 = FH.Responsavel1No,
                NºResponsável2 = FH.Responsavel2No,
                NºResponsável3 = FH.Responsavel3No,
                ValidadoresRhKm = FH.ValidadoresRHKM,
                CódigoRegião = FH.CodigoRegiao,
                CódigoÁreaFuncional = FH.CodigoAreaFuncional,
                CódigoCentroResponsabilidade = FH.CodigoCentroResponsabilidade,
                Validador = FH.Validador,
                DataHoraValidação = DateTime.Parse(FH.DataValidacaoTexto + " " + FH.HoraValidacaoTexto),
                IntegradorEmRh = FH.IntegradorEmRH,
                DataIntegraçãoEmRh = DateTime.Parse(FH.DataIntegracaoEmRHTexto + " " + FH.HoraIntegracaoEmRHTexto),
                IntegradorEmRhKm = FH.IntegradorEmRHKM,
                DataIntegraçãoEmRhKm = DateTime.Parse(FH.DataIntegracaoEmRHKMTexto + " " + FH.HoraIntegracaoEmRHKMTexto)
            });
        }

        public static PercursosEAjudasCustoDespesasFolhaDeHorasViewModel CastPercursoToPercursoViewModel(PercursosEAjudasCustoDespesasFolhaDeHoras Percurso)
        {
            return (new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel
            {
                FolhaDeHorasNo = Percurso.NºFolhaDeHoras,
                TipoCusto = Percurso.TipoCusto,
                LinhaNo = Percurso.NºLinha,
                Descricao = Percurso.Descrição,
                Origem = Percurso.Origem,
                Destino = Percurso.Destino,
                DataViagem = Percurso.DataViagem,
                DataViagemTexto = Percurso.DataViagem.Value.ToShortDateString(),
                Distancia = Convert.ToDecimal(Percurso.Distância),
                Quantidade = Convert.ToDecimal(Percurso.Quantidade),
                CustoUnitario = Convert.ToDecimal(Percurso.CustoUnitário),
                CustoTotal = Convert.ToDecimal(Percurso.CustoTotal),
                PrecoUnitario = Convert.ToDecimal(Percurso.PreçoUnitário),
                Justificacao = Percurso.Justificação,
                RubricaSalarial = Percurso.RúbricaSalarial,
                DataHoraCriacao = Percurso.DataHoraCriação,
                DataHoraCriacaoTexto = Percurso.DataHoraCriação.Value.ToShortDateString(),
                UtilizadorCriacao = Percurso.UtilizadorCriação,
                DataHoraModificacao = Percurso.DataHoraModificação,
                DataHoraModificacaoTexto = Percurso.DataHoraModificação.Value.ToShortDateString(),
                UtilizadorModificacao = Percurso.UtilizadorModificação
            });
        }


        public static PercursosEAjudasCustoDespesasFolhaDeHoras CastPercursoViewModelToPercurso(PercursosEAjudasCustoDespesasFolhaDeHorasViewModel Percurso)
        {
            return (new PercursosEAjudasCustoDespesasFolhaDeHoras
            {
                NºFolhaDeHoras = Percurso.FolhaDeHorasNo,
                TipoCusto = Convert.ToInt32(Percurso.TipoCusto),
                NºLinha = Convert.ToInt32(Percurso.LinhaNo),
                Descrição = Percurso.Descricao,
                Origem = Percurso.Origem,
                Destino = Percurso.Destino,
                DataViagem = Convert.ToDateTime(Percurso.DataViagemTexto),
                Distância = Percurso.Distancia,
                Quantidade = Percurso.Quantidade,
                CustoUnitário = Percurso.CustoUnitario,
                CustoTotal = Percurso.CustoTotal,
                PreçoUnitário = Percurso.PrecoUnitario,
                Justificação = Percurso.Justificacao,
                RúbricaSalarial = Percurso.RubricaSalarial,
                DataHoraCriação = Convert.ToDateTime(Percurso.DataHoraCriacaoTexto),
                UtilizadorCriação = Percurso.UtilizadorCriacao,
                DataHoraModificação = Convert.ToDateTime(Percurso.DataHoraModificacaoTexto),
                UtilizadorModificação = Percurso.UtilizadorModificacao
            });
        }

        public static PercursosEAjudasCustoDespesasFolhaDeHorasViewModel CastAjudaToAjudaViewModel(PercursosEAjudasCustoDespesasFolhaDeHoras Ajuda)
        {
            return (new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel
            {
                FolhaDeHorasNo = Ajuda.NºFolhaDeHoras,
                TipoCusto = Ajuda.TipoCusto,
                LinhaNo = Ajuda.NºLinha,
                Descricao = Ajuda.Descrição,
                Origem = Ajuda.Origem,
                Destino = Ajuda.Destino,
                DataViagem = Ajuda.DataViagem,
                DataViagemTexto = Ajuda.DataViagem.Value.ToShortDateString(),
                Distancia = Convert.ToDecimal(Ajuda.Distância),
                Quantidade = Convert.ToDecimal(Ajuda.Quantidade),
                CustoUnitario = Convert.ToDecimal(Ajuda.CustoUnitário),
                CustoTotal = Convert.ToDecimal(Ajuda.CustoTotal),
                PrecoUnitario = Convert.ToDecimal(Ajuda.PreçoUnitário),
                Justificacao = Ajuda.Justificação,
                RubricaSalarial = Ajuda.RúbricaSalarial,
                DataHoraCriacao = Ajuda.DataHoraCriação,
                DataHoraCriacaoTexto = Ajuda.DataHoraCriação.Value.ToShortDateString(),
                UtilizadorCriacao = Ajuda.UtilizadorCriação,
                DataHoraModificacao = Ajuda.DataHoraModificação,
                DataHoraModificacaoTexto = Ajuda.DataHoraModificação.Value.ToShortDateString(),
                UtilizadorModificacao = Ajuda.UtilizadorModificação
            });
        }


        public static PercursosEAjudasCustoDespesasFolhaDeHoras CastAjudaViewModelToAjuda(PercursosEAjudasCustoDespesasFolhaDeHorasViewModel Ajuda)
        {
            return (new PercursosEAjudasCustoDespesasFolhaDeHoras
            {
                NºFolhaDeHoras = Ajuda.FolhaDeHorasNo,
                TipoCusto = Convert.ToInt32(Ajuda.TipoCusto),
                NºLinha = Convert.ToInt32(Ajuda.LinhaNo),
                Descrição = Ajuda.Descricao,
                Origem = Ajuda.Origem,
                Destino = Ajuda.Destino,
                DataViagem = Convert.ToDateTime(Ajuda.DataViagemTexto),
                Distância = Ajuda.Distancia,
                Quantidade = Ajuda.Quantidade,
                CustoUnitário = Ajuda.CustoUnitario,
                CustoTotal = Ajuda.CustoTotal,
                PreçoUnitário = Ajuda.PrecoUnitario,
                Justificação = Ajuda.Justificacao,
                RúbricaSalarial = Ajuda.RubricaSalarial,
                DataHoraCriação = Convert.ToDateTime(Ajuda.DataHoraCriacaoTexto),
                UtilizadorCriação = Ajuda.UtilizadorCriacao,
                DataHoraModificação = Convert.ToDateTime(Ajuda.DataHoraModificacaoTexto),
                UtilizadorModificação = Ajuda.UtilizadorModificacao
            });
        }

        public static MaoDeObraFolhaDeHorasViewModel CastMaoDeObraToMaoDeObraViewModel(MãoDeObraFolhaDeHoras MaoDeObra)
        {
            return (new MaoDeObraFolhaDeHorasViewModel
            {
                FolhaDeHorasNo = MaoDeObra.NºFolhaDeHoras,
                LinhaNo = MaoDeObra.NºLinha,
                Date = MaoDeObra.Date,
                EmpregadoNo = MaoDeObra.NºEmpregado,
                ProjetoNo = MaoDeObra.NºProjeto,
                CodigoTipoTrabalho = MaoDeObra.CódigoTipoTrabalho,
                HoraInicio = Convert.ToDateTime("1753-01-01 " + MaoDeObra.HoraInício),
                HoraInicioTexto = MaoDeObra.HoraInício.ToString(),
                HoraFim = Convert.ToDateTime("1753-01-01 " + MaoDeObra.HoraFim),
                HoraFimTexto = MaoDeObra.HoraFim.ToString(),
                HorarioAlmoco = MaoDeObra.HorárioAlmoço,
                HorarioJantar = MaoDeObra.HorárioJantar,
                CodigoFamiliaRecurso = MaoDeObra.CódigoFamíliaRecurso,
                RecursoNo = MaoDeObra.NºRecurso,
                CodigoUnidadeMedida = MaoDeObra.CódUnidadeMedida,
                CodigoTipoOM = MaoDeObra.CódigoTipoOm,
                //abarros_
                //HorasNo = Convert.ToDateTime("1753-01-01 " + MaoDeObra.NºDeHotas),
                //HorasNoTexto = MaoDeObra.NºDeHotas.ToString(),
                CustoUnitarioDireto = Convert.ToDecimal(MaoDeObra.CustoUnitárioDireto),
                PrecoDeCusto = Convert.ToDecimal(MaoDeObra.PreçoDeCusto),
                PrecoDeVenda = Convert.ToDecimal(MaoDeObra.PreçoDeVenda),
                PrecoTotal = Convert.ToDecimal(MaoDeObra.PreçoTotal),
                DataHoraCriacao = MaoDeObra.DataHoraCriação,
                DataHoraCriacaoTexto = MaoDeObra.DataHoraCriação.Value.ToShortDateString(),
                UtilizadorCriacao = MaoDeObra.UtilizadorCriação,
                DataHoraModificacao = MaoDeObra.DataHoraModificação,
                DataHoraModificacaoTexto = MaoDeObra.DataHoraModificação.Value.ToShortDateString(),
                UtilizadorModificacao = MaoDeObra.UtilizadorModificação
            });
        }

        public static MãoDeObraFolhaDeHoras CastMaoDeObraViewModelToMaoDeObra(MaoDeObraFolhaDeHorasViewModel MaoDeObra)
        {
            return (new MãoDeObraFolhaDeHoras
            {
                NºFolhaDeHoras = MaoDeObra.FolhaDeHorasNo,
                NºLinha = Convert.ToInt32(MaoDeObra.LinhaNo),
                Date = MaoDeObra.Date,
                NºEmpregado = MaoDeObra.EmpregadoNo,
                NºProjeto = MaoDeObra.ProjetoNo,
                CódigoTipoTrabalho = MaoDeObra.CodigoTipoTrabalho,
                HoraInício = TimeSpan.Parse(MaoDeObra.HoraInicioTexto),
                HoraFim = TimeSpan.Parse(MaoDeObra.HoraFimTexto),
                HorárioAlmoço = MaoDeObra.HorarioAlmoco,
                HorárioJantar = MaoDeObra.HorarioJantar,
                CódigoFamíliaRecurso = MaoDeObra.CodigoFamiliaRecurso,
                NºRecurso = MaoDeObra.RecursoNo,
                CódUnidadeMedida = MaoDeObra.CodigoUnidadeMedida,
                CódigoTipoOm = MaoDeObra.CodigoTipoOM,
                //abarros_
                //NºDeHotas = TimeSpan.Parse(MaoDeObra.HorasNoTexto),
                CustoUnitárioDireto = MaoDeObra.CustoUnitarioDireto,
                PreçoDeCusto = MaoDeObra.PrecoDeCusto,
                PreçoDeVenda = MaoDeObra.PrecoDeVenda,
                PreçoTotal = MaoDeObra.PrecoTotal,
                DataHoraCriação = Convert.ToDateTime(MaoDeObra.DataHoraCriacaoTexto),
                UtilizadorCriação = MaoDeObra.UtilizadorCriacao,
                DataHoraModificação = Convert.ToDateTime(MaoDeObra.DataHoraModificacaoTexto),
                UtilizadorModificação = MaoDeObra.UtilizadorModificacao
            });
        }

        public static PresencasFolhaDeHorasViewModel CastPresencaToPresencaViewModel(PresençasFolhaDeHoras Presenca)
        {
            return (new PresencasFolhaDeHorasViewModel
            {
                FolhaDeHorasNo = Presenca.NºFolhaDeHoras,
                Data = Presenca.Data,
                DataTexto = Presenca.Data.ToShortDateString(),
                Hora1Entrada = Presenca.Hora1ªEntrada.ToString(),
                Hora1Saida = Presenca.Hora1ªSaída.ToString(),
                Hora2Entrada = Presenca.Hora2ªEntrada.ToString(),
                Hora2Saida = Presenca.Hora2ªSaída.ToString(),
                DataHoraCriacao = Presenca.DataHoraCriação,
                DataHoraCriacaoTexto = Presenca.DataHoraCriação.Value.ToShortDateString(),
                UtilizadorCriacao = Presenca.UtilizadorCriação,
                DataHoraModificacao = Presenca.DataHoraModificação,
                DataHoraModificacaoTexto = Presenca.DataHoraModificação.Value.ToShortDateString(),
                UtilizadorModificacao = Presenca.UtilizadorModificação
            });
        }

        public static PresençasFolhaDeHoras CastPresencaViewModelToPresenca(PresencasFolhaDeHorasViewModel Presenca)
        {
            return (new PresençasFolhaDeHoras
            {
                NºFolhaDeHoras = Presenca.FolhaDeHorasNo,
                Data = Convert.ToDateTime(Presenca.DataTexto),
                Hora1ªEntrada = TimeSpan.Parse(Presenca.Hora1Entrada),
                Hora1ªSaída = TimeSpan.Parse(Presenca.Hora1Saida),
                Hora2ªEntrada = TimeSpan.Parse(Presenca.Hora2Entrada),
                Hora2ªSaída = TimeSpan.Parse(Presenca.Hora2Saida),
                DataHoraCriação = Convert.ToDateTime(Presenca.DataHoraCriacaoTexto),
                UtilizadorCriação = Presenca.UtilizadorCriacao,
                DataHoraModificação = Convert.ToDateTime(Presenca.DataHoraModificacaoTexto),
                UtilizadorModificação = Presenca.UtilizadorModificacao
            });
        }
    }
}
