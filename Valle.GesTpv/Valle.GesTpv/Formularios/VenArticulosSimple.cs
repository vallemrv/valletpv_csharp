// VenArticulos.cs created with MonoDevelop
// User: valle at 13:20 20/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using System.Data;
using System.Collections;

using Valle.SqlUtilidades;
using Valle.GtkUtilidades;

namespace Valle.GesTpv
{
	public delegate void OnSalirAltaRapida(DataRow dr);
	
	public partial class VenArticulosSimple : Gtk.Window
	{
        
		private GesBaseLocal gesLocal;
		private DataTable tbFamilias;
		private DataTable tbPrincipal;
		private DataTable tbVentaPorKilos;
		private DataTable tbNoEnventa;
		private DataRow drActivo;
		private Valle.Utilidades.HBuscador buscador;
		private Gtk.ListStore lstEncontrados = new Gtk.ListStore(typeof(string));
		private ContenedorFlot flot;
	    private Gdk.Rectangle miLocalizacion;
	    private string columBus = "";
	    
	
		private event OnSalirAltaRapida salirAltaRapida;
		
		public VenArticulosSimple(GesBaseLocal gesL, OnSalirAltaRapida OnSal,DataTable tbFamilias,DataTable tbVentaPorKilos,
		DataTable tbNoEnventa, DataTable tbArticulos) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
            //inicializacion de variables locales         
			gesLocal = gesL; 
			this.salirAltaRapida += OnSal;
			
			this.tbFamilias = tbFamilias;
    		this.tbVentaPorKilos = tbVentaPorKilos;
    		this.tbNoEnventa = tbNoEnventa;
    		this.tbPrincipal = tbArticulos;
			
			this.btnBuscar.Visible = false;
			this.ConfiguracionIni();
		}
		
		public VenArticulosSimple(GesBaseLocal gesL, OnSalirAltaRapida OnSal) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
            //inicializacion de variables locales         
			gesLocal = gesL; 
			this.salirAltaRapida += OnSal;
			
