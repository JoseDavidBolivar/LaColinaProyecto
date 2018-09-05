using ColinaApplication.Data.Business;
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
        public void InsertaProductosSolicitud(List<TBL_PRODUCTOS_SOLICITUD> list1, List<object> list2)
        {
            List<List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD>> lista = new List<List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD>>();
            var fila = ((IEnumerable)list2).Cast<object>().ToList();

            

            for (int i = 0; i < fila.Count; i++)
            {
                var Vector = ((IEnumerable)fila[i]).Cast<object>().ToList();
                //string adaptador = "{\"TBL_COMPOSICION_PRODUCTOS_SOLICITUD\":" + JsonConvert.SerializeObject(fila[i]+"}");
                
                //var result = JsonConvert.DeserializeObject<TBL_COMPOSICION_PRODUCTOS_SOLICITUD>(adaptador);


                //var IdProdSolicitud = result.ID_PRODUCTO_SOLICITUD;
                //lista.Add(new List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD> { new TBL_COMPOSICION_PRODUCTOS_SOLICITUD { ID_PRODUCTO_SOLICITUD = IdProdSolicitud} });

            }

            //solicitud.InsertaProductos(list1, result);
            Clients.All.GuardoProductos();
        }

    }
}