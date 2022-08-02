using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.FolhaDeHora
{
    public class DBMaoDeObraFolhaDeHoras
    {
        #region CRUD
        public static MãoDeObraFolhaDeHoras GetByMaoDeObraNo(int MaoDeObraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.FirstOrDefault(x => x.NºLinha == MaoDeObraNo);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MãoDeObraFolhaDeHoras> GetByFolhaHoraNo(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<MãoDeObraFolhaDeHoras> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static decimal GetCustoTotalHorasByFolhaHoraNo(string FolhaHoraNo)
        {
            decimal CustoTotalHoras = 0;
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    List<MãoDeObraFolhaDeHoras> result = ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaHoraNo).ToList();
                    if (result != null && result.Count > 0)
                    {
                        result.ForEach(x =>
                        {
                            if (x.PreçoTotal != null)
                                CustoTotalHoras = CustoTotalHoras + (decimal)x.PreçoTotal;
                        });
                    }


                    return CustoTotalHoras;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static MãoDeObraFolhaDeHoras Create(MãoDeObraFolhaDeHoras ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.MãoDeObraFolhaDeHoras.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MãoDeObraFolhaDeHoras Update(MãoDeObraFolhaDeHoras ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.MãoDeObraFolhaDeHoras.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(string FolhaDeHoras, int MaoDeObraLinha)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MãoDeObraFolhaDeHoras.RemoveRange(ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºFolhaDeHoras == FolhaDeHoras && x.NºLinha == MaoDeObraLinha));
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

        public static List<MaoDeObraFolhaDeHorasViewModel> GetAllByMaoDeObraToList(string FolhaHoraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MãoDeObraFolhaDeHoras.Where(MaoDeObra => MaoDeObra.NºFolhaDeHoras == FolhaHoraNo).Select(MaoDeObra => new MaoDeObraFolhaDeHorasViewModel()
                    {
                        FolhaDeHorasNo = MaoDeObra.NºFolhaDeHoras,
                        LinhaNo = MaoDeObra.NºLinha,
                        Date = MaoDeObra.Date,
                        DateTexto = MaoDeObra.Date.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ProjetoNo = MaoDeObra.NºProjeto,
                        EmpregadoNo = MaoDeObra.NºEmpregado,
                        CodigoTipoTrabalho = MaoDeObra.CódigoTipoTrabalho,
                        HoraInicio = Convert.ToDateTime(DateTime.Now.Date + " " + MaoDeObra.HoraInício).ToShortTimeString(),
                        HoraInicioTexto = MaoDeObra.HoraInício.ToString(),
                        HorarioAlmoco = MaoDeObra.HorárioAlmoço,
                        HoraFim = Convert.ToDateTime(DateTime.Now.Date + " " + MaoDeObra.HoraFim).ToShortTimeString(),
                        HoraFimTexto = MaoDeObra.HoraFim.ToString(),
                        HorarioJantar = MaoDeObra.HorárioJantar,
                        CodigoRegiao = MaoDeObra.CodigoRegiao,
                        CodigoArea = MaoDeObra.CodigoArea,
                        CodigoFamiliaRecurso = MaoDeObra.CódigoFamíliaRecurso,
                        CodigoTipoOM = MaoDeObra.CódigoTipoOm,
                        HorasNo = Convert.ToDateTime(DateTime.Now.Date + " " + MaoDeObra.NºDeHoras).ToShortTimeString(),
                        HorasNoTexto = MaoDeObra.NºDeHoras.ToString(),
                        CustoUnitarioDireto = Convert.ToDecimal(MaoDeObra.CustoUnitárioDireto),
                        CodigoCentroResponsabilidade = MaoDeObra.CodigoCentroResponsabilidade,
                        PrecoTotal = Convert.ToDecimal(MaoDeObra.PreçoTotal),
                        Descricao = MaoDeObra.Descricao,
                        RecursoNo = MaoDeObra.NºRecurso,
                        CodigoUnidadeMedida = MaoDeObra.CódUnidadeMedida,
                        PrecoDeCusto = Convert.ToDecimal(MaoDeObra.PreçoDeCusto),
                        PrecoDeVenda = Convert.ToDecimal(MaoDeObra.PreçoDeVenda),
                        UtilizadorCriacao = MaoDeObra.UtilizadorCriação,
                        DataHoraCriacao = MaoDeObra.DataHoraCriação,
                        DataHoraCriacaoTexto = MaoDeObra.DataHoraCriação.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        UtilizadorModificacao = MaoDeObra.UtilizadorModificação,
                        DataHoraModificacao = MaoDeObra.DataHoraModificação,
                        DataHoraModificacaoTexto = MaoDeObra.DataHoraModificação.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
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
