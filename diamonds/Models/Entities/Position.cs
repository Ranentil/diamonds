using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Position
    {
        public int playerId { get; set; }
        public int positionId { get; set; }

        public Player Player { get; set; }

        public string name
        {
            get
            {
                switch (this.positionId)
                {
                    case 1: return "Pitcher";
                    case 2: return "Catcher";
                    case 3: return "Pierwsza baza";
                    case 4: return "Druga baza";
                    case 5: return "Trzecia baza";
                    case 6: return "Short Stop";
                    case 7: return "Lewe zapole";
                    case 8: return "Środkowe zapole";
                    case 9: return "Prawe zapole";
                    default: return "Zapole";
                }
            }
        }

        public string abbreviation
        {
            get
            {
                switch (this.positionId)
                {
                    case 1: return "P";
                    case 2: return "C";
                    case 3: return "1B";
                    case 4: return "2B";
                    case 5: return "3B";
                    case 6: return "SS";
                    case 7: return "LF";
                    case 8: return "CF";
                    case 9: return "RF";
                    default: return "OF";
                }
            }
        }
    }

    public class PositionMapping : EntityTypeConfiguration<Position>
    {
        public PositionMapping()
            : base()
        {
            this.HasRequired(e => e.Player).WithMany(e => e.Positions).HasForeignKey(e => e.playerId);
        }
    }
}