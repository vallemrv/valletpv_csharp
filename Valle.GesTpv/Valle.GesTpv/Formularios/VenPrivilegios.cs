// VenPrivilejios.cs created with MonoDevelop
// User: valle at 18:09 01/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{public delegate void OnSalirPrivilegios();
	
	
	public partial class VenPrivilegios : Gtk.Window
	{
		 enum Modos { Editar, Borrar, Añadir, salir};		
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbControles;
		private Modos modo;
		
		public event OnSalirPrivilegios SalirPrivilegios;
	
		
		public VenPrivilegios(GesBaseLocal gsL, String nombreUser) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			SalirPrivilegios = null;
			this.tbControles = gesLocal.ExtraerLaTabla("Controles",null);
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Privilegios","IDVinculacion");
			this.lstTabla.AppendColumn("IDBotonControl",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Activo",new Gtk.CellRendererToggle(),"active",1);
		    this.lstTabla.AppendColumn("Descripcion",new Gtk.CellRendererText(),"text",2);
			this.lstTabla.AppendColumn("Nombre Formulario",new Gtk.CellRendererText(),"text",3);
			this.lstTabla.Columns[0].Visible = false;
			
			//configurar el aspecto del control
			this.RellenarControles(); 
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarTabla();
			if(nombreUser != null){
			   this.paraUnUsuario = true;
			   this.cmbUser.AppendText(nombreUser);
			   this.cmbUser.Active=0;
			 }else{
			     this.paraUnUsuario = false;
			     this.RellenarUsuarios();
			 }
		}
		
		private void RellenarUsuarios(){
		    Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbUser.Model = st;
	    	  DataTable tbSuper = gesLocal.ExtraerLaTabla("SuperUsuario",null);
	
			foreach(DataRow dr in gesLocal.ExtraerLaTabla("Usuarios",null).Rows){
			 	    if(tbSuper.Select("IDUsuario = "+ dr["IDUsuario"].ToString()).Length<=0){
		            	st.AppendValues(dr["NombreUsuario"].ToString());
		            	}
			}
			
		}
		
		private void RellenarControles(){
			Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbFormularios.Model = st;
			 st.AppendValues("Todos los controles");
			foreach(DataRow dr in tbControles.Rows){
				     st.AppendValues(dr["NombreForm"].ToString());
			}
		  	
		}
		
		
			private void RellenarTabla(){
			
		    	if(this.cmbUser.ActiveText != null){
		          Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(bool),typeof(String),typeof(String));
	         	   this.lstTabla.Model = st;
		           String filtro;
			        if (this.cmbFormularios.ActiveText != null){
			              filtro ="NombreForm = '"+ this.cmbFormularios.ActiveText+"'" ;
			         }else{
			           filtro = "NombreForm = '*'";
			         }
			         
			   DataView dv;
			  		 dv = new DataView(tbControles,filtro,"IDVinculacion",DataViewRowState.CurrentRows);
		                                        
			foreach(DataRowView dr in dv){
			   DataRow[] drs = this.tbPrincipal.Select("IDBotonControl = '" +dr["IDBotonControl"]+"' AND "+ 
			                            "IDUsuario ="+ gesLocal.ExtraerLaTabla("Usuarios",null).Select("NombreUsuario ='"+
			                                         this.cmbUser.ActiveText)[0]["IDUsuario"].ToString());
			    	st.AppendValues(dr["IDVinculacion"].ToString(),(drs.Length>0)?true:false,
			    	        dr["Descripcion"].ToString(),dr["NombreForm"].ToString());
	     		
		      }
		    }
		}
		
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
	    }
        
        private bool paraUnUsuario{
             set{
               this.cmbUser.Sensitive = !value;
               this.btnAñadirUser.Sensitive = !value;
                 if(value){
                    this.lblInformacion.Text="Un solo usuarios";
                    }else{
                    this.lblInformacion.Text="Modifica cualquier usuario.";
                    }
             }
        
        }
    
        
        
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if(modo == Modos.salir){
				       if(this.SalirPrivilegios!= null){ SalirPrivilegios();}
				          this.Destroy();
				          }else{
				          this.modo = Modos.salir;
				          this.MostrarAceptar(false);
				          this.lblInformacion.Text= "Modo no editable activado";
				          this.RellenarTabla();
				          }
				}

		
				

				protected virtual void OnCmbFormulariosChanged (object sender, System.EventArgs e)
				{
				    this.RellenarTabla();
				}

				protected virtual void OnCmbUserChanged (object sender, System.EventArgs e)
				{
				   this.RellenarTabla();
				}

				protected virtual void OnLstTablaRowActivated (object o, Gtk.RowActivatedArgs args)
				{
				        Gtk.TreeModel model;
                         Gtk.TreeIter iter;
                           if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                              model.SetValue(iter,1,!(bool)model.GetValue(iter,1));
                                DataRow[] drs =  tbPrincipal.Select("IDBotonControl ='"+model.GetValue(iter,1).ToString()+
                                                     "'");
                               if((bool)model.GetValue(iter,1)){
                                  if(drs.Length<=0){
                                    DataRow dr = tbPrincipal.NewRow();
                                       dr["IDBotonControl"] = model.GetValue(iter,1).ToString();
                                       dr["IDUsuario"] = gesLocal.ExtraerLaTabla("Usuarios",null).Select("NombreUsuario ='"+
			                                         this.cmbUser.ActiveText)[0]["IDUsuario"].ToString();
			                            gesLocal.ActualizarSincronizar("Privilegios","IDVinculacion = "+dr["IDVinculacion"].ToString(),AccionesConReg.Agregar);            
			                            this.gesLocal.GuardarDatos(dr,"IDVinculacion = "+dr["IDVinculacion"].ToString(),AccionesConReg.Agregar);
                                  }
                               }else{
                                  if(drs.Length>0){
                                    gesLocal.ActualizarSincronizar("Privilegios","IDVinculacion = "+drs[0]["IDVinculacion"].ToString(),AccionesConReg.Agregar);
                                    this.gesLocal.GuardarDatos(drs[0],"IDVinculacion = "+drs[0]["IDVinculacion"].ToString(),AccionesConReg.Agregar);
                                    drs[0].Delete();
                                  }
                               
                               }
                              
                            }
                            
				}

				protected virtual void OnBtnAñadirUserClicked (object sender, System.EventArgs e)
				{
				     VenUsuarios ven = new VenUsuarios(gesLocal);
				      ven.salirUser += new OnSalirAñadirUser(this.RellenarUsuarios);
				        ven.Show();
				    
				}
				
			
	}
}
