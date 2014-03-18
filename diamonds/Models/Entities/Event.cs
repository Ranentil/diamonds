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
        public byte type { get; set; }
        public byte status { get; set; }

        public string name { get { return lang(namePl, nameEn); } }
        public string place { get { return lang(placePl, placeEn); } }

        public virtual ICollection<Lineup> Lineups { get; set; }


        private string lang(string pl, string en)
        {
            if (HttpContext.Current.Response.Cookies["lang"].Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }
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