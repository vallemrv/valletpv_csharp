// VenInstComision.cs created with MonoDevelop
// User: valle at 0:09 15/07/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.SqlUtilidades;

namespace Valle.GesTpv
{
    
    
    public partial class VenInstComision : Gtk.Window
    {
        enum Modos { Editar, Borrar, Añadir, salir};		
	
	
		private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbVista;
		private DataTable tbCamareros;
		private Modos modo;
		private DataRow drActivo;
		
        public VenInstComision(GesBaseLocal gsL) : 
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
        	//configurar variables locales
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("InstComision","IDVinculacion");
			tbCamareros = gesLocal.EjConsultaSelect("Camareros", 
			                  "SELECT Camareros.IDCamarero, Camareros.Nombre, Camareros.Apellidos FROM Camareros",
			                                             "Camareros");
			
			//configurar el aspecto del control
			this.cmbCamareros.Model = new Gtk.ListStore(typeof(string),typeof(string),  typeof(object));
	       
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.RellenaCamareros();
			this.RellenarTabla();
		
		}
		void RellenarTabla(){
		   System.Threading.Thread.Sleep(100);
			this.tbVista = gesLocal.EjConsultaSelect("tbVista", 
			"SELECT Camareros.IDCamarero, Camareros.Nombre, Camareros.Apellidos, (InstComision.PorcientoCom*100) AS PorcientoCom, InstComision.HoraInicio, InstComision.HoraFin, "+
			" InstComision.Tarifa FROM Camareros INNER JOIN InstComision ON Camareros.IDCamarero = InstComision.IDCamarero","Camareros","InstComision");
			
	     	this.ListaCamareros.rellenarLista(tbVista,new String[]{"Nombre","Apellidos","PorcientoCom","HoraInicio","HoraFin","Tarifa"},
			                                               new String[]{"Nombre","Apellidos","Porcentaje","Hora inicio","Hora fim","Tarifa"});
		
		}
	  
	    void RellenaCamareros(){
	        Gtk.ListStore ls =  (Gtk.ListStore)this.cmbCamareros.Model;
	        ls.Clear();
	         foreach(DataRow r in tbCamareros.Rows){
	          ls.AppendValues(r["Nombre"].ToString(),r["Apellidos"].ToString(),r);
	         }
	    }   
	   
		private void MostrarAceptar(bool mostrar){
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = !mostrar;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.txthorasFin.Sensitive = mostrar;
		   	 this.txthorasInicio.Sensitive = mostrar;
		   	 this.txtPorcentaje.Sensitive = mostrar;
		   	 this.txtTarifa.Sensitive = mostrar;
		   	 this.cmbCamareros.Sensitive = mostrar;
        }
        
        private bool CargarReg(){
           if(this.cmbCamareros.ActiveText!=null){
             drActivo["PorcientoCom"] = this.txtPorcentaje.Value/100;
             drActivo["HoraInicio"]= this.txthorasInicio.Hora;
             drActivo["HoraFin"] = this.txthorasFin.Hora;
             drActivo["Tarifa"] = this.txtTarifa.Value;
             Gtk.TreeIter iter;
             this.cmbCamareros.GetActiveIter(out iter);
             drActivo["IDCamarero"] = ((DataRow)((Gtk.ListStore)this.cmbCamareros.Model).GetValue(iter,2))["IDCamarero"];
           return true;
           }else{
            return false;
            }
        }
        private void VaciarControles(){
          this.txthorasInicio.Hora = "";
          this.txthorasFin.Hora = "";
          this.txtPorcentaje.Text = "";
          this.txtTarifa.Text = "";
          this.cmbCamareros.Active = -1;
         }
        
       
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if(modo == Modos.salir){
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
				              this.gesLocal.ActualizarSincronizar("InstComision","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				              this.gesLocal.GuardarDatos(drActivo,"IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Agregar);
				              this.drActivo = tbPrincipal.NewRow();
				              this.VaciarControles();
				              this.lblInformacion.Text = "Modo añadir activado";
				              }else{
				               this.lblInformacion.Text = "Error al introducir los datos";
				               }
				            break;
				           case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("InstComision","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("InstComision","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Borrar);
				             this.tbPrincipal.Rows.Remove(this.drActivo);
				             this.VaciarControles();
				             this.lblInformacion.Text = "Modo borrar activado";
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){;
				               this.gesLocal.ActualizarSincronizar("InstComision","IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				               this.gesLocal.GuardarDatos(drActivo,"IDVinculacion = "+drActivo["IDVinculacion"].ToString(),AccionesConReg.Modificar);
				               this.lblInformacion.Text = "Modo editar activado";
				              }else{
				               this.lblInformacion.Text = "Error al introducir los datos";
				               }
				           
				            break;
				         }
				        this.RellenarTabla(); 
				        }catch(Exception ex){
				          this.lblInformacion.Text = "Error: "+ ex.Message; 
				        }
				}

				

				protected virtual void OnLstTablaCursorChanged (object sender, System.EventArgs e)
				{
				       if((modo != Modos.Añadir)){
                        DataRow dr = this.ListaCamareros.ExtraerRegSelec();
                        if(dr!=null){
                              this.txtTarifa.Text = dr["Tarifa"].GetType().Name.Equals("DBNull") ? "0" : dr["Tarifa"].ToString();
                              this.txtPorcentaje.Value = (double)(decimal)dr["PorcientoCom"];
                              this.txthorasFin.Hora = dr["HoraFin"].ToString();
                              this.txthorasInicio.Hora = dr["HoraInicio"].ToString();
                              this.cmbCamareros.Active = tbCamareros.Rows.IndexOf(
                                                        tbCamareros.Select("IDCamarero = "+dr["IDCamarero"].ToString())[0]);
                              drActivo = this.tbPrincipal.Select("IDCamarero = "+dr["IDCamarero"].ToString())[0];                         
                          }
				     
				   }
				}

				
	}
}
    
