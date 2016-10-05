using System;
using System.Collections.Generic;
using Valle.Utilidades;

namespace Valle.GtkUtilidades
{
	public partial class VenBotones : FormularioBase
	{
		public event OnClickBtnBotonera clickBoton;
		
		Valle.Utilidades.IInfBoton[] botones;
		
		public Utilidades.IInfBoton[] Botones {
			get {
				return this.botones;
			}
			set {
				botones = value;
				this.botoneraTeclas.AgregarBotones(value);
			}
		}
		
		Utilidades.PaginasObj<IInfBoton> paginas = null;
		public Utilidades.PaginasObj<IInfBoton> PaginasBtn{
			get {
				return paginas;
			}
			set {
				paginas = value;
				this.botoneraTeclas.AgregarBotones(value);
			}
		}
		
		public Dictionary<IInfBoton,MiBoton> BotonesGtk{
		    get{return this.botoneraTeclas.BotonesGtk;}	
		}
		
		public bool MostrarControles{
		      get{
				return botoneraTeclas.MostrarControles;
			}
			set{
				botoneraTeclas.MostrarControles = value;
			}
			
		}
		
		public bool MostrarMas{
		      get{
				return botoneraTeclas.MostrarMas;
			}
			set{
				botoneraTeclas.MostrarMas = value;
			}
			
		}

		public void Redimensionar(int alto, int ancho){
		     botoneraTeclas	.Redimensionar(alto,ancho);
		}
		
		
		public bool MostrarSalir
		{
			set{ botoneraTeclas.MostrarSalir = value;}
			get{ return botoneraTeclas.MostrarSalir;}
		}
		
		
		public VenBotones () 
		{
			this.Init (); 
			this.LblTituloBase = this.lblTitulo;
			this.botoneraTeclas.clickBoton += HandleBotonera4handleclickBoton;
			this.KeepAbove = true;
			this.botoneraTeclas.paginacionClick+= delegate {
				PulsadoRecientemente = true;
			};
		}
		
		public VenBotones (int numBotones) 
		{
			this.Init (); 
			this.botoneraTeclas.Redimensionar(numBotones,numBotones);
			this.LblTituloBase = this.lblTitulo;
		    this.botoneraTeclas.clickBoton += HandleBotonera4handleclickBoton;
			this.KeepAbove = true;
			
		}
		
		protected void HandleBotonera4handleclickBoton (MiBoton boton, object args)
		{
			PulsadoRecientemente= true;
			if(clickBoton!=null) clickBoton(boton,args);
			if(SalirAlPulsar) this.CerrarFormulario();
		}
		
		
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			lblTitulo.Texto = Titulo;
			this.GrabFocus();
			return base.OnExposeEvent (evnt);
		}
		
		protected override void OnDestroyed ()
		{
			base.OnDestroyed ();
		}
	}
}

