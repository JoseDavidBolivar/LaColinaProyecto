using ColinaApplication.Data.Business;
using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using Entity;
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
        public void InsertaProductosSolicitud(TBL_PRODUCTOS_SOLICITUD model, int CantidadPlatos, string idMesa)
        {
            TBL_PRODUCTOS_SOLICITUD modelo = new TBL_PRODUCTOS_SOLICITUD();
            modelo = model;
            var cantidaddisponible = solicitud.ConsultaCantidadProducto(modelo.ID_PRODUCTO);
            if (cantidaddisponible >= CantidadPlatos)
            {
                //IMPRIMIR TICKET
                bool resp = ImprimeProductos(Convert.ToString(CantidadPlatos), Convert.ToString(modelo.ID_PRODUCTO), modelo.DESCRIPCION, idMesa);
                if (resp)
                    modelo.ESTADO_PRODUCTO = Estados.Entregado;
                else
                    modelo.ESTADO_PRODUCTO = Estados.NoEntregado;

                //ACTUALIZA CANTIDAD PRODUCTO
                var ActualizaCantidadProducto = solicitud.ActualizaCantidadProducto(modelo.ID_PRODUCTO, (cantidaddisponible - CantidadPlatos));
                for (int i = 0; i < CantidadPlatos; i++)
                {
                    //INSERTA LOS PRODUCTOS EN LA SOLICITUD
                    var InsertaSolicitud = solicitud.InsertaProductosSolicitud(modelo);
                    if (InsertaSolicitud.ID != 0)
                    {                        
                        //ACTUALIZA TOTAL SOLICITUD
                        var ActualizaSolicitud = solicitud.ActualizaTotalSolicitud(modelo.ID_SOLICITUD, modelo.PRECIO_PRODUCTO);
                    }
                    
                }
                Clients.Caller.GuardoProductos("Productos Insertados Exitosamente !");
                ConsultaMesaAbierta(idMesa);
            }
            else
            {
                Clients.Caller.GuardoProductos("Producto agotado. Elige menos o tal vez el inventario ya se acabo !");
                ConsultaMesaAbierta(idMesa);
            }
        }
        public void GuardaDatosCliente(decimal Id, string Cedula, string NombreCliente, string Observaciones, string OtrosCobros, string Descuentos, string SubTotal, string Estado, string IdMesa, string porcentajeServicio, string MetodoPago, string Voucher, string CantEfectivo)
        {
            TBL_SOLICITUD model = new TBL_SOLICITUD();
            model.ID = Id;
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