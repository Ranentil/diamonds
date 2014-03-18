using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class NewsController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /News/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /News/Create

        public ViewResult Create()
        {
            return View("Edit", new News());
        }

        //
        // POST: /News/Create/

        [HttpPost]
        public ActionResult Create(News news)
        {
            if (ModelState.IsValid)
            {
                news.User = db.Users.Single(u => u.email == User.Identity.Name);
                news.addDate = DateTime.Now;

                db.News.Add(news);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano artykuł.";
                return RedirectToAction("Edit", new { id = news.id });
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano artykułu.";
            return View("Edit", news);
        }

        //
        // GET: /News/Edit/

        public ViewResult Edit(int id)
        {
            News news = db.News.Single(n => n.id == id);
            return View(news);
        }

        //
        // POST: /News/Edit:/

        [HttpPost]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                db.News.Attach(news);
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano artykuł.";
                return RedirectToAction("Edit", new { id = news.id });
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano zmian.";
            return View(news);
        }

    }
}
