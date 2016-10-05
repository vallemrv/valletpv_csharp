// VenSecciones.cs created with MonoDevelop
// User: valle at 17:22 24/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
	
	public delegate void OnSalirSecciones();
	public partial class VenSecciones : Gtk.Window
	{
	    enum Modos { Editar, Borrar, Añadir,  salir};		
		
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbColores;
		private Hashtable listaColores;
		private Modos modo;
		private DataRow drActivo;
		
		public event OnSalirSecciones salirSecciones;
		
		public VenSecciones(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			salirSecciones = null;
			this.tbColores = gesLocal.ExtraerLaTabla("Colores",null);
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Secciones","IDSeccion");
			this.listaColores = new Hashtable();
			this.lstTabla.AppendColumn("IDSeccion",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.AppendColumn("Color",new Gtk.CellRendererText(),"text",2);
			this.lstTabla.Columns[0].Visible = false;
			
			
			//configurar el aspecto del control
			this.RellenarColores(); 
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarTabla();
		}
		
		private void RellenarColores(){
			int pos = 0;
			listaColores.Clear();
		    Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbColores.Model = st;
			foreach(DataRow dr in tbColores.Rows){
				st.AppendValues(dr["Nombre"].ToString());
				this.listaColores.Add(dr["Nombre"].ToString(),pos);
				pos++;
			}
			
		}
		 
		private int ActivarNumColores(String nombre){
			return (int)this.listaColores[nombre];
		}
		
		private void RellenarTabla(){
 		 Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String));
			this.lstTabla.Model = st;
			foreach(DataRow dr in tbPrincipal.Rows){
				st.AppendValues(dr["IDSeccion"].ToString(), dr["Nombre"].ToString(),
				      this.tbColores.Select("IDColor = "+dr["IDColor"].ToString())[0]["Nombre"].ToString());
			}
		}
		
		
		
         
        private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.txtNombre.Sensitive = mostrar;
		   	
        }
        
        private void VaciarControles(){
              this.txtNombre.Text = "";
	    }
        
        
        
        private bool CargarReg(){
         bool correcto = false;
         DataRow[] drs = tbPrincipal.Select("Nombre = '"+this.txtNombre.Text+"'");
         switch(modo){
           case Modos.Añadir:
              if((drs.Length<=0)&&(this.txtNombre.Text.Length>0)){
                drActivo["Nombre"] = this.txtNombre.Text;
                drActivo["IDColor"] = tbColores.Select("Nombre = '"+ this.cmbColores.ActiveText+"'")[0]["IDColor"];
                correcto = true;
                }
                break;
            case Modos.Editar:
              if((drs.Length<=1)&&(this.txtNombre.Text.Length>0)){
                drActivo["Nombre"] = this.txtNombre.Text;
                drActivo["IDColor"] = tbColores.Select("Nombre = '"+ this.cmbColores.ActiveText+"'")[0]["IDColor"];
                correcto = true;
                }
                break;
                }
                return correcto;
            
            
        }
        
				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{    try{
				         switch(modo){
				           case Modos.Añadir:
				             if( this.CargarReg()){
				              this.tbPrincipal.Rows.Add(drActivo);
				              this.gesLocal.ActualizarSincronizar("Secciones","IDSeccion ="+drActivo["IDSeccion"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDSeccion ="+drActivo["IDSeccion"].ToString(),AccionesConReg.Agregar);
				              this.drActivo = tbPrincipal.NewRow();
				              this.RellenarTabla();
				              this.VaciarControles();
				              this.lblInformacion.Text="Modo añadir activado";
				              }else{
				                this.lblInformacion.Text="Error en los datos introducidos";
				                }
				            break;
				           case Modos.Borrar:
				             //gesLocal.ActualizarEnCascadaBorrar(TablaActualizar.Secciones, drActivo["IDSeccion"].ToString());
				             this.gesLocal.ActualizarSincronizar("Secciones","IDSeccion ="+drActivo["IDSeccion"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Secciones","IDSeccion ="+drActivo["IDSeccion"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.VaciarControles();
				             this.RellenarTabla();
				                 this.lblInformacion.Text="Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Secciones","IDSeccion ="+drActivo["IDSeccion"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDSeccion ="+drActivo["IDSeccion"].ToString(),AccionesConReg.Modificar);
				             this.RellenarTabla();
				             this.lblInformacion.Text="Modo editar activado";
				            }else{
				                this.lblInformacion.Text="Error en los datos introducidos";
				                }
				            
				            break;
				         }
				        }catch(Exception ex){
				          this.lblInformacion.Text = "Error: "+ ex.Message; 
				        }
				}

				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				     this.modo = Modos.Editar;
				       this.MostrarAceptar(true);
				             this.lblInformacion.Text = "Modo editar activado";   
				
				}

				protected virtual void OnBtnAñadirClicked (object sender, System.EventArgs e)
				{
				     this.modo = Modos.Añadir;
				        this.MostrarAceptar(true);
				         this.VaciarControles();   this.drActivo = tbPrincipal.NewRow();
				             this.lblInformacion.Text = "Modo añadir activado";
				}

				protected virtual void OnBtnBorrarClicked (object sender, System.EventArgs e)
				{
				          
				     this.modo = Modos.Borrar;
				       this.MostrarAceptar(true);
				        
				           this.lblInformacion.Text = "Modo borrar activado";
				           
				}

				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				      if(modo == Modos.salir){
				         if (salirSecciones!=null){salirSecciones();}
				          this.Destroy();
				          }else{
				          this.modo = Modos.salir;
				          this.MostrarAceptar(false);
				          this.lblInformacion.Text= "Modo no editable activado";
				          this.VaciarControles();
				          this.RellenarTabla();
				          }
				}

				protected virtual void OnBtnAñadirColorClicked (object sender, System.EventArgs e)
				{ 
				        VenColores venColores = new VenColores(gesLocal);
				           venColores.salirColores += new OnSalirColores(this.RellenarColores);
				             venColores.Show();
				}

				

				
				
				protected virtual void OnCmbColoresChanged (object sender, System.EventArgs e)
				{
				        if (this.cmbColores.ActiveText!=null){
				                  DataRow[] drsColor = tbColores.Select("Nombre = '"+this.cmbColores.ActiveText+"'");
				                   if(drsColor.Length>0){ 
				                     DataRow drColor = drsColor[0];
				                         this.imgColor.ColorDeFondo = System.Drawing.Color.FromArgb(Int32.Parse(drColor["Rojo"].ToString()),
                                                                    Int32.Parse(drColor["Verde"].ToString()),
                                                                      Int32.Parse(drColor["Azul"].ToString()));
                                                                      
                                        }
                                 
				       }
				}

				protected virtual void OnLstTablaCursorChanged (object sender, System.EventArgs e)
				{
				      Gtk.TreeModel model;
                      Gtk.TreeIter iter;
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                        DataRow dr = tbPrincipal.Select("IDSeccion = "+model.GetValue(iter, 0).ToString())[0];
                        DataRow[] drsColor = tbColores.Select("IDColor = "+dr["IDColor"].ToString());
                        if(drsColor.Length>0){
                            DataRow drColor = drsColor[0];
                              this.txtNombre.Text = dr["Nombre"].ToString();
                              this.cmbColores.Active = this.ActivarNumColores(
                                               tbColores.Select("IDColor = "+dr["IDColor"].ToString())[0]["Nombre"].ToString());
                                         this.imgColor.ColorDeFondo = System.Drawing.Color.FromArgb(Int32.Parse(drColor["Rojo"].ToString()),
                                                                    Int32.Parse(drColor["Verde"].ToString()),
                                                                      Int32.Parse(drColor["Azul"].ToString()));
                                       
                              this.drActivo = dr;
                              }
                          }
				     				}
	}
}
