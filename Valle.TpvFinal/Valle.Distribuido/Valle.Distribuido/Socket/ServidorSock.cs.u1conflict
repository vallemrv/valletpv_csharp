using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using Valle.Utilidades;

namespace Valle.Distribuido.misSocket
{

    public class SocketBase
    {
    
    	protected IPAddress direc = IPAddress.Any;
        protected int port = 8888;
        public SocketBase(IPAddress ip, int port){
        	this.direc = ip;
        	this.port = port;
        }
        public SocketBase() { }
        public SocketBase(string direc, int port)
        {
            this.direc = Dns.GetHostAddresses(direc)[0];
            this.port = port;
        }
    }

    
    public class ServidorSock : SocketBase
    {
        public delegate void OnClienteConectado(Socket sock);
    	
        Socket m_socDeEscucha;
        event OnClienteConectado clienCon;
     
        public ServidorSock(OnClienteConectado onCon ) 
        {
        	this.clienCon += new OnClienteConectado(onCon);
        	this.EscucharPuerto();
        }
        
        public ServidorSock(OnClienteConectado onCon, int port ) :
        	base(IPAddress.Any,port)
        {
        	this.clienCon = new OnClienteConectado(onCon);
        	this.EscucharPuerto();
        }
        
        public ServidorSock(OnClienteConectado onCon, string dirNet, int port) : base(dirNet,port)
        {
        	this.clienCon = new OnClienteConectado(onCon);
        	this.EscucharPuerto(); 
        }
        
        private void EscucharPuerto()
        {
            try
            {
                //creamos el socket de escucha
                m_socDeEscucha = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(direc, this.port);
                //Cojemos la ip y el puerto ...
                m_socDeEscucha.Bind(ipLocal);
                //arrancamos la escucha...
                m_socDeEscucha.Listen(5);
                m_socDeEscucha.BeginAccept(new AsyncCallback(OnClientConnect), null);

            }
            catch (SocketException se)
            {
                Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",se.Message,
        		                         DateTime.Now.ToShortDateString(),"ServidorSock.EschucahrPuerto");
            }
        }
        
        
        protected void OnClientConnect(IAsyncResult asyn)
        {

            try
            {
                clienCon(m_socDeEscucha.EndAccept(asyn));
                m_socDeEscucha.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }

            catch (ObjectDisposedException se)
            {

                Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",se.Message,
        		                         DateTime.Now.ToShortDateString(),"ServidorSock.OnClientConnect");

            }

