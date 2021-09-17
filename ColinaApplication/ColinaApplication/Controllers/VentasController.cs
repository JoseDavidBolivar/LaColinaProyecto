using ColinaApplication.Data.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    public class VentasController : Controller
    {
        SolicitudBsuiness solicitud;
        public VentasController()
        {
            solicitud = new SolicitudBsuiness();
        }

        [HttpGet]
        public ActionResult Ingresos()
        {
            return View();
        }

    }
}
