using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Localization
    {
        public int id { get; set; }
        public string name { get; set; }
        public string pl { get; set; }
        public string en { get; set; }

        public string text { get { return lang(pl, en); } }

        private string lang(string pl, string en)
        {
            if (HttpContext.Current.Response.Cookies["lang"].Value == "en" && en != "" || pl == "")
                return en;
            return pl;
        }
    }

}