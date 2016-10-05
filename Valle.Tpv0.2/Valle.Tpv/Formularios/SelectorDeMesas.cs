using System;
using System.Collections.Generic;

using Valle.GtkUtilidades;
using Valle.ToolsTpv;
using Valle.Utilidades;

namespace Valle.TpvFinal
{
	 
	public class EventArgMesas : EventArgs
	{
	    public static int TIPO_HERRAMIENTAS = 0;
		public static int TIPO_BTN_MESAS = 1;
		
		int tipo;
		
		public int Tipo{
		   get{ return tipo;}	
		}
		
		public object Datos;
		
		public EventArgMesas(int tipo, object Datos){
		   this.tipo = tipo; this.Datos = Datos;	
		}
	}
	
	public partial class SelectorDeMesas : FormularioBase
	{
		public event EventHandler<EventArgMesas> selectorClick;
		public bool Saliendo = false;
		Dictionary<string, IInfBoton[]> botonesMesas = new Dictionary<string, IInfBoton[]>();
        Dictionary<string, MiBoton> asociaciones =new Dictionary<string, MiBoton>();
		VenBotones zonas;
		VenBotones mesas;
		GesMesas gesMesas;
		string nomCamarero="";
		MiBoton btnZonaActiva = null;
		
		
		public string NomCamarero {
			get {
				return this.nomCamarero;
			}
			set {
				nomCamarero = value;
		    }
		}
		string informacion = "Herramentas de mesas";
		public string Informacion{
		   set{ lblInformacion.Texto = value;
				informacion = value;		}
		}
		
		

		public SelectorDeMesas(GesMesas gesMesas )
		{
			this.gesMesas = gesMesas;
			this.Init ();
			
			this.CrearBontonesPorZona();
		
			zonas = new VenBotones();
			mesas = new VenBotones();
			zonas.Redimensionar(6,1);
			
			zonas.Botones = gesMesas.Zonas.ToArray();
			zonas.Titulo = "Zonas";
			zonas.MostrarControles = false;
			zonas.SalirAlPulsar = false;
			zonas.esTemporizado = false;
			zonas.clickBoton += HandleZonasclickZona; 
			
			mesas.MostrarControles = false;
			mesas.esTemporizado = false;
			mesas.SalirAlPulsar = false;
			mesas.clickBoton +=  HandleMesasclickMesa;
			
			MiBoton b = zonas.BotonesGtk[zonas.Botones[0]];
			HandleZonasclickZona(b,b.Datos);
			
			this.WindowPosition = Gtk.WindowPosition.None;
			mesas.WindowPosition = Gtk.WindowPosition.None;
			mesas.TransientFor = this;
			zonas.WindowPosition = Gtk.WindowPosition.None;
			zonas.TransientFor = this;
			
			this.lblInformacion.Font = new System.Drawing.Font(this.lblTitulo.Font.FontFamily,10,System.Drawing.FontStyle.Bold);
			this.lblInformacion.ColorDeFono = System.Drawing.Color.Azure;
			this.lblInformacion.AlienamientoH = System.Drawing.StringAlignment.Center;
			this.btnAsociar.Sensitive = false;
			
			this.LblTituloBase = this.lblTitulo;
				this.Titulo = "Selecionar Mesas";
			
		}

		void HandleMesasclickMesa (MiBoton boton, object args)
		{
			 if(SalirAlPulsar)
                     this.CerrarFormulario();
            if(selectorClick!=null) selectorClick(boton, new EventArgMesas(EventArgMesas.TIPO_BTN_MESAS, boton.Datos));
		}

		void HandleZonasclickZona (MiBoton boton, object args)
		{
		   this.PulsadoRecientemente = true;
      		  if(btnZonaActiva!=null)
	                 btnZonaActiva.ColorDeFono = ((ZonaMesas)btnZonaActiva.Datos).colorZona;
	                 
	         this.btnAsociar.Sensitive = true;        
	         MiBoton bZ = (MiBoton)boton;
	         btnZonaActiva = bZ;
	         bZ.ColorDeFono = System.Drawing. Color.Purple;
			 mesas.Redimensionar(((ZonaMesas)btnZonaActiva.Datos).botonesAlto,((ZonaMesas)btnZonaActiva.Datos).botonesAncho);
	         mesas.Botones = botonesMesas[((ZonaMesas)args).nomZona];
			 mesas.Titulo ="Lugar ---- "+  ((ZonaMesas)args).nomZona;
		     mesas.Move(50,220);
		
			
		}

