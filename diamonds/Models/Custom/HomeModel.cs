using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diamonds.Models.Entities;

namespace Diamonds.Models
{
    public class HomeModel
    {
        public News FeaturedNews;
        public ICollection<News> News;
        public ICollection<Event> Events;
        public ICollection<EventClone> EventClones
        {
            get
            {
                List<EventClone> items = new List<EventClone>();
                foreach (var e in Events)
                    items.Add(e.Clone());
                return items;
            }
        }
        public ICollection<Gallery> Galleries;
        public Photo FeaturedPhoto;

        public HomeModel() { }
    }

    
}