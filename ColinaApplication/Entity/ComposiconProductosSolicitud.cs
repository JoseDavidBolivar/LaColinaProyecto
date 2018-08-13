using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ComposiconProductosSolicitud
    {
        public decimal Id { get; set; }
        public decimal? IdProductoSolicitud { get; set; }
        public string Descripcion { get; set; }
        public decimal? Valor { get; set; }
    }
}
