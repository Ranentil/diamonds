using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    public class LiveController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Live/

        public ActionResult Index()
        {
            return View();
        }

    }
}
