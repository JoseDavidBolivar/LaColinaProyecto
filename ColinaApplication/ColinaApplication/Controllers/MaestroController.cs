using ColinaApplication.Data.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    [Expiring_Filter]
    public class MaestroController : Controller
    {
        [HttpGet]
        public ActionResult Personal()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Configuracion()
        {
            return View();
        }
    }
}