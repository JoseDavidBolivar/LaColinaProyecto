using ColinaApplication.Data.Clases;
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
                              SuledoDiario = a.SUELDO_DIARIO,
                              DiasTrabajados = a.DIAS_TRABAJADOS,
                              Propinas = a.PROPINAS,
                              PorcentajeGananciaPropina = context.TBL_PERFIL.Where(x => x.ID == a.ID_PERFIL).FirstOrDefault().PORCENTAJE_PROPINA,
                              FechaPago = a.FECHA_PAGO,
                              FechaNacimmiento = a.FECHA_NACIMIENTO,
                              DireccionResidencia = a.DIRECCION_RESIDENCIA,
                              Telefono = a.TELEFONO,
                              TotalPagar = a.TOTAL_PAGAR
                          }).ToList();
                foreach (var item in nomina)
                {
                    item.FechasAsignadas = context.TBL_DIAS_TRABAJADOS.Where(x => x.ID_USUARIO_NOMINA == item.Id).ToList().Where(x => x.FECHA_TRABAJADO.Value.Date >= item.FechaPago.Value.Date).Select(x => x.FECHA_TRABAJADO.Value.Date).ToList();
                }
            }
            return nomina;
        }
        public bool AsignaDiaTrabajo(decimal IdUsuarioNomina, DateTime fechaTrabajo)
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
                        actualiza.TOTAL_PAGAR = ((actualiza.SUELDO_DIARIO * actualiza.DIAS_TRABAJADOS) + actualiza.PROPINAS);
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
                    if (actualiza != null)
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
    }
}