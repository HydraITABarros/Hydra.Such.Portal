using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Such.Data.Logic.Compras
{
    public class BillingReceptionRepo : IDisposable
    {
        private SuchDBContext ctx;

        public BillingReceptionRepo()
        {
            ctx = new SuchDBContext();
        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }

        #region BillingReception

        public BillingReceptionModel Create(BillingReceptionModel item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            ctx.RececaoFaturacao.Add(item.ParseToDB());

            return item;
        }

        public BillingReceptionModel Update(BillingReceptionModel item)
        {
            ctx.RececaoFaturacao.Update(item.ParseToDB());
            return item;
        }

        public List<BillingReceptionModel> GetAll()
        {
            try
            {
                return ctx.RececaoFaturacao
                    .OrderByDescending(x => x.Id)
                    .ToList()
                    .ParseToViewModel();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BillingReceptionModel GetById(string id)
        {
            try
            {
                return ctx.RececaoFaturacao
                    .Include(x => x.RececaoFaturacaoWorkflow)
                    .SingleOrDefault(x => x.Id == id)
                    .ParseToViewModel();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Delete(BillingReceptionModel item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            ctx.RececaoFaturacao.Remove(item.ParseToDB());
        }

        #endregion

        #region WF

        public RececaoFaturacaoWorkflow Create(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            item.DataCriacao = DateTime.Now;
            var item1 = ctx.RececaoFaturacaoWorkflow.Add(item);

            return item;
        }

        public RececaoFaturacaoWorkflow Update(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            item.DataModificacao = DateTime.Now;
            ctx.RececaoFaturacaoWorkflow.Update(item);

            return item;
        }

        public void Delete(RececaoFaturacaoWorkflow item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            ctx.RececaoFaturacaoWorkflow.Remove(item);
        }

        #endregion

        void IDisposable.Dispose()
        {
            ctx.Dispose();
        }
    }
}
