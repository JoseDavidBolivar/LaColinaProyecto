using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
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
                        Subtotal = ConsultaSolicitud.SUBTOTAL,
                        PorcentajeIVA = ConsultaSolicitud.PORCENTAJE_IVA,
                        IVATotal = ConsultaSolicitud.IVA_TOTAL,
                        PorcentajeIConsumo = ConsultaSolicitud.PORCENTAJE_I_CONSUMO,
                        IConsumoTotal = ConsultaSolicitud.I_CONSUMO_TOTAL,
                        PorcentajeServicio = ConsultaSolicitud.PORCENTAJE_SERVICIO,
                        ServicioTotal = ConsultaSolicitud.SERVICIO_TOTAL,
                        Total = ConsultaSolicitud.TOTAL,
                        ProductosSolicitud = new List<ProductosSolicitud>(),
                        Impuestos = new List<Impuestos>()

                    });
                    var ConsultaProductosSolicitud = context.TBL_PRODUCTOS_SOLICITUD.Where(b => b.ID_SOLICITUD == ConsultaSolicitud.ID && b.ESTADO_PRODUCTO != Estados.Cancelado).ToList();
                    if (ConsultaProductosSolicitud.Count > 0)
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
                    var ConsultaImpuesto = context.TBL_IMPUESTOS.ToList();
                    if (ConsultaImpuesto.Count > 0)
                    {
                        foreach (var item in ConsultaImpuesto)
                        {
                            solicitudMesa[0].Impuestos.Add(new Impuestos
                            {
                                Id = item.ID,
                                NombreImpuesto = item.NOMBRE_IMPUESTO,
                                Porcentaje = item.PORCENTAJE,
                                Estado = item.ESTADO
                            });
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
                    model.IDENTIFICACION_CLIENTE = "";
                    model.NOMBRE_CLIENTE = "";
                    model.PORCENTAJE_IVA = contex.TBL_IMPUESTOS.Where(x => x.ID == 1 && x.ESTADO == Estados.Activo).FirstOrDefault() != null ? contex.TBL_IMPUESTOS.Where(x => x.ID == 1).FirstOrDefault().PORCENTAJE : 0;
                    model.PORCENTAJE_I_CONSUMO = contex.TBL_IMPUESTOS.Where(x => x.ID == 2 && x.ESTADO == Estados.Activo).FirstOrDefault() != null ? contex.TBL_IMPUESTOS.Where(x => x.ID == 2).FirstOrDefault().PORCENTAJE : 0;
                    model.PORCENTAJE_SERVICIO = contex.TBL_IMPUESTOS.Where(x => x.ID == 3 && x.ESTADO == Estados.Activo).FirstOrDefault() != null ? contex.TBL_IMPUESTOS.Where(x => x.ID == 3).FirstOrDefault().PORCENTAJE : 0;
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
        public TBL_PRODUCTOS_SOLICITUD InsertaProductosSolicitud(TBL_PRODUCTOS_SOLICITUD model)
        {
            TBL_PRODUCTOS_SOLICITUD respuesta = new TBL_PRODUCTOS_SOLICITUD();
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
                    var r = context.SaveChanges();
                    respuesta = modelo;

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
                        actualiza.ID_MESA = model.ID_MESA;
                        actualiza.IDENTIFICACION_CLIENTE = model.IDENTIFICACION_CLIENTE;
                        actualiza.NOMBRE_CLIENTE = model.NOMBRE_CLIENTE;
                        actualiza.ESTADO_SOLICITUD = model.ESTADO_SOLICITUD;
                        actualiza.OBSERVACIONES = model.OBSERVACIONES;
                        actualiza.OTROS_COBROS = model.OTROS_COBROS;
                        actualiza.DESCUENTOS = model.DESCUENTOS;
                        actualiza.SUBTOTAL = model.SUBTOTAL;
                        actualiza.PORCENTAJE_SERVICIO = model.PORCENTAJE_SERVICIO;
                        actualiza.SERVICIO_TOTAL = (model.SUBTOTAL * actualiza.PORCENTAJE_SERVICIO) / 100;
                        actualiza.TOTAL = actualiza.SUBTOTAL + actualiza.IVA_TOTAL + actualiza.I_CONSUMO_TOTAL + actualiza.SERVICIO_TOTAL + actualiza.OTROS_COBROS - actualiza.DESCUENTOS;
                        actualiza.METODO_PAGO = model.METODO_PAGO;
                        actualiza.VOUCHER = model.VOUCHER;
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
        public string ActualizaTotalSolicitud(decimal? Id, decimal? SubTotal)
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
                        actualiza.SUBTOTAL += SubTotal;
                        actualiza.IVA_TOTAL = (actualiza.SUBTOTAL * actualiza.PORCENTAJE_IVA) / 100;
                        actualiza.I_CONSUMO_TOTAL = (actualiza.SUBTOTAL * actualiza.PORCENTAJE_I_CONSUMO) / 100;
                        actualiza.SERVICIO_TOTAL = (actualiza.SUBTOTAL * actualiza.PORCENTAJE_SERVICIO) / 100;
                        actualiza.TOTAL = actualiza.SUBTOTAL + actualiza.IVA_TOTAL + actualiza.I_CONSUMO_TOTAL + actualiza.SERVICIO_TOTAL;
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
        public string CancelaProductosSolicitud(decimal IdSolicitud, bool RetornaInventario)
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
                            item.ESTADO_PRODUCTO = Estados.Cancelado;
                            contex.SaveChanges();
                            if (RetornaInventario)
                                ActualizaCantidadProducto(item.ID_PRODUCTO, (ConsultaCantidadProducto(item.ID_PRODUCTO) + 1));
                        };
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
        public string CancelaProductoSolicitudXId(decimal IdProductoSolicitud, bool RetornaInventario)
        {
            string Respuesta = "";
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_PRODUCTOS_SOLICITUD actualiza = new TBL_PRODUCTOS_SOLICITUD();
                    actualiza = contex.TBL_PRODUCTOS_SOLICITUD.Where(a => a.ID == IdProductoSolicitud).FirstOrDefault();
                    if (actualiza.ID > 0)
                    {
                        actualiza.ESTADO_PRODUCTO = Estados.Cancelado;
                        contex.SaveChanges();
                        if (RetornaInventario)
                            ActualizaCantidadProducto(actualiza.ID_PRODUCTO, (ConsultaCantidadProducto(actualiza.ID_PRODUCTO) + 1));
                        Respuesta = ActualizaTotalSolicitud(actualiza.ID_SOLICITUD, -actualiza.PRECIO_PRODUCTO);

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
        public bool ImprimirFactura(string idSolicitud)
        {
            bool respuesta;
            PrintDocument printDocument1 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            printDocument1.PrinterSettings.PrinterName = "IMPRESORA DONDE DEBE SALIR";
            printDocument1.PrintPage += ImprimirFact;
            printDocument1.Print();
            respuesta = true;
            return respuesta;
        }
        public void ImprimirFact(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 14);
            int ancho = 150;
            int y = 20;

            e.Graphics.DrawString("----- La Colina Restaurante Campestre ------", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            e.Graphics.DrawString(" Orden # 123", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            e.Graphics.DrawString(" Cliente: Juan Lopez ", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            e.Graphics.DrawString("Productos ", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            //lista los productos
            //foreach (var item in collection)
            //{

            //}
            e.Graphics.DrawString("Subtotal ", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            e.Graphics.DrawString("Impuestos ", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            e.Graphics.DrawString("Productos ", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
        }
        public bool ImprimirPedido(string cantidad, string idproducto, string descripcion)
        {
            bool respuesta;
            PrintDocument printDocument1 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            printDocument1.PrinterSettings.PrinterName = "IMPRESORA DONDE DEBE SALIR";
            printDocument1.PrintPage += ImprimirPed;
            //printDocument1.Print();
            respuesta = true;
            return respuesta;
        }
        public void ImprimirPed(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 14);
            int ancho = 150;
            int y = 20;

            e.Graphics.DrawString(" Mesero: ", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            e.Graphics.DrawString(" Mesa: ", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            e.Graphics.DrawString(" --- Productos ---", font, Brushes.Black, new RectangleF(0, y + 20, ancho, 20));
            //lista los productos
            //foreach (var item in collection)
            //{

            //}

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