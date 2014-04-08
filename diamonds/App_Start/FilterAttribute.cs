using System;
using System.Linq;

namespace Diamonds
{
    public class FilterAttribute : System.Web.Mvc.Controller
    {
        protected override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            Diamonds.Models.Entities.DiamondsEntities db = new Diamonds.Models.Entities.DiamondsEntities();
            filterContext.Controller.ViewBag.Lang = db.Localizations.ToDictionary(d => d.name, d => d.text);
            base.OnActionExecuted(filterContext);
        }
    }
}