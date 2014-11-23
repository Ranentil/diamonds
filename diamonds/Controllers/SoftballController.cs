using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class SoftballController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Softball/

        public ActionResult Index()
        {
            return View();
        }


        public ViewResult Rules()
        {
            return View();
        }


        public ViewResult Dictionary()
        {
            List<Dictionary> dictionaries = db.Dictionaries.ToList();
            return View(dictionaries.OrderBy(d => d.title));
        }

        [Authorize(Roles = "MODERATOR")]
        public ViewResult DictionaryEdit(int id)
        {
            Dictionary dictionary = db.Dictionaries.Single(d => d.id == id);
            return View(dictionary);
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR")]
        [ValidateInput(false)]
        public ActionResult DictionaryEdit(Dictionary dictionary)
        {
            if (ModelState.IsValid)
            {
                db.Dictionaries.Attach(dictionary);
                db.Entry(dictionary).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano wpis.";
                return RedirectToAction("Dictionary");
            }
            return View(dictionary);
        }

        #region Pages

        public ViewResult WhatIsSoftball()
        {
            ViewData.Model = "co-to-softball";
            return View("_Page");
        }

        #endregion

        #region Others

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}
