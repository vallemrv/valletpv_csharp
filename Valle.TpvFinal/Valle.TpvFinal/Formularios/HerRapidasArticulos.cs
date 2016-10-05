using System;
using System.Collections;
using System.Collections.Generic;
using Valle.ToolsTpv;

namespace Valle.TpvFinal
{
	public enum AccionesSepararArticulos { Salir, Caja, llenar }
    public enum TipoControlHerrmientaRapida { Separar, llenadoIden, Invitacion}
    public delegate void OnAccionesSepararTicket(AccionesSepararArticulos accion, List<Articulo> articulos, decimal totalDesglose);
  
	public partial class HerRapidasArticulos : Valle.GtkUtilidades.FormularioBase
	{
		public HerRapidasArticulos (TipoControlHerrmientaRapida tipo)
		{
			this.Init ();
			#region Construccion de la apariencia
			this.WindowPosition = Gtk.WindowPosition.CenterAlways;
			this.KeepAbove = true; this.Modal = true;
			this.scrollTacticlOrg.wScroll = this.scrollOrg;
			this.scroltactilSel.wScroll = this.scrollSel;
			
			this.LblTituloBase = this.lblTitulo;
			this.lblDisplay.Font = new System.Drawing.Font(this.lblDisplay.Font.FontFamily,30);
			this.lblDisplay.ColorDeFono = System.Drawing.Color.Black;
			this.lblDisplay.ColorLetras = System.Drawing.Color.Red;
			this.lblDisplay.AlienamientoH = System.Drawing.StringAlignment.Far;
			this.lblT.Font = new System.Drawing.Font(this.lblDisplay.Font.FontFamily,30);
			this.lblT.ColorDeFono = System.Drawing.Color.Black;
			this.lblT.ColorLetras = System.Drawing.Color.Red;
			this.lblT.AlienamientoH = System.Drawing.StringAlignment.Near;
			this.listSeleccionados.Model = new Gtk.ListStore(typeof(string), typeof(string), typeof(LineaListado));
			this.lstOriginal.Model =new Gtk.ListStore(typeof(string), typeof(string), typeof(LineaListado));
			Gtk.CellRendererText cell = new Gtk.CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 15");
			cell.SetFixedSize(200,50);
			this.listSeleccionados.AppendColumn("Descripcion   ", cell, "text", 0);
			cell = new Gtk.CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 15");
			cell.SetFixedSize(50,50);
			this.listSeleccionados.AppendColumn("Precio   ", cell, "text", 1);
			cell = new Gtk.CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 15");
			cell.SetFixedSize(200,50);
			Gtk.TreeViewColumn colum = new Gtk.TreeViewColumn("Descripcion   ", cell, "text", 0);
			this.lstOriginal.AppendColumn(colum);
			cell = new Gtk.CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 15");
			cell.SetFixedSize(50,50);
			colum = new Gtk.TreeViewColumn("Precio   ", cell, "text", 1);
			this.lstOriginal.AppendColumn(colum);
			this.lblInfOriginal.ColorDeFono = System.Drawing.Color.Aqua;
			this.lblInfSelcecionados.ColorDeFono= System.Drawing.Color.Aqua;
			this.lblInfOriginal.AlienamientoH = System.Drawing.StringAlignment.Center;
			this.lblInfSelcecionados.AlienamientoH = System.Drawing.StringAlignment.Center;
			#endregion
			
			
			switch (tipo)
            {
                case TipoControlHerrmientaRapida.Separar:
                    Titulo = "Separarar articulos del ticket";
                    btnCaja.Visible = true; this.btnAceptar.Visible = false;
                    break;
                case TipoControlHerrmientaRapida.llenadoIden:
                    Titulo = "Llenar iden";
                    btnAceptar.Visible = true; btnCaja.Visible = false;
                    break;
                case TipoControlHerrmientaRapida.Invitacion:
                    Titulo = "Elegir rondas invitadas";
                    btnCaja.Visible = false; this.btnAceptar.Visible = true;
                    break;
            }
			
		}
    	
        public event OnAccionesSepararTicket EjAccion;
        public AccionesSepararArticulos accion;
        int idRonda =0;
        decimal totalDesglose = 0;
        List<LineaListado> listaAMostrar = new List<LineaListado>();
		List<Articulo> rondasSelecionadas = new List<Articulo>();
		Dictionary<int,LineaRonda> listaRondas = new Dictionary<int, LineaRonda>();
     
