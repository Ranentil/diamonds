using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Photo
    {
        public int id { get; set; }
        public int galleryId { get; set; }
        public int userId { get; set; }
        public string description { get; set; }

        public Gallery Gallery { get; set; }
        public User User { get; set; }
    }

    public class PhotoMapping : EntityTypeConfiguration<Photo>
    {
        public PhotoMapping()
            : base()
        {
            this.HasRequired(e => e.Gallery).WithMany(e => e.Photos).HasForeignKey(e => e.galleryId);
            this.HasRequired(e => e.User).WithMany(e => e.Photos).HasForeignKey(e => e.userId);
        }
    }
}