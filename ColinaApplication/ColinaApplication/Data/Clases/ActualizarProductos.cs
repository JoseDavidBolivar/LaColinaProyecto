using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Clases
{
    public class ActualizarProductos
    {
        public decimal Id { get; set; }
        public string Llave { get; set; }
        public string Descripcion { get; set; }
        public decimal? ValorRestar { get; set; }


    }
}