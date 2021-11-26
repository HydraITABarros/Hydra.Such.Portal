using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;
using System.Linq;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Fornecedores;

namespace Hydra.Such.Data.Logic
{
    public static class DBQuestionarioFornecedorGestaoAmbiental
    {
        #region CRUD
        public static QuestionarioFornecedorGestaoAmbiental GetByCodigoAndVersao(string Codigo, int Versao)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.QuestionarioFornecedorGestaoAmbiental.Where(x => x.Codigo == Codigo && x.Versao == Versao).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionarioFornecedorGestaoAmbiental> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.QuestionarioFornecedorGestaoAmbiental.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionarioFornecedorGestaoAmbiental> GetAllByFornecedor(string ID_Fornecedor)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.QuestionarioFornecedorGestaoAmbiental.Where(x => x.ID_Fornecedor == ID_Fornecedor).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static QuestionarioFornecedorGestaoAmbiental Create(QuestionarioFornecedorGestaoAmbiental item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Criacao = DateTime.Now;
                    ctx.QuestionarioFornecedorGestaoAmbiental.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<QuestionarioFornecedorGestaoAmbiental> Create(List<QuestionarioFornecedorGestaoAmbiental> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    items.ForEach(x =>
                    {
                        x.DataHora_Criacao = DateTime.Now;
                        ctx.QuestionarioFornecedorGestaoAmbiental.Add(x);
                    });
                    ctx.SaveChanges();
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static QuestionarioFornecedorGestaoAmbiental Update(QuestionarioFornecedorGestaoAmbiental item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    item.DataHora_Modificacao = DateTime.Now;
                    ctx.QuestionarioFornecedorGestaoAmbiental.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static bool Delete(QuestionarioFornecedorGestaoAmbiental item)
        {
            return Delete(new List<QuestionarioFornecedorGestaoAmbiental> { item });
        }

        public static bool Delete(List<QuestionarioFornecedorGestaoAmbiental> items)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.QuestionarioFornecedorGestaoAmbiental.RemoveRange(items);
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

        public static QuestionarioFornecedorGestaoAmbientalViewModel ParseToViewModel(this QuestionarioFornecedorGestaoAmbiental item)
        {
            if (item != null)
            {
                return new QuestionarioFornecedorGestaoAmbientalViewModel()
                {
                    Codigo = item.Codigo,
                    Versao = item.Versao,
                    ID_Fornecedor = !string.IsNullOrEmpty(item.ID_Fornecedor) ? item.ID_Fornecedor : "",
                    Fornecedor = !string.IsNullOrEmpty(item.Fornecedor) ? item.Fornecedor : "",
                    Actividade = !string.IsNullOrEmpty(item.Actividade) ? item.Actividade : "",
                    Responsavel = !string.IsNullOrEmpty(item.Responsavel) ? item.Responsavel : "",
                    Funcao = !string.IsNullOrEmpty(item.Funcao) ? item.Funcao : "",
                    Telefone = !string.IsNullOrEmpty(item.Telefone) ? item.Telefone : "",
                    Procedimento = !string.IsNullOrEmpty(item.Procedimento) ? item.Procedimento: "",
                    Email = !string.IsNullOrEmpty(item.Email) ? item.Email : "",
                    Resposta_11_Sim = item.Resposta_11_Sim.HasValue ? item.Resposta_11_Sim : false,
                    Resposta_11_Sim_Texto = item.Resposta_11_Sim.HasValue ? item.Resposta_11_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_11_Nao = item.Resposta_11_Nao.HasValue ? item.Resposta_11_Nao : false,
                    Resposta_11_Nao_Texto = item.Resposta_11_Nao.HasValue ? item.Resposta_11_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_11_NA = item.Resposta_11_NA.HasValue ? item.Resposta_11_NA : false,
                    Resposta_11_NA_Texto = item.Resposta_11_NA.HasValue ? item.Resposta_11_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_11_Texto = !string.IsNullOrEmpty(item.Resposta_11_Texto) ? item.Resposta_11_Texto : "",
                    Resposta_12_Texto = !string.IsNullOrEmpty(item.Resposta_12_Texto) ? item.Resposta_12_Texto : "",
                    Resposta_13_Texto = !string.IsNullOrEmpty(item.Resposta_13_Texto) ? item.Resposta_13_Texto : "",
                    Resposta_21_Sim = item.Resposta_21_Sim.HasValue ? item.Resposta_21_Sim : false,
                    Resposta_21_Sim_Texto = item.Resposta_21_Sim.HasValue ? item.Resposta_21_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_21_Nao = item.Resposta_21_Nao.HasValue ? item.Resposta_21_Nao : false,
                    Resposta_21_Nao_Texto = item.Resposta_21_Nao.HasValue ? item.Resposta_21_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_21_NA = item.Resposta_21_NA.HasValue ? item.Resposta_21_NA : false,
                    Resposta_21_NA_Texto = item.Resposta_21_NA.HasValue ? item.Resposta_21_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_22_Sim = item.Resposta_22_Sim.HasValue ? item.Resposta_22_Sim : false,
                    Resposta_22_Sim_Texto = item.Resposta_22_Sim.HasValue ? item.Resposta_22_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_22_Nao = item.Resposta_22_Nao.HasValue ? item.Resposta_22_Nao : false,
                    Resposta_22_Nao_Texto = item.Resposta_22_Nao.HasValue ? item.Resposta_22_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_22_NA = item.Resposta_22_NA.HasValue ? item.Resposta_22_NA : false,
                    Resposta_22_NA_Texto = item.Resposta_22_NA.HasValue ? item.Resposta_22_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_23_Texto = !string.IsNullOrEmpty(item.Resposta_23_Texto) ? item.Resposta_23_Texto : "",
                    Resposta_2_Texto = !string.IsNullOrEmpty(item.Resposta_2_Texto) ? item.Resposta_2_Texto : "",
                    Resposta_31_Sim = item.Resposta_31_Sim.HasValue ? item.Resposta_31_Sim : false,
                    Resposta_31_Sim_Texto = item.Resposta_31_Sim.HasValue ? item.Resposta_31_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_31_Nao = item.Resposta_31_Nao.HasValue ? item.Resposta_31_Nao : false,
                    Resposta_31_Nao_Texto = item.Resposta_31_Nao.HasValue ? item.Resposta_31_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_31_NA = item.Resposta_31_NA.HasValue ? item.Resposta_31_NA : false,
                    Resposta_31_NA_Texto = item.Resposta_31_NA.HasValue ? item.Resposta_31_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_32_NA = item.Resposta_32_NA.HasValue ? item.Resposta_32_NA : false,
                    Resposta_32_NA_Texto = item.Resposta_32_NA.HasValue ? item.Resposta_32_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_32_Texto = !string.IsNullOrEmpty(item.Resposta_32_Texto) ? item.Resposta_32_Texto : "",
                    Resposta_3_Texto = !string.IsNullOrEmpty(item.Resposta_3_Texto) ? item.Resposta_3_Texto : "",
                    Resposta_41_Sim = item.Resposta_41_Sim.HasValue ? item.Resposta_41_Sim : false,
                    Resposta_41_Sim_Texto = item.Resposta_41_Sim.HasValue ? item.Resposta_41_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_41_Nao = item.Resposta_41_Nao.HasValue ? item.Resposta_41_Nao : false,
                    Resposta_41_Nao_Texto = item.Resposta_41_Nao.HasValue ? item.Resposta_41_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_41_NA = item.Resposta_41_NA.HasValue ? item.Resposta_41_NA : false,
                    Resposta_41_NA_Texto = item.Resposta_41_NA.HasValue ? item.Resposta_41_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_42_Sim = item.Resposta_42_Sim.HasValue ? item.Resposta_42_Sim : false,
                    Resposta_42_Sim_Texto = item.Resposta_42_Sim.HasValue ? item.Resposta_42_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_42_Nao = item.Resposta_42_Nao.HasValue ? item.Resposta_42_Nao : false,
                    Resposta_42_Nao_Texto = item.Resposta_42_Nao.HasValue ? item.Resposta_42_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_42_NA = item.Resposta_42_NA.HasValue ? item.Resposta_42_NA : false,
                    Resposta_42_NA_Texto = item.Resposta_42_NA.HasValue ? item.Resposta_42_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_43_Texto = !string.IsNullOrEmpty(item.Resposta_43_Texto) ? item.Resposta_43_Texto : "",
                    Resposta_4_Texto = !string.IsNullOrEmpty(item.Resposta_4_Texto) ? item.Resposta_4_Texto : "",
                    Resposta_51_Sim = item.Resposta_51_Sim.HasValue ? item.Resposta_51_Sim : false,
                    Resposta_51_Sim_Texto = item.Resposta_51_Sim.HasValue ? item.Resposta_51_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_51_Nao = item.Resposta_51_Nao.HasValue ? item.Resposta_51_Nao : false,
                    Resposta_51_Nao_Texto = item.Resposta_51_Nao.HasValue ? item.Resposta_51_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_51_NA = item.Resposta_51_NA.HasValue ? item.Resposta_51_NA : false,
                    Resposta_51_NA_Texto = item.Resposta_51_NA.HasValue ? item.Resposta_51_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_52_Sim = item.Resposta_52_Sim.HasValue ? item.Resposta_52_Sim : false,
                    Resposta_52_Sim_Texto = item.Resposta_52_Sim.HasValue ? item.Resposta_52_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_52_Nao = item.Resposta_52_Nao.HasValue ? item.Resposta_52_Nao : false,
                    Resposta_52_Nao_Texto = item.Resposta_52_Nao.HasValue ? item.Resposta_52_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_52_NA = item.Resposta_52_NA.HasValue ? item.Resposta_52_NA : false,
                    Resposta_52_NA_Texto = item.Resposta_52_NA.HasValue ? item.Resposta_52_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_5_Texto = !string.IsNullOrEmpty(item.Resposta_5_Texto) ? item.Resposta_5_Texto : "",
                    Resposta_61_Sim = item.Resposta_61_Sim.HasValue ? item.Resposta_61_Sim : false,
                    Resposta_61_Sim_Texto = item.Resposta_61_Sim.HasValue ? item.Resposta_61_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_61_Nao = item.Resposta_61_Nao.HasValue ? item.Resposta_61_Nao : false,
                    Resposta_61_Nao_Texto = item.Resposta_61_Nao.HasValue ? item.Resposta_61_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_61_NA = item.Resposta_61_NA.HasValue ? item.Resposta_61_NA : false,
                    Resposta_61_NA_Texto = item.Resposta_61_NA.HasValue ? item.Resposta_61_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_62_Sim = item.Resposta_62_Sim.HasValue ? item.Resposta_62_Sim : false,
                    Resposta_62_Sim_Texto = item.Resposta_62_Sim.HasValue ? item.Resposta_62_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_62_Nao = item.Resposta_62_Nao.HasValue ? item.Resposta_62_Nao : false,
                    Resposta_62_Nao_Texto = item.Resposta_62_Nao.HasValue ? item.Resposta_62_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_62_NA = item.Resposta_62_NA.HasValue ? item.Resposta_62_NA : false,
                    Resposta_62_NA_Texto = item.Resposta_62_NA.HasValue ? item.Resposta_62_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_63_Sim = item.Resposta_63_Sim.HasValue ? item.Resposta_63_Sim : false,
                    Resposta_63_Sim_Texto = item.Resposta_63_Sim.HasValue ? item.Resposta_63_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_63_Nao = item.Resposta_63_Nao.HasValue ? item.Resposta_63_Nao : false,
                    Resposta_63_Nao_Texto = item.Resposta_63_Nao.HasValue ? item.Resposta_63_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_63_NA = item.Resposta_63_NA.HasValue ? item.Resposta_63_NA : false,
                    Resposta_63_NA_Texto = item.Resposta_63_NA.HasValue ? item.Resposta_63_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_64_Sim = item.Resposta_64_Sim.HasValue ? item.Resposta_64_Sim : false,
                    Resposta_64_Sim_Texto = item.Resposta_64_Sim.HasValue ? item.Resposta_64_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_64_Nao = item.Resposta_64_Nao.HasValue ? item.Resposta_64_Nao : false,
                    Resposta_64_Nao_Texto = item.Resposta_64_Nao.HasValue ? item.Resposta_64_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_64_NA = item.Resposta_64_NA.HasValue ? item.Resposta_64_NA : false,
                    Resposta_64_NA_Texto = item.Resposta_64_NA.HasValue ? item.Resposta_64_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_65_Sim = item.Resposta_65_Sim.HasValue ? item.Resposta_65_Sim : false,
                    Resposta_65_Sim_Texto = item.Resposta_65_Sim.HasValue ? item.Resposta_65_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_65_Nao = item.Resposta_65_Nao.HasValue ? item.Resposta_65_Nao : false,
                    Resposta_65_Nao_Texto = item.Resposta_65_Nao.HasValue ? item.Resposta_65_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_65_NA = item.Resposta_65_NA.HasValue ? item.Resposta_65_NA : false,
                    Resposta_65_NA_Texto = item.Resposta_65_NA.HasValue ? item.Resposta_65_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_6_Texto = !string.IsNullOrEmpty(item.Resposta_6_Texto) ? item.Resposta_6_Texto : "",
                    Resposta_71_Sim = item.Resposta_71_Sim.HasValue ? item.Resposta_71_Sim : false,
                    Resposta_71_Sim_Texto = item.Resposta_71_Sim.HasValue ? item.Resposta_71_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_71_Nao = item.Resposta_71_Nao.HasValue ? item.Resposta_71_Nao : false,
                    Resposta_71_Nao_Texto = item.Resposta_71_Nao.HasValue ? item.Resposta_71_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_71_NA = item.Resposta_71_NA.HasValue ? item.Resposta_71_NA : false,
                    Resposta_71_NA_Texto = item.Resposta_71_NA.HasValue ? item.Resposta_71_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_72_Sim = item.Resposta_72_Sim.HasValue ? item.Resposta_72_Sim : false,
                    Resposta_72_Sim_Texto = item.Resposta_72_Sim.HasValue ? item.Resposta_72_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_72_Nao = item.Resposta_72_Nao.HasValue ? item.Resposta_72_Nao : false,
                    Resposta_72_Nao_Texto = item.Resposta_72_Nao.HasValue ? item.Resposta_72_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_72_NA = item.Resposta_72_NA.HasValue ? item.Resposta_72_NA : false,
                    Resposta_72_NA_Texto = item.Resposta_72_NA.HasValue ? item.Resposta_72_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_7_Texto = !string.IsNullOrEmpty(item.Resposta_7_Texto) ? item.Resposta_7_Texto : "",
                    Resposta_81_Sim = item.Resposta_81_Sim.HasValue ? item.Resposta_81_Sim : false,
                    Resposta_81_Sim_Texto = item.Resposta_81_Sim.HasValue ? item.Resposta_81_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_81_Nao = item.Resposta_81_Nao.HasValue ? item.Resposta_81_Nao : false,
                    Resposta_81_Nao_Texto = item.Resposta_81_Nao.HasValue ? item.Resposta_81_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_81_NA = item.Resposta_81_NA.HasValue ? item.Resposta_81_NA : false,
                    Resposta_81_NA_Texto = item.Resposta_81_NA.HasValue ? item.Resposta_81_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_82_Sim = item.Resposta_82_Sim.HasValue ? item.Resposta_82_Sim : false,
                    Resposta_82_Sim_Texto = item.Resposta_82_Sim.HasValue ? item.Resposta_82_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_82_Nao = item.Resposta_82_Nao.HasValue ? item.Resposta_82_Nao : false,
                    Resposta_82_Nao_Texto = item.Resposta_82_Nao.HasValue ? item.Resposta_82_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_82_NA = item.Resposta_82_NA.HasValue ? item.Resposta_82_NA : false,
                    Resposta_82_NA_Texto = item.Resposta_82_NA.HasValue ? item.Resposta_82_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_83_Sim = item.Resposta_83_Sim.HasValue ? item.Resposta_83_Sim : false,
                    Resposta_83_Sim_Texto = item.Resposta_83_Sim.HasValue ? item.Resposta_83_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_83_Nao = item.Resposta_83_Nao.HasValue ? item.Resposta_83_Nao : false,
                    Resposta_83_Nao_Texto = item.Resposta_83_Nao.HasValue ? item.Resposta_83_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_83_NA = item.Resposta_83_NA.HasValue ? item.Resposta_83_NA : false,
                    Resposta_83_NA_Texto = item.Resposta_83_NA.HasValue ? item.Resposta_83_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_8_Texto = !string.IsNullOrEmpty(item.Resposta_8_Texto) ? item.Resposta_8_Texto : "",
                    Resposta_91_Sim = item.Resposta_91_Sim.HasValue ? item.Resposta_91_Sim : false,
                    Resposta_91_Sim_Texto = item.Resposta_91_Sim.HasValue ? item.Resposta_91_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_91_Nao = item.Resposta_91_Nao.HasValue ? item.Resposta_91_Nao : false,
                    Resposta_91_Nao_Texto = item.Resposta_91_Nao.HasValue ? item.Resposta_91_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_91_NA = item.Resposta_91_NA.HasValue ? item.Resposta_91_NA : false,
                    Resposta_91_NA_Texto = item.Resposta_91_NA.HasValue ? item.Resposta_91_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_9_Texto = !string.IsNullOrEmpty(item.Resposta_9_Texto) ? item.Resposta_9_Texto : "",
                    Resposta_101_Sim = item.Resposta_101_Sim.HasValue ? item.Resposta_101_Sim : false,
                    Resposta_101_Sim_Texto = item.Resposta_101_Sim.HasValue ? item.Resposta_101_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_101_Nao = item.Resposta_101_Nao.HasValue ? item.Resposta_101_Nao : false,
                    Resposta_101_Nao_Texto = item.Resposta_101_Nao.HasValue ? item.Resposta_101_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_101_NA = item.Resposta_101_NA.HasValue ? item.Resposta_101_NA : false,
                    Resposta_101_NA_Texto = item.Resposta_101_NA.HasValue ? item.Resposta_101_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_101_Texto = !string.IsNullOrEmpty(item.Resposta_101_Texto) ? item.Resposta_101_Texto : "",
                    Resposta_102_Sim = item.Resposta_102_Sim.HasValue ? item.Resposta_102_Sim : false,
                    Resposta_102_Sim_Texto = item.Resposta_102_Sim.HasValue ? item.Resposta_102_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_102_Nao = item.Resposta_102_Nao.HasValue ? item.Resposta_102_Nao : false,
                    Resposta_102_Nao_Texto = item.Resposta_102_Nao.HasValue ? item.Resposta_102_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_102_NA = item.Resposta_102_NA.HasValue ? item.Resposta_102_NA : false,
                    Resposta_102_NA_Texto = item.Resposta_102_NA.HasValue ? item.Resposta_102_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_102_Texto = !string.IsNullOrEmpty(item.Resposta_102_Texto) ? item.Resposta_102_Texto : "",
                    Resposta_103_Sim = item.Resposta_103_Sim.HasValue ? item.Resposta_103_Sim : false,
                    Resposta_103_Sim_Texto = item.Resposta_103_Sim.HasValue ? item.Resposta_103_Sim == true ? "Sim" : "Não" : "Não",
                    Resposta_103_Nao = item.Resposta_103_Nao.HasValue ? item.Resposta_103_Nao : false,
                    Resposta_103_Nao_Texto = item.Resposta_103_Nao.HasValue ? item.Resposta_103_Nao == true ? "Sim" : "Não" : "Não",
                    Resposta_103_NA = item.Resposta_103_NA.HasValue ? item.Resposta_103_NA : false,
                    Resposta_103_NA_Texto = item.Resposta_103_NA.HasValue ? item.Resposta_103_NA == true ? "Sim" : "Não" : "Não",
                    Resposta_103_Texto = !string.IsNullOrEmpty(item.Resposta_103_Texto) ? item.Resposta_103_Texto : "",
                    Final_Responsavel = !string.IsNullOrEmpty(item.Final_Responsavel) ? item.Final_Responsavel : "",
                    Final_Data = item.Final_Data,
                    Final_Data_Texto = item.Final_Data.HasValue ? item.Final_Data.Value.ToString("yyyy-MM-dd") : "",
                    DataHora_Criacao = item.DataHora_Criacao,
                    DataHora_Criacao_Texto = item.DataHora_Criacao.HasValue ? item.DataHora_Criacao.Value.ToString("yyyy-MM-dd") : "",
                    Utilizador_Criacao = item.Utilizador_Criacao,
                    Utilizador_Criacao_Texto = !string.IsNullOrEmpty(item.Utilizador_Criacao) ? DBUserConfigurations.GetById(item.Utilizador_Criacao).Nome : "",
                    DataHora_Modificacao = item.DataHora_Modificacao,
                    DataHora_Modificacao_Texto = item.DataHora_Modificacao.HasValue ? item.DataHora_Modificacao.Value.ToString("yyyy-MM-dd") : "",
                    Utilizador_Modificacao = item.Utilizador_Modificacao,
                    Utilizador_Modificacao_Texto = !string.IsNullOrEmpty(item.Utilizador_Modificacao) ? DBUserConfigurations.GetById(item.Utilizador_Modificacao).Nome : ""
                };
            }
            return null;
        }

        public static List<QuestionarioFornecedorGestaoAmbientalViewModel> ParseToViewModel(this List<QuestionarioFornecedorGestaoAmbiental> items)
        {
            List<QuestionarioFornecedorGestaoAmbientalViewModel> Questionarios = new List<QuestionarioFornecedorGestaoAmbientalViewModel>();
            if (items != null)
                items.ForEach(x =>
                    Questionarios.Add(x.ParseToViewModel()));
            return Questionarios;
        }

        public static QuestionarioFornecedorGestaoAmbiental ParseToDB(this QuestionarioFornecedorGestaoAmbientalViewModel item)
        {
            if (item != null)
            {
                return new QuestionarioFornecedorGestaoAmbiental()
                {
                    Codigo = item.Codigo,
                    Versao = item.Versao,
                    ID_Fornecedor = item.ID_Fornecedor,
                    Fornecedor = item.Fornecedor,
                    Actividade = item.Actividade,
                    Responsavel = item.Responsavel,
                    Funcao = item.Funcao,
                    Telefone = item.Telefone,
                    Procedimento = item.Procedimento,
                    Email = item.Email,
                    Resposta_11_Sim = item.Resposta_11_Sim,
                    Resposta_11_Nao = item.Resposta_11_Nao,
                    Resposta_11_NA = item.Resposta_11_NA,
                    Resposta_11_Texto = item.Resposta_11_Texto,
                    Resposta_12_Texto = item.Resposta_12_Texto,
                    Resposta_13_Texto = item.Resposta_13_Texto,
                    Resposta_21_Sim = item.Resposta_21_Sim,
                    Resposta_21_Nao = item.Resposta_21_Nao,
                    Resposta_21_NA = item.Resposta_21_NA,
                    Resposta_22_Sim = item.Resposta_22_Sim,
                    Resposta_22_Nao = item.Resposta_22_Nao,
                    Resposta_22_NA = item.Resposta_22_NA,
                    Resposta_23_Texto = item.Resposta_23_Texto,
                    Resposta_2_Texto = item.Resposta_2_Texto,
                    Resposta_31_Sim = item.Resposta_31_Sim,
                    Resposta_31_Nao = item.Resposta_31_Nao,
                    Resposta_31_NA = item.Resposta_31_NA,
                    Resposta_32_NA = item.Resposta_32_NA,
                    Resposta_32_Texto = item.Resposta_32_Texto,
                    Resposta_3_Texto = item.Resposta_3_Texto,
                    Resposta_41_Sim = item.Resposta_41_Sim,
                    Resposta_41_Nao = item.Resposta_41_Nao,
                    Resposta_41_NA = item.Resposta_41_NA,
                    Resposta_42_Sim = item.Resposta_42_Sim,
                    Resposta_42_Nao = item.Resposta_42_Nao,
                    Resposta_42_NA = item.Resposta_42_NA,
                    Resposta_43_Texto = item.Resposta_43_Texto,
                    Resposta_4_Texto = item.Resposta_4_Texto,
                    Resposta_51_Sim = item.Resposta_51_Sim,
                    Resposta_51_Nao = item.Resposta_51_Nao,
                    Resposta_51_NA = item.Resposta_51_NA,
                    Resposta_52_Sim = item.Resposta_52_Sim,
                    Resposta_52_Nao = item.Resposta_52_Nao,
                    Resposta_52_NA = item.Resposta_52_NA,
                    Resposta_5_Texto = item.Resposta_5_Texto,
                    Resposta_61_Sim = item.Resposta_61_Sim,
                    Resposta_61_Nao = item.Resposta_61_Nao,
                    Resposta_61_NA = item.Resposta_61_NA,
                    Resposta_62_Sim = item.Resposta_62_Sim,
                    Resposta_62_Nao = item.Resposta_62_Nao,
                    Resposta_62_NA = item.Resposta_62_NA,
                    Resposta_63_Sim = item.Resposta_63_Sim,
                    Resposta_63_Nao = item.Resposta_63_Nao,
                    Resposta_63_NA = item.Resposta_63_NA,
                    Resposta_64_Sim = item.Resposta_64_Sim,
                    Resposta_64_Nao = item.Resposta_64_Nao,
                    Resposta_64_NA = item.Resposta_64_NA,
                    Resposta_65_Sim = item.Resposta_65_Sim,
                    Resposta_65_Nao = item.Resposta_65_Nao,
                    Resposta_65_NA = item.Resposta_65_NA,
                    Resposta_6_Texto = item.Resposta_6_Texto,
                    Resposta_71_Sim = item.Resposta_71_Sim,
                    Resposta_71_Nao = item.Resposta_71_Nao,
                    Resposta_71_NA = item.Resposta_71_NA,
                    Resposta_72_Sim = item.Resposta_72_Sim,
                    Resposta_72_Nao = item.Resposta_72_Nao,
                    Resposta_72_NA = item.Resposta_72_NA,
                    Resposta_7_Texto = item.Resposta_7_Texto,
                    Resposta_81_Sim = item.Resposta_81_Sim,
                    Resposta_81_Nao = item.Resposta_81_Nao,
                    Resposta_81_NA = item.Resposta_81_NA,
                    Resposta_82_Sim = item.Resposta_82_Sim,
                    Resposta_82_Nao = item.Resposta_82_Nao,
                    Resposta_82_NA = item.Resposta_82_NA,
                    Resposta_83_Sim = item.Resposta_83_Sim,
                    Resposta_83_Nao = item.Resposta_83_Nao,
                    Resposta_83_NA = item.Resposta_83_NA,
                    Resposta_8_Texto = item.Resposta_8_Texto,
                    Resposta_91_Sim = item.Resposta_91_Sim,
                    Resposta_91_Nao = item.Resposta_91_Nao,
                    Resposta_91_NA = item.Resposta_91_NA,
                    Resposta_9_Texto = item.Resposta_9_Texto,
                    Resposta_101_Sim = item.Resposta_101_Sim,
                    Resposta_101_Nao = item.Resposta_101_Nao,
                    Resposta_101_NA = item.Resposta_101_NA,
                    Resposta_101_Texto = item.Resposta_101_Texto,
                    Resposta_102_Sim = item.Resposta_102_Sim,
                    Resposta_102_Nao = item.Resposta_102_Nao,
                    Resposta_102_NA = item.Resposta_102_NA,
                    Resposta_102_Texto = item.Resposta_102_Texto,
                    Resposta_103_Sim = item.Resposta_103_Sim,
                    Resposta_103_Nao = item.Resposta_103_Nao,
                    Resposta_103_NA = item.Resposta_103_NA,
                    Resposta_103_Texto = item.Resposta_103_Texto,
                    Final_Responsavel = item.Final_Responsavel,
                    Final_Data = string.IsNullOrEmpty(item.Final_Data_Texto) ? (DateTime?)null : DateTime.Parse(item.Final_Data_Texto),
                    DataHora_Criacao = item.DataHora_Criacao,
                    Utilizador_Criacao = item.Utilizador_Criacao,
                    DataHora_Modificacao = item.DataHora_Modificacao,
                    Utilizador_Modificacao = item.Utilizador_Modificacao
                };
            }
            return null;
        }

        public static List<QuestionarioFornecedorGestaoAmbiental> ParseToDB(this List<QuestionarioFornecedorGestaoAmbientalViewModel> items)
        {
            List<QuestionarioFornecedorGestaoAmbiental> Questionarios = new List<QuestionarioFornecedorGestaoAmbiental>();
            if (items != null)
                items.ForEach(x =>
                    Questionarios.Add(x.ParseToDB()));
            return Questionarios;
        }
    }
}
