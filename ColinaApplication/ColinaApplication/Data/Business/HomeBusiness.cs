﻿using ColinaApplication.Data.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ColinaApplication.Data.Business
{
    public class HomeBusiness
    {
        public TBL_USUARIOS Login (string Codigo)
        {
            TBL_USUARIOS user = new TBL_USUARIOS();
            using (DBLaColina context = new DBLaColina())
            {
                user = context.TBL_USUARIOS.FirstOrDefault(a=>a.CONTRASEÑA == Codigo);
                if (user != null)
                {
                    
                }
                else
                {
                    user = new TBL_USUARIOS();
                    user.ID = -1;
                }
                context.TBL_MASTER_MESAS.ToList();
            }
            return user;
        }
    }
}