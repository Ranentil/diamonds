using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Lineup
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public string team { get; set; }
        public int playerId { get; set; }
        public byte battingOrder { get; set; }
        public byte positionId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Player Player { get; set; }
        public virtual Position Position { get; set; }
    }

    public class LineupMapping : EntityTypeConfiguration<Lineup>
    {
        public LineupMapping()
            : base()
        {
            this.HasRequired(e => e.Event).WithMany(e => e.Lineups).HasForeignKey(e => e.eventId);
            this.HasRequired(e => e.Player).WithMany(e => e.Lineups).HasForeignKey(e => e.playerId);
            this.HasOptional(e => e.Position).WithMany(e => e.Lineups).HasForeignKey(e => e.positionId);
        }
    }
}