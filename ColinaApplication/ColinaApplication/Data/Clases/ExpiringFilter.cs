using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Data.Clases
{
    public class Expiring_Filter : ActionFilterAttribute
    {

        //Aca igual se puede hacer vencimiento de sesion a 30 min if Session["timeCheck"] -Date.Now() >30 then ctx.Session= null;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            
            if (HttpContext.Current.Session["IdPerfil"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/LaColinaLogin");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}