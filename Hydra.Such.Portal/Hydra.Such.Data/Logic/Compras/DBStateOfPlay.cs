using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Microsoft.EntityFrameworkCore;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Logic.Request
{
    public static class DBStateOfPlay
    {
        #region CRUD

        public static List<PontosSituaçãoRq> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PontosSituaçãoRq
                        .OrderBy(x => x.NºRequisição)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PontosSituaçãoRq GetById(string requisitionId, int stateOfPlayId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PontosSituaçãoRq
                        .SingleOrDefault(x => x.NºRequisição == requisitionId && x.NºPedido == stateOfPlayId);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PontosSituaçãoRq Create(PontosSituaçãoRq item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PontosSituaçãoRq.Add(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static PontosSituaçãoRq Update(PontosSituaçãoRq item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PontosSituaçãoRq.Update(item);
                    ctx.SaveChanges();
                }

                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Delete(PontosSituaçãoRq item)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.PontosSituaçãoRq.Remove(item);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<PontosSituaçãoRq> GetForRequisition(string requisitionId)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.PontosSituaçãoRq
                        .Where(x => x.NºRequisição == requisitionId)
                        .OrderBy(x => x.NºRequisição)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Parse Utilities
        public static StateOfPlayViewModel ParseToViewModel(this PontosSituaçãoRq item)
        {
            if (item != null)
            {
                return new StateOfPlayViewModel()
                {
                    RequisitionNo = item.NºRequisição,
                    StateOfPlayId = item.NºPedido,
                    Question = item.PedidoDePontoSituação,
                    QuestionDate = item.DataPedido,
                    QuestionDateText = item.DataPedido == null ? "" : item.DataPedido.ToString("yyyy-MM-dd"),
                    QuestionTimeText = item.DataPedido == null ? "" : item.DataPedido.ToString("HH:mm"),
                    QuestionedBy = item.UtilizadorPedido,
                    QuestionedByText = item.UtilizadorPedido == null ? "" : DBUserConfigurations.GetById(item.UtilizadorPedido).Nome,
                    Answer = item.Resposta,
                    AnswerDate = item.DataResposta,
                    AnswerDateText = item.DataResposta == null ? "" : item.DataResposta.Value.ToString("yyyy-MM-dd"),
                    AnswerTimeText = item.DataResposta == null ? "" : item.DataResposta.Value.ToString("HH:mm"),
                    AnsweredBy = item.UtilizadorResposta,
                    AnsweredByText = item.UtilizadorResposta == null ? "" : DBUserConfigurations.GetById(item.UtilizadorResposta).Nome,
                    Read = item.ConfirmaçãoLeitura
                    //DimensionRegion = "",
                    //DimensionArea = "",
                    //DimensionCresp = ""
                };
            }
            return null;
        }

        public static List<StateOfPlayViewModel> ParseToViewModel(this List<PontosSituaçãoRq> items)
        {
            List<StateOfPlayViewModel> parsedItems = new List<StateOfPlayViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static PontosSituaçãoRq ParseToDB(this StateOfPlayViewModel item)
        {
            if (item != null)
            {
                return new PontosSituaçãoRq()
                {
                    NºRequisição = item.RequisitionNo,
                    NºPedido = item.StateOfPlayId,
                    PedidoDePontoSituação = item.Question,
                    DataPedido = item.QuestionDate,
                    UtilizadorPedido = item.QuestionedBy,
                    Resposta = item.Answer,
                    DataResposta = item.AnswerDate,
                    UtilizadorResposta = item.AnsweredBy,
                    ConfirmaçãoLeitura = item.Read,
                };
            }
            return null;
        }

        public static List<PontosSituaçãoRq> ParseToDB(this List<StateOfPlayViewModel> items)
        {
            List<PontosSituaçãoRq> parsedItems = new List<PontosSituaçãoRq>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion
    }
}
