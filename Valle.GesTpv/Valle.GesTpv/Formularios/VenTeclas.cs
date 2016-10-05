// VenTeclas.cs created with MonoDevelop
// User: valle at 22:28 26/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;

using Valle.SqlUtilidades;



namespace Valle.GesTpv
{
	
	
	public partial class VenTeclas : Gtk.Window
	{
		enum Modos { Editar, Borrar, Añadir, salir};		
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbSecciones;
		private Modos modo;
		private DataRow drActivo;
		private int ordenTecla = 0;
		private int numTeclasSeccion = 0;
		private AutoResetEvent puedoOrdenar = new AutoResetEvent(true);
		
	
		public VenTeclas(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Teclas","IDTecla");
			this.tbSecciones = gesLocal.ExtraerLaTabla("Secciones",null);
			
			//configurar el aspecto del control
			this.lstTabla.AppendColumn("IDTecla",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.AppendColumn("Nombre articulo",new Gtk.CellRendererText(),"text",2);
			this.lstTabla.Columns[0].Visible = false;
			this.btnAñadirArt.Sensitive = false;
			
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenaSecciones();
			this.MostrarNavegador (false);
		}
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.txtNombre.Sensitive = mostrar;
        }
      
        private void MostrarNavegador(bool mostrar){
            this.vboxNavigador.Visible = mostrar;
        }
        
        private void RellenaSecciones(){
			Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbSecciones.Model = st;
			foreach(DataRow dr in tbSecciones.Rows){
				st.AppendValues(dr["Nombre"].ToString());
			}
			this.cmbSecciones.Active = 0;
        }
        
