using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class GalleryController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Gallery/

        public ActionResult Index()
        {
            List<Gallery> galleries = db.Galleries.Where(g => g.isPublished).OrderBy(g => g.startDate).ToList();
            return View(galleries); 
        }

        //
        // GET: /Gallery/Photos/galeryId

        public ViewResult Photos(int id)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);
            return View(gallery);
        }

        //
        // GET: /Gallery/Create

        public ViewResult Create()
        {
            return View("Edit", new Gallery());
        }

        //
        // POST: /Gallery/Create/

        [HttpPost]
        public ActionResult Create(Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Galleries.Add(gallery);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano galerię.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano galerii.";
            return View("Edit", gallery);
        }

        //
        // GET: /Gallery/Edit/

        public ViewResult Edit(int id)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);
            return View(gallery);
        }

        //
        // POST: /Gallery/Edit:/

        [HttpPost]
        public ActionResult Edit(Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Galleries.Attach(gallery);
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano galerię.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano galerii.";
            return View(gallery);
        }

        //
        // GET: /Gallery/Photo/

        public FileResult Photo(int id)
        {
            Photo photo = db.Photos.Single(p => p.id == id);
            return File(photo.jpgPath, "image/jpeg");
        }

        //
        // GET: /Gallery/PhotoEdit/

        public ViewResult PhotosEdit(int id)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);
            return View(gallery);
        }

        //
        // POST: /Gallery/Edit:/

        [HttpPost]
        public ActionResult PhotosEdit(int id, IEnumerable<Photo> photos)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);

            if (ModelState.IsValid)
            {
                
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano zdjęcie.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano zdjęcia.";
            return View(gallery);
        }

    }
}
