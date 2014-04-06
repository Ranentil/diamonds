using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class rolegroup_role
    {
        public short roleGroupId { get; set; }
        public short roleId { get; set; }
        public bool value { get; set; }

        public virtual Role Role { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }
    }

    public class rolegroup_roleMapping : EntityTypeConfiguration<rolegroup_role>
    {
        public rolegroup_roleMapping()
        {
            this.HasKey(e => new { e.roleGroupId, e.roleId });

            this.HasRequired(e => e.Role).WithMany(e => e.rolegroup_role).HasForeignKey(e => e.roleId);
            this.HasRequired(e => e.RoleGroup).WithMany(e => e.rolegroup_role).HasForeignKey(e => e.roleGroupId);
        }
    }
}