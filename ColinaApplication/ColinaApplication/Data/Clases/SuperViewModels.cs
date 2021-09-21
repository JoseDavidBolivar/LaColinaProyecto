using ColinaApplication.Data.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Clases
{
    public class SuperViewModels
    {
        public List<TBL_CATEGORIAS> Categorias { get; set; }
        public TBL_CATEGORIAS CategoriasModel { get; set; }
        public List<TBL_PRODUCTOS> Productos { get; set; }
        public TBL_PRODUCTOS ProductosModel { get; set; }
    }
}