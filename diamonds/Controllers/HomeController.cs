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
            homeModel.News = db.News.Where(n => n.isPublished).OrderByDescending(n => n.addDate).ToList();
            homeModel.Events = db.Events.Where(d => d.startDate > DateTime.Now).ToList();
            return View(homeModel);
        }
        
        public ActionResult Contact()
        {
            return View();
        }
    }
}
