// VenElegirArt.cs created with MonoDevelop
// User: valle at 14:32 27/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Valle.GesTpv
{
	
	
	public delegate void OnSalirElegirArt(List<DataRow> arts);
	public partial class VenElegirArt : Gtk.Window
	{
	
	    private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbFamilias;
		private DataView tbTeclas;
		private bool mostrarTodos;
		private String idSeccion;
		private List<DataRow> arts = new List<DataRow>();
		
		public event OnSalirElegirArt salirElegir;
		
		public VenElegirArt(GesBaseLocal gsL, String idSec, OnSalirElegirArt salir) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			gesLocal = gsL;
			idSeccion = idSec;
			salirElegir+=new OnSalirElegirArt(salir);
			this.tbFamilias = gesLocal.ExtraerLaTabla("Familias");
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Articulos");
			this.tbTeclas = new DataView(gesLocal.ExtraerLaTabla("Teclas"),"","IDArticulo",
			                                        DataViewRowState.CurrentRows);
			
			this.lstTabla.AppendColumn("Añadir",new Gtk.CellRendererToggle(),"active",0);
			this.lstTabla.AppendColumn("IDArticulo",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",2);
			
			//configurar el aspecto del control
			this.RellenarFamilias();
			this.mostrarTodos = true;
			this.RellenarTabla(this.mostrarTodos);
			
		}
		
		private void RellenarFamilias(){
		 this.cmbFamilias.AppendText ("Todos los articulos");
			foreach(DataRow dr in tbFamilias.Rows){
			 	this.cmbFamilias.AppendText(dr["Nombre"].ToString());
				}
			
			
		}
		
		private void RellenarTabla(bool mostrartodas){
 		 Gtk.ListStore st = new Gtk.ListStore(typeof(bool),typeof(String),typeof(String),typeof(DataRow));
 		 DataView dv;
		 bool mostrar = false;                                    
		 String filtro;
		
		  this.lstTabla.Model = st;
	      if (mostrartodas){
			    filtro ="IDSeccion ="+ idSeccion +" AND IDArticulo = '";
			 }else{
			   filtro = "IDArticulo = '";
			 }
		
			 if(this.cmbFamilias.ActiveText!=null){
			           dv = new DataView(tbPrincipal,"IDFamilia = '"
			                        +tbFamilias.Select("Nombre = '"+this.cmbFamilias.ActiveText+"'")[0]["IDFamilia"].ToString()+"'"
			                                     ,"IDArticulo",DataViewRowState.CurrentRows);
			                                     }else{
			                                       dv = tbPrincipal.DefaultView;
			                                       }
			foreach(DataRowView dr in dv){
			   this.tbTeclas.RowFilter = filtro + dr["IDArticulo"].ToString()+"'";
			   if(this.tbTeclas.Count <= 0){
			    	st.AppendValues(arts.Contains(dr.Row), dr["IDArticulo"].ToString(), dr["Nombre"].ToString(),dr.Row);
			    	mostrar = true;
			}
             			
		}
		
		 this.btnAceptar.Sensitive = mostrar;
		
	}

		protected virtual void OnChkNoUtilizadosClicked (object sender, System.EventArgs e)
		{
		     this.mostrarTodos = !this.chkNoUtilizados.Active;
		     this.RellenarTabla(mostrarTodos);
		}

		protected virtual void OnCmbFamiliasChanged (object sender, System.EventArgs e)
		{
		  if(this.cmbFamilias.ActiveText != null){
		   if(this.cmbFamilias.ActiveText.Equals("Todos los articulos")){
		       this.cmbFamilias.Active = -1;}
		    this.RellenarTabla(mostrarTodos);
		    }
		}

		protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
		{
		             this.Destroy();
        }

	
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
		           this.salirElegir(arts);  
                   arts = new List<DataRow>();
		           this.RellenarTabla(this.mostrarTodos); 
		}


		protected virtual void OnLstTablaRowActivated (object o, Gtk.RowActivatedArgs args)
		{
                    Gtk.TreeModel model;
                    Gtk.TreeIter iter;
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                          bool activado = (bool)model.GetValue(iter,0);
                          if(!activado){
                              arts.Add((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(iter,3));
                             }else{
                              arts.Remove((DataRow)((Gtk.ListStore)this.lstTabla.Model).GetValue(iter,3));
                             }
                             model.SetValue(iter,0,!activado);
                          }
		                     
		}
		
	}	
}
