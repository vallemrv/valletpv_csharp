// VenAñadirTeclas.cs created with MonoDevelop
// User: valle at 20:08 02/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Valle.GesTpv
{
	
	
	public delegate void OnSalirAñadirTeclas(List<DataRow> arts);
	public partial class VenAñadirTeclas : Gtk.Window
	{
	
	    private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbSecciones;
		private DataView tbTeclasFav;
		private String idFavoritos;
		private List<DataRow> drs = new List<DataRow>() ;
		
		public event OnSalirAñadirTeclas salirElegir;
		
		public VenAñadirTeclas(GesBaseLocal gsL, String idFav, OnSalirAñadirTeclas salir) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			gesLocal = gsL;
			idFavoritos = idFav;
			salirElegir+=new OnSalirAñadirTeclas(salir);
			this.tbSecciones = gesLocal.ExtraerLaTabla("Secciones");
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Teclas");
			this.tbTeclasFav = new DataView(gesLocal.ExtraerLaTabla("TeclasFav"),
			                 "IDFavoritos = " +idFav ,"IDTecla", DataViewRowState.CurrentRows);
			
			this.lstTabla.AppendColumn("Añadir",new Gtk.CellRendererToggle(),"active",0);
			this.lstTabla.AppendColumn("IDTecla",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",2);
		    Gtk.ListStore st = new Gtk.ListStore(typeof(bool),typeof(String),typeof(String),typeof(DataRow));
 		    this.lstTabla.Model = st;
		
			
			//configurar el aspecto del control
			this.RellenarSecciones(); 
			this.RellenarTabla();
			
		}
		
		private void RellenarSecciones(){
		 this.cmbFamilias.AppendText ("Todos los articulos");
			foreach(DataRow dr in tbSecciones.Rows){
			 	this.cmbFamilias.AppendText(dr["Nombre"].ToString());
				}
			
			
		}
		
		private void RellenarTabla(){
 		 Gtk.ListStore st =(Gtk.ListStore)lstTabla.Model;
		 st.Clear();
		 bool mostrar = false;
 		 DataView dv;
		
		  if(this.cmbFamilias.ActiveText!=null){
			 dv = new DataView(tbPrincipal,"IDSeccion ="
			                        +tbSecciones.Select("Nombre = '"+this.cmbFamilias.ActiveText+"'")[0]["IDSeccion"].ToString()
			                                     ,"IDTecla",DataViewRowState.CurrentRows);
			                                     }else{
			                                       dv = tbPrincipal.DefaultView;
			                                       }
			                                    
			foreach(DataRowView dr in dv){
			   this.tbTeclasFav.RowFilter ="IDTecla =" + dr["IDTecla"].ToString()+" AND IDFavoritos ="+idFavoritos;
			   if(this.tbTeclasFav.Count <= 0){
			    	st.AppendValues(drs.Contains(dr.Row), dr["IDTecla"].ToString(), dr["Nombre"].ToString(),dr.Row);
				       mostrar = true;
			}
		}
		
		this.btnAceptar.Sensitive = mostrar;
		
	}

		
		protected virtual void OnCmbFamiliasChanged (object sender, System.EventArgs e)
		{
		  if(this.cmbFamilias.ActiveText != null){
		   if(this.cmbFamilias.ActiveText.Equals("Todos los articulos")){
		       this.cmbFamilias.Active = -1;}
		    this.RellenarTabla();
		    }
		}

		protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
		{
		
		             this.Destroy();
		}

	
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{ 
		           this.salirElegir(drs);  
                    drs = new List<DataRow>();
		           this.RellenarTabla(); 
		}


		protected virtual void OnLstTablaRowActivated (object o, Gtk.RowActivatedArgs args)
		{
		       Gtk.TreeModel model;
                    Gtk.TreeIter iter;
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                          bool activado = (bool)model.GetValue(iter,0);
                          if(!activado){
                              drs.Add((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(iter,3));
                             }else{
                              drs.Remove((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(iter,3));
                             }
                             model.SetValue(iter,0,!activado);
            }  
		}
		
	}	
}
