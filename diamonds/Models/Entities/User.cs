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

        public virtual RoleGroup RoleGroup { get; set; }
        public virtual ICollection<user_role> user_role { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        private const string prefixSalt = "3Yq(xsli7zRc<Ed]-A/I4)Y471`}5~rV$y()p)NM*K)d4- ptx&BKjnwq][|N5ba";
        private const string suffixSalt = "0*iKDO^J4f1ng|h&sE|8c`P=Kj#*O~%LYw#a(B#FBO^gu85-@+UKwz@>>IOX2wwv";
        private const double DateSalt = 54.7896;

        public User() { }

        public User(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.setPassword(password);
            this.lastLoginDate = DateTime.Parse("1900-01-01 00:00:00");
            this.createDate = DateTime.Now;
            this.roleGroupId = 1;
        }

        public void setPassword(string password)
        {
            this.password = BCrypt.Net.BCrypt.HashPassword(prefixSalt + password + suffixSalt, BCrypt.Net.BCrypt.GenerateSalt(9));
        }

        public bool checkPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(prefixSalt + password + suffixSalt, this.password);
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