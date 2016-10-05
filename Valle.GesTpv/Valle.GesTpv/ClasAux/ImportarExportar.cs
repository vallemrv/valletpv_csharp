﻿// ImportarExportar.cs created with MonoDevelop
// User: valle at 0:15 05/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Threading;

using Valle.Distribuido.SQLRemoting;
using Valle.SqlGestion;
using Valle.SqlUtilidades;
using Valle.GtkUtilidades;

namespace Valle.GesTpv
{
	
	public delegate void OnOperacionTerminada();
	
	public class ImportarExportar  : IInfProgreso
	{
	
	  GesBaseLocal gesLocal;
	  SQLClient gesRemoto;
	  DataRow drServActivo;
	  string[] listaTablas;
	  Splash sp;
	  Thread hSplas;
	  event OnOperacionTerminada opTerminada;
	  
	
	 
	     public ImportarExportar(GesBaseLocal gsL, SQLClient gsR, OnOperacionTerminada opTerm){
	       gesLocal = gsL;
	       gesRemoto=gsR;
	       drServActivo = gesLocal.gesServ.ServidorActivo;
	       opTerminada += opTerm;
	     }
		
	 	 public void Importar(string[] ListaTablasImp){
		    this.listaTablas = ListaTablasImp;
		     sp = new Splash("Importando tablas", Rutas.Ruta_Directa(Rutas.IMG_APP+System.IO.Path.DirectorySeparatorChar+"comunicacion.gif"),true);
	         hSplas = new Thread(new ThreadStart(this.gestionarLaImportacion));
			 sp.Show();
			 hSplas.Start();
		 }
		
		 private void gestionarLaImportacion(){
	       ImportarAsync impAsync = new ImportarAsync(this,gesRemoto,gesLocal);
	       sp.MostrarInformacion(new InfAMostrar("Importando la lista de tablas seleccionada",
	                                               TipoInfMostrar.MensajeLabel));
		
        	     double progreso = 0;
        		 double maxProceso = this.listaTablas.Length;
        		  
        		 foreach(String tabla in this.listaTablas){
        		     sp.MostrarInformacion(new InfAMostrar(String.Format("Importando la tabla {0}, {1}/{2}",tabla,progreso+1,maxProceso)
        		       	                                              ,TipoInfMostrar.MensajeBarra));
        		       	                                              
        		         sp.MostrarInformacion(new InfAMostrar(progreso++/maxProceso,TipoInfMostrar.progresoBarr));
        		         impAsync.ImportarAsyncTabla(tabla);
        		        
        		     this.SincronizadaTablaRemota(tabla);
        		     gesLocal.SincronizadaTabla(tabla);
				 }
        		 sp.Destroy();
        		 this.opTerminada();
            }
	
		
		public void Exportar(string[] listaAExportar){
		  this.listaTablas = listaAExportar;
		  sp = new Splash("Exportando tablas", Rutas.Ruta_Directa(Rutas.IMG_APP+System.IO.Path.DirectorySeparatorChar+"comunicacion.gif"), true);
		             hSplas = new System.Threading.Thread(new ThreadStart(this.gestionarLaExportacion));
		             sp.Show();
			         hSplas.Start();
			 
		  
	    }
	    
	    private void gestionarLaExportacion(){
	       sp.MostrarInformacion(new InfAMostrar("Exportando las tablas seleccionadas",TipoInfMostrar.MensajeBarra));
		        
	    
	    double progreso = 0;
	    double maxProceso = this.listaTablas.Length ;
	          
		 foreach(String tabla in this.listaTablas){
		      DataTable tbAux = gesLocal.ExtraerLaTabla(tabla);
		      sp.MostrarInformacion(new InfAMostrar(progreso++/maxProceso,TipoInfMostrar.progresoBarr));
		        
		           if(tbAux!=null){
		           
		             sp.MostrarInformacion(new InfAMostrar(
		                         String.Format("Borrando la tabla {0} puede tardar un poco.....",tabla)
		                                               ,TipoInfMostrar.MensajeBarra));
		     
		                    
		              gesRemoto.BorrarDatosTabla(tabla);
		              
		               sp.MostrarInformacion(new InfAMostrar(
		                         String.Format("Enviando la tabla {0}, {1}/{2}",tabla,progreso,maxProceso)
		                                               ,TipoInfMostrar.MensajeBarra));
		     
			           
			        
		              for(int r=0;r<tbAux.Rows.Count;r++){
			                 
			                 Registro regRemoto = SqlUtilidades.UtilidadesReg.DeDataRowARegistro(tbAux,r);
			                 regRemoto.NomTabla=tabla;
			                 regRemoto.AccionReg = AccionesConReg.Agregar;
			                 gesRemoto.ModificarReg(regRemoto);
			                 Gtk.Application.Invoke(delegate{
			                 MostrarInformacion(r,tbAux.Rows.Count);});
		                    }
			          this.SincronizadaTablaRemota(tabla);
			          gesLocal.SincronizadaTabla(tabla);
		             
		           
		          }
	           
	     }
	     sp.Destroy();
	     this.opTerminada();
	    }
	    
