// MSSqlSock.cs created with MonoDevelop
// User: valle at 0:27 10/05/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Valle.Distribuido.misSocket;
using Valle.SqlGestion;
using Valle.SqlUtilidades;
using Valle.Utilidades;

namespace Valle.Distribuido.SQLSocket
{
    
    public class ServSqlSock
    {
        IGesSql gesSql;
        LeerEsquemas leerEsquemas;
        DataTable tbTablas;
      
       
        public ServSqlSock(IGesSql gesSQl)
        {
           this.gesSql = gesSQl;
           leerEsquemas = new LeerEsquemas(gesSql);
           tbTablas = leerEsquemas.ListaDeTablas;
        }
        
            
            
			
        
        DataTable tbSelect;
              
        public void datosRecibidos(Byte[] datos, SockDeComunicacion sock)
		{
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                 string[] instr = CadenasTexto.SplitADosPuntos(Convertir.BytesAString(datos,0,datos.Length));
                  switch(instr[0]){
                     case "NumDeTablas":
                         sock.EnviarInstr(this.tbTablas.Rows.Count.ToString());
                     break;
                     
                     case "ListaTablas":
                         StringBuilder sb = new StringBuilder();
            			     foreach(DataRow r in tbTablas.Rows){
            			       sb.AppendFormat("{0}:",r[0].ToString());
            			     }
            			     sb.Remove(sb.Length-1,1);
                           sock.EnviarInstr(sb.ToString());
			   
                     break;
                      
                     case "EjConsultaSelect":
                      if((tbSelect == null) || (!tbSelect.TableName.Equals(instr[1]))){
                        if(instr.Length == 3)
                              tbSelect =  gesSql.EjecutarSqlSelect(instr[1],instr[2]);
                                else
                               tbSelect = gesSql.EjecutarSqlSelect(instr[1],instr[2],instr[3]);
                             }
                           
                              ms.Flush();ms.Position= 0;
			                        bf.Serialize(ms,tbSelect);
			                           sock.EnviarDatos(ms.ToArray());
                                           ms.Dispose();GC.Collect();
                        break;    
                     
                     case "NumRegEnTabla":
                      if((tbSelect == null) || (!tbSelect.TableName.Equals(instr[1]))){
                       if(instr.Length == 3)
                              tbSelect =  gesSql.EjecutarSqlSelect(instr[1], instr[2]);
                                else
                               tbSelect = gesSql.EjecutarSqlSelect(instr[1], instr[2], instr[3]);
                              }
                             sock.EnviarInstr(tbSelect.Rows.Count.ToString());
                       break;
        
                     case "ExtraerRegistro":
                     if((tbSelect == null) || (!tbSelect.TableName.Equals(instr[1]))){
                         if(instr.Length == 3)
                               tbSelect =  gesSql.EjecutarSqlSelect(instr[1],instr[2]);
                               else
                               tbSelect = gesSql.EjecutarSqlSelect(instr[1],instr[2],instr[3]);
                      }
                         
                          
			                          ms.Flush();ms.Position = 0;
			                             bf.Serialize(ms,UtilidadesReg.DeDataRowARegistro(tbSelect,Int32.Parse(instr[2])));
			                              sock.EnviarDatos(ms.ToArray());
                         
                      break;
                      
                     case "EjConsultaSelectEnReg":
                       if((tbSelect == null) || (!tbSelect.TableName.Equals(instr[1]))){
                        if(instr.Length == 3)
                              tbSelect =  gesSql.EjecutarSqlSelect(instr[1],instr[2]);
                                else
                               tbSelect = gesSql.EjecutarSqlSelect(instr[1],instr[2],instr[3]);
                       }    
                               
                               for(int r=0;r<tbSelect.Rows.Count;r++){
			                          ms.Flush();ms.Position = 0;
			                             bf.Serialize(ms,UtilidadesReg.DeDataRowARegistro(tbSelect,r));
			                              sock.EnviarDatos(ms.ToArray());
			                     }
			          
                           ms.Dispose();GC.Collect();       
                       break;
                       
                     case "EjConsultaNoSeletc":
                       int resultado = 0;
                           if(instr.Length == 3)
                                 resultado =  gesSql.EjConsultaNoSelect(instr[1],instr[2]);
                                  else
                                   resultado = gesSql.EjConsultaNoSelect(instr[1],instr[2],instr[3]);
                                     
                                     sock.EnviarInstr(resultado.ToString());
                       break;
                       
                       case "ExtraerSqlCreateTable":
                           if(instr.Length == 2){
                              Esquema esq = this.leerEsquemas.ExtraerEsquema(instr[1]);
                              ms.Flush();
                              bf.Serialize(ms,esq);
                              sock.EnviarDatos(ms.ToArray());
                              ms.Dispose();GC.Collect();
                              }
                              
                       break;
                  }
          }
      }
         
