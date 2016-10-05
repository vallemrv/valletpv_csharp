// StringParaSql.cs created with MonoDevelop
// User: valle at 0:13 19/03/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Text;
using Valle.SqlUtilidades.Tools;

namespace Valle.SqlUtilidades
{
    
    public class CadenasParaSql
    {

		
		public static string GetStrSelectParms(params ClausulaWhere[] sqlWhere){
		  string sql = "";
		   if(sqlWhere.Length>0){
			 sql = " WHERE " +  sqlWhere[0].getClausula();
			for(int i= 1;i<sqlWhere.Length;i++){
			  sql+= " AND "+ sqlWhere[i].getClausula();	
			}
			}
			return sql;
		}
		
		
        public static string CrearConsultaIntervaloHora(string nomColumna, string hInicio, string hfin)
        {
            string consulta = "";
            if (DateTime.Parse(hInicio) <= DateTime.Parse(hfin))
            {
                consulta = "(("+nomColumna + " >= '" + hInicio + "') AND (" + nomColumna + " <= '" + hfin + "'))";
            }
            else
            {
                consulta = "((("+nomColumna + " >= '" + hInicio + "') AND (" + nomColumna + " <= '24:59')) OR "+
                    "(("+nomColumna + " >= '00:00') AND (" + nomColumna + " <= '" + hfin + "')))"; 
            }

            return consulta;

        }

        public static string CrearConsultaUpdate(string tabla, string _set, string _where){
                string consulta = "UPDATE @TABLA SET @SET WHERE @WHERE";
                   consulta = consulta.Replace("@TABLA",tabla);
                   consulta = consulta.Replace("@SET", _set);
                   consulta = consulta.Replace("@WHERE", _where);
               return consulta;       
        
        }
        
        public static string CrearConsultaInsert(string tabla, string column, string valores){
               string consulta = "INSERT INTO @TABLA (@COLUMNAS) VALUES (@VALORES)";
               consulta = consulta.Replace("@TABLA",tabla);
               consulta = consulta.Replace("@COLUMNAS", column);
               consulta = consulta.Replace("@VALORES", valores);
               return consulta;
        }
     
        public static string CrearConsultaDel(string tabla, string where){
            string consulta = "DELETE FROM @TABLA WHERE @WHERE";
               consulta = consulta.Replace("@TABLA",tabla);
               consulta = consulta.Replace("@WHERE", where);
               return consulta;
        }
        
        public static string EncapsularCadenaSelect(string cadenaSelect){
             return cadenaSelect.Replace("'","!");       
        }
        
        public static string NormalizarCadenaSelect(string cadenaSelect){
           if(cadenaSelect.Contains("!")){
              return cadenaSelect.Replace("!","'");
             }else{
              return cadenaSelect;
             }
        }
        
        public static string EmpaquetarSegunTipo(object dato){
            switch(dato.GetType().Name){
               case "String":
                  return "'"+dato+"'";
               case "Decimal":
                  return dato.ToString().Replace(',','.');
               case "Boolean":
                  if(dato.ToString().Equals("False"))
                    return "0";
                     else
                      return "1";
                 
               default:
                  return dato.ToString();
               
            }
			
	   
			
        
        }
		
		  public static string LimpiarSignosValor(string s, char remplazo){
          string resultado = s.Replace(',',remplazo);
          resultado = resultado.Replace('(',remplazo);
          resultado = resultado.Replace(')',remplazo);
          resultado = resultado.Replace('.',remplazo);
          resultado = resultado.Replace('/',remplazo);
          resultado = resultado.Replace('-',remplazo);
		  resultado = resultado.Replace('\'',remplazo);
		  resultado = resultado.Replace('&',remplazo);  	
			 
			
		   return resultado;
        }
    }
}
