﻿using System;
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

        public virtual Photo Photo { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Lineup> Lineups { get; set; }
        public virtual ICollection<Action> BattingPlayers { get; set; }

        public Player() { }
    }

    public class PlayerMapping : EntityTypeConfiguration<Player>
    {
        public PlayerMapping()
            : base()
        {
            this.HasOptional(e => e.Photo).WithMany(e => e.Players).HasForeignKey(e => e.photoId);
            this.HasMany(e => e.BattingPlayers).WithRequired(e => e.BattingPlayer).HasForeignKey(e => e.battingPlayerId);

            this.HasMany(e => e.Positions).WithMany(e => e.Players).
                Map(e => e.MapLeftKey("playerId").MapRightKey("positionId").ToTable("players_positions"));
        }
    }
}