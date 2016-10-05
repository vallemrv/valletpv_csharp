// GesBaseLocal.cs created with MonoDevelop
// User: valle at 0:44 07/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Valle.SqlUtilidades;
using Valle.SqlGestion;
using Valle.Utilidades;




namespace Valle.GesTpv
{
	public enum TablaActualizar {Tpv, Familias, Usuarios, Favoritos, Zonas, Articulos, Secciones, Teclas, TeclasFav, SeccionesTpv}
	public delegate void OnEnviarInformacion(object inf);
	
	public class GesBaseLocal
	{
	    //lista de tablas que pertenezera a todos los tpv gestionados
       public String[] listaTablasComunes = new string[]{"Familias","Colores","Controles","Favoritos",
	                       "Secciones", "Articulos", "Teclas", "TeclasFav", "VentaPorKilos", "ArticuloNoVenta",
	                       "DesgloseArt"};
	    
	   //lista de las tablas que tendran distindos datos segun el tpv que pertenezca                                          
	   public String[] listaTablasIndividual = new string[]{"TPVs","Camareros","InstComision", "Configuracion","Rutas", "Zonas","Mesas",
	                          "ZonasTpv","FavoritosTpv","SeccionesTpv",  "ResComision",
	                          "Usuarios","Privilegios","SuperUsuario","Almacen","Inventarios",
	                          };
	                          
	    //Lista de tablas que tendra el gestor local mente que que se tendran que sincronizar con los tpv                      
	   public String[] listaTablasLocales = new string[]{"Familias","Colores","Controles","Favoritos",
	                       "Secciones","Articulos", "Teclas","VentaPorKilos","ArticuloNoVenta",
	                       "DesgloseArt","TPVs","Camareros", "InstComision",  "Configuracion","Rutas", "Zonas","Mesas",
	                       "ZonasTpv","FavoritosTpv","TeclasFav","SeccionesTpv",
	                       "Usuarios","Privilegios","SuperUsuario", "Almacen", "Inventarios",
	                       };
	                       
	   //esta lista es porque al ir exportando las tablas comunes se borraran algunas privadas
	   //en cascada
	   public String[] listaTablasExportables = new string[]{"Familias","Colores","Controles","Favoritos",
	                       "Secciones","Articulos","Teclas","VentaPorKilos","ArticuloNoVenta", "Zonas","Mesas","ZonasTpv",
	                       "DesgloseArt","Inventarios","TeclasFav","FavoritosTpv","SeccionesTpv"};
	                                                 
	                          
	   //toda las tablas de al base de datos ordenada segun claves externas para la correcta inportacion y exportacion                    
	   public String[] listaTodasLasTablas = new string[]{"Familias","Colores","Controles","Favoritos",
	                       "Secciones","Articulos", "Teclas","VentaPorKilos","ArticuloNoVenta",
	                       "DesgloseArt","TPVs","Camareros", "InstComision",  "Configuracion","Rutas", "Zonas","Mesas",
	                       "ZonasTpv","FavoritosTpv","TeclasFav","SeccionesTpv","Ticket", "CierreDeCaja", "ResComision", "LineasTicket",
	                       "Usuarios","Privilegios","SuperUsuario", "Almacen", "Inventarios",
	                       "GestionMesas","Rondas", "LineasNulas", "LineasRonda"};
	                          
       public  const  string NOM_TB_SINCRONIZADOS = "Sincronizados";
	   public  const  string TABLA_PERTENENCIA= "TablaPertenencia";
	   public  const  string CADENA_SELECT = "CadenaSelect";
	   public  const  string ACCION="Accion";
	   public  const  string IDSERV = "IDServidor";
       private        string NOM_CONJUNTO_DATOS = "BaseTpvComun";
       
	   
	    public GesServidores gesServ;
        
        private  GesMySQL               gsMysql;
	    private  string                 servidorActivo;
	    private  Thread                 hGuardarFich;
	    private  AutoResetEvent colaVacia; 
	    private  ColaCompartida<Registro> cola; 
	          
		
	  
