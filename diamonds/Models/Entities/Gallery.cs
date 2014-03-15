using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Gallery
    {
        public int id { get; set; }
        public int nameLid { get; set; }
        public int placeLid { get; set; }
        
        public DateTime startDate { get; set; }
        public Nullable<DateTime> endDate { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        public virtual Localization Name { get; set; }
        public virtual Localization Place { get; set; }
    }

    public class GalleryMapping : EntityTypeConfiguration<Gallery>
    {
        public GalleryMapping()
            : base()
        {
            this.HasMany(e => e.Photos).WithRequired(e => e.Gallery).HasForeignKey(e => e.galleryId);
            //this.HasRequired(e => e.Name).WithMany(e => e.GalleryNames).HasForeignKey(e => e.nameLid);
            //this.HasRequired(e => e.Place).WithMany(e => e.GalleryPlaces).HasForeignKey(e => e.placeLid);
        }
    }
}