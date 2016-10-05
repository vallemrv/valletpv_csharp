// VenTeclados.cs created with MonoDevelop
// User: valle at 2:11 09/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Valle.SqlUtilidades;
using Valle.GtkUtilidades;

namespace Valle.GesTpv
{
	
	
	public partial class VenTeclados : Gtk.Window
	{
	    enum Modos { Editar, Borrar, Añadir, salir};		
	
	    private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbSecciones;
		private Modos modo;
		private DataRow drActivo;
		private GeneradorTeclados generador;
		private PaginasArticulos pagActiva;
		private DatosTecla datosEnMovimiento;
		MiBoton TeclaPul;
		private AutoResetEvent puedoOrdenar = new AutoResetEvent(true);
		
		public VenTeclados(GesBaseLocal gsL) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			 Gtk.TargetEntry[] target = new Gtk.TargetEntry[] {new Gtk.TargetEntry ("STRING",Gtk.TargetFlags.App,0)};
	         Gtk.Drag.DestSet(this.btnSubirTeclado, Gtk.DestDefaults.All, target, Gdk.DragAction.Move);
	         Gtk.Drag.DestSet(this.btnBajarTeclado, Gtk.DestDefaults.All, target, Gdk.DragAction.Move);
			
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Teclas","IDTecla");
			this.btnAñadirArt.Sensitive = false;
			generador = new GeneradorTeclados(gesLocal,tbPrincipal);
			this.MostrarAceptar(false);
			this.modo = Modos.salir;
			this.btnAñadir.Visible = false;
			this.RellenarElTecladoDeTeclas();
			this.RellenaSecciones();
		}
		
		private void RellenarElTecladoDeTeclas(){
		    for(uint i = 4;i>0;i--){
                for(uint j= 8;j>0;j--){
                  this.tablaTeclado.Attach(new MiBoton(this.OnBtnEjAccionTecla),j-1,j,i-1,i);   
                }
		    }
		}
		
	    private void RellenarTeclado(){
	  
	    int pos = 0;
	     if(this.tablaTeclado.Children.Length>0){
	           foreach (Gtk.Widget tecla in this.tablaTeclado.Children){
	              if(tecla is Valle.GtkUtilidades.MiBoton){
	                  MiBoton estaTecla = ((MiBoton)tecla);
	                 
	                 if((pagActiva.NumPaginas>0)&&(pos<pagActiva.Pagina.Count)){
	                   estaTecla.Datos = pagActiva.Pagina[pos];
	                   estaTecla.Visible = true;
	                   estaTecla.ColorDeFono = pagActiva.Pagina[pos].colorDeAtras;
	                   estaTecla.Texto = pagActiva.Pagina[pos].nombreCorto;
	                  }else{
	                   estaTecla.Datos = null;
	                   estaTecla.Visible = false;
	                  }
	                  pos++;
	                }
	             }
	           }
	       
	      
	    }
		
		private void MostrarAceptar(bool mostrar){
		     this.btnBuscar.Visible = false;
			 this.btnAceptar.Visible = mostrar;
			 this.btnAñadir.Visible = false;
			 this.btnBorrar.Visible = !mostrar;
		   	 this.btnEditar.Visible = !mostrar;
		   	 this.txtNombre.Sensitive = mostrar;
        }
         
