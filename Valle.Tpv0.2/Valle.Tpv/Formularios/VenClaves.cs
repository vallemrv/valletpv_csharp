
using System;

namespace Valle.TpvFinal
{
	 public class EventArgClaves: EventArgs{
	     public bool estaCancelado = true;
		 public string clave = "";
		 public EventArgClaves(string clave){
			estaCancelado = false;
		    this.clave = clave;
		}
		public EventArgClaves(){}
	}

	public partial class VenClaves : Valle.GtkUtilidades.FormularioBase
	{
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
			if(salirClaves!=null) salirClaves(this,new EventArgClaves(txtPass.Data["clave"].ToString()));
			this.CerrarFormulario();
		}
		
		
		System.Timers.Timer t = new System.Timers.Timer(6000);
		string[] numreros = {"0","1","2","3","4","5","6","7","8","9"};
		
		public event EventHandler<EventArgClaves> salirClaves;

		public VenClaves () 
		{
			this.Build ();
			this.IniciarTeclado();
			this.LblTituloBase = this.lblTitulo;
			this.lblTitulo.HeightRequest = 25;
			this.Titulo = "Entrar clave desbloqueo";
			this.Decorated = false;
			this.AllowGrow = false;
			this.SkipTaskbarHint = true;
			this.KeepAbove = true;
			this.Modal = true;
			this.WindowPosition = Gtk.WindowPosition.CenterAlways;
			this.t.Elapsed+= delegate {
				this.t.Stop();
				foreach(Gtk.Widget w in this.pneTeclado.Children){
				   if(w is Gtk.Button && w.Name != "btnCancelar" && w.Name != "btnAceptar"){
						    Gtk.Button b = (Gtk.Button)w;
					          (b.Child as Gtk.Label)  .LabelProp = "<big>*</big>";
						      (b.Child as Gtk.Label) .UseMarkup =true;
					    	 }
				       }
			};
			
			
		}
		protected virtual void OnBtnCancelarClicked (object sender, System.EventArgs e)
		{
			if(salirClaves!=null) salirClaves(this,new EventArgClaves());
		    this.CerrarFormulario();

		}
		protected override void OnShown ()
		{
			base.OnShown ();
			this.PrepararTeclado();
			this.t.Start();
		}

         void IniciarTeclado(){
		    	foreach(Gtk.Widget w in this.pneTeclado.Children){
				   if(w is Gtk.Button && w.Name != "btnCancelar" && w.Name != "btnAceptar"){
						    Gtk.Button b = (Gtk.Button)w;
					             b.Clicked+= HandleBClicked; 
					        }
				       }
		    }
		

		  	void HandleBClicked (object sender, EventArgs e)
		    {
			        txtPass.Texto += "*";
		    		txtPass.Data["clave"] =txtPass.Data["clave"].ToString() +
				                        (sender as Gtk.Button).Data["key"].ToString();
		    }
		
		
         void PrepararTeclado(){
			
			txtPass.Texto = "";
			txtPass.Data["clave"] = "";
			System.Collections.ArrayList listaNum = new System.Collections.ArrayList();
			listaNum.AddRange(numreros);
			Random r = new Random();
			foreach(Gtk.Widget w in this.pneTeclado.Children){
				   if(w is Gtk.Button && w.Name != "btnCancelar" && w.Name != "btnAceptar"){
					        int i = r.Next(listaNum.Count);
						    Gtk.Button b = (Gtk.Button)w;
					          b.Data["key"] = listaNum[i].ToString();
					          (b.Child as Gtk.Label)  .LabelProp = "<big>"+listaNum[i].ToString()+"</big>";
						      (b.Child as Gtk.Label) .UseMarkup =true;
					           listaNum.RemoveAt(i);
					    	 }
				       }
			
			 
			
			
		}
	}
}
