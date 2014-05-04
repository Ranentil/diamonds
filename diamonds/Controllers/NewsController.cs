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
    public class NewsController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /News/

        [AllowAnonymous]
        public ViewResult Index()
        {
            List<News> news = db.News.Where(n => n.isPublished).OrderByDescending(n => n.addDate).ToList();
            return View(news);
        }

        //
        // GET: /News/Details/id

        [AllowAnonymous]
        public ViewResult Details(int id)
        {
            News news = db.News.Single(n => n.id == id);
            return View(news);
        }

        //
        // GET: /News/Admin

        public ViewResult Admin()
        {
            List<News> news = db.News.Where(n => n.isPublished).OrderByDescending(n => n.addDate).ToList();
            return View(news);
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
        [ValidateInput(false)]
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
        [ValidateInput(false)]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                db.News.Attach(news);
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano artykuł.";
                return RedirectToAction("Admin");
            }
            return View(news);
        }

        public ActionResult Delete(int id)
        {
            News news = db.News.Single(n => n.id == id);
            db.News.Remove(news);
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