        private void RellenarTabla(){
    
            DataView dv = new DataView(tbPrincipal,"IDSeccion =" 
                  +tbSecciones.Select("Nombre ='"+this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"].ToString(), 
                                                                     "Orden",DataViewRowState.CurrentRows);
		this.numTeclasSeccion = dv.Count;
         Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String),typeof(DataRow));
			this.lstTabla.Model = st; this.ordenTecla= 0;
		    	foreach(DataRowView dr in dv){
		    	    this.ordenTecla++;
			    	st.AppendValues(dr["IDTecla"].ToString(), dr["Nombre"].ToString()
			    	          ,gesLocal.ExtraerLaTabla("Articulos").Select("IDArticulo = '"+
			    	                 dr["IDArticulo"].ToString()+"'")[0]["Nombre"].ToString(), dr.Row);
		    	}
		    	
		}
	
		        
        private bool CargarReg(){
         bool correcto = false;
         DataRow[] drs = tbPrincipal.Select("Nombre ='" +txtNombre.Text+"' AND IDTecla = "+ 
                          tbSecciones.Select("Nombre ='"+this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"].ToString());
           switch(modo){
            case Modos.Añadir:
              if ((drs.Length<=0)&& (this.txtNombre.Text.Length>0)&&(this.cmbSecciones.ActiveText!=null)){
                   drActivo["Nombre"] = this.txtNombre.Text;
                     drActivo["IDSeccion"] = tbSecciones.Select("Nombre = '"+ this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"];
                       drActivo["Orden"] = this.ordenTecla;
                        correcto = true;
                      }
                      break;
             case Modos.Editar:
                  if ((drs.Length<=1)&& (this.txtNombre.Text.Length>0)&&(cmbSecciones.ActiveText!=null)){
                   drActivo["Nombre"] = this.txtNombre.Text;
                     drActivo["IDSeccion"] = tbSecciones.Select("Nombre = '"+ this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"];
                        correcto = true;
                      }
                      break;
              
                      }
                      return correcto;
                      
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
				          this.txtNombre.Text = "";
				          this.lstTabla.Reorderable = false;
				          this.btnAñadirArt.Sensitive = false;
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
				        this.btnAñadirArt.Sensitive = true;
				        this.txtNombre.Sensitive = false;
				        this.btnAceptar.Visible = false;
				        this.lstTabla.Reorderable = true;
				        this.txtNombre.Text=""; 
				        this.drActivo = tbPrincipal.NewRow();
				        this.lblInformacion.Text = "Modo añadir activado";
				}

				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				     this.modo = Modos.Editar;
				       this.MostrarAceptar(true);
				        // this.MostrarNavegador(true);
				           this.lstTabla.Reorderable = true;
				             this.lblInformacion.Text = "Modo editar activado";   
				}

				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{
				     try{
				         switch(modo){
				           case Modos.Añadir:
				             if(this.CargarReg()){
				              this.tbPrincipal.Rows.Add(drActivo);
				              this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Agregar);
				              this.RellenarTabla();
				              this.txtNombre.Text="";
				              this.drActivo = this.tbPrincipal.NewRow();
				              this.lblInformacion.Text="Modo añadir activado";
				              }else{
				                 this.lblInformacion.Text="Error al introducir los datos";
				              }
				            break;
				           case Modos.Borrar:
				             //gesLocal.ActualizarEnCascadaBorrar(TablaActualizar.Teclas,drActivo["IDTecla"].ToString());
				             this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Teclas","IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.RellenarTabla();
				              this.txtNombre.Text="";
				               this.lblInformacion.Text="Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Modificar);
				             this.RellenarTabla();
				                   this.lblInformacion.Text="Modo editar activado";
				             }else{
				                 this.lblInformacion.Text="Error al introducir los datos";
				              }
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
                              DataRow dr = tbPrincipal.Select("IDTecla = "+model.GetValue(iter, 0).ToString())[0];
                               this.txtNombre.Text = dr["Nombre"].ToString();
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
                              DataRow dr = tbPrincipal.Select("IDTecla = "+model.GetValue(iter, 0).ToString())[0];
                              if((int)dr["Orden"]>0){
                              DataRow drCambio = tbPrincipal.Select("Orden = " + ((int)dr["Orden"]-1))[0];
                                  int aux = (int)dr["Orden"];
                                  dr["Orden"]= (int)drCambio["Orden"];
                                  drCambio["Orden"] = aux ;
                                  this.OnBtnAceptarClicked(null,null);
                                  this.RellenarTabla();
                                   pos = new Gtk.TreePath(dr["Orden"].ToString());
                                  this.lstTabla.Selection.SelectPath(pos);
                                
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
                              DataRow dr = tbPrincipal.Select("IDTecla = "+model.GetValue(iter, 0).ToString())[0];
                              if((int)dr["Orden"]<this.numTeclasSeccion-1){
                              DataRow drCambio = tbPrincipal.Select("Orden = " + ((int)dr["Orden"]+1))[0];
                                  int aux = (int)dr["Orden"];
                                  dr["Orden"]= (int)drCambio["Orden"];
                                  drCambio["Orden"] = aux ;
                                  this.OnBtnAceptarClicked(null,null);
                                  this.RellenarTabla();
                                     pos = new Gtk.TreePath(dr["Orden"].ToString());
                                       this.lstTabla.Selection.SelectPath(pos);
                                       this.lstTabla.ChildVisible= true;
                                  
                                  }
                          }
				     
				}
				}

				protected virtual void OnBtnAñadirSeccionClicked (object sender, System.EventArgs e)
				{
				           VenSecciones ven = new VenSecciones(gesLocal);
				             ven.salirSecciones += new OnSalirSecciones(this.RellenaSecciones);
				              ven.Show();
				}
				private void CargarArticulos(List<DataRow> drs){
				     
				     
				      for(int i=0; i < drs.Count;i++){
				              DataRow r = (DataRow)drs[i];
				              DataRow drNuevo	 =  tbPrincipal.NewRow();
				                 drNuevo["IDArticulo"] = r["IDArticulo"];
				                 drNuevo["Nombre"] = r["Nombre"].ToString();
				                 drNuevo["Orden"] = this.ordenTecla++;
				                 drNuevo["IDSeccion"] =
				                         tbSecciones.Select("Nombre ='"+this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"];
				                tbPrincipal.Rows.Add(drNuevo); 
				               this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+drNuevo["IDTecla"].ToString(),
				                                       AccionesConReg.Agregar);
				                       this.gesLocal.GuardarDatos(drNuevo,"IDTecla = "+drNuevo["IDTecla"].ToString(),
				                                       AccionesConReg.Agregar);
				  
				      }
				      
				    this.RellenarTabla();
				}

				protected virtual void OnBtnAñadirArtClicked (object sender, System.EventArgs e)
				{
				  if(this.cmbSecciones.ActiveText != null){
				    VenElegirArt ven = new VenElegirArt(gesLocal,
				         tbSecciones.Select("Nombre ='"+this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"].ToString()
				                   ,this.CargarArticulos);
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
                              this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+aux["IDTecla"].ToString(),AccionesConReg.Modificar);
                              this.gesLocal.GuardarDatos(aux,"IDTecla = "+aux["IDTecla"].ToString(),AccionesConReg.Modificar);
				                                    
                      while(((Gtk.ListStore)this.lstTabla.Model).IterNext(ref tmpIter)) {
                         aux = ((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(tmpIter,3));
                            aux["Orden"] = pos++;
                                this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+aux["IDTecla"].ToString(),AccionesConReg.Modificar);
                                this.gesLocal.GuardarDatos(aux,"IDTecla = "+aux["IDTecla"].ToString(),AccionesConReg.Modificar);
				       
                      }
                      this.puedoOrdenar.Set();
                  }

                  
			}
}

