// VenMesas.cs created with MonoDevelop
// User: valle at 15:42 26/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
	
	
	public partial class VenMesas : Gtk.Window
	{
		enum Modos { Editar, Borrar, Añadir, salir};		
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbZonas;
		private Modos modo;
		private DataRow drActivo;
		private int ordenMesa = 0;
		private int numMesasZona = 0;
		
	
		public VenMesas(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Mesas","IDMesa");
			this.tbZonas = gesLocal.ExtraerLaTabla("Zonas","IDZona");
			this.lstTabla.AppendColumn("IDMesa",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.Columns[0].Visible = false;
			
			//configurar el aspecto del control
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarZonas();
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
        
        private void RellenarZonas(){
			Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbZonas.Model = st;
			foreach(DataRow dr in tbZonas.Rows){
				st.AppendValues(dr["Nombre"].ToString());
			}
			this.cmbZonas.Active = 0;
        }
        
        private void RellenarTabla(){
    
            DataView dv = new DataView(tbPrincipal,"IDZona =" 
                  +tbZonas.Select("Nombre ='"+this.cmbZonas.ActiveText+"'")[0]["IDZona"].ToString(), 
                                                                     "Orden",DataViewRowState.CurrentRows);
		this.numMesasZona = dv.Count;
         Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String),typeof(DataRow));
			this.lstTabla.Model = st; this.ordenMesa= 0;
		    	foreach(DataRowView dr in dv){
		    	    this.ordenMesa++;
			    	st.AppendValues(dr["IDMesa"].ToString(), dr["Nombre"].ToString(), dr.Row);
		    	}
		    	
		}
		
		        
        private bool CargarReg(){
         bool correcto = false;
         DataRow[] drs = tbPrincipal.Select("Nombre ='" +txtNombre.Text+"' AND IDZona = "+ 
                          tbZonas.Select("Nombre ='"+this.cmbZonas.ActiveText+"'")[0]["IDZona"].ToString());
           switch(modo){
            case Modos.Añadir:
              if ((drs.Length<=0)&& (this.txtNombre.Text.Length>0)&&(this.cmbZonas.ActiveText!=null)){
                   drActivo["Nombre"] = this.txtNombre.Text;
                     drActivo["IDZona"] = tbZonas.Select("Nombre = '"+ this.cmbZonas.ActiveText+"'")[0]["IDZona"];
                       drActivo["Orden"] = this.ordenMesa;
                        correcto = true;
                      }
                      break;
             case Modos.Editar:
                  if ((drs.Length<=1)&& (this.txtNombre.Text.Length>0)&&(this.cmbZonas.ActiveText!=null)){
                   drActivo["Nombre"] = this.txtNombre.Text;
                     drActivo["IDZona"] = tbZonas.Select("Nombre = '"+ this.cmbZonas.ActiveText+"'")[0]["IDZona"];
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
				          this.lstTabla.Reorderable = false;
				          this.lblInformacion.Text= "Modo no editable activado";
				          this.txtNombre.Text = "";
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
				        this.txtNombre.Text="";  this.drActivo = tbPrincipal.NewRow();
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
				              this.gesLocal.ActualizarSincronizar("Mesas","IDMesa = "+drActivo["IDMesa"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDMesa = "+drActivo["IDMesa"].ToString(),AccionesConReg.Agregar);
				              this.RellenarTabla();
				              this.txtNombre.Text="";
				              this.lblInformacion.Text="Modo añadir activado";
				              this.drActivo = this.tbPrincipal.NewRow();
				            
				              }else{
				                 this.lblInformacion.Text="Error al introducir los datos";
				              }
				            break;
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("Mesas","IDMesa = "+drActivo["IDMesa"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Mesas","IDMesa = "+drActivo["IDMesa"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.RellenarTabla();
				              this.txtNombre.Text="";
				               this.lblInformacion.Text="Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Mesas","IDMesa = "+drActivo["IDMesa"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDMesa = "+drActivo["IDMesa"].ToString(),AccionesConReg.Modificar);
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
                              DataRow dr = tbPrincipal.Select("IDMesa = "+model.GetValue(iter, 0).ToString())[0];
                               this.txtNombre.Text = dr["Nombre"].ToString();
                               this.ordenMesa = (int)dr["Orden"];
                               this.drActivo = dr;
                          }
				}

				protected virtual void OnCmbZonasChanged (object sender, System.EventArgs e)
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
                              DataRow dr = tbPrincipal.Select("IDMesa = "+model.GetValue(iter, 0).ToString())[0];
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
                              DataRow dr = tbPrincipal.Select("IDMesa = "+model.GetValue(iter, 0).ToString())[0];
                              if((int)dr["Orden"]<this.numMesasZona-1){
                              DataRow drCambio = tbPrincipal.Select("Orden = " + ((int)dr["Orden"]+1))[0];
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

				protected virtual void OnBtnAñadirZonaClicked (object sender, System.EventArgs e)
				{
				           VenZonas ven = new VenZonas(gesLocal,tbZonas);
				             ven.salirZonas += this.RellenarZonas;
				              ven.Show();
				}

				protected virtual void OnTxtNombreActivated (object sender, System.EventArgs e)
				{
				 this.OnBtnAceptarClicked(null,null);
				}

				protected virtual void OnLstTablaDragEnd (object o, Gtk.DragEndArgs args)
				{
				   
				    int pos = 0;
		             Gtk.TreeIter tmpIter = new Gtk.TreeIter(); 
                     ((Gtk.ListStore)this.lstTabla.Model).GetIterFirst(out tmpIter);
                     DataRow aux = ((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(tmpIter,2));
                          aux["Orden"] = pos++;
                              this.gesLocal.ActualizarSincronizar("Mesas","IDMesa = "+aux["IDMesa"].ToString(),AccionesConReg.Modificar);
                              this.gesLocal.GuardarDatos(aux,"IDMesa = "+aux["IDMesa"].ToString(),AccionesConReg.Modificar);
				                                    
                      while(((Gtk.ListStore)this.lstTabla.Model).IterNext(ref tmpIter)) {
                         aux = ((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(tmpIter,2));
                            aux["Orden"] = pos++;
                                this.gesLocal.ActualizarSincronizar("Mesas","IDMesa = "+aux["IDMesa"].ToString(),AccionesConReg.Modificar);
                                this.gesLocal.GuardarDatos(aux,"IDMesa = "+aux["IDMesa"].ToString(),AccionesConReg.Modificar);
				       
                      }
                         Gtk.TreePath posPath = new Gtk.TreePath(drActivo["Orden"].ToString());
                             this.lstTabla.Selection.SelectPath(posPath);
                              this.lstTabla.SetCursor(posPath,this.lstTabla.Columns[0],false) ;
                 }             

				protected virtual void OnButton131Clicked (object sender, System.EventArgs e)
				{
			          new VenPlanig(this.gesLocal);
			          this.Destroy();
				}

				
	
	}
}
