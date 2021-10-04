using ColinaApplication.Data.Conexion;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Business
{
    public class VentasBusiness
    {
        public TBL_CIERRES CierreUsuarioId(decimal? idusuario)
        {
            TBL_CIERRES UltimoCierre = new TBL_CIERRES();
            using (DBLaColina context = new DBLaColina())
            {
                UltimoCierre = context.TBL_CIERRES.Where(x => x.ID_USUARIO == idusuario).ToList().LastOrDefault();
            }
            return UltimoCierre;
        }

        public List<TBL_MASTER_MESAS> ConsultaMesasCargo(decimal? idusuario)
        {
            List<TBL_MASTER_MESAS> mesasACargo = new List<TBL_MASTER_MESAS>();
            using (DBLaColina context = new DBLaColina())
            {
                mesasACargo = context.TBL_MASTER_MESAS.Where(x => x.ID_USUARIO == idusuario).ToList();
            }
            return mesasACargo;
        }
        public bool ActualizaEstadoMesa(TBL_MASTER_MESAS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_MASTER_MESAS actualiza = new TBL_MASTER_MESAS();
                    actualiza = contex.TBL_MASTER_MESAS.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.ESTADO = model.ESTADO;
                        contex.SaveChanges();
                        Respuesta = true;
                    }
                }
                catch (Exception e)
                {
                    Respuesta = false;
                }
            }
            return Respuesta;
        }
        public bool InsertaNuevoCierre(TBL_CIERRES model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_CIERRES.Add(model);
                    contex.SaveChanges();
                    Respuesta = true;
                }
                catch (Exception e)
                {
                    Respuesta = false;
                }
            }
            return Respuesta;
        }
        public bool ActualizaCierre(TBL_CIERRES model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_CIERRES actualiza = new TBL_CIERRES();
                    actualiza = contex.TBL_CIERRES.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.FECHA_HORA_CIERRE = model.FECHA_HORA_CIERRE;
                        actualiza.CANT_MESAS_ATENDIDAS = model.CANT_MESAS_ATENDIDAS;
                        actualiza.CANT_FINALIZADAS = model.CANT_FINALIZADAS;
                        actualiza.TOTAL_FINALIZADAS = model.TOTAL_FINALIZADAS;
                        actualiza.CANT_LLEVAR = model.CANT_LLEVAR;
                        actualiza.TOTAL_LLEVAR = model.TOTAL_LLEVAR;
                        actualiza.CANT_CANCELADAS = model.CANT_CANCELADAS;
                        actualiza.TOTAL_CANCELADAS = model.TOTAL_CANCELADAS;
                        actualiza.CANT_CONSUMO_INTERNO = model.CANT_CONSUMO_INTERNO;
                        actualiza.TOTAL_CONSUMO_INTERNO = model.TOTAL_CONSUMO_INTERNO;
                        actualiza.SERVICIO_TOTAL = model.SERVICIO_TOTAL;
                        actualiza.TOTAL_EFECTIVO = model.TOTAL_EFECTIVO;
                        actualiza.TOTAL_TARJETA = model.TOTAL_TARJETA;
                        actualiza.VENTA_TOTAL = model.VENTA_TOTAL;
                        contex.SaveChanges();
                        Respuesta = true;
                    }
                }
                catch (Exception e)
                {
                    Respuesta = false;
                }
            }
            return Respuesta;
        }
        public List<TBL_SOLICITUD> ConsultaSolicitudes(decimal? idusuario, DateTime? fechaApertura)
        {
            List<TBL_SOLICITUD> solicitudes = new List<TBL_SOLICITUD>();
            using (DBLaColina context = new DBLaColina())
            {
                var mesasACargos = context.TBL_MASTER_MESAS.Where(x => x.ID_USUARIO == idusuario).Select(y => y.ID).ToList();
                solicitudes = context.TBL_SOLICITUD.Where(x => mesasACargos.Any(w => x.ID_MESA == w) && x.FECHA_SOLICITUD >= fechaApertura).ToList();
            }
            return solicitudes;
        }
        public List<ConsultaSolicitud> ConsultaSolicitudesXFecha(DateTime FechaInicial, DateTime FechaFinal)
        {
            List<ConsultaSolicitud> solicitudes = new List<ConsultaSolicitud>();
            using (DBLaColina context = new DBLaColina())
            {
                solicitudes = (from a in context.TBL_SOLICITUD
                               where a.FECHA_SOLICITUD >= FechaInicial && a.FECHA_SOLICITUD <= FechaFinal
                               select new ConsultaSolicitud { 
                                   NroFactura = a.ID,
                                   FechaSolicitud = a.FECHA_SOLICITUD,
                                   NumeroMesa = context.TBL_MASTER_MESAS.Where(x => x.ID == a.ID_MESA).FirstOrDefault().NUMERO_MESA,
                                   NombreMesa = context.TBL_MASTER_MESAS.Where(x => x.ID == a.ID_MESA).FirstOrDefault().NOMBRE_MESA,
                                   IdMesero = context.TBL_USUARIOS.Where(x => x.ID == a.ID_MESERO).FirstOrDefault().ID,
                                   NombreMesero = context.TBL_USUARIOS.Where(x => x.ID == a.ID_MESERO).FirstOrDefault().NOMBRE,
                                   IdCliente = a.IDENTIFICACION_CLIENTE,
                                   NombreCliente = a.NOMBRE_CLIENTE,
                                   EstadoSolicitud = a.ESTADO_SOLICITUD,
                                   Observaciones = a.OBSERVACIONES,
                                   OtrosCobros = a.OTROS_COBROS,
                                   Descuentos = a.DESCUENTOS,
                                   Subtotal = a.SUBTOTAL,
                                   PorcentajeIVA = a.PORCENTAJE_IVA,
                                   IVATotal = a.IVA_TOTAL,
                                   PorcentajeIConsumo = a.PORCENTAJE_I_CONSUMO,
                                   IConsumoTotal = a.I_CONSUMO_TOTAL,
                                   PorcentajeServicio = a.PORCENTAJE_SERVICIO,
                                   ServicioTotal = a.SERVICIO_TOTAL,
                                   Total = a.TOTAL,
                                   MetodoPago = a.METODO_PAGO,
                                   Voucher = a.VOUCHER
                               }).ToList();
                var idSolicitudes = solicitudes.Select(x => x.NroFactura).ToList();
                var productosSolicitud = context.TBL_PRODUCTOS_SOLICITUD.Where(x => idSolicitudes.Any(y => x.ID_SOLICITUD == y)).ToList();
                if (productosSolicitud.Count > 0)
                {
                    solicitudes[0].ProductosSolicitud = new List<ProductosSolicitud>();
                    foreach (var item in productosSolicitud)
                    {
                        solicitudes[0].ProductosSolicitud.Add(new ProductosSolicitud
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
                    
                }
                else
                {
                    solicitudes[0].ProductosSolicitud = new List<ProductosSolicitud>();
                }
            }
            return solicitudes;
        }
        public ConsultaSolicitud ConsultaSolicitudXId(decimal Id)
        {
            ConsultaSolicitud solicitud = new ConsultaSolicitud();
            using (DBLaColina context = new DBLaColina())
            {
                solicitud = (from a in context.TBL_SOLICITUD
                            where a.ID == Id
                            select new ConsultaSolicitud
                            {
                                NroFactura = a.ID,
                                FechaSolicitud = a.FECHA_SOLICITUD,
                                NumeroMesa = context.TBL_MASTER_MESAS.Where(x => x.ID == a.ID_MESA).FirstOrDefault().NUMERO_MESA,
                                NombreMesa = context.TBL_MASTER_MESAS.Where(x => x.ID == a.ID_MESA).FirstOrDefault().NOMBRE_MESA,
                                IdMesero = context.TBL_USUARIOS.Where(x => x.ID == a.ID_MESERO).FirstOrDefault().ID,
                                NombreMesero = context.TBL_USUARIOS.Where(x => x.ID == a.ID_MESERO).FirstOrDefault().NOMBRE,
                                IdCliente = a.IDENTIFICACION_CLIENTE,
                                NombreCliente = a.NOMBRE_CLIENTE,
                                EstadoSolicitud = a.ESTADO_SOLICITUD,
                                Observaciones = a.OBSERVACIONES,
                                OtrosCobros = a.OTROS_COBROS,
                                Descuentos = a.DESCUENTOS,
                                Subtotal = a.SUBTOTAL,
                                PorcentajeIVA = a.PORCENTAJE_IVA,
                                IVATotal = a.IVA_TOTAL,
                                PorcentajeIConsumo = a.PORCENTAJE_I_CONSUMO,
                                IConsumoTotal = a.I_CONSUMO_TOTAL,
                                PorcentajeServicio = a.PORCENTAJE_SERVICIO,
                                ServicioTotal = a.SERVICIO_TOTAL,
                                Total = a.TOTAL,
                                MetodoPago = a.METODO_PAGO,
                                Voucher = a.VOUCHER
                            }).FirstOrDefault();
                var productosSolicitud = context.TBL_PRODUCTOS_SOLICITUD.Where(x => x.ID_SOLICITUD == solicitud.NroFactura).ToList();
                if (productosSolicitud.Count > 0)
                {
                    solicitud.ProductosSolicitud = new List<ProductosSolicitud>();
                    foreach (var item in productosSolicitud)
                    {
                        solicitud.ProductosSolicitud.Add(new ProductosSolicitud
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

                }
                else
                {
                    solicitud.ProductosSolicitud = new List<ProductosSolicitud>();
                }
            }
            return solicitud;
        }
    }
}