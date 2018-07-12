using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Hydra.Such.Data.Logic.Telemoveis
{
    public static class DBTelemoveis
    {
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
    }
}
