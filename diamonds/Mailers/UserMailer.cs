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
				x.Subject = "Welcome";
				x.ViewName = "Welcome";
				x.To.Add(user.email);
			}); 
		}
 
		public virtual MvcMailMessage PasswordReset(User user)
		{
            ViewData.Model = user;

			return Populate(x =>
			{
				x.Subject = "PasswordReset";
				x.ViewName = "PasswordReset";
				x.To.Add(user.email);
			});
		}
 	}
}