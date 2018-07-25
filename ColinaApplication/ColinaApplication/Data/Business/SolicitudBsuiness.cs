using ColinaApplication.Data.Conexion;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Business
{
    public class SolicitudBsuiness
    {
        public List<TBL_MASTER_MESAS> ListaMesas()
        {
            List<TBL_MASTER_MESAS> ListMesas = new List<TBL_MASTER_MESAS>();
            using (DBLaColina context = new DBLaColina())
            {
                ListMesas = context.TBL_MASTER_MESAS.ToList();
            }

            return ListMesas;
        }

        public void ActualizaEstadoMesa (decimal Id, string Estado)
        {
            using (DBLaColina contex = new DBLaColina())
            {
                TBL_MASTER_MESAS modelActualizar = new TBL_MASTER_MESAS();

                modelActualizar = contex.TBL_MASTER_MESAS.FirstOrDefault(a=> a.ID == Id);

                modelActualizar.ESTADO = Estado;

                contex.SaveChanges();
            }
        }
    }
}