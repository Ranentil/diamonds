using System;
using System.Linq;

namespace Diamonds
{
    public class ControllerAttribute : System.Web.Mvc.Controller
    {
        protected override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            Diamonds.Models.Entities.DiamondsEntities db = new Diamonds.Models.Entities.DiamondsEntities();
            filterContext.Controller.ViewBag.Lang = db.Localizations.ToDictionary(d => d.code, d => d.text);
            filterContext.Controller.ViewBag.Slider = db.Galleries.Single(g => g.id == 2).Photos.OrderBy(p => Guid.NewGuid()).ToList();
            base.OnActionExecuted(filterContext);
        }
    }
}