		public GesBaseLocal(GesServidores ges)
		{
		        this.gesServ = ges;
			    this.gsMysql = new GesMySQL("nitrogeno","root","3306");
		        this.colaVacia = new AutoResetEvent(true);
		        this.cola = new ColaCompartida<Registro>(colaVacia);
		        
		       if(!gsMysql.ContieneBaseDatos(this.NOM_CONJUNTO_DATOS))
		                         gsMysql.CrearBaseDatos(this.NOM_CONJUNTO_DATOS);
		            
		            
		    	
               if(!this.ContieneTabla(GesBaseLocal.NOM_TB_SINCRONIZADOS))  this.CrearTablaSinc();
                     
                                                          
                
                  if(gesServ.ServidorActivo != null){
                      this.CambiarServidor(gesServ.ServidorActivo["Nombre"].ToString()); 
                   }else
                     this.servidorActivo = "";
                
               
                this.hGuardarFich = new Thread(new ThreadStart(this.HiloGuradarDatos));
                this.hGuardarFich.Start();
		}
		
		
		
		private void CrearTablaSinc()
		{
			 
			//Crea la tabla de sincronizados 
			 StringBuilder sb = new StringBuilder("CREATE TABLE "+GesBaseLocal.NOM_TB_SINCRONIZADOS +" (");
			    sb.Append("IDVinculacion INT NOT NULL AUTO_INCREMENT,");
			    sb.AppendFormat("{0} VARCHAR(50) NOT NULL, ",GesBaseLocal.TABLA_PERTENENCIA);
			    sb.AppendFormat("{0} VARCHAR(50) NOT NULL,  ",GesBaseLocal.CADENA_SELECT);
			    sb.AppendFormat("{0} TINYINT UNSIGNED NOT NULL CHECK {0}>=0 AND {0}<=3, ",GesBaseLocal.ACCION);
			    sb.AppendFormat("{0} INT NOT NULL, ",GesBaseLocal.IDSERV);
			    sb.Append("PRIMARY KEY (IDVinculacion) ");
			      sb.Append(")");
				gsMysql.EjConsultaNoSelect(GesBaseLocal.NOM_TB_SINCRONIZADOS,sb.ToString(),
		    	                                    this.NombreBaseDatos(NOM_TB_SINCRONIZADOS));
		 }
	     
	
	  
		
		
     //Devuelve el nombre de la base de datos donde esta la tabla
     public string NombreBaseDatos(string tabla){
       string baseDatos = "";
    	    if((tabla.Equals(GesBaseLocal.NOM_TB_SINCRONIZADOS))||
    	                    (this.PertenezeA(this.listaTablasComunes,tabla))){
    	                          baseDatos = this.NOM_CONJUNTO_DATOS;
                 			}else if(this.PertenezeA(this.listaTablasIndividual,tabla)){
                 			   baseDatos = this.servidorActivo;
          	                }
         return baseDatos; 	                
       }
	
	//Si existe devuelve la tabla, sino, null.
	private DataTable CargarTabla(string tabla){
	      DataTable tb = null;
	      string baseDatos = this.NombreBaseDatos(tabla);
	         if(gsMysql.ContienTabla(tabla,baseDatos)){
    	        	tb = gsMysql.EjecutarSqlSelect(tabla,"SELECT * FROM "+tabla,baseDatos);
    	    	 }
    	return tb;
		
	  }

	//Establece la entrada de un registro modificado, borrado o añadido. Y la accion ha realizar con este.	
    private void ActualizarSincronizar(string id, String tablaPertenece, String CadenaSelect, AccionesConReg accion){
        DataTable tbAux = this.gsMysql.EjecutarSqlSelect("Sincronizados","SELECT * FROM Sincronizados WHERE "+
                           GesBaseLocal.IDSERV+" = "+ id +" AND "+ GesBaseLocal.CADENA_SELECT + " = '" + CadenaSelect+"'",
                           this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS));
                           
