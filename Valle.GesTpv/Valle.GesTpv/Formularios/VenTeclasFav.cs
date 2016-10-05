// VenTeclasFav.cs created with MonoDevelop
// User: valle at 19:19 02/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Valle.SqlUtilidades;


namespace Valle.GesTpv
{
	
	
	public partial class VenTeclasFav : Gtk.Window
	{
		enum Modos { Editar, Borrar, Añadir, salir};		
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbFavortios;
		private Modos modo;
		private DataRow drActivo;
		private int ordenTecla = 0;
		private int numTeclasFav = 0;
		private AutoResetEvent puedoOrdenar = new AutoResetEvent(true);
		
	
		public VenTeclasFav(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("TeclasFav","IDVinculacion");
			this.tbFavortios = gesLocal.ExtraerLaTabla("Favoritos",null);
			
			//configurar el aspecto del control
			this.lstTabla.AppendColumn("IDVinculacion",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre tecla",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.AppendColumn("Seccion",new Gtk.CellRendererText(),"text",2);
			this.lstTabla.Columns[0].Visible = false;
			this.btnAñadirTeclas.Sensitive = false;
			
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarNombreFav();
			this.MostrarNavegador (false);
		}
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
	    }
      
        private void MostrarNavegador(bool mostrar){
            this.vboxNavigador.Visible = mostrar;
        }
        
        private void RellenarNombreFav(){
			Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbFavoritos.Model = st;
			foreach(DataRow dr in tbFavortios.Rows){
				st.AppendValues(dr["Nombre"].ToString());
			}
			this.cmbFavoritos.Active = 0;
        }
        
