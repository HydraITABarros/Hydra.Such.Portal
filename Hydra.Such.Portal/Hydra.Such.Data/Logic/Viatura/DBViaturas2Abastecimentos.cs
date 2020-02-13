using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2Abastecimentos
    {
        public static List<Viaturas2Abastecimentos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Abastecimentos.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Abastecimentos GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Abastecimentos.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2Abastecimentos> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2Abastecimentos.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2Abastecimentos Create(Viaturas2Abastecimentos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2Abastecimentos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2Abastecimentos ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2Abastecimentos.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2Abastecimentos Update(Viaturas2Abastecimentos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2Abastecimentos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2Abastecimentos ParseToDB(Viaturas2AbastecimentosViewModel x)
        {
            Viaturas2Abastecimentos viatura = new Viaturas2Abastecimentos()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Data = x.Data,
                Kms = x.Kms,
                Litros = x.Litros,
                PrecoUnitario = x.PrecoUnitario,
                PrecoTotal = x.PrecoTotal,
                IDCombustivel = x.IDCombustivel,
                IDEmpresa = x.IDEmpresa,
                IDCartao = x.IDCartao,
                Local = x.Local,
                IDCondutor = x.IDCondutor,
                NoDocumento = x.NoDocumento,
                Observacoes = x.Observacoes,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataTexto)) viatura.Data = Convert.ToDateTime(x.DataTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2Abastecimentos> ParseListToViewModel(List<Viaturas2AbastecimentosViewModel> x)
        {
            List<Viaturas2Abastecimentos> Viaturas2Abastecimentos = new List<Viaturas2Abastecimentos>();

            x.ForEach(y => Viaturas2Abastecimentos.Add(ParseToDB(y)));

            return Viaturas2Abastecimentos;
        }

        public static Viaturas2AbastecimentosViewModel ParseToViewModel(Viaturas2Abastecimentos x)
        {
            Viaturas2AbastecimentosViewModel viatura = new Viaturas2AbastecimentosViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,
                Data = x.Data,
                Kms = x.Kms,
                Litros = x.Litros,
                PrecoUnitario = x.PrecoUnitario,
                PrecoTotal = x.PrecoTotal,
                IDCombustivel = x.IDCombustivel,
                IDEmpresa = x.IDEmpresa,
                IDCartao = x.IDCartao,
                Local = x.Local,
                IDCondutor = x.IDCondutor,
                NoDocumento = x.NoDocumento,
                Observacoes = x.Observacoes,
                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.Data != null) viatura.DataTexto = x.Data.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2AbastecimentosViewModel> ParseListToViewModel(List<Viaturas2Abastecimentos> x)
        {
            List<Viaturas2AbastecimentosViewModel> Viaturas2Abastecimentos = new List<Viaturas2AbastecimentosViewModel>();

            x.ForEach(y => Viaturas2Abastecimentos.Add(ParseToViewModel(y)));

            return Viaturas2Abastecimentos;
        }
    }
}
