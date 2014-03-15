using System;
using System.Collections.Generic;
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
            return View();
        }

        //
        // POST: /News/Create/

        [HttpPost]
        public ActionResult Create(News news, HttpPostedFileBase fileJPG)
        {
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                db.SaveChanges();

                if (fileJPG != null)
                {
                    Photo photo = new Photo(fileJPG, 0);
                    photo.User = db.Users.Single(u => u.email == User.Identity.Name);
                }

                return RedirectToAction("Index");
            }
            return View("Edit", news);
        }

        //
        // GET: /News/Edit/

        public ViewResult Edit()
        {
            return View();
        }

        //
        // POST: /News/Edit:/

        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(news);
        }

    }
}
