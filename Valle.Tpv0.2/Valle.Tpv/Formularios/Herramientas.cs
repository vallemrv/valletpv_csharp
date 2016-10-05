using System;
using Valle.GtkUtilidades;

namespace Valle.TpvFinal
{
	
		 public delegate void OnAccionHerramientas(AccionesHerramientas accion, object datos);
         public enum AccionesHerramientas {MkClaves, ArqueoCaja, CajaDia, CajaMens, ListadoCierres, CambiarModoImp,ReiniciarTpv, CambiarTpv,
    	 ConfigConex, Minimizar, Nada}
   
	public partial class Herramientas : FormularioBase
	{
		
		
        public bool puedoImprimir;
        public event OnAccionHerramientas EjAccion;
		public AccionesHerramientas acion  = AccionesHerramientas.Nada;
		public bool noSombra = true;
		
		bool bloqueado = false;
		public bool esBloqueado
		{
		  set{
	           this.btnBorrarZ.Sensitive = !value;
			   this.btnMinimizar.Sensitive = !value;
			   this.btnComfiguracion.Sensitive = !value;
			   this.btnListado.Sensitive = !value;
			   this.btnTrimestres.Sensitive = !value;
			   this.lblAdminitrador.Texto = value ? "Bloqueado por el administrador":
					"Modo administrador";
				this.imgKey.Pixbuf = !value ? Gdk.Pixbuf.LoadFromResource("Valle.Tpv.iconos.MUNECO_CANDAO_03.png") :
					Gdk.Pixbuf.LoadFromResource("Valle.Tpv.iconos.key.jpeg");
				this.lblBtnBloquear.LabelProp = !value ? "<big>Bloquear</big>":"<big>Desbloquear</big>";
				bloqueado = value;
			}
		}
		
		public Herramientas (bool puedoImp, bool esBloqueado)
		{
			this.Init ();
			this.btnTrimestres.Visible = false;
			this.Modal = true;
		    this.LblTituloBase = lblTitulo;
			this.Titulo = "Opciones del TPV";
           	this.WindowPosition = Gtk.WindowPosition.CenterAlways;	
		    this.lblAdminitrador.AlienamientoH = System.Drawing.StringAlignment.Center;
			this.lblImprimir.AlienamientoH = System.Drawing.StringAlignment.Center;
			puedoImprimir = puedoImp;
			this.btnArqueoCaja.Clicked+= HandleBtnArqueoCajahandleClicked;
			this.esBloqueado = esBloqueado;
			
           
      	}

		

		void HandleBtnArqueoCajahandleClicked (object sender, EventArgs e)
		{
			noSombra = false;
	        PulsadoRecientemente = true;
            if(EjAccion!=null)    EjAccion(AccionesHerramientas.ArqueoCaja,null);
            if(SalirAlPulsar)  CerrarFormulario();   
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
			noSombra =true;
            PulsadoRecientemente = true;
            if(EjAccion!=null) EjAccion(AccionesHerramientas.CajaMens, null);
			if(SalirAlPulsar) CerrarFormulario();
                
        }

        private void btnBloqueo_Click(object sender, EventArgs e)
        {
		    noSombra = false;
            PulsadoRecientemente = true;
			this.CerrarFormulario();
		  
            if(EjAccion!=null) EjAccion(AccionesHerramientas.MkClaves, null);
	        
        }

        protected override void btnSalir_Click(object sender, EventArgs e)
        {
			noSombra =true;
            PulsadoRecientemente = true;
            if(EjAccion!=null) EjAccion(AccionesHerramientas.Nada, null);
            if(SalirAlPulsar) CerrarFormulario();
        }

        private void btnCambiarTpv_Click(object sender, EventArgs e)
        {
			noSombra = false;
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
			noSombra = false;
			PulsadoRecientemente = true;
			acion = AccionesHerramientas.ListadoCierres;
        	if(EjAccion!=null) EjAccion(AccionesHerramientas.ListadoCierres, null);
        	if(SalirAlPulsar) this.CerrarFormulario();
        }
        
        void BtnMinimizarClick(object sender, EventArgs e)
        {
			noSombra =true;
			PulsadoRecientemente = true;
			acion = AccionesHerramientas.Minimizar;
        	if(EjAccion!=null) EjAccion(AccionesHerramientas.Minimizar, null);
        	if(SalirAlPulsar) CerrarFormulario();
        }
        
        void BtnConfigClienteClick(object sender, EventArgs e)
        {
			noSombra =true;
			PulsadoRecientemente = true;
			acion = AccionesHerramientas.ConfigConex;
        	if(EjAccion!=null) EjAccion(AccionesHerramientas.ConfigConex, null);
        	if(SalirAlPulsar) CerrarFormulario();
        }
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			this.lblBtnImprimir.LabelProp = puedoImprimir ? "<big>No Imprimir</big>" : "<big>Imprimir</big>";
            this.lblImprimir.Texto = puedoImprimir ? "Ticket automatico activado" : "Ticket automatico desactivado";
        	 this.lblAdminitrador.Texto = bloqueado ? "Bloqueado por el administrador":
					"Modo administrador";
			return base.OnExposeEvent (evnt);
		}
		
		
		
		
	}
}

