using Mvc.Mailer;
using Diamonds.Models.Entities;

namespace Diamonds.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome(User user);
			MvcMailMessage PasswordReset(User user);
	}
}