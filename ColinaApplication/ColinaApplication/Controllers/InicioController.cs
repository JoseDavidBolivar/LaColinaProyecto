using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    public class InicioController : Controller
    {
        [HttpGet]
        public ActionResult LaColinaLogin()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LaColinaRestaurante()
        {
            return View();
        }
    }
}