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
        public bool isFromBase { get; set; }

        public virtual ICollection<News> NewsTitles { get; set; }
        public virtual ICollection<News> NewsTexts { get; set; }
        public virtual ICollection<Event> EventNames { get; set; }
        public virtual ICollection<Event> EventPlaces { get; set; }
        public virtual ICollection<Gallery> GalleryNames { get; set; }
        public virtual ICollection<Gallery> GalleryPlaces { get; set; }
        public virtual ICollection<Position> PositionPlayer { get; set; }
        public virtual ICollection<Photo> PhotoDescription { get; set; }
        
    }

    public class LocalizationMapping : EntityTypeConfiguration<Localization>
    {
        public LocalizationMapping()
            : base()
        {
            this.HasMany(e => e.GalleryNames).WithRequired(e => e.Name).HasForeignKey(e => e.nameLid);
            this.HasMany(e => e.GalleryPlaces).WithRequired(e => e.Place).HasForeignKey(e => e.placeLid);
            this.HasMany(e => e.NewsTitles).WithRequired(e => e.Title).HasForeignKey(e => e.titleLid);
            this.HasMany(e => e.NewsTexts).WithRequired(e => e.Text).HasForeignKey(e => e.textLid);
            this.HasMany(e => e.EventNames).WithRequired(e => e.Name).HasForeignKey(e => e.nameLid);
            this.HasMany(e => e.EventPlaces).WithRequired(e => e.Place).HasForeignKey(e => e.placeLid);
            this.HasMany(e => e.PositionPlayer).WithRequired(e => e.Player).HasForeignKey(e => e.playerLid);
            this.HasMany(e => e.PhotoDescription).WithRequired(e => e.Description).HasForeignKey(e => e.descriptionLid);
        }
    }
}