using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    public class CajaController : Controller
    {
        [HttpGet]
        public ActionResult Factura()
        {
            return View();
        }
    }
}