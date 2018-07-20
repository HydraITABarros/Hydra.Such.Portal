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
    public static class DBBillingReceptionWFAttach
    {
        #region CRUD
    
        public static RececaoFaturacaoWorkflowAnexo Create(RececaoFaturacaoWorkflowAnexo item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.RececaoFaturacaoWorkflowAnexo.Add(item);
                    ctx.SaveChanges();
                }
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Parse Utilities
        public static BillingRecWorkflowModelAttached ParseToViewModel(this RececaoFaturacaoWorkflowAnexo item)
        {
            if (item != null)
            {
                return new BillingRecWorkflowModelAttached()
                {
                    Id = item.Id,
                    IdWorkFlow=item.Idwokflow,
                    File=item.Caminho,
                    Description = item.Comentario
                };
    
            }
            return null;
        }

        public static List<BillingRecWorkflowModelAttached> ParseToViewModel(this List<RececaoFaturacaoWorkflowAnexo> items)
        {
            List<BillingRecWorkflowModelAttached> parsedItems = new List<BillingRecWorkflowModelAttached>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToViewModel()));
            return parsedItems;
        }

        public static RececaoFaturacaoWorkflowAnexo ParseToDB(this BillingRecWorkflowModelAttached item)
        {
            if (item != null)
            {
                return new RececaoFaturacaoWorkflowAnexo()
                {
                    Id = item.Id,
                    Idwokflow=item.IdWorkFlow,
                    Caminho=item.File,
                    Comentario=item.Description
                };
            }
            return null;
        }

        public static List<RececaoFaturacaoWorkflowAnexo> ParseToDB(this List<BillingRecWorkflowModelAttached> items)
        {
            List<RececaoFaturacaoWorkflowAnexo> parsedItems = new List<RececaoFaturacaoWorkflowAnexo>();
            if (items != null)
                items.ForEach(x =>
                    parsedItems.Add(x.ParseToDB()));
            return parsedItems;
        }
        #endregion

    }
}
