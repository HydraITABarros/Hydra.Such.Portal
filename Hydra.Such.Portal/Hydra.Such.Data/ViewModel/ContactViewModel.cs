using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ContactViewModel : ErrorHandler
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }

        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Regiao { get; set; }
        public string RegiaoText { get; set; }
        public string VATNumber { get; set; }
        public string PersonContact { get; set; }
        public string PhoneContact { get; set; }
        public string ContactFunction { get; set; }
        public string MobilePhoneContact { get; set; }
        public string EmailContact { get; set; }
        public string Notes { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }


        public ContactViewModel()
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.CreateDate = string.Empty;
            this.UpdateDate = string.Empty;
            this.CreateUser = string.Empty;
            this.UpdateUser = string.Empty;


            this.Address = string.Empty;
            this.ZipCode = string.Empty;
            this.City = string.Empty;
            this.Phone = string.Empty;
            this.Email = string.Empty;
            this.Regiao = string.Empty;
            this.VATNumber = string.Empty;
            this.PersonContact = string.Empty;
            this.PhoneContact = string.Empty;
            this.MobilePhoneContact = string.Empty;
            this.EmailContact = string.Empty;
            this.Notes = string.Empty;
        }
    }
}
