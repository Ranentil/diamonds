using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Diamonds.Models.Entities;

namespace Diamonds.Models
{
    public class DMembershipProvider : MembershipProvider 
    {
        /// <summary>
        ///     Tworzy użytkownika
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser user = GetUser(username, true);

            if (user == null)
            {
                DiamondsEntities db = new DiamondsEntities();

                User newUser = new User();
                newUser.name = username;
                newUser.setPassword(password);
                newUser.email = email;
                newUser.createDate = newUser.lastLoginDate = DateTime.Now;
                newUser.isConfirmed = isApproved;

                db.Users.Add(newUser);
                db.SaveChanges();

                status = MembershipCreateStatus.Success;

                return GetUser(username, true);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        /// <summary>
        /// Funkcja pobiera uzytkownika z bazy
        /// o określonym emailu
        /// </summary>
        /// <param name="username">E-mail użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest on-line</param>
        /// <returns>Użytkownik</returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            DiamondsEntities db = new DiamondsEntities();
            User user = db.Users.FirstOrDefault(u => u.email == username);

            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("DMembershipProvider", user.name, user.id, user.email,
                    string.Empty, string.Empty, user.isConfirmed, false, DateTime.MinValue,
                    DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                return memUser;
            }
            return null;
        }

        /// <summary>
        /// Funkcja zwraca użytkownika o określonym ID
        /// </summary>
        /// <param name="providerUserKey">ID użytkownika</param>
        /// <param name="userIsOnline">Nieważne</param>
        /// <returns>Zwraca użytkownika</returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            DiamondsEntities db = new DiamondsEntities();
            User user = db.Users.First(u => u.id == (int)providerUserKey);

            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("DMembershipProvider", user.name, user.id, user.email,
                    string.Empty, string.Empty, user.isConfirmed, false, DateTime.MinValue,
                    DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                return memUser;
            }
            return null;
        }

        /// <summary>
        /// Zwraca wszystkich użytkowników, określonych przez paginację
        /// </summary>
        /// <param name="pageIndex">Nr strony</param>
        /// <param name="pageSize">Ilość uzytkownikow na stronie</param>
        /// <param name="totalRecords">Ilość wszystkich użytkowników</param>
        /// <returns>Kolekcja użytkowników</returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection userColl = new MembershipUserCollection();
            DiamondsEntities db = new DiamondsEntities();
            totalRecords = db.Users.Count();
            foreach (User user in db.Users.OrderBy(u => u.name).Skip(pageIndex * pageSize).Take(pageSize))
            {
                userColl.Add(new MembershipUser("DMembershipProvider", user.name, user.id, user.email,
                                string.Empty, string.Empty, user.isConfirmed, false, DateTime.MinValue,
                                DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue));
            }

            return userColl;
        }

        /// <summary>
        /// Nazwa aplikacji
        /// </summary>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// Minimalna długość hasła
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        /// <summary>
        /// Email czy ma być unikalny
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        /// <summary>
        /// Możliwość zresetowania hasła
        /// </summary>
        public override bool EnablePasswordReset
        {
            get { return EnablePasswordReset; }
        }

        /// <summary>
        /// Możliwość odczytania hasła
        /// (niemożliwa - hasła zahashowane)
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        /// <summary>
        /// Liczba możliwych wpisanych błędnych haseł
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get { return MaxInvalidPasswordAttempts; }
        }

        /// <summary>
        /// Przedział czasu w jakim użytkownik zostanie zablokowany,
        /// jeżeli wprowadzi błędne hasło
        /// </summary>
        public override int PasswordAttemptWindow
        {
            get { return PasswordAttemptWindow; }
        }

        /// <summary>
        /// Format hasła, zahashowane.
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        /// <summary>
        /// Czy wymaga pytania i odpowiedzi
        /// (nie wymaga)
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        /// <summary>
        /// Sprawdza czy hasło i użytkownik się zgadzają
        /// </summary>
        /// <param name="username">Nazwa użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <returns>Prawda jeżeli wszystko się zgadza</returns>
        public override bool ValidateUser(string username, string password)
        {
            DiamondsEntities db = new DiamondsEntities();
            User user = db.Users.FirstOrDefault(u => u.email == username);
            if (user != null && user.checkPassword(password))
            {
                user.lastLoginDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            DiamondsEntities db = new DiamondsEntities();
            User user = db.Users.FirstOrDefault(u => u.email == username);
            if (!user.checkPassword(oldPassword))
                return false;
            else
            {
                user.setPassword(newPassword);
                db.SaveChanges();
            }

            return true;
        }

        #region Methods and properties not implemented yet

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}