using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Such.NavApi
{
    public static class NAVProjects
    {
        public static bool CreateNavProject(WSCreateProject.WSJob ProjToCreate)
        {
            WSCreateProject.WSJob_Service NavServ = new WSCreateProject.WSJob_Service();

            NavServ.Credentials = new NetworkCredential()
            {
                Domain = "such",
                UserName = "navserver",
                Password = "Nav#2017!"
            };

            NavServ.Create(ref ProjToCreate);

            return true;
        }
    }
}