			tbFamilias = gesLocal.ExtraerLaTabla("Familias","IDFamilia");
    		tbVentaPorKilos = gesLocal.ExtraerLaTabla("VentaPorKilos","IDVinculacion");
    		this.tbNoEnventa = gesLocal.ExtraerLaTabla("ArticuloNoVenta","IDVinculacion");
    		tbPrincipal = gesLocal.ExtraerLaTabla("Articulos","IDVinculacion");
			
			
			this.ConfiguracionIni();
		}
		
		void ConfiguracionIni(){
	    	
			
			//Componentes para visualizar si ya existen los campos editados
			 buscador = new Valle.Utilidades.HBuscador(this.InsertarLinea,tbPrincipal);
		     Gtk.TreeView treeEncontrados = new Gtk.TreeView();
		     Gtk.ScrolledWindow scroll = new Gtk.ScrolledWindow();
		
			 treeEncontrados.AppendColumn("",new Gtk.CellRendererText(),"text",0);
			 treeEncontrados.HeadersVisible = false;
			 treeEncontrados.Model = lstEncontrados;
			 scroll.Add(treeEncontrados);
			 this.flot = new ContenedorFlot(scroll);
			 flot.Hide(); 
			 
			//No de pende de si hay articulos o no
			cargarFamilias();
		}
		private void cargarFamilias(){
		    Gtk.ListStore st = new Gtk.ListStore(typeof(String), typeof(DataRow));
			this.cmbFamilias.Model = st;
			foreach(DataRow dr in tbFamilias.Rows){
				st.AppendValues(dr["Nombre"].ToString(),dr);
			}
		}
		
		
		
		private void InsertarLinea(DataRow r){
		  Gtk.Application.Invoke(delegate {
    		if(r==null){
				   lstEncontrados.Clear();
				   flot.Hide();
			}else{
			 
			        flot.ShowAll();
			        flot.Move(miLocalizacion.X,miLocalizacion.Y);
			        flot.SetSizeRequest(miLocalizacion.Width,miLocalizacion.Height);
			     	lstEncontrados.AppendValues(r[columBus].ToString());
    			}
			});
			}
		
		
		
		private bool cargarReg(){
		 
		 Gtk.TreeIter iter;
                    
		    bool correcto = false;
            DataRow[] drID = this.tbPrincipal.Select("IDArticulo = '"+ this.txtID.Text+"'");
          if((drID.Length<=0)&&(txtNombre.Text.Length>0)&&(this.cmbFamilias.ActiveText!=null)){ 
            drActivo["IDArticulo"] = txtID.Text;
			drActivo["Nombre"] = txtNombre.Text;
			drActivo["Precio1"] = Decimal.Parse(txtPrecioUno.Text.Length>0?txtPrecioUno.Text:"0");
			drActivo["Precio2"] = Decimal.Parse(txtPrecioDos.Text.Length>0?txtPrecioDos.Text:"0");
			drActivo["Precio3"] = Decimal.Parse(txtPrecioTres.Text.Length>0?txtPrecioTres.Text:"0");
			this.cmbFamilias.GetActiveIter(out iter);
			DataRow rF = (DataRow)this.cmbFamilias.Model.GetValue(iter,1);
			drActivo["IDFamilia"] = rF["IDFamilia"];
			correcto = true;
			}
			return correcto;
		}
		
		private void cargarControl(){
			txtID.Text= drActivo["IDArticulo"].ToString() ;
			txtNombre.Text = drActivo["Nombre"].ToString();
			txtPrecioUno.Text= String.Format("{0}",drActivo["Precio1"]) ;
			txtPrecioDos.Text= String.Format("{0}",drActivo["Precio2"]) ;
			txtPrecioTres.Text=String.Format("{0}",drActivo["Precio3"])  ;
			this.chkPorKilos.Active = this.tbVentaPorKilos.Select("IDArticulo = "+drActivo["IDArticulo"].ToString()).Length>0;
	        this.cmbFamilias.Active = tbFamilias.Rows.IndexOf(tbFamilias.Select("IDFamilia = "+(int)drActivo["IDFamilia"])[0]);
	  }
		
		private void VaciarControles(){
		    //ponemos a cero la pagina de Datos de articulos
		    txtNombre.Text = "";
			txtPrecioUno.Text= "" ;
			txtPrecioDos.Text= "" ;
			txtPrecioTres.Text=""  ;
			this.chkPorKilos.Active = false;
			this.chkNoEnVenta.Active = false;
			this.txtID.GrabFocus();
			          
		}
		
		
		
		protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
		{
		   buscador.Parar();flot.Hide();
		   this.OnBtnAceptarClicked(null,null);
		   
		   
		}

		
	

		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
		    buscador.Parar();
		    flot.Destroy(); 
			this.Dispose();
			
		}

		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
		     try{
				  drActivo = tbPrincipal.NewRow();
				  if((this.cargarReg())){
				        tbPrincipal.Rows.Add(drActivo);
				        gesLocal.ActualizarSincronizar("Articulos",
				                         "IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				        gesLocal.GuardarDatos(drActivo,
				                         "IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Agregar);                 
				        
				          //para los articulos con venta por kilos
				          if(this.chkPorKilos.Active){
				                 DataRow  rKilos = this.tbVentaPorKilos.NewRow();
				                             rKilos["IDArticulo"] = drActivo["IDArticulo"];
				                             this.tbVentaPorKilos.Rows.Add(rKilos);
				                             string cadenaSelect = "IDVinculacion = "+rKilos["IDVinculacion"].ToString();
				                             gesLocal.ActualizarSincronizar("VentaPorKilos",cadenaSelect,AccionesConReg.Agregar);
							                 this.gesLocal.GuardarDatos(rKilos,cadenaSelect,AccionesConReg.Agregar); 
				                     }
				                     
				          //Articulos que no estan a la venta
				          if(this.chkNoEnVenta.Active){
		                    DataRow rVenta = this.tbNoEnventa.NewRow();
		                    rVenta["IDArticulo"] = drActivo["IDArticulo"];
				            this.tbNoEnventa.Rows.Add(rVenta);
				            string cadenaSelect = "IDVinculacion = "+rVenta["IDVinculacion"].ToString();
				            gesLocal.ActualizarSincronizar("ArticuloNoVenta",cadenaSelect,AccionesConReg.Agregar);
						    this.gesLocal.GuardarDatos(rVenta,cadenaSelect,AccionesConReg.Agregar);
						    
				           }                     
				            
				        this.VaciarControles();
				        this.lblInformacion.Text = "Modo añadir activado";
			            this.salirAltaRapida(drActivo);
                        this.Destroy();
				        }else{
				        this.lblInformacion.Text="Error en la introducion de datos";
				        }
				}catch (Exception ex){
					lblInformacion.Text = "Error : "+ex.Message;
				}
			
		}
		
		public void RegEncontrado(DataRow id){
		     drActivo = id;
		     this.cargarControl(); 
		}

		protected virtual void OnBtnBuscarClicked (object sender, System.EventArgs e)
		{
		      VenBuscadorSimple busSim = new VenBuscadorSimple(this.RegEncontrado,tbPrincipal,
		                                                 new String[]{"IDArticulo","Nombre"},"Nombre");
		                  busSim.Show();
		}

		
		

		protected virtual void OnTxtIDFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
		     buscador.Parar();
			if((!flot.IsFocus)&&(flot!=null)){
			      this.Modal = true;
			      this.flot.Hide();
			      }
		}

		protected virtual void OnTxtIDChanged (object sender, System.EventArgs e)
		{
		       if((this.cmbFamilias.ActiveText != null)&&(this.txtID.IsFocus)){
		            this.Modal = false;
		            this.lstEncontrados.Clear();
				    this.columBus = "IDArticulo";
			        buscador.Parar();
			        buscador.Buscar("IDArticulo like '"+this.txtID.Text+"%'");
				 	
				   //para saber las cordenadas de posicion de la ventana
		           //relativas a la pantalla
		           
				   	int x =0;
				   	int y =0;
				   	this.GetPosition(out x, out y);
				    
				    this.miLocalizacion = new Gdk.Rectangle(x+this.txtID.Allocation.X,y+this.txtID.Allocation.Y+
                                                      this.txtID.Allocation.Height+25,this.txtID.Allocation.Width,
                                                              100);
                                              
        		   
			}
		}

		protected virtual void OnTxtNombreChanged (object sender, System.EventArgs e)
		{
			if(this.cmbFamilias.ActiveText != null){	
			       this.lstEncontrados.Clear();
		           this.Modal = false;
		           //para saber las cordenadas de posicion de la ventana
		           //relativas a la pantalla
		            int x =0;
				   	int y =0;
				   	this.GetPosition(out x, out y);
				        
		        this.miLocalizacion = new Gdk.Rectangle(x+this.txtNombre.Allocation.X,y+this.txtNombre.Allocation.Y+
                                                      this.txtNombre.Allocation.Height+25,this.txtNombre.Allocation.Width,
                                                              100);
               this.columBus = "Nombre";                                               
               buscador.Parar();
               buscador.Buscar("Nombre like '"+this.txtNombre.Text+"%'");
			}
			
		}

	

		protected virtual void OnTxtNombreFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			   this.buscador.Parar();
               if((!flot.IsFocus)&&(flot!=null)){
			      this.Modal = true;
			      this.flot.Hide();
			      }
		}
		
		private void SalirFamilias(){
		    cargarFamilias();
		}

		protected virtual void OnBtnAñadirFamClicked (object sender, System.EventArgs e)
		{
		       VenFamilias venFam = new VenFamilias(gesLocal,this.tbFamilias);
		       venFam.salirFamilias+= new OnSalirFamilias(this.SalirFamilias);
		       venFam.Show();
		}

		protected virtual void OnCmbFamiliasChanged (object sender, System.EventArgs e)
		{
		   Gtk.TreeIter iter;
				this.cmbFamilias.GetActiveIter(out iter);
			    DataRow rF = (DataRow)this.cmbFamilias.Model.GetValue(iter,1);
			    this.txtID.Text = rF["IDFamilia"].ToString();
		}

		

	}
}
