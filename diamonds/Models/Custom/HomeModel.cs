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
        public ICollection<Gallery> Galleries;
        public Photo FeaturedPhoto;

        public HomeModel() { }
    }
}