		protected override void btnSalir_Click(object sender, EventArgs e)
	    {   
			this.Saliendo = true;
			this.CerrarFormulario();
	        if(selectorClick!=null) selectorClick(this,new EventArgMesas(EventArgMesas.TIPO_HERRAMIENTAS,"salir"));
	    }
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			    this.lblTitulo.Texto = "Camarero >>>  "+ nomCamarero;
			    this.lblInformacion.Texto = informacion;
			    	this.Move(Screen.Width-this.Allocation.Width-zonas.Allocation.Width-25,
			              Screen.Height-this.Allocation.Height-20);
			        zonas.Move(Screen.Width-zonas.Allocation.Width-5,15);
	    		return base.OnExposeEvent (evnt);
			    
		}
		
	   public bool OperandoConMesas{
			set{
				this.btnCancelar.Visible = value;
				this.btnSalir.Visible = !value;
				this.btnAbrirCajon.Visible =!value;
				this.btnAsociar.Sensitive = !value;
				this.btnQuitarAsociacion.Sensitive= !value;
				this.btnCambiar.Sensitive =!value;
				this.btnInvitar.Sensitive=!value;
				this.btnJuntar.Sensitive=!value;
				if(!value)
					  HandleZonasclickZona(btnZonaActiva,btnZonaActiva.Datos);
			}
		}
		

    
        void CrearBontonesPorZona()
        {
            
            foreach(ZonaMesas z in gesMesas.zonas){ 
               	botonesMesas.Add(z.nomZona, (IInfBoton[])gesMesas.mesas_botones[z.nomZona]);
             }  
           
        }
    
        
		protected override bool OnVisibilityNotifyEvent (Gdk.EventVisibility evnt)
		{
			zonas.Visible = mesas.Visible = this.Visible;
			return base.OnVisibilityNotifyEvent (evnt);
		}
		
	
		public override void Destroy ()
		{  
			zonas.Destroy(); mesas.Destroy();
			base.Destroy ();
		}
		
		protected override void OnShown ()
		{
			this.Saliendo = false;
			if(asociaciones.ContainsKey(nomCamarero))
				        HandleZonasclickZona(asociaciones[nomCamarero],asociaciones[nomCamarero].Datos);    
        	   else if (btnZonaActiva!=null)
				                       HandleZonasclickZona(btnZonaActiva,btnZonaActiva.Datos);
				
			base.OnShown ();
		}
		
		
		protected override void OnHidden ()
		{
			  if((zonas!=null)&&(mesas!=null)){	
			      zonas.Hide();mesas.Hide();
			}
				base.OnHidden ();
		}
		
		protected virtual void OnBtnAsociarClicked (object sender, System.EventArgs e)
		{
			PulsadoRecientemente =true;
           if(!asociaciones.ContainsKey(nomCamarero)){
              asociaciones.Add(nomCamarero,btnZonaActiva);
			    this.btnAsociar.Visible = false;
				this.btnQuitarAsociacion.Visible = true;
			}	
       	}
		
		protected virtual void OnBtnCambiarClicked (object sender, System.EventArgs e)
		{
			this.OperandoConMesas = true;
			if(esTemporizado) this.TemporizadorDeCierre.Stop();
			if(selectorClick!=null) selectorClick(this,new EventArgMesas(EventArgMesas.TIPO_HERRAMIENTAS,"cambiar"));
	   }
		
		protected virtual void OnBtnJuntarClicked (object sender, System.EventArgs e)
		{
			this.OperandoConMesas = true;
			if(esTemporizado) this.TemporizadorDeCierre.Stop();
			if(selectorClick!=null) selectorClick(this,new EventArgMesas(EventArgMesas.TIPO_HERRAMIENTAS,"juntar"));
	  	}
		
		protected virtual void OnBtnInvitarClicked (object sender, System.EventArgs e)
		{
			this.OperandoConMesas = true;
			if(esTemporizado) this.TemporizadorDeCierre.Stop();
			if(selectorClick!=null) selectorClick(this,new EventArgMesas(EventArgMesas.TIPO_HERRAMIENTAS,"invitar"));
		}
		
		protected virtual void OnBtnAbrirCajonClicked (object sender, System.EventArgs e)
		{
			if(SalirAlPulsar){
				this.CerrarFormulario();
				if(selectorClick!=null) selectorClick(this,new EventArgMesas(EventArgMesas.TIPO_HERRAMIENTAS,"AbrirCajon"));
			}
		}
		
	    
		protected virtual void OnBtnCandelarClicked (object sender, System.EventArgs e)
		{
			this.OperandoConMesas = false;
			if(esTemporizado) this.TemporizadorDeCierre.Start();
			if(selectorClick!=null) selectorClick(this,new EventArgMesas(EventArgMesas.TIPO_HERRAMIENTAS,"cancelar"));
		}
		
		protected virtual void OnBtnQuitarAsociacionClicked (object sender, System.EventArgs e)
		{
			this.btnAsociar.Visible =true;
			this.btnQuitarAsociacion.Visible = false;
			this.asociaciones.Remove(nomCamarero);
			
		}
		
	}
}

