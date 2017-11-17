using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.FH;
using Hydra.Such.Data.ViewModel.Viaturas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Viatura
{
    public class DBCartoesEApolices
    {

        public static CartõesEApólices GetByTipoAndNumero(int tipo, string numero)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CartõesEApólices.FirstOrDefault(x => x.Tipo == tipo && x.Número == numero);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static List<CartõesEApólices> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.CartõesEApólices.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static CartõesEApólices Create(CartõesEApólices ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToCreate.DataHoraCriação = DateTime.Now;
                    ctx.CartõesEApólices.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static CartõesEApólices Update(CartõesEApólices ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ObjectToUpdate.DataHoraModificação = DateTime.Now;
                    ctx.CartõesEApólices.Update(ObjectToUpdate);
                    ctx.SaveChanges();
                }

                return ObjectToUpdate;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static bool Delete(CartõesEApólices ObjectToDelete)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.CartõesEApólices.Remove(ObjectToDelete);
                    ctx.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static CartõesEApólices ParseToDB(CartoesEApolicesViewModel x)
        {
            CartõesEApólices cartoesEApolices = new CartõesEApólices()
            {
                Tipo = x.Tipo,
                Número = x.Numero,
                Descrição = x.Descricao,
                Fornecedor = x.Fornecedor,
                DataHoraCriação = x.DataHoraCriacao,
                DataHoraModificação = x.DataHoraModificacao,
                UtilizadorCriação = x.UtilizadorCriacao,
                UtilizadorModificação = x.UtilizadorModificacao
            };

            cartoesEApolices.DataInício = x.DataInicio != "" && x.DataInicio != null ? DateTime.Parse(x.DataInicio) : (DateTime?)null;
            cartoesEApolices.DataFim = x.DataFim != "" && x.DataFim != null ? DateTime.Parse(x.DataFim) : (DateTime?)null;

            return cartoesEApolices;
        }

        public static CartoesEApolicesViewModel ParseToViewModel(CartõesEApólices x)
        {
            CartoesEApolicesViewModel cartoesEApolices = new CartoesEApolicesViewModel()
            {
                Tipo = x.Tipo,
                Numero = x.Número,
                Descricao = x.Descrição,
                Fornecedor = x.Fornecedor,
                DataHoraCriacao = x.DataHoraCriação,
                DataHoraModificacao = x.DataHoraModificação,
                UtilizadorCriacao = x.UtilizadorCriação,
                UtilizadorModificacao = x.UtilizadorModificação
            };

            if (x.DataInício != null) cartoesEApolices.DataInicio = x.DataInício.Value.ToString("yyyy-MM-dd");
            if (x.DataFim != null) cartoesEApolices.DataFim = x.DataFim.Value.ToString("yyyy-MM-dd");

            return cartoesEApolices;
        }

        public static List<CartoesEApolicesViewModel> ParseListToViewModel(List<CartõesEApólices> x)
        {
            List<CartoesEApolicesViewModel> result = new List<CartoesEApolicesViewModel>();

            x.ForEach(y => result.Add(ParseToViewModel(y)));
            return result;
        }

    }
}
