using Data.Conexion;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class SolicitudData
    {
        public List<MasterMesas> ListaMesas()
        {
            List<MasterMesas> ListMesas = new List<MasterMesas>();
            using (BDLaColina context = new BDLaColina())
            {
                ListMesas = context.TBL_MASTER_MESAS.Cast<MasterMesas>().ToList();
            }

            return ListMesas;
        }
    }
}
