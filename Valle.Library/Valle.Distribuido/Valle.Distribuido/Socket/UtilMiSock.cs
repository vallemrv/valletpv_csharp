// UtilMiSock.cs created with MonoDevelop
// User: valle at 0:01 24/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using Valle.Utilidades;
using System.Net.Sockets;
using System.Collections.Generic;


namespace Valle.Distribuido.misSocket
{
   
    
    public class CSocketPacket
    {

        private System.Net.Sockets.Socket esteSocket;
        private byte[] buferDeDatos;
        
        public Socket EsteSocket {
            get {
                return esteSocket;
            }
            set {
                esteSocket = value;
            }
        }

        public byte[] BuferDeDatos {
            get {
                return buferDeDatos;
            }
            set {
                buferDeDatos = value;
            }
        }
        
        public CSocketPacket(int logBuff)
        {

            buferDeDatos = new byte[logBuff];
        }

    }
    
    public class ConstantesSock
    {
        public const string CLIEN_CN = "FD000000";
        public const string BYTE_RECIBIDO = "FD000001";
        public const string INI_TRD = "FD000002";
        public const string INI_TRI = "FD000003";
        public const string DESCONECTAR = "FD000005";
        public const string REINICIAR = "FD000004";
        public const string REINICIADO = "FD000006";
    }
    
   
    public class PackComando
    {
      	public PackComando(string instruccion)
     	{
            this.longitud = instruccion.Length;
            this.instStr = instruccion;
           
        }

   
       
        public byte[] ToByte()
	    {
	        List<byte> result = new List<byte>();
	        result.AddRange(Convertir.StringHexABytes(ConstantesSock.INI_TRI));
	        result.AddRange(BitConverter.GetBytes(longitud));
	                        result.AddRange(Convertir.StringAbytes(instStr));
	        return result.ToArray();
	    }
        
        
      public int longitud;
      public string instStr;
      
    }
   
    public class PackDatos
    {
      	public PackDatos(byte[] datos)
     	{
            this.longitud = datos.Length;
            this.msgDato = datos;
        }

   
        
        public byte[] ToByte()
	    {
        	
	        List<byte> result = new List<byte>();
	        result.AddRange(Convertir.StringHexABytes(ConstantesSock.INI_TRD));
	        result.AddRange(BitConverter.GetBytes(longitud));
	                        result.AddRange(msgDato);
	        return result.ToArray();
	    }
      public int longitud;
      public byte[] msgDato;
    }
    
    public class MiProtocolo {
    
       public bool desconectar = false;
       
        
       ColaCompartida<byte[]> buffEntrada = new ColaCompartida<byte[]>(); 
       SockDeComunicacion sockComunicacion;
       
    	 
        public MiProtocolo (ColaCompartida<byte[]> colaComun, SockDeComunicacion sock)
        {
            this.buffEntrada = colaComun;
            this.sockComunicacion = sock;
        }

        
        private void EjecutarIntr(byte[] d)
        {
            string codigo = Convertir.DeBytesAStringHEX(d);
            switch (codigo)
            {
                case ConstantesSock.INI_TRD:
                    this.sockComunicacion.SonDatos = true;
                    break;
                case ConstantesSock.INI_TRI:
                    this.sockComunicacion.SonDatos = false;
                    break;
               
                case ConstantesSock.DESCONECTAR:
                    this.desconectar = true;
                    break;
                 
            }
          
        }
       
        private void CargarBytes(List<byte>buff, int can){
        	while((buff.Count<can)&&(!desconectar)){
        		buff.AddRange(buffEntrada.Descolar());
        	}
        }
        
        private void CargarIntr(List<byte> buff){
        	bool completada = false;
        	while((!completada)&&(!desconectar)){
        		    CargarBytes(buff,4);
        		    if(!Convertir.DeBytesAStringHEX(buff[0]).Equals("FD")){
        		    	buff.RemoveAt(0);
        		    }else{
        		    	int indice = Bytes.BuscarHEX(buff.ToArray(),1,3,"FD");
        		    	if(indice<0)
        		    		  completada =true;
        		    	        else
        		    	        	Bytes.ExtraerBytes(buff, indice);
        		    		
        		    }
        	}
        }
        
        public void GestionarDatos(){
            this.desconectar = false;
        	List<byte> miniBuff = new List<byte>();
        	int cantidadBytes = 0;
        	while(!desconectar){
        			CargarIntr(miniBuff);//intruccion
        			EjecutarIntr(Bytes.ExtraerBytes(miniBuff,4));//ejecuto instruccion
        			//averiguo longitud del paquete
        			CargarBytes(miniBuff,4);
        			cantidadBytes = BitConverter.ToInt32(Bytes.ExtraerBytes(miniBuff,4),0);
        			//cargo el paquete
        			CargarBytes(miniBuff,cantidadBytes);
        			sockComunicacion._DatosRecibidos(Bytes.ExtraerBytes(miniBuff,cantidadBytes));
        		 }
        	
            }

    }
}