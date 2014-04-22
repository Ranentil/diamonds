using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    [Authorize(Roles = "MODERATOR")]
    public class LocalizationController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        public ActionResult Index()
        {
            return RedirectToAction("Admin");
        }

        public ActionResult Admin()
        {
            List<Localization> localizations = db.Localizations.OrderBy(l => l.code).ToList();
            return View(localizations);
        }

        [Authorize(Roles = "ADMIN")]
        public ViewResult Create()
        {
            return View("Edit", new Localization());
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "ADMIN")]
        public ActionResult Create(Localization localization)
        {
            if (ModelState.IsValid)
            {
                db.Localizations.Add(localization);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano";
                return RedirectToAction("Admin");
            }
            return View("Edit", localization);
        }


        public ViewResult Edit(int id)
        {
            Localization localization = db.Localizations.Single(l => l.id == id);
            return View(localization);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Localization localization)
        {
            if (ModelState.IsValid)
            {
                db.Localizations.Attach(localization);
                db.Entry(localization).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano.";
                return RedirectToAction("Admin");
            }
            return View(localization);
        }


        [Authorize(Roles = "ADMIN")]
        public ActionResult Delete(int id)
        {
            Localization localization = db.Localizations.Single(l => l.id == id);
            db.Localizations.Remove(localization);
            db.SaveChanges();

            return RedirectToAction("Admin");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
