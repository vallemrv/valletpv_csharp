using System;
using Valle.GtkUtilidades;

namespace Valle.TpvFinal
{
	public class EventoSalirElegirMonedas : EventArgs
	{
	     public	decimal Importe = 0m;
		 public decimal Cambio = 0m;
		 public decimal entrega = 0m;
		 public bool cancelado = true;
		
		public EventoSalirElegirMonedas(decimal Cambio){
			cancelado = false;
			this.Cambio = Cambio;
		}
		
		public EventoSalirElegirMonedas(){
			
		}
		
		public EventoSalirElegirMonedas(decimal Cambio, decimal Importe, decimal entrega){
			cancelado = false;
			this.Cambio = Cambio;
			this.Importe = Importe;
			this.entrega = entrega;
		}
		
		
	}
	
	public partial class ElegirMoneda : FormularioBase
	{
		protected virtual void OnBtnManualClicked (object sender, System.EventArgs e)
		{
			this.modoBilletes = false;
			this.pneMonedas.Visible =true;
			this.btnSalir.Visible =false;
			this.btnBackSp.Visible = true;
			this.btnJusto.Sensitive = false;
			this.SetSizeRequest(700,500);
			
		}
		
		public event EventHandler<EventoSalirElegirMonedas> SalirElegirMonedas;
	
		public Valle.ToolsTpv.Ticket ticket;
			
		decimal cambio = 0m;
		public decimal Cambio {
		    get{ return cambio;}
		}
		
		public bool SepararArticulos = false;
		object listaAritulos;
	    public object Articulos{
		    
			   get{
			    	SepararArticulos = false;
				    return listaAritulos;
			     }
			
			   set {
				  listaAritulos = value;
				   SepararArticulos = true;
		    	}
		}
		
		decimal importe =0m;
		public decimal Importe{
			set{
				 importe = value;
				 acomulado = 0m;
				 pilaAcomulados.Clear();
                 this.lblImporte.LabelProp = String.Format("<span size=\"xx-large\">Importe {0:c}</span>",importe);
				 this.modoBilletes = true;
			     this.pneMonedas.Visible =false;
			     this.btnSalir.Visible =true;
			     this.btnBackSp.Visible = false;
				 this.SetSizeRequest(500,500);
				
			}
			
			get{
			   return importe;	
			}
		}
		
		
		decimal acomulado = 0m;
		bool modoBilletes = true;
		System.Collections.Generic.Stack<decimal> pilaAcomulados;
		
		public ElegirMoneda () 
		{
			this.Init (); 
			this.LblTituloBase = this.lblTitulo;
			pilaAcomulados = new System.Collections.Generic.Stack<decimal>();
			Titulo = "Introducir metalico";
			this.pneMonedas.Visible = false;
			this.btnBackSp.Visible = false;
			this.WindowPosition = Gtk.WindowPosition.CenterAlways;
			this.KeepAbove = true;
		}
		
		void SumarMoneda(decimal mon){
		    pilaAcomulados.Push (mon);
			acomulado += mon;
			cambio = acomulado >= importe ? acomulado - importe : -1;
			this.MostrarResultado();
		}
		
		void MostrarResultado(){
		   	this.lblImporte.LabelProp = String.Format("<small>Importe {0:c} </small>\n"+
			                                          "<span size=\"xx-large\">Metalico {1:c} </span>\n"+
			                                          "<big>Cambio {2:c} </big>",importe,acomulado,cambio > 0 ? cambio : 0);
			
		}
		
		protected virtual void OnBtnJusto1Clicked (object sender, System.EventArgs e)
		{
			if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(0m,importe,0m));
			this.CerrarFormulario();
		}
		
		protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
		{
			if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas());
			this.CerrarFormulario();
		}
		
		protected virtual void OnBtnCancelarClicked (object sender, System.EventArgs e)
		{
		         this.modoBilletes = true;
			     this.pneMonedas.Visible =false;
			     this.btnSalir.Visible =true;
			     this.btnBackSp.Visible = false;
			     this.btnJusto.Sensitive = true;
			     this.SetSizeRequest(500,500);
		}
		
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
			if(cambio>-1m){
				if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,acomulado));
				OnBtnCancelarClicked(null,null);
				this.CerrarFormulario();
			}
		}
		
		protected virtual void OnBtn500Clicked (object sender, System.EventArgs e)
		{
		     if(modoBilletes && (500m >= importe))
			 {
				cambio = 500m - importe;
			   if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,500m));
               this.CerrarFormulario();			 
			  }	else if(!modoBilletes)
				  this.SumarMoneda(500m);
			
		}
		
		protected virtual void OnBtn200Clicked (object sender, System.EventArgs e)
		{
			if(modoBilletes && (200m >= importe))
			 {
				cambio = 200m - importe;
			   if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,200m));
               this.CerrarFormulario();			 
			  }	else if(!modoBilletes)
				this.SumarMoneda(200m);
			
		}
		
		protected virtual void OnBtn100Clicked (object sender, System.EventArgs e)
		{
			if(modoBilletes && (100m >= importe))
			 {
				cambio = 100m - importe;
			   if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,100m));
               this.CerrarFormulario();			 
			  }	else if(!modoBilletes)
				  this.SumarMoneda(100m);
			
		}
		
		protected virtual void OnBtn50Clicked (object sender, System.EventArgs e)
		{
			if(modoBilletes && (50m >= importe))
			 {
			   cambio = 50m - importe;
			   if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,50m));
               this.CerrarFormulario();			 
			  }	else{
				  this.SumarMoneda(50m);
			}
		}
		
		protected virtual void OnBtn20Clicked (object sender, System.EventArgs e)
		{
			if(modoBilletes && (20m >= importe))
			 {
				cambio = 20m - importe;
			   if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,20m));
               this.CerrarFormulario();			 
			  }	else if(!modoBilletes){
				  this.SumarMoneda(20m);
			}
		}
		
		protected virtual void OnBtn5Clicked (object sender, System.EventArgs e)
		{
			if(modoBilletes && (5m >= importe))
			 {
				cambio = 5m - importe;
			   if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,5m));
               this.CerrarFormulario();			 
				
			  }	else if (!modoBilletes)
			 	            this.SumarMoneda(5m);
			
		}
		
		protected virtual void OnBtn10Clicked (object sender, System.EventArgs e)
		{
			if(modoBilletes && (10m >= importe))
			 {
				cambio = 10m - importe;
			   if(SalirElegirMonedas!=null) SalirElegirMonedas(this,new EventoSalirElegirMonedas(cambio,importe,10m));
               this.CerrarFormulario();			 
			  }	else if(!modoBilletes)
				      this.SumarMoneda(10m);
			
		}
		
		protected virtual void OnBtn2Clicked (object sender, System.EventArgs e)
		{
			this.SumarMoneda(2m);
		}
		
		protected virtual void OnBtn1Clicked (object sender, System.EventArgs e)
		{
			this.SumarMoneda(1m);
		}
		
		protected virtual void OnBtn51Clicked (object sender, System.EventArgs e)
		{
			this.SumarMoneda(0.5m);
		}
		
		protected virtual void OnBtn21Clicked (object sender, System.EventArgs e)
		{
			this.SumarMoneda(0.20m);
		}
		
		protected virtual void OnBtn11Clicked (object sender, System.EventArgs e)
		{
			this.SumarMoneda(0.10m);
		}
		
		protected virtual void OnBtn6Clicked (object sender, System.EventArgs e)
		{
			this.SumarMoneda(0.05m);
		}
		
		
		protected virtual void OnBtnBackSpClicked (object sender, System.EventArgs e)
		{
		  if(pilaAcomulados.Count>0){
			decimal mon = pilaAcomulados.Pop();
			acomulado -= mon;
			cambio = acomulado >= importe ? acomulado - importe : -1;
			this.MostrarResultado();
			}
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
	}
}

