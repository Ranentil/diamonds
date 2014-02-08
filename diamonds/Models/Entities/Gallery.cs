using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Gallery
    {
        public int id { get; set; }
        public string name { get; set; }
        public string place { get; set; }
        public DateTime startDate { get; set; }
        public Nullable<DateTime> endDate { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }

    public class GalleryMapping : EntityTypeConfiguration<Gallery>
    {
        public GalleryMapping()
            : base()
        {
            this.HasMany(e => e.Photos).WithRequired(e => e.Gallery).HasForeignKey(e => e.galleryId);
        }
    }
}