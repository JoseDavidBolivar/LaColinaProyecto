using ColinaApplication.Data.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;

namespace ColinaApplication.Data.Clases
{
    public class SuperViewModels
    {
        public List<TBL_CATEGORIAS> Categorias { get; set; }
        public TBL_CATEGORIAS CategoriasModel { get; set; }
        public List<TBL_PRODUCTOS> Productos { get; set; }
        public TBL_PRODUCTOS ProductosModel { get; set; }
        public List<TBL_MASTER_MESAS> Mesas { get; set; }
        public TBL_MASTER_MESAS MesasModel { get; set; }
        public List<TBL_USUARIOS> Usuarios { get; set; }
        public TBL_USUARIOS UsuariosModel { get; set; }

        public List<TBL_IMPUESTOS> Impuestos { get; set; }
        public TBL_IMPUESTOS ImpuestosModel { get; set; }
        public List<TBL_PERFIL> Perfiles { get; set; }
        public TBL_PERFIL PerfilesModel { get; set; }
        public List<TBL_NOMINA> NominaEmpleados { get; set; }
        public TBL_NOMINA NominaEmpleadosModel { get; set; }


        public List<ConsultaSolicitud> Solicitudes { get; set; }
        public ConsultaSolicitud SolicitudModel { get; set; }
        public TBL_CIERRES Cierre { get; set; }
        public List<ConsultaNomina> Nomina { get; set; }
    }
}