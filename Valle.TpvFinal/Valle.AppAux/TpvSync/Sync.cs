
using System;
using System.Threading;
using System.Timers;


namespace TpvSync
{


	public class Sync
	{
	    System.Timers.Timer run;
		Thread h = null;
		

		public Sync ()
		{
			//lanzamos la sincronizcion
			h = new Thread(new ThreadStart(EjSync));
				  h.Start();               
			
			//por si el programa esta en reposo no ocupar mucho el procesador
			run = new System.Timers.Timer(30000);
			run.Elapsed += delegate {
				if((h!=null)&&(!h.IsAlive)){
				  h = new Thread(new ThreadStart(EjSync));
				  h.Start();               
				}
			};
			run.Start();
		}
		
		void EjSync(){
		    bool exec = true;
			while(exec){
				//exec = ComprobarMensajes();
				//exec = SyncBaseTpv();
			
			}
		}
		
	}
}
