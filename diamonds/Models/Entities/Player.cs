using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public List<Position> Positions { get; set; }
    }

    public class PlayerMapping : EntityTypeConfiguration<Player>
    {
        public PlayerMapping()
            : base()
        {
            
        }
    }
}