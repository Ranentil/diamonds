using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class EventType
    {
        public byte id { get; set; }
        public string namePl { get; set; }
        public string nameEn { get; set; }

        public string name { get { return lang(namePl, nameEn); } }

        public virtual ICollection<Event> Events { get; set; }

        private string lang(string pl, string en)
        {
            var cookie = HttpContext.Current.Request.Cookies["diamonds-lang"];
            if (cookie != null && cookie.Value == "en" || pl == "")
                if (en != "")
                    return en;
            return pl;
        }
    }

    public class EventTypeMapping : EntityTypeConfiguration<EventType>
    {
        public EventTypeMapping()
            : base()
        {
            this.HasMany(e => e.Events).WithRequired(e => e.EventType).HasForeignKey(e => e.eventTypeId);
        }
    }
}