        private void RellenarTabla(){
        if(this.cmbFavoritos.ActiveText!=null){
            DataTable tbTeclas = this.gesLocal.ExtraerLaTabla("Teclas",null);
            DataView dv = new DataView(tbPrincipal,"IDFavoritos =" 
                  +tbFavortios.Select("Nombre ='"+this.cmbFavoritos.ActiveText+"'")[0]["IDFavoritos"].ToString(), 
                                                                     "Orden",DataViewRowState.CurrentRows);
		this.numTeclasFav = dv.Count;
         Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String),typeof(DataRow));
			this.lstTabla.Model = st; this.ordenTecla= 0;
		    	foreach(DataRowView dr in dv){
		    	    this.ordenTecla++;
		    	    DataRow[] drsTecla = tbTeclas.Select("IDTecla ="+dr["IDTecla"].ToString());
			    	st.AppendValues(dr["IDVinculacion"].ToString(), drsTecla[0]["Nombre"].ToString(),
			    	          gesLocal.ExtraerLaTabla("Secciones",null).Select("IDSeccion = "+ drsTecla[0]["IDSeccion"].ToString())[0]["Nombre"].ToString()
			    	           ,dr.Row);
		    	}
		    	}
		}
		
		        
        
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if(modo == Modos.salir){
				          this.Destroy();
				          }else{
				          this.modo = Modos.salir;
				          this.MostrarAceptar(false);
				          this.MostrarNavegador(false);
				          this.lblInformacion.Text= "Modo no editable activado";
				          this.btnAñadirTeclas.Sensitive = false;
				          this.lstTabla.Reorderable = false;
				          this.RellenarTabla();
				          }
				}

				protected virtual void OnBtnBorrarClicked (object sender, System.EventArgs e)
				{
				    this.modo = Modos.Borrar;
				       this.MostrarAceptar(true);
				           this.lblInformacion.Text = "Modo borrar activado";
				}

				protected virtual void OnBtnAñadirClicked (object sender, System.EventArgs e)
				{
				  this.modo = Modos.Añadir;
				        this.MostrarAceptar(true);
				        this.btnAñadirTeclas.Sensitive = true;
				        this.btnAceptar.Visible = false;
				        this.lstTabla.Reorderable = true;
				        this.lblInformacion.Text = "Modo añadir activado";
				}

				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				     this.modo = Modos.Editar;
				       this.MostrarAceptar(true);
				         //this.MostrarNavegador(true);
				           this.btnAceptar.Visible = false;
				             this.lstTabla.Reorderable = true;
				             this.lblInformacion.Text = "Modo editar activado";   
				}

				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{
				     try{
				         switch(modo){
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("TeclasFav","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("TeclasFav","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.RellenarTabla();
				              this.lblInformacion.Text="Modo borrar activado";
				            break;
				            case Modos.Editar:
				             this.gesLocal.ActualizarSincronizar("TeclasFav","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				             this.RellenarTabla();
				               this.lblInformacion.Text="Modo editar activado";
				             break;
				        
				          }
				        }catch(Exception ex){
				          this.lblInformacion.Text = "Error: "+ ex.Message; 
				        }
				        
				}

			
				protected virtual void OnLstTablaCursorChanged (object sender, System.EventArgs e)
				{
			        Gtk.TreeModel model;
                    Gtk.TreeIter iter;
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                              DataRow dr = tbPrincipal.Select("IDVinculacion = "+model.GetValue(iter, 0).ToString())[0];
                               this.ordenTecla = (int)dr["Orden"];
                               this.drActivo = dr;
                          }
				}

				protected virtual void OnCmbSeccionesChanged (object sender, System.EventArgs e)
				{
				      this.RellenarTabla();
				}

				protected virtual void OnBtnSubirClicked (object sender, System.EventArgs e)
				{
				    Gtk.TreeModel model;
                    Gtk.TreeIter iter;
                    Gtk.TreePath pos;
                    if(modo == Modos.Editar){
                             
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                              DataRow dr = tbPrincipal.Select("IDVinculacion = "+model.GetValue(iter, 0).ToString())[0];
                              if((int)dr["Orden"]>0){
                              DataRow drCambio = tbPrincipal.Select("Orden = " + ((int)dr["Orden"]-1))[0];
                                  int aux = (int)dr["Orden"];
                                  dr["Orden"]= (int)drCambio["Orden"];
                                  drCambio["Orden"] = aux ;
                                  this.OnBtnAceptarClicked(null,null);
                                  this.RellenarTabla();
                                   pos = new Gtk.TreePath(dr["Orden"].ToString());
                                  this.lstTabla.Selection.SelectPath(pos);
                                   this.lstTabla.SetCursor(pos,this.lstTabla.Columns[0],false) ;
                               
                                     
                          }
				     }
				}
				}

				protected virtual void OnBtnBajarClicked (object sender, System.EventArgs e)
				{
				   
				    Gtk.TreeModel model;
                    Gtk.TreeIter iter;
                    Gtk.TreePath pos;
                    if(modo == Modos.Editar){
                             
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                              DataRow dr = tbPrincipal.Select("IDVinculacion = "+model.GetValue(iter, 0).ToString())[0];
                              if((int)dr["Orden"]<this.numTeclasFav-1){
                              DataRow drCambio = tbPrincipal.Select("Orden = " + ((int)dr["Orden"]+1))[0];
                                  int aux = (int)dr["Orden"];
                                  dr["Orden"]= (int)drCambio["Orden"];
                                  drCambio["Orden"] = aux ;
                                  this.OnBtnAceptarClicked(null,null);
                                  this.RellenarTabla();
                                     pos = new Gtk.TreePath(dr["Orden"].ToString());
                                  this.lstTabla.Selection.SelectPath(pos);
                                   this.lstTabla.SetCursor(pos,this.lstTabla.Columns[0],false) ;
                                       
                                    
                                 }
                          }
				     
				}
				}

				protected virtual void OnBtnAñadirSeccionClicked (object sender, System.EventArgs e)
				{
				           VenFavoritos ven = new VenFavoritos(gesLocal);
				             ven.SalirFav += new OnSalirFav(this.RellenarNombreFav);
				              ven.Show();
				}
				private void CargarTeclas(List<DataRow> drs){
				     
				     
				      for(int i=0; i < drs.Count;i++){
				              DataRow r = drs[i];
				              DataRow drNuevo	 =  tbPrincipal.NewRow();
				                 drNuevo["IDTecla"] = r["IDTecla"];
				                 drNuevo["Orden"] = this.ordenTecla++;
				                 drNuevo["IDFavoritos"] =
				                         tbFavortios.Select("Nombre ='"+this.cmbFavoritos.ActiveText+"'")[0]["IDFavoritos"];
				                tbPrincipal.Rows.Add(drNuevo); 
				         this.gesLocal.ActualizarSincronizar("TeclasFav","IDVinculacion ="+drNuevo["IDVinculacion"].ToString(),
				                                       AccionesConReg.Agregar);
				            this.gesLocal.GuardarDatos(drNuevo,"IDVinculacion ="+drNuevo["IDVinculacion"].ToString(),
				                                       AccionesConReg.Agregar);
				      }
				    this.RellenarTabla();
				}

				protected virtual void OnBtnAñadirArtClicked (object sender, System.EventArgs e)
				{
				  if(this.cmbFavoritos.ActiveText != null){
				    VenAñadirTeclas ven = new VenAñadirTeclas(gesLocal,
				         tbFavortios.Select("Nombre ='"+this.cmbFavoritos.ActiveText+"'")[0]["IDFavoritos"].ToString()
				                   ,this.CargarTeclas);
				        ven.Show();
				    }
				                    
				}

				protected virtual void OnLstTablaDragEnd (object o, Gtk.DragEndArgs args)
				{
				  Thread hOrdenar = new Thread(OrdenarLista);
				     hOrdenar.Start();
                }
                  
                  private void OrdenarLista()
                  {
                    this.puedoOrdenar.WaitOne();
				    int pos = 0;
		             Gtk.TreeIter tmpIter = new Gtk.TreeIter(); 
                     ((Gtk.ListStore)this.lstTabla.Model).GetIterFirst(out tmpIter);
                     DataRow aux = ((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(tmpIter,3));
                          aux["Orden"] = pos++;
                              this.gesLocal.ActualizarSincronizar("TeclasFav","IDVinculacion = "+aux["IDVinculacion"].ToString(),AccionesConReg.Modificar);
                              this.gesLocal.GuardarDatos(aux,"IDVinculacion = "+aux["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				                                    
                      while(((Gtk.ListStore)this.lstTabla.Model).IterNext(ref tmpIter)) {
                         aux = ((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(tmpIter,3));
                            aux["Orden"] = pos++;
                                this.gesLocal.ActualizarSincronizar("TeclasFav","IDVinculacion = "+aux["IDVinculacion"].ToString(),AccionesConReg.Modificar);
                                this.gesLocal.GuardarDatos(aux,"IDVinculacion = "+aux["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				       
                      }
                         this.puedoOrdenar.Set();
                  
                  }

			}
}