	    public void Cancelar(){
		  if((hSplas!=null)&&(hSplas.IsAlive)) hSplas.Abort();
	      
	   }
	    public void VisualizarSplash(bool ver){
	     if(sp!=null) sp.KeepAbove = ver;
	   }
	   
	    private void gestionarLaSincronizacion(){
	   
         #region sincronizar el gestor
		 sp.MostrarInformacion(new InfAMostrar("Sincronizando con el servidor "+this.drServActivo["nombre"].ToString(),TipoInfMostrar.MensajeBarra));
		 
            			
	         DataTable tb = this.gesRemoto.ExtraerTabla(GesBaseLocal.NOM_TB_SINCRONIZADOS);
	         double progreso = 0;
	         double total = tb.Rows.Count;
		  
	       foreach(string tabla in gesLocal.listaTablasLocales){
	        DataRow[] rs = tb.Select(GesBaseLocal.TABLA_PERTENENCIA +"='"+ tabla+"'");
	         foreach(DataRow r in rs){
				 sp.MostrarInformacion(new InfAMostrar("Extrayendo registros ",TipoInfMostrar.MensajeBarra));
	             sp.MostrarInformacion(new InfAMostrar(progreso++/total,TipoInfMostrar.progresoBarr));
		 	
	          
	            Registro reg = null;;
	            string select = r[GesBaseLocal.CADENA_SELECT].ToString();
	            AccionesConReg accion = Valle.SqlUtilidades.UtilidadesReg.StringToAccionesReg(r[GesBaseLocal.ACCION].ToString());
	            switch(accion){
                  case AccionesConReg.Borrar:
                     reg = new Registro();
                  break;
                  case AccionesConReg.Modificar:
                  case AccionesConReg.Agregar:
                   List<Registro> listReg = this.gesRemoto.EjConsultaSelectEnReg(tabla,
	                  "SELECT * FROM "+tabla+ " WHERE "+SqlUtilidades.CadenasParaSql.NormalizarCadenaSelect(select));
	                  if(listReg.Count>0)
	                        reg= listReg[0];
	              break;
	              
			     }
			     if(reg!=null){
			      reg.NomTabla = tabla;
	              reg.CadenaSelect = select;
	              reg.AccionReg = accion;
	              gesLocal.GuardarDatos(reg);
	             }
	           }  
	            this.gesRemoto.EjConsultaNoSelect("Sincronizados","DELETE FROM Sincronizados WHERE "+GesBaseLocal.TABLA_PERTENENCIA +"='"+tabla+"'");
		     
	         }
            #endregion 	         
			
	         
	         tb = this.gesLocal.ExtraerLaTabla(GesBaseLocal.NOM_TB_SINCRONIZADOS);
	         progreso = 0;
	         total = tb.Rows.Count;
	     
	       foreach(string tabla in gesLocal.listaTablasLocales){
	         DataRow[] rs = tb.Select(GesBaseLocal.TABLA_PERTENENCIA +"='"+ tabla+"'");
	        
	         foreach(DataRow r in rs){
            
			   sp.MostrarInformacion(new InfAMostrar("Enviando registros ",TipoInfMostrar.MensajeBarra));
	           sp.MostrarInformacion(new InfAMostrar(progreso++/total,TipoInfMostrar.progresoBarr));
		 	
	          Registro reg = null;
	          string select = r[GesBaseLocal.CADENA_SELECT].ToString();
				
	           AccionesConReg accion = Valle.SqlUtilidades.UtilidadesReg.StringToAccionesReg( r[GesBaseLocal.ACCION].ToString());
	           switch(accion){
                  case AccionesConReg.Borrar:
                     reg = new Registro();
                  break;
                  case AccionesConReg.Modificar:
                  case AccionesConReg.Agregar:
                    DataTable tbReg = this.gesLocal.EjConsultaSelect(tabla,
	                             "SELECT * FROM "+tabla+ " WHERE "+SqlUtilidades.CadenasParaSql.NormalizarCadenaSelect(select),tabla);
				  if(tbReg.Rows.Count > 0){
	                reg = SqlUtilidades.UtilidadesReg.DeDataRowARegistro(tbReg,0);
	              }
				
	              break;
	              
			     }
			    
	              if(reg!=null){         	
				    reg.NomTabla = tabla;
	                reg.CadenaSelect = select;
	                reg.AccionReg = accion;
	                gesRemoto.ModificarReg(reg);
	                
	              }
		      }
			}
			
	         gesLocal.SincronizadoServidor();
	         this.opTerminada();
	         sp.Destroy();
	  }
	   
	   
	 
			 
	   public void Sincronizar(){
	       sp = new Splash("Sincronizando equipos", Rutas.Ruta_Directa(Rutas.IMG_APP+System.IO.Path.DirectorySeparatorChar+"comunicacion.gif"), true);
	       sp.Show();
	       this.VisualizarSplash(true);
		   hSplas = new System.Threading.Thread(new ThreadStart(this.gestionarLaSincronizacion));
		   hSplas.Start();
	   }


