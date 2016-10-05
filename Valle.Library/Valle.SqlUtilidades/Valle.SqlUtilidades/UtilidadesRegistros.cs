using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections;


namespace Valle.SqlUtilidades
{
    
    public enum AccionesConReg {Nada, Agregar, Modificar, Borrar }
    
    public class UtilidadesReg{
        
        public static DataRow DeRegistroADataRow(DataRow r, Registro reg){
                        foreach(Columna c in reg.Columnas){
                           r[c.nombreColumna] = c.valor;
                        }
                   return r;
        }
        
        public static Registro DeDataRowARegistro(DataTable tb, int pos){
           if(tb.Rows.Count>pos){
		       Registro reg = new Registro();
                  reg.NomTabla=tb.TableName;
                  for(int i=0;i<tb.Columns.Count;i++){
                     Columna c = new Columna();
                     c.nombreColumna =  tb.Columns[i].ColumnName;
                     c.valor = tb.Rows[pos][tb.Columns[i].ColumnName];
                     reg.Columnas.Add(c);
                   }
                   return reg;      
		    }else
             return null;
          }
         
         public static Registro DeDataRowARegistro(DataColumnCollection cs, DataRow r){
              Registro reg = new Registro();
                  reg.NomTabla= r.Table.TableName;
                  for(int i=0;i< cs.Count;i++){
                     Columna c = new Columna();
                     c.nombreColumna =  cs[i].ColumnName;
                     c.valor = r[c.nombreColumna];
                     reg.Columnas.Add(c);
                   } 
                   return reg;
           }
         
