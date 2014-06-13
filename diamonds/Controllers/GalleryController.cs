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
    public class GalleryController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Gallery/

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Gallery> galleries = db.Galleries.Where(g => g.isPublished && g.Photo != null)
                .OrderByDescending(g => g.startDate).ToList();
            return View(galleries); 
        }

        //
        // GET: /Gallery/Photos/galleryId

        [AllowAnonymous]
        public ViewResult Photos(int id)
        {
            List<Photo> photos = db.Photos.Where(p => p.galleryId == id).OrderByDescending(p => p.no).ToList();
            return View(photos);
        }


        //
        // GET: /Gallery/Photo/photoId

        [AllowAnonymous]
        public FileResult Photo(int id, string size)
        {
            Photo photo = db.Photos.Single(p => p.id == id);
            string path = size == "small" ? photo.thumbPath : size == "big" ? photo.originalPath : photo.photoPath;
            return File(path, "image/jpeg");
        }

        //
        // GET: /Gallery/Admin

        public ActionResult Admin()
        {
            List<Gallery> galleries = db.Galleries.OrderByDescending(g => g.startDate).ToList();
            return View(galleries);
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
                return RedirectToAction("PhotosAdmin", new { id = gallery.id });
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
                return RedirectToAction("Admin");
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano galerii.";
            return View(gallery);
        }

        //
        // GET: /Gallery/PhotosAdmin/galleryId

        public ViewResult PhotosAdmin(int id)
        {
            List<Photo> photos = db.Photos.Where(g => g.galleryId == id).OrderByDescending(p => p.no).ToList();
            return View(photos);
        }

        [HttpPost]
        public ActionResult PhotosAdmin(int id, IEnumerable<Photo> photos)
        {
            foreach (var photo in photos)
            {
                Photo dbPhoto = db.Photos.Single(p => p.id == photo.id);
                dbPhoto.no = photo.no;
                db.SaveChanges();

                TempData["Message"] = "Zapisano kolejność";
                return RedirectToAction("PhotosAdmin", new { id = id });
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano galerii.";
            return RedirectToAction("PhotosAdmin", new { id = id });
        }

        //
        // GET: /Gallery/AddPhotos/galleryId

        public ViewResult AddPhotos(int id)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);
            return View(gallery);
        }

        //
        // POST: /Gallery/AddPhotos/

        [HttpPost]
        public ActionResult AddPhotos(int id, IEnumerable<HttpPostedFileBase> photos, bool? multi)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);
            foreach (var file in photos)
            {
                User user = db.Users.Single(u => u.email == User.Identity.Name);
                Photo photo = new Photo(id, user.id, file);
                if (db.Photos.Any(p => p.galleryId == id))
                    photo.no = (short)db.Photos.Where(p => p.galleryId == id).Max(p => p.no + 1);
                gallery.Photos.Add(photo);
                db.SaveChanges();
                photo.saveJpg(file);
            }
            TempData["Message"] = "Zapisano";
            ViewBag.Multi = multi ?? true;
            return View(gallery);
        }

        public ViewResult Select(int id)
        {
            List<Photo> photos = db.Photos.Where(p => p.galleryId == id)
                .OrderByDescending(p => p.no).ToList();
            return View(photos);
        }

        //
        // GET: /Gallery/PhotoEdit/galleryId

        public ViewResult PhotosEdit(int id)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);
            return View(gallery);
        }

        //
        // POST: /Gallery/Edit/

        [HttpPost]
        public ActionResult PhotosEdit(int id, IEnumerable<Photo> photos)
        {
            Gallery gallery = db.Galleries.Single(g => g.id == id);

            if (ModelState.IsValid)
            {
                
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano zdjęcia.";
                return RedirectToAction("Admin");
            }
            TempData["Error"] = "Coś poszło nie tak! Nie zapisano zdjęcia.";
            return View(gallery);
        }

        //
        // GET: /Gallery/PhotoEdit/galleryId

        public ViewResult PhotoEdit(int id)
        {
            Photo photo = db.Photos.Single(g => g.id == id);
            return View(photo);
        }

        //
        // POST: /Gallery/Edit/

        [HttpPost]
        public ActionResult PhotoEdit()
        {

            TempData["Error"] = "Coś poszło nie tak! Nie zapisano zdjęcia.";
            return View();
        }

        //
        // POST: /Gallery/Delete/

        [HttpPost]
        public JsonResult PhotoDelete(int id)
        {
            Photo photo = db.Photos.Single(p => p.id == id);
            db.Photos.Remove(photo);
            db.SaveChanges();

            photo.deleteFiles();

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
