using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Action
    {
        public int id { get; set; }
        public int battingPlayerId { get; set; }
        public byte balls { get; set; }
        public byte strikes { get; set; }
        public byte batting { get; set; }
        public byte toPositionId { get; set; }
        public byte first { get; set; }
        public byte second { get; set; }
        public byte third { get; set; }
        public byte home { get; set; }

        public virtual Player BattingPlayer { get; set; }
        public virtual Position ToPosition { get; set; }
    }
    
    public class ActionMapping : EntityTypeConfiguration<Action>
    {
        public ActionMapping()
            :base()
        {
            this.HasRequired(e => e.BattingPlayer).WithMany(e => e.BattingPlayers).HasForeignKey(e => e.battingPlayerId);
            this.HasOptional(e => e.ToPosition).WithMany(e => e.Actions).HasForeignKey(e => e.toPositionId);
        }
    }
}