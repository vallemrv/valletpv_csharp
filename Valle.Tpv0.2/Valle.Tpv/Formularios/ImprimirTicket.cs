using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using Gtk;
using Valle.GtkUtilidades;
using Valle.ToolsTpv;
using Valle.SqlGestion;


namespace Valle.TpvFinal
{
	public partial class ImprimirTicket : FormularioBase
	{
		public event EventHandler salirImprimirTicket;
		
		string rutImp;
	    string nomImp;
		DataTable tbFacturas;
        DataTable tbLineas;
        int numTicketMostrar = 20;
        int numTicekt;
        DateTime fechaCobro;
        String camarero;
        String mesa;
        IGesSql gesBase;
        int idTpv;
        int modo = 0;
		
        delegate void FuncSinArg();
		
        ArrayList nota = new ArrayList();
        string SELECT_TOP_LIMIT = "SELECT   @TOP @TTicket Ticket.NumTicket,Ticket.Cambio, Ticket.FechaCobrado, Ticket.HoraCobrado, Ticket.Camarero, Ticket.Mesa, Lineas.Importe " +
                                    "FROM Ticket INNER JOIN " +
                                    "(SELECT  numTicket, SUM(TotalLinea) AS Importe " +
                                    "FROM LineasTicket " +
                                    "GROUP BY numTicket) AS Lineas ON Ticket.NumTicket = Lineas.numTicket " +
                                    "WHERE     (Ticket.IDTpv = @IDTpv )" +
                                    "ORDER BY Ticket.NumTicket DESC @LIMIT @LTicket ";
       		
		public ImprimirTicket (IGesSql ges, int id, string rut, string nomImp)
		{
			this.Init ();
			this.rutImp = rut;
			this.nomImp= nomImp;
			this.WidthRequest=800;
			this.HeightRequest=700;
			this.WindowPosition = WindowPosition.CenterAlways;
			this.LblTituloBase = this.lblTitulo;
			this.scroolOriginal.wScroll = this.GtkScrolledWindow;
			this.scrollSeleccionados.wScroll = this.GtkScrolledWindow1;
			
			gesBase = ges;
            idTpv = id;
            //para la consultas de los diez primeros ticket segun base de datos
			if(ges is GesMySQL){
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@TOP @TTicket","");
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@LIMIT @LTicket","LIMIT @NumTicket");
            }else{
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@LIMIT @LTicket","");
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@TOP @TTicket","TOP @NumTicket");
            }
			ListStore ls = new ListStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(DataRow));
			CellRendererText cell = new CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 10");
			cell.SetFixedSize(30,50); 
			lstListadoTicket.AppendColumn("Num",cell,"text",0);
			cell = new CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 10");
			cell.SetFixedSize(150,50); 
			lstListadoTicket.AppendColumn("Fecha",cell,"text",1);
			cell = new CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 10");
			cell.SetFixedSize(50,50); 
			lstListadoTicket.AppendColumn("Mesa",cell,"text",2);
			cell = new CellRendererText();
			cell.FontDesc = Pango.FontDescription.FromString("monospace 10");
			cell.SetFixedSize(50,50); 
			lstListadoTicket.AppendColumn("Importe",cell,"text",3);
				lstListadoTicket.Model = ls;
			this.btnTicket.Visible = false;
		}
		
		private void RealizarConsulta(){
			Gtk.Application.Invoke(delegate{
			   Titulo = "Imprimir ticket";
            this.barImformacion.Text = "Cargando facturas......";
            this.barImformacion.Fraction = 1/3;
			
				System.Threading.Thread.Sleep(50);
				
            string consulta = SELECT_TOP_LIMIT.Replace("@IDTpv",idTpv.ToString());
             consulta = consulta.Replace("@NumTicket",numTicketMostrar.ToString());
             
			tbFacturas = gesBase.EjecutarSqlSelect("ImpTicket",consulta);
			if(tbFacturas.Rows.Count>0)	
             tbLineas = gesBase.EjecutarSqlSelect("ImpTicketLienas",
               "SELECT *, TotalLinea/Cantidad AS Precio FROM LineasTicket WHERE NumTicket >= "+tbFacturas.Rows[tbFacturas.Rows.Count-1]["NumTicket"].ToString() +
               " AND NumTicket <= "+tbFacturas.Rows[0]["NumTicket"].ToString());
			   
				this.barImformacion.Fraction = 2/3;
            	System.Threading.Thread.Sleep(50);
				
                 rellenarTablaFacturas();
			    
				
				this.barImformacion.Fraction = 3/3;
				System.Threading.Thread.Sleep(50);
            
            if(esTemporizado) this.TemporizadorDeCierre.Start();
            this.barImformacion.Visible=false;
            this.pnePrimcipal.Sensitive = true;
			this.lblInfOriginal.Texto = "Listado de los "+numTicketMostrar.ToString()+" ticket";
				
			});
		}
		
		private void EstablecerFacturas()
        {
			if(this.esTemporizado)
				   this.TemporizadorDeCierre.Stop();
            ((ListStore)lstListadoTicket.Model).Clear();
			this.barImformacion.Visible = true;
			this.pnePrimcipal.Sensitive = false;
           
			 new System.Threading.Thread(new System.Threading.ThreadStart(this.RealizarConsulta)).Start();
			
        }
        
