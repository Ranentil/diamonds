using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Team
    {
        public short id { get; set; }
        public string name { get; set; }
        public string clubName { get; set; }
        public string city { get; set; }
        public string fullName { get { return name + " " + city; } }

        public virtual ICollection<Result> ResultsHome { get; set; }
        public virtual ICollection<Result> ResultsGuest { get; set; }


        public Team() { }

        public short won(int year)
        {
            byte c = 0;
            foreach (var item in this.ResultsHome.Where(r => r.eventDate.Year == year))
                if (item.resultHome > item.resultGuest)
                    c++;
            foreach (var item in this.ResultsGuest.Where(r => r.eventDate.Year == year))
                if (item.resultGuest > item.resultHome)
                    c++;
            return c;
        }
        public short lost(int year)
        {
            byte c = 0;
            foreach (var item in this.ResultsHome.Where(r => r.eventDate.Year == year))
                if (item.resultHome < item.resultGuest)
                    c++;
            foreach (var item in this.ResultsGuest.Where(r => r.eventDate.Year == year))
                if (item.resultGuest < item.resultHome)
                    c++;
            return c;
        }


    }
    

    public class TeamMapping : EntityTypeConfiguration<Team>
    {
        public TeamMapping()
            : base()
        {
            this.HasMany(e => e.ResultsHome).WithRequired(e => e.TeamHome).HasForeignKey(e => e.teamHomeId);
            this.HasMany(e => e.ResultsGuest).WithRequired(e => e.TeamGuest).HasForeignKey(e => e.teamGuestId);
        }
    }
}