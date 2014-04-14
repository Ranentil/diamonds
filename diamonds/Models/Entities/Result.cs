using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace Diamonds.Models.Entities
{
    public partial class Result
    {
        public int id { get; set; }
        public short teamHomeId { get; set; }
        public short teamGuestId { get; set; }
        public byte resultHome { get; set; }
        public byte resultGuest { get; set; }
        public DateTime eventDate { get; set; }

        public virtual Team TeamHome { get; set; }
        public virtual Team TeamGuest { get; set; }

        public Result()
        {
            this.eventDate = DateTime.Today;
        }
    }


    public class ResultMapping : EntityTypeConfiguration<Result>
    {
        public ResultMapping()
            : base()
        {
            this.HasRequired(e => e.TeamHome).WithMany(e => e.ResultsHome).HasForeignKey(e => e.teamHomeId);
            this.HasRequired(e => e.TeamGuest).WithMany(e => e.ResultsGuest).HasForeignKey(e => e.teamGuestId);
        }
    }
}