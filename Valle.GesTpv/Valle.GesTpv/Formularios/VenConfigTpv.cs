// VenConfiguracionTpv.cs created with MonoDevelop
// User: valle at 20:15 28/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
	
	
	public partial class VenConfigTpv : Gtk.Window
	{
	  enum Modos { Editar, Borrar, Añadir, salir};		
	
	  private GesBaseLocal gesLocal;
	  private DataTable tbSeccionesTpv;
	  private DataTable tbZonasTpv;
	  private DataTable tbFavoritosTpv;
	  private DataTable tbConfigTpv;
	  private DataTable tbTpv;
	          DataTable tbFavoritos;
	          DataTable tbZonas;
	          DataTable tbMesas;
	          DataTable tbTeclas;
	          DataTable tbTeclasFav;
	          DataTable tbArticulos;
	          DataTable tbSecciones;
	          
	  private int IDTpvEditado;
	  private DataRow rFav;
	  
	  private Modos modo;
	  
		
		public VenConfigTpv(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbConfigTpv = gesLocal.ExtraerLaTabla("Configuracion","IDVinculacion");
			this.tbSeccionesTpv = gesLocal.ExtraerLaTabla("SeccionesTpv","IDVinculacion");
			this.tbZonasTpv = gesLocal.ExtraerLaTabla("ZonasTpv","IDVinculacion");
			this.tbFavoritosTpv = gesLocal.ExtraerLaTabla("FavoritosTpv","IDVinculacion");
	        this.tbTpv = gesLocal.ExtraerLaTabla("TPVs","IDTpv");
	        tbMesas = gesLocal.ExtraerLaTabla("Mesas");
	        tbZonas = gesLocal.ExtraerLaTabla("Zonas");
	        tbTeclas = gesLocal.ExtraerLaTabla("Teclas"); 
	        tbFavoritos = gesLocal.ExtraerLaTabla("Favoritos");
	        tbSecciones = gesLocal.ExtraerLaTabla("Secciones");
	        tbTeclasFav = gesLocal.ExtraerLaTabla("TeclasFav");
	        tbArticulos = gesLocal.ExtraerLaTabla("Articulos");
			
			//Configurar el aspecto de las tablas SIMPLES
			this.tblBaseFav.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			this.tblTpvFav.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			this.tblTpvFav.AppendColumn("Hora muestra",new Gtk.CellRendererText(),"text",1);
			this.tblSeccionesBase.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			this.tblSeccionesTpv.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			this.tblZonasBase.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			this.tblZonasTpv.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			
			//configurar el aspecto de las tablas 
			this.tblMesasZona.AppendColumn("ID mesa",new Gtk.CellRendererText(),"text",0);
			this.tblMesasZona.AppendColumn("Nombre mesa",new Gtk.CellRendererText(),"text",1);
			this.tblTeclasFav.AppendColumn("Nombre tecla",new Gtk.CellRendererText(),"text",0);
			this.tblTeclasFav.AppendColumn("Nombre seccion",new Gtk.CellRendererText(),"text",1);
			this.tblTeclasSec.AppendColumn("Nombre tecla",new Gtk.CellRendererText(),"text",0);
			this.tblTeclasSec.AppendColumn("Nombre articulo",new Gtk.CellRendererText(),"text",1);
			
			
		
		   //configurar el aspecto del control
			this.modo = Modos.salir;
			this.RellenarTpvs();
			this.RellenarTablasBase(0);
			this.RellenarTablasTpv(0);
			this.RellenarConfig();
			this.MostrarAceptar(false);
			
			
		}
		
		private void RellenarConfig(){
		  DataRow[] drsConf = this.tbConfigTpv.Select("IDTpv = " + this.IDTpvEditado.ToString());
		    if (drsConf.Length>0){
		      this.checkImprimirAuto.Active = (bool)drsConf[0]["ImprimirAutomatico"];
	      	   this.checkMesasPrimero.Active = (bool)drsConf[0]["IdentificacionPrimero"];
		         this.txtContraseña.Text = drsConf[0]["Bloqueado"].ToString().Length>0?
		                                    Valle.Seguridad.Encriptar.DescriptarCadena(drsConf[0]["Bloqueado"].ToString()):"";
		          this.txtHoraIniTpv.Hora = drsConf[0]["HoraInicioTpv"].ToString();  
		           this.checkVarios.Active = (bool)drsConf[0]["MostrarVarios"];
		            this.checkVariosConNombre.Active = (bool)drsConf[0]["MostrarVariosConNombre"];
		            this.txtTempControles.Text = drsConf[0]["TiempoFormAc"].ToString();
		            this.txtTempNoAc.Text = drsConf[0]["TiempoFormNoAc"].ToString();
		  }
		
		}
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAñadir.Visible = false;
			 this.btnBorrar.Visible = false;
			 this.btnSalir.Visible = !mostrar;
			 this.btnAceptar.Visible = mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.cmbTpvs.Sensitive = !mostrar;
		   	 this.btnAñadirTpvs.Sensitive = !mostrar;
		   	 this.tableConfig.Sensitive = mostrar;
		   	 this.txtHoraIniFav.Sensitive = mostrar;
		   	 this.btnAceptarHor.Sensitive = mostrar;
		   	 this.btnAñadirFav.Sensitive = mostrar;
		   	 this.btnAñadirZona.Sensitive = mostrar;
		   	 this.btnAñadirSec.Sensitive = mostrar;
		   	 this.btnQuitarFav.Sensitive = mostrar;
		   	 this.btnQuitarSec.Sensitive = mostrar;
		   	 this.btnQuitarZona.Sensitive = mostrar;
        }
        
        private void RellenarTpvs(){
			Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(DataRow));
			this.cmbTpvs.Model = st;
			if(tbTpv.Rows.Count>0){
			foreach(DataRow dr in tbTpv.Rows){
				st.AppendValues(dr["Nombre"].ToString(),dr);
			}
			}else
			    st.AppendValues("No hay tpv creados",null);
			    
			this.cmbTpvs.Active = 0;
        }
        
        private void RellenarTablasBase(int Tabla){
            Gtk.ListStore st;
            
            switch(Tabla){
            case 1:
               st = new Gtk.ListStore(typeof(String),typeof(String));
               this.tblZonasBase.Model = st;
               foreach(DataRow dr in tbZonas.Rows){
                  if(this.tbZonasTpv.Select("IDZona = "+dr["IDZona"].ToString()+
                    "AND IDTpv = "+ this.IDTpvEditado).Length<=0){
				        st.AppendValues(dr["Nombre"].ToString(),dr["IDZona"].ToString());
				    }
			}
			break;
			case 2:
               st = new Gtk.ListStore(typeof(String),typeof(String));
               this.tblSeccionesBase.Model = st;
               foreach(DataRow dr in tbSecciones.Rows){
                  if(this.tbSeccionesTpv.Select("IDSeccion = "+dr["IDSeccion"].ToString()+
                         "AND IDTpv = "+ this.IDTpvEditado).Length<=0){
				        st.AppendValues(dr["Nombre"].ToString(),dr["IDSeccion"].ToString());
				    }
			}
			break;
			case 3:
			   st = new Gtk.ListStore(typeof(String),typeof(String));
               this.tblBaseFav.Model = st;
               foreach(DataRow dr in tbFavoritos.Rows){
                  if(this.tbFavoritosTpv.Select("IDFavoritos = "+dr["IDFavoritos"].ToString()+
                   "AND IDTpv = "+ this.IDTpvEditado ).Length<=0){
				        st.AppendValues(dr["Nombre"].ToString(),dr["IDFavoritos"].ToString());
				    }
			}
			break;
			default:
			   this.RellenarTablasBase(1);this.RellenarTablasBase(2);this.RellenarTablasBase(3);
			   break;
         }
        }
        
        private void RellenarTablasTpv(int Tabla){
            Gtk.ListStore st;
            DataTable tbAux;
            DataView dwAux;
            switch(Tabla){
               case 1:
               st = new Gtk.ListStore(typeof(String),typeof(String));
               this.tblZonasTpv.Model = st;
               tbAux = tbZonas;
               dwAux = new DataView(tbZonasTpv,"IDTpv ="+this.IDTpvEditado.ToString(),
                                                "",DataViewRowState.CurrentRows);
               foreach(DataRowView dr in dwAux){
                 DataRow[] drsZona = tbAux.Select("IDZona = "+dr["IDZona"].ToString());
                  if(drsZona.Length>0){
				        st.AppendValues(drsZona[0]["Nombre"].ToString(),dr["IDZona"].ToString());
				    }
			}
			break;
			case 2:
               st = new Gtk.ListStore(typeof(String),typeof(String));
               this.tblSeccionesTpv.Model = st;
               tbAux = tbSecciones;
                     dwAux = new DataView(this.tbSeccionesTpv,"IDTpv ="+this.IDTpvEditado.ToString(),
                                                "",DataViewRowState.CurrentRows);
         
               foreach(DataRowView dr in dwAux){
                DataRow[] drsSec = tbAux.Select("IDSeccion = "+dr["IDSeccion"].ToString());
                  if(drsSec.Length>0){
				        st.AppendValues(drsSec[0]["Nombre"].ToString(),dr["IDSeccion"].ToString());
				    }
			}
			break;
			case 3:
			   st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String));
               this.tblTpvFav.Model = st;
               tbAux = tbFavoritos;
               dwAux = new DataView(this.tbFavoritosTpv,"IDTpv ="+this.IDTpvEditado.ToString(),
                                                "",DataViewRowState.CurrentRows);
         
               foreach(DataRowView dr in dwAux){
                 DataRow[] drsFav = tbAux.Select("IDFavoritos = "+dr["IDFavoritos"].ToString()); 
                  if(drsFav.Length>0){
				        st.AppendValues(drsFav[0]["Nombre"].ToString(),dr["HoraInicioFav"].ToString(),dr["IDFavoritos"].ToString());
				    }
			}
			break;
			default:
			  this.RellenarTablasTpv(1);this.RellenarTablasTpv(2);this.RellenarTablasTpv(3);
            break;
            }
        }
                
                protected virtual void OnBtnAñadirTpvsClicked (object sender, System.EventArgs e)
				{
				     VenTpvs ven = new VenTpvs(gesLocal,tbTpv);
				       ven.salirTpv += new OnSalirTpv(this.RellenarTpvs);
				         ven.Show();
				}

        
                protected virtual void OnCmbTpvsChanged (object sender, System.EventArgs e)
				{
				   this.IDTpvEditado = (int)tbTpv.Select("Nombre ='"+this.cmbTpvs.ActiveText+"'")[0]["IDTpv"];
				   this.RellenarTablasBase(0);this.RellenarTablasTpv(0);this.RellenarConfig();
				}


				protected virtual void OnBtnQuitarZonaClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblZonasTpv.Selection.GetSelected(out model, out iter)) {
			             DataRow r = this.tbZonasTpv.Select("IDZona = "+model.GetValue(iter,1).ToString()+
			                                             " AND IDTpv = "+this.IDTpvEditado)[0];
			             gesLocal.ActualizarSincronizar("ZonasTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Borrar);
			             this.gesLocal.GuardarDatos("ZonasTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Borrar);
			             r.Delete();
				         this.RellenarTablasBase(1);this.RellenarTablasTpv(1);
		             }
		 		}

				protected virtual void OnBtnAñadirZonaClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblZonasBase.Selection.GetSelected(out model, out iter)) {
			            DataRow r = tbZonasTpv.NewRow();
			             r["IDTpv"] = this.IDTpvEditado;
			             r["IDZona"] = model.GetValue(iter,1).ToString();
				         tbZonasTpv.Rows.Add(r);
				         gesLocal.ActualizarSincronizar("ZonasTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				         this.gesLocal.GuardarDatos(r,"IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				         this.RellenarTablasBase(1);this.RellenarTablasTpv(1);
		             }
				     
				}

				protected virtual void OnTblZonasTpvCursorChanged (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (tblZonasTpv.Selection.GetSelected(out model, out iter)) {
			            DataRow[] rs = tbMesas.Select("IDZona = "+model.GetValue(iter,1).ToString());
			            Gtk.ListStore st = new Gtk.ListStore(typeof(string),typeof(string));
			             this.tblMesasZona.Model = st;
			             foreach(DataRow r in rs){
			                st.AppendValues(r["IDMesa"].ToString(), r["Nombre"].ToString());
			             }
		             }
				        
				    
				}

			
				
				protected virtual void OnBtnAñadirSecClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblSeccionesBase.Selection.GetSelected(out model, out iter)) {
			            DataRow r = tbSeccionesTpv.NewRow();
			             r["IDTpv"] = this.IDTpvEditado;
			             r["IDSeccion"] = model.GetValue(iter,1).ToString();
				         tbSeccionesTpv.Rows.Add(r);
				         gesLocal.ActualizarSincronizar("SeccionesTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				         this.gesLocal.GuardarDatos(r,"IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				         this.RellenarTablasBase(2);this.RellenarTablasTpv(2);
		             }
				}

				protected virtual void OnBtnQuitarSecClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblSeccionesTpv.Selection.GetSelected(out model, out iter)) {
                      string idSeccion = model.GetValue(iter,1).ToString();
                      if(!this.PodemosQuitarFav(idSeccion)){
                         Gtk.MessageDialog DlgNoPodemos =   new Gtk.MessageDialog(this,Gtk.DialogFlags.Modal,Gtk.MessageType.Info,Gtk.ButtonsType.Ok,
	               	                        "No podemos quitar esta seccion porque hay algun\nteclado de favoritos que hace referencia");
	               	        DlgNoPodemos.Run ();
                            DlgNoPodemos.Destroy();
	               	
	               	    }else{
			             DataRow r = this.tbSeccionesTpv.Select("IDSeccion = "+idSeccion+" AND IDTpv = "+this.IDTpvEditado)[0];
			             gesLocal.ActualizarSincronizar("SeccionesTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Borrar);
			             this.gesLocal.GuardarDatos("SeccionesTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Borrar);
			             r.Delete();
				         this.RellenarTablasBase(2);this.RellenarTablasTpv(2);
		             }
		  	       }
				}

				protected virtual void OnTblSeccionesTpvCursorChanged (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblSeccionesTpv.Selection.GetSelected(out model, out iter)) {
			            DataRow[] rs = tbTeclas.Select("IDSeccion = "+model.GetValue(iter,1).ToString());
			            Gtk.ListStore st = new Gtk.ListStore(typeof(string),typeof(string));
			             this.tblTeclasSec.Model = st;
			             foreach(DataRow r in rs){
			                st.AppendValues(r["Nombre"].ToString(), tbArticulos.Select(
			                                          "IDArticulo = '"+ r["IDArticulo"].ToString()+"'")[0]["Nombre"].ToString());
			             }
		             }
				}
				
				bool PodemosAgregarFav(string idFav){
				                                         
				   DataTable tb = gesLocal.EjConsultaSelect("PuedeFav","Select DISTINCT Secciones.IDSeccion From TeclasFav INNER JOIN "+
				                                          "Teclas ON TeclasFav.IDTecla = Teclas.IDTecla INNER JOIN "+
				                                          "Secciones ON Teclas.IDSeccion = Secciones.IDSeccion "+
				                                          "WHERE TeclasFav.IDFavoritos = "+idFav,"TeclasFav","FavoritosTpv","Teclas","Secciones");
				   bool esta = true;
				      foreach(DataRow r in tb.Rows){
				         if(this.tbSeccionesTpv.Select("IDSeccion ="+r["IDSeccion"].ToString()+
				                                       " AND IDTpv ="+this.IDTpvEditado.ToString()).Length<=0) return false;
				      }
				   return esta;  
				}
				
				bool PodemosQuitarFav(string idSeccion){
				   DataTable tb = gesLocal.EjConsultaSelect("PuedeFav","Select DISTINCT Secciones.IDSeccion From TeclasFav INNER JOIN "+
				                                          "Teclas ON TeclasFav.IDTecla = Teclas.IDTecla INNER JOIN "+
				                                          "Secciones ON Teclas.IDSeccion = Secciones.IDSeccion INNER JOIN FavoritosTpv ON "+
				                                          "FavoritosTpv.IDFavoritos = TeclasFav.IDFavoritos "+
				                                          "WHERE FavoritosTpv.IDTpv = "+IDTpvEditado,"TeclasFav","FavoritosTpv","Teclas","Secciones");
				   bool esta = true;
				   
				         if(tb.Select("IDSeccion ="+idSeccion).Length>0) return false;
				      
				   return esta;  
				}
				
				

				protected virtual void OnBtnAñadirFavClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblBaseFav.Selection.GetSelected(out model, out iter)) {
	               	   int idFav = (int)model.GetValue(iter,1);
	               	   if(!PodemosAgregarFav(idFav.ToString())){
	               	     Gtk.MessageDialog DlgNoPodemos = new Gtk.MessageDialog(this,Gtk.DialogFlags.Modal,Gtk.MessageType.Info,Gtk.ButtonsType.Ok,
	               	                        "No podemos agregar este favoritos\nsin antes agregar las secciones que hace referencia");
	               	              DlgNoPodemos.Run ();
                            DlgNoPodemos.Destroy();           
	               	   }else{
	                     rFav = this.tbFavoritosTpv.NewRow();
			             rFav["IDTpv"] = this.IDTpvEditado;
			             rFav["IDFavoritos"] = idFav;
				         try{
    				         rFav["HoraInicioFav"] = "00:00";
    				         this.tbFavoritosTpv.Rows.Add(rFav);
    				         gesLocal.ActualizarSincronizar("FavoritosTpv","IDVinculacion = "+rFav["IDVinculacion"].ToString(),AccionesConReg.Agregar);
    				         this.gesLocal.GuardarDatos(rFav,"IDVinculacion = "+rFav["IDVinculacion"].ToString(),AccionesConReg.Agregar);
    				         this.RellenarTablasBase(3);this.RellenarTablasTpv(3);
    				        }catch(Exception ex){
				          this.lblInformacion.Text = "Error : "+ ex.Message;
				        }
				        
				     }
				    }
				         
				}

				protected virtual void OnBtnQuitarFavClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblTpvFav.Selection.GetSelected(out model, out iter)) {
			             DataRow r = this.tbFavoritosTpv.Select("IDFavoritos = "+model.GetValue(iter,2).ToString()+
			                                             " AND IDTpv = "+this.IDTpvEditado)[0];
			             gesLocal.ActualizarSincronizar("FavoritosTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Borrar);
			             this.gesLocal.GuardarDatos("FavoritosTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Borrar);
			             r.Delete();
				         this.RellenarTablasBase(3);this.RellenarTablasTpv(3);
		             }
		 		}
				

				protected virtual void OnBtnAceptarHorClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
	               	if (this.tblTpvFav.Selection.GetSelected(out model, out iter)) {
			             DataRow r = this.tbFavoritosTpv.Select("IDFavoritos = "+model.GetValue(iter,2).ToString()+
			                                             " AND IDTpv = "+this.IDTpvEditado)[0];
			             r["HoraInicioFav"] = this.txtHoraIniFav.Hora;
			             gesLocal.ActualizarSincronizar("FavoritosTpv","IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Modificar);
			             this.gesLocal.GuardarDatos(r,"IDVinculacion = "+r["IDVinculacion"].ToString(),AccionesConReg.Modificar);
			             this.RellenarTablasBase(3);this.RellenarTablasTpv(3);
		             }
				     
				}

				protected virtual void OnTblTpvFavCursorChanged (object sender, System.EventArgs e)
				{
				
				    Gtk.TreeModel model;
		            Gtk.TreeIter iter;
		           	if (tblTpvFav.Selection.GetSelected(out model, out iter)) {
		           	    DataRow rFavAct = this.tbFavoritosTpv.Select("IDFavoritos = "+model.GetValue(iter,2).ToString()+
			                                             " AND IDTpv = "+this.IDTpvEditado)[0];
			                                             
			             this.txtHoraIniFav.Hora = rFavAct["HoraInicioFav"].ToString();
			             
			            DataRow[] rs = tbTeclasFav.Select("IDFavoritos = "+model.GetValue(iter,2).ToString());
			            Gtk.ListStore st = new Gtk.ListStore(typeof(string),typeof(string));
			             this.tblTeclasFav.Model = st;
			             foreach(DataRow r in rs){
			               DataRow drTecla = tbTeclas.Select(
			                                          "IDTecla = "+ r["IDTecla"].ToString())[0];
			                   DataRow drSec = tbSecciones.Select(
			                                          "IDSeccion = "+  drTecla["IDSeccion"].ToString())[0];
			                                          
			                st.AppendValues(drTecla["Nombre"].ToString(),drSec["Nombre"].ToString());
			             }
		            }
				}

				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				    this.modo = Modos.Editar;
		     	       this.MostrarAceptar(true);
				             this.lblInformacion.Text = "Modo editar activado";  
				}

		
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				          this.Destroy();
				}


		     void cargarConfiguracion(){
        		  DataRow[] drs = this.tbConfigTpv.Select("IDTpv = " + this.IDTpvEditado.ToString());
        		     if(drs.Length>0){
        		         drs[0]["ImprimirAutomatico"] = this.checkImprimirAuto.Active;
        		         drs[0]["IdentificacionPrimero"] = this.checkMesasPrimero.Active;
        		         drs[0]["MostrarVarios"] = this.checkVarios.Active;
        		         drs[0]["MostrarVariosConNombre"] = this.checkVariosConNombre.Active;
        		         drs[0]["Bloqueado"] = (this.txtContraseña.Text.Length>0) ?
        		                                    Valle.Seguridad.Encriptar.EncriptarCadena(this.txtContraseña.Text):"";
        		         drs[0]["HoraInicioTpv"] = this.txtHoraIniTpv.Hora;
        		         drs[0]["IDTpv"] = this.IDTpvEditado;
        		         drs[0]["TiempoFormNoAc"] = this.txtTempNoAc.Text;
        		         drs[0]["TiempoFormAc"] = this.txtTempControles.Text;
        		         gesLocal.ActualizarSincronizar("Configuracion","IDVinculacion = "+drs[0]["IDVinculacion"].ToString(),AccionesConReg.Modificar);
        		         this.gesLocal.GuardarDatos(drs[0],"IDVinculacion = "+drs[0]["IDVinculacion"].ToString(),AccionesConReg.Modificar);
        		         
        		     }else{
        		        DataRow rconf = this.tbConfigTpv.NewRow();
        		          rconf["IDTpv"] = this.IDTpvEditado;
        		          rconf["activo"]= false;
        		          rconf["ImprimirAutomatico"] = this.checkImprimirAuto.Active;
        		          rconf["IdentificacionPrimero"] = this.checkMesasPrimero.Active;
        		          rconf["MostrarVarios"] = this.checkVarios.Active;
        		          rconf["MostrarVariosConNombre"] = this.checkVariosConNombre.Active;
        		          rconf["Bloqueado"] = this.txtContraseña.Text.Length>0?
        		                                    Valle.Seguridad.Encriptar.EncriptarCadena(this.txtContraseña.Text):"";
        		          rconf["HoraInicioTpv"] = this.txtHoraIniTpv.Hora;
        		          rconf["TiempoFormNoAc"] = this.txtTempNoAc.Text;
        		          rconf["TiempoFormAc"] = this.txtTempControles.Text;
        		        this.tbConfigTpv.Rows.Add(rconf);
        		        gesLocal.ActualizarSincronizar("Configuracion","IDVinculacion = "+rconf["IDVinculacion"].ToString(),AccionesConReg.Agregar);
        		        this.gesLocal.GuardarDatos(rconf,"IDVinculacion = "+rconf["IDVinculacion"].ToString(),AccionesConReg.Agregar);
        		     }
        		
		}

		protected virtual void OnTxtHoraVerFavActivated (object sender, System.EventArgs e)
		{
		   this.OnBtnAceptarHorClicked(null,null);
		}

		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
		  if(modo == Modos.salir)
		      this.OnBtnSalirClicked(null,null);
		      else
		       this.cargarConfiguracion();
		}

		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
		          this.cargarConfiguracion();
	              this.modo = Modos.salir;
		          this.MostrarAceptar(false);
		          this.RellenarConfig();
		          this.lblInformacion.Text= "Modo no editable activado";
			
		}

		
		
	}
}
