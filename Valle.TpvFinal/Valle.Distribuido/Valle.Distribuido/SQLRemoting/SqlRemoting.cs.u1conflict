/*
 * Creado por SharpDevelop.
 * Usuario: vallevm
 * Fecha: 14/09/2008
 * Hora: 19:40
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */

using System;
using System.Data;
using System.Data.SqlClient;

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;

using Valle.SqlGestion;
using Valle.SqlUtilidades;

namespace Valle.Distribuido.SQLRemoting
{
    
	/// <summary>
	/// Description of GesRegBarRemoto.
	/// </summary>
   
   	public  class SQLServ: MarshalByRefObject
	{
           
       IGesSql gestorDatos;
       LeerEsquemas leerEsquemas;
       
       public  DataTable tbTablas
       {
          get{return leerEsquemas.ListaDeTablas;}
       }
      
      public IGesSql GestorDatos{
            set{
               gestorDatos = value;
               leerEsquemas = new LeerEsquemas(gestorDatos);
            }
       }
         
        
		public string NombreTablas {
			get { 
			  try{
			  if(gestorDatos!=null){
			    StringBuilder sb = new StringBuilder();
			     foreach(DataRow r in tbTablas.Rows){
			       sb.AppendFormat("{0}:",r[0].ToString());
			     }
			     sb.Remove(sb.Length-1,1);
			      return sb.ToString();
			   }else
			       return "";
			  }catch (Exception e){
       			gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.NombreTablas");
       
        	}
			 return "";
			}
			
		}
		
	 
		
		public int NumDeTablas {
		   
			get {
                    try{			
        			 if(tbTablas != null)
        			  return tbTablas.Rows.Count;
        			  else
        			  return 0;
        			  
        		}catch (Exception e){
       			gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.NumDeTablas");
       
            	}
            	
        	 return -1;
			}
		  
		}

        DataTable tbDeCarga;
		
		public int CargarTabla(String nombreTb){
		try{
		       tbDeCarga = this.gestorDatos.EjecutarSqlSelect(nombreTb,"SELECT * FROM "+nombreTb);
               return tbDeCarga.Rows.Count;
            
        		
           }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.CargarTabla");
       
        	}
        	
           return -1;	
		}
		
       
        public int NumRegEnTabla(string tabla){
          try{
              return this.CargarTabla(tabla);
           }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.NumRegTabla");
       
        	}
        	
