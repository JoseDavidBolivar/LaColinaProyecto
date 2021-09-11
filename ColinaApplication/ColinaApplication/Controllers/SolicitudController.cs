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
        //public JsonResult ListaComposicionSubProductos(string IdSubProducto)
        //{
        //    var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ComposicionSubProductos(Convert.ToDecimal(IdSubProducto))), JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
        public JsonResult DatosElementoInventario(string Id)
        {
            var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ElementoInventario(Convert.ToDecimal(Id))), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        //public JsonResult ListaPreciosSubproductos(string IdSubproducto)
        //{
        //    var jsonResult = Json(JsonConvert.SerializeObject(solicitud.ListaPreciosSubproductos(Convert.ToDecimal(IdSubproducto))), JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
        //public JsonResult ConsultaComposicionSubProducto(decimal idProducto)
        //{
        //    List<TBL_COMPOSICION_PRODUCTOS> list = new List<TBL_COMPOSICION_PRODUCTOS>();
        //    list = solicitud.ComposicionSubProductos(idProducto);
        //    var jsonResult = Json(JsonConvert.SerializeObject(list), JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}

    }
}