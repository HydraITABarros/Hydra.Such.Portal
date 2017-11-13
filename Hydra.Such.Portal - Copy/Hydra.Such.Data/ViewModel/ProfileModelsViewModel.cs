using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ProfileModelsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        
        public List<AccessProfileModelView> ProfileModelAccesses { get; set; }
    }

    public class UserProfileViewModel : ProfileModelsViewModel
    {
        public string UserId { get; set; }
    }
}
