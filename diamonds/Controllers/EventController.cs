using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class EventController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Event/

        public ActionResult Index()
        {
            var date = DateTime.Today;
            List<Event> events = db.Events.Where(e => e.startDate > date).OrderByDescending(e => e.startDate).ToList();
            return View(events);
        }

        public ActionResult Admin()
        {
            var date = DateTime.Today;
            List<Event> events = db.Events.OrderByDescending(e => e.startDate).ToList();
            return View(events);
        }

        public ViewResult Create()
        {
            ViewBag.eventTypeId = new SelectList(db.EventTypes, "id", "name");
            return View("Edit", new Event());
        }

        [HttpPost]
        public ActionResult Create(Event item)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(item);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano";
                return RedirectToAction("Index");
            }
            ViewBag.eventTypeId = new SelectList(db.EventTypes, "id", "name");
            return View("Edit", item);
        }

        public ViewResult Edit(int id)
        {
            ViewBag.eventTypeId = new SelectList(db.EventTypes, "id", "name");
            Event item = db.Events.Single(p => p.id == id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Event item)
        {
            if (ModelState.IsValid)
            {
                db.Events.Attach(item);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano.";
                return RedirectToAction("Index");
            }
            ViewBag.eventTypeId = new SelectList(db.EventTypes, "id", "name");
            return View(item);
        }

        public ActionResult Delete(int id)
        {
            Event item = db.Events.Single(e => e.id == id);
            db.Events.Remove(item);
            db.SaveChanges();

            TempData["Message"] = "Usunięto";
            return RedirectToAction("Index");
        }

    }
}