            catch (SocketException se)
            {
                
                Valle.Utilidades.RutasArchivos.EscribirEnFicheroErr("SegErr.log",se.Message,
        		                         DateTime.Now.ToShortDateString(),"ServidorSock.OnClientConnect");

            }
        }

        public void Desconectar()
        {
            if (m_socDeEscucha != null)
            {
                m_socDeEscucha.Close();
            }
         }
        
       
    }

   
    public delegate void OnDatosRecibidos(Byte[] datos, SockDeComunicacion sock);
    public delegate void OnCantidadDatosRecibidos(int c);
    public delegate void OnDesconectado(SockDeComunicacion sock);
    public delegate void OnErrorDeConexion(SockDeComunicacion sock, string error);
       

    public class SockDeComunicacion : SocketBase
    {
    	private int longBuff = 2048;
    	private string nombreCliente = "Remoto";
    	private string nombreLocal = "Local";
    	
		public string NombreLocal {
			get { return nombreLocal; }
			set { nombreLocal = value; }
		}
    	
    	private bool sonDatos = false;
    	private object gestorDeSocket;
    	private bool gestionado = false;
    	
        public event OnCantidadDatosRecibidos OnNumBytesRec = null;
        public event OnErrorDeConexion OnError = null;
        public event OnDesconectado OnSockDesconectado = null;
     	public event OnDatosRecibidos OnHayDatos = null ;
     	
     	
       	protected AsyncCallback asynRecibido;
       	protected AsyncCallback asynEnviado;
       	protected bool desconectado = false;
       	protected bool desconexionForzada = true;
    	protected ColaCompartida<Byte[]> buffEntrada = new ColaCompartida<byte[]>();
    	protected MiProtocolo protocolo = null;
    	protected Thread hProtocolo;
    
       	
		public bool Desconectado {
			get { return desconectado; }
		}

		public ColaCompartida<byte[]> BuffEntrada {
		    get {
		        return buffEntrada;
		    }
		}

		public int LongBuff {
		    get {
		        return longBuff;
		    }
		    set {
		        longBuff = value;
		    }
		}

		public string NombreCliente {
		    get {
		        return nombreCliente;
		    }
		    set {
		        nombreCliente = value;
		    }
		}

		public bool SonDatos {
		    get {
		        return sonDatos;
		    }
		    set {
		        sonDatos = value;
		    }
		}

		public object GestorDeSocket {
		    get {
		        return gestorDeSocket;
		    }
		    set {
		        gestorDeSocket = value;
		    }
		}

		public bool Gestionado {
		    get {
		        return gestionado;
		    }
		    set{
		        gestionado = value;
		         if(gestionado){
		            if(protocolo == null)
		                protocolo =  new MiProtocolo(this.BuffEntrada,this);
		            hProtocolo = new Thread(new ThreadStart(protocolo.GestionarDatos));
		            hProtocolo.Start();
		                
		         }
		        }
		}
       
       	Socket m_sockDeComunicacion;
       	
       
        public SockDeComunicacion ()
        {
            
        }

        public SockDeComunicacion(string dirNet, int port) 
        	: base(dirNet,port) {
            
        }
        
        protected void WaitForData(System.Net.Sockets.Socket soc)
        {

            try
            {
                if (soc != null)
                {
                    if (m_sockDeComunicacion == null) { m_sockDeComunicacion = soc; }
                    if (asynRecibido == null)
                    {
                        asynRecibido = new AsyncCallback(OnDataReceived);
                    }

                    CSocketPacket ElSocket = new CSocketPacket(longBuff);
                    ElSocket.EsteSocket = soc;
                    soc.BeginReceive(ElSocket.BuferDeDatos, 0, ElSocket.BuferDeDatos.Length, SocketFlags.None, asynRecibido, ElSocket);
                }
            }
            catch (SocketException se)
            {
               	_ErrorDeConexion(se.Message);
               	FDesconectar();
            }



        }

        protected void OnDataReceived(IAsyncResult asyn)
        {

            try
            {
            	if(!desconectado){
            	  CSocketPacket ElSocket = (CSocketPacket)asyn.AsyncState;
                  int iRx = 0;
                  iRx = ElSocket.EsteSocket.EndReceive(asyn);
                  if(iRx>0){
                    _CantidadDeDatosRec(iRx);
                    buffEntrada.Encolar(Bytes.CopiarBytes(ElSocket.BuferDeDatos,0,iRx));
                    WaitForData(m_sockDeComunicacion);
                  }else{
                    _ErrorDeConexion("El terminal remoto ha cerrado la comunicacion");
                    FDesconectar();
                  }
            	}
            	
            }

            catch (ObjectDisposedException d)
            {
              	_ErrorDeConexion(d.Message);
              	FDesconectar();
            }

            catch (SocketException se)
            {
                _ErrorDeConexion(se.Message);
               	FDesconectar();
            }

        }
        
        protected void OnDatosEnviados(IAsyncResult asyn){
        	if(!desconectado)
        	((Socket)asyn.AsyncState).EndSend(asyn);
        }
        
        protected void enviarByte(Byte[] dato)
        {
        	try{
        		if(!desconectado){
        			if(asynEnviado==null) asynEnviado+=new AsyncCallback(OnDatosEnviados);
                 m_sockDeComunicacion.BeginSend(dato,0,dato.Length,SocketFlags.None,asynEnviado,m_sockDeComunicacion);
        		}
        	}
        	catch (ObjectDisposedException d)
            {
              	_ErrorDeConexion(d.Message);
              	FDesconectar();
            }

            catch (SocketException se)
            {
                _ErrorDeConexion(se.Message);
               	FDesconectar();
            }
        }
        
        private void FDesconectar(){
        	   
        	   desconectado = true;
        	
        	   if(desconexionForzada)
        	                    _Desconectado();
        	   
	           if(m_sockDeComunicacion!=null){
	            	m_sockDeComunicacion.Shutdown(SocketShutdown.Both);
	        		m_sockDeComunicacion.Close();
	        		m_sockDeComunicacion = null ;
	        		buffEntrada.Baciar();
	                buffEntrada.Encolar(Convertir.StringHexABytes(ConstantesSock.DESCONECTAR));
	                hProtocolo.Abort();
	                GC.Collect();
	           } 
        }
        
      
        
        public virtual void Desconectar(){
        	desconexionForzada = false;
        	FDesconectar();
        }
        
        public void _ErrorDeConexion(string err){
         	if(OnError!=null){OnError(this,err);}
        }
        
        public void _DatosRecibidos(byte[] datos){
           if(this.OnHayDatos!=null){ this.OnHayDatos(datos,this);}
        }
        
        public void _CantidadDeDatosRec(int i){
          if(this.OnNumBytesRec!=null){this.OnNumBytesRec(i);}
        }
        
        private void _Desconectado(){
          if(this.OnSockDesconectado!=null){this.OnSockDesconectado(this);}
        }
        
        public void EnviarDatos(byte[] datos){
           if(this.gestionado)
        	 this.enviarByte(new PackDatos(datos).ToByte());
        	else
        	 this.enviarByte(datos);
        	
	    }
        
        
        public void EnviarInstr(string inst){
           	if(this.gestionado)
        	 this.enviarByte(new PackComando(inst).ToByte());
        	else
        	 this.enviarByte(Convertir.StringAbytes(inst));
        	
	    }   
    }
        
        
    public class ServidorDeTrabajo : SockDeComunicacion
    {
        
        public ServidorDeTrabajo(OnDatosRecibidos datRec,OnDesconectado OnDes, bool esGestionado, Socket sock)
        {
        	desconectado = false;
        	OnSockDesconectado+= new OnDesconectado(OnDes);
        	OnHayDatos +=new OnDatosRecibidos(datRec);
        	Gestionado = esGestionado;
        	WaitForData(sock);
        }
       
   }
   
}
