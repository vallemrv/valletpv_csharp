// VenUsuarios.cs created with MonoDevelop
// User: valle at 23:00 28/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.Seguridad;
using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
	
	public delegate void OnSalirAñadirUser();
	public partial class VenUsuarios : Gtk.Window
	{
		enum Modos { Editar, Borrar, Añadir, salir};		
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbSuperUsuario;
		private Modos modo;
		private DataRow drActivo;
	
		public event OnSalirAñadirUser salirUser;
		public VenUsuarios(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			salirUser = null;
			//tabla principal 
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Usuarios","IDUsuario");
			            
			     //super usuarios
	        this.tbSuperUsuario = gesLocal.ExtraerLaTabla("SuperUsuario","IDVinculacion");
			
			this.lstTabla.AppendColumn("IDUsuario",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre Usuario",new Gtk.CellRendererText(),"text",1);
    		
		    
			//configurar el aspecto del control
			this.MostrarAceptar(false);
			this.btnPrivilegios.Sensitive = false;
		    this.modo = Modos.salir;
			this.RellenarTabla();
		}
		
		
		
			private void RellenarTabla(){
        	 Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String),typeof(bool));
	     		this.lstTabla.Model = st;
			   foreach(DataRow dr in tbPrincipal.Rows){
			      DataRow[] drs =tbSuperUsuario.Select("IDUsuario = "+dr["IDUsuario"].ToString());
                  st.AppendValues(dr["IDUsuario"].ToString(), dr["NombreUsuario"].ToString(), ((drs.Length>0)?true:false));
			}
		}
		
		private void VaciarControles(){
              this.txtPass.Text = "";
				this.txtPass1.Text="";
				 this.txtNombre.Text = "";
				 this.chekPass.Active = false;
        } 
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.txtPass.Sensitive = mostrar;
		   	 this.txtNombre.Sensitive = mostrar;
		   	 this.txtPass1.Sensitive = mostrar;
		   	 this.chekPass.Sensitive = mostrar;
		   	
        }
        
        private bool CargarReg(){
         bool correcto = false;
           DataRow[] drNom	 = tbPrincipal.Select("NombreUsuario ='"+this.txtNombre.Text+"'");
          switch(modo){
            case Modos.Añadir:
             if((drNom.Length<=0)&&(this.txtPass.Text.Length>0)&&(this.txtNombre.Text.Length>0)
                     &&(this.txtPass1.Text.Length>0)&&(this.txtPass.Text.Equals(this.txtPass1.Text))){
               drActivo["NombreUsuario"] = this.txtNombre.Text;
               int esnumero = Int32.Parse(this.txtPass.Text);
               drActivo["Contraseña"] = Encriptar.EncriptarCadena(this.txtPass.Text);
               correcto = true && (esnumero>0);
           }
           break;
           case Modos.Editar:
             if((drNom.Length<=1)&&(this.txtPass.Text.Length>0)&&(this.txtNombre.Text.Length>0)
                     &&(this.txtPass1.Text.Length>0)&&(this.txtPass.Text.Equals(this.txtPass1.Text))){
               drActivo["NombreUsuario"] = this.txtNombre.Text;
            
               int esnumero = Int32.Parse(this.txtPass.Text);
               drActivo["Contraseña"] = Encriptar.EncriptarCadena(this.txtPass.Text);
              correcto = true && (esnumero>0);
           }
           break;
           }
  
           return correcto;
        }
        
        
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if(modo == Modos.salir){
				        if(salirUser != null) {salirUser();} 
				          this.Destroy();
				          }else{
				          this.modo = Modos.salir;
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
				           this.lblInformacion.Text = "Modo borrar activado";
				}

				protected virtual void OnBtnAñadirClicked (object sender, System.EventArgs e)
				{
				  this.modo = Modos.Añadir;
				        this.MostrarAceptar(true);
				      this.VaciarControles();  this.drActivo = tbPrincipal.NewRow();
				             this.lblInformacion.Text = "Modo añadir activado. La contraseña solo puede ser numeros";
				}

				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				     this.modo = Modos.Editar;
				       this.MostrarAceptar(true);this.btnPrivilegios.Sensitive = !this.chekPass.Active;
				             this.lblInformacion.Text = "Modo editar activado. La contraseña solo puede ser numeros";   
				}

				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{
				    DataTable tbSuper = tbSuperUsuario; 
    
				     try{
				         switch(modo){
				           case Modos.Añadir:
				              if(this.CargarReg()){
				              this.tbPrincipal.Rows.Add(drActivo);
				              this.gesLocal.ActualizarSincronizar("Usuarios","IDUsuario = "+drActivo["IDUsuario"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDUsuario = "+drActivo["IDUsuario"].ToString(),AccionesConReg.Agregar);
			                     if((this.chekPass.Active)&&
                                        (tbSuper.Select("IDUsuario = " + drActivo["IDUsuario"].ToString()).Length<=0)){
                                               DataRow drSuper = tbSuper.NewRow();
                                               drSuper["IDUsuario"] = drActivo["IDUsuario"];
                                                tbSuper.Rows.Add(drSuper);
                                                gesLocal.ActualizarSincronizar("SuperUsuario",
                                                "IDVinculacion = "+drSuper["IDVinculacion"].ToString(),AccionesConReg.Agregar);
                                                this.gesLocal.GuardarDatos(drSuper,"IDVinculacion = "+drSuper["IDVinculacion"].ToString(),AccionesConReg.Agregar);
                                            }
                	          this.drActivo = tbPrincipal.NewRow();
				              this.RellenarTabla();
				                  this.lblInformacion.Text = "Modo añadir activado";
				             }else{
				               this.lblInformacion.Text = "Error al introducir los datos";
				               }
				           
				            break;
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("Usuarios","IDUsuario = "+drActivo["IDUsuario"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Usuarios","IDUsuario = "+drActivo["IDUsuario"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.RellenarTabla();
			                 this.lblInformacion.Text = "Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Usuarios","IDUsuario = "+drActivo["IDUsuario"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDUsuario = "+drActivo["IDUsuario"].ToString(),AccionesConReg.Modificar);
			 	             
			 	             DataRow[] drsSuper = tbSuperUsuario.Select("IDUsuario ="+drActivo["IDUsuario"].ToString());
                              if(this.chekPass.Active){
			 	                    if(drsSuper.Length<=0){
                                        DataRow drSuper = tbSuper.NewRow();
                                         drSuper["IDUsuario"] = drActivo["IDUsuario"];
                                         tbSuper.Rows.Add(drSuper);
                                          gesLocal.ActualizarSincronizar("SuperUsuario",
                                               "IDVinculacion = "+drSuper["IDVinculacion"].ToString(),AccionesConReg.Agregar);
                                               this.gesLocal.GuardarDatos(drSuper,"IDVinculacion = "+drSuper["IDVinculacion"].ToString(),AccionesConReg.Agregar);
                                       }
                                 }else{
                                       if(drsSuper.Length>0){
                                           gesLocal.ActualizarSincronizar("SuperUsuario",
                                                  "IDVinculacion = "+drsSuper[0]["IDVinculacion"].ToString(),AccionesConReg.Borrar);
                                                  this.gesLocal.GuardarDatos("SuperUsuario",
                                                  "IDVinculacion = "+drsSuper[0]["IDVinculacion"].ToString(),AccionesConReg.Borrar);
                                                    drsSuper[0].Delete();
                                            }
                                 }
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
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                        DataRow dr = tbPrincipal.Select("IDUsuario = "+model.GetValue(iter, 0).ToString())[0];
                        DataRow[] drsSuper = tbSuperUsuario.Select("IDUsuario = "+dr["IDUsuario"].ToString());
                              this.txtNombre.Text = dr["NombreUsuario"].ToString();
                              this.txtPass.Text = Encriptar.DescriptarCadena(dr["Contraseña"].ToString());
                              this.txtPass1.Text = Encriptar.DescriptarCadena(dr["Contraseña"].ToString());
                              this.chekPass.Active = drsSuper.Length>0;
                              this.btnPrivilegios.Sensitive = (drsSuper.Length<=0)&&(modo==Modos.Editar);
                              this.drActivo = dr;
                          }
				     
				}

				protected virtual void OnBtnPrivilegiosClicked (object sender, System.EventArgs e)
				{
				   if(this.txtNombre.Text.Length>0){
				       VenPrivilegios ven = new VenPrivilegios(gesLocal,this.txtNombre.Text);
				            ven.Show();
				            }
				}

				
				
			
	}
}
