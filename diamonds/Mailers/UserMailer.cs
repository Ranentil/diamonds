using Mvc.Mailer;
using Diamonds.Models.Entities;

namespace Diamonds.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome(User user)
		{
            ViewData.Model = user;

			return Populate(x =>
			{
				x.Subject = "Witamy na softball.pl";
				x.ViewName = "Welcome";
				x.To.Add(user.email);
			}); 
		}
 
		public virtual MvcMailMessage PasswordReset(User user, string salt)
		{
            ViewData.Model = user;
            ViewBag.salt = salt;

			return Populate(x =>
			{
				x.Subject = "Reset has³a na softball.pl";
				x.ViewName = "PasswordReset";
				x.To.Add(user.email);
			});
		}
 	}
}