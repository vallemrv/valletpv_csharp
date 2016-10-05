using Gtk;
using System;
using System.Data;
using System.Collections.Generic;

using Valle.GtkUtilidades;
using Valle.SqlGestion;
using Valle.ToolsTpv;


namespace Valle.TpvFinal
{
	
	
	public partial class ListadoCierres : FormularioBase
	{
		string nomImp;
		string rutFicherosImp;
		
		public ListadoCierres (IGesSql ges, int id, string nomImp, string rutFicherosImp)
		{
			
			this.Init (); 
			
			
			
			this.nomImp = nomImp;
			this.rutFicherosImp = rutFicherosImp;
			
			this.LblTituloBase = this.lblTitulo;
			if(ges is GesMSSQL){
            	SELECT_TOP_LIMIT =	SELECT_TOP_LIMIT.Replace("@TOP","TOP @MAXIMO");
            	SELECT_TOP_LIMIT =	SELECT_TOP_LIMIT.Replace("@LIMIT","");
            }else{
            	SELECT_TOP_LIMIT =	SELECT_TOP_LIMIT.Replace("@LIMIT","LIMIT @MAXIMO");
            	SELECT_TOP_LIMIT =	SELECT_TOP_LIMIT.Replace("@TOP","");
            }
		
			Titulo = "Listado de cierres de TPV";
            SalirAlPulsar = false;
			this.btnDesglose.Visible = false;
            gesBase = ges;
            idTpv = id;
            tbTpv = gesBase.ExtraerTabla("TPVs",null);
			
			Gtk.ListStore ls = new Gtk.ListStore(typeof(string),typeof(string),typeof(string),typeof(DataRow));
			
			 lstCierres.Model = ls;
			 CellRendererText cell = new CellRendererText();
			 cell.FontDesc = Pango.FontDescription.FromString("monospace 12");
			 cell.SetFixedSize(110,50); 
			 lstCierres.AppendColumn("Fecha",cell,"text",0);
			 cell = new CellRendererText();
			 cell.FontDesc = Pango.FontDescription.FromString("monospace 12");
			 cell.SetFixedSize(60,50); 
			 lstCierres.AppendColumn("Hora",cell,"text",1);
			 cell = new CellRendererText();
			 cell.FontDesc = Pango.FontDescription.FromString("monospace 12");
			 cell.SetFixedSize(80,50); 
			 lstCierres.AppendColumn("Nombre Tpv",cell,"text",2);
			  this.lblInfOriginal.Texto = "Lista de cierres de caja";
			
		}
		
		 delegate void MetodoSimuladorAcc(int tiempo);
		
		public event EventHandler salirListadoCierres;
		
        
        IGesSql gesBase;
        int idTpv;
        //Filtro filtro;
        DataTable tbTpv;
        DataTable tbCierres;
        DesgloseCierre desInv = null;
        int modo = 0;
		int maximo = 20;
        string SELECT_TOP_LIMIT = "Select @TOP * FROM CierreDeCaja WHERE IDTpv = @IDTpv ORDER BY fechaCierre DESC, horaCierre DESC @LIMIT";
        List<string> ListaVentaCamareros = new List<string>();
        List<string> ListaVentaHoras = new List<string>();

        
        void RellenarCierres(){
            
           Gtk.ListStore lStr = (Gtk.ListStore)lstCierres.Model;
		   lStr.Clear();
			
				foreach(DataRow r in tbCierres.Rows){
					lStr.AppendValues(Utilidades.CadenasTexto.RotarFecha(r["fechaCierre"].ToString()),
        		    r["horaCierre"].ToString(), this.tbTpv.Select("IDTpv = "+r["IDTpv"].ToString())[0]["Nombre"].ToString(),r);
        	}
        	
        }
		
      	
        /*void btnFiltros_Click(object sender, EventArgs e)
        {
          FiltrosCierres fil = new FiltrosCierres(filtro);
           fil.MiVista.MostrarFormulario(true, true, ModoSombra.control);
           
	
           string consultaTpv = "";
           if(fil.filtro.tpv.Tag != null)
                          consultaTpv = "IDTpv = "+ ((DataRow)fil.filtro.tpv.Tag)["IDTpv"].ToString()+"  AND ";
                          
           if((fil.filtro.FechaHasta!=null)&&(fil.filtro.FechaDesde !=null)){
               tbCierres = gesBase.EjecutarSqlSelect("MisCierres",
               "SELECT * FROM CierreDeCaja WHERE "+consultaTpv+" FechaCierre >= '"+Utilidades.CadenasTexto.RotarFecha(fil.filtro.FechaDesde.ToShortDateString())+
               "' AND FechaCierre <= '"+ Utilidades.CadenasTexto.RotarFecha(fil.filtro.FechaHasta.ToShortDateString())+"' ORDER BY FechaCierre DESC, horaCierre DESC");

               this.RellenarCierres();    
           }
        }*/
		
		
        void lstCierres_SelectedIndexChanged(object o, EventArgs args)
        {
		
			 if (lstCierres.Selection.CountSelectedRows() > 0){
             switch(modo){
	                case 0:
	                      new MetodosAsync(new MetodoSinArg(CalcularCierre)).GtkFuncAsync();     
	                break;
	                case 1:
	                    new MetodosAsync(new  MetodoSinArg(VentaPorCamareros)).GtkFuncAsync();     
	                break;
	                case 2:
	                    new MetodosAsync(new MetodoSinArg(VentaPorHoras)).GtkFuncAsync();     
	                break;
					}
				}else{
				     lblInfSelcecionados.Texto = "No hay cierres seleccionados";
				     ListaVentaHoras.Clear();
				     ListaVentaCamareros.Clear();
				     desInv = null;
				     ((Gtk.ListStore)lstCierres.Model).Clear();
				     txtTicketInf.Buffer.Clear();
				}
             
                
            
        }
		
