/*
 * Creado por SharpDevelop.
 * Usuario: vallevm
 * Fecha: 15/09/2008
 * Hora: 16:21
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Permissions;
using System.Security;
using System.Collections;
using System.Runtime.Remoting.Lifetime;

using Valle.SqlGestion;



namespace Valle.Distribuido.SQLRemoting
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class RegistrarServidor
	{
	    IGesSql gesL;
		int port;
		string protocolo;
		
		
		public void hRegistrarServ(){
		//Damos permisos de ejecucion de eventos remotos
            BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

            IDictionary props = new Hashtable();
            props["port"] = port;

            //registramos la clase servidora
            //Abrimos puerto de escucha
            TcpChannel chan = new TcpChannel(props,clientProv,serverProv);
            ChannelServices.RegisterChannel(chan, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
              typeof(SQLServ),
               "SQLServ", WellKnownObjectMode.Singleton);
            RemotingConfiguration.CustomErrorsEnabled(false);

            
            //nos comunicamos con la clase para introducir algunos campos
            SQLServ gesRemoto = (SQLServ)Activator.GetObject(typeof(SQLServ),
               protocolo+"://LocalHost:"+port+"/SQLServ");
            gesRemoto.GestorDatos = gesL;	
		 
		}
		
		
      	public RegistrarServidor(IGesSql gesL, int port, string protocolo){
			
			this.gesL = gesL;
			this.port = port;
			this.protocolo = protocolo;
			
			System.Threading.Thread h = new System.Threading.Thread(new System.Threading.ThreadStart(this.hRegistrarServ));
			h.Start();
                
            
           
   		}

      
        
	}
			
    }
