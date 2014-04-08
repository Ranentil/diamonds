using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Gallery
    {
        public int id { get; set; }
        public string namePl { get; set; }
        public string nameEn { get; set; }
        public string placePl { get; set; }
        public string placeEn { get; set; }
        public DateTime startDate { get; set; }
        public Nullable<DateTime> endDate { get; set; }
        public Nullable<int> coverId { get; set; }
        public bool isPublished { get; set; }

        public string name { get { return lang(namePl, nameEn); } }
        public string place { get { return lang(placePl, placeEn); } }

        public virtual Photo Cover { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

        public Gallery()
        {
            startDate = DateTime.Today;
        }

        private string lang(string pl, string en)
        {
            var cookie = HttpContext.Current.Request.Cookies["diamonds-lang"];
            if (cookie != null && cookie.Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }
    }

    public class GalleryMapping : EntityTypeConfiguration<Gallery>
    {
        public GalleryMapping()
            : base()
        {
            this.HasOptional(e => e.Cover).WithMany(e => e.Galleries).HasForeignKey(e => e.coverId);
            this.HasMany(e => e.Photos).WithRequired(e => e.Gallery).HasForeignKey(e => e.galleryId);
        }
    }
}