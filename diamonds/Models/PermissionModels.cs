using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diamonds.Models.Entities;

namespace Diamonds.Models.PermissionModels
{
    public class RoleAccess
    {
        public Group Group { get; set; }
        public bool value { get; set; }

        public rolegroup_role ToRoleGroupRole()
        {
            rolegroup_role rgr = new rolegroup_role();
            rgr.roleGroupId = Group.id;
            rgr.value = value;
            return rgr;
        }
    }

    public class GroupAccess
    {
        public int roleId { get; set; }
        public int value { get; set; }

        public bool isAccess { get { return value == 1; } }
        public bool isDenied { get { return value == 0; } }
        public bool isDefault { get { return value == 2; } }
    }

    public class RoleGroupAccess
    {
        public Role Role { get; set; }
        public RoleGroup RoleGroup { get; set; }
        public int value { get; set; }

        public bool isAccess { get { return value == 1; } }
        public bool isDenied { get { return value == 0; } }
        public bool isDefault { get { return value == 2; } }
    }

    public class Group
    {
        public short id { get; set; }
        public string name { get; set; }

        public Group() { }
        public Group(Entities.RoleGroup roleGroup)
        {
            id = roleGroup.id;
            name = roleGroup.name;
        }
    }
}