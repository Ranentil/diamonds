using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Role
    {
        // Admin Moderator Diament Photographer User
        public short id { get; set; }
        public string name { get; set; }
        public string code { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public class RoleMapping : EntityTypeConfiguration<Role>
    {
        public RoleMapping()
            : base()
        {
            this.HasMany(e => e.Users).WithRequired(e => e.Role).HasForeignKey(e => e.roleId);
        }
    }
}