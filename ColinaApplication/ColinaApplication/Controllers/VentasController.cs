using ColinaApplication.Data.Business;
using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using Entity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColinaApplication.Controllers
{
    [Expiring_Filter]
    public class VentasController : Controller
    {
        VentasBusiness ventas;
        public VentasController()
        {
            ventas = new VentasBusiness();
        }

        [HttpGet]
        public ActionResult Ingresos()
        {
            SuperViewModels model = new SuperViewModels();
            ViewBag.ultimoCierre = UltimoCierre();
            return View(model);
        }
        [HttpPost]
        public ActionResult CambiarEstadoCaja(string Cierre)
        {
            List<TBL_MASTER_MESAS> mesas = new List<TBL_MASTER_MESAS>();
            switch (Cierre)
            {
                case "Abrir Caja":
                    mesas = ventas.ConsultaMesasCargo(Convert.ToDecimal(Session["IdUsuario"].ToString()));
                    foreach (var item in mesas)
                    {
                        TBL_MASTER_MESAS model = new TBL_MASTER_MESAS();
                        model.ID = item.ID;
                        model.ESTADO = Estados.Libre;
                        ventas.ActualizaEstadoMesa(model);
                    }
                    TBL_CIERRES modelCierres = new TBL_CIERRES();
                    modelCierres.FECHA_HORA_APERTURA = DateTime.Now;
                    modelCierres.ID_USUARIO = Convert.ToDecimal(Session["IdUsuario"].ToString());
                    TempData["CierraCaja"] = ventas.InsertaNuevoCierre(modelCierres);
                    break;
                case "Cerrar Caja":
                    var solicitudes = ventas.ConsultaSolicitudes(Convert.ToDecimal(Session["IdUsuario"].ToString()), ventas.CierreUsuarioId(Convert.ToDecimal(Session["IdUsuario"].ToString())).FECHA_HORA_APERTURA);
                    if (solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.Abierta || x.ESTADO_SOLICITUD == Estados.Llevar).ToList().Count == 0)
                    {
                        mesas = ventas.ConsultaMesasCargo(Convert.ToDecimal(Session["IdUsuario"].ToString()));
                        foreach (var item in mesas)
                        {
                            TBL_MASTER_MESAS model = new TBL_MASTER_MESAS();
                            model.ID = item.ID;
                            model.ESTADO = Estados.Cerrado;
                            ventas.ActualizaEstadoMesa(model);
                        }
                        TBL_CIERRES modelCierres2 = new TBL_CIERRES();
                        var ultimocierre = ventas.CierreUsuarioId(Convert.ToDecimal(Session["IdUsuario"].ToString()));
                        modelCierres2.ID = ultimocierre.ID;
                        modelCierres2.FECHA_HORA_CIERRE = DateTime.Now;
                        modelCierres2.ID_USUARIO = Convert.ToDecimal(Session["IdUsuario"].ToString());
                        modelCierres2.CANT_MESAS_ATENDIDAS = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.CancelaPedido).Count();
                        modelCierres2.CANT_FINALIZADAS = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.Finalizada).Count();
                        modelCierres2.TOTAL_FINALIZADAS = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.Finalizada).Sum(a => a.TOTAL);
                        modelCierres2.CANT_LLEVAR = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.Llevar).Count();
                        modelCierres2.TOTAL_LLEVAR = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.Llevar).Sum(a => a.TOTAL);
                        modelCierres2.CANT_CANCELADAS = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.CancelaPedido).Count();
                        modelCierres2.TOTAL_CANCELADAS = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.CancelaPedido).Sum(a => a.TOTAL);
                        modelCierres2.CANT_CONSUMO_INTERNO = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.ConsumoInterno).Count();
                        modelCierres2.TOTAL_CONSUMO_INTERNO = solicitudes.Where(x => x.ESTADO_SOLICITUD == Estados.ConsumoInterno).Sum(a => a.TOTAL);
                        modelCierres2.OTROS_COBROS_TOTAL = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.Cancelado).Sum(a => a.OTROS_COBROS);
                        modelCierres2.DESCUENTOS_TOTAL = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.Cancelado).Sum(a => a.DESCUENTOS);
                        modelCierres2.IVA_TOTAL = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.Cancelado).Sum(a => a.IVA_TOTAL);
                        modelCierres2.I_CONSUMO_TOTAL = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.Cancelado).Sum(a => a.I_CONSUMO_TOTAL);
                        modelCierres2.SERVICIO_TOTAL = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.Cancelado).Sum(a => a.SERVICIO_TOTAL);
                        modelCierres2.TOTAL_EFECTIVO = solicitudes.Where(x => x.METODO_PAGO == "EFECTIVO").Sum(a => a.TOTAL);
                        modelCierres2.TOTAL_TARJETA = solicitudes.Where(x => x.METODO_PAGO == "TARJETA").Sum(a => a.TOTAL);
                        modelCierres2.VENTA_TOTAL = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.CancelaPedido).Sum(a => a.TOTAL);
                        TempData["CierraCaja"] = ventas.ActualizaCierre(modelCierres2);
                    }
                    else
                    {
                        TempData["CierraCaja"] = "Mesas Abiertas";
                    }

                    break;
                default:
                    break;
            }
            return RedirectToAction("Ingresos");
        }
        [HttpPost]
        public ActionResult ConsultaFechas(DateTime fechaInicial, DateTime fechaFinal, string Consulta)
        {
            ViewBag.ultimoCierre = UltimoCierre();
            SuperViewModels model = new SuperViewModels();
            fechaInicial = fechaInicial.Date.Add(new TimeSpan(0, 0, 0));
            fechaFinal = fechaFinal.Date.Add(new TimeSpan(23, 59, 59));
            if (Consulta == "Consultar")
            {
                model.Solicitudes = ventas.ConsultaSolicitudesXFecha(fechaInicial, fechaFinal);
                return View("Ingresos", model);
            }
            else
            {
                model.Solicitudes = ventas.ConsultaSolicitudesXFecha(fechaInicial, fechaFinal);
                
                return View("Ingresos");
            }
        }
        [HttpPost]
        public ActionResult ConsultaFactura(string NumeroFactura)
        {
            ViewBag.ultimoCierre = UltimoCierre();
            SuperViewModels model = new SuperViewModels();
            model.SolicitudModel = ventas.ConsultaSolicitudXId(Convert.ToDecimal(NumeroFactura));
            if (model.SolicitudModel == null)
            {
                model.SolicitudModel = new ConsultaSolicitud();
                model.SolicitudModel.ProductosSolicitud = new List<ProductosSolicitud>();
            }
                          
            return View("Ingresos", model);

        }
        [HttpGet]
        public ActionResult Gastos()
        {
            return View();
        }


        public string UltimoCierre()
        {
            var ultimocierre = ventas.CierreUsuarioId(Convert.ToDecimal(Session["IdUsuario"].ToString()));
            string respuesta = "";
            if (ultimocierre != null)
            {
                if (ultimocierre.FECHA_HORA_CIERRE == null)
                    respuesta = "Cerrar Caja";
                else
                    respuesta = "Abrir Caja";
            }
            else
            {
                respuesta = "Abrir Caja";
            }
            return respuesta;
        }
    }
}
