using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBLinhasFolhaHoras
    {
        #region CRUD PERCURSO
        public static LinhasFolhaHoras GetByPercursoNo(string NoFolhaHoras, int PercursoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFolhaHoras.FirstOrDefault(x => x.NoFolhaHoras == NoFolhaHoras && x.NoLinha == PercursoNo);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasFolhaHoras> GetPercursoByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 1 = KM = PERCURSO
                    return ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == FolhaHoraNo && x.TipoCusto == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetMaxPercursoByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    int max = 0;

                    List<LinhasFolhaHorasViewModel> result = DBLinhasFolhaHoras.GetAllByPercursoToList(FolhaHoraNo);
                    result.ForEach(x =>
                    {
                        if (x.NoLinha >= max) max = x.NoLinha;
                    });

                    max = max + 1;

                    return max;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int GetMaxByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    int max = 0;

                    List<LinhasFolhaHoras> result = ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == FolhaHoraNo).ToList();
                    result.ForEach(x =>
                    {
                        if (x.NoLinha > max) max = x.NoLinha;
                    });

                    max = max + 1;

                    return max;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool ExistsRegiaoAutonomaByFolhaHoraNo(string NoFolhaHoras)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    LinhasFolhaHoras Linhas = ctx.LinhasFolhaHoras.FirstOrDefault(x => x.NoFolhaHoras == NoFolhaHoras && x.RegiaoAutonoma == true);

                    if (Linhas != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static decimal GetNoTotalKmByFolhaHoraNo(string FolhaHoraNo)
        {
            decimal NoTotalKm = 0;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<LinhasFolhaHoras> result = ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == FolhaHoraNo && x.TipoCusto == 1).ToList();
                    if (result != null && result.Count > 0)
                    {
                        result.ForEach(x =>
                        {
                            decimal DistanciaEfetuada = x.Distancia.HasValue ? (decimal)x.Distancia : 0;
                            decimal DistanciaPrevista = x.DistanciaPrevista.HasValue ? (decimal)x.DistanciaPrevista : 0;

                            if (DistanciaEfetuada == 0)
                            {
                                NoTotalKm = NoTotalKm + 0;
                            }
                            else
                            {
                                if (DistanciaPrevista == 0)
                                    NoTotalKm = NoTotalKm + DistanciaEfetuada;
                                else
                                    NoTotalKm = NoTotalKm + DistanciaPrevista;
                            }
                        });
                    }

                    return NoTotalKm;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static decimal GetCustoTotalKMByFolhaHoraNo(string FolhaHoraNo, int TipoDeslocacao, bool ForaConcelho, decimal DistanciaMinima)
        {
            decimal CustoTotalKM = 0;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<LinhasFolhaHoras> result = ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == FolhaHoraNo && x.TipoCusto == 1 && x.RegiaoAutonoma == false).ToList();
                    if (TipoDeslocacao == 2 && ForaConcelho == true) //Viatura Própria"
                    {
                        if (result != null && result.Count > 0)
                        {
                            result.ForEach(x =>
                            {
                                decimal DistanciaEfetuada = x.Distancia.HasValue ? (decimal)x.Distancia : 0;
                                decimal DistanciaPrevista = x.DistanciaPrevista.HasValue ? (decimal)x.DistanciaPrevista : 0;
                                decimal CustoUnitario = x.CustoUnitario.HasValue ? (decimal)x.CustoUnitario : 0;

                                if (DistanciaEfetuada >= DistanciaMinima)
                                {
                                    CustoTotalKM = CustoTotalKM + (DistanciaEfetuada * CustoUnitario);
                                }
                                else
                                {
                                    if (DistanciaEfetuada == 0 && DistanciaPrevista >= DistanciaMinima)
                                        CustoTotalKM = CustoTotalKM + (DistanciaPrevista * CustoUnitario);
                                }
                                //if (DistanciaEfetuada == 0)
                                //{
                                //    CustoTotalKM = CustoTotalKM + 0;
                                //}
                                //else
                                //{
                                //    if (DistanciaPrevista == 0)
                                //        CustoTotalKM = CustoTotalKM + (DistanciaEfetuada * CustoUnitario);
                                //    else
                                //        CustoTotalKM = CustoTotalKM + (DistanciaPrevista * CustoUnitario);
                                //}
                            });
                        }
                    }
                    else
                        CustoTotalKM = 0;

                    return CustoTotalKM;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static decimal GetCustoTotalAjudaCustoByFolhaHoraNo(string FolhaHoraNo)
        {
            decimal CustoTotalAjudaCusto = 0;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<LinhasFolhaHoras> result = ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == FolhaHoraNo && x.TipoCusto == 2).ToList();
                    if (result != null && result.Count > 0)
                    {
                        result.ForEach(x =>
                        {
                            if (x.CustoTotal != null)
                                CustoTotalAjudaCusto = CustoTotalAjudaCusto + (decimal)x.CustoTotal;
                        });
                    }

                    return CustoTotalAjudaCusto;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<LinhasFolhaHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFolhaHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasFolhaHoras CreatePercurso(LinhasFolhaHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.LinhasFolhaHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasFolhaHoras UpdatePercurso(LinhasFolhaHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.LinhasFolhaHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool DeletePercurso(string NoFolhaHoras, int PercursoNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasFolhaHoras.RemoveRange(ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == NoFolhaHoras && x.NoLinha == PercursoNo));
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
        public static LinhasFolhaHoras GetByAjudaNo(string NoFolhaHoras, int AjudaNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFolhaHoras.FirstOrDefault(x => x.NoFolhaHoras == NoFolhaHoras && x.NoLinha == AjudaNo);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasFolhaHoras> GetAjudaByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 2 || 3 = AJUDA
                    return ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == FolhaHoraNo && (x.TipoCusto == 2 || x.TipoCusto == 3)).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasFolhaHoras> GetAllAjudasCustoByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    //TipoCusto = 2
                    return ctx.LinhasFolhaHoras.Where(x => x.TipoCusto == 2 && x.CalculoAutomatico == true && x.NoFolhaHoras == FolhaHoraNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int GetMaxAjudaByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    int max = 0;

                    List<LinhasFolhaHorasViewModel> result = DBLinhasFolhaHoras.GetAllByAjudaToList(FolhaHoraNo);
                    result.ForEach(x =>
                    {
                        if (x.NoLinha >= max) max = x.NoLinha;
                    });

                    max = max + 1;

                    return max;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static LinhasFolhaHoras CreateAjuda(LinhasFolhaHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.LinhasFolhaHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static LinhasFolhaHoras UpdateAjuda(LinhasFolhaHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.LinhasFolhaHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool DeleteAjuda(string NoFolhaHoras, int AjudaNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.LinhasFolhaHoras.RemoveRange(ctx.LinhasFolhaHoras.Where(x => x.NoFolhaHoras == NoFolhaHoras && x.NoLinha == AjudaNo));
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

        public static List<LinhasFolhaHorasViewModel> GetAllByPercursoToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFolhaHoras.Where(Percurso => Percurso.NoFolhaHoras == FolhaHoraNo && Percurso.TipoCusto == 1).Select(Percurso => new LinhasFolhaHorasViewModel()
                    {
                        NoFolhaHoras = Percurso.NoFolhaHoras,
                        NoLinha = Percurso.NoLinha,
                        TipoCusto = Percurso.TipoCusto,
                        CodTipoCusto = Percurso.CodTipoCusto,
                        DescricaoTipoCusto = Percurso.DescricaoTipoCusto,
                        Quantidade = Percurso.Quantidade,
                        CustoUnitario = Percurso.CustoUnitario,
                        CustoTotal = Percurso.CustoTotal,
                        PrecoUnitario = Percurso.PrecoUnitario,
                        PrecoVenda = Percurso.PrecoVenda,
                        CodOrigem = Percurso.CodOrigem,
                        DescricaoOrigem = Percurso.DescricaoOrigem,
                        CodDestino = Percurso.CodDestino,
                        DescricaoDestino = Percurso.DescricaoDestino,
                        Distancia = Percurso.Distancia,
                        DistanciaPrevista = Percurso.DistanciaPrevista,
                        RegiaoAutonoma = Percurso.RegiaoAutonoma,
                        RubricaSalarial = Percurso.RubricaSalarial,
                        RegistarSubsidiosPremios = Percurso.RegistarSubsidiosPremios,
                        Observacao = Percurso.Observacao,
                        RubricaSalarial2 = Percurso.RubricaSalarial2,
                        DataDespesa = Percurso.DataDespesa,
                        DataDespesaTexto = Percurso.DataDespesa.HasValue ? Percurso.DataDespesa.Value.ToString("yyyy-MM-dd") : "",
                        Funcionario = Percurso.Funcionario,
                        CodRegiao = Percurso.CodRegiao,
                        CodArea = Percurso.CodArea,
                        CodCresp = Percurso.CodCresp,
                        CalculoAutomatico = Percurso.CalculoAutomatico,
                        Matricula = Percurso.Matricula,
                        UtilizadorCriacao = Percurso.UtilizadorCriacao,
                        DataHoraCriacao = Percurso.DataHoraCriacao,
                        DataHoraCriacaoTexto = Percurso.DataHoraCriacao.HasValue ? Percurso.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                        UtilizadorModificacao = Percurso.UtilizadorModificacao,
                        DataHoraModificacao = Percurso.DataHoraModificacao,
                        DataHoraModificacaoTexto = Percurso.DataHoraModificacao.HasValue ? Percurso.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : "",
                        NoProjeto = Percurso.NoProjeto,
                        ProjetoDescricao = Percurso.ProjetoDescricao,
                        DescricaoCodTipoCusto = Percurso.DescricaoTipoCusto
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<LinhasFolhaHorasViewModel> GetAllByAjudaToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.LinhasFolhaHoras.Where(Ajuda => Ajuda.NoFolhaHoras == FolhaHoraNo && (Ajuda.TipoCusto == 2 || Ajuda.TipoCusto == 3)).Select(Ajuda => new LinhasFolhaHorasViewModel()
                    {
                        NoFolhaHoras = Ajuda.NoFolhaHoras,
                        NoLinha = Ajuda.NoLinha,
                        TipoCusto = Ajuda.TipoCusto,
                        CodTipoCusto = Ajuda.CodTipoCusto,
                        DescricaoTipoCusto = Ajuda.DescricaoTipoCusto,
                        Quantidade = Ajuda.Quantidade,
                        CustoUnitario = Ajuda.CustoUnitario,
                        CustoTotal = Ajuda.CustoTotal,
                        PrecoUnitario = Ajuda.PrecoUnitario,
                        PrecoVenda = Ajuda.PrecoVenda,
                        CodOrigem = Ajuda.CodOrigem,
                        DescricaoOrigem = Ajuda.DescricaoOrigem,
                        CodDestino = Ajuda.CodDestino,
                        DescricaoDestino = Ajuda.DescricaoDestino,
                        Distancia = Ajuda.Distancia,
                        DistanciaPrevista = Ajuda.DistanciaPrevista,
                        RegiaoAutonoma = Ajuda.RegiaoAutonoma,
                        RubricaSalarial = Ajuda.RubricaSalarial,
                        RegistarSubsidiosPremios = Ajuda.RegistarSubsidiosPremios,
                        Observacao = Ajuda.Observacao,
                        RubricaSalarial2 = Ajuda.RubricaSalarial2,
                        DataDespesa = Ajuda.DataDespesa,
                        DataDespesaTexto = Ajuda.DataDespesa.HasValue ? Ajuda.DataDespesa.Value.ToString("yyyy-MM-dd") : "",
                        Funcionario = Ajuda.Funcionario,
                        CodRegiao = Ajuda.CodRegiao,
                        CodArea = Ajuda.CodArea,
                        CodCresp = Ajuda.CodCresp,
                        CalculoAutomatico = Ajuda.CalculoAutomatico,
                        Matricula = Ajuda.Matricula,
                        UtilizadorCriacao = Ajuda.UtilizadorCriacao,
                        DataHoraCriacao = Ajuda.DataHoraCriacao,
                        DataHoraCriacaoTexto = Ajuda.DataHoraCriacao.HasValue ? Ajuda.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                        UtilizadorModificacao = Ajuda.UtilizadorModificacao,
                        DataHoraModificacao = Ajuda.DataHoraModificacao,
                        DataHoraModificacaoTexto = Ajuda.DataHoraModificacao.HasValue ? Ajuda.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : "",
                        NoProjeto = Ajuda.NoProjeto,
                        ProjetoDescricao = Ajuda.ProjetoDescricao,
                        DescricaoCodTipoCusto = Ajuda.DescricaoTipoCusto
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
