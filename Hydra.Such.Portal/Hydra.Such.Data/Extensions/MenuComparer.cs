using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Such.Data.Extensions
{
    public class MenuComparer : EqualityComparer<Menu>
    {
        public override bool Equals(Menu i1, Menu i2)
        {
            bool rslt = (i1.Id == i2.Id) && (i1.Title == i2.Title);
            return rslt;
        }

        public override int GetHashCode(Menu x)
        {
            return x.Id.GetHashCode() ^ x.Id.GetHashCode();
        }
    }
}
