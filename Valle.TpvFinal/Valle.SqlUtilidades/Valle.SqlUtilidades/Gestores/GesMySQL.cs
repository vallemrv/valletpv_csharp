// GesMysql.cs created with MonoDevelop
// User: valle at 22:58 05/03/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// GesBaseLocal.cs created with MonoDevelop
// User: valle at 0:44 07/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using MySql.Data.MySqlClient;

using Valle.SqlUtilidades;



namespace Valle.SqlGestion
{

	[Serializable]
	public class GesMySQL : IGesSql
	{
	    string StrConAMySql = "Data source=localhost;port=@PORT;Database=@BASE;Uid=@USER;Pwd=@PASS;CHARSET=utf8";
	    DataSet baseLocal = new DataSet("BaseLocal");
        AutoResetEvent exmut = new AutoResetEvent(true);
        MySqlConnection cn;

       public GesMySQL(string pass,string usuario,string puerto){
           StrConAMySql = StrConAMySql.Replace("@PASS",pass);
		   StrConAMySql = StrConAMySql.Replace("@USER",usuario);
		   StrConAMySql = StrConAMySql.Replace("@PORT",puerto);	
        }
		
       public GesMySQL(string DirCompletaBase,string pass,string usuario,string puerto)
       {
            StrConAMySql = this.StrConAMySql.Replace("@BASE",DirCompletaBase);
            StrConAMySql = StrConAMySql.Replace("@PASS",pass);
		    StrConAMySql = StrConAMySql.Replace("@USER",usuario);
		    StrConAMySql = StrConAMySql.Replace("@PORT",puerto);
			cn = new MySqlConnection(StrConAMySql);
       }
		
	  [Obsolete]
      public void CargarTabla(string tabla, String columKey)
       {
		
			if (!baseLocal.Tables.Contains(tabla))
            {
                 DataTable tb = this.EjecutarSqlSelect(tabla, "SELECT * FROM " + tabla);
	               
                 
               if (columKey != null)
                {
            	    tb.Columns[columKey].AutoIncrement = true;
                    tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                    (int)tb.Rows[tb.Rows.Count - 1][columKey] + 1 : 1;
                    
                }
				
                cn.Close();
                 baseLocal.Tables.Add(tb);
            }
            
            
        }
		
	  [Obsolete]
	  public DataTable ExtraerTabla(string tabla, String columKey)
        {
			    string Key = columKey==null ? "" : columKey;
		        if (!baseLocal.Tables.Contains(tabla))
	            {
	               DataTable tb = this.EjecutarSqlSelect(tabla, "SELECT * FROM " + tabla);
	                    
	                if (Key.Length>0)
	                {
	                    tb.Columns[Key].AutoIncrement = true;
	                    tb.Columns[Key].AutoIncrementSeed = tb.Rows.Count > 0 ?
	                    (int)tb.Rows[tb.Rows.Count - 1][Key] + 1 : 1;
	                    
	                }
						
	                cn.Close();
	                baseLocal.Tables.Add(tb);
				    
	            }
	             
	             
	           return baseLocal.Tables[tabla];
			
        }
		
	  [Obsolete]
	  public DataTable ExtraerTabla(string tabla, String columKey, string dirBaseDatos)
      {
		   if (!baseLocal.Tables.Contains(tabla))
            {
           
			DataTable tb = new DataTable(tabla);
            
				tb =   this.EjecutarSqlSelect(tabla, "SELECT * FROM " + tabla, dirBaseDatos);
                     
                if (columKey != null)
                {
                    tb.Columns[columKey].AutoIncrement = true;
                    tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                    (int)tb.Rows[tb.Rows.Count - 1][columKey] + 1 : 1;
                    
                }
                
                baseLocal.Tables.Add(tb);
				
			}
             return baseLocal.Tables[tabla];
			
            
			
        }
		
  
	 public DataTable ExtraerTabla(string tabla){
           if (!baseLocal.Tables.Contains(tabla))
            {
                baseLocal.Tables.Add(this.EjecutarSqlSelect(tabla,"SELECT * FROM "+tabla));
			}
             return baseLocal.Tables[tabla];
			
        }
		
