using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Valle.SqlGestion;
using Valle.SqlUtilidades;

namespace Valle.ToolsTpv
{
    public class GesSincronizar
    {
       
       public const String NOM_TB_SINCRONIZADOS = "Sincronizados";
	   public const string NOMBRE= "TablaPertenencia";
	   public const string CADENA_SELECT = "CadenaSelect";
	   public const string ACCION="Accion";
        IGesSql gesBase;
        DataTable tbSinc;
        
		public GesSincronizar(IGesSql ges)
		{
            gesBase = ges;
            tbSinc = gesBase.ExtraerTabla("Sincronizados","IDVinculacion");
       	}
		
	
	     public void ActualizarSincronizar(String tablaPertenece, String CadenaSelect, AccionesConReg accion){
             
                 DataRow[] drs = tbSinc.Select("CadenaSelect = ' " + CadenaSelect + " '");
			     if (drs.Length <= 0)
                 {
                     DataRow dr = tbSinc.NewRow();
                     dr["TablaPertenencia"] = tablaPertenece;
                     dr["CadenaSelect"] = CadenasParaSql.EncapsularCadenaSelect(CadenaSelect);
                     dr["Accion"] = accion;
                     tbSinc.Rows.Add(dr);
				     gesBase.EjConsultaNoSelect("Sincronizados",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(dr,AccionesConReg.Agregar,
				                                                                  "").Replace(@"\",@"\\"));
			       }
                 else
                 {
                     if (accion == AccionesConReg.Borrar)
                     {
                         if (UtilidadesReg.StringToAccionesReg(drs[0]["Accion"].ToString()).Equals(AccionesConReg.Agregar))
                         {
						     gesBase.EjConsultaNoSelect("Sincronizados",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(drs[0],AccionesConReg.Borrar,
				                                                                 "CadenaSelect = ' " + CadenaSelect + " '"));
                             drs[0].Delete();
                         }
                         else
                         {
						       drs[0]["Accion"] = accion;
						     gesBase.EjConsultaNoSelect("Sincronizados",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(drs[0],AccionesConReg.Modificar,
				                                                                 "CadenaSelect = ' " + CadenaSelect + " '"));
                             
                         }
                     }
                 }
                
             
         }
    }
}
