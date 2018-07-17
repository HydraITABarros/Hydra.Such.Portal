using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Telemoveis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Hydra.Such.Data.Logic.Telemoveis
{
    public class DBTelemoveis
    {
        #region CRUD
        /// <summary>
        /// Lista de todos os registos (Telemóveis e placas de rede)
        /// </summary>
        /// <returns></returns>
        public static List<TelemoveisEquipamentos> GetAllTelemoveisEquipamentosToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TelemoveisEquipamentos.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        /// <summary>
        /// Lista de todos os registos por tipo, Telemóveis [0] ou placas de rede [1]
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static List<TelemoveisEquipamentos> GetAllTelemoveisEquipamentosTypeToList(int tipo)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TelemoveisEquipamentos.Where(p => p.Tipo == tipo).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Devolve os dados de um registo da tabela Telemoveis_Equipamentos
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static TelemoveisEquipamentos GetTelemoveisEquipamentos(int tipo, string imei)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TelemoveisEquipamentos.Where(p => p.Tipo == tipo).Where(p => p.Imei == imei).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Criação de registo
        /// </summary>
        /// <param name="ObjectToCreate"></param>
        /// <returns></returns>
        public static TelemoveisEquipamentos Create (TelemoveisEquipamentos ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriacao = DateTime.Now;
                    ctx.TelemoveisEquipamentos.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Actualização de registo
        /// </summary>
        /// <param name="ObjectToUpdate"></param>
        /// <returns></returns>
        public static TelemoveisEquipamentos Update(TelemoveisEquipamentos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificacao = DateTime.Now;
                    ctx.TelemoveisEquipamentos.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion


        public static List<TelemoveisEquipamentosView> GetAllTelemoveisEquipamentosViewToList()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.TelemoveisEquipamentos.Select(TelemoveisEquipamentos => new TelemoveisEquipamentosView()
                    {
                        Tipo = TelemoveisEquipamentos.Tipo,
                        Imei = TelemoveisEquipamentos.Imei,
                        Marca = TelemoveisEquipamentos.Marca,
                        Modelo = TelemoveisEquipamentos.Modelo,
                        Estado = TelemoveisEquipamentos.Estado,
                        Cor = TelemoveisEquipamentos.Cor,
                        Observacoes = TelemoveisEquipamentos.Observacoes,
                        DataRecepcao = TelemoveisEquipamentos.DataRecepcao,
                        Documento = TelemoveisEquipamentos.Documento,
                        DocumentoRecepcao = TelemoveisEquipamentos.DocumentoRecepcao,
                        Utilizador = TelemoveisEquipamentos.Utilizador,
                        DataAlteracao = TelemoveisEquipamentos.DataAlteracao,
                        DevolvidoBk = TelemoveisEquipamentos.DevolvidoBk,
                        NumEmpregadoComprador = TelemoveisEquipamentos.NumEmpregadoComprador,
                        NomeComprador = TelemoveisEquipamentos.NomeComprador,
                        Devolvido = TelemoveisEquipamentos.Devolvido,
                        UtilizadorCriacao = TelemoveisEquipamentos.UtilizadorCriacao,
                        DataHoraCriacao = TelemoveisEquipamentos.DataHoraCriacao,
                        UtilizadorModificacao = TelemoveisEquipamentos.UtilizadorModificacao,
                        DataHoraModificacao = TelemoveisEquipamentos.DataHoraModificacao,
                        Tipo_Show = TelemoveisEquipamentos.Tipo == 0 ? "Equipamento" : "Placa de Rede",
                        Estado_Show = TelemoveisEquipamentos.Estado == 0 ? "Novo" : "Usado",
                        Devolvido_Show = TelemoveisEquipamentos.Devolvido == 0 ? "" : TelemoveisEquipamentos.Devolvido == 1 ? "Devolvido" : TelemoveisEquipamentos.Devolvido == 2 ? "Abate TMN" : TelemoveisEquipamentos.Devolvido == 3 ? "Vendido" : TelemoveisEquipamentos.Devolvido == 4 ? "Perdido" : TelemoveisEquipamentos.Devolvido == 5 ? "Roubado" : TelemoveisEquipamentos.Devolvido == 6 ? "Empréstimo" : TelemoveisEquipamentos.Devolvido == 7 ? "Não Devolvido" : "",
                        DataRecepcao_Show = TelemoveisEquipamentos.DataRecepcao == null ? "" : TelemoveisEquipamentos.DataRecepcao.Value.ToString("yyyy-MM-dd"),
                        DataAlteracao_Show = TelemoveisEquipamentos.DataAlteracao == null ? "" : TelemoveisEquipamentos.DataAlteracao.Value.ToString("yyyy-MM-dd")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static TelemoveisEquipamentosView CastTelemoveisEquipamentosToView(TelemoveisEquipamentos ObjectToTransform)
        {
            TelemoveisCartoes telemoveisCartoes = new TelemoveisCartoes();
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    telemoveisCartoes = ctx.TelemoveisCartoes.Where(p => p.Imei == ObjectToTransform.Imei).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                
            }

            
            TelemoveisEquipamentosView view = new TelemoveisEquipamentosView()
            {
                Tipo = ObjectToTransform.Tipo,
                Imei = ObjectToTransform.Imei,
                Marca = ObjectToTransform.Marca,
                Modelo = ObjectToTransform.Modelo,
                Estado = ObjectToTransform.Estado,
                Cor = ObjectToTransform.Cor,
                Observacoes = ObjectToTransform.Observacoes,
                DataRecepcao = ObjectToTransform.DataRecepcao,
                Documento = ObjectToTransform.Documento,
                DocumentoRecepcao = ObjectToTransform.DocumentoRecepcao,
                Utilizador = ObjectToTransform.Utilizador,
                DataAlteracao = ObjectToTransform.DataAlteracao,
                DevolvidoBk = ObjectToTransform.DevolvidoBk,
                NumEmpregadoComprador = ObjectToTransform.NumEmpregadoComprador,
                NomeComprador = ObjectToTransform.NomeComprador,
                Devolvido = ObjectToTransform.Devolvido,
                UtilizadorCriacao = ObjectToTransform.UtilizadorCriacao,
                DataHoraCriacao = ObjectToTransform.DataHoraCriacao,
                UtilizadorModificacao = ObjectToTransform.UtilizadorModificacao,
                DataHoraModificacao = ObjectToTransform.DataHoraModificacao,
                Tipo_Show = ObjectToTransform.Tipo == 0 ? "Equipamento" : "Placa de Rede",
                Estado_Show = ObjectToTransform.Estado == 0 ? "Novo" : "Usado",
                Devolvido_Show = ObjectToTransform.Devolvido == 0 ? "" : ObjectToTransform.Devolvido == 1 ? "Devolvido" : ObjectToTransform.Devolvido == 2 ? "Abate TMN" : ObjectToTransform.Devolvido == 3 ? "Vendido" : ObjectToTransform.Devolvido == 4 ? "Perdido" : ObjectToTransform.Devolvido == 5 ? "Roubado" : ObjectToTransform.Devolvido == 6 ? "Empréstimo" : ObjectToTransform.Devolvido == 7 ? "Não Devolvido" : "",
                DataRecepcao_Show = ObjectToTransform.DataRecepcao == null ? "" : ObjectToTransform.DataRecepcao.Value.ToString("yyyy-MM-dd"),
                DataAlteracao_Show = ObjectToTransform.DataAlteracao == null ? "" : ObjectToTransform.DataAlteracao.Value.ToString("yyyy-MM-dd"),
                NomeUtilizadorCartao_Show = telemoveisCartoes != null ? telemoveisCartoes.Nome : string.Empty,
                DataAtribuicaoUtilizadorCartao_Show = telemoveisCartoes != null ? telemoveisCartoes.DataAtribuicao.Value.ToString("yyyy-MM-dd") : string.Empty
            };

            return view;
        }
    }
}
