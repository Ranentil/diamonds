using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Event
    {
        public int id { get; set; }
        public string namePl { get; set; }
        public string nameEn { get; set; }
        public string placePl { get; set; }
        public string placeEn { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool repetitive { get; set; }
        public byte eventTypeId { get; set; }
        public byte status { get; set; } 

        public string name { get { return lang(namePl, nameEn); } }
        public string place { get { return lang(placePl, placeEn); } }

        public virtual EventType EventType { get; set; }
        public virtual ICollection<Lineup> Lineups { get; set; }


        public Event() 
        {
            this.startDate = DateTime.Now;
            this.endDate = DateTime.Now;
        }

        private string lang(string pl, string en)
        {
            var cookie = HttpContext.Current.Request.Cookies["diamonds-lang"];
            if (cookie != null && cookie.Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }

        public EventClone Clone()
        {
            EventClone item = new EventClone();
            item.name = this.name;
            item.place = this.place;
            item.repetitive = this.repetitive;
            item.type = this.EventType.name;
            item.status = this.status;
            item.startDate = this.startDate;
            item.endDate = this.endDate;

            return item;
        }
    }

    public class EventClone
    {
        public string name;
        public string place;
        public bool repetitive;
        public string type;
        public byte status;
        public DateTime startDate;
        public DateTime endDate;
    }

    public class EventMapping : EntityTypeConfiguration<Event>
    {
        public EventMapping()
            : base()
        {
            this.HasRequired(e => e.EventType).WithMany(e => e.Events).HasForeignKey(e => e.eventTypeId);
            this.HasMany(e => e.Lineups).WithRequired(e => e.Event).HasForeignKey(e => e.eventId);
        }
    }
}