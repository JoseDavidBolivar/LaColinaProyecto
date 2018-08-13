using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProductosSolicitud
    {
        public decimal Id { get; set; }
        public System.DateTime? FechaRegistro { get; set; }
        public decimal? IdSolicitud { get; set; }
        public decimal? IdSubProducto { get; set; }
        public string NombreSubProducto { get; set; }
        public decimal? IdMesero { get; set; }
        public string NombreMesero { get; set; }
        public decimal? PrecioProducto { get; set; }
        public decimal? PrecioFinal { get; set; }
        public string EstadoProductos { get; set; }
        public List<ComposiconProductosSolicitud> CompoProductSolicitud { get; set; }
    }
}
