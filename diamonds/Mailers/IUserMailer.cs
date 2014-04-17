using Mvc.Mailer;

namespace Diamonds.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome(Diamonds.Models.Entities.User user);
			MvcMailMessage PasswordReset();
	}
}