using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class RoleGroup
    {
        public short id { get; set; }
        public string name { get; set; }


        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<rolegroup_role> rolegroup_role { get; set; }

        public RoleGroup()
        {
            this.rolegroup_role = new HashSet<rolegroup_role>();
            this.Users = new HashSet<User>();
        }
    }

    public class RoleGroupMapping : EntityTypeConfiguration<RoleGroup>
    {
        public RoleGroupMapping()
        {
            this.HasMany(e => e.rolegroup_role).WithRequired(e => e.RoleGroup).HasForeignKey(e => e.roleGroupId);
            this.HasMany(e => e.Users).WithRequired(e => e.RoleGroup).HasForeignKey(e => e.roleGroupId);
        }
    }
}