        public decimal TotalDesglose
        {
            get { return totalDesglose; }
            set { 
				totalDesglose = value;
                lblDisplay.Texto = String.Format("{0:c}", totalDesglose);
            }
        }
   
		
        public List<Ronda> Rondas
        {
            set {
              
				int posItem = 0;
				   
				List<Ronda> rondas = value;
             	
				((Gtk.ListStore)listSeleccionados.Model).Clear();
				listaAMostrar.Clear();
				rondasSelecionadas.Clear();
				listaRondas.Clear();
				
                idRonda = 0;
                TotalDesglose = 0;
                
				foreach (Ronda r in rondas)
                {
					LineaRonda lr = new LineaRonda(posItem++,"Ronda num  "+(idRonda+1).ToString(),idRonda);
					listaAMostrar.Add(lr);
					listaRondas.Add(idRonda, lr);
					
					
				     IEnumerator rondActivas = r.lineasArtActivos.Values.GetEnumerator();
                       while (rondActivas.MoveNext())
                        {
                            foreach (Articulo art in estraerArticulos((Articulo)rondActivas.Current))
                            {
							    LineaArticulo lArt = new LineaArticulo(posItem++,art,idRonda);
						     	listaAMostrar.Add( lArt);
							    lr.AgregarLinea(lArt); 
						    }
				       }
					idRonda++;
					
		        }
				this.RellenarListaOrg();
            }
        }
		
		void RellenarListaOrg(){
			  
			    ((Gtk.ListStore)lstOriginal.Model).Clear();
			  
			    
			   foreach (LineaListado l in listaAMostrar)
               {
				  if(l.visible){
				    if (l.Tipo == LineaListado.TIPO_RONDA)
                       ((Gtk.ListStore)lstOriginal.Model).AppendValues(l.Descripcion,"",l);
                          else  
					            	((Gtk.ListStore)lstOriginal.Model).AppendValues(l.Descripcion,
							                String.Format("{0:###0.00}",((LineaArticulo)l).art.TotalLinea),l);
				    }
                }
		}

        private List<Articulo> estraerArticulos(Articulo a)
        {
            List<Articulo> articulos = new List<Articulo>();
            if (!a.VentaPorKilos)
            {
                Articulo aux;
                for (int i = 0; i < Int32.Parse(a.Cantidad.ToString()); i++)
                {
                     aux = a.Clone();
                     aux.Cantidad = 1; aux.TotalLinea = aux.precio;
                     articulos.Add(aux);
                }
            }
            else
            {
                articulos.Add(a.Clone());
            }

            return articulos;
        }
        
		

//        private int NumArticulosRonda(Ronda rond)
//        {
//            int numlineas = 0;
//            IEnumerator rondActivas = rond.lineasArtActivos.Values.GetEnumerator();
//            while (rondActivas.MoveNext())
//            {
//                numlineas += Int32.Parse(((Articulo)rondActivas.Current).Cantidad.ToString());
//            }
//            return numlineas;
//        }

        

        private void btnCaja_Click(object sender, EventArgs e)
        {
            if (this.rondasSelecionadas.Count > 0)
            {
                PulsadoRecientemente = true;
                accion = AccionesSepararArticulos.Caja;
                if (SalirAlPulsar) { base.btnSalir_Click(sender, e); }
                if (EjAccion != null) { EjAccion(accion, this.rondasSelecionadas, totalDesglose); }
            }
        }