        private void rellenarTablaFacturas()
        {
            for (int i = 0; i < tbFacturas.Rows.Count; i++)
            {
                ((ListStore)lstListadoTicket.Model).AppendValues(Convert.ToString((int)tbFacturas.Rows[i]["NumTicket"]),
                Valle.Utilidades.CadenasTexto.RotarFecha(tbFacturas.Rows[i]["FechaCobrado"].ToString())+" "+
                                tbFacturas.Rows[i]["HoraCobrado"].ToString(),
                                tbFacturas.Rows[i]["Mesa"].ToString(),
				                String.Format("{0:c}", (decimal)tbFacturas.Rows[i]["Importe"]),
                                tbFacturas.Rows[i]);
                
            }
        
		}
      
       

        

       protected override void btnSalir_Click(object sender, EventArgs e)
        {
            this.txtTicketInf.Buffer.Clear();
			if(salirImprimirTicket!=null) salirImprimirTicket(sender,e);
            this.CerrarFormulario();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
          if(this.txtTicketInf.Buffer.CharCount > 0)
		  {
                 if (SalirAlPulsar){
					Gtk.TreeIter item;
			            if (lstListadoTicket.Selection.GetSelected(out item))
			            {
						   System.Collections.Hashtable lineasHas = new System.Collections.Hashtable();
				
			                DataRow drFac = (DataRow)lstListadoTicket.Model.GetValue(item,4);
			
			                numTicekt = (int)drFac["NumTicket"];
			                fechaCobro = DateTime.Parse(drFac["FechaCobrado"].ToString()+" "+drFac["HoraCobrado"].ToString());
			                mesa = drFac["Mesa"].ToString();
			                camarero = drFac["Camarero"].ToString();
						    
			               if(modo == 0){
							 
							 DataView lineas = new DataView(tbLineas, "numTicket = " + numTicekt, "numTicket", DataViewRowState.CurrentRows);
			                  nota.Clear();
						 	
			                 foreach (DataRowView drLin in lineas)
			                  {
			                    Articulo articulo = new Articulo();
			                    articulo.Cantidad =Decimal.Parse(String.Format("{0:#0.###}",drLin["Cantidad"]));
			                    articulo.Descripcion = drLin["nomArticulo"].ToString();
			                    articulo.TotalLinea = (decimal)drLin["TotalLinea"];
			                    articulo.precio = (decimal)drLin["Precio"];
			                    lineasHas.Add(articulo.GetHashCode(),articulo);
							 }
							
								ImpresoraTicket.ImprimirTicket(rutImp, nomImp, lineasHas.GetEnumerator(),camarero,
									mesa,fechaCobro,numTicekt,(decimal)drFac["Importe"],(decimal)drFac["Cambio"]);
			                
				             }else if(modo == 1){
				                ImpresoraTicket.ImprimirInforme(rutImp,nomImp,this.txtTicketInf.Buffer.Text.Split('\n'),
								        "Informacion ticket");
				             }
					    }
					
					if(salirImprimirTicket!=null) salirImprimirTicket(sender,e);
					this.CerrarFormulario();
				}
          }
        }

        private void btnMasFacturas_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            numTicketMostrar += 10;
            this.EstablecerFacturas();
        }

        private void lstListadoTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            this.txtTicketInf.Buffer.Clear();
			Gtk.TreeIter item;
            if (lstListadoTicket.Selection.GetSelected(out item))
            {
				
                DataRow drFac = (DataRow)lstListadoTicket.Model.GetValue(item,4);
				
                numTicekt = (int)drFac["NumTicket"];
                fechaCobro = DateTime.Parse(drFac["FechaCobrado"].ToString()+" "+drFac["HoraCobrado"].ToString());
                mesa = drFac["Mesa"].ToString();
                camarero = drFac["Camarero"].ToString();
             if(modo == 0){
                DataView lineas = new DataView(tbLineas, "numTicket = " + numTicekt, "numTicket", DataViewRowState.CurrentRows);
                nota.Clear();
                foreach (DataRowView drLin in lineas)
                {
                    Articulo articulo = new Articulo();
                    articulo.Cantidad =Decimal.Parse(String.Format("{0:#0.###}",drLin["Cantidad"]));
                    articulo.Descripcion = drLin["nomArticulo"].ToString();
                    articulo.TotalLinea = (decimal)drLin["TotalLinea"];
                    articulo.precio = (decimal)drLin["Precio"];
                    nota.Add(articulo);
                }
                VisorDeInformes.MostrarTicket(this.txtTicketInf.Buffer, nota.GetEnumerator(),camarero,mesa,fechaCobro,numTicekt);
                
             }else if(modo == 1){
                VisorDeInformes.MostrarInfTicket(this.txtTicketInf.Buffer, numTicekt,gesBase,camarero);
             }
				
              this.lblInfSelcecionados.Texto = this.txtTicketInf.Buffer.CharCount > 0 ? String.Format("Desglose de Ticket {0}", numTicekt) : "Desglose de Ticket";
             }
			
            }
         
      
         
        void BtnEstadisticaClick(object sender, EventArgs e)
        {
           PulsadoRecientemente= true; 
        	 modo = 1; 
             this.lstListadoTicket_SelectedIndexChanged(null, null);
			 this.btnInfTicket.Visible = false;
			 this.btnTicket
					.Visible = true;
           
        }
        
        void btnTicketClick(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
        	 modo = 0; 
             this.lstListadoTicket_SelectedIndexChanged(null, null);
			 this.btnInfTicket.Visible = true;
			 this.btnTicket
					.Visible = false;
           
        }
		
		
        protected override void OnShown ()
		{
			this.EstablecerFacturas();
			base.OnShown ();
		}
		
	}
}

