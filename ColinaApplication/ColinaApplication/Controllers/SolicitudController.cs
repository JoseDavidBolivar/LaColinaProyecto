using ColinaApplication.Data.Business;
using ColinaApplication.Data.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    public class SolicitudController : Controller
    {
        SolicitudBsuiness solicitud;
        public SolicitudController()
        {
            solicitud = new SolicitudBsuiness();
        }

        [HttpGet]
        public ActionResult SeleccionarMesa()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Pedido(string Id)
        {
            TBL_SOLICITUD model = new TBL_SOLICITUD();
            model.ID = Convert.ToDecimal(Id);
            return View(model);
        }
    }
}