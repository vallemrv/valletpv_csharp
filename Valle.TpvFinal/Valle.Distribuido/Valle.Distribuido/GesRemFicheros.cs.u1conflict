// GesComunicacion.cs created with MonoDevelop
// User: valle at 23:00 23/06/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

using Valle.Distribuido.misSocket;
using Valle.Utilidades;

namespace Valle.Distribuido
{
    public enum tipoGestor {servidor, cliente}
    
    public delegate void OnListaRecibida(InfFichero[] lista);
    public delegate void OnCantidadDatosEnviados(int num, int total);
    public delegate void OnFicheroRecibido(string path);
    public delegate void OnRecibiendoFichero(int longuitud);
    
    public class InfFichero{
       public string Nom_Fichero = "";
       public string FechaModificacion = "";
       public InfFichero (string nom, string modifi){
          this.Nom_Fichero = nom;
          this.FechaModificacion = modifi;
       }
    }
    
    public class GesRemFicheros
    {
        public const string grabarFichero = "grabarFichero:";
        public const string enviarFichero = "enviarFichero:";
        public const string listaFicheros = "listaFicheros:";
        
        
        
        public event OnCantidadDatosRecibidos CanDatosRecibidos;
        public event OnCantidadDatosEnviados CanDatosEnviados;
        public event OnListaRecibida listaRecibida;
        public event OnFicheroRecibido ficheroRecibido;
        public event OnRecibiendoFichero recibiendoFichero;
        
        private tipoGestor tipo;
        private ServidorSock m_servidor;
        private ClienteSock m_cliente;
        private List<ServidorDeTrabajo> listaServTrabajo = new List<ServidorDeTrabajo>(); 
        private string[] instrEnProceso = null;
        private FileStream ficheroProceso;
        
        
        public GesRemFicheros(int portServidor)
        {
            this.tipo = tipoGestor.servidor;
            m_servidor = new ServidorSock(this.OnClienteConectado,portServidor);
        }
        
        public GesRemFicheros(string dirCom, int portCom){
           this.tipo = tipoGestor.cliente;
           m_cliente = new ClienteSock(this.OnErrorDeConexion,this.OnDatosRecibidos,true,dirCom,portCom);
           m_cliente.OnNumBytesRec += this.OnNumBytesRec;
        }
        
        void OnClienteConectado(Socket sock){
           ServidorDeTrabajo sT = new ServidorDeTrabajo(OnDatosRecibidos,this.OnClienteDesconectado,true,sock);
           sT.OnNumBytesRec += this.OnNumBytesRec;
           this.listaServTrabajo.Add(sT);
           
        }
        
        void OnNumBytesRec(int num){
          if(this.CanDatosRecibidos!= null) this.CanDatosRecibidos(num);
        }
        
        void OnClienteDesconectado (SockDeComunicacion sock)
        {
              this.listaServTrabajo.Remove((ServidorDeTrabajo)sock);
        }
        
         
        void OnDatosRecibidos(Byte[] datos, SockDeComunicacion sock)
		{
             if(!sock.SonDatos){   
		       string[] instr = CadenasTexto.SplitADosPuntos(Convertir.BytesAString(datos,0,datos.Length));
		       switch(instr[0]){
		           case listaFicheros:
		             if(tipo == tipoGestor.servidor)
		                sock.EnviarInstr(listaFicheros+this.ExListaDeFicherosLoc(instr[1]));
		                else{
		                 if(this.listaRecibida!=null) this.listaRecibida(this.CrearListaFicheros(Convertir.BytesAString(datos,0,datos.Length)));
		                 }
		           break;
		           case grabarFichero:
		               if(this.recibiendoFichero!=null)
		                                this.recibiendoFichero(Int32.Parse(instr[1]));
		                                
		               this.instrEnProceso = CadenasTexto.SplitADosPuntos(Convertir.BytesAString(datos,0,datos.Length));           
		           break;
		           case enviarFichero:
		                this.SubirFichero(sock,instr[1],instr[2]);
		           break;
		        }
		       
              }else{
               switch(instrEnProceso[0]){
		           case grabarFichero:
	                  if(this.ficheroProceso == null){
		                           FileInfo FInf = new FileInfo(this.instrEnProceso[3]);
	                               if(FInf.Exists)
	                                        FInf.Delete();
	                  
		                           this.ficheroProceso = new FileStream(this.instrEnProceso[3],  FileMode.Append, FileAccess.Write);
		                          }
		            
		              if(this.ficheroProceso.Length<Int32.Parse(this.instrEnProceso[2])){
		                    this.ficheroProceso.Write(datos,0,datos.Length);
		              }else{
		                 this.ficheroProceso.Close();
		                 this.ficheroProceso = null;
		                 if(this.ficheroRecibido!=null) this.ficheroRecibido(this.instrEnProceso[3]);
		                 this.instrEnProceso = null;
		              }
		           break;
		           
		        }
              
              }
        }
        
         
         
		 void SubirFichero(SockDeComunicacion sock, string dirOrigen, string dirDestino){
                      Stream fichero = new FileStream(dirOrigen,FileMode.Open,FileAccess.Read);
                      sock.EnviarInstr(grabarFichero+dirOrigen+":"+fichero.Length+":"+dirDestino);
                      byte[] buff = new byte[1024];
                      fichero.Position = 0;
                      int leidos = fichero.Read(buff,0,buff.Length);
                      while(leidos > 0){
                        if(this.CanDatosEnviados!=null) this.CanDatosEnviados(leidos,(int)fichero.Length);
                        sock.EnviarDatos(buff);
                        leidos = fichero.Read(buff,0,buff.Length);
                      }
          }
         
		 
         public void OnErrorDeConexion(SockDeComunicacion sock, string error){
                  Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",error,
        		                         DateTime.Now.ToShortDateString(),"MSSqlSock.ErroresDeConexion");
       
         }
           
         public void ListaDeFicherosRem(string dir){
              if(tipo == tipoGestor.cliente)
                     this.m_cliente.EnviarInstr(listaFicheros+dir);
         }
         
         
         
         public void SubirFichero(string dirOrigen, string dirDestino){
              if(tipo == tipoGestor.cliente){
                this.SubirFichero(this.m_cliente, dirOrigen, dirDestino);
                }
          }
          
         public void DescargarFichero(string dirOrigen, string dirDestino){
           if(tipo == tipoGestor.cliente)
                         this.m_cliente.EnviarInstr(enviarFichero+dirOrigen+":"+dirDestino);
           
           
         }
         
         public void Dispose(){
           if(tipo != tipoGestor.cliente){
                   foreach(ServidorDeTrabajo s in this.listaServTrabajo){
                      s.Desconectar();
                    }
         
		           this.m_servidor.Desconectar();
		       }else
		         this.m_cliente.Desconectar();
         }
         
        public string ExListaDeFicherosLoc(string path){
           DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                   if(dir.Exists){
		                StringBuilder sb = new StringBuilder();
		                foreach (FileInfo f in dir.GetFiles()){
		                     sb.Append(":"+f.Name+"@"+f.LastWriteTime);
		                }
		                return sb.ToString();
		            }
		        return "";    
		 }
		  
               
        public InfFichero[] CrearListaFicheros(string lista){
              string[] infCom = CadenasTexto.SplitADosPuntos(lista.Replace(listaFicheros,""));
		      List<InfFichero> lf= new List<InfFichero>();
		      foreach(string s in infCom){
		               string[] infSim = s.Split('@');
		                     lf.Add(new InfFichero(infSim[0],infSim[1]));
		                  }
		          return lf.ToArray();          
        }
        
    }
}
