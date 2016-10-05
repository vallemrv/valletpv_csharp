// VenColores.cs created with MonoDevelop
// User: valle at 13:52 25/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Drawing;

using Valle.SqlUtilidades;


namespace Valle.GesTpv
{
	public delegate void OnSalirColores();
	
	public partial class VenColores : Gtk.Window
	{
	  enum Modos { Editar, Borrar, Añadir, salir};		
		
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private Modos modo;
		private DataRow drActivo;
		bool agregar = false;
		public event OnSalirColores salirColores;
		
		
		public VenColores(GesBaseLocal gsL,DataTable tbColores) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = tbColores;
			this.ConfiguracionIni();
			this.agregar = true;
			this.OnBtnAñadirClicked(null,null);
		}
		
		public VenColores(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Colores","IDColor");
			this.ConfiguracionIni();
		}
		
		
		void ConfiguracionIni(){
		  salirColores = null;
			this.lstTabla.AppendColumn("IDColor",new Gtk.CellRendererText(),"text",0);
			this.lstTabla.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",1);
			this.lstTabla.AppendColumn("Rojo",new Gtk.CellRendererText(),"text",2);
			this.lstTabla.AppendColumn("Verde",new Gtk.CellRendererText(),"text",3);
			this.lstTabla.AppendColumn("Azul",new Gtk.CellRendererText(),"text",4);
			this.lstTabla.Columns[0].Visible = false;
			
			
			
			//configurar el aspecto del control
			this.imgColor.Visible = true;
			this.btnColor.Visible = false;
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenarTabla();
		}
		
		
		private void RellenarTabla(){
 		 Gtk.ListStore st = new Gtk.ListStore(typeof(String),typeof(String),typeof(String),typeof(String),typeof(String));
			this.lstTabla.Model = st;
			foreach(DataRow dr in tbPrincipal.Rows){
				st.AppendValues(dr["IDColor"].ToString(), dr["Nombre"].ToString(),
				   dr["Rojo"].ToString(), dr["Verde"].ToString(),dr["Azul"].ToString());
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
           bool correcto = false;
           DataRow[] drNom = tbPrincipal.Select("Nombre = '" +this.txtNombre.Text+"'");
         switch(modo){
           case Modos.Añadir:
             if((drNom.Length<=0)&&(txtNombre.Text.Length>0)){
                  drActivo["Nombre"] = this.txtNombre.Text;
                   drActivo["Rojo"] = this.btnColor.Color.Red>>8;
                    drActivo["Verde"] = this.btnColor.Color.Green>>8;
                     drActivo["Azul"] = this.btnColor.Color.Blue>>8;
                 correcto = true;
              }
            break;
            case Modos.Editar:
              if((drNom.Length<=1)&&(txtNombre.Text.Length>0)){
                  drActivo["Nombre"] = this.txtNombre.Text;
                   drActivo["Rojo"] = this.btnColor.Color.Red>>8;
                    drActivo["Verde"] = this.btnColor.Color.Green>>8;
                     drActivo["Azul"] = this.btnColor.Color.Blue>>8;
                 correcto = true;
              }
            break;
            
           }
           return correcto;
        }
        

				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if((modo == Modos.salir)||(agregar)){
				          if(this.salirColores != null){ salirColores();}
				          this.Destroy();
				          }else{
				           this.btnColor.Visible = false;
				           this.imgColor.Visible = true;
				    
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
				         this.btnColor.Visible = true;
				         this.imgColor.Visible = false;
				         this.txtNombre.Text=""; 
				         this.drActivo = tbPrincipal.NewRow();
				         this.lblInformacion.Text = "Modo añadir activado";
				}

				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				     this.modo = Modos.Editar;
				     this.btnColor.Visible = true;
				     this.imgColor.Visible = false;
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
				              this.gesLocal.ActualizarSincronizar("Colores","IDColor = "+drActivo["IDColor"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDColor = "+drActivo["IDColor"].ToString(),AccionesConReg.Agregar);
				              this.drActivo = tbPrincipal.NewRow();
				              this.txtNombre.Text="";
				              this.lblInformacion.Text = "Modo Añadir activado";
				              }else{
				                this.lblInformacion.Text= "Error al introducir los datos";
				                }
				            break;
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("Colores","IDColor = "+drActivo["IDColor"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Colores","IDColor = "+drActivo["IDColor"].ToString(),AccionesConReg.Borrar);
				             this.drActivo.Delete();
				             this.lblInformacion.Text = "Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Colores","IDColor = "+drActivo["IDColor"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDColor = "+drActivo["IDColor"].ToString(),AccionesConReg.Modificar);
				                this.lblInformacion.Text = "Modo editar activado";
				             }else{
				                this.lblInformacion.Text= "Error al introducir los datos";
				                }
				           
				            break;
				         }
				         
				        }catch(Exception ex){
				          this.lblInformacion.Text = "Error: "+ ex.Message; 
				        }
				        this.tbPrincipal.AcceptChanges();
				        this.RellenarTabla();
				        
				 }

				protected virtual void OnLstTablaCursorChanged (object sender, System.EventArgs e)
				{
				   
				      Gtk.TreeModel model;
                      Gtk.TreeIter iter;
                     if (this.lstTabla.Selection.GetSelected(out model, out iter)) {
                              DataRow dr = tbPrincipal.Select("IDColor = "+model.GetValue(iter, 0).ToString())[0];
                           if(modo != Modos.Añadir){this.txtNombre.Text = dr["Nombre"].ToString();            this.drActivo = dr;}
                              this.btnColor.Color = new Gdk.Color(Byte.Parse(dr["Rojo"].ToString()),
                                                                    Byte.Parse(dr["Verde"].ToString()),
                                                                      Byte.Parse(dr["Azul"].ToString()));
                              this.imgColor.ColorDeFondo = System.Drawing.Color.FromArgb(Int32.Parse(dr["Rojo"].ToString()),
                                                                    Int32.Parse(dr["Verde"].ToString()),
                                                                      Int32.Parse(dr["Azul"].ToString()));
                         			     
				}
				}

				protected virtual void OnTxtNombreActivated (object sender, System.EventArgs e)
				{
				    this.OnBtnAceptarClicked(null,null);
				}
	}
}
