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

        public ContactViewModel()
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.CreateDate = string.Empty;
            this.UpdateDate = string.Empty;
            this.CreateUser = string.Empty;
            this.UpdateUser = string.Empty;
        }
    }
}
