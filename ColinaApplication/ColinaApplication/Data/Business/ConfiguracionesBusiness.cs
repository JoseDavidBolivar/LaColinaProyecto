using ColinaApplication.Data.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Business
{
    public class ConfiguracionesBusiness
    {
        public List<TBL_CATEGORIAS> ListaCategorias()
        {
            List<TBL_CATEGORIAS> listCategorias = new List<TBL_CATEGORIAS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listCategorias = contex.TBL_CATEGORIAS.ToList();
            }
            return listCategorias;
        }
        public List<TBL_PRODUCTOS> ListaProductos()
        {
            List<TBL_PRODUCTOS> listProductos = new List<TBL_PRODUCTOS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listProductos = contex.TBL_PRODUCTOS.ToList();
            }
            return listProductos;
        }
        public List<TBL_MASTER_MESAS> ListaMesas()
        {
            List<TBL_MASTER_MESAS> listMesas = new List<TBL_MASTER_MESAS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listMesas = contex.TBL_MASTER_MESAS.ToList();
            }
            return listMesas;
        }
        public List<TBL_PERFIL> ListaPerfiles()
        {
            List<TBL_PERFIL> listPerfiles = new List<TBL_PERFIL>();
            using (DBLaColina contex = new DBLaColina())
            {
                listPerfiles = contex.TBL_PERFIL.ToList();
            }
            return listPerfiles;
        }
        public List<TBL_USUARIOS> ListaUsuarios()
        {
            List<TBL_USUARIOS> listMesas = new List<TBL_USUARIOS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listMesas = contex.TBL_USUARIOS.ToList();
            }
            return listMesas;
        }
        public List<TBL_IMPUESTOS> ListaImpuestos()
        {
            List<TBL_IMPUESTOS> listImpuestos = new List<TBL_IMPUESTOS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listImpuestos = contex.TBL_IMPUESTOS.ToList();
            }
            return listImpuestos;
        }
        public List<TBL_IMPRESORAS> ListaImpresoras()
        {
            List<TBL_IMPRESORAS> listImpresoras = new List<TBL_IMPRESORAS>();
            using (DBLaColina contex = new DBLaColina())
            {
                listImpresoras = contex.TBL_IMPRESORAS.ToList();
            }
            return listImpresoras;
        }
        public List<TBL_NOMINA> ListaNominaEmpleados()
        {
            List<TBL_NOMINA> listImpuestos = new List<TBL_NOMINA>();
            using (DBLaColina contex = new DBLaColina())
            {
                listImpuestos = contex.TBL_NOMINA.ToList();
            }
            return listImpuestos;
        }
        public bool InsertaCategoria(TBL_CATEGORIAS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_CATEGORIAS.Add(model);
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
        public bool ActualizaCategoria(TBL_CATEGORIAS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_CATEGORIAS actualiza = new TBL_CATEGORIAS();
                    actualiza = contex.TBL_CATEGORIAS.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.CATEGORIA = model.CATEGORIA;
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
        public bool InsertaProducto(TBL_PRODUCTOS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    model.FECHA_INGRESO = DateTime.Now;
                    contex.TBL_PRODUCTOS.Add(model);
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
        public bool ActualizaProducto(TBL_PRODUCTOS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_PRODUCTOS actualiza = new TBL_PRODUCTOS();
                    actualiza = contex.TBL_PRODUCTOS.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.ID_CATEGORIA = model.ID_CATEGORIA;
                        actualiza.NOMBRE_PRODUCTO = model.NOMBRE_PRODUCTO;
                        actualiza.PRECIO = model.PRECIO;
                        actualiza.CANTIDAD = model.CANTIDAD;
                        actualiza.DESCRIPCION = model.DESCRIPCION;
                        actualiza.ID_IMPRESORA = model.ID_IMPRESORA;
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
        public bool InsertaMesa(TBL_MASTER_MESAS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_MASTER_MESAS.Add(model);
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
        public bool ActualizaMesa(TBL_MASTER_MESAS model)
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
                        actualiza.NOMBRE_MESA = model.NOMBRE_MESA;
                        actualiza.DESCRIPCION = model.DESCRIPCION;
                        actualiza.ESTADO = model.ESTADO;
                        actualiza.ID_USUARIO = model.ID_USUARIO;
                        actualiza.NUMERO_MESA = model.NUMERO_MESA;
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
        public bool InsertaUsuario(TBL_USUARIOS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_USUARIOS.Add(model);
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
        public bool ActualizaUsuario(TBL_USUARIOS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_USUARIOS actualiza = new TBL_USUARIOS();
                    actualiza = contex.TBL_USUARIOS.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.CEDULA = model.CEDULA;
                        actualiza.NOMBRE = model.NOMBRE;
                        actualiza.CONTRASEÑA = model.CONTRASEÑA;
                        actualiza.ID_PERFIL = model.ID_PERFIL;
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
        public bool InsertaImpresora(TBL_IMPRESORAS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_IMPRESORAS.Add(model);
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
        public bool ActualizaImpresora(TBL_IMPRESORAS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_IMPRESORAS actualiza = new TBL_IMPRESORAS();
                    actualiza = contex.TBL_IMPRESORAS.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.NOMBRE_IMPRESORA = model.NOMBRE_IMPRESORA;
                        actualiza.DESCRIPCION = model.DESCRIPCION;
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
        public bool InsertaImpuesto(TBL_IMPUESTOS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_IMPUESTOS.Add(model);
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
        public bool ActualizaImpuesto(TBL_IMPUESTOS model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_IMPUESTOS actualiza = new TBL_IMPUESTOS();
                    actualiza = contex.TBL_IMPUESTOS.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.NOMBRE_IMPUESTO = model.NOMBRE_IMPUESTO;
                        actualiza.PORCENTAJE = model.PORCENTAJE;
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
        public bool InsertaPerfil(TBL_PERFIL model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_PERFIL.Add(model);
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
        public bool ActualizaPerfil(TBL_PERFIL model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_PERFIL actualiza = new TBL_PERFIL();
                    actualiza = contex.TBL_PERFIL.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.NOMBRE_PERFIL = model.NOMBRE_PERFIL;
                        actualiza.PORCENTAJE_PROPINA = model.PORCENTAJE_PROPINA;
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
        public bool InsertaNominaEmpleados(TBL_NOMINA model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    contex.TBL_NOMINA.Add(model);
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
        public bool ActualizaNominaEmpleados(TBL_NOMINA model)
        {
            bool Respuesta = false;
            using (DBLaColina contex = new DBLaColina())
            {
                try
                {
                    TBL_NOMINA actualiza = new TBL_NOMINA();
                    actualiza = contex.TBL_NOMINA.Where(a => a.ID == model.ID).FirstOrDefault();
                    if (actualiza != null)
                    {
                        actualiza.ID_USUARIO_SISTEMA = model.ID_USUARIO_SISTEMA;
                        actualiza.ID_PERFIL = model.ID_PERFIL;
                        actualiza.CEDULA = model.CEDULA;
                        actualiza.NOMBRE = model.NOMBRE;
                        actualiza.CARGO = model.CARGO;
                        actualiza.SUELDO_DIARIO = model.SUELDO_DIARIO;
                        actualiza.FECHA_NACIMIENTO = model.FECHA_NACIMIENTO;
                        actualiza.DIRECCION_RESIDENCIA = model.DIRECCION_RESIDENCIA;
                        actualiza.TELEFONO = model.TELEFONO;
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
    }
}