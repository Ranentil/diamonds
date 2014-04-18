using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using Mvc.Mailer;
using Diamonds.Models;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        //
        // GET: /Index/

        public ActionResult Index()
        {
            User user = db.Users.Single(u => u.email == User.Identity.Name);
            return View(user);
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Podane hasło jest nieprawidłowe.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User(model.UserName, model.Email, model.Password);
                db.Users.Add(user);
                db.SaveChanges();

                new Diamonds.Mailers.UserMailer().Welcome(user);
                // Send verification mail
                // Information about send mail
            }
            TempData["Message"] = "Rejestracja powiodła się, sprawdź skrzynkę pocztową, aby potwiedzić Twój adres e-mail.";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmEmail(string email, string hash)
        {
            User user = db.Users.SingleOrDefault(u => u.email == email);

            if (user != null && user.checkEmail(hash))
            {
                user.isConfirmed = true;
                db.SaveChanges();
                TempData["Message"] = ViewBag.Lang["user-confirm-email"];
            }

            TempData["Message"] = "Wystąpił błąd i nie udało się potwiedzić Twojego e-maila.";
            return RedirectToAction("Index", "Home");
        }
        
        public ViewResult Admin()
        {
            return View();
        }



        public ViewResult Edit()
        {

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(LocalPasswordModel model)
        {

            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Profile");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
