using System;
using Valle.GtkUtilidades;
using Valle.ToolsTpv;
using Gdk;


namespace Valle.TpvFinal
{
	public partial class ImfTicket : FormularioBase
	{
		public ImfTicket () 
		{
			this.Init ();
			this.LblTituloBase = this.lblTitulo;
		    scrooltactil1.wScroll =	this.GtkScrolledWindow;
		}
		
		public void MostrarInformacion(Mesa mesa){
		       	
			
		}
		
		
		protected override bool OnExposeEvent (EventExpose evnt)
		{
			Titulo = "Informacion de ticket";
			return base.OnExposeEvent (evnt);
		}
	}
}

