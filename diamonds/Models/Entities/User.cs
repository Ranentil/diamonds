using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public DateTime createDate { get; set; }
        public DateTime lastLoginDate { get; set; }
        public short roleGroupId { get; set; }
        public bool isConfirmed { get; set; }

        public virtual RoleGroup RoleGroup { get; set; }
        public virtual ICollection<user_role> user_role { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public User() { }

        public User(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.setPassword(password);
            this.lastLoginDate = DateTime.Parse("1900-01-01 00:00:00");
            this.createDate = DateTime.Now;
            this.roleGroupId = 1;
            this.isConfirmed = false;
        }

        public void setPassword(string password)
        {
            this.password = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(9));
        }

        public bool checkPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, this.password);
            }
            catch (BCrypt.Net.SaltParseException e) { }

            return false;
        }

        public bool checkEmail(string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(this.createDate + this.name, hash);
            }
            catch (BCrypt.Net.SaltParseException e) { }

            return false;
        }

        public bool hasAccess(string code)
        {
            user_role ur = this.user_role.SingleOrDefault(r => r.Role.code == code);

            if (ur != null)
            {
                if (ur.value)
                    return true;
                else
                    return false;
            }

            RoleGroup rolegroup = this.RoleGroup;

            rolegroup_role rgr = rolegroup.rolegroup_role.SingleOrDefault(r => r.Role.code == code);
            if (rgr != null)
            {
                if (rgr.value)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }

    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
            : base()
        {
            this.HasRequired(e => e.RoleGroup).WithMany(e => e.Users).HasForeignKey(e => e.roleGroupId);

            this.HasMany(e => e.News).WithRequired(e => e.User).HasForeignKey(e => e.userId);
            this.HasMany(e => e.Photos).WithRequired(e => e.User).HasForeignKey(e => e.userId);
            this.HasMany(e => e.Comments).WithRequired(e => e.User).HasForeignKey(e => e.userId);
        }
    }
}