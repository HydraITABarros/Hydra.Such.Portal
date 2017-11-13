using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ConfigNumerationsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? Auto { get; set; }
        public bool? Manual { get; set; }
        public string Prefix { get; set; }
        public int? TotalDigitIncrement { get; set; }
        public int? IncrementQuantity { get; set; }
        public string LastNumerationUsed { get; set; }
    }
}