	   public void MostrarInformacion (int procesados, int total)
       {
                sp.MostrarInformacion(new InfAMostrar((double)procesados/total, TipoInfMostrar.progresoBarr, DirBarra.BarDos));
                sp.MostrarInformacion(new InfAMostrar(String.Format("Procesando registros ...  {0}/{1}",procesados,total),TipoInfMostrar.MensajeBarra,DirBarra.BarDos));
       }
       
       public int SincronizadaTablaRemota(string tabla){
			   return this.gesRemoto.EjConsultaNoSelect("Sincronizados", 
			                                                  "DELETE FROM Sincronizados WHERE TablaPertenencia = '"+tabla+ "'");
     	}

	}
	
	
	public class ImportarAsync{
	
	        
	         
	        SQLClient gesRemoto;
	        GesBaseLocal gesLocal;
	        GtkUtilidades.IInfProgreso mostrar;
	        
	        public ImportarAsync(GtkUtilidades.IInfProgreso mostrar, SQLClient gesRemoto, GesBaseLocal gesLocal){
	               this.gesLocal = gesLocal;
	               this.gesRemoto = gesRemoto;
	               this.mostrar = mostrar;
	         
	        
	        }
	         
            public void ImportarAsyncTabla(string tabla){
	             if(gesLocal.ContieneTabla(tabla))
	                        gesLocal.BorrarDatosTabla(tabla);
	                      else{
	                      
	                       
	                        Esquema esq = gesRemoto.ExtraerSqlCreateTable(tabla);
	                        Console.WriteLine(gesRemoto.NombreTablas[2]);
	                        /*foreach (ClaveExt cl in esq.clavesExt){
	                           cl.BaseDatos = gesLocal.NombreBaseDatos(cl.nomTablaPadre);
	                        }
	                        string strCrearTabla = esq.StrCrearTabla().Replace("money","decimal");
	                        strCrearTabla = strCrearTabla.Replace("bit","TINYINT(1)");
	                        strCrearTabla = strCrearTabla.Replace(",,",",");
	                        gesLocal.CrearTabla(tabla,strCrearTabla + " TYPE = InnoDB" );
	                        }
	                        
	                  int numReg =gesRemoto.NumRegEnTabla(tabla);
	                  for(int i=0;i<numReg;i++){
	                    ProcesarReg(gesRemoto.LeerRegistro(tabla,i),i+1,numReg);*/
	                  }
	                           
	        }
	        
	       
	        
	        public void ProcesarReg(Registro r, int procesados, int total){
	               r.CadenaSelect = "";
	               r.AccionReg = AccionesConReg.Agregar;
	               gesLocal.GuardarDatos(r);
	              Gtk.Application.Invoke(delegate{ mostrar.MostrarInformacion(procesados, total);});
	          }
	        
	
	}
}
