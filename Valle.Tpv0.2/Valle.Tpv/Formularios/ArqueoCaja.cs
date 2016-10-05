
using System;
using Valle.GtkUtilidades;

namespace Valle.TpvFinal
{

	public class ArqueoCajaArgs : EventArgs{
	    public System.Collections.Generic.Dictionary<String,decimal> monedas = new System.Collections.Generic.Dictionary<String, decimal>();
		public bool estaCancelado = true;
		public ArqueoCajaArgs(System.Collections.Generic.Dictionary<String,decimal> monedas ){
			this.monedas = monedas;
		    estaCancelado = false;
		}
		public ArqueoCajaArgs(){}
	}

	public partial class ArqueoCaja : Valle.GtkUtilidades.FormularioBase
	{
		Valle.GtkUtilidades.Sombra sombra = new Valle.GtkUtilidades.Sombra ();
		Valle.GtkUtilidades.TecladoNumerico EntradaCantidad;
		System.Collections.Generic.Dictionary<String,decimal> monedas = new System.Collections.Generic.Dictionary<String, decimal>();
		string cantidadActiva = "";
		MiLabel lblActivo = null;
		
		public event EventHandler<ArqueoCajaArgs> SalirArqueoCaja;
		
		protected virtual void OnBtn500Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="500";
			lblActivo = lbl500;
			EntradaCantidad.Titulo="Introduce num billetes 500";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			EntradaCantidad.Show();
		}
		
		public void Inicializar ()
		{
			this.monedas.Clear();
			
			Gtk.Widget[] controles = this.pneBotonera.Children;
			 foreach(Gtk.Widget c in controles){
				if(c is Gtk.VBox){
					foreach(Gtk.Widget l in (c as Gtk.VBox).Children){
			             if(l is MiLabel){
							MiLabel ml =(l as MiLabel);
				           ml.Font = new System.Drawing.Font(ml.Font.FontFamily,10, System.Drawing.FontStyle.Bold);
						   ml.AlienamientoH = System.Drawing.StringAlignment.Center;
						    ml.Texto = "0";
					     }
			         }
			    }
		     }
		}	
		string rut="",nom="";
		public ArqueoCaja (string rut, string nom) 
		{
			this.Build ();
			this.rut = rut;
			this.nom = nom;
			this.Inicializar();
			LblTituloBase = txtTitulo;
			Titulo =  "Contabilizar metalico y gastos del cajón";
			
			sombra.TransientFor = this;
			sombra.KeepAbove = true;
			this.Decorated = false;
			this.AllowGrow = false;
			this.SkipTaskbarHint = true;
			this.KeepAbove = true;
			this.WindowPosition = Gtk.WindowPosition.CenterAlways;
			
			EntradaCantidad = new TecladoNumerico(FormatosNumericos.F_Decimal,"Introduzca Monedas");
			EntradaCantidad.WindowPosition = Gtk.WindowPosition.CenterAlways;
			EntradaCantidad.SalirAlPulsar =false;
			EntradaCantidad.TransientFor = this;
			EntradaCantidad.Modal = true;
			EntradaCantidad.KeepAbove = true;
			EntradaCantidad.salirNumerico+= delegate(AccionesNumerico accion, string numero) {
			
				sombra.Hide();
				if(accion == AccionesNumerico.AC_Aceptar && numero.Length>0){
				     lblActivo.Texto = EntradaCantidad.Numero ; 
					   
					   if(monedas.ContainsKey(cantidadActiva)) monedas[cantidadActiva]= Decimal.Parse(EntradaCantidad.Numero);
					   else   monedas.Add(cantidadActiva, Decimal.Parse( EntradaCantidad.Numero));
				}
			};
			
		   
		}
		
		protected virtual void OnBtn200Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="200";
			lblActivo = lbl200;
			EntradaCantidad.Titulo="Introduce num billetes 200";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			this.Modal = false;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn100Clicked (object sender, System.EventArgs e)
		{
		    cantidadActiva="100";
			lblActivo = lbl100;
			EntradaCantidad.Titulo="Introduce num billetes 100";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			this.Modal = false;
			EntradaCantidad.Show();	
		}
		
