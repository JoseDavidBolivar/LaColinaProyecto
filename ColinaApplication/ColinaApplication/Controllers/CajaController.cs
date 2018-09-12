using ColinaApplication.Data.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    [Expiring_Filter]
    public class CajaController : Controller
    {
        [HttpGet]
        public ActionResult Factura()
        {
            return View();
        }
    }
}