              if(tbAux.Rows.Count<=0){//Sin no se hizo nada con este registro con aterioridad
	                 DataRow dr = tbAux.NewRow();
	                    dr[GesBaseLocal.TABLA_PERTENENCIA] = tablaPertenece;
	                    dr[GesBaseLocal.CADENA_SELECT] = CadenasParaSql.EncapsularCadenaSelect(CadenaSelect);
	                    dr[GesBaseLocal.ACCION] = accion;
	                    dr[GesBaseLocal.IDSERV] = id;
	                    this.GuardarDatos(dr,"",AccionesConReg.Agregar);
	                    
	              }else{
				    // si se hizo algo con este registro con aterioridad 
				    DataRow drs = tbAux.Rows[0];
	               if(accion == AccionesConReg.Borrar){
	                 if (UtilidadesReg.StringToAccionesReg(drs[GesBaseLocal.ACCION].ToString()) 
	                                       == (AccionesConReg.Agregar)) {
	                     
	                     this.GuardarDatos("Sincronizados",
	                        GesBaseLocal.CADENA_SELECT+" = '"+ CadenasParaSql.EncapsularCadenaSelect(CadenaSelect)+"' AND "+
	                        GesBaseLocal.IDSERV +" = "+gesServ.ServidorActivo[GesBaseLocal.IDSERV].ToString(),AccionesConReg.Borrar);
	                        
	                   }else{
	                     
	                     drs[GesBaseLocal.ACCION] = accion;
	                     
	                     this.GuardarDatos(drs,GesBaseLocal.TABLA_PERTENENCIA+" = '"+tablaPertenece+"' AND "+
	                        GesBaseLocal.CADENA_SELECT+" = '"+ CadenasParaSql.EncapsularCadenaSelect(CadenaSelect)+"' AND "+
	                        GesBaseLocal.IDSERV +" = "+gesServ.ServidorActivo[GesBaseLocal.IDSERV].ToString(),AccionesConReg.Modificar);
	                   }
	                 }
	               }
	   
    }
    

    //hilo que guarda los datos en el disco
	private void HiloGuradarDatos(){
	 while(true){
	   Registro r = cola.Descolar();
	   this.gsMysql.ModificarReg(r,this.NombreBaseDatos(r.NomTabla));
     }
	}
        
    
     public int BorrarDatosTabla(string tabla){
         return this.gsMysql.EjConsultaNoSelect(tabla, "DELETE FROM "+tabla,  this.NombreBaseDatos(tabla));
      }

     
     /// <summary>
     /// Metodo utilizado para las consultas compuestas por mas de una tabla.
     /// </summary>
     /// <param name="nomConsulta">
     /// Nombre que tomara la tabla retornada
     /// </param>
     /// <param name="tablas">
     /// Array que contiene todas las tablas que intervienen en la consulta
     /// </param>
     /// <param name="select">
     /// Consulta de recuperacion de datos
     /// </param>
     /// <returns>
     /// la tabla con los datos de la consulta
     /// </returns>
     public DataTable EjConsultaSelect(string nomConsulta,string select, params string[] tablas){
		if(tablas != null){	
		  
		    foreach(string tabla in tablas){
              select = select.Replace(tabla+" ",NombreBaseDatos(tabla)+"."+tabla+" ");
              select = select.Replace(tabla+".",NombreBaseDatos(tabla)+"."+tabla+".");
             }
           }
        return this.gsMysql.EjecutarSqlSelect(nomConsulta,select,this.NOM_CONJUNTO_DATOS);
     }
     
     
     public DataTable ExtraerLaTabla(string tabla){
         return this.CargarTabla(tabla);		
	 }
	 
	 //carga una tabla y si la columna principal no es null la convierte en autoincrementada
	 public DataTable ExtraerLaTabla(string nomTabla, string columKey){
	       DataTable tb = this.CargarTabla(nomTabla);
	         if (columKey != null)//acciones para la columna autoincremantada
              {
                  DataView dw = tb.DefaultView;dw.Sort= columKey;
                  tb.Columns[columKey].AutoIncrement = true;
                  tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                  (int)dw[tb.Rows.Count - 1][columKey] + 1 : 1;
              }
	       return tb;
	 }
	 
