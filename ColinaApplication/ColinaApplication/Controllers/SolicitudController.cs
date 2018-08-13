using ColinaApplication.Data.Business;
using ColinaApplication.Data.Conexion;
using Newtonsoft.Json;
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
        public JsonResult ListaProductos()
        {
            var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ListaProductos()), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult ListaSubProductos(string IdProducto)
        {
            var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ListaSubProductos(Convert.ToDecimal(IdProducto))), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult ListaComposicionSubProductos(string IdSubProducto)
        {
            var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ComposicionSubProductos(Convert.ToDecimal(IdSubProducto))), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult DatosElementoInventario(string Id)
        {
            var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ElementoInventario(Convert.ToDecimal(Id))), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
    }
}