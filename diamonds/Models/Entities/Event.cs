using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Event
    {
        public int id { get; set; }
        public int nameLid { get; set; }
        public int placeLid { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool repetitive { get; set; }
        public byte type { get; set; }
        public byte status { get; set; }

        public string name { get { return Name.pl; } }
        public string place { get { return Place.pl; } }

        public virtual ICollection<Lineup> Lineups { get; set; }
        public virtual Localization Name { get; set; }
        public virtual Localization Place { get; set; }
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