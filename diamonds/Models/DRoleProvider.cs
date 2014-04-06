using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Diamonds.Models.Entities;

namespace Diamonds.Models
{
    public class DRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            DiamondsEntities db = new DiamondsEntities();
            User user = db.Users.FirstOrDefault(u => u.email == username);

            if (user == null)
                return false;

            return user.hasAccess(roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            if (username == null)
                throw new ArgumentNullException("username");
            if (username == "")
                throw new ArgumentException("An argument can't be empty", "username");

            DiamondsEntities db = new DiamondsEntities();
            List<string> roles = new List<string>();

            User user = db.Users.FirstOrDefault(u => u.email == username);

            if (user == null)
                return roles.ToArray();

            RoleGroup roleGroup = user.RoleGroup;
            
            foreach (rolegroup_role rgr in roleGroup.rolegroup_role)
                if (rgr.value) roles.Add(rgr.Role.code);
                else roles.Remove(rgr.Role.code);

            foreach (user_role ur in user.user_role)
                if (ur.value) roles.Add(ur.Role.code);
                else roles.Remove(ur.Role.code);

            return roles.ToArray();
        }

        #region Methods and properties not implemented yet

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}