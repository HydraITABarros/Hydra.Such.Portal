using System;
using System.Collections.Generic;
using System.Text;
using static Hydra.Such.Data.Enumerations;

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
        public string Centroresp { get; set; }
        public string EmployeeNo { get; set; }
        public string ProcedimentosEmailEnvioParaCA { get; set; }
        public string ProcedimentosEmailEnvioParaArea { get; set; }
        public string ProcedimentosEmailEnvioParaArea2 { get; set; }
        public int? ReceptionConfig { get; set; }
        public BillingReceptionAreas? RFPerfil { get; set; }
        public BillingReceptionUserProfiles? RFPerfilVisualizacao { get; set; }
        public string RFFiltroArea { get; set; }
        public string RFNomeAbreviado { get; set; }
        public bool? RFRespostaContabilidade { get; set; }
        public bool? RFAlterarDestinatarios { get; set; }
        public string RFMailEnvio { get; set; }
        public string NumSerieNotasCredito { get; set; }
        public string NumSerieNotasDebito { get; set; }
        public string NumSerieFaturas { get; set; }
        public string NumSeriePreFaturasCompra { get; set; }
        public string SuperiorHierarquico { get; set; }

        public List<UserAccessesViewModel> UserAccesses { get; set; }
        public List<ProfileModelsViewModel> UserProfiles { get; set; }
        public List<UserDimensionsViewModel> AllowedUserDimensions { get; set; }
        public List<UserAcessosLocalizacoesViewModel> UserAcessosLocalizacoes { get; set; }
    }
}
