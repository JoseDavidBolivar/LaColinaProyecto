using ColinaApplication.Data.Business;
using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    [Expiring_Filter]
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
            model.ID_MESA = Convert.ToDecimal(Id);
            return View(model);
        }
        public JsonResult ListaCategorias()
        {
            var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ListaCategorias()), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult ListaProductos(string IdProducto)
        {
            var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ListaProductos(Convert.ToDecimal(IdProducto))), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        

    }
}