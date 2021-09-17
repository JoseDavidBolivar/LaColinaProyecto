using ColinaApplication.Data.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    public class ConfiguracionController : Controller
    {
        SolicitudBsuiness solicitud;
        public ConfiguracionController()
        {
            solicitud = new SolicitudBsuiness();
        }

        [HttpGet]
        public ActionResult Configuraciones()
        {
            return View();
        }


    }
}
