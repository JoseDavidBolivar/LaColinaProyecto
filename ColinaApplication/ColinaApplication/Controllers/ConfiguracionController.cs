using ColinaApplication.Data.Business;
using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    [Expiring_Filter]
    public class ConfiguracionController : Controller
    {
        SolicitudBsuiness solicitud;
        ConfiguracionesBusiness configuraciones;
        public ConfiguracionController()
        {
            solicitud = new SolicitudBsuiness();
            configuraciones = new ConfiguracionesBusiness();
        }

        [HttpGet]
        public ActionResult Configuraciones()
        {
            SuperViewModels model = new SuperViewModels();
            model.Categorias = configuraciones.ListaCategorias();
            model.Productos = configuraciones.ListaProductos();
            return View(model);
        }
        [HttpPost]
        public ActionResult AgregarEditarCategoria(SuperViewModels model)
        {
            return RedirectToAction("Configuraciones");
        }
        [HttpPost]
        public ActionResult AgregarEditarProducto(SuperViewModels model)
        {
            return RedirectToAction("Configuraciones");
        }



    }
}
