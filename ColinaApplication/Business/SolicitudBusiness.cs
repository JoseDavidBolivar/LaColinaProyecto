using Data.Data;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class SolicitudBusiness
    {
        public List<MasterMesas> ListaMesas()
        {
            SolicitudData solicitudData = new SolicitudData();
            return solicitudData.ListaMesas();
        }
    }
}
