using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Diamonds.Models.Entities
{
    public partial class DiamondsEntities : DbContext
    {
        public DiamondsEntities()
            : base("name=DiamondsEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new GalleryMapping());
            modelBuilder.Configurations.Add(new NewsMapping());
            modelBuilder.Configurations.Add(new PhotoMapping());
            modelBuilder.Configurations.Add(new PlayerMapping());
            modelBuilder.Configurations.Add(new PositionMapping());
            modelBuilder.Configurations.Add(new RoleMapping());
            modelBuilder.Configurations.Add(new UserMapping());
        }

        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}