         public static void ModificarRegEnTabla(Registro reg, DataTable tb){
           DataRow[] rs;
           switch(reg.AccionReg){
             case AccionesConReg.Agregar:
                   tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));     
             break;
             case AccionesConReg.Borrar:
                 rs = tb.Select(CadenasParaSql.NormalizarCadenaSelect(reg.CadenaSelect));
                   foreach(DataRow r in rs)
                                   r.Delete();
             break;
             case AccionesConReg.Modificar:
                        rs = tb.Select(CadenasParaSql.NormalizarCadenaSelect(reg.CadenaSelect));
                          if(rs.Length>0) UtilidadesReg.DeRegistroADataRow(rs[0],reg);
             break;
           }
          
         }
         
		public static string ExConsultaNoSelet(DataRow dr, AccionesConReg accion, string cadenaSelect){
         
          string res = "";
           switch(accion){
             case AccionesConReg.Agregar:
                        StringBuilder valores = new StringBuilder();
			            StringBuilder columnas = new StringBuilder();
			             foreach (DataColumn col in dr.Table.Columns){
			               if(!dr[col.ColumnName].GetType().Name.Equals("DBNull")){
			                  columnas.Append(col.ColumnName + ",");
			                  valores.Append( CadenasParaSql.EmpaquetarSegunTipo(dr[col.ColumnName])+",");
			                  }
			             }
			            columnas.Remove(columnas.Length-1,1);
			            valores.Remove(valores.Length-1,1);
			     res =  CadenasParaSql.CrearConsultaInsert(dr.Table.TableName,columnas.ToString(),
			                                                              valores.ToString());
             break;
             case AccionesConReg.Borrar:
                 res =  CadenasParaSql.CrearConsultaDel(dr.Table.TableName, cadenaSelect);
             break;
             case AccionesConReg.Modificar:
                        StringBuilder colVal = new StringBuilder();
			              foreach (DataColumn col in dr.Table.Columns){
			                if(!dr[col.ColumnName].GetType().Name.Equals("DBNull"))
			                  colVal.Append(col.ColumnName + "="+ CadenasParaSql.EmpaquetarSegunTipo(dr[col.ColumnName])+",");
			             }
			            colVal.Remove(colVal.Length-1,1);
			    res =  CadenasParaSql.CrearConsultaUpdate(dr.Table.TableName, colVal.ToString(),cadenaSelect);
			                                        
             break;
           }
           return res;
         }

		
		
         public static string ExConsultaNoSelet(Registro reg){
         
          string res = "";
           switch(reg.AccionReg){
             case AccionesConReg.Agregar:
                        StringBuilder valores = new StringBuilder();
			            StringBuilder columnas = new StringBuilder();
			             foreach (Columna col in reg.Columnas){
			               if(!col.valor.GetType().Name.Equals("DBNull")){
			                  columnas.Append(col.nombreColumna + ",");
			                  valores.Append( CadenasParaSql.EmpaquetarSegunTipo(col.valor)+",");
			                  }
			             }
			            columnas.Remove(columnas.Length-1,1);
			            valores.Remove(valores.Length-1,1);
			     res =  CadenasParaSql.CrearConsultaInsert(reg.NomTabla,columnas.ToString(),
			                                                              valores.ToString());
             break;
             case AccionesConReg.Borrar:
                 res =  CadenasParaSql.CrearConsultaDel(reg.NomTabla, reg.CadenaSelect);
             break;
             case AccionesConReg.Modificar:
                        StringBuilder colVal = new StringBuilder();
			             foreach (Columna col in reg.Columnas){
			                if(!col.valor.GetType().Name.Equals("DBNull"))
			                  colVal.Append(col.nombreColumna + "="+ CadenasParaSql.EmpaquetarSegunTipo(col.valor)+",");
			             }
			            colVal.Remove(colVal.Length-1,1);
			    res =  CadenasParaSql.CrearConsultaUpdate(reg.NomTabla, colVal.ToString(),reg.CadenaSelect);
			                                        
             break;
           }
           return res;
         }
		
         public static AccionesConReg StringToAccionesReg (string accion){
		   	 AccionesConReg accionReg = AccionesConReg.Nada;
			  switch (accion){
			case "1":
				accionReg = AccionesConReg.Agregar;
				break;
			case "2":
				accionReg = AccionesConReg.Modificar;
				break;
			case "3":
				accionReg = AccionesConReg.Borrar;
				break;
			default:
				accionReg = AccionesConReg.Nada;
					break;
			}
			return accionReg;
		  }
        
    }

    [Serializable]
    public class Columna : ISerializable
    {
        public string nombreColumna = "";
        public object valor = null;
		
        public Columna(SerializationInfo info, StreamingContext context)
        {
            nombreColumna = info.GetString("nombreColumna");
            valor = info.GetValue("valor",typeof(object));
        }

        public Columna() { }

        #region Miembros de ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("nombreColumna", nombreColumna);
            info.AddValue("valor", valor);
        }

        #endregion
    }

    [Serializable]
    public class Registro : ISerializable
    {
        private string nomTabla = "";

        public string NomTabla
        {
            get { return nomTabla; }
            set { nomTabla = value; }
        }
       [NonSerialized] private List<Columna> columnas;

        public List<Columna> Columnas
        {
            get { return columnas; }
            set { columnas = value; }
        }
        private string cadenaSelect = "";

        public string CadenaSelect
        {
            get { return cadenaSelect; }
            set { cadenaSelect = value; }
        }
        private AccionesConReg accionReg = AccionesConReg.Agregar ;

        public AccionesConReg AccionReg
        {
            get { return accionReg; }
            set { accionReg = value; }
        }
        public Registro()
        {
            columnas = new List<Columna>();
        }
        public Registro(SerializationInfo info, StreamingContext context)
        {
            columnas = new List<Columna>();
        
            NomTabla = info.GetString("NomTabla");
            AccionReg = (AccionesConReg)info.GetValue("AccionReg",
                         typeof(AccionesConReg));
            CadenaSelect = info.GetString("CadenaSelec");
            int numColumnas = info.GetInt32("NumColumnas");
           
            for (int i = 0; i < numColumnas; i++)
            {
                Columnas.Add((Columna)info.GetValue("Columna" + i, typeof(Columna)));

            }
            
        }

       

        #region Miembros de ISerializable

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("NomTabla", NomTabla);
            info.AddValue("AccionReg", AccionReg, typeof(AccionesConReg));
            info.AddValue("CadenaSelec", CadenaSelect);
            info.AddValue("NumColumnas", columnas.Count);
           
            for (int i = 0; i < columnas.Count; i++)
            {
                info.AddValue("Columna" + i, columnas[i], typeof(Columna));

            }
            
      
        }
      #endregion
    }
}
