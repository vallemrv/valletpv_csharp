// HBuscador.cs created with MonoDevelop
// User: valle at 23:54 22/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Threading;

namespace Valle.Utilidades
{
	
	public delegate void OnEjecutarAcionBus(DataRow val);
	
	public class HBuscador
	{
		event OnEjecutarAcionBus insertar;
		DataTable tb;
		string criterio;
		Thread hilo;
		
		public HBuscador(OnEjecutarAcionBus ej, DataTable vTb)
		{
			insertar+=ej;
			tb = vTb;
		}
		
		public void Buscar(string criterio){
		    this.criterio = criterio;
	        if((hilo!=null)&&(hilo.IsAlive)) hilo.Abort();
		    hilo = new Thread(new ThreadStart(this.HBuscar));
	        hilo.Start();
		}
		
		public void Parar(){
		      if((hilo!=null)&&(hilo.IsAlive)) hilo.Abort();
		}
		
		private void HBuscar(){
		   DataRow[] drs = tb.Select(this.criterio);
			  if((drs.Length>0)&&(drs.Length<tb.Rows.Count)){
			     CrearLista(drs);
			   }else{
                 insertar(null);
			   }
		}
		
		private void CrearLista(DataRow[] drs){
			foreach(DataRow dr in drs){
				insertar(dr);
				Thread.Sleep(50);
			}
		}
	}
}
