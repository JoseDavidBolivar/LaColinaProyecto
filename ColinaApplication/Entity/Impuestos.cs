using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Impuestos
    {
        public decimal Id { get; set; }
        public string NombreImpuesto { get; set; }
        public decimal? Porcentaje { get; set; }
        public string Estado { get; set; }
    }
}
