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
        public void InsertaProductosSolicitud(List<TBL_PRODUCTOS_SOLICITUD> list1, List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD> list2, string IdMesa)
        {
            List<List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD>> model = new List<List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD>>();
            var count = (from a in list2 select new { a.ID}).Distinct().Count();
            
            for (int i = 0; i < count; i++)
            {
                var fila = Convert.ToInt32(list2[i].ID);
                
                List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD> arrays = list2.Where(a => a.ID == fila && (a.DESCRIPCION != null || a.VALOR != null)).ToList();
                model.Add(arrays);                
            }
            var respuesta = solicitud.InsertaProductos(list1, model);

            Clients.Caller.GuardoProductos("Productos Insertados Exitosamente");
            Clients.All.ActualizaCantidadProductos(respuesta);
            ConsultaMesaAbierta(IdMesa);
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
            Clients.Caller.GuardoProductos(respuesta);
            ConsultaMesaAbierta(IdMesa);
        }
        public void CancelaPedido(decimal IdSolicitud)
        {
            var respuesta = solicitud.CancelaProductosSolicitud(IdSolicitud);

        }

    }
}