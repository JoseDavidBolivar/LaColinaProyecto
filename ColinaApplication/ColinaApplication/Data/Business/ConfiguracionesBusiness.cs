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
    }
}