using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Lifetime;
using System.Net.Sockets;

using Valle.Distribuido.misSocket;
using Valle.Utilidades;

namespace Valle.Distribuido
{

    public enum accionesRemotas { reinicar, bloquear, desbloquear }
    
    public delegate void OnMensajeRemoto(String men);
    public delegate void OnAccionRemota(accionesRemotas accion);

    public class GesMenRemotosRemoting : MarshalByRefObject
    {
        
        event OnMensajeRemoto menCliente;
        event OnAccionRemota  accionRem;
       
        public GesMenRemotosRemoting(OnMensajeRemoto OnMen)
        {
            menCliente += OnMen;
        }

        public GesMenRemotosRemoting(OnAccionRemota OnAccion)
        {
            accionRem += OnAccion;
        }
        
        public GesMenRemotosRemoting(OnAccionRemota OnAccion, OnMensajeRemoto OnMen)
        {
             menCliente += OnMen;
               accionRem += OnAccion;
        }

        public void Accion(accionesRemotas accion){
           accionRem(accion);
        }

        public void Mensaje(string men)
        {
            menCliente(men);
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
    
    public class GesMenRemotosSocket
    {
        public const string reiniciar = "Reiniciar";
        public const string bloquear = "Bloquear";
        public const string desbloquear = "desbloquear";
        
        
        
        public event OnMensajeRemoto menCliente;
        public event OnAccionRemota  accionRem;
        
        private tipoGestor tipo;
        private ServidorSock m_servidor;
        private ClienteSock m_cliente;
        private List<ServidorDeTrabajo> listaServTrabajo = new List<ServidorDeTrabajo>(); 
       
       
        public GesMenRemotosSocket(int portServidor)
        {
            this.tipo = tipoGestor.servidor;
            m_servidor = new ServidorSock(this.OnClienteConectado,portServidor);
        }

        public GesMenRemotosSocket(string dirCom, int portCom){
           this.tipo = tipoGestor.cliente;
           m_cliente = new ClienteSock(this.OnErrorDeConexion,this.OnDatosRecibidos,true,dirCom,portCom);
        }
        
        void OnClienteConectado(Socket sock){
           ServidorDeTrabajo sT = new ServidorDeTrabajo(OnDatosRecibidos,this.OnClienteDesconectado,true,sock);
           this.listaServTrabajo.Add(sT);
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
		          case reiniciar:
		            if(this.accionRem!=null) this.accionRem(accionesRemotas.reinicar);
		          break;
		          case bloquear:
		            if(this.accionRem!=null) this.accionRem(accionesRemotas.bloquear);
		          break;
		          case desbloquear:
		            if(this.accionRem!=null) this.accionRem(accionesRemotas.desbloquear);
		          break;
		          default:
		           if(this.menCliente!=null) this.menCliente(Convertir.BytesAString(datos,0,datos.Length));
		          break;
		        }
            		       
             } 
        }
        
         
         
		 
         public void OnErrorDeConexion(SockDeComunicacion sock, string error){
                  Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",error,
        		                         DateTime.Now.ToShortDateString(),"SqlSock.ErroresDeConexion");
       
         }
           
                 
         public void Dispose(){
           if(tipo == tipoGestor.cliente){
                   foreach(ServidorDeTrabajo s in this.listaServTrabajo){
                      s.Desconectar();
                    }
         
		           this.m_cliente.Desconectar();
		       }else
		         this.m_servidor.Desconectar();
         }
               
         public void Reiniciar(){
             if(tipo == tipoGestor.cliente)
                    this.m_cliente.EnviarInstr(reiniciar);
         }
         
         public void Bloquear(){
             if(tipo == tipoGestor.cliente)
                    this.m_cliente.EnviarInstr(bloquear);
         }
         
         public void DesBloquear(){
           if(tipo == tipoGestor.cliente)
                    this.m_cliente.EnviarInstr(desbloquear);
         }
        
        }
    
}
