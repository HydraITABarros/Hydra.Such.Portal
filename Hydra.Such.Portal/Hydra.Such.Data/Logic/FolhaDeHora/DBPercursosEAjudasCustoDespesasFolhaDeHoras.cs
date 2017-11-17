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
        #region CRUD PERCURSO
        public static PercursosEAjudasCustoDespesasFolhaDeHoras GetByPercursoNo(int PercursoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 1 = PERCURSO
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºLinha == PercursoNo && x.CodPercursoAjuda == 1).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHoras> GetPercursoByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 1 = PERCURSO
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo && x.CodPercursoAjuda == 1).ToList();
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

        public static PercursosEAjudasCustoDespesasFolhaDeHoras CreatePercurso(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 1 = PERCURSO
                    ObjectToCreate.CodPercursoAjuda = 1;
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

        public static PercursosEAjudasCustoDespesasFolhaDeHoras UpdatePercurso(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 1 = PERCURSO
                    ObjectToUpdate.CodPercursoAjuda = 1;
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

        public static bool DeletePercurso(int PercursoNo)
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

        #region CRUD AJUDA
        public static PercursosEAjudasCustoDespesasFolhaDeHoras GetByAjudaNo(int AjudaNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 2 = AJUDA
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºLinha == AjudaNo && x.CodPercursoAjuda == 2).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<PercursosEAjudasCustoDespesasFolhaDeHoras> GetAjudaByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 2 = AJUDA
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo && x.CodPercursoAjuda == 2).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PercursosEAjudasCustoDespesasFolhaDeHoras CreateAjuda(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 2 = AJUDA
                    ObjectToCreate.CodPercursoAjuda = 2;
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

        public static PercursosEAjudasCustoDespesasFolhaDeHoras UpdateAjuda(PercursosEAjudasCustoDespesasFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //CodPercursoAjuda = 2 = AJUDA
                    ObjectToUpdate.CodPercursoAjuda = 2;
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

        public static bool DeleteAjuda(int AjudaNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.RemoveRange(ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(x => x.NºLinha == AjudaNo));
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
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(Percurso => Percurso.NºFolhaDeHoras == FolhaHoraNo && Percurso.CodPercursoAjuda == 1).Select(Percurso => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = Percurso.NºFolhaDeHoras,
                        CodPercursoAjuda = 1,
                        LinhaNo = Percurso.NºLinha,
                        Origem = Percurso.Origem,
                        OrigemDescricao = Percurso.OrigemDescricao,
                        Destino = Percurso.Destino,
                        DestinoDescricao = Percurso.DestinoDescricao,
                        DataViagem = Percurso.DataViagem,
                        DataViagemTexto = Percurso.DataViagem.Value.ToString("yyyy-MM-dd"),
                        Justificacao = Percurso.Justificação,
                        Distancia = Convert.ToDecimal(Percurso.Distância),
                        DistanciaPrevista = Convert.ToDecimal(Percurso.DistanciaPrevista),
                        CustoUnitario = Convert.ToDecimal(Percurso.CustoUnitário),
                        CustoTotal = Convert.ToDecimal(Percurso.CustoTotal),
                        TipoCusto = Percurso.TipoCusto,
                        CodTipoCusto = Percurso.CodTipoCusto,
                        Descricao = Percurso.Descrição,
                        Quantidade = Convert.ToDecimal(Percurso.Quantidade),
                        PrecoUnitario = Convert.ToDecimal(Percurso.PreçoUnitário),
                        PrecoVenda = Convert.ToDecimal(Percurso.PrecoVenda),
                        RubricaSalarial = Percurso.RúbricaSalarial,
                        UtilizadorCriacao = Percurso.UtilizadorCriação,
                        DataHoraCriacao = Percurso.DataHoraCriação,
                        DataHoraCriacaoTexto = Percurso.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        UtilizadorModificacao = Percurso.UtilizadorModificação,
                        DataHoraModificacao = Percurso.DataHoraModificação,
                        DataHoraModificacaoTexto = Percurso.DataHoraModificação.Value.ToString("yyyy-MM-dd")
                    }).ToList();
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
                    return ctx.PercursosEAjudasCustoDespesasFolhaDeHoras.Where(Ajuda => Ajuda.NºFolhaDeHoras == FolhaHoraNo && Ajuda.CodPercursoAjuda == 2).Select(Ajuda => new PercursosEAjudasCustoDespesasFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = Ajuda.NºFolhaDeHoras,
                        CodPercursoAjuda = 2,
                        LinhaNo = Ajuda.NºLinha,
                        Origem = Ajuda.Origem,
                        OrigemDescricao = Ajuda.OrigemDescricao,
                        Destino = Ajuda.Destino,
                        DestinoDescricao = Ajuda.DestinoDescricao,
                        DataViagem = Ajuda.DataViagem,
                        DataViagemTexto = Ajuda.DataViagem.Value.ToString("yyyy-MM-dd"),
                        Justificacao = Ajuda.Justificação,
                        Distancia = Convert.ToDecimal(Ajuda.Distância),
                        DistanciaPrevista = Convert.ToDecimal(Ajuda.DistanciaPrevista),
                        CustoUnitario = Convert.ToDecimal(Ajuda.CustoUnitário),
                        CustoTotal = Convert.ToDecimal(Ajuda.CustoTotal),
                        TipoCusto = Ajuda.TipoCusto,
                        CodTipoCusto = Ajuda.CodTipoCusto,
                        Descricao = Ajuda.Descrição,
                        Quantidade = Convert.ToDecimal(Ajuda.Quantidade),
                        PrecoUnitario = Convert.ToDecimal(Ajuda.PreçoUnitário),
                        PrecoVenda = Convert.ToDecimal(Ajuda.PrecoVenda),
                        RubricaSalarial = Ajuda.RúbricaSalarial,
                        UtilizadorCriacao = Ajuda.UtilizadorCriação,
                        DataHoraCriacao = Ajuda.DataHoraCriação,
                        DataHoraCriacaoTexto = Ajuda.DataHoraCriação.Value.ToString("yyyy-MM-dd"),
                        UtilizadorModificacao = Ajuda.UtilizadorModificação,
                        DataHoraModificacao = Ajuda.DataHoraModificação,
                        DataHoraModificacaoTexto = Ajuda.DataHoraModificação.Value.ToString("yyyy-MM-dd")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