      public class ClientSqlSock
      {
	      
	      public delegate void OnInfProgreso(int procesados, int totales);
	      
	      ClienteSock m_ClienteSock;
	      System.Threading.AutoResetEvent PeticionRecibida = new System.Threading.AutoResetEvent(false);
	      BinaryFormatter bf = new BinaryFormatter();
	      object resultado;
	      
	      public event OnInfProgreso OnProgreso;
	       
	       public int NumDeTablas {
    			get {
    			  this.m_ClienteSock.EnviarInstr("NumDeTablas");
    			  this.PeticionRecibida.WaitOne();
    			  return Int32.Parse(this.resultado.ToString());
    			}
       		}
         
        
    		public string NombreTablas {
    			get { 
    			  this.m_ClienteSock.EnviarInstr("ListaTablas");
    			  this.PeticionRecibida.WaitOne();
    			  return resultado.ToString(); 
    			}
    		} 
    	      
	        public bool EstaConectado{
	        	 get{
	        	  return this.m_ClienteSock != null;
	        	 }
	        }
	        
            public void Conectar(int port, string dir, string protocolo){
                this.m_ClienteSock = new ClienteSock(this.OnErroresDeConexion,this.OnDatosRecibidos,true);
            }
            
            void OnDatosRecibidos(Byte[] datos, SockDeComunicacion sock){
                     if(sock.SonDatos)
                          this.resultado = datos;
                          else
                          this.resultado = Convertir.BytesAString(datos,0,datos.Length);
                          
                    this.PeticionRecibida.Set();              
            }
            
            public void OnErroresDeConexion(SockDeComunicacion sock, string error){
                  Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",error,
        		                         DateTime.Now.ToShortDateString(),"MSSqlSock.ErroresDeConexion");
       
            }
            
            public void Desconectar(){
               this.m_ClienteSock.Desconectar();
               this.m_ClienteSock=null;
            }
            
            private void RellenarColumnas(DataTable tb, Columna[] col){
               foreach(Columna s in col){
			         DataColumn cl = new DataColumn(s.nombreColumna, s.valor.GetType());
			         tb.Columns.Add(cl);
			   }
			   
            }
			
			public DataTable ExtraerTabla(string tabla){
			   DataTable tb = new DataTable(tabla);
			   int numReg = 0;
			   Registro reg;
			   
			   
			   this.m_ClienteSock.EnviarInstr("NumRegEnTabla:"+tabla+":SELECT * FROM "+tabla);
			   this.PeticionRecibida.WaitOne();
			   numReg = Int32.Parse(this.resultado.ToString());
			     
			     this.m_ClienteSock.EnviarInstr("ExtraerRegistro:"+tabla+":SELECT * FROM "+tabla+":"+0);
			     this.PeticionRecibida.WaitOne();
			     reg = (Registro)bf.Deserialize(new MemoryStream((byte[])this.resultado));
			     this.RellenarColumnas(tb,reg.Columnas.ToArray());
			     this.infProgreso(1,numReg);
			     tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
			     
			     
	     	     for(int i=1;i<numReg;i++){
			        this.m_ClienteSock.EnviarInstr("ExtraerRegistro:"+tabla+":SELECT * FROM "+tabla+":"+i);
			        this.PeticionRecibida.WaitOne();
			        reg = (Registro)bf.Deserialize(new MemoryStream((byte[])this.resultado));
			     
                    if(reg!=null){
                        tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
                    }
                    this.infProgreso(i+1,numReg);
			     }
			   
			   return tb; 
			}
			
			public Esquema ExtraerSqlCreateTable(string tabla){
			   this.m_ClienteSock.EnviarInstr("ExtraerSqlCreateTable:"+tabla);
			   this.PeticionRecibida.WaitOne();
			   return (Esquema) bf.Deserialize(new MemoryStream((byte[])this.resultado));
			}
			
