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
	
	
	public partial class VenArticulos : Gtk.Window
	{
        enum Modos { Editar, Borrar, Añadir, Salir};		
		
		private GesBaseLocal gesLocal;
		private DataTable tbFamilias;
		private DataTable tbPrincipal;
		private DataTable tbVentaPorKilos;
		private DataTable tbInventarios;
		private DataTable tbAlmacen;
		private DataTable tbDesgloseArt;
		private DataTable tbNoEnventa;

		private int reg = 0;
		private DataRow drActivo;
		private Modos modo;
		private Valle.Utilidades.HBuscador buscador;
		private ContenedorFlot flot;
	    private Gdk.Rectangle miLocalizacion;
	    Gtk.ListStore lstEncontrados = new Gtk.ListStore(typeof(string));
		string columBus = "";
		
		
		public VenArticulos(GesBaseLocal gesL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
            //inicializacion de variables locales         
			gesLocal = gesL; 
			this.tbFamilias = gesLocal.ExtraerLaTabla("Familias","IDVinculacion");
    		this.tbVentaPorKilos = gesLocal.ExtraerLaTabla("VentaPorKilos","IDVinculacion");
    		this.tbNoEnventa = gesLocal.ExtraerLaTabla("ArticuloNoVenta","IDVinculacion");
    		this.tbAlmacen = gesLocal.ExtraerLaTabla("Almacen","IDVinculacion");
    		this.tbDesgloseArt = gesLocal.ExtraerLaTabla("DesgloseArt","IDVinculacion");
    		this.tbInventarios = gesLocal.ExtraerLaTabla("Inventarios","IDVinculacion");
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Articulos","IDVinculacion");
			
			
			 //Componentes para visualizar si ya existen los campos editados
			 buscador = new Valle.Utilidades.HBuscador(this.InsertarLinea,tbPrincipal);
		     Gtk.TreeView treeEncontrados = new Gtk.TreeView();
		     Gtk.ScrolledWindow scroll = new Gtk.ScrolledWindow();
		     Gtk.EventBox cajaEventos = new Gtk.EventBox();
		
			 treeEncontrados.AppendColumn("",new Gtk.CellRendererText(),"text",0);
			 treeEncontrados.HeadersVisible = false;
			 treeEncontrados.Model = lstEncontrados;
			 scroll.Add(treeEncontrados);
			 cajaEventos.Add(scroll);
			 this.flot = new ContenedorFlot(cajaEventos);
			 
			// flot.Hide();
			 
			 
			
			//inicializacion de apariencia
			modo = Modos.Salir; 
			this.ModoEditable(false);
			this.MostrarAceptar(false);
			this.btnGrabar.Visible = false;
		    this.lblTotalReg.Text = String.Format("Num reg {0}",tbPrincipal.Rows.Count);
			this.lblRegAct.Text = "";
			this.lblIDArt.Text = "";
			this.lblNomArt.Text = "";
			this.lblNomArtDes.Text = "";
			lblInformacion.Text = "No hay registros que mostrar";
			Gtk.ListStore st = new Gtk.ListStore(typeof(String), typeof(DataRow));
			this.cmbFamilias.Model = st;
			st = new Gtk.ListStore(typeof(String), typeof(DataRow));
			this.cmbAlmacen.Model = st;
			st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String),typeof(String),
		                                typeof(String), typeof(String), typeof(DataRow), typeof(DataRow));
			
			this.treeAlmacenes.Model = st;
		    st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String),
		                                                typeof(String), typeof(DataRow), typeof(DataRow));
			this.treeDesgloses.Model = st;
		
			
			
			//Apariencia para desgloses
			this.treeDesgloses.AppendColumn("Nombre Art",new Gtk.CellRendererText(),"text",0);
			this.treeDesgloses.AppendColumn("Grupo ",new Gtk.CellRendererText(),"text",1);
			this.treeDesgloses.AppendColumn("Art que genera",new Gtk.CellRendererText(),"text",2);
			this.treeDesgloses.AppendColumn("Incremento ",new Gtk.CellRendererText(),"text",3);
			
			//Apariencia para inventarios (Stock)
			this.treeAlmacenes.AppendColumn("Nom Almacen",new Gtk.CellRendererText(),"text",0);
			this.treeAlmacenes.AppendColumn("Descripcion ",new Gtk.CellRendererText(),"text",1);
			this.treeAlmacenes.AppendColumn("Nivel ",new Gtk.CellRendererText(),"text",2);
			this.treeAlmacenes.AppendColumn("Stock Max ",new Gtk.CellRendererText(),"text",3);
			this.treeAlmacenes.AppendColumn("Stock Min ",new Gtk.CellRendererText(),"text",4);
			this.treeAlmacenes.AppendColumn("Stock actual ",new Gtk.CellRendererText(),"text",5);
			
			//No de pende de si hay articulos o no
			cargarFamilias();
		    cargarAlmacenes();
                     
                      
            //Depende de los articulos que hayan
			if(tbPrincipal.Rows.Count>0){
			           
			           this.lblRegAct.Text = String.Format("Reg {0}",reg+1);
			           drActivo = tbPrincipal.Rows[reg];
		              
				       lblInformacion.Text = "Esta usted en modo no editable";
                       cargarControl();
                       
				      }
			
		}
		
		private void cargarFamilias(){
			Gtk.ListStore st = (Gtk.ListStore)cmbFamilias.Model;
			                    st.Clear();
		    foreach(DataRow dr in tbFamilias.Rows){
				st.AppendValues(dr["Nombre"].ToString(),dr);
			}
		}
		
		private void cargarAlmacenes(){
			Gtk.ListStore st = (Gtk.ListStore)cmbAlmacen.Model;
			                    st.Clear();
		    	
		    foreach(DataRow dr in this.tbAlmacen.Rows){
				st.AppendValues(dr["NombreAlmacen"].ToString()+" "+dr["Descripcion"].ToString(),dr);
			}
		}
		
		private void cargarInfInv(){
		   	Gtk.ListStore st = (Gtk.ListStore)treeAlmacenes.Model;
			                    st.Clear();
		    
					
			DataRow[] rs = this.tbInventarios.Select("IDArt = '"+drActivo["IDArticulo"].ToString()+"'");
			
			foreach(DataRow dr in rs){
	            DataRow rArtInv = this.tbAlmacen.Select("IDVinculacion = "+dr["IDAlm"].ToString())[0];		
				st.AppendValues(rArtInv["NombreAlmacen"].ToString(), rArtInv["Descripcion"].ToString(), dr["Nivel"].ToString(),
				                        dr["MaxStock"].ToString(), dr["MinStock"].ToString(), dr["Stock"].ToString(), dr, rArtInv);
			}
		
		}
		
		private void cargarDesgloses(){
			Gtk.ListStore st = (Gtk.ListStore)treeDesgloses.Model;
		   	st.Clear();
			
			DataRow[] rs = this.tbDesgloseArt.Select("IDArtPrimario = '"+drActivo["IDArticulo"].ToString()+"'");
			
			foreach(DataRow dr in rs){
			    DataRow rArtDes = this.tbPrincipal.Select("IDArticulo = '"+dr["IDArtDesglose"].ToString()+"'")[0];		
				st.AppendValues(rArtDes["Nombre"].ToString(), dr["Grupo"].ToString(), dr["CanArtGenera"].ToString(),
				                        dr["Incremento"].ToString(), dr, rArtDes);
			}
		
		}

		private void InsertarLinea(DataRow l){
		  Gtk.Application.Invoke(delegate {
    		if(l==null){
				   lstEncontrados.Clear();
				   flot.Hide();
			}else{
			        flot.ShowAll();
			        flot.Move(miLocalizacion.X,miLocalizacion.Y);
			        flot.SetSizeRequest(miLocalizacion.Width,miLocalizacion.Height);
			     	lstEncontrados.AppendValues(l[columBus].ToString());
    			}
			});
			}
		
		private void ModoEditable(bool editable){
		    this.pneDatosArt.Sensitive = editable;
            this.pneDesglose.Sensitive = editable;
            this.pneControlStock.Sensitive = editable;
     	}
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar && modo==Modos.Borrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.btnMasArt.Visible = false;
	    }
		
		
		
		private bool cargarReg(){
		 
		 Gtk.TreeIter iter;
                    
		    bool correcto = false;
            DataRow[] drID = this.tbPrincipal.Select("IDArticulo = '"+ this.txtID.Text+"'");
         switch(modo){
         case Modos.Añadir:
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
			break;
	     case Modos.Editar:
	       if((drID.Length<=1)&&(txtNombre.Text.Length>0)&&(this.cmbFamilias.ActiveText!=null)){ 
            drActivo["IDArticulo"] = txtID.Text;
			drActivo["Nombre"] = txtNombre.Text;
			drActivo["Precio1"] = Decimal.Parse(txtPrecioUno.Text.Remove(txtPrecioUno.Text.Length-1));
			drActivo["Precio2"] = Decimal.Parse(txtPrecioDos.Text.Remove(txtPrecioDos.Text.Length-1));
			drActivo["Precio3"] = Decimal.Parse(txtPrecioTres.Text.Remove(txtPrecioTres.Text.Length-1));
			this.cmbFamilias.GetActiveIter(out iter);
			DataRow rF = (DataRow)this.cmbFamilias.Model.GetValue(iter,1);
			drActivo["IDFamilia"] = rF["IDFamilia"];
			correcto = true;
			}
			break;
	
			}
			
			return correcto;
		}
		
		private void cargarControl(){
		    this.lblRegAct.Text = "Reg "+ (reg+1).ToString();
	        txtID.Text= drActivo["IDArticulo"].ToString() ;
			txtNombre.Text = drActivo["Nombre"].ToString();
			this.lblIDArt.Text= "ID: "+drActivo["IDArticulo"].ToString() ;
			this.lblNomArt.Text = "Nombre: "+ drActivo["Nombre"].ToString();
			txtPrecioUno.Text= String.Format("{0}",drActivo["Precio1"]) ;
			txtPrecioDos.Text= String.Format("{0}",drActivo["Precio2"]) ;
			txtPrecioTres.Text=String.Format("{0}",drActivo["Precio3"])  ;
			this.chkPorKilos.Active = this.tbVentaPorKilos.Select("IDArticulo = '"+drActivo["IDArticulo"].ToString()+"'").Length>0;
			this.chkNoEnVenta.Active = this.tbNoEnventa.Select ("IDArticulo = '"+drActivo["IDArticulo"].ToString()+"'").Length>0;
	     	 cargarInfInv();
             cargarDesgloses();
		    this.cmbFamilias.Active = tbFamilias.Rows.IndexOf(tbFamilias.Select("IDFamilia = '"+drActivo["IDFamilia"]+"'")[0]);
	        this.cmbAlmacen.Active = -1;
		}
	
		private void VaciarControles(){
		    //ponemos a cero la pagina de Datos de articulos
		    this.lblIDArt.Text = "Creando un registro nuevo";
		    this.lblNomArt.Text = "";
		    this.txtNombre.Text= "";
			txtPrecioUno.Text= "" ;
			txtPrecioDos.Text= "" ;
			txtPrecioTres.Text=""  ;
			this.chkPorKilos.Active = false;
			this.chkNoEnVenta.Active = false;
			
			//ponemos a cero la pagina Desgloses
			this.spnCantidad.Text = "0";
			this.spnGrupo.Text = "0";
			this.spnIncremento.Text = "0";
			((Gtk.ListStore)this.treeDesgloses.Model).Clear();
			this.lblNomArtDes.Text = "";
			
			//ponemos a cero la pagina Inventarios
			this.spnNivel.Text = "0";
			this.spnStockActual.Text = "0";
			this.spnStockMax.Text = "0";
			((Gtk.ListStore)this.treeAlmacenes.Model).Clear();
			
			
			this.tabArticulos.CurrentPage = 0;
			this.txtID.GrabFocus();
			          
		}
		
		
		protected virtual void OnBtnPrimeroClicked (object sender, System.EventArgs e)
		{
			reg=0;
			drActivo= tbPrincipal.Rows[reg];
			cargarControl();
			this.lblRegAct.Text = String.Format("Reg {0}",reg+1);
			
		}

		protected virtual void OnBtnAtrasClicked (object sender, System.EventArgs e)
		{
			if(reg>0){reg--;}
			drActivo= tbPrincipal.Rows[reg];
			cargarControl();
			
			this.lblRegAct.Text = String.Format("Reg {0}",reg+1);
		}

		protected virtual void OnBtnSiguienteClicked (object sender, System.EventArgs e)
		{
			if(tbPrincipal.Rows.Count-1>reg){reg++;}
			drActivo= tbPrincipal.Rows[reg];
			cargarControl();
			
			this.lblRegAct.Text = String.Format("Reg {0}",reg+1);
		}

		protected virtual void OnBtnUltimoClicked (object sender, System.EventArgs e)
		{
			reg=tbPrincipal.Rows.Count-1;
			drActivo= tbPrincipal.Rows[reg];
			cargarControl();
			this.lblRegAct.Text = String.Format("Reg {0}",reg+1);

		}

		protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
		{
			if(modo == Modos.Salir){
			    if(flot!=null)flot.Destroy();
				this.Destroy();
			}else{
				
				this.ModoEditable(false);
				this.MostrarAceptar(false);
				this.grpNavegador.Sensitive = true;
				this.grpModos.Sensitive = true;
				
				this.flot.Hide(); //ocultar el listado de encontrados;
				
				this.modo = Modos.Salir;
				drActivo= tbPrincipal.Rows[reg];
	            cargarControl();
			
			    this.lblRegAct.Text = String.Format("Reg {0}",reg+1);
			   	this.lblInformacion.Text = "Esta en modo no editable";
			   	
			}
		}

		protected virtual void OnBtnBorrarClicked (object sender, System.EventArgs e)
		{
			     this.modo = Modos.Borrar;
			     this.lblInformacion.Text = "Ha entrado en modo borrar. Pulse aceptar para borrar el registro";
			     this.MostrarAceptar(true);
			    
		}

		protected virtual void OnBtnAñadirArtClicked (object sender, System.EventArgs e)
		{
			lblInformacion.Text = "Esta en modo añadir, pulse aceptar para añadir registro";
			this.lblRegAct.Text = "Nuevo";
			this.modo = Modos.Añadir;
			this.MostrarAceptar(true);
			this.grpNavegador.Sensitive = false;
			this.pneDatosArt.Sensitive = true;
			this.btnGrabar.Visible = true;
			this.btnMasArt.Visible = true;
			this.VaciarControles();
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
			 switch(modo){
			   case Modos.Añadir:
			            this.VaciarControles();
				        this.lblTotalReg.Text = String.Format("Num reg {0}",tbPrincipal.Rows.Count);
				        this.lblInformacion.Text = "Modo añadir activado";
				break;
			case Modos.Borrar:
			      gesLocal.ActualizarSincronizar("Articulos","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				  gesLocal.GuardarDatos("Articulos","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				  this.tbPrincipal.Rows.Remove(drActivo);
				  this.lblInformacion.Text = "Modo borrar activado";
				  this.OnBtnSiguienteClicked(null,null);
				break;
			case Modos.Editar:
			        this.lblInformacion.Text = "Modo editar activado";
    			  	this.OnBtnSiguienteClicked(null,null);
    				
				break;
			default:
				break;
				}
				}catch (Exception ex){
					lblInformacion.Text = "Error : "+ex.Message;
				}
			
		}
		
		public void RegEncontrado(DataRow dr){
		     drActivo = dr;
		     this.reg = tbPrincipal.Rows.IndexOf(drActivo);
		     this.lblRegAct.Text = "Reg "+ reg.ToString();
		     this.cargarControl(); 
		}

		protected virtual void OnBtnBuscarClicked (object sender, System.EventArgs e)
		{
		      VenBuscadorSimple busSim = new VenBuscadorSimple(this.RegEncontrado,tbPrincipal,
		                                                 new String[]{"IDArticulo","Nombre"},"Nombre");
		                  busSim.Show();
		}

		protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
		{
                 this.ModoEditable(true);
			     this.MostrarAceptar(true);
			     this.modo = Modos.Editar;
			     this.btnGrabar.Visible = true;
			     this.lblInformacion.Text = "Ha entrado en modo editar. Pulse aceptar para editar el registro";
	
		}

		protected virtual void OnCmbFamiliasChanged (object sender, System.EventArgs e)
		{
			if((this.modo==Modos.Añadir)&&(this.cmbFamilias.ActiveText!=null)){
			    Gtk.TreeIter iter;
				this.cmbFamilias.GetActiveIter(out iter);
			    DataRow rF = (DataRow)this.cmbFamilias.Model.GetValue(iter,1);
			    this.txtID.Text = rF["IDFamilia"].ToString();
		   	}
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
		   if(((this.modo==Modos.Añadir)||(this.modo==Modos.Editar))&& 
			              (this.cmbFamilias.ActiveText!=null)&&(this.txtID.IsFocus)){
			        this.Modal    = false;
			        this.lstEncontrados.Clear();
				    columBus = "IDArticulo";
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
			if(((this.modo==Modos.Añadir)||(this.modo==Modos.Editar))&&(this.txtNombre.IsFocus)){
				   
				    this.Modal = false;
			        buscador.Parar();
				    this.lstEncontrados.Clear();
				    columBus = "Nombre";
				    buscador.Buscar("Nombre LIKE '"+this.txtNombre.Text+"%'");
		           
		           //para saber las cordenadas de posicion de la ventana
		           //relativas a la pantalla
		            int x =0;
				   	int y =0;
				   	this.GetPosition(out x, out y);
				        
		        this.miLocalizacion = new Gdk.Rectangle(x+this.txtNombre.Allocation.X,y+this.txtNombre.Allocation.Y+
                                                      this.txtNombre.Allocation.Height+25,this.txtNombre.Allocation.Width,
                                                              100);
                                                              
               
			}
			
		}

	

		protected virtual void OnTxtNombreFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			   buscador.Parar();   
              if((!flot.IsFocus)&&(flot!=null)){
			      this.Modal = true;
			      this.flot.Hide();
			      }
		}
		
		private void SalirFamilias(){
		    this.lblIDArt .Text = String.Format("Num reg {0}",this.tbPrincipal.Rows.Count);
		    cargarFamilias();
		}

		protected virtual void OnBtnAñadirFamClicked (object sender, System.EventArgs e)
		{
		       VenFamilias venFam = new VenFamilias(gesLocal,tbFamilias);
		       venFam.salirFamilias+= new OnSalirFamilias(this.SalirFamilias);
		       venFam.Show();
		}
	
	    void OnAgregarRegBusOCreado(DataRow rArt){
	       this.Modal = true;
	       if(rArt!=null){
		    DataRow rDes = this.tbDesgloseArt.NewRow();
			     	    rDes["IDArtPrimario"] = drActivo["IDArticulo"];
        			    rDes["IDArtDesglose"] = rArt["IDArticulo"];
        			    rDes["CanArtGenera"] = this.spnCantidad.Value;
        			    rDes["Grupo"] = this.spnGrupo.Value;
        			    rDes["Incremento"] = this.spnIncremento.Value;
        			    this.tbDesgloseArt.Rows.Add(rDes);
        			    gesLocal.ActualizarSincronizar("DesgloseArt","IDVinculacion = "+rDes["IDVinculacion"].ToString(),AccionesConReg.Agregar);
						this.gesLocal.GuardarDatos(rDes,"IDVinculacion = "+rDes["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				     
		             this.cargarDesgloses();
		             }
		}

		protected virtual void OnBtnBuscarAgreClicked (object sender, System.EventArgs e)
		{
		     VenBuscadorSimple busSim = new VenBuscadorSimple(this.OnAgregarRegBusOCreado,tbPrincipal,
		                                                 new String[]{"IDArticulo","Nombre"},"Nombre");
		                  busSim.Show();
		}

		protected virtual void OnBtnCrearAgreClicked (object sender, System.EventArgs e)
		{
		    this.Modal = false;
		     VenArticulosSimple veSim = new  VenArticulosSimple(this.gesLocal,this.OnAgregarRegBusOCreado,tbFamilias,
		                                        tbVentaPorKilos,tbNoEnventa,tbPrincipal);
		     veSim.Show();
		}

		
		protected virtual void OnTreeDesglosesCursorChanged (object sender, System.EventArgs e)
		{
		            Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.treeDesgloses.Selection.GetSelected(out model, out iter)) {
			            DataRow rArt = (DataRow)model.GetValue(iter,5);
			            DataRow rDes = (DataRow)model.GetValue(iter,4);
			            this.lblNomArtDes.Text = "Articulo: " + rArt["Nombre"].ToString();
			            this.spnCantidad.Text = rDes["CanArtGenera"].ToString();
			            this.spnGrupo.Text = rDes["Grupo"].ToString();
			            this.spnIncremento.Text = rDes["Incremento"].ToString();
			         }
		}

		
		protected virtual void OnTreeAlmacenesCursorChanged (object sender, System.EventArgs e)
		{
		           Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.treeAlmacenes.Selection.GetSelected(out model, out iter)) {
			            DataRow rAlm = (DataRow)model.GetValue(iter,7);
			            DataRow rInv = (DataRow)model.GetValue(iter,6);
			            this.spnSockMini.Text = rInv["MinStock"].ToString();
			            this.spnStockMax.Text = rInv["MaxStock"].ToString();
			            this.spnStockActual.Text = rInv["Stock"].ToString();
			            this.spnNivel.Text = rInv["Nivel"].ToString();
			            this.cmbAlmacen.Active = this.tbAlmacen.Rows.IndexOf(rAlm);
			         }
		}

		
		protected virtual void OnBtnAceptarDesClicked (object sender, System.EventArgs e)
		{
		            Gtk.TreeModel model;
		            Gtk.TreeIter iter;
		            DataRow rDes;
		            DataRow rArt;
		            
	               	if (this.treeDesgloses.Selection.GetSelected(out model, out iter)) {
			            rDes = (DataRow)model.GetValue(iter,4);
			     		rArt = (DataRow)model.GetValue(iter,5);
        			    rDes["IDArtPrimario"] = drActivo["IDArticulo"];
        			    rDes["IDArtDesglose"] = rArt["IDArticulo"];
        			    rDes["CanArtGenera"] = this.spnCantidad.Value;
        			    rDes["Grupo"] = this.spnGrupo.Value;
        			    rDes["Incremento"] = this.spnIncremento.Value;
        			    
        			    gesLocal.ActualizarSincronizar("DesgloseArt","IDVinculacion = "+rDes["IDVinculacion"].ToString(),AccionesConReg.Modificar);
						this.gesLocal.GuardarDatos(rDes,"IDVinculacion = "+rDes["IDVinculacion"].ToString(),AccionesConReg.Modificar);
						this.VaciarControlesDes();
						
        			 }
		             this.cargarDesgloses();
		           
		}

		protected virtual void OnChkPorKilosClicked (object sender, System.EventArgs e)
		{
		       DataRow rKilos;
		       if((this.modo == Modos.Editar)&&(this.chkPorKilos.IsFocus)){
		            if(this.chkPorKilos.Active){
		                  rKilos = this.tbVentaPorKilos.NewRow();
		                  rKilos["IDArticulo"] = drActivo["IDArticulo"];
				            this.tbVentaPorKilos.Rows.Add(rKilos);
				            string cadenaSelect = "IDVinculacion = "+rKilos["IDVinculacion"].ToString();
				            gesLocal.ActualizarSincronizar("VentaPorKilos",cadenaSelect,AccionesConReg.Agregar);
						    this.gesLocal.GuardarDatos(rKilos,cadenaSelect,AccionesConReg.Agregar);
		            }else{
		                 rKilos = this.tbVentaPorKilos.Select("IDVinculacion = "+drActivo["IDVinculacion"].ToString())[0];
		                 gesLocal.ActualizarSincronizar("VentaPorKilos","IDVinculacion = "+rKilos["IDVinculacion"].ToString(),AccionesConReg.Borrar);
						 this.gesLocal.GuardarDatos("VentaPorKilos", "IDVinculacion = "+rKilos["IDVinculacion"].ToString(), AccionesConReg.Borrar);
						 rKilos.Delete();
		              
		            }
		       }
		}

		protected virtual void OnBtnBorrarAlmClicked (object sender, System.EventArgs e)
		{
		         Gtk.TreeModel model;
		            Gtk.TreeIter iter;
		            DataRow rInv;
		            
	               	if (this.treeAlmacenes.Selection.GetSelected(out model, out iter)) {
			            rInv = (DataRow)model.GetValue(iter,6);
			     		gesLocal.ActualizarSincronizar("Inventarios","IDVinculacion = "+rInv["IDVinculacion"].ToString(),AccionesConReg.Borrar);
						this.gesLocal.GuardarDatos("Inventarios","IDVinculacion = "+rInv["IDVinculacion"].ToString(),AccionesConReg.Borrar);
						rInv.Delete();tbAlmacen.AcceptChanges();
						  this.cargarInfInv();
						this.VaciarControlesAlm();
        			 }
		           
		     
		}

		protected virtual void OnBtnBorrarDesClicked (object sender, System.EventArgs e)
		{
		            Gtk.TreeModel model;
		            Gtk.TreeIter iter;
                    DataRow rDes;
		            
	               	if (this.treeDesgloses.Selection.GetSelected(out model, out iter)) {
			            rDes = (DataRow)model.GetValue(iter,4);
			     		gesLocal.ActualizarSincronizar("DesgloseArt","IDVinculacion = "+rDes["IDVinculacion"].ToString(),AccionesConReg.Borrar);
						this.gesLocal.GuardarDatos("DesgloseArt","IDVinculacion = "+rDes["IDVinculacion"].ToString(),AccionesConReg.Borrar);
						rDes.Delete();tbDesgloseArt.AcceptChanges();
						this.cargarDesgloses();
						this.VaciarControlesDes();
        			 }
		            
		     
		}

		protected virtual void OnBtnAceptarAlmClicked (object sender, System.EventArgs e)
		{ 
		            Gtk.TreeModel model;
		            Gtk.TreeIter iter;
		            Gtk.TreeIter iterAlm;
		            DataRow rInv;
		            DataRow rAlm;
		            
	               	if (this.treeAlmacenes.Selection.GetSelected(out model, out iter)) {
			            rInv = (DataRow)model.GetValue(iter,6);
			     		this.cmbAlmacen.GetActiveIter(out iterAlm);
        			    rAlm = (DataRow)this.cmbAlmacen.Model.GetValue(iterAlm,1);
        			    rInv["IDArt"] = drActivo["IDArticulo"];
        			    rInv["IDAlm"] = rAlm["IDVinculacion"];
        			    rInv["MaxStock"] = this.spnStockMax.Value;
        			    rInv["MinStock"] = this.spnSockMini.Value;
        			    rInv["Stock"] = this.spnStockActual.Value;
        			    rInv["Nivel"] = this.spnNivel.Value;
        			    gesLocal.ActualizarSincronizar("Inventarios","IDVinculacion = "+rInv["IDVinculacion"].ToString(),AccionesConReg.Modificar);
						this.gesLocal.GuardarDatos(rInv,"IDVinculacion = "+rInv["IDVinculacion"].ToString(),AccionesConReg.Modificar);
					    this.VaciarControlesAlm();
        			 }else{
		                rInv = this.tbInventarios.NewRow();
			     		this.cmbAlmacen.GetActiveIter(out iterAlm);
        			    rAlm = (DataRow)this.cmbAlmacen.Model.GetValue(iterAlm,1);
        			    rInv["IDArt"] = drActivo["IDArticulo"];
        			    rInv["IDAlm"] = rAlm["IDVinculacion"];
        			    rInv["MaxStock"] = this.spnStockMax.Value;
        			    rInv["MinStock"] = this.spnSockMini.Value;
        			    rInv["Stock"] = this.spnStockActual.Value;
        			    rInv["Nivel"] = this.spnNivel.Value;
        			    this.tbInventarios.Rows.Add(rInv);
        			    gesLocal.ActualizarSincronizar("Inventarios","IDVinculacion = "+rInv["IDVinculacion"].ToString(),AccionesConReg.Agregar);
						this.gesLocal.GuardarDatos(rInv,"IDVinculacion = "+rInv["IDVinculacion"].ToString(),AccionesConReg.Agregar);
		             }
		             
		             this.cargarInfInv();
		      
		}
		void VaciarControlesDes(){
		   this.lblNomArtDes.Text = "";
		   this.spnCantidad.Text = "0";
		   this.spnGrupo.Text="0";
		   this.spnIncremento.Text="0";
		   this.treeDesgloses.Selection.UnselectAll();
		}
		
		void VaciarControlesAlm(){
		      this.spnStockActual.Text = "0";
		      this.spnSockMini.Text = "0";
		      this.spnStockMax.Text = "0";
		      this.spnNivel.Text = "0";
		      this.cmbAlmacen.Active = -1;
		      this.treeAlmacenes.Selection.UnselectAll();
		}
		
		void salirAlmacen(){
		  this.cargarAlmacenes();
		}

		protected virtual void OnBtnMasAlmClicked (object sender, System.EventArgs e)
		{
		   VenAlmacenes venalm = new VenAlmacenes(this.gesLocal,tbAlmacen);
		   venalm.salirAlmacen += this.salirAlmacen;
		}

		protected virtual void OnBtnGrabarClicked (object sender, System.EventArgs e)
		{
		         switch(modo){
		         case Modos.Añadir:
		         drActivo = tbPrincipal.NewRow();
				  if(this.cargarReg()){
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
				                   
				        this.btnGrabar.Visible = false; 
				        this.ModoEditable(true);
				        this.lblTotalReg.Text = String.Format("Num reg {0}",tbPrincipal.Rows.Count);
				        this.lblRegAct.Text = "Reg grabado";
				        this.lblInformacion.Text = "Modo añadir activado";
				        }else{
				          this.lblInformacion.Text="Error en la introducion de datos";
				        }
				        break;
				        case Modos.Editar:
				             if(cargarReg()){
    			    	      gesLocal.ActualizarSincronizar("Articulos","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
    				           gesLocal.GuardarDatos(drActivo,"IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
    				           this.lblInformacion.Text = "Guardado los cambios....";
                				  }else{
            				        this.lblInformacion.Text="Error en la introducion de datos";
            				    }
			    
				        break;
				        }
		}

		protected virtual void OnChkNoEnVentaClicked (object sender, System.EventArgs e)
		{
		       DataRow rVenta;
		       if((this.modo == Modos.Editar)&&(this.chkNoEnVenta.IsFocus)){
		            if(this.chkNoEnVenta.Active){
		                  rVenta = this.tbNoEnventa.NewRow();
		                  rVenta["IDArticulo"] = drActivo["IDArticulo"];
				            this.tbNoEnventa.Rows.Add(rVenta);
				            string cadenaSelect = "IDVinculacion = "+rVenta["IDVinculacion"].ToString();
				            gesLocal.ActualizarSincronizar("ArticuloNoVenta",cadenaSelect,AccionesConReg.Agregar);
						    this.gesLocal.GuardarDatos(rVenta,cadenaSelect,AccionesConReg.Agregar);
		            }else{
		                 rVenta = this.tbVentaPorKilos.Select("IDVinculacion = "+drActivo["IDVinculacion"].ToString())[0];
		                 gesLocal.ActualizarSincronizar("ArticuloNoVenta","IDVinculacion = "+rVenta["IDVinculacion"].ToString(),AccionesConReg.Borrar);
						 this.gesLocal.GuardarDatos("ArtiucloNoVenta", "IDVinculacion = "+rVenta["IDVinculacion"].ToString(), AccionesConReg.Borrar);
						 rVenta.Delete();
		              
		            }
		       }
		}

		ContenedorFlot ayuda;
		protected virtual void OnSpnNivelFocusInEvent (object o, Gtk.FocusInEventArgs args)
		{
			if(ayuda!=null) ayuda.Destroy();
			ayuda = new ContenedorFlot(new Gtk.Label(" El Nivel 0 es el almacen principal.\n "+
                                                        " Los niveles 1,2,3,4.....etc. se surten del almacen padre.\n   "+
                                                         " El nivel 1 del 0, 2 del 1, etc.\n\n "+
			                                             " Ejmp: del nivel 1 en adelante, pueden ser almacenes      \n"+
			                                             " intermedios o de ventas \n"+
                                                         " Como son las neveras, grifos, expositores etc.  " ));
			int x;
			int y;
			this.GetPosition(out x, out y);
			ayuda.Move(((Gtk.Widget)o).Allocation.X+x+5,((Gtk.Widget)o).Allocation.Y+y+60);
			ayuda.ShowAll();
		}

		protected virtual void OnSpnNivelFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
		{
			 if(ayuda!=null) ayuda.Destroy();
		}

		protected virtual void OnSpnCantidadFocusInEvent (object o, Gtk.FocusInEventArgs args)
		{
			if(ayuda!=null) ayuda.Destroy();
			ayuda = new ContenedorFlot(new Gtk.Label(" Cuantos  elementos del articulo editado genera  \n"+
                                                        " el principal producto.   \n"+
                                                         " \n\n "+
			                                             " Ejmp: de una botella de 1L salen cinco vasos de un quinto o  \n "+
			                                             " cuantas cervezas salen de un barril, etc."));
			int x;
			int y;
			this.GetPosition(out x, out y);
			ayuda.Move(((Gtk.Widget)o).Allocation.X+x+5,((Gtk.Widget)o).Allocation.Y+y+60);
			ayuda.ShowAll();
		}

		
		protected virtual void OnSpnGrupoFocusInEvent (object o, Gtk.FocusInEventArgs args)
		{
			if(ayuda!=null) ayuda.Destroy();
			ayuda = new ContenedorFlot(new Gtk.Label(" Esto es para los combinados y los productos principales. "+
                                                        "\n\n "+
			                                             " grupo 0 no sale en la pantalla de combinados.      \n"+
			                                             " Del 1 en adelante son los grupos que tienen algo en comun y \n"+
                                                         " se veran en la pantalla de combinados para su eleccion \n  "+
			                                             " ejmp: Un suizo, el grupo 0 es el suizo pues este no se elige \n"+
			                                             " 1 para los ingredientes, mantequilla, aceite, sobrasada, etc. \n"+
			                                             " 2 para los ingredientes mixtos, tomate, mermelada etc."));
			int x;
			int y;
			this.GetPosition(out x, out y);
			ayuda.Move(((Gtk.Widget)o).Allocation.X+x+5,((Gtk.Widget)o).Allocation.Y+y+60);
			ayuda.ShowAll();
		}

		
		protected virtual void OnSpnIncrementoFocusInEvent (object o, Gtk.FocusInEventArgs args)
		{
			if(ayuda!=null) ayuda.Destroy();
			ayuda = new ContenedorFlot(new Gtk.Label(" Incrementa el precio total en la cantidad  \n"+
			                                         " estipulada si se elige este ingrediente."));
			int x;
			int y;
			this.GetPosition(out x, out y);
			ayuda.Move(((Gtk.Widget)o).Allocation.X+x+5,((Gtk.Widget)o).Allocation.Y+y+60);
			ayuda.ShowAll();
		}

		

		
		
	}
}
