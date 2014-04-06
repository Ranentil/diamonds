using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Role
    {
        public short roleId { get; set; }
        public string name { get; set; }
        public string code { get; set; }

        public virtual ICollection<rolegroup_role> rolegroup_role { get; set; }
        public virtual ICollection<user_role> user_role { get; set; }

        public Role()
        {
            this.rolegroup_role = new HashSet<rolegroup_role>();
            this.user_role = new HashSet<user_role>();
        }
    }

    public class RoleMapping : EntityTypeConfiguration<Role>
    {
        public RoleMapping()
            : base()
        {
            this.HasMany(e => e.rolegroup_role).WithRequired(e => e.Role).HasForeignKey(e => e.roleId);
            this.HasMany(e => e.user_role).WithRequired(e => e.Role).HasForeignKey(e => e.roleId);
        }
    }
}