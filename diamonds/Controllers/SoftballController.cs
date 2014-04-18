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
            return View();
        }

        #region Pages

        public ViewResult WhatIsSoftball()
        {
            ViewData.Model = "co-to-softball";
            return View("Page");
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
