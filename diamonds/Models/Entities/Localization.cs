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

        public Localization() { }

        private string lang(string pl, string en)
        {
            var cookie = HttpContext.Current.Request.Cookies["diamonds-lang"];
            if (cookie != null && cookie.Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }
    }

}