using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2
    {
        public static List<Viaturas2> GetAllToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2 GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2.Where(p => p.Matricula == Matricula).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2 Create(Viaturas2 ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2 ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2 Update(Viaturas2 ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2 ParseToDB(Viaturas2ViewModel x)
        {
            Viaturas2 viatura = new Viaturas2()
            {
                Matricula = x.Matricula,
                IDEstado = x.IDEstado,
                IDMarca = x.IDMarca,
                IDModelo = x.IDModelo,
                Data1Matricula = x.Data1Matricula,
                Cor = x.Cor,
                DataMatricula = x.DataMatricula,
                IDCategoria = x.IDCategoria,
                IDTipo = x.IDTipo,
                Classificacao = x.Classificacao,
                Cilindrada = x.Cilindrada,
                IDCombustivel = x.IDCombustivel,
                ConsumoReferencia = x.ConsumoReferencia,
                CapacidadeDeposito = x.CapacidadeDeposito,
                Autonomia = x.Autonomia,
                PesoBruto = x.PesoBruto,
                CargaMaxima = x.CargaMaxima,
                Tara = x.Tara,
                Potencia = x.Potencia,
                DistanciaEixos = x.DistanciaEixos,
                NoLugares = x.NoLugares,
                NoAnosGarantia = x.NoAnosGarantia,
                NoQuadro = x.NoQuadro,
                IDTipoCaixa = x.IDTipoCaixa,
                PneuFrente = x.PneuFrente,
                PneuRetaguarda = x.PneuRetaguarda,
                Observacoes = x.Observacoes,
                NomeImagem = x.NomeImagem,
                DataEstado = x.DataEstado,
                IDTipoPropriedade = x.IDTipoPropriedade,
                IDPropriedade = x.IDPropriedade,
                IDSegmentacao = x.IDSegmentacao,
                DataProximaInspecao = x.DataProximaInspecao,
                IntervaloRevisoes = x.IntervaloRevisoes,
                IDLocalParqueamento = x.IDLocalParqueamento,
                AlvaraLicenca = x.AlvaraLicenca,
                CodRegiao = x.CodRegiao,
                CodAreaFuncional = x.CodAreaFuncional,
                CodCentroResponsabilidade = x.CodCentroResponsabilidade,
                NoProjeto = x.NoProjeto,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao,
            };

            if (!string.IsNullOrEmpty(x.Data1MatriculaTexto)) viatura.Data1Matricula = Convert.ToDateTime(x.Data1MatriculaTexto);
            if (!string.IsNullOrEmpty(x.DataMatriculaTexto)) viatura.DataMatricula = Convert.ToDateTime(x.DataMatriculaTexto);
            if (!string.IsNullOrEmpty(x.DataEstadoTexto)) viatura.DataEstado = Convert.ToDateTime(x.DataEstadoTexto);
            if (!string.IsNullOrEmpty(x.DataProximaInspecaoTexto)) viatura.DataProximaInspecao = Convert.ToDateTime(x.DataProximaInspecaoTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2> ParseListToViewModel(List<Viaturas2ViewModel> x)
        {
            List<Viaturas2> viaturas2 = new List<Viaturas2>();

            x.ForEach(y => viaturas2.Add(ParseToDB(y)));

            return viaturas2;
        }

        public static Viaturas2ViewModel ParseToViewModel(Viaturas2 x)
        {
            Viaturas2ViewModel viatura = new Viaturas2ViewModel()
            {
                Matricula = x.Matricula,
                IDEstado = x.IDEstado,
                IDMarca = x.IDMarca,
                IDModelo = x.IDModelo,
                Data1Matricula = x.Data1Matricula,
                Cor = x.Cor,
                DataMatricula = x.DataMatricula,
                IDCategoria = x.IDCategoria,
                IDTipo = x.IDTipo,
                Classificacao = x.Classificacao,
                Cilindrada = x.Cilindrada,
                IDCombustivel = x.IDCombustivel,
                ConsumoReferencia = x.ConsumoReferencia,
                CapacidadeDeposito = x.CapacidadeDeposito,
                Autonomia = x.Autonomia,
                PesoBruto = x.PesoBruto,
                CargaMaxima = x.CargaMaxima,
                Tara = x.Tara,
                Potencia = x.Potencia,
                DistanciaEixos = x.DistanciaEixos,
                NoLugares = x.NoLugares,
                NoAnosGarantia = x.NoAnosGarantia,
                NoQuadro = x.NoQuadro,
                IDTipoCaixa = x.IDTipoCaixa,
                PneuFrente = x.PneuFrente,
                PneuRetaguarda = x.PneuRetaguarda,
                Observacoes = x.Observacoes,
                NomeImagem = x.NomeImagem,
                DataEstado = x.DataEstado,
                IDTipoPropriedade = x.IDTipoPropriedade,
                IDPropriedade = x.IDPropriedade,
                IDSegmentacao = x.IDSegmentacao,
                DataProximaInspecao = x.DataProximaInspecao,
                IntervaloRevisoes = x.IntervaloRevisoes,
                IDLocalParqueamento = x.IDLocalParqueamento,
                AlvaraLicenca = x.AlvaraLicenca,
                CodRegiao = x.CodRegiao,
                CodAreaFuncional = x.CodAreaFuncional,
                CodCentroResponsabilidade = x.CodCentroResponsabilidade,
                NoProjeto = x.NoProjeto,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.Data1Matricula != null) viatura.Data1MatriculaTexto = x.Data1Matricula.Value.ToString("yyyy-MM-dd");
            if (x.DataMatricula != null) viatura.DataMatriculaTexto = x.DataMatricula.Value.ToString("yyyy-MM-dd");
            if (x.DataEstado != null) viatura.DataEstadoTexto = x.DataEstado.Value.ToString("yyyy-MM-dd");
            if (x.DataProximaInspecao != null) viatura.DataProximaInspecaoTexto = x.DataProximaInspecao.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2ViewModel> ParseListToViewModel(List<Viaturas2> x)
        {
            List<Viaturas2ViewModel> viaturas2 = new List<Viaturas2ViewModel>();

            x.ForEach(y => viaturas2.Add(ParseToViewModel(y)));

            return viaturas2;
        }
    }
}
