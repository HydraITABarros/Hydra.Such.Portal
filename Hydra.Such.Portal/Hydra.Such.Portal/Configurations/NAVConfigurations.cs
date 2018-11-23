using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Portal.Configurations
{
    public class NAVConfigurations
    {
        public string NAVDatabaseName { get; set; }
        public string NAVCompanyName { get; set; }
        public string NAV2009DatabaseName { get; set; }
        public string NAV2009CompanyName { get; set; }
        public string ReportServerURL { get; set; }
        public string ReportServerURL_PDF { get; set; }
    }

    public class GeneralConfigurations
    {
        public string FileUploadFolder { get; set; }
        public string Conn { get; set; }
    }
}
