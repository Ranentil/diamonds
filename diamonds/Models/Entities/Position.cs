using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Position
    {
        public byte id { get; set; }
        public string name { get; set; }
        public string playerPl { get; set; }
        public string playerEn { get; set; }

        public string player { get { return lang(playerPl, playerEn); } }

        public virtual ICollection<Player> Players { get; set; }


        private string lang(string pl, string en)
        {
            var cookie = HttpContext.Current.Request.Cookies["diamonds-lang"];
            if (cookie != null && cookie.Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }
    }

    public class PositionMapping : EntityTypeConfiguration<Position>
    {
        public PositionMapping()
            : base()
        {
            this.HasMany(e => e.Players).WithMany(e => e.Positions).
                Map(e => e.MapLeftKey("playerId").MapRightKey("positionId").ToTable("players_positions"));
        }
    }
}