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
        //public static MovimentoDeProdutos GetById(int NºMovimentoProduto)
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.MovimentoDeProdutos.Where(x => x.NºMovimentos == NºMovimentoProduto).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}
        

        //public static List<MovimentoDeProdutos> GetAll()
        //{
        //    try
        //    {
        //        using (var ctx = new SuchDBContext())
        //        {
        //            return ctx.MovimentoDeProdutos.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

     
        //#endregion

        //public static ProductMovementViewModel ParseToViewModel(this MovimentoDeProdutos item)
        //{
        //    if (item != null)
        //    {
        //        return new ProductMovementViewModel()
        //        {
        //            MovementNo=item.NºMovimentos,
        //            DateRegister=item.DataRegisto,
        //            MovementType=item.TipoMovimento,
        //            DocumentNo=item.NºDocumento,
        //            ProductNo=item.NºProduto,
        //            Description=item.Descrição,
        //            CodLocation=item.CódLocalização,
        //            Quantity=item.Quantidade,
        //            UnitCost=item.CustoUnitário,
        //            Val=item.Valor,
        //            ProjectNo=item.NºProjecto,
        //            CodeRegion=item.CódigoRegião,
        //            CodeFunctionalArea=item.CódigoÁrea,
        //            CodeResponsabilityCenter=item.CódigoCentroResponsabilidade

        //        };
        //    }
        //    return null;
        //}

        //public static List<ProductivityUnitViewModel> ParseToViewModel(this List<UnidadesProdutivas> items)
        //{
        //    List<ProductivityUnitViewModel> parsedItems = new List<ProductivityUnitViewModel>();
        //    if (items != null)
        //        items.ForEach(x =>
        //            parsedItems.Add(x.ParseToViewModel()));
        //    return parsedItems;
        //}
    }
}
