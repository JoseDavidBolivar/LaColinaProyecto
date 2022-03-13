using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ConsultaNomina
    {
        public decimal Id { get; set; }        
        public decimal? IdUsuarioSistema { get; set; }
        public string NombreUsuarioSistema { get; set; }
        public decimal? IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public decimal? Cedula { get; set; }
        public string NombreUsuario { get; set; }
        public string Cargo { get; set; }
        public List<decimal?> SuledoDiario { get; set; }
        public decimal? DiasTrabajados { get; set; }
        public decimal? Propinas { get; set; }
        public decimal? PorcentajeGananciaPropina { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaNacimmiento { get; set; }
        public string DireccionResidencia { get; set; }
        public decimal? Telefono { get; set; }
        public string Estado { get; set; }
        public decimal? TotalPagar { get; set; }
        public List<DateTime> FechasAsignadas { get; set; }
        public decimal? ConsumoInterno { get; set; }
        public List<decimal?> PerfilFecha { get; set; }
    }
}
