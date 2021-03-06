﻿using System;
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
using BotDetect.Web.UI.Mvc;

namespace Diamonds.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        #region Main Views

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
        [CaptchaValidation("CaptchaCode", "SampleCaptcha", "Incorrect CAPTCHA code!")]
        public ActionResult Register(RegisterModel model)
        {
            if (db.Users.Any(u => u.email == model.Email))
                ModelState.AddModelError("email", "Użytkownik o tym adresie już istnieje");

            if (ModelState.IsValid)
            {
                User user = new User(model.UserName, model.Email, model.Password);
                db.Users.Add(user);
                db.SaveChanges();

                new Diamonds.Mailers.UserMailer().Welcome(user);
                // Send verification mail
                // Information about send mail

                TempData["Message"] = "Rejestracja powiodła się, możesz się teraz zalogować."; 
                //TempData["Message"] = "Rejestracja powiodła się, sprawdź skrzynkę pocztową, aby potwiedzić Twój adres e-mail.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Rejestracja nie powiodła się.";
            return View(model);
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

        #endregion

        #region Manage Account

        public ViewResult Admin()
        {
            return View();
        }

        public ViewResult Manage()
        {
            User user = db.Users.Single(u => u.email == User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        public ActionResult Manage(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Zapisano";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Błąd";
            return View(user);
        }

        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                bool changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);

                if (changePasswordSucceeded)
                {
                    TempData["Message"] = "Hasło zostało zmienione";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Niepoprawne stare lub nowe hasło.");
                }
            }
            return View(model);
        }


        [AllowAnonymous]
        public JsonResult RemindPassword(string email)
        {
            User user = db.Users.SingleOrDefault(u => u.email == email);
            if (user == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            string salt = Crypto.sha256encrypt(user.email + user.lastLoginDate.ToString());
            new Diamonds.Mailers.UserMailer().PasswordReset(user, salt).Send();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ChangeForgottenPassword(string email, string reset)
        {
            User user = db.Users.SingleOrDefault(u => u.email == email);
            if (user == null)
            {
                TempData["Error"] = "Niepoprawny adres URL";
                return RedirectToAction("Login");
            }

            string salt = Crypto.sha256encrypt(user.email + user.lastLoginDate.ToString());
            if (salt != reset)
            {
                TempData["Error"] = "Niepoprawny adres URL";
                return RedirectToAction("Login");
            }

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangeForgottenPassword(string email, string reset, ChangePasswordModel model)
        {
            User user = db.Users.SingleOrDefault(u => u.email == email);
            if (user == null)
            {
                TempData["Error"] = "Niepoprawny adres URL";
                return RedirectToAction("Login");
            }

            string salt = Crypto.sha256encrypt(user.email + user.lastLoginDate.ToString());
            if (salt != reset)
            {
                TempData["Error"] = "Niepoprawny adres URL";
                return RedirectToAction("Login");
            }

            user.setPassword(model.NewPassword);
            db.SaveChanges();
            TempData["Message"] = "Hasło zostało zresetowane! Zaloguj się używając swojego nowego hasła";

            return RedirectToAction("Login");
        }

        #endregion

        #region External Logins

        //
        // POST: /Account/ExternalLogin

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
            //return View();
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                //using (UsersContext db = new UsersContext())
                //{
                //    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                //    // Check if user already exists
                //    if (user == null)
                //    {
                //        // Insert name into the profile table
                //        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                //        db.SaveChanges();

                //        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                //        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                //        return RedirectToLocal(returnUrl);
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                //    }
                //}
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //[AllowAnonymous, ChildActionOnly]
        //public ActionResult ExternalLoginsList(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        //}

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            //ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            //List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            //foreach (OAuthAccount account in accounts)
            //{
            //    AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

            //    externalLogins.Add(new ExternalLogin
            //    {
            //        Provider = account.Provider,
            //        ProviderDisplayName = clientData.DisplayName,
            //        ProviderUserId = account.ProviderUserId,
            //    });
            //}

            //ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //return PartialView("_RemoveExternalLoginsPartial", externalLogins);
            return View();
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion

        #region Others

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

        #endregion
    }
}
