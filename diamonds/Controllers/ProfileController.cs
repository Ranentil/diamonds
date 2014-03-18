using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class ProfileController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            User user = db.Users.Single(u => u.email == User.Identity.Name);
            return View(user);
        }

        //
        // GET: /Profile/Admin/

        public ViewResult Admin()
        {
            return View();
        }

    }
}
