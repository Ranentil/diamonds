using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Comment
    {
        public int id { get; set; }
        public string text { get; set; }
        public DateTime addDate { get; set; }
        public int userId { get; set; }

        public virtual User User { get; set; }
    }

    public class CommentMapping : EntityTypeConfiguration<Comment>
    {
        public CommentMapping()
            : base()
        {
            this.HasRequired(e => e.User).WithMany(e => e.Comments).HasForeignKey(e => e.userId);
        }
    }
}