	public DataTable ExtraerTablaEX(string tabla, string nomBase){
           if (!baseLocal.Tables.Contains(tabla))
            {
                baseLocal.Tables.Add(this.EjecutarSqlSelect(tabla,"SELECT * FROM "+tabla,nomBase));
			}
             return baseLocal.Tables[tabla];
			
        }
		
	public DataTable ExtraerTablaEX(string tabla, params Valle.SqlUtilidades.Tools.ClausulaWhere[] sqlwhere){
           if (!baseLocal.Tables.Contains(tabla))
            {
                baseLocal.Tables.Add(this.EjecutarSqlSelect(tabla,"SELECT * FROM "+tabla+" "+Valle.SqlUtilidades.CadenasParaSql.GetStrSelectParms(sqlwhere)));
			}
             return baseLocal.Tables[tabla];
			
        }
		
		public DataTable ExtraerTablaEX(string tabla,string nomBase, params Valle.SqlUtilidades.Tools.ClausulaWhere[] sqlwhere){
           if (!baseLocal.Tables.Contains(tabla))
            {
                baseLocal.Tables.Add(this.EjecutarSqlSelect(tabla,"SELECT * FROM "+tabla+" "+Valle.SqlUtilidades.CadenasParaSql.GetStrSelectParms(sqlwhere)
				                                            ,nomBase));
			}
             return baseLocal.Tables[tabla];
			
        }
		
	   [ObsoleteAttribute]
       public bool ActualizarTabla(string tabla)
        {
			throw new Exception("En MySql Server no esta implementada");
        }
		

		public void AgregarTabla(DataTable tb)
        {
        	baseLocal.Tables.Add(tb);
        }
		
       public DataTable EjecutarSqlSelect(string nombre, string consulta){
		DataTable tb = null;	
		 try{
		   exmut.WaitOne();
            if (cn.State != ConnectionState.Open) cn.Open();
            MySqlDataAdapter ad = new MySqlDataAdapter(consulta,cn);
            tb = new DataTable(nombre);
            ad.Fill(tb); 
            cn.Close();
		 }catch(Exception e){
			   Utilidades.RutasArchivos.EscribirEnFicheroErr("ErroresGesMySql.log",consulta +" "+e.Message,DateTime.Now.ToString(),"EjConsultaSelec1");	
		       exmut.Set();
			}			
        	 exmut.Set();
            return tb;
         }
     
		public DataTable EjecutarSqlSelect(string nombre, string consulta, string dirBaseDatos){
		 DataTable tb = null;	
          try{
			exmut.WaitOne();
           MySqlConnection cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",dirBaseDatos));
            cnAux.Open();
            MySqlDataAdapter ad = new MySqlDataAdapter(consulta,cnAux);
            tb = new DataTable(nombre);
            ad.Fill(tb); 
            cnAux.Close();
		   }catch(Exception e){
			   Utilidades.RutasArchivos.EscribirEnFicheroErr("ErroresGesMySql.log",e.Message,DateTime.Now.ToString(),"EjConsultaSelec2");	
		       exmut.Set();
			}			
        
          exmut.Set();  
            return tb;
         }
		
	   public string[] BakupSQL(string nomTabla){
			List<string> bakup = new List<string>();
			DataTable tb = this.EjecutarSqlSelect(nomTabla,"Select * From "+nomTabla);
			for(int i= 0;i<tb.Rows.Count;i++){
			  bakup.Add(UtilidadesReg.ExConsultaNoSelet(UtilidadesReg.DeDataRowARegistro(tb,i)));	
			}
			return bakup.ToArray();
	   }
		
		
       public Object EjEscalar(string consulta, string dirBaseDatos)
         {
             object resul = null;
             exmut.WaitOne();
             MySqlConnection cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE", dirBaseDatos));
             MySqlCommand cmd = new MySqlCommand(consulta, cnAux);
             cmd.CommandTimeout = 0;
             resul = cmd.ExecuteScalar();
             cn.Close();
             exmut.Set();
             return resul;
         }

