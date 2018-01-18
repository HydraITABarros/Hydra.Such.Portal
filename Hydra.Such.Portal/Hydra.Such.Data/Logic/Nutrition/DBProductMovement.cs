using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Nutrition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Nutrition
{
    public static class DBProductMovement
    {
        #region CRUD
        public static MovimentoDeProdutos GetById(int NºMovimentoProduto)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentoDeProdutos.Where(x => x.NºMovimentos == NºMovimentoProduto).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<MovimentoDeProdutos> GetByNoprodLocation(string NºProduto, string CodeLocal)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentoDeProdutos.Where(x => x.NºProduto == NºProduto && x.CódLocalização== CodeLocal).ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<MovimentoDeProdutos> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.MovimentoDeProdutos.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static MovimentoDeProdutos Update(MovimentoDeProdutos ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.MovimentoDeProdutos.Update(ObjectToUpdate);
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

        public static ProductMovementViewModel ParseToViewModel(this MovimentoDeProdutos item)
        {
            if (item != null)
            {
                return new ProductMovementViewModel()
                {
                    MovementNo = item.NºMovimentos,
                    DateRegister = item.DataRegisto.HasValue ? item.DataRegisto.Value.ToString("yyyy-MM-dd") : "",
                    MovementType = item.TipoMovimento,
                    DocumentNo = item.NºDocumento,
                    ProductNo = item.NºProduto,
                    Description = item.Descrição,
                    CodLocation = item.CódLocalização,
                    Quantity = item.Quantidade,
                    UnitCost = item.CustoUnitário,
                    Val = item.Valor,
                    ProjectNo = item.NºProjecto,
                    CodeRegion = item.CódigoRegião,
                    CodeFunctionalArea = item.CódigoÁrea,
                    CodeResponsabilityCenter = item.CódigoCentroResponsabilidade

                };
            }
            return null;
        }

        public static List<ProductMovementViewModel> ParseToViewModel(this List<MovimentoDeProdutos> items)
        {
            List<ProductMovementViewModel> parsedItems = new List<ProductMovementViewModel>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static MovimentoDeProdutos ParseToDatabase(ProductMovementViewModel x)
        {
            return new MovimentoDeProdutos()
            {
                NºMovimentos=x.MovementNo,
                DataRegisto= x.DateRegister != "" && x.DateRegister != null ? DateTime.Parse(x.DateRegister) : (DateTime?)null,
                TipoMovimento =x.MovementType,
                NºDocumento=x.DocumentNo,
                NºProduto=x.ProductNo,
                Descrição=x.Description,
                CódLocalização=x.CodLocation,
                Quantidade=x.Quantity,
                CustoUnitário=x.UnitCost,
                Valor=x.Val,
                NºProjecto=x.ProjectNo,
                CódigoRegião=x.CodeRegion,
                CódigoÁrea=x.CodeFunctionalArea,
                CódigoCentroResponsabilidade=x.CodeResponsabilityCenter
            };
        }

        public static List<MovimentoDeProdutos> ParseToDatabase(this List<ProductMovementViewModel> items)
        {
            List<MovimentoDeProdutos> movements = new List<MovimentoDeProdutos>();
            if (items != null)
                items.ForEach(x =>
                    movements.Add(ParseToDatabase(x)));
            return movements;
        }
    }
}
