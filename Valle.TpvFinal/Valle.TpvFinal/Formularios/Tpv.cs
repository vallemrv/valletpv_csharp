using System;
using System.Collections;
using System.Timers;
using Gtk;
using Pango;

using Valle.GtkUtilidades;
using Valle.Utilidades;
using Valle.ToolsTpv;


namespace Valle.TpvFinal
{
	public enum AccionesTpv { abriCajon, herramietas, salir, mesas, tiempoEspirado,camareros, ventaPorKilos, llenarLinea, cobrar, BorrarLinea,
			   varios, variosnombre, cerrarInfTicket,informacionTicket, secciones, favoritos, buscador, imprimir, opciones}
	
	public class EventTpv : EventArgs{
		public object dato;
		public AccionesTpv accion = AccionesTpv.salir;
		public EventTpv(AccionesTpv a){
			    accion = a;
		}
		public EventTpv(AccionesTpv a,object d){
			    accion = a;
			    dato = d;
		}
	}
	
	
	
	public partial class Tpv : Gtk.Window
	{
		decimal totalFactura = 0;
        
		public bool esMesaUnica {
		    set{
				 this.btnCamareros.Sensitive = !value;
				 this.btnMesas.Sensitive = !value;
				 this.btnHerrDesglose.Sensitive = !value;
			}
		}
		
		public decimal TotalFactura {
			get {
				return this.totalFactura;
			}
		}

        string cantidad = "";
        int tarifa = 1;
		
		public decimal Cantidad{
		   get{
			   return cantidad.Length > 0 ? Convert.ToDecimal(cantidad) : 1;	
			}
		}
		
		public string NombreCamarero{
		     set{
				this.lblNomCamarero.Markup = "<span size='x-large'>"+value+"</span>"	;
			}
		}
		public string NombreMesa{
		    set{
			   this.lblNomMesas.Markup = "<span size='x-large'>"+value+"</span>";	
			}
		}
		
		public int Tarifa{
		   set { 
				lblTarifa.LabelProp = "<span size='x-large' foreground=\"red\">T. "+value+"</span>";	
				tarifa = value;
			}
		}
       
		public int NumLinesaNotas{
			get{ return lstNotas.Model.IterNChildren();}
		}
		
		PaginasObj<Articulo> pagsArt;
		System.Timers.Timer tiempo;
	    
		public void PararTiempo(bool parar){
		   if((parar)	&& (tiempo!=null)) tiempo.Stop();
			else if (tiempo != null) tiempo.Start();
		}
		
		bool pulsadoReciente = false;
		public event EventHandler<EventTpv> eventTpv ;
		
		
		public PaginasObj<Articulo> PagsArt {
			get {
				return this.pagsArt;
			}
			set {
			     this.MostrarTeclado(value);
			}
		}

		public void ResetNotas(){
			//actualizar apariencia de tpv
                ((ListStore)lstNotas.Model).Clear();
                tarifa = 1;
                lblTarifa.LabelProp = "<span size='x-large' foreground=\"red\">T. "+tarifa+"</span>";
                this.lblDisplayInf.Texto = "CAJA";
			    this.lblDisplay.Texto = string.Format("{0:c}",totalFactura);
                totalFactura = 0;
		}   
		
        Articulo[] pagVisible;
		
		public void BorrarLineasNotas ()
	    {
	    	((ListStore)lstNotas.Model).Clear();
			this.lblDisplayInf.Texto = "CAJA";
			this.lblDisplay.Texto = string.Format("{0:c}",totalFactura);
            totalFactura = 0;
	    }
		
		public void VolverTpv(){
		    pulsadoReciente = true;
			if(tiempo!=null) tiempo.Start();
		}
		
