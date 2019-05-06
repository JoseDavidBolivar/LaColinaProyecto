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
                var ConsultaSolicitud = context.TBL_SOLICITUD.Where(a=> a.ID_MESA == IdMesa).ToList().LastOrDefault();
                if(ConsultaSolicitud != null)
                {
                    var lista = context.TBL_PRODUCTOS_SOLICITUD.Where(a => a.ID_SOLICITUD == ConsultaSolicitud.ID).ToList();
                    var total = lista.Sum(a=>a.PRECIO_FINAL);
                    solicitudMesa.Add(new ConsultaSolicitudGeneral
                    {
                        Id = ConsultaSolicitud.ID,
                        FechaSolicitud = ConsultaSolicitud.FECHA_SOLICITUD,
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
                    Respuesta = "Solicitud Insertada exitosamente";
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
        public List<TBL_PRECIOS_SUBPRODUCTOS> ListaPreciosSubproductos(decimal IdSubProducto)
        {
            List<TBL_PRECIOS_SUBPRODUCTOS> list = new List<TBL_PRECIOS_SUBPRODUCTOS>();
            using (DBLaColina contex = new DBLaColina())
            {
                list = contex.TBL_PRECIOS_SUBPRODUCTOS.Where(a => a.ID_SUBPRODUCTO == IdSubProducto).ToList();
            }
            return list;
        }
        public TBL_SUBPRODUCTOS InsertaProductos(List<TBL_PRODUCTOS_SOLICITUD> list1, List<List<TBL_COMPOSICION_PRODUCTOS_SOLICITUD>> list2)
        {
            TBL_SUBPRODUCTOS respuesta = new TBL_SUBPRODUCTOS();
            using (DBLaColina context = new DBLaColina())
            {
                try
                {
                    foreach (var item in list2)
                    {
                        decimal? precioFinal = 0;
                        foreach (var item2 in item)
                        {
                            precioFinal = precioFinal + item2.VALOR;
                        }
                        
                        TBL_PRODUCTOS_SOLICITUD model1 = new TBL_PRODUCTOS_SOLICITUD();
                        model1.FECHA_REGISTRO = DateTime.Now;
                        model1.ID_SOLICITUD = list1[0].ID_SOLICITUD;
                        model1.ID_SUBPRODUCTO = list1[0].ID_SUBPRODUCTO;
                        model1.ID_MESERO = list1[0].ID_MESERO;
                        model1.PRECIO_PRODUCTO = list1[0].PRECIO_PRODUCTO;
                        model1.PRECIO_FINAL = precioFinal;
                        model1.ESTADO_PRODUCTOS = list1[0].ESTADO_PRODUCTOS;

                        context.TBL_PRODUCTOS_SOLICITUD.Add(model1);
                        context.SaveChanges();

                        foreach (var item3 in item)
                        {
                            TBL_COMPOSICION_PRODUCTOS_SOLICITUD model2 = new TBL_COMPOSICION_PRODUCTOS_SOLICITUD();

                            model2.ID_PRODUCTO_SOLICITUD = model1.ID;
                            model2.DESCRIPCION = item3.DESCRIPCION;
                            model2.VALOR = item3.VALOR;
                            
                            context.TBL_COMPOSICION_PRODUCTOS_SOLICITUD.Add(model2);

                            TBL_PRECIOS_SUBPRODUCTOS consulta = new TBL_PRECIOS_SUBPRODUCTOS();
                            consulta = context.TBL_PRECIOS_SUBPRODUCTOS.Where(a => a.ID_SUBPRODUCTO == 7 && a.DESCRIPCION == model2.DESCRIPCION).FirstOrDefault();
                            if (consulta != null)
                            {
                                consulta.CANTIDAD_PORCION = consulta.CANTIDAD_PORCION - consulta.VALOR_MEDIDA;

                                TBL_SOLICITUD solicitud = new TBL_SOLICITUD();
                                solicitud = context.TBL_SOLICITUD.FirstOrDefault(a=>a.ID == model1.ID_SOLICITUD);
                                solicitud.TOTAL = solicitud.TOTAL + solicitud.OTROS_COBROS - solicitud.DESCUENTOS + model2.VALOR;
                                context.SaveChanges();

                            }
                            else
                            {
                                TBL_PRECIOS_SUBPRODUCTOS consulta2 = new TBL_PRECIOS_SUBPRODUCTOS();
                                consulta2 = context.TBL_PRECIOS_SUBPRODUCTOS.Where(a => a.DESCRIPCION == model2.DESCRIPCION).FirstOrDefault();
                                if (consulta2 != null)
                                {
                                    TBL_SUBPRODUCTOS RegistroActualizar = new TBL_SUBPRODUCTOS();
                                    RegistroActualizar = context.TBL_SUBPRODUCTOS.FirstOrDefault(a=>a.ID == model1.ID_SUBPRODUCTO);
                                    if (RegistroActualizar != null)
                                    {
                                        RegistroActualizar.CANTIDAD_EXISTENCIA = RegistroActualizar.CANTIDAD_EXISTENCIA - consulta2.VALOR_MEDIDA;
                                    }

                                    TBL_SOLICITUD solicitud = new TBL_SOLICITUD();
                                    solicitud = context.TBL_SOLICITUD.FirstOrDefault(a => a.ID == model1.ID_SOLICITUD);
                                    solicitud.TOTAL = solicitud.TOTAL + solicitud.OTROS_COBROS - solicitud.DESCUENTOS + model2.VALOR;
                                    context.SaveChanges();
                                }
                            }
                        }

                        context.SaveChanges();
                        respuesta = context.TBL_SUBPRODUCTOS.FirstOrDefault(a=>a.ID == model1.ID_SUBPRODUCTO);
                    }
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
                            item.ESTADO_PRODUCTOS = "CANCELADO";
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
    }
}