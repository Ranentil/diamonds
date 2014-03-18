using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Localization
    {
        public int id { get; set; }
        public string name { get; set; }
        public string pl { get; set; }
        public string en { get; set; }
        
    }

}