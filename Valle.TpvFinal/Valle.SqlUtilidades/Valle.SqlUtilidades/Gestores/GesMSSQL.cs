using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient; 
using System.Data;
using System.Threading;

using Valle.SqlUtilidades;

namespace Valle.SqlGestion
{
	[Serializable]
    public class GesMSSQL: IGesSql
    {
       
        
        string StrConAMSSql = @"Data Source=.\SQLEXPRESS;AttachDbFilename=@BASE;Integrated Security=True;User Instance=True";
        Dictionary<string,SqlDataAdapter> adaptadores = new Dictionary<string,SqlDataAdapter>();
        DataSet baseLocal = new DataSet("BaseLocal");
        AutoResetEvent exmut = new AutoResetEvent(true);
        string cadenaConexion;
        SqlConnection cn;

         
        public GesMSSQL(string DirCompletaBase)
        {
            cadenaConexion = this.StrConAMSSql.Replace("@BASE",DirCompletaBase);
            cn = new SqlConnection(cadenaConexion);
            adaptadores = new Dictionary<string, SqlDataAdapter>();
        }
        
        public void CargarTabla(string tabla, String columKey)
        {
           exmut.WaitOne();
            if (!baseLocal.Tables.Contains(tabla))
            {
                
        	    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + tabla, cn);
                DataTable tb = new DataTable(tabla);
                SqlCommandBuilder cmdB = new SqlCommandBuilder(da);
                da.InsertCommand = cmdB.GetInsertCommand();
                   
                if (columKey != null)
                {
                    da.UpdateCommand = cmdB.GetUpdateCommand();
                    da.DeleteCommand = cmdB.GetDeleteCommand();
                    da.Fill(tb); da.FillSchema(tb, SchemaType.Mapped);
                    tb.Columns[columKey].AutoIncrement = true;
                    tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                    (int)tb.Rows[tb.Rows.Count - 1][columKey] + 1 : 1;
                    
                }else
                   da.Fill(tb); da.FillSchema(tb, SchemaType.Mapped);
                adaptadores.Add(tabla,da);
                baseLocal.Tables.Add(tb);
            }
            cn.Close();
            exmut.Set();
        }
       
        public DataTable ExtraerTabla(string tabla, String columKey)
        {
           exmut.WaitOne();
            if (!baseLocal.Tables.Contains(tabla))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + tabla, cn);
                DataTable tb = new DataTable(tabla);
                SqlCommandBuilder cmdB = new SqlCommandBuilder(da);
                da.InsertCommand = cmdB.GetInsertCommand();
                   
                if (columKey != null)
                {
                    da.UpdateCommand = cmdB.GetUpdateCommand();
                    da.DeleteCommand = cmdB.GetDeleteCommand();
                    da.Fill(tb); da.FillSchema(tb, SchemaType.Mapped);
                    tb.Columns[columKey].AutoIncrement = true;
                    tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                    (int)tb.Rows[tb.Rows.Count - 1][columKey] + 1 : 1;
                    
                }else
                   da.Fill(tb); da.FillSchema(tb, SchemaType.Mapped);
                
