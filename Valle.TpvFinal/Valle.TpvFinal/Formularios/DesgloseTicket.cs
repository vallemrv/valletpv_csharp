using System;
using Valle.GtkUtilidades;
	
namespace Valle.TpvFinal
{
	
	public enum AccionesDesglose { dividir, separar, llenar, informacion, salir }
    public delegate void OnAccionDesglose(AccionesDesglose accion);
   
	public partial class DesgloseTicket : FormularioBase
	{
		public DesgloseTicket ()  
		{
			this.Init();
			this.LblTituloBase = this.lblTitulo;
			this.WindowPosition = Gtk.WindowPosition.CenterAlways;
			this.KeepAbove = true; this.Modal = true;
			this.Titulo = "Herramientas de tickets";
		}
		 
		public AccionesDesglose accion;
        public event OnAccionDesglose EjAccion;
        public bool bloquear{
           set{ 
             this.btnSepararTicket.Sensitive = value;
             this.btnInfTicket.Sensitive = value;
             this.btnDividirTicket.Sensitive = value;
             }
           
        }
        
        private void btnDividir_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            accion = AccionesDesglose.dividir;
            if (SalirAlPulsar) { base.btnSalir_Click(sender, e); }
            if(EjAccion != null){EjAccion(accion);}
	    }

        private void btnSeparar_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            accion = AccionesDesglose.separar;
            if (SalirAlPulsar) { base.btnSalir_Click(sender, e); }
            if (EjAccion != null) { EjAccion(accion); }
        }

        private void btnLlenar_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            accion = AccionesDesglose.llenar;
            if (SalirAlPulsar) { base.btnSalir_Click(sender, e); }
            if (EjAccion != null) { EjAccion(accion); }
          
        }
        protected override void btnSalir_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            accion = AccionesDesglose.salir;
            base.btnSalir_Click(sender, e);
            if (EjAccion != null) { EjAccion(accion); }
        }

        
        
        void BtnEstadisticaClick(object sender, EventArgs e)
        {
        	PulsadoRecientemente = true;
            accion = AccionesDesglose.informacion;
            base.btnSalir_Click(sender, e);
            if (EjAccion != null) { EjAccion(accion); }
        }
		
        
		
		
	}
}

