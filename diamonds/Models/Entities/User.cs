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
        public string login { get; set; }
        public string displayName { get; set; }
        public DateTime createDate { get; set; }
        public DateTime lastLoginDate { get; set; }
        public int roleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        private const string prefixSalt = "3Yq(xsliMzZc<Ed]-A/I4)X471`}5~rV$y()p)FM*K)d4- ptx&BKjnwq][|Noba";
        private const string suffixSalt = "r*iKDO^Jlf1ig|h&sE|8c`P=Hj#*O~%LYw#a(B#FBO^gub5-@+UKwz@>>IOX2wwv";
        private const double DateSalt = 54.7896;

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
    }

    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
            : base()
        {
            this.HasRequired(e => e.Role).WithMany(e => e.Users).HasForeignKey(e => e.roleId);

            this.HasMany(e => e.News).WithRequired(e => e.User).HasForeignKey(e => e.userId);
            this.HasMany(e => e.Photos).WithRequired(e => e.User).HasForeignKey(e => e.userId);
            this.HasMany(e => e.Comments).WithRequired(e => e.User).HasForeignKey(e => e.userId);
        }
    }
}