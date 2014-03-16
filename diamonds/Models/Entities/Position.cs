using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Position
    {
        public byte id { get; set; }
        public string name { get; set; }
        public int playerLid { get; set; }

        public string player { get { return Player.pl; } }

        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Lineup> Lineups { get; set; }
        public virtual Localization Player { get; set; }
    }

    public class PositionMapping : EntityTypeConfiguration<Position>
    {
        public PositionMapping()
            : base()
        {
            this.HasMany(e => e.Actions).WithOptional(e => e.ToPosition).HasForeignKey(e => e.toPositionId);
            this.HasMany(e => e.Lineups).WithOptional(e => e.Position).HasForeignKey(e => e.positionId);

            this.HasMany(e => e.Players).WithMany(e => e.Positions).
                Map(e => e.MapLeftKey("positionId").MapRightKey("playerId").ToTable("players_positions"));
        }
    }
}