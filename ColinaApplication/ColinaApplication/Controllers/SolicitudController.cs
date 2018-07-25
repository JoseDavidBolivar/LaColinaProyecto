using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    public class SolicitudController : Controller
    {
        //SolicitudBusiness solicitud;

        //public SolicitudController()
        //{
        //    solicitud = new SolicitudBusiness();
        //}

        [HttpGet]
        public ActionResult SeleccionarMesa()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Pedido()
        {
            return View();
        }
    }
}