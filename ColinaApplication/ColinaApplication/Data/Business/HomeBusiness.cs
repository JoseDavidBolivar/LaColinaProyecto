using ColinaApplication.Data.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Business
{
    public class HomeBusiness
    {
        public TBL_USUARIOS Login (decimal Cedula, string Contraseña)
        {
            TBL_USUARIOS user = new TBL_USUARIOS();
            using (DBLaColina context = new DBLaColina())
            {
                
                user = context.TBL_USUARIOS.FirstOrDefault(a=>a.CEDULA == Cedula);
                if (user != null)
                {
                    if(user.CONTRASEÑA == Contraseña)
                    {

                    }
                    else
                    {
                        user = new TBL_USUARIOS();
                        user.ID = -1;
                    }
                }
                else
                {
                    user = new TBL_USUARIOS();
                }
                context.TBL_MASTER_MESAS.ToList();
            }
            return user;
        }
    }
}