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
    public class ResultController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Result/

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Team> teams = db.Teams.Where(t => t.ResultsGuest.Any(r => r.eventDate.Year == DateTime.Now.Year)
                || t.ResultsHome.Any(r => r.eventDate.Year == DateTime.Now.Year)).ToList();
            ViewBag.Results = db.Results.OrderBy(r => r.eventDate).ToList();
            return View(teams);
        }

        public ActionResult Admin()
        {
            List<Result> results = db.Results.OrderBy(r => r.eventDate).ToList();
            return View(results);
        }

        public ViewResult Create()
        {
            ViewBag.team1Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            ViewBag.team2Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            return View("Edit", new Result());
        }

        [HttpPost]
        public ActionResult Create(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano.";
                return RedirectToAction("Admin");
            }
            ViewBag.team1Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            ViewBag.team2Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            return View("Edit", result);
        }


        public ViewResult Edit(int id)
        {
            ViewBag.team1Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            ViewBag.team2Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            Result result = db.Results.Single(n => n.id == id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Attach(result);
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano.";
                return RedirectToAction("Edit", new { id = result.id });
            }
            ViewBag.team1Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            ViewBag.team2Id = new SelectList(db.Teams.OrderBy(t => t.name), "id", "name");
            return View(result);
        }

        public ActionResult Delete(int id)
        {
            Result result = db.Results.Single(r => r.id == id);
            db.Results.Remove(result);
            db.SaveChanges();

            return RedirectToAction("Admin");
        }

    }
}
