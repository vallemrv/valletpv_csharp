// VenFamilias.cs created with MonoDevelop
// User: valle at 21:49 23/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
	
    public delegate void OnSalirFamilias();
	 
	public partial class VenFamilias : Gtk.Window
	{
        
        enum Modos { Editar, Borrar, Añadir, Salir};
	
        
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private Modos modo = Modos.Salir;
		private DataRow drActivo;
		bool agregar = false;
		
		
		public event OnSalirFamilias salirFamilias;
		
		
		public VenFamilias(GesBaseLocal gesL,DataTable tbFamilias) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			//preparar variables locales
			gesLocal = gesL;
			tbPrincipal = tbFamilias;
			this.ConfiguracionIni(); 
			this.agregar = true;
		    this.OnBtnAñadirClicked(null,null);
			
		}
		
		
		public VenFamilias(GesBaseLocal gesL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			//preparar variables locales
			 gesLocal = gesL;
		     tbPrincipal = gesL.ExtraerLaTabla("Familias","IDVinculacion");
			 this.ConfiguracionIni();
		}
		
		void ConfiguracionIni(){
		 	 this.salirFamilias = null;
			
		   //preparar apariencia inicial
			this.lblInformacion.Text = "Modo no editable esta activado";
			this.mostrarAceptar(false);
			Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String));
			this.lstTabla.Model = st;
			this.lstTabla.AppendColumn("IDFamilia",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
			this.RellenarLista();
			
		}
		
		 private void mostrarAceptar(bool mostrar){
		      this.btnAceptar.Visible = mostrar;
			     this.btnBorrar.Visible = !mostrar;
			       this.btnEditar.Visible = !mostrar;
			         this.btnAñadir .Visible = !mostrar;
			           this.txtIDFamilia.Sensitive =mostrar;
			            this.txtNombre.Sensitive =mostrar;
		 }
	
	    private void RellenarLista(){
		    ((Gtk.ListStore)this.lstTabla.Model).Clear();
		     foreach(DataRow dr in tbPrincipal.Rows){
		         ((Gtk.ListStore)this.lstTabla.Model).AppendValues(dr["IDFamilia"].ToString(),dr["Nombre"].ToString());
		         }
		    }
		    
		   private bool CargarReg(){
		      bool correcto = false;
		       DataRow[] drsID = tbPrincipal.Select("IDFamilia = '"+this.txtIDFamilia.Text+"'");
		       DataRow[] drsNom = tbPrincipal.Select("Nombre = '"+this.txtNombre.Text+"'");
		       switch(modo){
		        case Modos.Añadir:
		                  if(((drsID.Length+drsNom.Length)<=0)&&(this.txtNombre.Text.Length>0)&&
		                        (this.txtIDFamilia.Text.Length>0)){
		                        drActivo["IDFamilia"]=this.txtIDFamilia.Text;
		                        drActivo["Nombre"]=this.txtNombre.Text;
		                        correcto = true;
		                        }
		                       break;
		         case Modos.Editar:
		                    if((drsID.Length<=1)&&(drsNom.Length<=1)&&(this.txtNombre.Text.Length>0)&&
		                        (this.txtIDFamilia.Text.Length>0)){
		                        drActivo["IDFamilia"]=this.txtIDFamilia.Text;
		                        drActivo["Nombre"]=this.txtNombre.Text;
		                       correcto = true;
		                       }
		                       break;
		            }
		    return correcto;
		   }
		 
		   private void CargarControles(){
		          this.txtIDFamilia.Text = drActivo["IDFamilia"].ToString();
                  this.txtNombre.Text = drActivo["Nombre"].ToString();
           }
	
	       private void VaciarControles(){
		      this.txtIDFamilia.Text="";
		      this.txtNombre.Text="";
		     }

				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{
				   try{
				    switch(modo){
				       case Modos.Añadir:
				           if(this.CargarReg()){ 
				                 tbPrincipal.Rows.Add(drActivo);
				                   this.gesLocal.ActualizarSincronizar("Familias","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				                   this.gesLocal.GuardarDatos(drActivo,"IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				                   
				                    this.RellenarLista();
				                     this.lblInformacion.Text = "Modo añadir activado";
				                      this.VaciarControles();
				                       this.drActivo = tbPrincipal.NewRow();
				                   }else{
				                      this.lblInformacion.Text= "Error en los datos introducidos";
				                   }
				        break;
				      
				      case Modos.Borrar:
				           this.gesLocal.ActualizarSincronizar("Familias","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				           this.gesLocal.GuardarDatos("Familias","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				            tbPrincipal.Rows.Remove(this.drActivo);
				            this.lblInformacion.Text = "Modo borrar activado";
				         break;
				    
				      case Modos.Editar:
				        if(this.CargarReg()){
				           this.gesLocal.ActualizarSincronizar("Familias","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				           this.gesLocal.GuardarDatos(drActivo,"IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				           this.lblInformacion.Text = "Modo editar activado";
				           }else{
				             this.lblInformacion.Text= "Error en los datos introducidos";
				             }
				        
				        break;
				    }
				 
				    this.RellenarLista();
				    }catch (Exception ex){
				       this.lblInformacion.Text = "Error: "+ ex.Message;
				    }
				}
				
					
			
				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				         this.modo = Modos.Editar;
				         this.mostrarAceptar(true);
				         this.lblInformacion.Text = "Modo editar activado";
				}
			
				protected virtual void OnBtnBorrarClicked (object sender, System.EventArgs e)
				{
				         this.modo = Modos.Borrar;
				         this.mostrarAceptar(true);
				         this.lblInformacion.Text = "Modo borrar activado";
				}
				protected virtual void OnBtnAñadirClicked(object sender, System.EventArgs e){
				         this.modo = Modos.Añadir;
				         this.mostrarAceptar(true);
				         this.drActivo = this.tbPrincipal.NewRow();
				         this.VaciarControles();
				         this.lblInformacion.Text = "Modo añadir activado";
				}

				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				               if((modo == Modos.Salir)||(this.agregar)){
				                    if(this.salirFamilias!=null){this.salirFamilias();}
			                    	this.Destroy();
			                   }else{
			                       this.modo = Modos.Salir;
			                       this.mostrarAceptar(false);
			                       this.lblInformacion.Text= ("Modo  no editable activado");
			                       this.VaciarControles();
			                   }
				}

				
				protected virtual void OnLstTablaCursorChanged (object sender, System.EventArgs e)
				{
				    if(modo != Modos.Añadir){
				      Gtk.TreeModel model;
                      Gtk.TreeIter iter;
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                            DataRow dr = tbPrincipal.Select("IDFamilia = '"+model.GetValue(iter, 0).ToString()+"'","",DataViewRowState.CurrentRows)[0];
                              this.drActivo = dr;
                                 this.CargarControles();
                           
                          }
                          }
				}

				
	}
}
