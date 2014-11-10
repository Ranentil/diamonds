using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Diamonds.Models.Entities;

namespace Diamonds.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }

    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("Password")]
        public string ConfirmPassword { get; set; }
    }


    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        //[Display(Name = lang("login-obecne-haslo"))]
        public string OldPassword { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = lang("login-za-krotkie-haslo"), MinimumLength = 6)]
        [DataType(DataType.Password)]
        //[Display(Name = lang("login-nowe-haslo"))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        //[Display(Name = lang("login-potwierdzenie-hasla"))]
        //[System.Web.Mvc.Compare("NewPassword", ErrorMessage = lang("login-hasla-nie-sa-takie-same"))]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        //[Display(Name = lang("login-login"))]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
