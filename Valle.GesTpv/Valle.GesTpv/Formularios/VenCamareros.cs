// VenCamareros.cs created with MonoDevelop
// User: valle at 21:29 26/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.SqlUtilidades;


namespace Valle.GesTpv
{
	
	
	public partial class VenCamareros : Gtk.Window
	{
	
	 enum Modos { Editar, Borrar, Añadir, salir};		
	
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private Modos modo;
		private DataRow drActivo;
		bool agregar = false;
		
		public VenCamareros(GesBaseLocal gsL, DataTable tbCamareros) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = tbCamareros;
			this.ConfiguracionIni();
			agregar= true;
			this.OnBtnAñadirClicked(null,null);
		}
		
		
		public VenCamareros(GesBaseLocal gsL) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Camareros","IDCamarero");
			this.ConfiguracionIni();
		}
		
		void ConfiguracionIni(){
		   this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.AppendColumn("Apellidos",new Gtk.CellRendererText(),"text",2);
			this.lstTabla.AppendColumn("Direccion",new Gtk.CellRendererText(),"text",3);
			this.lstTabla.AppendColumn("Telefono",new Gtk.CellRendererText(),"text",4);
			Gtk.ListStore st = new Gtk.ListStore(typeof(DataRow),typeof(String),typeof(String),typeof(String),typeof(String));
			this.lstTabla.Model = st;
		
			
			//configurar el aspecto del control
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarTabla();
		}
	
	   private void RellenarTabla(){
			((Gtk.ListStore)lstTabla.Model).Clear();
 		   	foreach(DataRow dr in tbPrincipal.Rows){
				((Gtk.ListStore)lstTabla.Model).AppendValues(dr, dr["Nombre"].ToString(), dr["Apellidos"].ToString()
				, dr["Direccion"].ToString(), dr["Telefono"].ToString());
			}
		}
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.txtApellidos.Sensitive = mostrar;
		   	 this.txtDireccion.Sensitive = mostrar;
		   	 this.txtNombre.Sensitive = mostrar;
		   	 this.txtTelfono.Sensitive = mostrar;
        }
        
        private bool CargarReg(){
           if(this.txtNombre.Text.Length>0){
           drActivo["Nombre"] = this.txtNombre.Text;
           drActivo["Direccion"]=this.txtDireccion.Text;
           drActivo["Apellidos"] = this.txtApellidos.Text;
           drActivo["Telefono"] = this.txtTelfono.Text.Length!=9?"":this.txtTelfono.Text;
           return true;
           }else{
            return false;
            }
        }
        private void VaciarControles(){
          this.txtNombre.Text = "";
          this.txtDireccion.Text = "";
           this.txtApellidos.Text = "";
           this.txtTelfono.Text = "";
         }
        
       
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if((modo == Modos.salir)||(agregar)){
				          this.Destroy();
				          }else{
				          this.modo = Modos.salir;
				          this.hbox1.Sensitive = true;
			              this.hbox2.Sensitive = true;
				          this.MostrarAceptar(false);
				          this.lblInformacion.Text= "Modo no editable activado";
		                  this.VaciarControles();
				          this.RellenarTabla();
				          }
				}

				protected virtual void OnBtnBorrarClicked (object sender, System.EventArgs e)
				{
				    this.modo = Modos.Borrar;
				       this.MostrarAceptar(true);
			              this.hbox1.Sensitive = false;
			              this.hbox2.Sensitive = false;
				        
				           this.lblInformacion.Text = "Modo borrar activado";
				}

				protected virtual void OnBtnAñadirClicked (object sender, System.EventArgs e)
				{
				  this.modo = Modos.Añadir;
				        this.MostrarAceptar(true);
				      this.VaciarControles();  this.drActivo = tbPrincipal.NewRow();
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
				              this.gesLocal.ActualizarSincronizar("Camareros","IDCamarero = "+drActivo["IDCamarero"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDCamarero = "+drActivo["IDCamarero"].ToString(),AccionesConReg.Agregar);
				              this.drActivo = tbPrincipal.NewRow();
				              this.RellenarTabla();
				              this.lblInformacion.Text = "Modo añadir activado";
				              }else{
				               this.lblInformacion.Text = "Error al introducir los datos";
				               }
				            break;
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("Camareros","IDCamarero = "+drActivo["IDCamarero"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Camareros","IDCamarero = "+drActivo["IDCamarero"].ToString(),AccionesConReg.Borrar);
				             this.tbPrincipal.Rows.Remove(this.drActivo);
				             this.RellenarTabla();
				             this.VaciarControles();
				             this.lblInformacion.Text = "Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){;
				             this.gesLocal.ActualizarSincronizar("Camareros","IDCamarero = "+drActivo["IDCamarero"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDCamarero = "+drActivo["IDCamarero"].ToString(),AccionesConReg.Modificar);
				             this.RellenarTabla();
				           this.lblInformacion.Text = "Modo editar activado";
				              }else{
				               this.lblInformacion.Text = "Error al introducir los datos";
				               }
				           
				            break;
				         }
				        this.VaciarControles();
				        }catch(Exception ex){
				          this.lblInformacion.Text = "Error: "+ ex.Message; 
				        }
				}
     			protected virtual void OnLstTablaCursorChanged (object sender, System.EventArgs e)
				{
				      Gtk.TreeModel model;
                      Gtk.TreeIter iter;
                      if((modo != Modos.Añadir)){
                             
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                              DataRow dr = (DataRow)model.GetValue(iter, 0);
                          
                              this.txtNombre.Text = dr["Nombre"].ToString();
                              this.txtApellidos.Text = dr["Apellidos"].ToString();
                              this.txtDireccion.Text = dr["Direccion"].ToString();
                              this.txtTelfono.Text = dr["Telefono"].ToString();
                              this.drActivo = dr;
                          }
				     
				}
				}

	}
}
