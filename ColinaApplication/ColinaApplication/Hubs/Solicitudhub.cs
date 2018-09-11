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
        public void InsertaProductosSolicitud(List<TBL_PRODUCTOS_SOLICITUD> list1, List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD> list2, decimal valorDescontar, string IdMesa)
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

            List<ActualizarProductos> lista = new List<ActualizarProductos>();
            foreach (var item in list2)
            {
                var llave = "";
                decimal? valorrestar = 0;
                if(item.DESCRIPCION != null && item.DESCRIPCION != "")
                {
                    if ((item.DESCRIPCION == "CHURRASCO 350 GR") || (item.DESCRIPCION == "CARNE BABY BEEF 350 GR"))
                    {
                        llave = "TABLA SUBPRODUCTOS";
                        valorrestar = 1;
                    }
                    else
                    {
                        llave = "TABLA PRECIOS SUBPRODUCTOS";
                        valorrestar = null;
                    }
                    lista.Add(new ActualizarProductos { Id = Convert.ToDecimal(list1[0].ID_SUBPRODUCTO), Llave = llave, Descripcion = item.DESCRIPCION, ValorRestar = valorrestar });
                }
            }
            var respuesta2 = solicitud.ActualizaCantidadSubProducto(lista);

            Clients.Caller.GuardoProductos(respuesta);
            Clients.All.ActualizaCantidadProductos(respuesta2);
            ConsultaMesaAbierta(IdMesa);
        }

    }
}