        	return -1;
        }
        
        
		public Registro ExtraerRegistro(string tabla, int pos){
          try{
            this.CargarTabla(tabla);
            return ExtraerRegistro(pos);
            
         }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.ExtraerReg2");
       
        	}
        	
        	return null;
        }
        
        public Registro ExtraerRegistro(int pos){
           try{
              if(tbDeCarga != null)
    		           return UtilidadesReg.DeDataRowARegistro(tbDeCarga, pos);      
    		      else                
                     return null;
                 
              }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.ExtraerReg");
       
        	}
        	
        	return null;
        }
        
        public int ModificarTabla(Registro r)
        {
           try{
              if(gestorDatos !=null){
        			if(this.tbDeCarga.TableName.Equals(r.NomTabla)) this.tbDeCarga = null;
		           return gestorDatos.ModificarReg(r);
                 }else
                  return 0;
                  
               }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.ModificarReg");
       
        	   }
        	   return -1;
                 
        }


        public int ejecutarConsultaSelect(String consulta, String nombre)
        {
           try{
                    tbDeCarga = this.gestorDatos.EjecutarSqlSelect(nombre, consulta);
        	         return tbDeCarga.Rows.Count;
                  
             }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.EjConsultaSelect");
       
        	 }
        	 
        	 return -1;
        }
        
        
        public int ejecutarConsultaNoSelect(string tabla, String consulta)
        {
          try{
           if(gestorDatos !=null){
        	   return this.gestorDatos.EjConsultaNoSelect(tabla, consulta);
            }else
             return 0;
             
             }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message+" Consulta: "+consulta,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.EjecutarNoSelect");
       
        	}
        	return -1;
        }

       
        public Esquema ExtraerEsquema(string tabla){
          try{          
           return leerEsquemas.ExtraerEsquema(tabla);
            
            }catch (Exception e){
        		gestorDatos.Desbloquear();
        		Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",e.Message,
        		                         DateTime.Now.ToShortDateString(),"SQLRemoting.ExtraerEsquema");
       
        	}   
             return null;
        }

        
        public override Object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();

            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.Zero;
            }
            return lease;
        }


    	}
	
	    public class SQLClient : IGesSql {
	      
	      public delegate void OnInfProgreso(int procesados, int totales);
		  public delegate void OnRegRecuperado(int procesados, int totales, Registro reg);
		  public delegate void OnDrRecuperado(int procesados, int totales, DataRow dr, string[] columnas);
	      
	      SQLServ servSql;
	      TcpChannel chan = new TcpChannel();
		  OnRegRecuperado regRecuperado;
		  OnDrRecuperado drRecuperado;
		  Thread hRecuperarDr;
		  Thread hRecuperarReg;
		  DataSet tablas = new DataSet();
		  
		
	      public event OnInfProgreso OnProgreso;
	         
	        public int NumDeTablas {
    			get { 
    			 return this.servSql.NumDeTablas;
    			}
       		}
    		public string[] NombreTablas {
    			get { 
    			  return this.servSql.NombreTablas.Split(':'); 
    			}
    		} 
            public bool EstaConectado{
	        	 get{
				  try{
	        	     return this.servSql.NumDeTablas >= 0;
				    }catch{
				      this.Desconectar();
					  return false;
				    }
	        	 }
	        }
	        
            public void Conectar(int port, string dir, string protocolo){
              
              bool estaRegistrado = false;
              foreach(IChannel c in ChannelServices.RegisteredChannels){
                   estaRegistrado = c.Equals(chan);
              }
               if(!estaRegistrado){
                   ChannelServices.RegisterChannel(chan,false);
                   }
                   
                   servSql =  (SQLServ)Activator.GetObject(typeof(SQLServ),
                        protocolo+"://"+dir+":"+port+"/SQLServ");
                        
	        }
            public void Desconectar(){
              bool estaRegistrado = false;
              foreach(IChannel c in ChannelServices.RegisteredChannels){
                   estaRegistrado = c.Equals(chan);
              }
               if(estaRegistrado){
                 ChannelServices.UnregisterChannel(chan);
                 this.servSql = null; GC.Collect();
               }
            }
      
            void RellenarColumnas(DataTable tb, Columna[] col){
               foreach(Columna s in col){
			         DataColumn cl = new DataColumn(s.nombreColumna, s.valor.GetType());
			         tb.Columns.Add(cl);
			   }
			   
            }
			public DataTable ExtraerTabla(string tabla){
			   DataTable tb = new DataTable(tabla);
			   int numReg = this.servSql.CargarTabla(tabla);
			   if(numReg>0){
			     Registro reg = this.servSql.ExtraerRegistro(0);
			     this.RellenarColumnas(tb,reg.Columnas.ToArray());
			     this.infProgreso(1,numReg);
			     tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
			     
	     	     for(int i=1;i<numReg;i++){
			       reg = this.servSql.ExtraerRegistro(i);
                  if(reg!=null){
                        tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
                    }
                    this.infProgreso(i+1,numReg);
			     }
			    }
			   return tb;
			   
			}
			public Esquema ExtraerSqlCreateTable(string tabla){
			   return this.servSql.ExtraerEsquema(tabla);
			}
			public DataTable EjConsultaSelect(string tabla, string consulta){
			    DataTable tb = new DataTable(tabla);
			    int numReg = this.servSql.ejecutarConsultaSelect(consulta, tabla);
			    if(numReg>0){
			     Registro reg = this.servSql.ExtraerRegistro(0);
			     this.RellenarColumnas(tb,reg.Columnas.ToArray());
			     this.infProgreso(1,numReg);
			     tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
			     
			    
			     for(int i=1;i<numReg;i++){
			      reg = this.servSql.ExtraerRegistro(i);
                  if(reg!=null){
                          tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
                    }
                    this.infProgreso(i+1,numReg);
			     }
			   }
			     return tb;
			}
			public int EjConsultaNoSelect(string tabla, string consulta){
			   return this.servSql.ejecutarConsultaNoSelect(tabla,consulta);
			}
	  		public int EjConsultaNoSelect(string consulta){
			   return this.servSql.ejecutarConsultaNoSelect("", consulta);
			}
			public List<Registro> ExtraerTablaEnReg(string tabla){
			  List<Registro> listReg = new List<Registro>();
		    	int numReg = this.servSql.CargarTabla(tabla);
			    
			     for(int i=0;i<numReg;i++){
			        listReg.Add(this.servSql.ExtraerRegistro(i));
                    this.infProgreso(i+1,numReg);
			     }
			   return listReg;
			}
			public List<Registro> EjConsultaSelectEnReg(string tabla, string consulta){
			    List<Registro> listReg = new List<Registro>();
		    	int numReg = this.servSql.ejecutarConsultaSelect(consulta, tabla);
			    
			     for(int i=0;i<numReg;i++){
			        listReg.Add(this.servSql.ExtraerRegistro(i));
                    this.infProgreso(i+1,numReg);
			     }
			   return listReg;
			}
			public void PararConsultasAsync(){
			   if((this.hRecuperarDr!=null)&&(this.hRecuperarDr.IsAlive))this.hRecuperarDr.Abort();
			   if((this.hRecuperarReg!=null)&&(this.hRecuperarReg.IsAlive))this.hRecuperarReg.Abort();
			   
			}
		    public void ConsultaRegAsync(string consulta, OnRegRecuperado RegRecuperado){
			    this.regRecuperado = RegRecuperado;
			     if((this.hRecuperarReg!=null)&&(this.hRecuperarReg.IsAlive))
				                         this.hRecuperarReg.Abort();
			     this.hRecuperarReg = new Thread(new ParameterizedThreadStart(this.ExtraerRegistros));
			     this.hRecuperarReg.Start(consulta);
			}
		    private void ExtraerRegistros(object ObConsulta){
			   string consulta = ObConsulta.ToString();
			   int numReg = this.servSql.ejecutarConsultaSelect(consulta, "");
			    if(numReg>0){
			     for(int i=0;i<numReg;i++){
			        this.regRecuperado(i+1,numReg,this.servSql.ExtraerRegistro(i));
			     }
			     }else
			         this.regRecuperado(0,0,null);
			     
		    }
		    public void ConsultaDrAsync(string consulta, OnDrRecuperado DrRecuperado){
			    this.drRecuperado = DrRecuperado;
			     if((this.hRecuperarDr!=null)&&(this.hRecuperarDr.IsAlive))
				                         this.hRecuperarDr.Abort();
			     this.hRecuperarDr = new Thread(new ParameterizedThreadStart(this.ExtraerDr));
			     this.hRecuperarDr.Start(consulta);
			}
		    private void ExtraerDr(object ObConsulta){
		       DataTable tb = new DataTable();
		       string consulta = ObConsulta.ToString();
			   int numReg = this.servSql.ejecutarConsultaSelect(consulta, "");
			   if(numReg>0){
    			   Registro reg = this.servSql.ExtraerRegistro(0);
    			   
    			   string[] columnas = new string[reg.Columnas.Count];
    			     for(int c=0;c<columnas.Length;c++){
    			        columnas[c] = reg.Columnas[c].nombreColumna;
    			     }
    			     
    			   this.RellenarColumnas(tb,reg.Columnas.ToArray());
    			   DataRow r = tb.NewRow();
    			   Valle.SqlUtilidades.UtilidadesReg.DeRegistroADataRow(r,reg);
    			   
    			   this.drRecuperado(1,numReg,r,columnas);
    			   
    			     for(int i=1;i<numReg;i++){
    			        reg = this.servSql.ExtraerRegistro(i);
    			        r = tb.NewRow();
    			        Valle.SqlUtilidades.UtilidadesReg.DeRegistroADataRow(r,reg);
    			        this.drRecuperado(i+1,numReg,r,columnas);
    			     }
    			    }else
    			       this.drRecuperado(0,0,null,null);
			     
		    }
			public void AgregarDelegado(OnInfProgreso del){
			    this.OnProgreso += del;
			}
			public void EliminarDelegado(OnInfProgreso del){
			 this.OnProgreso -= del;
			 this.OnProgreso = null;
			}
			public int ModificarReg(Registro r){
			   string consultaMod = UtilidadesReg.ExConsultaNoSelet(r);
			   return this.servSql.ejecutarConsultaNoSelect(r.NomTabla,consultaMod);
			}
			public int BorrarDatosTabla(string tabla){
			 return this.servSql.ejecutarConsultaNoSelect(tabla,"DELETE FROM "+tabla);
			}
			public int NumRegEnTabla(string tabla){
			    return this.servSql.NumRegEnTabla(tabla);
			}
			public Registro LeerRegistro(string tabla, int numReg){
			       return this.servSql.ExtraerRegistro(tabla,numReg);
			}
		    private void infProgreso(int procesados, int total){
			  if(this.OnProgreso !=null) this.OnProgreso(procesados, total);
			}
		
		    #region IGesSql implementation
		    public void CargarTabla (string tabla, string columKey)
		    {
		       if(!tablas.Tables.Contains(tabla)){
		            tablas.Tables.Add(this.ExtraerTabla(tabla));
		           }
		         DataTable tb = tablas.Tables[tabla]; 
		         if((columKey!=null)&&(columKey.Length>0)){
		            tb.Columns[columKey].AutoIncrement = true;
                    tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                    (int)tb.Rows[tb.Rows.Count - 1][columKey] + 1 : 1;
		         }
		       
		    }
		  
		    
		    public DataTable ExtraerTabla (string tabla, string columKey)
		    {
		       if(!tablas.Tables.Contains(tabla)){
		            tablas.Tables.Add(this.ExtraerTabla(tabla));
		           }
		         DataTable tb = tablas.Tables[tabla]; 
		         if((columKey!=null)&&(columKey.Length>0)){
		            tb.Columns[columKey].AutoIncrement = true;
                    tb.Columns[columKey].AutoIncrementSeed = tb.Rows.Count > 0 ?
                    (int)tb.Rows[tb.Rows.Count - 1][columKey] + 1 : 1;
		         }
		         return tb;
		         
		    }
		    
		    public DataTable ExtraerTabla (string tabla, string columKey, string dirBaseDatos)
		    {
		    	throw new System.NotImplementedException();
		
		    }
		    
		    public bool ActualizarTabla (string tabla)
		    {
		    	throw new System.NotImplementedException();
		    }
		    public DataTable EjecutarSqlSelect (string nombre, string consulta, string dirBaseDatos)
		    {
		    	throw new System.NotImplementedException();
		
		    }
		    public DataTable EjecutarSqlSelect (string nombre, string consulta)
		    {
		    	return  this.EjConsultaSelect(nombre,consulta);
		    }
		    
		    public object EjEscalar (string consulta, string dirBaseDatos)
		    {
		    	throw new System.NotImplementedException();
		
		    }
		    public object EjEscalar (string consulta)
		    {
		    	throw new System.NotImplementedException();
		
		    }
		    public int EjConsultaNoSelect (string tabla, string consulta, string dirBaseDatos)
		    {
		    	throw new System.NotImplementedException();
		
		    }
		    public int ModificarReg (Registro reg, string baseDatos)
		    {
		    	throw new System.NotImplementedException();
		
		    }
		    public int NumRegEnTabla (string tabla, string baseDatos)
		    {
		    	throw new System.NotImplementedException();
		
		    }
		    public void AgregarTabla (DataTable tb)
		    {
		    	if(!tablas.Tables.Contains(tb.TableName))
		    	    tablas.Tables.Add(tb);
		
		    }
		    public void Desbloquear ()
		    {
		    	throw new System.NotImplementedException();
		    }   
		    
		      #endregion
   
			
	    }
}
