using ColinaApplication.Data.Business;
using ColinaApplication.Data.Conexion;
using Entity;
using Microsoft.AspNet.SignalR;
using System;
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
        public void ActualizaMesa(string id, string Estado)
        {
            solicitud.ActualizaEstadoMesa(Convert.ToDecimal(id), Estado);
            ListarEstadoMesas();
        }
        public void InsertaSolicitud(string IdMesa, string Estado, string IdUser)
        {
            TBL_SOLICITUD model = new TBL_SOLICITUD();
            model.ID_MESA = Convert.ToDecimal(IdMesa);
            model.ID_MESERO = Convert.ToDecimal(IdUser);
            model.ESTADO_SOLICITUD = Estado;
            solicitud.InsertaSolicitud(model);

        }
        public void ListaMesas()
        {
            ListarEstadoMesas();
        }
        public void ListarEstadoMesas()
        {
            List<TBL_MASTER_MESAS> listamesas = new List<TBL_MASTER_MESAS>();
            List<MasterMesas> mesas = new List<MasterMesas>();
            listamesas = solicitud.ListaMesas();
            Clients.All.ListaMesas(listamesas);
        }
        
        public void ConsultaMesaAbierta(string Id)
        {
            List<ConsultaSolicitudGeneral> ConsultaMesa = new List<ConsultaSolicitudGeneral>();
            ConsultaMesa = solicitud.ConsultaSolicitudMesa(Convert.ToDecimal(Id));
            Clients.All.ListaDetallesMesa(ConsultaMesa);
        }
        public void ConsultaComposicionSubProducto(decimal idProducto)
        {
            List<TBL_COMPOSICION_SUBPRODUCTOS> list = new List<TBL_COMPOSICION_SUBPRODUCTOS>();
            list = solicitud.ComposicionSubProductos(idProducto);
            Clients.All.ListaCompoSubProdu(list);
        }


    }
}