         private void OrdenarLista(){
           puedoOrdenar.WaitOne();
           int pos = 0;
              foreach(DatosTecla dato in pagActiva.ListaTeclas){
                  dato.DR["Orden"] = pos;
                  dato.Orden = pos;pos++;
                  this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+dato.DR["IDTecla"].ToString(),
                                             AccionesConReg.Modificar);
                  this.gesLocal.GuardarDatos(dato.DR,"IDTecla = "+dato.DR["IDTecla"].ToString(),
                                             AccionesConReg.Modificar);
				       
            }
            puedoOrdenar.Set();
         }
         
		 private void RellenaSecciones(){
			this.tbSecciones = gesLocal.ExtraerLaTabla("Secciones");
			Gtk.ListStore st = new Gtk.ListStore(typeof(String));
			this.cmbSecciones.Model = st;
			foreach(DataRow dr in tbSecciones.Rows){
				st.AppendValues(dr["Nombre"].ToString());
			}
			if(tbPrincipal.Rows.Count>0){this.cmbSecciones.Active = 0;}
        }		
        
        private bool CargarReg(){
         bool correcto = false;
         DataRow[] drs = tbPrincipal.Select("Nombre ='" +txtNombre.Text+"' AND IDTecla = "+ 
                          tbSecciones.Select("Nombre ='"+this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"].ToString());
           switch(modo){
            case Modos.Añadir:
             case Modos.Editar:
				  if ((drs.Length<=1)&& (this.txtNombre.Text.Length>0)&&(cmbSecciones.ActiveText!=null)){
                   this.datosEnMovimiento.nombreCorto = this.txtNombre.Text;
                    drActivo["Nombre"] = this.txtNombre.Text;
                     drActivo["IDSeccion"] = tbSecciones.Select("Nombre = '"+ this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"];
                        correcto = true;
                      }
                      break;
              
                      }
                      return correcto;
                      
        }
        
           protected void OnBtnEjAccionTecla(MiBoton.AccionesTecla accion, object res)
           {
                TeclaPul = (MiBoton)res;			
                DatosTecla dato = (DatosTecla)TeclaPul.Datos;
                      switch(accion)
                      {
                        case MiBoton.AccionesTecla.IniciarMover:
                            this.lblNombreArticulo.Text = dato.nombreLargo;
                            this.txtNombre.Text = dato.nombreCorto;
                            this.datosEnMovimiento = dato;
                            this.drActivo = dato.DR;
                        break;
                        case MiBoton.AccionesTecla.Mover:
                           if(((this.modo == Modos.Añadir)||(this.modo == Modos.Editar))&&                                 
                                      (!datosEnMovimiento.Equals(dato))){
                                this.pagActiva.ListaTeclas.Remove(datosEnMovimiento);
                                this.pagActiva.ListaTeclas.Insert(dato.Orden,datosEnMovimiento);
                                this.pagActiva.PaginarAriculos();
                                this.OnCmbSeccionesChanged(null,null);
                                Thread hOrdenar = new Thread(this.OrdenarLista);
				                hOrdenar.Start();
				            }
                        break;
                        case MiBoton.AccionesTecla.Clickado:
                          this.lblNombreArticulo.Text = dato.nombreLargo;
                          this.txtNombre.Text = dato.nombreCorto;
                          this.drActivo = dato.DR;
                          this.datosEnMovimiento = dato;
                        break;
                      }
           
                   }
         
				protected virtual void OnBtnSubirTecladoClicked (object sender, System.EventArgs e)
				{
				    if((pagActiva.NumPaginas >0)&&(pagActiva.PuntPagina>0)) 
				    {
				       pagActiva.PuntPagina--;
				       this.RellenarTeclado();
				    }
				}

				protected virtual void OnBtnBajarTecladoClicked (object sender, System.EventArgs e)
				{
				  if((pagActiva.NumPaginas >0)&&(pagActiva.PuntPagina<(pagActiva.NumPaginas-1)))
				    {
				       pagActiva.PuntPagina++;
				                this.RellenarTeclado();
				     
				    }
				}

				protected virtual void OnBtnAñadirArtClicked (object sender, System.EventArgs e)
				{
				   if(this.cmbSecciones.ActiveText != null){
				    VenElegirArt ven = new VenElegirArt(gesLocal,
				         tbSecciones.Select("Nombre ='"+this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"].ToString()
				                   ,this.CargarArticulos);
				        ven.Show();
				    }
				             
				}
                
                private void CargarArticulos(List<DataRow> drs){
				      
				      int ordenArticulos = pagActiva!=null ? pagActiva.MaxOrden: 0;
				      DataRow drNuevo;
		              object IDSeccion = tbSecciones.Select("Nombre ='"+this.cmbSecciones.ActiveText+"'")[0]["IDSeccion"];
				      for(int i=0; i < drs.Count;i++){
				              DataRow r = drs[i];
				                 drNuevo =  tbPrincipal.NewRow();
				                 drNuevo["IDArticulo"] = r["IDArticulo"];
				                 drNuevo["Nombre"] = r["Nombre"].ToString();
				                 drNuevo["Orden"] = ++ordenArticulos;
				                 drNuevo["IDSeccion"] = IDSeccion;
				                 tbPrincipal.Rows.Add(drNuevo); 
				               this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+drNuevo["IDTecla"].ToString(),
				                                       AccionesConReg.Agregar);
				               this.gesLocal.GuardarDatos(drNuevo,"IDTecla = "+drNuevo["IDTecla"].ToString(),
				                                       AccionesConReg.Agregar);
				               this.generador.AñadirTeclas(drNuevo,this.pagActiva);
				               
				                             
				                                       
				      }
				      this.pagActiva.PaginarAriculos();
				      this.RellenarTeclado();
				}
                
		        
				
		
				protected virtual void OnBtnAñadirSeccionClicked (object sender, System.EventArgs e)
				{
				  
				           VenSecciones ven = new VenSecciones(gesLocal);
				             ven.salirSecciones += new OnSalirSecciones(this.RellenaSecciones);
				              ven.Show();  
				}

				protected virtual void OnCmbSeccionesChanged (object sender, System.EventArgs e)
				{
				    if((this.cmbSecciones.ActiveText != null)&&
				          (generador.ListaDeTeclados.ContainsKey(this.cmbSecciones.ActiveText))){
				                  
				          this.pagActiva = generador.ListaDeTeclados[this.cmbSecciones.ActiveText];
				          this.RellenarTeclado();
				          
				    }
				   
				}

				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{
				     try{
				         switch(modo){
				            case Modos.Borrar:
				             this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Borrar);
				             this.gesLocal.GuardarDatos("Teclas","IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Borrar);
				             drActivo.Delete();
				             drActivo = null;
					         tbPrincipal.AcceptChanges();
					         this.pagActiva.ListaTeclas.Remove(this.datosEnMovimiento);
					         this.pagActiva.PaginarAriculos();
					         this.txtNombre.Text="";
				             this.lblNombreArticulo.Text = "Ninguna tecla activa";
				             this.lblInformacion.Text="Modo borrar activado";
				             this.RellenarTeclado();
				            break;
				           case Modos.Editar:
				             if(this.CargarReg()){
				             this.gesLocal.ActualizarSincronizar("Teclas","IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Modificar);
				             this.gesLocal.GuardarDatos(drActivo,"IDTecla = "+drActivo["IDTecla"].ToString(),AccionesConReg.Modificar);
                             this.TeclaPul.Texto = this.txtNombre.Text;
						     this.lblNombreArticulo.Text = this.txtNombre.Text;
				             this.lblInformacion.Text="Modo editar activado";
				             }else{
				                 this.lblInformacion.Text="Error al introducir los datos";
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
			            this.btnAñadirArt.Sensitive = true;
				       
				             this.lblInformacion.Text = "Modo editar activado"; 
				}

				protected virtual void OnBtnAñadirClicked (object sender, System.EventArgs e)
				{
				   this.modo = Modos.Añadir;
				        this.MostrarAceptar(true);
				        this.btnAñadirArt.Sensitive = true;
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
				          this.Destroy();
				          }else{
				          this.modo = Modos.salir;
				          this.MostrarAceptar(false);
				          this.lblInformacion.Text= "Modo no editable activado";
				          this.txtNombre.Text = "";
				          this.btnAñadirArt.Sensitive = false;
				          this.RellenarTeclado();
				          }
				}

				protected virtual void OnTxtNombreActivated (object sender, System.EventArgs e)
				{
				 this.btnAceptar.Click();
				}

				protected virtual void OnBtnSubirTecladoDragMotion (object o, Gtk.DragMotionArgs args)
				{
				  this.btnSubirTeclado.Click();
				}

				protected virtual void OnBtnBajarTecladoDragMotion (object o, Gtk.DragMotionArgs args)
				{
				     this.btnBajarTeclado.Click();
				}		
	}
}