			public DataTable EjConsultaSelect(string tabla, string consulta){
			     DataTable tb = new DataTable(tabla);
			   int numReg = 0;
			   Registro reg;
			   
			   
			   this.m_ClienteSock.EnviarInstr("NumRegEnTabla:"+tabla+":"+consulta);
			   this.PeticionRecibida.WaitOne();
			   numReg = Int32.Parse(this.resultado.ToString());
			     
			     this.m_ClienteSock.EnviarInstr("ExtraerRegistro:"+tabla+":"+consulta+":"+0);
			     this.PeticionRecibida.WaitOne();
			     reg = (Registro)bf.Deserialize(new MemoryStream((byte[])this.resultado));
			     this.RellenarColumnas(tb,reg.Columnas.ToArray());
			     this.infProgreso(1,numReg);
			     tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
			     
			     
	     	     for(int i=1;i<numReg;i++){
			        this.m_ClienteSock.EnviarInstr("ExtraerRegistro:"+tabla+":"+consulta+":"+i);
			        this.PeticionRecibida.WaitOne();
			        reg = (Registro)bf.Deserialize(new MemoryStream((byte[])this.resultado));
			     
                    if(reg!=null){
                        tb.Rows.Add(UtilidadesReg.DeRegistroADataRow(tb.NewRow(),reg));
                    }
                    this.infProgreso(i+1,numReg);
			     }
			   
			   return tb; 
			}
			
			public int EjConsultaNoSelect(string tabla, string consulta){
			   this.m_ClienteSock.EnviarInstr("EjConsultaNoSeletc:"+tabla+":"+consulta);
			   this.PeticionRecibida.WaitOne();
			   return Int32.Parse(this.resultado.ToString());
			}
			
			public List<Registro> ExtraerTablaEnReg(string tabla){
			  List<Registro> listReg = new List<Registro>();
		    	this.m_ClienteSock.EnviarInstr("NumRegEnTabla:"+tabla+":SELECT * FROM "+tabla);
			    this.PeticionRecibida.WaitOne();
			    int numReg = Int32.Parse(this.resultado.ToString());
			   
			    
			     for(int i=0;i<numReg;i++){
			        this.m_ClienteSock.EnviarInstr("ExtraerRegistro:"+tabla+":SELECT * FROM "+tabla+":"+i);
			        this.PeticionRecibida.WaitOne();
			        listReg.Add((Registro)bf.Deserialize(new MemoryStream((byte[])this.resultado)));
			     
			     }
			   return listReg;
			}
			
			public List<Registro> EjConsultaSelectEnReg(string tabla, string consulta){
			    List<Registro> listReg = new List<Registro>();
		    	this.m_ClienteSock.EnviarInstr("NumRegEnTabla:"+tabla+":"+consulta);
			    this.PeticionRecibida.WaitOne();
			    int numReg = Int32.Parse(this.resultado.ToString());
			   
			    
			     for(int i=0;i<numReg;i++){
			        this.m_ClienteSock.EnviarInstr("ExtraerRegistro:"+tabla+":"+consulta+":"+i);
			        this.PeticionRecibida.WaitOne();
			        listReg.Add((Registro)bf.Deserialize(new MemoryStream((byte[])this.resultado)));
			     
			     }
			   return listReg;
			}
			
			public void AgregarDelegado(OnInfProgreso del){
			    this.OnProgreso += del;
			}
			
			
			public void EliminarDelegado(OnInfProgreso del){
			 this.OnProgreso -= del;
			 this.OnProgreso = null;
			}
			
			
			public int ModificarReg(Registro r){
			   return this.EjConsultaNoSelect(r.NomTabla,UtilidadesReg.ExConsultaNoSelet(r));
			}
			
			public int BorrarDatosTabla(string tabla){
			  return this.EjConsultaNoSelect(tabla, "DELETE FROM "+tabla);
			}
		 	
			
			public int NumRegEnTabla(string tabla){
			   this.m_ClienteSock.EnviarInstr("NumRegEnTabla:"+tabla+":SELECT * FROM "+tabla);
			   this.PeticionRecibida.WaitOne();
			   return Int32.Parse(this.resultado.ToString());
			  
			}
			
			public Registro LeerRegistro(string tabla, int numReg){
			       this.m_ClienteSock.EnviarInstr("ExtraerRegistro:"+tabla+":SELECT * FROM "+tabla+":"+numReg);
			        this.PeticionRecibida.WaitOne();
			        return (Registro)bf.Deserialize(new MemoryStream((byte[])this.resultado));
			     
			}
			
			private void infProgreso(int procesados, int total){
			  if(this.OnProgreso !=null) this.OnProgreso(procesados, total);
			}
			
			
	    }
   }

