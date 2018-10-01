using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Database
{
    public partial class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Weight { get; set; }
        public string Icon { get; set; }
        public int? Parent { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string RouteParameters { get; set; }
        public string HtmlAttributes { get; set; }
        public bool Active { get; set; }

        public ICollection<FeaturesMenus> FeaturesMenus { get; set; }

        [NotMapped]
        public ICollection<Features> Features { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}

