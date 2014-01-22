using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class News
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public DateTime addDate { get; set; }
        public int userId { get; set; }

        public User User { get; set; }
    }

    public class NewsMapping : EntityTypeConfiguration<News>
    {
        public NewsMapping()
            : base()
        {
            this.HasRequired(e => e.User).WithMany(e => e.News).HasForeignKey(e => e.userId);
        }
    }
}