        void VentaPorHoras(){
			       this.barImformacion.Visible = true;
                   this.barImformacion.Text = "Calculando la venta por horas...";
                    Gtk.TreeIter item;
			        lstCierres.Selection.GetSelected(out item);
			        this.barImformacion.Fraction = 1/3;
                    DataRow r = (DataRow)lstCierres.Model.GetValue(item,3);
                    this.barImformacion.Fraction = 2/3;
                    ListaVentaHoras = new GesVentas().CalcularVentaHoras(gesBase, (int)r["IDCierre"]);
                    lblInfSelcecionados.Texto = "Fecha = " + Utilidades.CadenasTexto.RotarFecha(r["FechaCierre"].ToString());
                    this.barImformacion.Fraction = 3/3;
                    VisorDeInformes.mostrarInforme(txtTicketInf.Buffer, ListaVentaHoras.ToArray());
                    this.barImformacion.Visible = false;
        }
		
        void VentaPorCamareros(){
                   this.barImformacion.Visible = true;
                   this.barImformacion.Text = "Calculando la venta por camareros...";
                    Gtk.TreeIter item;
			        lstCierres.Selection.GetSelected(out item);
			        DataRow r = (DataRow)lstCierres.Model.GetValue(item,3);
                       ListaVentaCamareros = new GesVentas().CalcularVentaCamareros(gesBase, (int)r["IDCierre"],
                    new BarraProgreso(this.barImformacion));
                    lblInfSelcecionados.Texto = "Fecha = " + Utilidades.CadenasTexto.RotarFecha(r["FechaCierre"].ToString()) ;
                    VisorDeInformes.mostrarInforme(txtTicketInf.Buffer, ListaVentaCamareros.ToArray());
                    this.barImformacion.Visible = false;
        }
       
        void CalcularCierre(){
            
			    this.barImformacion.Visible = true;
                   this.barImformacion.Text = "Calculando el desglose de cierre";
                    Gtk.TreeIter item;
			        lstCierres.Selection.GetSelected(out item);
			        this.barImformacion.Fraction = 1/4;
                    DataRow r = (DataRow)lstCierres.Model.GetValue(item,3);
                      this.barImformacion.Fraction = 2/4;
                  
                      
                      desInv = new GesVentas().CalcularCierre(gesBase, (int)r["IDTpv"],
                                                          (int)r["DesdeTicket"], (int)r["HastaTicket"]);
                        desInv.fechaCierre = r["FechaCierre"].ToString();
                        desInv.HoraCierre = r["HoraCierre"].ToString();
                      
                       this.barImformacion.Fraction = 3/4;
                  
                      
                      this.lblInfSelcecionados.Texto = " Num ticket = " + desInv.numTicket +
                                " Media = " +String.Format("{0:##0.00}", desInv.totalCierre / desInv.numTicket);
                      List<string> listado = new List<string>();
                      listado.Add(String.Format("Total del dia: {0:c}", desInv.totalCierre));
                      listado.Add(String.Format("Numero de ticket: {0}", desInv.numTicket));
                      listado.Add(String.Format("Media por ticket: {0:#0.00}", desInv.totalCierre / desInv.numTicket));
                      listado.Add("Fecha " + desInv.fechaCierre + "   Hora " + desInv.HoraCierre);
                      listado.Add("");
                        foreach (string s in desInv.lienasArticulos)
                        {
                                s.Trim(':',' ');
                                listado.Add(s);
                        }         
                        this.barImformacion.Fraction = 4/4;
                  
                       
                       VisorDeInformes.mostrarInforme(txtTicketInf.Buffer, listado.ToArray());
                       
                        this.barImformacion.Visible = false;
                  
                     
        }
		
