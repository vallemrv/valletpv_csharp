using System;
using Valle.GtkUtilidades;

namespace Valle.TpvFinal
{
	
		 public delegate void OnAccionHerramientas(AccionesHerramientas accion, object datos);
         public enum AccionesHerramientas { CajaDia, CajaMens, ListadoCierres, CambiarModoImp,ReiniciarTpv, CambiarTpv,
    	 ConfigConex, Minimizar, Nada}
   
	public partial class Herramientas : FormularioBase
	{
		
		
        public bool puedoImprimir;
        public event OnAccionHerramientas EjAccion;
		public AccionesHerramientas acion  = AccionesHerramientas.Nada;
    
		public Herramientas (bool puedoImp)
		{
			this.Init ();
			this.Modal = true;
			this.LblTituloBase = lblTitulo;
			this.Titulo = "Opciones del TPV";
           	this.WindowPosition = Gtk.WindowPosition.CenterAlways;	
		    this.lblAdminitrador.AlienamientoH = System.Drawing.StringAlignment.Center;
			this.lblImprimir.AlienamientoH = System.Drawing.StringAlignment.Center;
			puedoImprimir = puedoImp;
             
      	}
		
	

        private void btnModoImp_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            puedoImprimir = !puedoImprimir;
            this.lblBtnImprimir.LabelProp = puedoImprimir ? "<big>No Imprimir</big>" : "<big>Imprimir</big>";
            lblImprimir.Texto = puedoImprimir ? "Ticket automatico activado":"Ticket automatico desactivado";
            if(EjAccion!=null) EjAccion(AccionesHerramientas.CambiarModoImp,puedoImprimir);
        }

        private void btnCajaDia_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            if(EjAccion!=null)    EjAccion(AccionesHerramientas.CajaDia,null);
            if(SalirAlPulsar)  CerrarFormulario();   
        }

        private void btnMesesTrim_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            if(EjAccion!=null) EjAccion(AccionesHerramientas.CajaMens, null);
			if(SalirAlPulsar) CerrarFormulario();
                
        }

        private void btnBloqueo_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
        	//DataTable tb = new DataTable();
            /*FrmClaves claves = new FrmClaves(null, false);
            claves.ShowDialog();
            string pasEnc = Encriptar.EncriptarCadena(claves.Pass);
            MessageBox.Show(String.Format("Cadena Encriptado: {0}",pasEnc)+
                            String.Format("\nCadena Desencriptada: {0}",Encriptar.DescriptarCadena(pasEnc)+
                            String.Format("\nCadena real {0}",claves.Pass)));
            */

        }

        protected override void btnSalir_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            if(EjAccion!=null) EjAccion(AccionesHerramientas.Nada, null);
            if(SalirAlPulsar) CerrarFormulario();
        }

        private void btnCambiarTpv_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
			acion = AccionesHerramientas.CambiarTpv;
            if(EjAccion!=null) EjAccion(AccionesHerramientas.CambiarTpv, null);
            if(SalirAlPulsar) this.CerrarFormulario();
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
			acion = AccionesHerramientas.ReiniciarTpv;
            if(EjAccion!=null) EjAccion(AccionesHerramientas.ReiniciarTpv, null);
			if(SalirAlPulsar) this.CerrarFormulario();
       
        }
        
        void BtnListCierresClick(object sender, EventArgs e)
        {
			PulsadoRecientemente = true;
			acion = AccionesHerramientas.ListadoCierres;
        	if(EjAccion!=null) EjAccion(AccionesHerramientas.ListadoCierres, null);
        	if(SalirAlPulsar) this.CerrarFormulario();
        }
        
        void BtnMinimizarClick(object sender, EventArgs e)
        {
			PulsadoRecientemente = true;
			acion = AccionesHerramientas.Minimizar;
        	if(EjAccion!=null) EjAccion(AccionesHerramientas.Minimizar, null);
        	if(SalirAlPulsar) CerrarFormulario();
        }
        
        void BtnConfigClienteClick(object sender, EventArgs e)
        {
			PulsadoRecientemente = true;
			acion = AccionesHerramientas.ConfigConex;
        	if(EjAccion!=null) EjAccion(AccionesHerramientas.ConfigConex, null);
        	if(SalirAlPulsar) CerrarFormulario();
        }
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			this.lblBtnImprimir.LabelProp = puedoImprimir ? "<big>No Imprimir</big>" : "<big>Imprimir</big>";
            this.lblImprimir.Texto = puedoImprimir ? "Ticket automatico activado" : "Ticket automatico desactivado";
        	this.lblAdminitrador.Texto="Modo administrador";
	        
			return base.OnExposeEvent (evnt);
		}
		
		
		
		
	}
}

