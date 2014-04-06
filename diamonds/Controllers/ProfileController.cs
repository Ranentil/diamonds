using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
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

        public ViewResult Edit()
        {
            
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(LocalPasswordModel model)
        {
            
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult LogOn()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult LogOn(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Podane hasło jest nieprawidłowe.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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

                // Send verification mail
                // Information about send mail
            }
            TempData["Message"] = "Rejestracja powiodła się, sprawdź skrzynkę pocztową";
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Profile/Admin/

        public ViewResult Admin()
        {
            return View();
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
    }
}
