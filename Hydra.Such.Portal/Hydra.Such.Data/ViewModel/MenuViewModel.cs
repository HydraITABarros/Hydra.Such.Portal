using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel
{
    public partial class MenuViewModel
    {
        public string Title { get; set; }
        public int Weight { get; set; }
        public string Icon { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public object RouteParameters { get; set; }
        public object HtmlAttributes { get; set; }

        public IList<MenuViewModel> Submenu { get; set; }
    }
}
