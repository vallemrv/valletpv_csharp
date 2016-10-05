using System;
using Valle.GtkUtilidades;
using Gtk;

namespace Valle.TpvFinal
{
	public partial class TicketCobrado : FormularioBase
	{
		public TicketCobrado(int x, int y)
        {
            this.Init (); 
			this.SetPosition(WindowPosition.None);
            this.Move(x,y);this.LblTituloBase = this.milabel1;
			this.lblC.ColorDeFono = System.Drawing.Color.Red;
			this.lblC.ColorLetras = System.Drawing.Color.Black;
			this.lblC.Font = new System.Drawing.Font(lblC.Font.FontFamily,15);
			this.lblC.AlienamientoH = System.Drawing.StringAlignment.Near;
			this.lblCambio.ColorDeFono = System.Drawing.Color.Red;
			this.lblCambio.ColorLetras = System.Drawing.Color.Black;
			this.lblCambio.Font = new System.Drawing.Font(lblCambio.Font.FontFamily,15);
			this.lblCambio.AlienamientoH = System.Drawing.StringAlignment.Far;
			this.lblT.ColorDeFono = System.Drawing.Color.Black;
			this.lblT.ColorLetras = System.Drawing.Color.Red;
			this.lblT.Font = new System.Drawing.Font(lblT.Font.FontFamily,38);
			this.lblT.AlienamientoH = System.Drawing.StringAlignment.Near;
			this.lblImporte.ColorDeFono = System.Drawing.Color.Black;
			this.lblImporte.ColorLetras = System.Drawing.Color.Red;
			this.lblImporte.Font = new System.Drawing.Font(lblImporte.Font.FontFamily,38);
			this.lblImporte.AlienamientoH = System.Drawing.StringAlignment.Far;
			KeepAbove=true;
		 }

        string cambio ;
		string importe;
		

        public void EstablecerInformacion(String numTicket, String camarero, String importe, String cambio)
        {
            this.Titulo = String.Format("Ticket numero {0} por {1}", numTicket, camarero);
            this.cambio = cambio;
			this.importe = importe;
			this.lblC.Texto = "Cambio";
			this.lblT.Texto = "Total";
			this.lblImporte.Texto = importe;
			this.lblCambio.Texto = cambio;
			
        }
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			this.lblC.Texto = "Cambio";
			this.lblT.Texto = "Total";
			this.lblImporte.Texto = importe;
			this.lblCambio.Texto = cambio;
			return base.OnExposeEvent (evnt);
		}
	}
}

