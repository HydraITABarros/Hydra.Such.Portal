using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.VisitasVM;

using System;
using System.Collections.Generic;
using System.Linq;


namespace Hydra.Such.Data.Logic.VisitasDB
{
    public static class DBVisitas
    {
        public static List<Visitas> GetAllToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Visitas.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Visitas> GetAllAtivas(bool Ativas = true)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    if (Ativas == true)
                        return ctx.Visitas.Where(x => x.CodEstado == null || x.CodEstado == 1 || x.CodEstado == 2).ToList();
                    else
                        return ctx.Visitas.Where(x => x.CodEstado == 3).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Visitas GetByVisita(string Visita)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.Visitas.FirstOrDefault(p => p.CodVisita == Visita);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Visitas Create(Visitas ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.Visitas.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(Visitas ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.Visitas.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static Visitas Update(Visitas ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.Visitas.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static Visitas ParseToDB(VisitasViewModel x)
        {
            Visitas visita = new Visitas()
            {
                CodVisita = x.CodVisita,
                Objetivo = x.Objetivo,
                Local = x.Local,
                CodCliente = x.CodCliente,
                NomeCliente = x.NomeCliente,
                CodFornecedor = x.CodFornecedor,
                NomeFornecedor = x.NomeFornecedor,
                Entidade = x.Entidade,
                CodRegiao = x.CodRegiao,
                NomeRegiao = x.NomeRegiao,
                CodArea = x.CodArea,
                NomeArea = x.NomeArea,
                CodCresp = x.CodCresp,
                NomeCresp = x.NomeCresp,
                InicioDataHora = x.InicioDataHora,
                FimDataHora = x.FimDataHora,
                CodEstado = x.CodEstado,
                NomeEstado = x.NomeEstado,
                IniciativaCriador = x.IniciativaCriador,
                IniciativaCriadorNome = x.IniciativaCriadorNome,
                IniciativaResponsavel = x.IniciativaResponsavel,
                IniciativaResponsavelNome = x.IniciativaResponsavelNome,
                IniciativaIntervinientes = x.IniciativaIntervinientes,
                RececaoCriador = x.RececaoCriador,
                RececaoResponsavel = x.RececaoResponsavel,
                RececaoIntervinientes = x.RececaoIntervinientes,
                RelatorioSimplificado = x.RelatorioSimplificado,
                UtilizadorCriacao = x.UtilizadorCriacao,
                UtilizadorCriacaoNome = x.UtilizadorCriacaoNome,
                DataHoraCriacao = x.DataHoraCriacao,
                UtilizadorModificacao = x.UtilizadorModificacao,
                UtilizadorModificacaoNome = x.UtilizadorModificacaoNome,
                DataHoraModificacao = x.DataHoraModificacao,
            };

            if (!string.IsNullOrEmpty(x.InicioDataHoraTexto)) visita.InicioDataHora = Convert.ToDateTime(x.InicioDataHoraTexto);
            if (!string.IsNullOrEmpty(x.FimDataHoraTexto)) visita.FimDataHora = Convert.ToDateTime(x.FimDataHoraTexto);
            if (!string.IsNullOrEmpty(x.DataHoraCriacaoTexto)) visita.DataHoraCriacao = Convert.ToDateTime(x.DataHoraCriacaoTexto);
            if (!string.IsNullOrEmpty(x.DataHoraModificacaoTexto)) visita.DataHoraModificacao = Convert.ToDateTime(x.DataHoraModificacaoTexto);

            return visita;
        }

        public static List<Visitas> ParseListToViewModel(List<VisitasViewModel> x)
        {
            List<Visitas> Visitas = new List<Visitas>();

            x.ForEach(y => Visitas.Add(ParseToDB(y)));

            return Visitas;
        }

        public static VisitasViewModel ParseToViewModel(Visitas x)
        {
            VisitasViewModel visita = new VisitasViewModel()
            {
                CodVisita = x.CodVisita,
                Objetivo = x.Objetivo,
                Local = x.Local,
                CodCliente = x.CodCliente,
                NomeCliente = x.NomeCliente,
                CodFornecedor = x.CodFornecedor,
                NomeFornecedor = x.NomeFornecedor,
                Entidade = x.Entidade,
                CodRegiao = x.CodRegiao,
                NomeRegiao = x.NomeRegiao,
                CodArea = x.CodArea,
                NomeArea = x.NomeArea,
                CodCresp = x.CodCresp,
                NomeCresp = x.NomeCresp,
                InicioDataHora = x.InicioDataHora,
                InicioDataTexto = x.InicioDataHora.HasValue ? x.InicioDataHora.Value.ToString("yyyy-MM-dd") : "",
                InicioHoraTexto = x.InicioDataHora.HasValue ? x.InicioDataHora.Value.ToString("HH:mm") : "",
                FimDataHora = x.FimDataHora,
                FimDataTexto = x.FimDataHora.HasValue ? x.FimDataHora.Value.ToString("yyyy-MM-dd") : "",
                FimHoraTexto = x.FimDataHora.HasValue ? x.FimDataHora.Value.ToString("HH:mm") : "",
                CodEstado = x.CodEstado,
                NomeEstado = x.NomeEstado,
                IniciativaCriador = x.IniciativaCriador,
                IniciativaCriadorNome = x.IniciativaCriadorNome,
                IniciativaResponsavel = x.IniciativaResponsavel,
                IniciativaResponsavelNome = x.IniciativaResponsavelNome,
                IniciativaIntervinientes = x.IniciativaIntervinientes,
                RececaoCriador = x.RececaoCriador,
                RececaoResponsavel = x.RececaoResponsavel,
                RececaoIntervinientes = x.RececaoIntervinientes,
                RelatorioSimplificado = x.RelatorioSimplificado,
                UtilizadorCriacao = x.UtilizadorCriacao,
                UtilizadorCriacaoNome = x.UtilizadorCriacaoNome,
                DataHoraCriacao = x.DataHoraCriacao,
                DataHoraCriacaoTexto = x.DataHoraCriacao.HasValue ? x.DataHoraCriacao.Value.ToString("yyyy-MM-dd") : "",
                UtilizadorModificacao = x.UtilizadorModificacao,
                UtilizadorModificacaoNome = x.UtilizadorModificacaoNome,
                DataHoraModificacao = x.DataHoraModificacao,
                DataHoraModificacaoTexto = x.DataHoraModificacao.HasValue ? x.DataHoraModificacao.Value.ToString("yyyy-MM-dd") : ""
            };

            return visita;
        }

        public static List<VisitasViewModel> ParseListToViewModel(List<Visitas> x)
        {
            List<VisitasViewModel> Visitas = new List<VisitasViewModel>();

            x.ForEach(y => Visitas.Add(ParseToViewModel(y)));

            return Visitas;
        }
    }
}
