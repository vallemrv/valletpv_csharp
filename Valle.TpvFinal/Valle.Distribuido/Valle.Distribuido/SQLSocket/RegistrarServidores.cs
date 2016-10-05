// RegistrarServidores.cs created with MonoDevelop
// User: valle at 14:32 29/05/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Net.Sockets;


using Valle.SqlGestion;
using Valle.Distribuido.misSocket;

namespace Valle.Distribuido.SQLSocket
{
    
    
    public class RegistrarServidores : IDisposable
    {
         IGesSql gesSql;
         ServidorSock m_servidorEscucha;
         List<ServidorDeTrabajo> sockList = new List<ServidorDeTrabajo>();
       
       
        public RegistrarServidores(IGesSql gesSQl, int port)
        {
           gesSql = gesSQl;
           m_servidorEscucha = new ServidorSock(this.OnClienteConectado,port);
           
        }
        
        void OnClienteConectado(Socket sock){
            ServSqlSock ges = new  ServSqlSock(this.gesSql); 
            ServidorDeTrabajo sT = new ServidorDeTrabajo(ges.datosRecibidos,this.clienteDesconectado,true,sock);
            sockList.Add(sT);
        }
        
        void clienteDesconectado(SockDeComunicacion sock)
        {
               sockList.Remove((ServidorDeTrabajo)sock);
        }
       
        public void Dispose(){
          foreach(ServidorDeTrabajo s in sockList){
             s.Desconectar();
          }
		   this.m_servidorEscucha.Desconectar();
         }
        
    }
}
