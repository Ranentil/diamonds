using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class News
    {
        public int id { get; set; }
        public int titleLid { get; set; }
        public int textLid { get; set; }
        public DateTime addDate { get; set; }
        public int userId { get; set; }
        public Nullable<int> photoId { get; set; }
        public bool isPublished { get; set; }

        public string title { get { return Title.pl; } }
        public string text { get { return Text.pl; } }

        public virtual User User { get; set; }
        public virtual Localization Title { get; set; }
        public virtual Localization Text { get; set; }
        public virtual Photo Photo { get; set; }

        public News() { }
    }

    public class NewsMapping : EntityTypeConfiguration<News>
    {
        public NewsMapping()
            : base()
        {
            this.HasRequired(e => e.User).WithMany(e => e.News).HasForeignKey(e => e.userId);
            this.HasOptional(e => e.Photo).WithMany(e => e.News).HasForeignKey(e => e.photoId);
            //this.HasRequired(e => e.Title).WithMany(e => e.NewsTitles).HasForeignKey(e => e.titleLid);
            //this.HasRequired(e => e.Text).WithMany(e => e.NewsTexts).HasForeignKey(e => e.textLid);
        }
    }
}