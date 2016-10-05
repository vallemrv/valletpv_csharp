/*
 * Creado por SharpDevelop.
 * Usuario: valle
 * Fecha: 16/09/2009
 * Hora: 23:36
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Data;
using Valle.SqlGestion;

namespace Valle.ToolsTpv
{
	/// <summary>
	/// Description of GesAlmacen.
	/// </summary>
	public class GesAlmacen
	{
		IGesSql gesLocal;
		public GesAlmacen(IGesSql gesLocal)
		{
		   this.gesLocal = gesLocal;	
		}
		
		public void GestionarArticulo(string idArticulo, decimal cant){
		DataTable  tbInventarios = gesLocal.EjecutarSqlSelect("Inventarios","SELECT * FROM Inventarios WHERE IDArt = "+idArticulo+
			                                       " ORDER BY Nivel DESC");
			/*for(int i = 0;i<tbInventarios.Rows.Count;i++){
				decimal stockRes = (decimal)tbInventarios.Rows[i]["Stock"]-cant;
			    	if(stockRes>0)||(i == tbInventarios.Rows.Count-1))
					      r["Stock"] = stockRes;
			    	else{
			    		cant = cant-(decimal)tbInventarios.Rows[i]["Stock"];
			    		tbInventarios.Rows[i]["Stock"] = 0;
			    	}
				  
			}*/
		}
		
		void GestinarArtDesglose(string idArticulo){
			DataTable tbDesglArt = gesLocal.EjecutarSqlSelect("DesgloseArt","SECLET * FROM Inventarios WHERE IDArtPrimario="+idArticulo);
			
			                                                  
			                                                  
		}
	}
}
