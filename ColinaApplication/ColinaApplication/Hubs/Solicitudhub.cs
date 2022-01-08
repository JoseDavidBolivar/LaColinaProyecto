using ColinaApplication.Data.Business;
using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using Entity;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Hubs
{
    public class Solicitudhub : Hub
    {
        SolicitudBsuiness solicitud;

        public Solicitudhub()
        {
            solicitud = new SolicitudBsuiness();
        }
        public void ActualizaMesa(string id, string Estado, string IdUser, string Redirecciona, string Ruta)
        {
            solicitud.ActualizaEstadoMesa(Convert.ToDecimal(id), Estado);
            if (Estado == Estados.Ocupado && Redirecciona == "SI")
            {
                InsertaSolicitud(id, Estados.Ocupado, IdUser);
            }
            ListarEstadoMesas(Redirecciona, Convert.ToDecimal(id), Ruta);
        }
        public void InsertaSolicitud(string IdMesa, string Estado, string IdUser)
        {
            TBL_SOLICITUD model = new TBL_SOLICITUD();
            model.ID_MESA = Convert.ToDecimal(IdMesa);
            model.ID_MESERO = Convert.ToDecimal(IdUser);
            model.ESTADO_SOLICITUD = Estado;
            model.OTROS_COBROS = 0;
            model.DESCUENTOS = 0;
            model.SUBTOTAL = 0;
            model.IVA_TOTAL = 0;
            model.I_CONSUMO_TOTAL = 0;
            model.SERVICIO_TOTAL = 0;
            model.TOTAL = 0;
            solicitud.InsertaSolicitud(model);

        }
        public void ListarEstadoMesas(string Redirecciona, decimal Idmesa, string Ruta)
        {
            List<TBL_MASTER_MESAS> listamesas = new List<TBL_MASTER_MESAS>();
            List<MasterMesas> mesas = new List<MasterMesas>();
            listamesas = solicitud.ListaMesas();
            Clients.All.ListaMesas(listamesas, Redirecciona, Idmesa, Ruta);
        }

        public void ConsultaMesaAbierta(string Id)
        {
            List<ConsultaSolicitudGeneral> ConsultaMesa = new List<ConsultaSolicitudGeneral>();
            ConsultaMesa = solicitud.ConsultaSolicitudMesa(Convert.ToDecimal(Id));
            Clients.All.ListaDetallesMesa(ConsultaMesa);
        }
        public void InsertaProductosSolicitud(List<TBL_PRODUCTOS_SOLICITUD> model, string idMesa)
        {
            bool cantDisponible = true;
            List<string> data = new List<string>();
            List<TBL_PRODUCTOS_SOLICITUD> ConteoProductos = new List<TBL_PRODUCTOS_SOLICITUD>();

            ConteoProductos = AgrupaProductos(model);
            // VALIDA SI ALGUN PRODUCTO NO TIENE CANTIDAD EN EXISTENCIA
            foreach (var item in ConteoProductos)
            {
                var cantidaddisponible = solicitud.ConsultaCantidadProducto(item.ID_PRODUCTO);
                if (cantidaddisponible < item.ID)
                {
                    cantDisponible = false;
                    data.Add(item.ESTADO_PRODUCTO);
                }
            }
            
            if (cantDisponible)
            {
                //IMPRIMIR TICKET
                bool resp = solicitud.ImprimirPedidoFactura(model, Convert.ToDecimal(idMesa));
                foreach (var item in model)
                {
                    if (resp)
                        item.ESTADO_PRODUCTO = Estados.Entregado;
                    else
                        item.ESTADO_PRODUCTO = Estados.NoEntregado;
                }

                //ACTUALIZA CANTIDAD PRODUCTO
                foreach (var item in ConteoProductos)
                {
                    var cantidaddisponible = solicitud.ConsultaCantidadProducto(item.ID_PRODUCTO);
                    var ActualizaCantidadProducto = solicitud.ActualizaCantidadProducto(item.ID_PRODUCTO, (cantidaddisponible - item.ID));
                }

                for (int i = 0; i < model.Count; i++)
                {
                    //INSERTA LOS PRODUCTOS EN LA SOLICITUD
                    for (int j = 0; j < model[i].ID; j++)
                    {
                        var InsertaSolicitud = solicitud.InsertaProductosSolicitud(model[i]);
                        if (InsertaSolicitud.ID != 0)
                        {
                            //ACTUALIZA TOTAL SOLICITUD
                            var ActualizaSolicitud = solicitud.ActualizaTotalSolicitud(model[i].ID_SOLICITUD, model[i].PRECIO_PRODUCTO);
                        }
                    }
                    
                    
                }
                Clients.Caller.GuardoProductos("Productos Insertados Exitosamente !");
                ConsultaMesaAbierta(idMesa);
            }
            else
            {
                Clients.Caller.FaltaExistencias(data);
                ConsultaMesaAbierta(idMesa);
            }
        }
        public List<TBL_PRODUCTOS_SOLICITUD> AgrupaProductos(List<TBL_PRODUCTOS_SOLICITUD> productosSolicitud)
        {
            List<TBL_PRODUCTOS_SOLICITUD> resultado = new List<TBL_PRODUCTOS_SOLICITUD>();
            var distinctProductos = productosSolicitud.DistinctBy(c => c.ID_PRODUCTO).ToList();
            foreach (var item in distinctProductos)
            {
                TBL_PRODUCTOS_SOLICITUD model = new TBL_PRODUCTOS_SOLICITUD();
                //CANTIDAD DEL PRODUCTO
                model.ID = productosSolicitud.Where(x => x.ID_PRODUCTO == item.ID_PRODUCTO).Sum(x => x.ID);
                model.ID_PRODUCTO = item.ID_PRODUCTO;
                //NOMBRE PRODUCTO
                model.ESTADO_PRODUCTO = item.ESTADO_PRODUCTO;
                
                resultado.Add(model);
            }
            return resultado;
        }
        public void GuardaDatosCliente(decimal Id, string Cedula, string NombreCliente, string Observaciones, string OtrosCobros, string Descuentos, string SubTotal, string Estado, string IdMesa, string porcentajeServicio, string MetodoPago, string Voucher, string CantEfectivo, decimal idMesero)
        {
            TBL_SOLICITUD model = new TBL_SOLICITUD();
            model.ID = Id;
            model.ID_MESERO = idMesero;
            model.ID_MESA = Convert.ToDecimal(IdMesa);
            model.IDENTIFICACION_CLIENTE = Cedula;
            model.NOMBRE_CLIENTE = NombreCliente;
            if(!string.IsNullOrEmpty(Observaciones))
                model.OBSERVACIONES = Observaciones.ToUpper();
            else
                model.OBSERVACIONES = Observaciones;
            model.ESTADO_SOLICITUD = Estado;
            model.OTROS_COBROS = string.IsNullOrEmpty(OtrosCobros) ? 0 : Convert.ToDecimal(OtrosCobros);
            model.DESCUENTOS = string.IsNullOrEmpty(Descuentos) ? 0 : Convert.ToDecimal(Descuentos);
            model.SUBTOTAL = Convert.ToDecimal(SubTotal);
            model.PORCENTAJE_SERVICIO = Convert.ToDecimal( porcentajeServicio);
            model.METODO_PAGO = MetodoPago;
            model.VOUCHER = Voucher;
            model.CANT_EFECTIVO = Convert.ToDecimal(CantEfectivo);
            var respuesta = solicitud.ActualizaSolicitud(model);
            Clients.Caller.GuardoCliente(respuesta);
            ConsultaMesaAbierta(IdMesa);
        }
        public void CancelaPedido(decimal IdSolicitud, bool RetornaInventario)
        {
            var respuesta = solicitud.CancelaProductosSolicitud(IdSolicitud, RetornaInventario);

        }
        public void CancelaPedidoXId(decimal IdProductoSolicitud, bool RetornaInventario)
        {
            var respuesta = solicitud.CancelaProductoSolicitudXId(IdProductoSolicitud, RetornaInventario);
        }
        public void ImprimirFactura(string IdMesa)
        {
            bool respuesta = solicitud.ImprimirFactura(IdMesa);
            ConsultaMesaAbierta(IdMesa);
        }
        public bool ImprimeProductos(string cantidad, string idproducto, string descripcion, string idMesa)
        {
            bool respuesta = solicitud.ImprimirPedido(cantidad, idproducto, descripcion, idMesa);
            return respuesta;
        }
        public void ActualizaIdmesaHTML(string idmesa, string idmesaAnterior)
        {
            Clients.All.CambiaIdMesa(idmesa, idmesaAnterior);
        }
    }
}