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
            homeModel.Events = db.Events.ToList();
            homeModel.Galleries = db.Galleries.Where(g => g.isPublished && g.photoId != null).OrderByDescending(g => g.startDate).Take(2).ToList();
            var random = new Random();
            var photos = db.Photos.ToList();
            homeModel.FeaturedPhoto = db.Photos.Any(p => p.Gallery.isPublished) ? db.Photos.Where(p => p.Gallery.isPublished).OrderBy(p => Guid.NewGuid()).First() : null;
            homeModel.Team = db.Teams.Single(t => t.id == 1);

            return View(homeModel);
        }

        public ActionResult Contact()
        {
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
