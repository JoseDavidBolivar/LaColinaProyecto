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
    public class HomeController : Controller
    {
        HomeBusiness inicio;
        public HomeController()
        {
            inicio = new HomeBusiness();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [HttpGet]
        public ActionResult LaColinaLogin()
        {
            return View();
        }
        public JsonResult InicioSesion(decimal Cedula, string Contraseña)
        {
            Session.Clear();
            TBL_USUARIOS user = new TBL_USUARIOS();
            user = inicio.Login(Cedula, Contraseña);
            if (user.ID > 0)
            {
                Session["IdUsuario"] = user.ID;
                Session["Cedula"] = user.CEDULA;
                Session["Nombre"] = user.NOMBRE;
                Session["IdPerfil"] = user.ID_PERFIL;
            }
            var jsonResult = Json(JsonConvert.SerializeObject(user), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult Salir()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("LaColinaLogin");
        }

    }
}