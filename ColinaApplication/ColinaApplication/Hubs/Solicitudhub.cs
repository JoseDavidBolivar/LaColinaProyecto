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
        public void ActualizaMesa(string id, string Estado, string IdUser)
        {
            solicitud.ActualizaEstadoMesa(Convert.ToDecimal(id), Estado);
            if (Estado == "OCUPADO")
            {
                InsertaSolicitud(id, Estado, IdUser);
            }
            ListarEstadoMesas("SI", Convert.ToDecimal(id));
        }
        public void InsertaSolicitud(string IdMesa, string Estado, string IdUser)
        {
            TBL_SOLICITUD model = new TBL_SOLICITUD();
            model.ID_MESA = Convert.ToDecimal(IdMesa);
            model.ID_MESERO = Convert.ToDecimal(IdUser);
            model.ESTADO_SOLICITUD = Estado;
            model.OTROS_COBROS = 0;
            model.DESCUENTOS = 0;
            model.TOTAL = 0;
            solicitud.InsertaSolicitud(model);

        }
        public void ListaMesas()
        {
            ListarEstadoMesas("NO", 0);
        }
        public void ListarEstadoMesas(string Redirecciona, decimal Idmesa)
        {
            List<TBL_MASTER_MESAS> listamesas = new List<TBL_MASTER_MESAS>();
            List<MasterMesas> mesas = new List<MasterMesas>();
            listamesas = solicitud.ListaMesas();
            Clients.All.ListaMesas(listamesas, Redirecciona, Idmesa);
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
        public void GuardaDatosCliente(decimal Id, string Cedula, string NombreCliente, string Observaciones, string OtrosCobros, string Descuentos, string Total, string Estado, string IdMesa)
        {
            TBL_SOLICITUD model = new TBL_SOLICITUD();
            model.ID = Id;
            model.IDENTIFICACION_CLIENTE = Cedula;
            model.NOMBRE_CLIENTE = NombreCliente;
            model.OBSERVACIONES = Observaciones;
            model.ESTADO_SOLICITUD = Estado;
            model.OTROS_COBROS = Convert.ToDecimal(OtrosCobros);
            model.DESCUENTOS = Convert.ToDecimal(Descuentos);
            model.TOTAL = Convert.ToDecimal(Total);

            var respuesta = solicitud.ActualizaSolicitud(model);
            Clients.Caller.GuardoCliente(respuesta);
            ConsultaMesaAbierta(IdMesa);
        }
        public void CancelaPedido(decimal IdSolicitud)
        {
            var respuesta = solicitud.CancelaProductosSolicitud(IdSolicitud);

        }

    }
}