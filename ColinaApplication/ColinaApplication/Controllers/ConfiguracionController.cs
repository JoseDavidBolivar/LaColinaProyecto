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
        ConfiguracionesBusiness configuraciones;
        public ConfiguracionController()
        {
            configuraciones = new ConfiguracionesBusiness();
        }

        [HttpGet]
        public ActionResult Configuraciones()
        {
            SuperViewModels model = new SuperViewModels();
            model.Categorias = configuraciones.ListaCategorias();
            model.Productos = configuraciones.ListaProductos();
            model.Mesas = configuraciones.ListaMesas();
            model.Usuarios = configuraciones.ListaUsuarios();
            model.Perfiles = configuraciones.ListaPerfiles();
            model.Impuestos = configuraciones.ListaImpuestos();
            //LISTA DE SELECCIONABLE CATEGORIA
            ViewBag.listaCategoriasDDL = (model.Categorias.Where(x => x.ESTADO == Estados.Activo).Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.CATEGORIA }).ToList<SelectListItem>());
            //LISTA DE USUARIOS ADMINS PARA ASIGNAR MESAS
            ViewBag.listaUsuariosAdmin = (model.Usuarios.Where(x => x.ID_PERFIL == 1 || x.ID_PERFIL == 2).Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.NOMBRE }).ToList<SelectListItem>());
            //LISTA DE SELECCIONABLE PERFILES
            ViewBag.listaIdPerfilDDL = (model.Perfiles.Select(p => new SelectListItem() { Value = p.ID.ToString(), Text = p.NOMBRE_PERFIL }).ToList<SelectListItem>());

            return View(model);
        }
        [HttpPost]
        public ActionResult AgregarEditarCategoria(SuperViewModels model)
        {
            if (model.CategoriasModel.ID > 0)
                TempData["Resultado"] = configuraciones.ActualizaCategoria(model.CategoriasModel);
            else
                TempData["Resultado"] = configuraciones.InsertaCategoria(model.CategoriasModel);

            TempData["Posicion"] = "DivCategoria";
            return RedirectToAction("Configuraciones");
        }
        [HttpPost]
        public ActionResult AgregarEditarProducto(SuperViewModels model)
        {
            if (model.ProductosModel.ID > 0)
                TempData["Resultado"] = configuraciones.ActualizaProducto(model.ProductosModel);
            else
                TempData["Resultado"] = configuraciones.InsertaProducto(model.ProductosModel);

            TempData["Posicion"] = "DivProductos";
            return RedirectToAction("Configuraciones");
        }
        [HttpPost]
        public ActionResult AgregarEditarMesas(SuperViewModels model)
        {
            if (model.MesasModel.ID > 0)
                TempData["Resultado"] = configuraciones.ActualizaMesa(model.MesasModel);
            else
                TempData["Resultado"] = configuraciones.InsertaMesa(model.MesasModel);

            TempData["Posicion"] = "DivMesas";
            return RedirectToAction("Configuraciones");
        }
        [HttpPost]
        public ActionResult AgregarEditarUsuarios(SuperViewModels model)
        {
            if (model.UsuariosModel.ID > 0)
                TempData["Resultado"] = configuraciones.ActualizaUsuario(model.UsuariosModel);
            else
                TempData["Resultado"] = configuraciones.InsertaUsuario(model.UsuariosModel);

            TempData["Posicion"] = "DivUsuarios";
            return RedirectToAction("Configuraciones");
        }
        [HttpPost]
        public ActionResult AgregarEditarImpuestos(SuperViewModels model)
        {
            if (model.ImpuestosModel.ID > 0)
                TempData["Resultado"] = configuraciones.ActualizaImpuesto(model.ImpuestosModel);
            else
                TempData["Resultado"] = configuraciones.InsertaImpuesto(model.ImpuestosModel);

            TempData["Posicion"] = "DivImpuestos";
            return RedirectToAction("Configuraciones");
        }
        [HttpPost]
        public ActionResult AgregarEditarPerfiles(SuperViewModels model)
        {
            if (model.PerfilesModel.ID > 0)
                TempData["Resultado"] = configuraciones.ActualizaPerfil(model.PerfilesModel);            
            else
                TempData["Resultado"] = configuraciones.InsertaPerfil(model.PerfilesModel);

            TempData["Posicion"] = "DivPerfiles";
            return RedirectToAction("Configuraciones");
        }


    }
}
