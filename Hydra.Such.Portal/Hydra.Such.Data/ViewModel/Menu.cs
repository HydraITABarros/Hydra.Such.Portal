using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel
{
    public partial class MenuViewModel
    {
        public string Title { get; set; }
        public int Weight { get; set; }
        public string Icon { get; set; }
        public string ActionLink { get; set; }

        public IList<MenuViewModel> Submenu { get; set; }
    }
}