		public void MostrarCierres(int idTpv){
			this.idTpv = idTpv;this.Show();
		
		    ListaLosPrimerosCierres();
			
			}
		
        
        
       
		void ListaLosPrimerosCierres(){
			maximo = 20;
			string consulta = SELECT_TOP_LIMIT.Replace("@IDTpv",idTpv.ToString())
			                                      .Replace("@MAXIMO" ,maximo.ToString());
			tbCierres = gesBase.EjecutarSqlSelect("ListadoCierre",consulta);
          	this.RellenarCierres();
          	
		}
       
        
        protected virtual void OnBtnAbrirCajonClicked (object sender, System.EventArgs e)
		{
			 	ImpresoraTicket.AbrirCajon(rutFicherosImp,nomImp);
     
		}
		
        protected virtual void OnBtnDesgloseClicked (object sender, System.EventArgs e)
		{
			this.btnHoras.Visible = true;
			this.btnDesglose.Visible = false;
			this.btnCamareros.Visible = true;
			modo = 0; this.lstCierres_SelectedIndexChanged(sender,e);

		}
		
        protected virtual void OnBtnCamarerosClicked (object sender, System.EventArgs e)
		{
			this.btnCamareros.Visible = false;
			this.btnDesglose.Visible = true;
			this.btnHoras.Visible = true;
			
			modo = 1; this.lstCierres_SelectedIndexChanged(sender,e);

		}
		
        protected virtual void OnBtnHorasClicked (object sender, System.EventArgs e)
		{
			this.btnHoras.Visible = false;
			this.btnDesglose.Visible = true;
			this.btnCamareros.Visible = true;
			
			modo = 2; this.lstCierres_SelectedIndexChanged(sender,e);
		}
		
        protected virtual void OnBtnImprimirClicked (object sender, System.EventArgs e)
		{
			switch(modo){
                case 0:
                  if (desInv != null)
                {
                    List<string> listado = new List<string>();
                    listado.Add(String.Format("Total del dia: {0:c}", desInv.totalCierre));
                    listado.Add(String.Format("Numero de ticket: {0}", desInv.numTicket));
                    listado.Add(String.Format("Media por ticket: {0:#0.00}", desInv.totalCierre / desInv.numTicket));
                    listado.Add("Fecha " + desInv.fechaCierre + "   Hora " + desInv.HoraCierre);
                    listado.Add("");
                    foreach (string s in desInv.lienasArticulos)
                    {
                            s.Replace(':',' ');
                            listado.Add(s);
                    }
                    ImpresoraTicket.ImprimeResumen(rutFicherosImp,nomImp, listado.ToArray(), "Cierre de caja");
                }
                break;
                case 1:
                  if (ListaVentaCamareros != null)
                  {
                    ImpresoraTicket.ImprimeResumen(rutFicherosImp,nomImp, ListaVentaCamareros.ToArray(), "Venta por camareros ");
                  }
                break;
                case 2:
                   if(ListaVentaHoras!=null){
                          ImpresoraTicket.ImprimeResumen(rutFicherosImp,nomImp,  ListaVentaHoras.ToArray(), "Venta por horas ");
                  }
                break;
             }
               
            
		}
		
        protected virtual void OnBtnMasClicked (object sender, System.EventArgs e)
		{
			maximo += 20;
			tbCierres = gesBase.EjecutarSqlSelect("ListadoCierre",SELECT_TOP_LIMIT.Replace("@IDTpv",idTpv.ToString())
			                                      .Replace("@MAXIMO" , maximo.ToString()));
                            this.RellenarCierres();
        }
		
        protected virtual void OnBtnCierreClicked (object sender, System.EventArgs e)
		{
			String[] lineas = new GesVentas().CalcularCierre(gesBase, idTpv);
                    if (lineas != null)
                    {
                      new  MetodosAsync(new MetodoSinArg(ListaLosPrimerosCierres)).GtkFuncAsync();
                    }
                    else
                    {
                         new DialogoTactil("Aviso en caja diaria", "No hay ticket nuevos desde el ultimo cierre." +
                            " No se puede hacer un cierre de caja", TipoRes.aceptar);
                         
                    }
            
		}
		
        protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
		{
		    if(salirListadoCierres != null) salirListadoCierres(this,new EventArgs());
			this.CerrarFormulario();
		}
		
		
		
		
		
	}
}

