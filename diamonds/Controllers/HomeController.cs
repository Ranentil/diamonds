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
            homeModel.Galleries = db.Galleries.Where(g => g.isPublished).OrderByDescending(g => g.startDate).Take(3).ToList();
            var random = new Random();
            var photos = db.Photos.ToList();
            homeModel.FeaturedPhoto = db.Photos.OrderBy(p => Guid.NewGuid()).First();

            return View(homeModel);
        }
        
        public ActionResult Contact()
        {
            return View();
        }


        public ViewResult Localizations()
        {
            List<Localization> localizations = db.Localizations.OrderBy(l => l.name).ToList();
            return View(localizations);
        }

        public ViewResult LocalizationCreate()
        {
            return View("LocalizationEdit", new Localization());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LocalizationCreate(Localization localization)
        {
            if (ModelState.IsValid)
            {
                db.Localizations.Add(localization);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano";
                return RedirectToAction("Localizations");
            }
            return View("LocalizationEdit", localization);
        }

        public ViewResult LocalizationEdit(int id)
        {
            Localization localization = db.Localizations.Single(l => l.id == id);
            return View(localization);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LocalizationEdit(Localization localization)
        {
            if (ModelState.IsValid)
            {
                db.Localizations.Attach(localization);
                db.Entry(localization).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano.";
                return RedirectToAction("Localizations", new { id = localization.id });
            }
            return View(localization);
        }
    }
}
