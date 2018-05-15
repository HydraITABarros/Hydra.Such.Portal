using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class UserConfigurationsViewModel : ErrorHandler
    {
        public string IdUser { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public bool Administrator { get; set; }
        public string Regiao { get; set; }
        public string Area { get; set; }
        public string Cresp { get; set; }
        public List<UserAccessesViewModel> UserAccesses { get; set; }
        public List<ProfileModelsViewModel> UserProfiles { get; set; }
        public List<UserDimensionsViewModel> AllowedUserDimensions { get; set; }
        public List<UserAcessosLocalizacoesViewModel> UserAcessosLocalizacoes { get; set; }
    }
}
