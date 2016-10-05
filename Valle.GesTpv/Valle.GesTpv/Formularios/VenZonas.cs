// VenZonas.cs created with MonoDevelop
// User: valle at 18:21 25/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
	public delegate void OnSalirZonas();
	
	
	public partial class VenZonas : Gtk.Window
	{
		 enum Modos { Editar, Borrar, Añadir, salir};		
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbColores;
		private Modos modo;
		private DataRow drActivo;
			bool agregar = false;

		public event OnSalirZonas salirZonas;
	
		
		public VenZonas(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			salirZonas = null;
			this.tbColores = gesLocal.ExtraerLaTabla("Colores","IDColor");
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Zonas","IDZona");
			this.ConfiguracionIni();
			
		}
		
		
		public VenZonas(GesBaseLocal gsL,DataTable tbZonas) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			salirZonas = null;
			this.tbColores = gesLocal.ExtraerLaTabla("Colores","IDColor");
			this.tbPrincipal = tbZonas;
			this.ConfiguracionIni();
			this.agregar = true;
		    this.OnBtnAñadirClicked(null,null);	
		}
		
		
	   void ConfiguracionIni(){
	    	this.lstTabla.AppendColumn("IDZona",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
		    this.lstTabla.AppendColumn("Tarifa",new Gtk.CellRendererText(),"text",2);
			this.lstTabla.AppendColumn("Color",new Gtk.CellRendererText(),"text",3);
			
			
			//configurar el aspecto del control
			this.RellenarColores(); 
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarTabla();
	   }
		
		private void RellenarColores(){
		    Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbColores.Model = st;
			foreach(DataRow dr in tbColores.Rows){
				st.AppendValues(dr["Nombre"].ToString());
			}
		}
		
		
		
		private void RellenarTabla(){
         Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String),typeof(String));
			this.lstTabla.Model = st;
			foreach(DataRow dr in tbPrincipal.Rows){
			    string nombreColor = "Por defecto";
			    if(!dr["IDColor"].GetType().Name.Equals("DBNull"))
			       nombreColor = this.tbColores.Select("IDColor = "+dr["IDColor"].ToString())[0]["Nombre"].ToString();
			     
			    st.AppendValues(dr["IDZona"].ToString(), dr["Nombre"].ToString(),dr["tarifa"].ToString(),nombreColor);
				     
			}
		}
		
		private void VaciarControles(){
              this.txtNombre.Text = "";
				this.cmbColores.Active = 0;
				 this.txtTarifa.Text = "1";
        }
		
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.txtNombre.Sensitive = mostrar;
		   	 this.txtTarifa.Sensitive = mostrar;
		   	
        }
        
        private bool CargarReg(){
         bool correcto = false;
           DataRow[] drNom	 = tbPrincipal.Select("Nombre ='"+this.txtNombre.Text+"'");
          switch(modo){
            case Modos.Añadir:
             if((drNom.Length<=0)&&(this.txtNombre.Text.Length>0)){
               drActivo["Nombre"] = this.txtNombre.Text;
                drActivo["IDColor"] = tbColores.Select("Nombre = '"+ this.cmbColores.ActiveText+"'")[0]["IDColor"];
               drActivo["tarifa"] = Int32.Parse(this.txtTarifa.Text);
               correcto = true;
           }
           break;
           case Modos.Editar:
           if((drNom.Length<=1)&&(this.txtNombre.Text.Length>0)){
               drActivo["Nombre"] = this.txtNombre.Text;
                drActivo["IDColor"] = tbColores.Select("Nombre = '"+ this.cmbColores.ActiveText+"'")[0]["IDColor"];
               drActivo["tarifa"] = Int32.Parse(this.txtTarifa.Text);
               correcto = true;
           }
           break;
           }
  
           return correcto;
        }
        
         

				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if((modo == Modos.salir)||(agregar)){
				          if(this.salirZonas!= null){ salirZonas();}
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
				              this.gesLocal.ActualizarSincronizar("Zonas","IDZona = "+drActivo["IDZona"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDZona = "+drActivo["IDZona"].ToString(),AccionesConReg.Agregar);
				              this.RellenarTabla();
				              this.lblInformacion.Text = "Modo añadir activado";
				              this.drActivo = tbPrincipal.NewRow();
				             }else{
				               this.lblInformacion.Text = "Error al introducir los datos";
				               }
				           
				            break;
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("Zonas","IDZona = "+drActivo["IDZona"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Zonas","IDZona = "+drActivo["IDZona"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.RellenarTabla();
				             this.lblInformacion.Text = "Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Zonas","IDZona = "+drActivo["IDZona"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDZona = "+drActivo["IDZona"].ToString(),AccionesConReg.Modificar);
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

				protected virtual void OnBtnAñadirColorClicked (object sender, System.EventArgs e)
				{ 
				        VenColores venColores = new VenColores(gesLocal,tbColores);
				        venColores.salirColores += new OnSalirColores(this.RellenarColores);
				        
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
				      Console.WriteLine("hola mnaolo");
        
				      Gtk.TreeModel model;
                      Gtk.TreeIter iter;
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                         DataRow dr = tbPrincipal.Select("IDZona = "+model.GetValue(iter, 0).ToString())[0];
                         this.txtNombre.Text = dr["Nombre"].ToString();
                         this.txtTarifa.Text = dr["tarifa"].ToString();
                         this.drActivo = dr;
                        
                             
                        if(!dr["IDColor"].GetType().Name.Equals("DBNull")){
			     
                           DataRow[] drsColor = tbColores.Select("IDColor = "+dr["IDColor"].ToString());
                           DataRow drColor = drsColor[0];
                           this.cmbColores.Active = tbColores.Rows.IndexOf(tbColores.Select("IDColor = "+dr["IDColor"].ToString())[0]);
                           this.imgColor.ColorDeFondo = System.Drawing.Color.FromArgb(Int32.Parse(drColor["Rojo"].ToString()),
                                                                    Int32.Parse(drColor["Verde"].ToString()),
                                                                      Int32.Parse(drColor["Azul"].ToString()));
                                       
                              }else{
                                this.imgColor.ColorDeFondo = System.Drawing.Color.Gray;
                                this.cmbColores.Active = -1;
                              }
                            
                            
                          }
				     
				}

				protected virtual void OnTxtNombreActivated (object sender, System.EventArgs e)
				{
				  this.OnBtnAceptarClicked(null,null);
				}

				protected virtual void OnTxtTarifaActivated (object sender, System.EventArgs e)
				{
				   this.OnBtnAceptarClicked(null,null);
				}
				
			
	}
}
