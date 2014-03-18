using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Diamonds.Models;
using Diamonds.Models.Entities;

namespace Diamonds.Controllers
{
    [Authorize]
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

        [HttpPost, AllowAnonymous]
        public ActionResult LogOn(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
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
                User user = new User();
                user.name = model.UserName;
                user.setPassword(model.Password);
                user.email = model.Email;
                user.lastLoginDate = DateTime.MinValue;
                user.createDate = DateTime.Now;
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

    }
}