        private void listTicketOriginal_SelectedIndexChanged(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
			Gtk.TreeIter itmeSel;
            if (this.lstOriginal.Selection.GetSelected(out itmeSel))
            {
				LineaListado l = (LineaListado)this.lstOriginal.Model.GetValue(itmeSel,2);
				
                if (l.Tipo == LineaListado.TIPO_ARTICULO) 
                {
	                   LineaArticulo la = (LineaArticulo)l;				
					   Articulo art =  la.art;
					
                        ((Gtk.ListStore)listSeleccionados.Model).AppendValues(art.Descripcion,
						                         String.Format("{0:###0.00}", art.TotalLinea), l);
					
                        this.TotalDesglose += art.TotalLinea;
                        this.rondasSelecionadas.Add(art);
					   
					    ((Gtk.ListStore)this.lstOriginal.Model).Remove(ref itmeSel);
					    listaRondas[l.idRonda].BorrarLinea(la);
					    
					if(listaRondas[l.idRonda].estaVacia)
						               this.RellenarListaOrg();
			    }
                else
                {
						
				    foreach (LineaArticulo la in (l as LineaRonda).LineasPorRonda)
                    {
					   
						Articulo art =  la.art;
						
                        ((Gtk.ListStore)listSeleccionados.Model).AppendValues(art.Descripcion,
						                         String.Format("{0:###0.00}", art.TotalLinea), la);
						
                        this.TotalDesglose += art.TotalLinea;
                        this.rondasSelecionadas.Add(art);
						(l as LineaRonda).BorrarLinea(la);
						
					  }
					
					this.RellenarListaOrg();
					
			    }
     	    }
			
	    }

      
		protected override void btnSalir_Click(object sender, EventArgs e)
        {
            accion = AccionesSepararArticulos.Salir;
            if(EjAccion!=null){EjAccion(accion,null,0);}
			this.CerrarFormulario();
        }

        private void btnAcepar_Click(object sender, EventArgs e)
        {
            if (this.rondasSelecionadas.Count > 0)
            {
                PulsadoRecientemente = true;
                accion = AccionesSepararArticulos.llenar;
                if (SalirAlPulsar) { this.CerrarFormulario(); }
                if (EjAccion != null) { EjAccion(accion, this.rondasSelecionadas, totalDesglose); }
            }
        }

               
        
       
        
       
        
		bool primeravez = true;
		protected override void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			if(primeravez){
			   this.lblT.Texto = "TOTAL";
			   this.lblInfOriginal.Texto = "Ticket original";
				this.lblInfSelcecionados.Texto = "Lineas Seleccionadas";
				lblDisplay.Texto = String.Format("{0:c}", totalDesglose);
				primeravez = false;
			}
			base.OnExposeEvent(o,args);
		}
		
		
		protected virtual void OnListSeleccionadosRowActivated (object o, System.EventArgs args)
		{
			PulsadoRecientemente = true;
			Gtk.TreeIter item;
			
			if(this.listSeleccionados.Selection.GetSelected(out item)){
				   LineaArticulo l = (LineaArticulo)listSeleccionados.Model.GetValue(item,2);
				   Articulo art = l.art;
				
				  ((Gtk.ListStore)this.listSeleccionados.Model).Remove(ref item);
                    this.TotalDesglose -= art.TotalLinea;
                    this.rondasSelecionadas.Remove(art);
				    this.listaRondas[l.idRonda].AgregarLinea(l);
				    
				
				this.RellenarListaOrg();
				
            }
		}
		
	 
		
    }
	
	
	
	
    
    class LineaListado
    {
		public static int TIPO_ARTICULO = 200;
		public static int TIPO_RONDA = 100;
		
		public int Tipo = LineaListado.TIPO_ARTICULO;
		public int pos;
		public string Descripcion = "";
		public int idRonda;
		public bool visible = true;
		
		public LineaListado(int tipo){
			this.Tipo = tipo;
		}
     }
	
	class LineaArticulo : LineaListado{
	   	public Articulo art = null;
		
		public LineaArticulo(int pos, Articulo art, int id):
		base(LineaListado.TIPO_ARTICULO){
			this.art = art; this.pos = pos;
			this.idRonda = id;
			this.Descripcion = this.art.Descripcion;
		}
		
		
		
	}
	
	class LineaRonda : LineaListado{
	    List<LineaArticulo> lineasPorRonda = new List<LineaArticulo>();
		
		public LineaRonda(int pos, string lRonda,int id):
		base(LineaListado.TIPO_RONDA){
		   this.Descripcion = lRonda; this.pos = pos;
			this.idRonda = id;
		}
		
		public LineaArticulo[] LineasPorRonda{
		    get{
				return lineasPorRonda.ToArray();
			}
		}
		
		public void Vaciar(){
		   this.lineasPorRonda.Clear();
		   this.visible = false;
		}
		
		public bool estaVacia{
					get {
							return lineasPorRonda.Count == 0;
						}
					}
		
		public void BorrarLinea(LineaArticulo l){
			lineasPorRonda.Remove(l);
			l.visible = false;
			 if(lineasPorRonda.Count == 0)
				   this.visible = false;
		}
		
		public void AgregarLinea(LineaArticulo l){
		   lineasPorRonda.Add (l);
	        l.visible = true;
			this.visible = true;
		}
		
	}

}

