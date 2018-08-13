﻿using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Business
{
    public class SolicitudBsuiness
    {
        public List<TBL_MASTER_MESAS> ListaMesas()
        {
            List<TBL_MASTER_MESAS> ListMesas = new List<TBL_MASTER_MESAS>();
            using (DBLaColina context = new DBLaColina())
            {
                ListMesas = context.TBL_MASTER_MESAS.ToList();
            }

            return ListMesas;
        }
        public List<ConsultaSolicitudGeneral> ConsultaSolicitudMesa(decimal IdMesa)
        {
            List<ConsultaSolicitudGeneral> solicitudMesa = new List<ConsultaSolicitudGeneral>();
            using (DBLaColina context = new DBLaColina())
            {
                var ConsultaSolicitud = context.TBL_SOLICITUD.Where(a=> a.ID_MESA == IdMesa).ToList().LastOrDefault();
                if(ConsultaSolicitud != null)
                {
                    solicitudMesa.Add(new ConsultaSolicitudGeneral
                    {
                        Id = ConsultaSolicitud.ID,
                        FechaSolicitud = ConsultaSolicitud.FECHA_SOLICITUD,
                        IdMesa = ConsultaSolicitud.ID_MESA,
                        NombreMesa = context.TBL_MASTER_MESAS.Where(z => z.ID == IdMesa).FirstOrDefault().NOMBRE_MESA,
                        IdMesero = ConsultaSolicitud.ID_MESERO,
                        NombreMesero = context.TBL_USUARIOS.Where(a=>a.ID == ConsultaSolicitud.ID_MESERO).FirstOrDefault().NOMBRE,
                        IdentificacionCliente = ConsultaSolicitud.IDENTIFICACION_CLIENTE,
                        NombreCliente = ConsultaSolicitud.NOMBRE_CLIENTE,
                        EstadoSolicitud = ConsultaSolicitud.ESTADO_SOLICITUD,
                        Observaciones = ConsultaSolicitud.OBSERVACIONES,
                        ProductosSolicitud = new List<ProductosSolicitud>()

                    });                    
                    var ConsultaProductosSolicitud = context.TBL_PRODUCTOS_SOLICITUD.Where(b=> b.ID_SOLICITUD == ConsultaSolicitud.ID).ToList();
                    if(ConsultaProductosSolicitud != null)
                    {
                        var count = 0;
                        foreach (var item in ConsultaProductosSolicitud)
                        {
                            try
                            {
                                solicitudMesa[0].ProductosSolicitud.Add(new ProductosSolicitud
                                {
                                    Id = item.ID,
                                    FechaRegistro = item.FECHA_REGISTRO,
                                    IdSolicitud = item.ID_SOLICITUD,
                                    IdSubProducto = item.ID_SUBPRODUCTO,
                                    NombreSubProducto = context.TBL_SUBPRODUCTOS.Where(a=> a.ID == item.ID_SUBPRODUCTO).FirstOrDefault().NOMBRE_SUBPRODUCTO,
                                    IdMesero = item.ID_MESERO,
                                    NombreMesero = context.TBL_USUARIOS.Where(a => a.ID == item.ID_MESERO).FirstOrDefault().NOMBRE,
                                    PrecioProducto = item.PRECIO_PRODUCTO,
                                    PrecioFinal = item.PRECIO_FINAL,
                                    EstadoProductos = item.ESTADO_PRODUCTOS,
                                    CompoProductSolicitud = new List<ComposiconProductosSolicitud>()

                                });
                                
                                var CompProdSolicitud = context.TBL_COMPOSICION_PRODUCTOS_SOLICITUD.Where(c => c.ID_PRODUCTO_SOLICITUD == item.ID).ToList();
                                for (int i = 0; i < CompProdSolicitud.Count; i++)
                                {
                                    solicitudMesa[0].ProductosSolicitud[count].CompoProductSolicitud.Add(
                                    new ComposiconProductosSolicitud
                                    {
                                        Id = CompProdSolicitud[i].ID,
                                        IdProductoSolicitud = CompProdSolicitud[i].ID_PRODUCTO_SOLICITUD,
                                        Descripcion = CompProdSolicitud[i].DESCRIPCION,
                                        Valor = CompProdSolicitud[i].VALOR
                                    });
                                }
                                count++;
                            }
                            catch (Exception E)
                            {
                                
                            }
                        }
                    }
                }
            }
            
            return solicitudMesa;
        }
        public void ActualizaEstadoMesa (decimal Id, string Estado)
        {
            using (DBLaColina contex = new DBLaColina())
            {
                TBL_MASTER_MESAS modelActualizar = new TBL_MASTER_MESAS();
                modelActualizar = contex.TBL_MASTER_MESAS.FirstOrDefault(a=> a.ID == Id);

                if(modelActualizar != null)
                {
                    modelActualizar.ESTADO = Estado;
                    contex.SaveChanges();
                }
            }
        }
        public string InsertaSolicitud (TBL_SOLICITUD model)
        {
            string Respuesta = "";
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    model.FECHA_SOLICITUD = DateTime.Now;
                    model.ESTADO_SOLICITUD = Estados.Abierta;
                    contex.TBL_SOLICITUD.Add(model);
                    contex.SaveChanges();
                    Respuesta = "Solicitud Insertado exitosamente";
                }
                catch (Exception e)
                {
                    Respuesta = "Error Servidor: " + e;
                }
                
            }
            return Respuesta;
        }
        public List<TBL_PRODUCTOS> ListaProductos()
        {
            List<TBL_PRODUCTOS> listproductos = new List<TBL_PRODUCTOS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listproductos = contex.TBL_PRODUCTOS.Where(a => a.ESTADO == Estados.Activo).ToList();
            }
            return listproductos;
        }
        public List<TBL_SUBPRODUCTOS> ListaSubProductos(decimal IdProducto)
        {
            List<TBL_SUBPRODUCTOS> listSubproductos = new List<TBL_SUBPRODUCTOS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listSubproductos = contex.TBL_SUBPRODUCTOS.Where(a => a.ID_PRODUCTO == IdProducto).ToList();
            }
            return listSubproductos;
        }
        public List<TBL_COMPOSICION_SUBPRODUCTOS> ComposicionSubProductos(decimal IdSubProducto)
        {
            List<TBL_COMPOSICION_SUBPRODUCTOS> listComposicion = new List<TBL_COMPOSICION_SUBPRODUCTOS>();
            using (DBLaColina context = new DBLaColina())
            {
                listComposicion = context.TBL_COMPOSICION_SUBPRODUCTOS.Where(a=>a.ID_SUBPRODUCTO == IdSubProducto).ToList();
            }

            return listComposicion;
        }
        public TBL_SUBPRODUCTOS ElementoInventario(decimal Id)
        {
            TBL_SUBPRODUCTOS subrpoducto = new TBL_SUBPRODUCTOS();
            using (DBLaColina context = new DBLaColina())
            {
                subrpoducto = context.TBL_SUBPRODUCTOS.Where(a=>a.ID == Id).ToList().LastOrDefault();
            }

            return subrpoducto;
        }
    }
}