       public Object EjEscalar(String consulta)
         {
             object resul = null;
             exmut.WaitOne();
             if (cn.State != ConnectionState.Open) cn.Open();
             MySqlCommand cmd = new MySqlCommand(consulta, cn);
             cmd.CommandTimeout = 0;
             resul = cmd.ExecuteScalar();
             cn.Close();
             exmut.Set();
             return resul;

         }

         //Argumento 'tabla' no sirve paranada es para mantener compatibilidad con las clases que depende de esta.
         //Este metodo ha sido variado
        public int EjConsultaNoSelect(string tabla, String consulta)
        {
           int resul = -1;
           exmut.WaitOne();
           if (cn.State != ConnectionState.Open) cn.Open();
            MySqlCommand cmd = new MySqlCommand(consulta,cn);
            cmd.CommandTimeout = 0;
            resul = cmd.ExecuteNonQuery();
            cn.Close();
           exmut.Set();
            
            return resul;
        }
        
        //Argumento 'tabla' no sirve paranada es para mantener compatibilidad con las clases que depende de esta.
        //Este metodo ha sido variado
        public int EjConsultaNoSelect(string tabla, String consulta, string dirBaseDatos)
        {
            int resul = -1;
           exmut.WaitOne();
            MySqlConnection cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",dirBaseDatos));
              cnAux.Open();
              MySqlCommand cmd = new MySqlCommand(consulta,cnAux);
              cmd.CommandTimeout = 0;
              resul = cmd.ExecuteNonQuery();
              cnAux.Close();
          exmut.Set();
            
            return resul;
                   
       
        }
		
		public object[] GetDatosUnaColumna(string tabla,string nomColum, 
			          params Valle.SqlUtilidades.Tools.ClausulaWhere[] sqlSelect)
		{
		    cn.Open();
			
			MySqlCommand cmd = new MySqlCommand("SELECT "+nomColum+" FROM "+tabla+" "+
				 SqlUtilidades.CadenasParaSql.GetStrSelectParms(sqlSelect),cn);
			 
			MySqlDataReader dr = cmd.ExecuteReader();
           dr.Read();
			
			  object[] salida = new object[dr.FieldCount];
			
			for(int i = 0;i<dr.FieldCount;i++){
			         salida[i] = dr.GetValue(i);	
			}
			
			cn.Close();
          
			return salida;
		}
		
		 public void Desbloquear(){
			if(cn!=null)
                   	cn.Close();
        	exmut.Set();
        }
       
         public int ModificarReg(DataRow r, AccionesConReg accion,string filtro){
		  Registro reg =	UtilidadesReg.DeDataRowARegistro(r.Table,r.Table.Rows.IndexOf(r));
			reg.AccionReg = accion;
			
			if(accion!=AccionesConReg.Agregar)
		              	reg.CadenaSelect = filtro;
			
			return ModificarReg(reg);
		}
	
		
       
