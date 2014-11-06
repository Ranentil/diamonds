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

        [AllowAnonymous]
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

        public ViewResult Details(int id)
        {
            Player player = db.Players.Single(p => p.id == id);
            return View(player);
        }

        #region Pages

        public ViewResult Results()
        {
            ViewData.Model = "wyniki";
            return View("Page");
        }

        public ViewResult Recruitment()
        {
            ViewData.Model = "rekrutacja";
            return View("Page");
        }

        public ViewResult History()
        {
            ViewData.Model = "historia";
            return View("Page");
        }

        public ViewResult Field()
        {
            ViewData.Model = "boisko";
            return View("Page");
        }

        #endregion

        #region Players Edit

        [Authorize(Roles = "MODERATOR")]
        public ViewResult Create()
        {
            return View("Edit", new Player());
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR")]
        public ActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano";
                return RedirectToAction("Edit", new { id = player.id });
            }
            return View("Edit", player);
        }

        [Authorize(Roles = "MODERATOR")]
        public ViewResult Edit(int id)
        {
            Player player = db.Players.Single(p => p.id == id);
            ViewBag.Positions = db.Positions.ToList();
            return View(player);
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR")]
        public ActionResult Edit(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Attach(player);
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Pomyślnie zapisano.";
                return RedirectToAction("Admin", new { id = player.id });
            }
            return View(player);
        }

        public JsonResult SavePosition(int id, short positionId)
        {
            Player player = db.Players.Single(p => p.id == id);
            Position position = db.Positions.Single(p => p.id == positionId);
            player.Positions.Add(position);
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePosition(int id, short positionId)
        {
            Player player = db.Players.Single(p => p.id == id);
            Position position = db.Positions.Single(p => p.id == positionId);
            player.Positions.Remove(position);
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "MODERATOR")]
        public ActionResult Delete(int id)
        {
            Player player = db.Players.Single(p => p.id == id);
            db.Players.Remove(player);
            db.SaveChanges();

            return RedirectToAction("Admin");
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
