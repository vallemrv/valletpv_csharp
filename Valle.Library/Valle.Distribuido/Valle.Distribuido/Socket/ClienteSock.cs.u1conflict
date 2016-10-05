using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using Valle.Utilidades;

namespace Valle.Distribuido.misSocket
{

    public class ClienteSock : SockDeComunicacion
    {
        Socket m_SocCliente;
        
        public ClienteSock(OnErrorDeConexion onErr,OnDatosRecibidos onDatRec,bool esGesitonado, string dirNet): base(dirNet, 8888) 
        {   
        	this.OnError += new OnErrorDeConexion(onErr);
        	this.OnHayDatos+=new OnDatosRecibidos(onDatRec);
        	Gestionado = esGesitonado;
        	this.conectar(); 
        }
        
        public ClienteSock(OnErrorDeConexion onErr,OnDatosRecibidos onDatRec,bool esGesitonado)
        {
        	this.OnError += new OnErrorDeConexion(onErr);
        	this.OnHayDatos+=new OnDatosRecibidos(onDatRec);
        	Gestionado = esGesitonado;
        	
        	this.conectar();
        }
        
        public ClienteSock(OnErrorDeConexion onErr,OnDatosRecibidos onDatRec,bool esGesitonado, string dirNet, int port)
            : base(dirNet, port) 
        { 
        	this.OnError += new OnErrorDeConexion(onErr);
        	this.OnHayDatos+=new OnDatosRecibidos(onDatRec);
        	Gestionado = esGesitonado;
        	this.conectar(); 
        }
        
        private void conectar()
        {
        	try{
            m_SocCliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(direc, this.port);
            m_SocCliente.Connect(remoteEndPoint); desconectado = false;
            this.WaitForData(m_SocCliente);
        	}catch(SocketException ex){
        		this._ErrorDeConexion(ex.Message);
        		this.Desconectar();
        	}
        }
        
       
       
    }
}
