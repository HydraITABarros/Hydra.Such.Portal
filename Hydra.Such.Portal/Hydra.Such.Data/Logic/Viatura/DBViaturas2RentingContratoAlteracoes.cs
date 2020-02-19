using Hydra.Such.Data.ViewModel.Viaturas;
using Hydra.Such.Data.Database;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Such.Data.Logic.Viatura
{
    public static class DBViaturas2RentingContratoAlteracoes
    {
        public static List<Viaturas2RentingContratoAlteracoes> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2RentingContratoAlteracoes.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2RentingContratoAlteracoes GetByID(int ID)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2RentingContratoAlteracoes.Where(p => p.ID == ID).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2RentingContratoAlteracoes> GetByMatricula(string Matricula)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2RentingContratoAlteracoes.Where(p => p.Matricula == Matricula).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Viaturas2RentingContratoAlteracoes> GetByIDContrato(int IDContrato)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Viaturas2RentingContratoAlteracoes.Where(p => p.IDContrato == IDContrato).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Viaturas2RentingContratoAlteracoes Create(Viaturas2RentingContratoAlteracoes ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataCriacao = DateTime.Now;
                    ctx.Viaturas2RentingContratoAlteracoes.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Viaturas2RentingContratoAlteracoes ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Viaturas2RentingContratoAlteracoes.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Viaturas2RentingContratoAlteracoes Update(Viaturas2RentingContratoAlteracoes ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataModificacao = DateTime.Now;
                    ctx.Viaturas2RentingContratoAlteracoes.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Viaturas2RentingContratoAlteracoes ParseToDB(Viaturas2RentingContratoAlteracoesViewModel x)
        {
            Viaturas2RentingContratoAlteracoes viatura = new Viaturas2RentingContratoAlteracoes()
            {
                ID = x.ID,
                Matricula = x.Matricula,

                IDContrato = x.IDContrato,
                DataPedido = x.DataPedido,
                AutorizadoFINLOG = x.AutorizadoFINLOG,
                DataAlteracaoKm = x.DataAlteracaoKm,
                KmAlteracao = x.KmAlteracao,
                MensalidadeAlteracao = x.MensalidadeAlteracao,
                Observacoes = x.Observacoes,

                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (!string.IsNullOrEmpty(x.DataPedidoTexto)) viatura.DataPedido = Convert.ToDateTime(x.DataPedidoTexto);
            if (!string.IsNullOrEmpty(x.DataAlteracaoKmTexto)) viatura.DataAlteracaoKm = Convert.ToDateTime(x.DataAlteracaoKmTexto);
            if (!string.IsNullOrEmpty(x.DataCriacaoTexto)) viatura.DataCriacao = Convert.ToDateTime(x.DataCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataModificacaoTexto)) viatura.DataModificacao = Convert.ToDateTime(x.DataModificacaoTexto);

            return viatura;
        }

        public static List<Viaturas2RentingContratoAlteracoes> ParseListToViewModel(List<Viaturas2RentingContratoAlteracoesViewModel> x)
        {
            List<Viaturas2RentingContratoAlteracoes> Viaturas2RentingContratoAlteracoes = new List<Viaturas2RentingContratoAlteracoes>();

            x.ForEach(y => Viaturas2RentingContratoAlteracoes.Add(ParseToDB(y)));

            return Viaturas2RentingContratoAlteracoes;
        }

        public static Viaturas2RentingContratoAlteracoesViewModel ParseToViewModel(Viaturas2RentingContratoAlteracoes x)
        {
            Viaturas2RentingContratoAlteracoesViewModel viatura = new Viaturas2RentingContratoAlteracoesViewModel()
            {
                ID = x.ID,
                Matricula = x.Matricula,

                IDContrato = x.IDContrato,
                DataPedido = x.DataPedido,
                AutorizadoFINLOG = x.AutorizadoFINLOG,
                DataAlteracaoKm = x.DataAlteracaoKm,
                KmAlteracao = x.KmAlteracao,
                MensalidadeAlteracao = x.MensalidadeAlteracao,
                Observacoes = x.Observacoes,

                UtilizadorCriacao = x.UtilizadorCriacao,
                DataCriacao = x.DataCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                DataModificacao = x.DataModificacao
            };

            if (x.DataPedido != null) viatura.DataPedidoTexto = x.DataPedido.Value.ToString("yyyy-MM-dd");
            if (x.DataAlteracaoKm != null) viatura.DataAlteracaoKmTexto = x.DataAlteracaoKm.Value.ToString("yyyy-MM-dd");
            if (x.DataCriacao != null) viatura.DataCriacaoTexto = x.DataCriacao.Value.ToString("yyyy-MM-dd");
            if (x.DataModificacao != null) viatura.DataModificacaoTexto = x.DataModificacao.Value.ToString("yyyy-MM-dd");

            return viatura;
        }

        public static List<Viaturas2RentingContratoAlteracoesViewModel> ParseListToViewModel(List<Viaturas2RentingContratoAlteracoes> x)
        {
            List<Viaturas2RentingContratoAlteracoesViewModel> Viaturas2RentingContratoAlteracoes = new List<Viaturas2RentingContratoAlteracoesViewModel>();

            x.ForEach(y => Viaturas2RentingContratoAlteracoes.Add(ParseToViewModel(y)));

            return Viaturas2RentingContratoAlteracoes;
        }
    }
}
