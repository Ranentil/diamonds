using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diamonds.Models.Entities
{
    public class Dictionary
    {
        public int id { get; set; }
        public string titlePl { get; set; }
        public string titleEn { get; set; }
        public string textPl { get; set; }
        public string textEn { get; set; }

        public string title { get { return lang(titlePl, titleEn); } }
        public string text { get { return lang(textPl, textEn); } }

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