		protected virtual void OnBtn50Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="50";
			lblActivo = lbl50;
			EntradaCantidad.Titulo="Introduce num billetes 50";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn20Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="20";
			lblActivo = lbl20;
			EntradaCantidad.Titulo="Introduce num billetes 20";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn10Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="10";
			lblActivo = lbl10;
			EntradaCantidad.Titulo="Introduce num billetes 10";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn5Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="5";
			lblActivo = lbl5;
			EntradaCantidad.Titulo="Introduce num billetes 5";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn2Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="2";
			lblActivo = lbl2;
			EntradaCantidad.Titulo="Introduce num monedas 2";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn1Clicked (object sender, System.EventArgs e)
		{
			cantidadActiva="1";
			lblActivo = lbl1;
			EntradaCantidad.Titulo="Introduce num monedas 1";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn50cClicked (object sender, System.EventArgs e)
		{
			cantidadActiva="0,50";
			lblActivo = lbl50c;
			EntradaCantidad.Titulo="Introduce num mondedas 50c";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtn20cClicked (object sender, System.EventArgs e)
		{
			cantidadActiva="0,20";
			lblActivo = lbl20c;
			EntradaCantidad.Titulo="Introduce num monedas 20c";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_entero;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtnVariosClicked (object sender, System.EventArgs e)
		{
			cantidadActiva="varios";
			lblActivo = lblVarios;
			EntradaCantidad.Titulo="Importe de monedas pequeñas";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_Decimal;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtnTargetaClicked (object sender, System.EventArgs e)
		{
			cantidadActiva="targeta";
			lblActivo = lblTargeta;
			EntradaCantidad.Titulo="Importe de cobros por targeta";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_Decimal;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtnGastosClicked (object sender, System.EventArgs e)
		{
			cantidadActiva="gastos";
			lblActivo = lblGastos;
			EntradaCantidad.Titulo="Suma total de gastos pagados por caja";
			EntradaCantidad.Numero = "";
			EntradaCantidad.Formato = FormatosNumericos.F_Decimal;
			sombra.Show();
			
			EntradaCantidad.Show();
		}
		
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
			
		  Valle.GtkUtilidades.TecladoNumerico	cambio = new TecladoNumerico(FormatosNumericos.F_Decimal,"Introduzca el cambio remanente");
			cambio.WindowPosition = Gtk.WindowPosition.CenterAlways;
			cambio.SalirAlPulsar =false;
			cambio.TransientFor = this;
			cambio.Modal = true;
			cambio.KeepAbove = true;
			cambio.Numero = "";
			cambio.salirNumerico+= delegate(AccionesNumerico accion, string numero) {
				sombra.Hide();
				if(accion == AccionesNumerico.AC_Aceptar&& numero.Length>0){
					 monedas.Add("cambio", Decimal.Parse(numero));
					 if(SalirArqueoCaja!=null) SalirArqueoCaja(this,new ArqueoCajaArgs(this.monedas));
	                 this.CerrarFormulario();
				}
			};
			sombra.Show();
			cambio.Show();
			GC.Collect();
		}
		
		protected virtual void OnBtnCancelarClicked (object sender, System.EventArgs e)
		{
			if(SalirArqueoCaja!=null) SalirArqueoCaja(this,new ArqueoCajaArgs());
			this.CerrarFormulario();
			GC.Collect();
		}
		
		protected override void OnShown ()
		{   
			this.Inicializar();
			base.OnShown ();
		}

		
		protected override void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			int x=0,y=0,width=0, height=0;
			this.GetSize(out width, out height);
			this.GetPosition(out x, out y);
			sombra.Move(x,y);
			sombra.SetSizeRequest(width,height);
			base.OnExposeEvent(o,args);
		}
		
		protected virtual void OnBtnAbrirCajonClicked (object sender, System.EventArgs e)
		{
			ImpresoraTicket.AbrirCajon(rut,nom);
		}
		
		
		
		

		
		
		
		
		
		
		
		
		
		
		
		
		
		
	}
}
