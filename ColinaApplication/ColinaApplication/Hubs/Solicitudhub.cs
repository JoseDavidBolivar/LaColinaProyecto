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
        public void ActualizaMesa(string id)
        {

        }
        public void ListaMesas()
        {
            ListarEstadoMesas();
        }
        public void ListarEstadoMesas()
        {
            List<TBL_MASTER_MESAS> listamesas = new List<TBL_MASTER_MESAS>();
            listamesas = solicitud.ListaMesas();
            Clients.All.ListaMesas(listamesas);
        }


    }
}