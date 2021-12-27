using ColinaApplication.Data.Clases;
using ColinaApplication.Data.Conexion;
using Entity;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
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
                        actualiza.OTROS_COBROS_TOTAL = model.OTROS_COBROS_TOTAL;
                        actualiza.DESCUENTOS_TOTAL = model.DESCUENTOS_TOTAL;
                        actualiza.IVA_TOTAL = model.IVA_TOTAL;
                        actualiza.I_CONSUMO_TOTAL = model.I_CONSUMO_TOTAL;
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
                                   Voucher = a.VOUCHER,
                                   Efectivo = a.CANT_EFECTIVO
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
                    //solicitudes[0].ProductosSolicitud = new List<ProductosSolicitud>();
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
                if (solicitud != null)
                {
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
                        //solicitud.ProductosSolicitud = new List<ProductosSolicitud>();
                    }
                }
            }
            return solicitud;
        }
        public List<ConsultaNomina> ConsultaNomina()
        {
            List<ConsultaNomina> nomina = new List<ConsultaNomina>();

            using (DBLaColina context = new DBLaColina())
            {
                nomina = (from a in context.TBL_NOMINA
                          where a.ESTADO == "ACTIVO"
                          select new ConsultaNomina
                          {
                              Id = a.ID,
                              IdUsuarioSistema = a.ID_USUARIO_SISTEMA,
                              NombreUsuarioSistema = context.TBL_USUARIOS.Where(x => x.ID == a.ID_USUARIO_SISTEMA).FirstOrDefault().NOMBRE != null ? context.TBL_USUARIOS.Where(x => x.ID == a.ID_USUARIO_SISTEMA).FirstOrDefault().NOMBRE : "N/A",
                              IdPerfil = a.ID_PERFIL,
                              NombrePerfil = context.TBL_PERFIL.Where(x => x.ID == a.ID_PERFIL).FirstOrDefault().NOMBRE_PERFIL,
                              Cedula = a.CEDULA,
                              NombreUsuario = a.NOMBRE,
                              Cargo = a.CARGO,
                              DiasTrabajados = a.DIAS_TRABAJADOS,
                              Propinas = a.PROPINAS,
                              PorcentajeGananciaPropina = context.TBL_PERFIL.Where(x => x.ID == a.ID_PERFIL).FirstOrDefault().PORCENTAJE_PROPINA,
                              FechaPago = a.FECHA_PAGO,
                              FechaNacimmiento = a.FECHA_NACIMIENTO,
                              DireccionResidencia = a.DIRECCION_RESIDENCIA,
                              Telefono = a.TELEFONO,
                              TotalPagar = a.TOTAL_PAGAR,
                              ConsumoInterno = a.CONSUMO_INTERNO
                          }).ToList();
                foreach (var item in nomina)
                {
                    item.FechasAsignadas = context.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == item.Id).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date >= item.FechaPago.Value.Date).Select(x => x.FECHA_TRABAJADO.Value.Date).ToList();
                    item.SuledoDiario = context.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == item.Id).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date >= item.FechaPago.Value.Date).Select(x => x.SUELDO_DIARIO).ToList();
                }
            }
            return nomina;
        }
        public bool AsignaDiaTrabajo(decimal IdUsuarioNomina, DateTime fechaTrabajo, decimal sueldoDiario)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_NOMINA actualiza = new TBL_NOMINA();
                    actualiza = contex.TBL_NOMINA.Where(a => a.ID == IdUsuarioNomina).FirstOrDefault();
                    if (actualiza != null)
                    {
                        TBL_DIAS_TRABAJADOS validacion = new TBL_DIAS_TRABAJADOS();
                        validacion = contex.TBL_DIAS_TRABAJADOS.Where(a => a.ID_USUARIO_NOMINA == IdUsuarioNomina).ToList().Where(a => a.FECHA_TRABAJADO.Value.Date == fechaTrabajo.Date).FirstOrDefault();
                        if (validacion == null)
                        {
                            TBL_DIAS_TRABAJADOS model = new TBL_DIAS_TRABAJADOS();
                            model.ID_USUARIO_NOMINA = IdUsuarioNomina;
                            model.FECHA_TRABAJADO = fechaTrabajo;
                            model.SUELDO_DIARIO = sueldoDiario;
                            contex.TBL_DIAS_TRABAJADOS.Add(model);
                            actualiza.DIAS_TRABAJADOS += 1;
                            contex.SaveChanges();
                            Respuesta = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Respuesta = false;
                }
            }
            return Respuesta;
        }
        public bool CalcularPagos(decimal IdUsuarioNomina)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_NOMINA actualiza = new TBL_NOMINA();
                    TBL_DIAS_TRABAJADOS actualiza2 = new TBL_DIAS_TRABAJADOS();
                    actualiza = contex.TBL_NOMINA.Where(a => a.ID == IdUsuarioNomina).FirstOrDefault();
                    if (actualiza != null)
                    {
                        decimal? propinas = 0;
                        if (actualiza.ID_PERFIL == 3)
                        {
                            List<TBL_DIAS_TRABAJADOS> listaFechasTrabajadas = new List<TBL_DIAS_TRABAJADOS>();
                            listaFechasTrabajadas = contex.TBL_DIAS_TRABAJADOS.Where(x => x.FECHA_TRABAJADO >= actualiza.FECHA_PAGO && x.ID_USUARIO_NOMINA == actualiza.ID).ToList();
                            foreach (var fecha in listaFechasTrabajadas)
                            {
                                propinas += (contex.TBL_SOLICITUD.Where(x => x.ID_MESERO == actualiza.ID_USUARIO_SISTEMA && x.ESTADO_SOLICITUD != Estados.CancelaPedido && x.ESTADO_SOLICITUD != Estados.Inhabilitar).ToList().Where(x => x.FECHA_SOLICITUD.Value.Date == fecha.FECHA_TRABAJADO.Value.Date).Sum(a => a.SERVICIO_TOTAL)) * ((contex.TBL_PERFIL.Where(x => x.ID == actualiza.ID_PERFIL).FirstOrDefault().PORCENTAJE_PROPINA) / 100);
                            }
                        }
                        else
                        {
                            if (contex.TBL_PERFIL.Where(x => x.ID == actualiza.ID_PERFIL).FirstOrDefault().PORCENTAJE_PROPINA > 0)
                            {
                                List<TBL_DIAS_TRABAJADOS> listaFechasTrabajadas = new List<TBL_DIAS_TRABAJADOS>();
                                listaFechasTrabajadas = contex.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == actualiza.ID).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date >= actualiza.FECHA_PAGO.Value.Date).ToList();
                                var IdUsuariosPropina = contex.TBL_NOMINA.Where(x => x.ID_PERFIL == 4).Select(x => x.ID).ToList();
                                foreach (var fecha in listaFechasTrabajadas)
                                {
                                    var cantUsuarios = contex.TBL_DIAS_TRABAJADOS.Where(x => IdUsuariosPropina.Any(a => x.ID_USUARIO_NOMINA == a)).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date == fecha.FECHA_TRABAJADO).ToList().Count;
                                    var propinaFecha = ((contex.TBL_SOLICITUD.Where(x => x.ESTADO_SOLICITUD != Estados.CancelaPedido && x.ESTADO_SOLICITUD != Estados.Inhabilitar).ToList().Where(x => x.FECHA_SOLICITUD.Value.Date == fecha.FECHA_TRABAJADO.Value.Date).Sum(a => a.SERVICIO_TOTAL)) * ((contex.TBL_PERFIL.Where(x => x.ID == actualiza.ID_PERFIL).FirstOrDefault().PORCENTAJE_PROPINA) / 100) / cantUsuarios);
                                    if (propinaFecha != null)
                                        propinas += propinaFecha;
                                    actualiza2 = contex.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == actualiza.ID).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date == fecha.FECHA_TRABAJADO).FirstOrDefault();
                                    if (actualiza2 != null)
                                    {
                                        actualiza2.PROPINAS = propinaFecha;
                                        contex.SaveChanges();
                                    }
                                }
                            }
                        }
                        actualiza.PROPINAS = propinas;
                        actualiza.TOTAL_PAGAR = ((contex.TBL_DIAS_TRABAJADOS.Where(x => x.FECHA_TRABAJADO >= actualiza.FECHA_PAGO && x.ID_USUARIO_NOMINA == actualiza.ID).ToList().Sum(x => x.SUELDO_DIARIO)) + propinas);
                        actualiza.CONSUMO_INTERNO = contex.TBL_SOLICITUD.Where(x => x.ESTADO_SOLICITUD == Estados.ConsumoInterno && x.FECHA_SOLICITUD >= actualiza.FECHA_PAGO).ToList().Where(x => Convert.ToDecimal(x.IDENTIFICACION_CLIENTE) == Convert.ToDecimal(actualiza.CEDULA)).Sum(x => x.TOTAL);
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
        public bool LiquidarUsuario(decimal IdUsuarioNomina)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_NOMINA actualiza = new TBL_NOMINA();
                    TBL_DIAS_TRABAJADOS actualiza2 = new TBL_DIAS_TRABAJADOS();
                    actualiza = contex.TBL_NOMINA.Where(a => a.ID == IdUsuarioNomina).FirstOrDefault();
                    bool impresionNomina = ImprimirNomina(IdUsuarioNomina);
                    if (actualiza != null && impresionNomina)
                    {
                        var fechaPago = DateTime.Now;
                        List<TBL_DIAS_TRABAJADOS> listaFechasTrabajadas = new List<TBL_DIAS_TRABAJADOS>();
                        listaFechasTrabajadas = contex.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == actualiza.ID && x.FECHA_PAGO == null).ToList();
                        foreach (var item in listaFechasTrabajadas)
                        {
                            item.FECHA_PAGO = fechaPago;
                            contex.SaveChanges();
                        }

                        actualiza.DIAS_TRABAJADOS = 0;
                        actualiza.PROPINAS = 0;
                        actualiza.FECHA_PAGO = fechaPago;
                        actualiza.TOTAL_PAGAR = 0;
                        actualiza.CONSUMO_INTERNO = 0;
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
        public bool ImprimirCierre(List<TBL_SOLICITUD> solicitudes, decimal idUsuario)
        {
            bool respuesta;
            PrintDocument printDocument1 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            printDocument1.PrinterSettings.PrinterName = "CAJA";
            printDocument1.PrintPage += (object sender, PrintPageEventArgs e) =>
            {
                List<ProductosSolicitud> productosSolicitudes = new List<ProductosSolicitud>();
                TBL_USUARIOS usuario = new TBL_USUARIOS();
                //CONSULTA PRODUCTOS
                var idSolicitudes = solicitudes.Where(x => x.ESTADO_SOLICITUD != Estados.CancelaPedido && x.ESTADO_SOLICITUD != Estados.Inhabilitar).Select(x => x.ID).ToList();
                using (DBLaColina contex = new DBLaColina())
                {
                    try
                    {
                        //productosSolicitudes = contex.TBL_PRODUCTOS_SOLICITUD.Where(x => idSolicitudes.Any(a => a == x.ID)).ToList();                            var ListAgrupaProductos = AgrupaProductos(productosSolicitudes);
                        productosSolicitudes = (from a in contex.TBL_PRODUCTOS_SOLICITUD
                                                where idSolicitudes.Any(b => a.ID_SOLICITUD == b)
                                                select new ProductosSolicitud
                                                {
                                                    IdProducto = a.ID_PRODUCTO,
                                                    NombreProducto = contex.TBL_PRODUCTOS.Where(x => x.ID == a.ID_PRODUCTO).FirstOrDefault().NOMBRE_PRODUCTO,
                                                    PrecioProducto = a.PRECIO_PRODUCTO
                                                }).ToList();
                        usuario = contex.TBL_USUARIOS.Where(x => x.ID == idUsuario).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                var productosAgrupados = AgrupaProductos(productosSolicitudes);
                var ultimoCierre = CierreUsuarioId(idUsuario);

                //FORMATO FACTURA
                Font Titulo = new Font("MS Mincho", 14, FontStyle.Bold);
                Font body = new Font("MS Mincho", 10);
                Font bodyNegrita = new Font("MS Mincho", 11, FontStyle.Bold);
                Font bodySubrayado = new Font("MS Mincho", 10, FontStyle.Underline);
                int ancho = 280;
                int YProductos = 0;
                e.Graphics.DrawString("Cierre - Caja", Titulo, Brushes.Black, new RectangleF(85, 10, ancho, 20));
                e.Graphics.DrawString("Fecha Apertura: " + ultimoCierre.FECHA_HORA_APERTURA, body, Brushes.Black, new RectangleF(0, 60, ancho, 20));
                e.Graphics.DrawString("Fecha Cierre: " + ultimoCierre.FECHA_HORA_CIERRE, body, Brushes.Black, new RectangleF(0, 75, ancho, 15));
                e.Graphics.DrawString("Cajero: " + usuario.CEDULA + " - " + usuario.NOMBRE, body, Brushes.Black, new RectangleF(0, 90, ancho, 15));
                e.Graphics.DrawString("______________________________________", body, Brushes.Black, new RectangleF(0, 105, ancho, 15));
                e.Graphics.DrawString("Productos", bodyNegrita, Brushes.Black, new RectangleF(0, 135, ancho, 15));
                //LISTA LOS PRODUCTOS
                foreach (var item in productosAgrupados)
                {
                    YProductos += 15;
                    e.Graphics.DrawString("" + item.NombreProducto, body, Brushes.Black, new RectangleF(0, 135 + YProductos, ancho, 15));
                    e.Graphics.DrawString("" + item.Id, body, Brushes.Black, new RectangleF((280 - (item.Id.ToString().Length * 9)), 135 + YProductos, ancho, 15));
                    //PRECIO UNITARIO
                    //e.Graphics.DrawString("" + item.PrecioProducto, body, Brushes.Black, new RectangleF(160, 215 + YProductos, ancho, 15));
                    //PRECIO TOTAL
                    //e.Graphics.DrawString("" + item.IdMesero, body, Brushes.Black, new RectangleF((280 - (item.IdMesero.ToString().Length * 8)), 215 + YProductos, ancho, 15));
                }
                e.Graphics.DrawString("• Mesas atendidas:", bodyNegrita, Brushes.Black, new RectangleF(0, 180 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.CANT_MESAS_ATENDIDAS, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.CANT_MESAS_ATENDIDAS.ToString().Length * 8)), 180 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Finalizadas:", body, Brushes.Black, new RectangleF(16, 195 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.CANT_FINALIZADAS, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.CANT_FINALIZADAS.ToString().Length * 8)), 195 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Total: ", body, Brushes.Black, new RectangleF(16, 210 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.TOTAL_FINALIZADAS, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.TOTAL_FINALIZADAS.ToString().Length * 8)), 210 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Consumo Interno:", body, Brushes.Black, new RectangleF(16, 225 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.CANT_CONSUMO_INTERNO, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.CANT_CONSUMO_INTERNO.ToString().Length * 8)), 225 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Total: ", body, Brushes.Black, new RectangleF(16, 240 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.TOTAL_CONSUMO_INTERNO, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.TOTAL_CONSUMO_INTERNO.ToString().Length * 8)), 240 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Canceladas:", body, Brushes.Black, new RectangleF(16, 255 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.CANT_CANCELADAS, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.CANT_CANCELADAS.ToString().Length * 8)), 255 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Total: ", body, Brushes.Black, new RectangleF(16, 270 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.TOTAL_CANCELADAS, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.TOTAL_CANCELADAS.ToString().Length * 8)), 270 + YProductos, ancho, 15));

                e.Graphics.DrawString("• Otros Cobros:", bodyNegrita, Brushes.Black, new RectangleF(0, 300 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.OTROS_COBROS_TOTAL, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.OTROS_COBROS_TOTAL.ToString().Length * 8)), 300 + YProductos, ancho, 15));

                e.Graphics.DrawString("• Descuentos:", bodyNegrita, Brushes.Black, new RectangleF(0, 330 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.DESCUENTOS_TOTAL, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.DESCUENTOS_TOTAL.ToString().Length * 8)), 330 + YProductos, ancho, 15));

                e.Graphics.DrawString("• Impuestos:", bodyNegrita, Brushes.Black, new RectangleF(0, 360 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> I.V.A.:", body, Brushes.Black, new RectangleF(16, 375 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.IVA_TOTAL, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.IVA_TOTAL.ToString().Length * 9)), 375 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Impuesto Consumo: ", body, Brushes.Black, new RectangleF(16, 390 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.I_CONSUMO_TOTAL, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.I_CONSUMO_TOTAL.ToString().Length * 8)), 390 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Servicio: ", body, Brushes.Black, new RectangleF(16, 405 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.SERVICIO_TOTAL, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.SERVICIO_TOTAL.ToString().Length * 8)), 405 + YProductos, ancho, 15));

                e.Graphics.DrawString("• Totales:", bodyNegrita, Brushes.Black, new RectangleF(0, 435 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Efectivo: ", body, Brushes.Black, new RectangleF(16, 450 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.TOTAL_EFECTIVO, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.TOTAL_EFECTIVO.ToString().Length * 8)), 450 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> Tarjeta: ", body, Brushes.Black, new RectangleF(16, 465 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.TOTAL_TARJETA, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.TOTAL_TARJETA.ToString().Length * 8)), 465 + YProductos, ancho, 15));
                e.Graphics.DrawString("-> TOTAL VENTAS: ", body, Brushes.Black, new RectangleF(16, 480 + YProductos, ancho, 15));
                e.Graphics.DrawString("" + ultimoCierre.VENTA_TOTAL, body, Brushes.Black, new RectangleF((280 - (ultimoCierre.VENTA_TOTAL.ToString().Length * 8)), 480 + YProductos, ancho, 15));
                e.Graphics.DrawString("_", body, Brushes.Black, new RectangleF(135, 600 + YProductos, ancho, 15));
            };
            printDocument1.Print();
            respuesta = true;
            return respuesta;
        }
        public List<ProductosSolicitud> AgrupaProductos(List<ProductosSolicitud> productosSolicitud)
        {
            List<ProductosSolicitud> resultado = new List<ProductosSolicitud>();
            var distinctProductos = productosSolicitud.DistinctBy(c => c.IdProducto).ToList();
            foreach (var item in distinctProductos)
            {
                ProductosSolicitud model = new ProductosSolicitud();
                model.Id = productosSolicitud.Where(x => x.IdProducto == item.IdProducto).Count();
                model.NombreProducto = item.NombreProducto;
                //PRECIO UNITARIO
                //model.PrecioProducto = item.PrecioProducto;
                //PRECIO TOTAL
                //model.IdMesero = productosSolicitud.Where(x => x.IdProducto == item.IdProducto).Sum(x => x.PrecioProducto);
                resultado.Add(model);
            }
            return resultado;
        }
        public bool ImprimirNomina(decimal id)
        {
            bool respuesta;
            PrintDocument printDocument1 = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            printDocument1.PrinterSettings.PrinterName = "CAJA";
            printDocument1.PrintPage += (object sender, PrintPageEventArgs e) =>
            {
                //CONSULTA USUARIO EN NOMINA
                var usuarioNomina = ConsultaUsuarioNomina(id);

                //FORMATO FACTURA
                Font Titulo = new Font("MS Mincho", 14, FontStyle.Bold);
                Font body = new Font("MS Mincho", 10);
                Font bodyNegrita = new Font("MS Mincho", 11, FontStyle.Bold);
                int ancho = 280;
                int Ymargen = 0;

                e.Graphics.DrawString("PAGO - NÓMINA", Titulo, Brushes.Black, new RectangleF(70, 10, ancho, 20));
                e.Graphics.DrawString("Fecha: " + DateTime.Now, body, Brushes.Black, new RectangleF(0, 60, ancho, 20));
                e.Graphics.DrawString("Identificación: " + usuarioNomina.Cedula, body, Brushes.Black, new RectangleF(0, 75, ancho, 15));
                e.Graphics.DrawString("Nombre: " + usuarioNomina.NombreUsuario, body, Brushes.Black, new RectangleF(0, 90, ancho, 20));
                e.Graphics.DrawString("______________________________________", body, Brushes.Black, new RectangleF(0, 105, ancho, 15));

                e.Graphics.DrawString("Dias trabajados: ", bodyNegrita, Brushes.Black, new RectangleF(0, 135, ancho, 15));
                e.Graphics.DrawString("$$$ ", bodyNegrita, Brushes.Black, new RectangleF(256, 135, ancho, 15));
                for (int i = 0; i < usuarioNomina.FechasAsignadas.Count; i++)
                {
                    Ymargen += 15;
                    e.Graphics.DrawString("* " + usuarioNomina.FechasAsignadas[i].ToString("dd-MM-yyyy"), body, Brushes.Black, new RectangleF(0, 135 + Ymargen, ancho, 15));
                    e.Graphics.DrawString("" + usuarioNomina.SuledoDiario[i], body, Brushes.Black, new RectangleF((280 - (usuarioNomina.SuledoDiario[i].ToString().Length * 8)), 135 + Ymargen, ancho, 15));
                }
                e.Graphics.DrawString("______________________________________", body, Brushes.Black, new RectangleF(0, 165 + Ymargen, ancho, 15));
                
                e.Graphics.DrawString("Propinas: " + usuarioNomina.Propinas, body, Brushes.Black, new RectangleF(0, 195 + Ymargen, ancho, 15));
                e.Graphics.DrawString("Deudas: " + usuarioNomina.ConsumoInterno, body, Brushes.Black, new RectangleF(0, 210 + Ymargen, ancho, 15));

                e.Graphics.DrawString("______________________________________", body, Brushes.Black, new RectangleF(0, 225 + Ymargen, ancho, 15));
                e.Graphics.DrawString("TOTAL: " + usuarioNomina.TotalPagar, bodyNegrita, Brushes.Black, new RectangleF(0, 270 + Ymargen, ancho, 15));
                e.Graphics.DrawString("_", body, Brushes.Black, new RectangleF(0, 355 + Ymargen, ancho, 15));

            };
            printDocument1.Print();
            respuesta = true;
            return respuesta;
        }
        public ConsultaNomina ConsultaUsuarioNomina(decimal id)
        {
            ConsultaNomina usuarioNomina = new ConsultaNomina();

            using (DBLaColina context = new DBLaColina())
            {
                usuarioNomina = (from a in context.TBL_NOMINA
                                 where a.ID == id
                                 select new ConsultaNomina
                                 {
                                     Id = a.ID,
                                     IdUsuarioSistema = a.ID_USUARIO_SISTEMA,
                                     NombreUsuarioSistema = context.TBL_USUARIOS.Where(x => x.ID == a.ID_USUARIO_SISTEMA).FirstOrDefault().NOMBRE != null ? context.TBL_USUARIOS.Where(x => x.ID == a.ID_USUARIO_SISTEMA).FirstOrDefault().NOMBRE : "N/A",
                                     IdPerfil = a.ID_PERFIL,
                                     NombrePerfil = context.TBL_PERFIL.Where(x => x.ID == a.ID_PERFIL).FirstOrDefault().NOMBRE_PERFIL,
                                     Cedula = a.CEDULA,
                                     NombreUsuario = a.NOMBRE,
                                     Cargo = a.CARGO,
                                     DiasTrabajados = a.DIAS_TRABAJADOS,
                                     Propinas = a.PROPINAS,
                                     PorcentajeGananciaPropina = context.TBL_PERFIL.Where(x => x.ID == a.ID_PERFIL).FirstOrDefault().PORCENTAJE_PROPINA,
                                     FechaPago = a.FECHA_PAGO,
                                     FechaNacimmiento = a.FECHA_NACIMIENTO,
                                     DireccionResidencia = a.DIRECCION_RESIDENCIA,
                                     Telefono = a.TELEFONO,
                                     TotalPagar = a.TOTAL_PAGAR,
                                     ConsumoInterno = a.CONSUMO_INTERNO
                                 }).FirstOrDefault();
                if (usuarioNomina != null)
                {
                    usuarioNomina.FechasAsignadas = context.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == id).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date >= usuarioNomina.FechaPago.Value.Date).Select(x => x.FECHA_TRABAJADO.Value.Date).ToList();
                    usuarioNomina.SuledoDiario = context.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == id).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date >= usuarioNomina.FechaPago.Value.Date).Select(x => x.SUELDO_DIARIO).ToList();
                }
            }
            return usuarioNomina;
        }
    }
}