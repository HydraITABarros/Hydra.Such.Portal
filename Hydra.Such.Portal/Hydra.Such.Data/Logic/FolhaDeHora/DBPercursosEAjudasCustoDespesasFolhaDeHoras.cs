using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBPercursosEAjudasCustoDespesasFolhaDeHoras
    {
        #region CRUD
        public static PercursosEAjudasCustoDespesasFolhaDeHoras GetByPercursoNo(int PercursoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºLinha == PercursoNo && x.TipoCusto == 1).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHoras> GetByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo && x.TipoCusto == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PercursosEAjudasCustoDespesasFolhaDeHoras Create(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    ObjectToCreate.TipoCusto = 1;
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PercursosEAjudasCustoDespesasFolhaDeHoras Update(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = PERCURSO
                    ObjectToUpdate.TipoCusto = 1;
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(int PercursoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.RemoveRange(ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºLinha == PercursoNo));
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        public static List<PercursosEAjudasCustoDespesasFolhaDeHorasViewModel> GetAllByPercursoToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(Percurso => Percurso.NºFolhaDeHoras == FolhaHoraNo && Percurso.TipoCusto == 1).Select(Percurso => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
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
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHorasViewModel> GetAllByAjudaToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(Ajuda => Ajuda.NºFolhaDeHoras == FolhaHoraNo && Ajuda.TipoCusto == 2).Select(Ajuda => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
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
                    }).ToList(); ;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
