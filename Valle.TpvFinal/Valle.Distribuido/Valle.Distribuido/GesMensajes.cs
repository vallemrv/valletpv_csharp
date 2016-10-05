// GesMensajes.cs created with MonoDevelop
// User: valle at 23:05 07/09/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net.Sockets;
using System.Collections.Generic;

using Valle.Distribuido.misSocket;
using Valle.Utilidades;

namespace Valle.Distribuido
{
    
    public delegate void OnMensajeRecibido(string men);
    public delegate void OnDatosRecibidos(byte[] datos);
    public delegate void OnMensajeRecibidoServ(string men, SockDeComunicacion sock);
    public delegate void OnDatosRecibidosServ(byte[] datos, SockDeComunicacion sock);
   
   
    public class GesMensajes 
    {
        private tipoGestor tipo;
        private ServidorSock m_servidor;
        private ClienteSock m_cliente;
        private List<ServidorDeTrabajo>  s_trabajo
               = new List<ServidorDeTrabajo>();
        
       
        public event OnMensajeRecibido mensajeRecibido;
        public event OnDatosRecibidos datosRecibidos;
        public event OnMensajeRecibidoServ mensajeRecibidoServ;
        public event OnDatosRecibidosServ datosRecibidosServ;
        
       
        public GesMensajes(int portServidor)
        {
            this.tipo = tipoGestor.servidor;
            m_servidor = new ServidorSock(this.OnClienteConectado,portServidor);
        }
        
        public GesMensajes(string dirCom, int portCom){
        
           this.tipo = tipoGestor.cliente;
           m_cliente = new ClienteSock(this.OnErrorDeConexion,this.OnDatosRecibidos,true,dirCom,portCom);
        }
        
       
        
        void OnClienteConectado(Socket sock){
            s_trabajo.Add(new ServidorDeTrabajo(OnDatosRecibidos,this.OnClienteDesconectado,true,sock));
        }
        
        void OnClienteDesconectado (SockDeComunicacion sock)
        {
             s_trabajo.Remove((ServidorDeTrabajo)sock);
        }
        
         
        void OnDatosRecibidos(Byte[] datos, SockDeComunicacion sock)
		{
             if(!sock.SonDatos){   
		       if(this.mensajeRecibido!=null) this.mensajeRecibido(Convertir.BytesAString(datos,0,datos.Length));  		           
		       else if(this.mensajeRecibidoServ!=null) this.mensajeRecibidoServ(Convertir.BytesAString(datos,0,datos.Length),sock);  		           
		     }else{
		      if(this.datosRecibidos!=null) this.datosRecibidos(datos);  		           
		      else if(this.datosRecibidosServ!=null) this.datosRecibidosServ(datos,sock);  		           
		     }
		     
		}
		
		void OnErrorDeConexion(SockDeComunicacion sock, string error){
                  Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",error,
        		                         DateTime.Now.ToShortDateString(),"GesMensajes.ErroresDeConexion");
       
        }
        
        public void EnviarMensajeMasivo(string mensaje){
               foreach(SockDeComunicacion sock in s_trabajo){
                   sock.EnviarInstr(mensaje);
                   }
        }
       
        public void EnviarDatosMasivo(byte[] datos){
                    foreach(SockDeComunicacion sock in s_trabajo){
                          sock.EnviarDatos(datos);
                          }
        }
		
       
       public void EnviarMensaje(string mensaje, SockDeComunicacion sock){
                       sock.EnviarInstr(mensaje);
        }
       
        public void EnviarDatos(byte[] datos, SockDeComunicacion sock){
                      sock.EnviarDatos(datos);
        }
		
		
		public void EnviarMensaje(string mensaje){
                       this.m_cliente.EnviarInstr(mensaje);
        }
       
        public void EnviarDatos(byte[] datos){
                      this.m_cliente.EnviarDatos(datos);
        }
		
	    public void Dispose(){
           if(tipo != tipoGestor.cliente){
                   if(this.s_trabajo!=null){
                     foreach(ServidorDeTrabajo s in s_trabajo){
                         s.Desconectar();
                         }   
                     }             
		           this.m_servidor.Desconectar();
		       }else
		         this.m_cliente.Desconectar();
         }
    }
}
