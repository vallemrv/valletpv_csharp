// VenFavoritos.cs created with MonoDevelop
// User: valle at 15:24 02/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
	
	public delegate void OnSalirFav();
	
	public partial class VenFavoritos : Gtk.Window
	{
	  
	 enum Modos { Editar, Borrar, Añadir, salir};		
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private Modos modo;
		private DataRow drActivo;
		public event OnSalirFav SalirFav;
	
		
		public VenFavoritos(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			SalirFav = null;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Favoritos","IDFavoritos");
			this.lstTabla.AppendColumn("IDFavoritos",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
			
			
			//configurar el aspecto del control
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarTabla();
		}
			private void RellenarTabla(){
 	       	 Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String));
	     		this.lstTabla.Model = st;
		     	foreach(DataRow dr in tbPrincipal.Rows){
			    	st.AppendValues(dr["IDFavoritos"].ToString(), dr["Nombre"].ToString());
			     }
		}
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	  this.txtNombre.Sensitive = mostrar;
        }
        
        private bool CargarReg(){
             DataRow[] drs = tbPrincipal.Select("Nombre ='"+this.txtNombre.Text+"'");
              if((drs.Length<=0)&&(this.txtNombre.Text.Length>0)){
              drActivo["Nombre"] = this.txtNombre.Text;
              return true;
              }else{
                return false;
                }
        }
        
       
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if(modo == Modos.salir){
				       if(this.SalirFav!= null){ SalirFav();}
				          this.Destroy();
				          }else{
				          this.modo = Modos.salir;
				          this.MostrarAceptar(false);
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
				        
				             this.lblInformacion.Text = "Modo editar activado";   
				}

				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{
				     try{
				         switch(modo){
				           case Modos.Añadir:
				             if(this.CargarReg()){
				              this.tbPrincipal.Rows.Add(drActivo);
				              this.gesLocal.ActualizarSincronizar("Favoritos","IDFavoritos = "+drActivo["IDFavoritos"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDFavoritos = "+drActivo["IDFavoritos"].ToString(),AccionesConReg.Agregar);
				              this.RellenarTabla();
				              this.lblInformacion.Text= "Modo Añadir activado";
				              this.txtNombre.Text="";
				              this.drActivo = tbPrincipal.NewRow();
				              }else{
				                this.lblInformacion.Text= "Error al introducir el nombre";
				                }
				            break;
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("Favoritos","IDFavoritos = "+drActivo["IDFavoritos"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Favoritos","IDFavoritos = "+drActivo["IDFavoritos"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.RellenarTabla();
				             this.lblInformacion.Text= "Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Favoritos","IDFavoritos = "+drActivo["IDFavoritos"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDFavoritos = "+drActivo["IDFavoritos"].ToString(),AccionesConReg.Modificar);
				             this.RellenarTabla();
				                  this.lblInformacion.Text= "Modo editar activado";
				            }else{
				                this.lblInformacion.Text= "Error al introducir el nombre";
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
                              DataRow dr = tbPrincipal.Select("IDFavoritos = "+model.GetValue(iter, 0).ToString())[0];
                              this.txtNombre.Text = dr["Nombre"].ToString();
                              this.drActivo = dr;
                          }
				     
				}

				protected virtual void OnTxtNombreActivated (object sender, System.EventArgs e)
				{
				 this.OnBtnAceptarClicked(null,null);
				}
	}
	
}
