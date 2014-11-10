using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Player
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nickName { get; set; }
        public short number { get; set; }
        public bool isActive { get; set; }
        public int? photoId { get; set; }
        public bool throwing { get; set; }
        public bool? batting { get; set; }

        public virtual Photo Photo { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Photo> Tags { get; set; }

        public Player() 
        {
            Positions = new HashSet<Position>();
            isActive = true;
            throwing = true;
            batting = true;
        }
    }

    public class PlayerMapping : EntityTypeConfiguration<Player>
    {
        public PlayerMapping()
            : base()
        {
            this.HasOptional(e => e.Photo).WithMany(e => e.Players).HasForeignKey(e => e.photoId);

            this.HasMany(e => e.Positions).WithMany(e => e.Players).
                Map(e => e.MapLeftKey("positionId").MapRightKey("playerId").ToTable("players_positions"));
            this.HasMany(e => e.Tags).WithMany(e => e.Tags).
                Map(e => e.MapLeftKey("photoId").MapRightKey("playerId").ToTable("players_photos"));
        }
    }
}