using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class HomeController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        public ActionResult Index()
        {
            HomeModel homeModel = new HomeModel();
            homeModel.FeaturedNews = db.News.Where(n => n.isPublished).OrderByDescending(n => n.addDate).First();
            homeModel.News = db.News.Where(n => n.isPublished).OrderByDescending(n => n.addDate).Skip(1).Take(5).ToList();
            homeModel.Events = db.Events.Where(d => d.startDate > DateTime.Now).ToList();
            homeModel.Galleries = db.Galleries.Where(g => g.isPublished && g.coverId != null).OrderByDescending(g => g.startDate).Take(2).ToList();
            var random = new Random();
            var photos = db.Photos.ToList();
            homeModel.FeaturedPhoto = db.Photos.OrderBy(p => Guid.NewGuid()).First();

            return View(homeModel);
        }
        
        public ActionResult Contact()
        {
            return View();
        }
    }
}
