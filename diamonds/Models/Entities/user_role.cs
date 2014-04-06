using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class user_role
    {
        public int userId { get; set; }
        public short roleId { get; set; }
        public bool value { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    public class user_roleMapping : EntityTypeConfiguration<user_role>
    {
        public user_roleMapping()
            : base()
        {
            this.HasKey(e => new { e.roleId, e.userId });

            this.HasRequired(e => e.User).WithMany(e => e.user_role).HasForeignKey(e => e.userId);
            this.HasRequired(e => e.Role).WithMany(e => e.user_role).HasForeignKey(e => e.roleId);
        }
    }
}