                adaptadores.Add(tabla,da);
                baseLocal.Tables.Add(tb);
            }
            
            cn.Close();
            exmut.Set();
             
           
            return baseLocal.Tables[tabla];
        }
       
        public DataTable ExtraerTabla(string tabla, String columKey, string dirBaseDatos)
        {
           exmut.WaitOne();
            if (!baseLocal.Tables.Contains(tabla))
            {
                SqlConnection cnAux = new SqlConnection(this.StrConAMSSql.Replace("@BASE",dirBaseDatos)); 
        	    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + tabla, cnAux);
                DataTable tb = new DataTable(tabla);
                SqlCommandBuilder cmdB = new SqlCommandBuilder(da);
                da.InsertCommand = cmdB.GetInsertCommand();
                   
                if (columKey != null)
                {
                    da.UpdateCommand = cmdB.GetUpdateCommand();
                    da.DeleteCommand = cmdB.GetDeleteCommand();
                    da.Fill(tb); da.FillSchema(tb, SchemaType.Mapped);
                    tb.Columns[columKey].AutoIncrement = true;
                    tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                    (int)tb.Rows[tb.Rows.Count - 1][columKey] + 1 : 1;
                    
                }else
                   da.Fill(tb); da.FillSchema(tb, SchemaType.Mapped);
                   
                adaptadores.Add(tabla,da);
                baseLocal.Tables.Add(tb);
            }
            cn.Close();
            exmut.Set();
            return baseLocal.Tables[tabla];
        }
       
        
        public DataTable ExtraerTabla(string tabla){
            if(baseLocal.Tables.Contains(tabla))
             	return baseLocal.Tables[tabla];
             	else{
             	  return ExtraerTabla(tabla,null);
             	}
             	
        }
        
        public void Desbloquear(){
        	cn.Close();
        	exmut.Set();
        }
        
        public bool ActualizarTabla(string tabla)
        {
        	exmut.WaitOne();
        	if(adaptadores.ContainsKey(tabla)){
        	      adaptadores[tabla].Update(baseLocal, tabla);
                  baseLocal.AcceptChanges();
                    cn.Close();
                    exmut.Set();
                    return true;
               }
        	   cn.Close();
        	   exmut.Set();
        	   return false;
        }
        
        public void AgregarTabla(DataTable tb)
        {
        	baseLocal.Tables.Add(tb);
        }
        
        public DataTable EjecutarSqlSelect(string nombre, string consulta){
           DataTable tb = null;
           exmut.WaitOne();
          
            SqlDataAdapter ad = new SqlDataAdapter(consulta,cn);
            tb = new DataTable(nombre);
            ad.Fill(tb);
            cn.Close();
           exmut.Set();
            return tb;
         }
        
        public DataTable EjecutarSqlSelect(string nombre, string consulta, string dirBaseDatos){
         DataTable tb = new DataTable(nombre);
           	
          exmut.WaitOne();
           SqlConnection cnAux = new SqlConnection(this.StrConAMSSql.Replace("@BASE",dirBaseDatos));
            SqlDataAdapter ad = new SqlDataAdapter(consulta,cnAux);
            ad.Fill(tb); 
            cn.Close();
          exmut.Set();  
            return tb;

        }

        public  Object EjEscalar( string consulta, string dirBaseDatos)
        {
            object resul = null;
            exmut.WaitOne();
            SqlConnection cnAux = new SqlConnection(this.StrConAMSSql.Replace("@BASE", dirBaseDatos));
            cnAux.Open();
            SqlCommand cmd = new SqlCommand(consulta, cnAux);
            cmd.CommandTimeout = 0;
            resul = cmd.ExecuteScalar();
            cnAux.Close();
            exmut.Set();
            return resul;
        }
      
        public Object EjEscalar(String consulta)
        {
            object resul = null;
            exmut.WaitOne();
            if (cn.State != ConnectionState.Open) cn.Open();
            SqlCommand cmd = new SqlCommand(consulta, cn);
            cmd.CommandTimeout = 0;
            resul = cmd.ExecuteScalar();
            cn.Close();
            exmut.Set();
            return resul;
        	
        }
       
          public string[] BakupSQL(string nomTabla){
			List<string> bakup = new List<string>();
			DataTable tb = this.EjecutarSqlSelect(nomTabla,"Select * From "+nomTabla);
			for(int i= 0;i<tb.Rows.Count;i++){
			  bakup.Add(UtilidadesReg.ExConsultaNoSelet(UtilidadesReg.DeDataRowARegistro(tb,i)));	
			}
			return bakup.ToArray();
	   }
        
	   
        public int EjConsultaNoSelect(string tabla, String consulta)
        {
        	int resul = -1;
        	exmut.WaitOne();
       		if(cn.State != ConnectionState.Open) cn.Open();
            SqlCommand cmd = new SqlCommand(consulta,cn);
            cmd.CommandTimeout = 0;
            resul = cmd.ExecuteNonQuery();
            cn.Close();
            exmut.Set();
        	return resul;
        	
        }
        
        public int EjConsultaNoSelect(string tabla, String consulta, string dirBaseDatos)
        {
           int resul = -1;
           exmut.WaitOne();
            SqlConnection cnAux = new SqlConnection(this.StrConAMSSql.Replace("@BASE",dirBaseDatos));
              if(cnAux.State != ConnectionState.Open) cnAux.Open();
              SqlCommand cmd = new SqlCommand(consulta,cnAux);
              cmd.CommandTimeout = 0;
              resul = cmd.ExecuteNonQuery();
              cnAux.Close();
           exmut.Set();
            return resul;
        }
        
       public int ModificarReg(Registro reg){
         
          int res = -1;
          	  res = this.EjConsultaNoSelect(reg.NomTabla,UtilidadesReg.ExConsultaNoSelet(reg));
			return res;
         }
        
       
        
        public int ModificarReg(Registro reg, string baseDatos){
          int res = -1;
            res = this.EjConsultaNoSelect(reg.NomTabla,UtilidadesReg.ExConsultaNoSelet(reg),baseDatos);    
			return res;
         }
        
        public int NumRegEnTabla(string tabla){
           exmut.WaitOne();
           if(cn.State == ConnectionState.Closed) cn.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM "+tabla, cn);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int numReg  = dr.GetInt32(0);
            cn.Close();
        	exmut.Set();
            return numReg;
         }
	     
         public int NumRegEnTabla(string tabla, string baseDatos){
          exmut.WaitOne();
            SqlConnection  cnAux = new SqlConnection(this.StrConAMSSql.Replace("@BASE",baseDatos));
	        cnAux.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM "+tabla, cnAux);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int numReg  = dr.GetInt32(0);
            cnAux.Close();
           exmut.Set();
            return numReg;
         }
	         
        
    }
}
