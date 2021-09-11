using ColinaApplication.Data.Clases;
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
                var ConsultaSolicitud = context.TBL_SOLICITUD.Where(a => a.ID_MESA == IdMesa).ToList().LastOrDefault();
                if (ConsultaSolicitud != null)
                {
                    var lista = context.TBL_PRODUCTOS_SOLICITUD.Where(a => a.ID_SOLICITUD == ConsultaSolicitud.ID).ToList();
                    var total = lista.Sum(a => a.PRECIO_PRODUCTO);
                    solicitudMesa.Add(new ConsultaSolicitudGeneral
                    {
                        Id = ConsultaSolicitud.ID,
                        //FechaSolicitud = ConsultaSolicitud.FECHA_SOLICITUD,
                        IdMesa = ConsultaSolicitud.ID_MESA,
                        NombreMesa = context.TBL_MASTER_MESAS.Where(z => z.ID == IdMesa).FirstOrDefault().NOMBRE_MESA,
                        IdMesero = ConsultaSolicitud.ID_MESERO,
                        NombreMesero = context.TBL_USUARIOS.Where(a => a.ID == ConsultaSolicitud.ID_MESERO).FirstOrDefault().NOMBRE,
                        IdentificacionCliente = ConsultaSolicitud.IDENTIFICACION_CLIENTE,
                        NombreCliente = ConsultaSolicitud.NOMBRE_CLIENTE,
                        EstadoSolicitud = ConsultaSolicitud.ESTADO_SOLICITUD,
                        Observaciones = ConsultaSolicitud.OBSERVACIONES,
                        OtrosCobros = ConsultaSolicitud.OTROS_COBROS,
                        Descuentos = ConsultaSolicitud.DESCUENTOS,
                        Total = total,
                        ProductosSolicitud = new List<ProductosSolicitud>()

                    });
                    var ConsultaProductosSolicitud = context.TBL_PRODUCTOS_SOLICITUD.Where(b => b.ID_SOLICITUD == ConsultaSolicitud.ID).ToList();
                    if (ConsultaProductosSolicitud != null)
                    {
                        foreach (var item in ConsultaProductosSolicitud)
                        {
                            try
                            {
                                solicitudMesa[0].ProductosSolicitud.Add(new ProductosSolicitud
                                {
                                    Id = item.ID,
                                    FechaRegistro = item.FECHA_REGISTRO,
                                    IdSolicitud = item.ID_SOLICITUD,
                                    IdProducto = item.ID_PRODUCTO,
                                    NombreProducto = context.TBL_PRODUCTOS.Where(a => a.ID == item.ID_PRODUCTO).FirstOrDefault().NOMBRE_PRODUCTO,
                                    IdMesero = item.ID_MESERO,
                                    NombreMesero = context.TBL_USUARIOS.Where(a => a.ID == item.ID_MESERO).FirstOrDefault().NOMBRE,
                                    PrecioProducto = item.PRECIO_PRODUCTO,
                                    EstadoProducto = item.ESTADO_PRODUCTO,
                                    Descripcion = item.DESCRIPCION

                                });
                            }
                            catch (Exception E)
                            {
                                throw E;
                            }
                        }
                    }
                }
            }

            return solicitudMesa;
        }
        public void ActualizaEstadoMesa(decimal Id, string Estado)
        {
            using (DBLaColina contex = new DBLaColina())
            {
                TBL_MASTER_MESAS modelActualizar = new TBL_MASTER_MESAS();
                modelActualizar = contex.TBL_MASTER_MESAS.FirstOrDefault(a => a.ID == Id);

                if (modelActualizar != null)
                {
                    modelActualizar.ESTADO = Estado;
                    contex.SaveChanges();
                }
            }
        }
        public string InsertaSolicitud(TBL_SOLICITUD model)
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
                    Respuesta = "Solicitud Insertada exitosamente";
                }
                catch (Exception e)
                {
                    Respuesta = "Error Servidor: " + e;
                }

            }
            return Respuesta;
        }
        public List<TBL_CATEGORIAS> ListaCategorias()
        {
            List<TBL_CATEGORIAS> listproductos = new List<TBL_CATEGORIAS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listproductos = contex.TBL_CATEGORIAS.Where(x => x.ESTADO.Equals(Estados.Activo)).ToList();
            }
            return listproductos;
        }
        public List<TBL_PRODUCTOS> ListaProductos(decimal IdProducto)
        {
            List<TBL_PRODUCTOS> listProductos = new List<TBL_PRODUCTOS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listProductos = contex.TBL_PRODUCTOS.Where(a => a.ID_CATEGORIA == IdProducto).ToList();
            }
            return listProductos;
        }
        public decimal ConsultaCantidadProducto(decimal? idProducto)
        {
            decimal? CantidadDisponible;
            using (DBLaColina contex = new DBLaColina())
            {
                var busquedaProducto = contex.TBL_PRODUCTOS.Where(x => x.ID == idProducto).FirstOrDefault();
                CantidadDisponible = busquedaProducto.CANTIDAD;
            }
            return Convert.ToInt32(CantidadDisponible);
        }
        public TBL_PRODUCTOS InsertaProductosSolicitud(TBL_PRODUCTOS_SOLICITUD model)
        {
            TBL_PRODUCTOS respuesta = new TBL_PRODUCTOS();
            using (DBLaColina context = new DBLaColina())
            {
                try
                {
                    TBL_PRODUCTOS_SOLICITUD modelo = new TBL_PRODUCTOS_SOLICITUD();
                    modelo.FECHA_REGISTRO = DateTime.Now;
                    modelo.ID_SOLICITUD = model.ID_SOLICITUD;
                    modelo.ID_PRODUCTO = model.ID_PRODUCTO;
                    modelo.ID_MESERO = model.ID_MESERO;
                    modelo.PRECIO_PRODUCTO = model.PRECIO_PRODUCTO;
                    modelo.ESTADO_PRODUCTO = model.ESTADO_PRODUCTO;
                    modelo.DESCRIPCION = model.DESCRIPCION;

                    context.TBL_PRODUCTOS_SOLICITUD.Add(modelo);
                    context.SaveChanges();

                    //respuesta = context.TBL_PRODUCTOS.FirstOrDefault(a => a.ID == model1.ID_SUBPRODUCTO);

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return respuesta;
        }
        public string ActualizaSolicitud(TBL_SOLICITUD model)
        {
            string Respuesta = "";
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_SOLICITUD actualiza = new TBL_SOLICITUD();
                    actualiza = contex.TBL_SOLICITUD.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.IDENTIFICACION_CLIENTE = model.IDENTIFICACION_CLIENTE;
                        actualiza.NOMBRE_CLIENTE = model.NOMBRE_CLIENTE;
                        actualiza.ESTADO_SOLICITUD = model.ESTADO_SOLICITUD;
                        actualiza.OBSERVACIONES = model.OBSERVACIONES;
                        actualiza.OTROS_COBROS = model.OTROS_COBROS;
                        actualiza.DESCUENTOS = model.DESCUENTOS;
                        actualiza.TOTAL = model.TOTAL;
                        contex.SaveChanges();
                        Respuesta = "Solicitud actualizada exitosamente";
                    }
                    else
                    {
                        Respuesta = "No existe la solicitud " + model.ID;
                    }
                }
                catch (Exception e)
                {
                    Respuesta = "Error Servidor: " + e;
                }

            }
            return Respuesta;
        }
        public string ActualizaTotalSolicitud(decimal? Id, decimal? Total)
        {
            string Respuesta = "";
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_SOLICITUD actualiza = new TBL_SOLICITUD();
                    actualiza = contex.TBL_SOLICITUD.Where(a => a.ID == Id).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.TOTAL += Total;
                        contex.SaveChanges();
                        Respuesta = "Total Actualizado exitosamente";
                    }
                    else
                    {
                        Respuesta = "No existe la solicitud " + Id;
                    }
                }
                catch (Exception e)
                {
                    Respuesta = "Error Servidor: " + e;
                }

            }
            return Respuesta;
        }
        public string ActualizaCantidadProducto(decimal? Id, decimal? Total)
        {
            string Respuesta = "";
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_PRODUCTOS actualiza = new TBL_PRODUCTOS();
                    actualiza = contex.TBL_PRODUCTOS.Where(a => a.ID == Id).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.CANTIDAD = Total;
                        contex.SaveChanges();
                        Respuesta = "Total Actualizado exitosamente";
                    }
                    else
                    {
                        Respuesta = "No existe la solicitud " + Id;
                    }
                }
                catch (Exception e)
                {
                    Respuesta = "Error Servidor: " + e;
                }

            }
            return Respuesta;
        }
        public string CancelaProductosSolicitud(decimal IdSolicitud)
        {
            string Respuesta = "";
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    List<TBL_PRODUCTOS_SOLICITUD> actualiza = new List<TBL_PRODUCTOS_SOLICITUD>();
                    actualiza = contex.TBL_PRODUCTOS_SOLICITUD.Where(a => a.ID_SOLICITUD == IdSolicitud).ToList();
                    if (actualiza.Count > 0)
                    {
                        foreach (var item in actualiza)
                        {
                            item.ESTADO_PRODUCTO = "CANCELADO";
                            contex.SaveChanges();
                        };
                        Respuesta = "Productos actualizados exitosamente";
                    }
                    else
                    {
                        Respuesta = "No existe Productos para esta solicitud";
                    }
                }
                catch (Exception e)
                {
                    Respuesta = "Error Servidor: " + e;
                }

            }
            return Respuesta;
        }



        public TBL_PRODUCTOS ElementoInventario(decimal Id)
        {
            TBL_PRODUCTOS subrpoducto = new TBL_PRODUCTOS();
            using (DBLaColina context = new DBLaColina())
            {
                subrpoducto = context.TBL_PRODUCTOS.Where(a => a.ID == Id).ToList().LastOrDefault();
            }

            return subrpoducto;
        }
    }
}