using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViatura
    {

        public static List<Viaturas> GetAllToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas.Where(p => p.Matrícula == Matricula).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas Create(Viaturas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.Viaturas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas Update(Viaturas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.Viaturas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas ParseToDB(ViaturasViewModel x)
        {
            Viaturas viatura = new Viaturas();
            viatura.Matrícula = x.Matricula;
            viatura.NºQuadro = x.NQuadro;
            viatura.Estado = x.Estado;
            viatura.CódigoTipoViatura = Int32.Parse(x.CodigoTipoViatura);
            viatura.CódigoMarca = Int32.Parse(x.CodigoMarca);
            if (!string.IsNullOrEmpty(x.CodigoModelo))
                viatura.CódigoModelo = Int32.Parse(x.CodigoModelo);
            viatura.CartãoCombustível = x.CartaoCombustivel;
            viatura.Apólice = x.Apolice;
            viatura.PesoBruto = x.PesoBruto;
            viatura.Tara = x.Tara;
            viatura.Cilindrada = x.Cilindrada;
            viatura.TipoCombustível = x.TipoCombustivel;
            viatura.NºLugares = x.NLugares;
            viatura.Cor = x.Cor;
            viatura.AtribuídaA = x.AtribuidaA;
            viatura.CódigoRegião = x.CodigoRegiao;
            viatura.CódigoÁreaFuncional = x.CodigoAreaFuncional;
            viatura.CódigoCentroResponsabilidade = x.CodigoCentroResponsabilidade;
            viatura.TipoPropriedade = x.TipoPropriedade;
            viatura.NºImobilizado = x.NImobilizado;
            viatura.UtilizadorCriação = x.UtilizadorCriacao;
            viatura.UtilizadorModificação = x.UtilizadorModificacao;
            viatura.LocalParqueamento = x.LocalParqueamento;
            viatura.Observações = x.Observacoes;
            viatura.Potência = x.Potencia;
            viatura.DistânciaEntreEixos = x.DistanciaEntreEixos;
            viatura.PneumáticosFrente = x.PneumaticosFrente;
            viatura.PneumáticosRetaguarda = x.PneumaticosRetaguarda;
            viatura.ConsumoIndicativo = x.ConsumoIndicativo;
            viatura.CartaVerde = x.CartaVerde;
            viatura.NºViaVerde = x.NViaVerde;
            viatura.ValorAquisição = x.ValorAquisicao;
            viatura.ValorVenda = x.ValorVenda;
            //Imagem = x.Imagem,
            viatura.KmUltimaRevisão = x.KmUltimaRevisao;
            viatura.IntervaloRevisões = x.IntervaloRevisoes;
            viatura.DuraçãoPneus = x.DuracaoPneus;
            viatura.NoProjeto = x.NoProjeto;

            viatura.DataMatrícula = x.DataMatricula != "" && x.DataMatricula != null ? DateTime.Parse(x.DataMatricula) : (DateTime?)null;
            viatura.DataHoraModificação = x.DataHoraModificacao != "" && x.DataHoraModificacao != null ? DateTime.Parse(x.DataHoraModificacao) : (DateTime?)null;
            viatura.DataAbate = x.DataAbate != "" && x.DataAbate != null ? DateTime.Parse(x.DataAbate) : (DateTime?)null;
            viatura.DataAquisição = x.DataAquisicao != "" && x.DataAquisicao != null ? DateTime.Parse(x.DataAquisicao) : (DateTime?)null;
            viatura.DataEntradaFuncionamento = x.DataEntradaFuncionamento != "" && x.DataEntradaFuncionamento != null ? DateTime.Parse(x.DataEntradaFuncionamento) : (DateTime?)null;
            viatura.DataHoraCriação = x.DataHoraCriacao != "" && x.DataHoraCriacao != null ? DateTime.Parse(x.DataHoraCriacao) : (DateTime?)null;
            viatura.DataUltimaInspeção = x.DataUltimaInspecao != "" && x.DataUltimaInspecao != null ? DateTime.Parse(x.DataUltimaInspecao) : (DateTime?)null;
            viatura.DataUltimaRevisão = x.DataUltimaRevisao != "" && x.DataUltimaRevisao != null ? DateTime.Parse(x.DataUltimaRevisao) : (DateTime?)null;
            viatura.ValidadeApólice = x.ValidadeApolice != "" && x.ValidadeApolice != null ? DateTime.Parse(x.ValidadeApolice) : (DateTime?)null;
            viatura.ValidadeCartaVerde = x.ValidadeCartaVerde != "" && x.ValidadeCartaVerde != null ? DateTime.Parse(x.ValidadeCartaVerde) : (DateTime?)null;
            viatura.ValidadeCartãoCombustivel = x.ValidadeCartaoCombustivel != "" && x.ValidadeCartaoCombustivel != null ? DateTime.Parse(x.ValidadeCartaoCombustivel) : (DateTime?)null;

            return viatura;
        }

        public static ViaturasViewModel ParseToViewModel(Viaturas x)
        {
            ViaturasViewModel viatura = new ViaturasViewModel()
            {
                Matricula = x.Matrícula,
                NQuadro = x.NºQuadro,
                Estado = x.Estado,
                CodigoTipoViatura = x.CódigoTipoViatura.ToString(),
                CodigoMarca = x.CódigoMarca.ToString(),
                CodigoModelo = x.CódigoModelo.ToString(),
                CartaoCombustivel = x.CartãoCombustível,
                Apolice = x.Apólice,
                PesoBruto = x.PesoBruto,
                Tara = x.Tara,
                Cilindrada = x.Cilindrada,
                TipoCombustivel = x.TipoCombustível,
                NLugares = x.NºLugares,
                Cor = x.Cor,
                AtribuidaA = x.AtribuídaA,
                CodigoRegiao = x.CódigoRegião,
                CodigoAreaFuncional = x.CódigoÁreaFuncional,
                CodigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                TipoPropriedade = x.TipoPropriedade,
                NImobilizado = x.NºImobilizado,
                UtilizadorCriacao = x.UtilizadorCriação,
                UtilizadorModificacao = x.UtilizadorModificação,
                LocalParqueamento = x.LocalParqueamento,
                Observacoes = x.Observações,
                Potencia = x.Potência,
                DistanciaEntreEixos = x.DistânciaEntreEixos,
                PneumaticosFrente = x.PneumáticosFrente,
                PneumaticosRetaguarda = x.PneumáticosRetaguarda,
                ConsumoIndicativo = x.ConsumoIndicativo,
                CartaVerde = x.CartaVerde,
                NViaVerde = x.NºViaVerde,
                ValorAquisicao = x.ValorAquisição,
                ValorVenda = x.ValorVenda,
                //Imagem = x.Imagem,
                KmUltimaRevisao = x.KmUltimaRevisão,
                IntervaloRevisoes = x.IntervaloRevisões,
                DuracaoPneus = x.DuraçãoPneus,
                NoProjeto = x.NoProjeto
            };

            if (x.DataMatrícula != null) viatura.DataMatricula = x.DataMatrícula.Value.ToString("yyyy-MM-dd");
            if (x.DataHoraModificação != null) viatura.DataHoraModificacao = x.DataHoraModificação.Value.ToString("yyyy-MM-dd hh:mm:ss");
            if (x.DataAbate != null) viatura.DataAbate = x.DataAbate.Value.ToString("yyyy-MM-dd");
            if (x.DataAquisição != null) viatura.DataAquisicao = x.DataAquisição.Value.ToString("yyyy-MM-dd");
            if (x.DataEntradaFuncionamento != null) viatura.DataEntradaFuncionamento = x.DataEntradaFuncionamento.Value.ToString("yyyy-MM-dd");
            if (x.DataHoraCriação != null) viatura.DataHoraCriacao = x.DataHoraCriação.Value.ToString("yyyy-MM-dd hh:mm:ss");
            if (x.DataUltimaInspeção != null) viatura.DataUltimaInspecao = x.DataUltimaInspeção.Value.ToString("yyyy-MM-dd");
            if (x.DataUltimaRevisão != null) viatura.DataUltimaRevisao = x.DataUltimaRevisão.Value.ToString("yyyy-MM-dd");
            if (x.ValidadeApólice != null) viatura.ValidadeApolice = x.ValidadeApólice.Value.ToString("yyyy-MM-dd");
            if (x.ValidadeCartaVerde != null) viatura.ValidadeCartaVerde = x.ValidadeCartaVerde.Value.ToString("yyyy-MM-dd");
            if (x.ValidadeCartãoCombustivel != null) viatura.ValidadeCartaoCombustivel = x.ValidadeCartãoCombustivel.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<ViaturasViewModel> ParseListToViewModel(List<Viaturas> x)
        {
            List<ViaturasViewModel> viatura = new List<ViaturasViewModel>();

            x.ForEach(y => viatura.Add(ParseToViewModel(y)));

            return viatura;
        }

    }
}