		public void AgregarLinea(Articulo art){
		    	this.pulsadoReciente = true;
			    art.Cantidad = Cantidad;
	    	    ListStore ls = (ListStore)lstNotas.Model;
		        Gtk.TreeIter item = ls.AppendValues(String.Format("{0:#0.###}",art.Cantidad),
                art.Descripcion, String.Format("{0:c}", art.Precio),String.Format("{0:c}", art.TotalLinea),art);
                totalFactura += art.TotalLinea;
                lblDisplayInf.Texto = "Total";
                lblDisplay.Texto = String.Format("{0:c}", totalFactura);
                cantidad = "";
               if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.llenarLinea,art));	
			   if(tiempo!=null) tiempo.Start();
			   lstNotas.SetCursor(ls.GetPath(item),lstNotas.Columns[0],false);
		}
		
		public void MostrarLinea(Articulo art){
			    this.pulsadoReciente = true;
			    ListStore ls = (ListStore)lstNotas.Model;
			    Gtk.TreeIter item = ls.AppendValues(String.Format("{0:#0.###}",art.Cantidad),
                art.Descripcion, String.Format("{0:c}", art.Precio),String.Format("{0:c}", art.TotalLinea),art);
                totalFactura += art.TotalLinea;
                lblDisplayInf.Texto = "Total";
                lblDisplay.Texto = String.Format("{0:c}", totalFactura);
                cantidad = "";
              lstNotas.SetCursor(ls.GetPath(item),lstNotas.Columns[0],false);
		}
		
		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			if(tiempo!=null)tiempo.Stop();
		    if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.salir));	
		}
		
		
		public Tpv (bool temporizado, int tmp) : base(Gtk.WindowType.Toplevel)
		{
			
			if(temporizado){
			    tiempo = new System.Timers.Timer(tmp);	
				tiempo.Elapsed+= delegate(object sender, ElapsedEventArgs e) {
					 if(!pulsadoReciente){
					   if(eventTpv!=null) this.eventTpv(this,new EventTpv(AccionesTpv.tiempoEspirado));
					   tiempo.Stop();
					}else 
						pulsadoReciente = false;
						                  
				};
			}
			
			this.Build ();
			#region propiedades auxiliares controles
			 this.Hide();
			 this.IniciarBotonesNumeros();
			 this.CrearBotonesArt();
			 lstNotas.EnableGridLines = TreeViewGridLines.Both ; 
			 lblDisplay.ColorDeFono = System.Drawing.Color.Black;
			 lblDisplay.Font = new System.Drawing.Font(lblDisplay.Font.FontFamily,25f);
			 lblDisplay.ColorLetras = System.Drawing.Color.Red;
		     lblDisplay.AlienamientoH = System.Drawing.StringAlignment.Far;
			 
			 lblDisplayInf.Font = new System.Drawing.Font(lblDisplay.Font.FontFamily,25f);
			 lblDisplayInf.ColorDeFono = System.Drawing.Color.Black;
			 lblDisplayInf.ColorLetras = System.Drawing.Color.Red;
			 
			 CellRendererText cell = new CellRendererText();
			 cell.FontDesc = Pango.FontDescription.FromString("monospace 12");
			 cell.SetFixedSize(30,50); 
			 lstNotas.AppendColumn("Can",cell,"text",0);
			 cell = new CellRendererText();
			 cell.FontDesc = Pango.FontDescription.FromString("monospace 12");
			 cell.SetFixedSize(200,50); 
			 lstNotas.AppendColumn("Descripcion",cell,"text",1);
			 cell = new CellRendererText();
			 cell.FontDesc = Pango.FontDescription.FromString("monospace 12");
			 cell.SetFixedSize(70,50); 
			 lstNotas.AppendColumn("Precio",cell,"text",2);
			 cell = new CellRendererText();
			 cell.FontDesc = Pango.FontDescription.FromString("monospace 12");
			 cell.SetFixedSize(50,50); 
			 lstNotas.AppendColumn("Total",cell,"text",3);
             lstNotas.Model = new ListStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(Articulo));
			 lstNotas.CursorChanged+= delegate {
				  pulsadoReciente = true;
			}; 
			
			this.scrollNotas.wScroll = this.scrollNotasGtk;
			this.scrollNotas.moviendoCursor += delegate{
				pulsadoReciente = true;
			};
			
				
			#endregion
			
		}
		
		public void mostrarVariosConNombre(bool mostrar){
			this.btnVariosConNombre.Sensitive = mostrar;
		}
		
		public void mostrarVarios (bool mostrar)
		{
			this.btnVarios.Sensitive = mostrar;
		}

		protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
		{
		  if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.salir));	
       	}
		
        public void MostrarMesa(Articulo[] articulos){
		    this.BorrarLineasNotas();
			 foreach (Articulo art in articulos){
			    this.MostrarLinea(art);	
			}	
		}
		
		public void MostrarMesa(Articulo[] articulos,string nomMesa, string nomCamarero, int tarifa){
			if(tiempo!=null) tiempo.Start();
			 this. Tarifa =  tarifa;
			 this.NombreMesa = nomMesa;
		     this.NombreCamarero = nomCamarero;
		     this.BorrarLineasNotas();
			 foreach (Articulo art in articulos){
			    this.MostrarLinea(art);	
			}
			
		}
		
		void CrearBotonesArt(){
			 for(uint i = 4;i>0;i--){
                for(uint j= 8;j>0;j--){
                  this.pneArticulos.Attach(new MiBoton(this.OnBtnArticulos),j-1,j,i-1,i);   
                }
		    }
		}
		
		
		void IniciarBotonesNumeros(){
			
		 this.btnC.Texto = "C"; this.btnC.Font = new System.Drawing.Font("Arial",30f);
		 this.btnC.Datos = "C";
		  this.btnUno.Texto = "1";
			this.btnUno.Datos = "1";
			 this.btnUno.Font = new System.Drawing.Font("Arial",30f);
		 this.btnDos .Texto = "2";
			this.btnDos.Datos = "2";
			 this.btnDos.Font = new System.Drawing.Font("Arial",30f);
		 this.btnTres.Texto = "3";
			this.btnTres.Datos = "3";
			 this.btnTres.Font = new System.Drawing.Font("Arial",30f);
			
		  this.btn4.Texto = "4";
			this.btn4.Datos = "4";
			 this.btn4.Font = new System.Drawing.Font("Arial",30f);
		  this.btn5.Texto = "5";
			this.btn5.Datos = "5";
			 this.btn5.Font = new System.Drawing.Font("Arial",30f);
		  this.btn6.Texto = "6";
			this.btn6.Datos = "6";
			 this.btn6.Font = new System.Drawing.Font("Arial",30f);
		  this.btn7.Texto = "7";
		    this.btn7.Datos = "7";
			 this.btn7.Font = new System.Drawing.Font("Arial",30f);
		   this.btn8.Texto = "8";
			this.btn8.Datos = "8";
		    this.btn8.Font = new System.Drawing.Font("Arial",30f);	
		  this.btn9.Texto = "9";
		     this.btn9.Datos = "9";
			 this.btn9.Font = new System.Drawing.Font("Arial",30f);
		  this.btnCero.Texto = "0";
		    this.btnCero.Datos = "0";
		     this.btnCero.Font = new System.Drawing.Font("Arial",30f);
			
		}
		
		public void MostrarTeclado(PaginasObj<Articulo> pagArt){
	       this.pagsArt = pagArt;
		   pagVisible =  pagsArt.Primera;
		   btnPgAbajo.Sensitive  = this.btnPgArriba.Sensitive = this.pagsArt.NumPaginas > 1;
		   mostrarVistaTelcado();
		}
		
		
		//Gestiona la seleccion de una seccion
        void mostrarVistaTelcado()
        {
            if(pagVisible!=null){
				int pos = 0;
		        foreach (Gtk.Widget tecla in this.pneArticulos.Children){
	              if(tecla is Valle.GtkUtilidades.MiBoton){
	                  MiBoton estaTecla = ((MiBoton)tecla);
	                 if(pos < pagVisible.Length){
	                   estaTecla.Datos = pagVisible[pos];
	                   estaTecla.Visible = true;
	                   estaTecla.ColorDeFono = pagVisible[pos].MiColor;
	                   estaTecla.Texto = pagVisible[pos].NombreCorto;
					   
	                  }else{
	                   estaTecla.Datos = null;
	                   estaTecla.Visible = false;
					  }
	                  pos++;
	                }
	             }
	           }
	       }
		
        protected virtual void OnBtnAbrirCajonClicked (object sender, System.EventArgs e)
		{
			 
			 if(eventTpv!=null){
				eventTpv(this, new EventTpv(AccionesTpv.abriCajon));
			}
		}
		
        protected virtual void OnBtnBorrarClicked (object sender, System.EventArgs e)
		{
			this.pulsadoReciente= true;
			TreeIter iterSel;
			if(lstNotas.Selection.GetSelected(out iterSel)){
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.BorrarLinea,lstNotas.Model.GetValue(iterSel,4)));
				totalFactura-= ((Articulo)lstNotas.Model.GetValue(iterSel,4)).TotalLinea;
				lblDisplay.Texto = String.Format("{0:c}", totalFactura);
				(( ListStore)lstNotas.Model).Remove(ref iterSel);
			}
		}
		
        protected virtual void OnBtnCajaClicked (object sender, System.EventArgs e)
		{
			Hashtable nota = new Hashtable();
			IEnumerator arts =  ((ListStore)lstNotas.Model).GetEnumerator(); 
            while(arts.MoveNext())			
			{
				Articulo articuloA =(Articulo)((object[])arts.Current)[4];
                    if (nota.Contains(articuloA.IDArticulo.ToString()+articuloA.precio.ToString()+articuloA.Descripcion))
                    {
                        Articulo articuloB = (Articulo)nota[articuloA.IDArticulo.ToString()+articuloA.Precio.ToString()+articuloA.Descripcion];
                        articuloB.TotalLinea += articuloA.TotalLinea;
                        articuloB.Cantidad += articuloA.Cantidad;
                    }
                    else
                    {
                        nota.Add(articuloA.IDArticulo.ToString()+articuloA.Precio.ToString()+articuloA.Descripcion, articuloA.Clone());
                    }
				
              }
			
			if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cobrar, nota));
			
		}
		
        protected virtual void OnBtnHerrDesgloseClicked (object sender, System.EventArgs e)
		{
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.herramietas));	
		}
		
        protected virtual void OnBtnVariosClicked (object sender, System.EventArgs e)
		{ 
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.varios));	
		}
		
        protected virtual void OnBtnVariosConNombreClicked (object sender, System.EventArgs e)
		{
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.variosnombre));	
		}
		
        protected virtual void OnBtnPgAbajoClicked (object sender, System.EventArgs e)
		{ 
			   //para que oculte la informacion del ticket
		     if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cerrarInfTicket));	
		 
			pagVisible = pagsArt.Sigiente;
			this.mostrarVistaTelcado();
		}
		
        protected virtual void OnBtnPgArribaClicked (object sender, System.EventArgs e)
		{
			  //para que oculte la informacion del ticket
		      if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cerrarInfTicket));	
		 
			pagVisible = pagsArt.Atras;
			this.mostrarVistaTelcado();
		}
		
        protected virtual void OnBtnBuscadorClicked (object sender, System.EventArgs e)
		{
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.buscador));
		}
		
        protected virtual void OnBtnImprimirClicked (object sender, System.EventArgs e)
		{
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.imprimir));
		}
		
        protected virtual void OnBtnFavoritosClicked (object sender, System.EventArgs e)
		{
			  if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.favoritos));
		}
	
        protected virtual void OnBtnBorrNumClicked (object sender, System.EventArgs e)
		{
			if(cantidad.Length>0){
				cantidad = cantidad.Remove(cantidad.Length-1,1);
				this.lblDisplay.Texto=cantidad;
			}else{
				this.lblDisplayInf.Texto = "CAJA";
			    this.lblDisplay.Texto = string.Format("{0:c}",this.totalFactura);
			}
		
			//para que oculte la informacion del ticket
		    if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cerrarInfTicket));	
		 	
            pulsadoReciente= true;
		}
		
        protected virtual void OnBtnCamarerosClicked (object sender, System.EventArgs e)
		{
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.camareros));
		}
		
        protected virtual void OnBtnSeccionesClicked (object sender, System.EventArgs e)
		{
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.secciones));
		}
		
        protected virtual void OnBtnOpcionesClicked (object sender, System.EventArgs e)
		{
			 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.opciones));
		}
		
        protected virtual void OnBtnTarifaClicked (object sender, System.EventArgs e)
		{
			pulsadoReciente = true;
		    if(tarifa<3) tarifa++; else tarifa = 1;
			lblTarifa.LabelProp = "<span size='x-large' foreground=\"red\">T. "+tarifa+"</span>";
		 	  //para que oculte la informacion del ticket
		     if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cerrarInfTicket));	
		 
		}
		
        protected virtual void OnBtnMesasClicked (object sender, System.EventArgs e)
		{ 
			  if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.mesas));
		}
		
        protected virtual void OnBtnNumerico (Valle.GtkUtilidades.MiBoton.AccionesTecla accion, object dato)
		{
			if(accion==Valle.GtkUtilidades.MiBoton.AccionesTecla.Clickado){
				string tecla = ((MiBoton)dato).Texto;
			
			  if(!(tecla == "0" && cantidad.Length<=0)){
					if (tecla=="C"){
						lblDisplayInf.Texto="Total";
						lblDisplay.Texto= String.Format("{0:c}",totalFactura);
						cantidad="";
					}
					else{
						cantidad+=tecla;
						lblDisplay.Texto= cantidad;
						this.lblDisplayInf.Texto= "Nº Art";
					}
				}
				
	             pulsadoReciente = true ;  
	        }
			
		   //para que oculte la informacion del ticket
		 if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cerrarInfTicket));	
		 
				
		}
		
		
     protected virtual void OnBtnArticulos (Valle.GtkUtilidades.MiBoton.AccionesTecla accion, object dato)
	 {
		    if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cerrarInfTicket));	
		 
            Articulo articulo = (Articulo)((MiBoton)dato).Datos;
                articulo.Tarifa = tarifa;
                articulo.Cantidad = cantidad.Length>0 ? Int32.Parse(cantidad) : 1;

                if (cantidad.Length > 0) { articulo.Cantidad = Decimal.Parse(cantidad); }
                if (articulo.VentaPorKilos)
                {
           		    if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.ventaPorKilos));	
                }else
				   AgregarLinea(articulo.Clone());
    	}
		
	
		
        protected virtual void OnLstNotasSelectCursorRow (object o, Gtk.SelectCursorRowArgs args)
		{
			  if(eventTpv!=null) eventTpv(this,new EventTpv(AccionesTpv.cerrarInfTicket));	
		 
		     this.btnBorrar.Sensitive = lstNotas.Selection.CountSelectedRows()>0;
		}
		
	
		
		
	}
}

