//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ColinaApplication.Data.Conexion
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_CIERRES
    {
        public decimal ID { get; set; }
        public Nullable<System.DateTime> FECHA_HORA_APERTURA { get; set; }
        public Nullable<System.DateTime> FECHA_HORA_CIERRE { get; set; }
        public Nullable<decimal> ID_USUARIO { get; set; }
        public Nullable<decimal> CANT_MESAS_ATENDIDAS { get; set; }
        public Nullable<decimal> CANT_FINALIZADAS { get; set; }
        public Nullable<decimal> TOTAL_FINALIZADAS { get; set; }
        public Nullable<decimal> CANT_LLEVAR { get; set; }
        public Nullable<decimal> TOTAL_LLEVAR { get; set; }
        public Nullable<decimal> CANT_CANCELADAS { get; set; }
        public Nullable<decimal> TOTAL_CANCELADAS { get; set; }
        public Nullable<decimal> CANT_CONSUMO_INTERNO { get; set; }
        public Nullable<decimal> TOTAL_CONSUMO_INTERNO { get; set; }
        public Nullable<decimal> OTROS_COBROS_TOTAL { get; set; }
        public Nullable<decimal> DESCUENTOS_TOTAL { get; set; }
        public Nullable<decimal> IVA_TOTAL { get; set; }
        public Nullable<decimal> I_CONSUMO_TOTAL { get; set; }
        public Nullable<decimal> SERVICIO_TOTAL { get; set; }
        public Nullable<decimal> TOTAL_EFECTIVO { get; set; }
        public Nullable<decimal> TOTAL_TARJETA { get; set; }
        public Nullable<decimal> VENTA_TOTAL { get; set; }
    }
}
