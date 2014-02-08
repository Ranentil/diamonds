using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Event
    {
        public int id { get; set; }
        public string name { get; set; }
        public string place { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool repetitive { get; set; }
        public byte type { get; set; }
        public byte status { get; set; }

        public virtual ICollection<Lineup> Lineups { get; set; }
    }

    public class EventMapping : EntityTypeConfiguration<Event>
    {
        public EventMapping()
            : base()
        {
            this.HasMany(e => e.Lineups).WithRequired(e => e.Event).HasForeignKey(e => e.eventId);
        }
    }
}