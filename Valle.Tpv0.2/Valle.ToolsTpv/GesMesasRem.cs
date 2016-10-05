/*
 * Creado por SharpDevelop.
 * Usuario: BlackCrystal™
 * Fecha: 16/05/2010
 * Hora: 15:04
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Threading;


namespace Valle.ToolsTpv
{
	/// <summary>
	/// Description of GesMesasRem.
	/// </summary>
	public class GesMesasRem: MarshalByRefObject
	{
	    
	    AutoResetEvent exmut = new AutoResetEvent(true);
	    List<string> mesasBloqueadas = new List<string>();
	    public string Rut_mesas;
		
		
		public bool bloquearMesa(string nomMensa){
		   exmut.WaitOne();
		   if(estaBloqueada(nomMensa)){
		      exmut.Set();
		      return false;
		   }else{
		    mesasBloqueadas.Add(nomMensa);
		    exmut.Set();
		    return true;
		    }
		}
		public void desbloquearMesa(string nomMesa){
		   exmut.WaitOne();
		     mesasBloqueadas.Remove(nomMesa);
		   exmut.Set();
		}
		
		bool estaBloqueada(string nomMesa){
		     return mesasBloqueadas.Contains(nomMesa);
		}
		
		public void GuardarMesas(Mesa mesa, string nomMesaActiva){
		 FileStream f = new FileStream(Rut_mesas + Path.DirectorySeparatorChar + nomMesaActiva, FileMode.OpenOrCreate, FileAccess.Write);
				BinaryFormatter b = new BinaryFormatter();
				b.Serialize(f, mesa);
				f.Close();
		}
		
		public void CerrarMesas(string nomMesaActiva){
		  FileInfo f = new FileInfo(Rut_mesas + Path.DirectorySeparatorChar + nomMesaActiva);
				f.Delete();
	    }
		
		public Mesa Deserializar(string NomMesaActiva){
		   BinaryFormatter formateAdorBinario = new BinaryFormatter();
           FileInfo mesa = new FileInfo(Rut_mesas + Path.DirectorySeparatorChar + NomMesaActiva);
		   FileStream f = mesa.OpenRead();
		   Mesa mesaActiva = (Mesa)formateAdorBinario.Deserialize(f);
		   f.Close();
		   return mesaActiva;
		}
		
		public bool EstaLaMesaAbierta(string nomMesa)
		{

			FileInfo mesa = new FileInfo(Rut_mesas + Path.DirectorySeparatorChar + nomMesa);
			return mesa.Exists;

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
}
