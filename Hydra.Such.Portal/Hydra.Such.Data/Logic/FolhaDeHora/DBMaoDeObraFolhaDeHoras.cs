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
                    return ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºLinha == MaoDeObraNo).FirstOrDefault();
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

        public static bool Delete(int MaoDeObraNo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MãoDeObraFolhaDeHoras.RemoveRange(ctx.MãoDeObraFolhaDeHoras.Where(x => x.NºLinha == MaoDeObraNo));
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