	 public void SincronizadoServidor(){
    	 this.gsMysql.EjConsultaNoSelect("Sincronizados",
			                         "DELETE FROM Sincronizados WHERE IDServidor = "+this.gesServ.ServidorActivo["IDServidor"].ToString(),
			                         this.NombreBaseDatos("Sincronizados"));
			 if(gsMysql.NumRegEnTabla("Sincronizados",this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS))<=0)
	            gsMysql.EjConsultaNoSelect("Sincronizados","ALTER TABLE Sincronizados AUTO_INCREMENT = 1",
	                                                       this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS));                        
	 }
     
     public void SincronizadoServidor(string serv){
    	 this.gsMysql.EjConsultaNoSelect("Sincronizados",
			                         "DELETE FROM Sincronizados WHERE IDServidor = "+
			                         this.gesServ.ServidorActivo["IDServidor"].ToString(),
			                         this.NombreBaseDatos("Sincronizados"));
		
		if(gsMysql.NumRegEnTabla("Sincronizados",this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS))<=0)
	            gsMysql.EjConsultaNoSelect("Sincronizados","ALTER TABLE Sincronizados AUTO_INCREMENT = 1",
	                                                       this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS));	                         
	 }
     
	 
     public void SincronizadaTabla(string tabla){
          this.gsMysql.EjConsultaNoSelect("Sincronizados",
			                         "DELETE FROM Sincronizados WHERE IDServidor = "+this.gesServ.ServidorActivo["IDServidor"].ToString()+
			                                  " AND  TablaPertenencia = '"+tabla+ "'", this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS));
	    
	    if(gsMysql.NumRegEnTabla("Sincronizados",this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS))<=0)
	            gsMysql.EjConsultaNoSelect("Sincronizados","ALTER TABLE Sincronizados AUTO_INCREMENT = 1",
	                                                       this.NombreBaseDatos(GesBaseLocal.NOM_TB_SINCRONIZADOS));
     }
		
	 	
        
     //caraga el conjunto de datos para el servidor selecionado
   	 public void CambiarServidor(string servidor){
             
		     if(!this.gsMysql.ContieneBaseDatos(servidor))
		                              this.gsMysql.CrearBaseDatos(servidor);
		                              
		     this.servidorActivo = servidor;
		}
	
	 public void GuardarDatos(Registro r){
	      cola.Encolar(r);
	 }
	
	
	 public void GuardarDatos(string tabla, string cadenaSelect, AccionesConReg accion){
      Registro r;
	        r = new Registro();
	        r.NomTabla = tabla;
            r.AccionReg = accion;
            r.CadenaSelect = cadenaSelect;
            cola.Encolar(r);
     }
	
	 public void GuardarDatos(DataRow dr, string cadenaSelect, AccionesConReg accion){
	 
	 Registro r;
	  r = UtilidadesReg.DeDataRowARegistro(dr.Table.Columns,dr);
       dr.Table.AcceptChanges();
        r.AccionReg = accion;
         r.CadenaSelect = cadenaSelect;
          cola.Encolar(r);
           
	 }
	
	 public int CrearTabla(string tabla, string createTabla){
	    return this.gsMysql.EjConsultaNoSelect(tabla,createTabla,this.NombreBaseDatos(tabla));
	 }
     
	 public void Terminar(){
	         while (this.cola.NumElementos > 0)
                {
                   this.colaVacia.WaitOne();
                }
                this.hGuardarFich.Abort();
         }
	
     //Elimina un servidor completo
     public void EliminarDatosServ(string serv)
     {
        this.gsMysql.EliminarBaseDatos(serv);
        this.SincronizadoServidor(serv);
     }
     
    
     //para saber si la columna pertenece al conjunto de datos
	 public bool ContieneTabla(String tabla){
	     bool esta = this.gsMysql.ContienTabla(tabla,this.NombreBaseDatos(tabla));
	      return esta;
	 }	 
	
	 
	//numero de tablas que tiene el servidor activo 
	//comunes y no comunes
	 public int NumeroDeTablas(){
	   return this.gsMysql.NumTablasBase(this.NOM_CONJUNTO_DATOS)+this.gsMysql.NumTablasBase(this.servidorActivo);
	 }
	 
		
	//para saber a que lista de tablas pertenece esta tabla (individual, todas, exportables, etc..);	
    public bool PertenezeA(string[] tablas, string tabla){
       bool contiene = false;
       foreach (string s in tablas){
        if(s.Equals(tabla)){
          contiene = true;
        }
       }
       return contiene;
    }
    
    
     
	
    
    //Actualizacion para modificacion, borrado o añadido de registros manual
	//compruebo si es una tabla comun, si es, la modifico entodos los servidores, y sino, en solo uno
	public void ActualizarSincronizar(String tablaPertenece, String CadenaSelect, AccionesConReg accion){
	        if(this.PertenezeA(listaTablasComunes, tablaPertenece)){
	            foreach(DataRow drServ in gesServ.TbServidores.Rows){
	                ActualizarSincronizar(drServ["IDServidor"].ToString(),tablaPertenece,CadenaSelect,accion);
	                }
	           }else{
	                ActualizarSincronizar(gesServ.ServidorActivo["IDServidor"].ToString(),tablaPertenece,CadenaSelect,accion);
	           }
		   }
	       
	
	
	
     
	}
}