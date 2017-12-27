using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Approvals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Approvals
{
    public static class DBApprovalEmails
    {
        #region CRUD
        public static EmailsAprovações GetById(int id)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.EmailsAprovações.Where(x => x.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<EmailsAprovações> GetAll()
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    return ctx.EmailsAprovações.ToList();
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static EmailsAprovações Create(EmailsAprovações ObjectToCreate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.EmailsAprovações.Add(ObjectToCreate);
                    ctx.SaveChanges();
                }

                return ObjectToCreate;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static EmailsAprovações Update(EmailsAprovações ObjectToUpdate)
        {
            try
            {
                using (var ctx = new SuchDBContext())
                {
                    ctx.EmailsAprovações.Update(ObjectToUpdate);
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


        #region Parses
        public static ApprovalEmailViewModel ParseToViewModel(EmailsAprovações x)
        {
            return new ApprovalEmailViewModel()
            {
                Id = x.Id,
                MovementNo = x.NºMovimento,
                ToEmail = x.EmailDestinatário,
                ToName = x.NomeDestinatário,
                Subject = x.Assunto,
                SentDate = x.DataHoraEmail,
                EmailContent = x.TextoEmail,
                Sent = x.Enviado,
                SendObs = x.ObservaçõesEnvio
            };
        }
        public static EmailsAprovações ParseToDatabase(ApprovalEmailViewModel x)
        {
            return new EmailsAprovações()
            {
                Id = x.Id,
                NºMovimento = x.MovementNo,
                EmailDestinatário = x.ToEmail,
                NomeDestinatário = x.ToName,
                Assunto = x.Subject,
                DataHoraEmail = x.SentDate,
                TextoEmail = x.EmailContent,
                Enviado = x.Sent,
                ObservaçõesEnvio = x.SendObs
            };
        }
        #endregion
    }
}
