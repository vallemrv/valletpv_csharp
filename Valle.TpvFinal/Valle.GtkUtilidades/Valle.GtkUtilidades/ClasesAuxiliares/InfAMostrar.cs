// InfSplash.cs created with MonoDevelop
// User: valle at 14:45 22/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Valle.GtkUtilidades
{
	
	public enum TipoInfMostrar { MensajeLabel, MensajeBarra, progresoBarr} 
	public enum DirBarra{ BarUno, BarDos}
	public class InfAMostrar
	{
		public TipoInfMostrar tipo;
		private double progreso;
		
		public double Progreso {
			get {
				return progreso;
			}
			set {
				progreso = value;
				tipo = TipoInfMostrar.progresoBarr;
			}
		}

		public string Mensaje {
			get {
				return mensaje;
			}
			set {
				mensaje = value;
				tipo = TipoInfMostrar.MensajeLabel;
			}
		}

		public string Informacion {
			get {
				return informacion;
			}
			set {
				informacion = value;
				tipo = TipoInfMostrar.MensajeBarra;
			}
		}
		
		private string mensaje;
		
		private string informacion;
		
		public DirBarra dirBarra = DirBarra.BarUno;
		
		public InfAMostrar(Object inf, TipoInfMostrar tipo){
		   switch(tipo){
	               case TipoInfMostrar.MensajeBarra:
	                this.Informacion = inf.ToString();
	               break;
	               case TipoInfMostrar.MensajeLabel:
	                this.Mensaje = inf.ToString();
	               break;
	               case TipoInfMostrar.progresoBarr:
	                this.Progreso = Double.Parse(inf.ToString());
	               break;
	           }
	           
		
		}
		
		public InfAMostrar(Object inf, TipoInfMostrar tipo, DirBarra dir){
		     this.dirBarra = dir;
		   switch(tipo){
		           case TipoInfMostrar.MensajeBarra:
	                this.Informacion = inf.ToString();
	               break;
	               case TipoInfMostrar.MensajeLabel:
	                this.Mensaje = inf.ToString();
	               break;
	               case TipoInfMostrar.progresoBarr:
	                this.Progreso = Double.Parse(inf.ToString());
	               break;
	           }
	           
		
		}
		
	}
}
