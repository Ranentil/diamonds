using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class TeamController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Team/

        public ActionResult Index()
        {
            List<Player> players = db.Players.Where(p => p.isActive).OrderBy(p => p.lastName).ThenBy(p => p.firstName).ToList();
            return View(players);
        }

        public ActionResult Admin()
        {
            List<Player> players = db.Players.Where(p => p.isActive).OrderBy(p => p.lastName).ThenBy(p => p.firstName).ToList();
            return View(players);
        }

        public ViewResult Create()
        {
            return View("Edit", new Player());
        }

        [HttpPost]
        public ActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano";
                return RedirectToAction("Index");
            }
            return View("Edit", player);
        }

        public ViewResult Edit(int id)
        {
            Player player = db.Players.Single(p => p.id == id);
            return View(player);
        }

        [HttpPost]
        public ActionResult Edit(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Attach(player);
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano.";
                return RedirectToAction("Localizations", new { id = player.id });
            }
            return View(player);
        }

    }
}
