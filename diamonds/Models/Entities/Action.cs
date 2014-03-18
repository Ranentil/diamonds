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
        public Nullable<byte> balls { get; set; }
        public Nullable<byte> strikes { get; set; }
        public Nullable<byte> batting { get; set; }
        public Nullable<byte> toPositionId { get; set; }
        public Nullable<byte> first { get; set; }
        public Nullable<byte> second { get; set; }
        public Nullable<byte> third { get; set; }
        public Nullable<byte> home { get; set; }

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