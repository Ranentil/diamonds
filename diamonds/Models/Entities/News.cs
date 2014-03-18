using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;
using System.Web;

namespace Diamonds.Models.Entities
{
    public partial class News
    {
        public int id { get; set; }
        public string titlePl { get; set; }
        public string titleEn { get; set; }
        public string textPl { get; set; }
        public string textEn { get; set; }
        public DateTime addDate { get; set; }
        public int userId { get; set; }
        public Nullable<int> photoId { get; set; }
        public bool isPublished { get; set; }

        public string title { get { return lang(titlePl, titleEn); } }
        public string text { get { return lang(textPl, textEn); } }

        public virtual User User { get; set; }
        public virtual Photo Photo { get; set; }

        public News() { }

        private string lang(string pl, string en) {
            if (HttpContext.Current.Response.Cookies["lang"].Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }
    }

    public class NewsMapping : EntityTypeConfiguration<News>
    {
        public NewsMapping()
            : base()
        {
            this.HasRequired(e => e.User).WithMany(e => e.News).HasForeignKey(e => e.userId);
            this.HasOptional(e => e.Photo).WithMany(e => e.News).HasForeignKey(e => e.photoId);
        }
    }
}