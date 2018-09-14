using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Menu
    {
        public Menu()
        {
            FeaturesMenus = new HashSet<FeaturesMenus>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Weight { get; set; }
        public string Icon { get; set; }
        public int? Parent { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string RouteParameters { get; set; }
        public string HtmlAttributes { get; set; }

        public ICollection<FeaturesMenus> FeaturesMenus { get; set; }
    }
}