		public int ModificarReg(Registro reg){
              int res = -1;
          	  res = this.EjConsultaNoSelect(reg.NomTabla,UtilidadesReg.ExConsultaNoSelet(reg).Replace(@"\",@"\\"));
			return res;
         }
        
       	
        public int ModificarReg(Registro reg, string baseDatos){
              int res = -1;
          	  res = this.EjConsultaNoSelect(reg.NomTabla,UtilidadesReg.ExConsultaNoSelet(reg).Replace(@"\",@"\\"),baseDatos);    
			return res;
         }
         
         public int NumRegEnTabla(string tabla){
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM "+tabla, cn);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int numReg  = dr.GetInt32(0);
            cn.Close();
            return numReg;
         }
	     
         public int NumRegEnTabla(string tabla, string baseDatos){
			exmut.WaitOne();
            MySqlConnection  cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",baseDatos));
	        cnAux.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM "+tabla, cnAux);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int numReg  = dr.GetInt32(0);
            cnAux.Close();
			exmut.Set();
            return numReg;
         }
		
		 public int NumRegEnTabla(string tabla,  params Valle.SqlUtilidades.Tools.ClausulaWhere[] sqlSelect){
            cn.Open();
			string sql = "SELECT COUNT(*) FROM "+tabla+" "+SqlUtilidades.CadenasParaSql.GetStrSelectParms(sqlSelect);
		   MySqlCommand cmd = new MySqlCommand(sql
				, cn);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int numReg  = dr.GetInt32(0);
            cn.Close();
            return numReg;
         }
	       
	     public int NumRegEnTabla(string tabla, string baseDatos, 
			     params Valle.SqlUtilidades.Tools.ClausulaWhere[] sqlSelect){
			exmut.WaitOne();
            MySqlConnection  cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",baseDatos));
	        cnAux.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM "+tabla+" "+SqlUtilidades.CadenasParaSql.GetStrSelectParms(sqlSelect)
				, cnAux);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int numReg  = dr.GetInt32(0);
            cnAux.Close();
			exmut.Set();
            return numReg;
         }
         
			
	     public bool ContienTabla(string tabla, string baseDatos){
			exmut.WaitOne();
            bool esta = false;
            MySqlConnection  cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",baseDatos));
	        cnAux.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT *"+
                                                "FROM INFORMATION_SCHEMA.TABLES "+
                                                "WHERE TABLE_NAME = '"+tabla+"' AND TABLE_SCHEMA = '"+baseDatos+"'",cnAux);
            MySqlDataReader dr = cmd.ExecuteReader();
            esta = dr.HasRows;
            cnAux.Close();
		    exmut.Set();
            return esta; 
         
         }
         
         public int NumTablasBase(string baseDatos){
			exmut.WaitOne();
            MySqlConnection  cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",baseDatos));
	        cnAux.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(TABLE_NAME) AS NumeroTablas "+
                                                "FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '"+baseDatos+"'",cnAux);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int numTablas  = dr.GetInt32(0);
            cnAux.Close();
			exmut.Set();
            return numTablas;
         
         }
	     
        #region Para gestionar las bases de datos
        public int CrearBaseDatos(String nombre){
			exmut.WaitOne();
            MySqlConnection  cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",""));
	        cnAux.Open();
            MySqlCommand cmd = new MySqlCommand("Create Database "+nombre,cnAux);
            int i = cmd.ExecuteNonQuery(); cnAux.Close();
			exmut.Set();
            return i;
        }
        
        public bool ContieneBaseDatos(string nombre){
			exmut.WaitOne();
		   bool esta = false;
            MySqlConnection  cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",""));
		    cnAux.Open();
		
            MySqlCommand cmd = new MySqlCommand("SELECT SCHEMA_NAME "+
                                                "FROM INFORMATION_SCHEMA.SCHEMATA "+
                                                "WHERE SCHEMA_NAME = '"+nombre+"'",cnAux);
            MySqlDataReader dr = cmd.ExecuteReader();
	
            esta = dr.HasRows;
            cnAux.Close();
			exmut.Set();
            return esta;
				 
          } 
          
         public int EliminarBaseDatos(string nombre){
			exmut.WaitOne();
            MySqlConnection  cnAux = new MySqlConnection(this.StrConAMySql.Replace("@BASE",""));
	        cnAux.Open();
            MySqlCommand cmd = new MySqlCommand("Drop Database "+nombre,cnAux);
            int i = cmd.ExecuteNonQuery(); cnAux.Close();
			exmut.Set();
            return i;
         }
         
         
         #endregion
         
         
        
        
         
         
	}
}
