// User: valle at 2:55 25/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Drawing;
using System.Data;


using Valle.SqlUtilidades;
using Valle.Utilidades.Imagenes;


namespace Valle.GesTpv
{
	
	
	public partial class VenPlanig : Gtk.Window
	{
    	enum Modos { Editar, Borrar, Añadir, salir};		
		
	
	    private GesBaseLocal gesLocal;
		private DataTable tbPrincipal;
		private DataTable tbZonas;
		private Modos modo;
		private DibujoFlotante flotante;
		private Planing plano;
		private GestorPlaning gestorDePlano;
		private GestorEscenario escPrincipal;
		private DataRow drActivo;
		private bool pulsado;
		private InfMesa mesaSel;
		private BotonesObj ObjSel;
		private LosDatosMesa datosSel;
		private bool modificado;
		
		
		//para los graficos
		private Point DePuenteroALocObj;
		private Point DeCorMonitorAVentana;
		
		public InfMesa MesaSel{
		     set{
		        mesaSel = value;
		        if(value!=null){
		               this.drActivo = tbPrincipal.Select("IDMesa = " + mesaSel.IDObjeto)[0];
    	               this.txtNombre.Text = drActivo["Nombre"].ToString();
    	               this.txtRef.Text = mesaSel.Referencia;
	               }else{
	                   this.drActivo = null;
	                   this.txtNombre.Text = "";
	                   this.txtRef.Text = "";
	               }
		     }
		     get {
		        return mesaSel;
		     }
		
		}
	    
	  
		public VenPlanig(GesBaseLocal gsL) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			gesLocal = gsL;
			this.tbPrincipal = gesLocal.ExtraerLaTabla("Mesas","IDMesa");
			this.tbZonas = gesLocal.ExtraerLaTabla("Zonas","IDZona");
			this.modo = Modos.salir;
			
			
			//configurar el aspecto del control
			Gtk.ListStore modelo = new Gtk.ListStore(typeof(string),typeof(DataRow));
			this.cmbZonas.Model = modelo;
			this.MostrarAceptar(false);
			
		  
           //Inicializamos el gestor de planos
			Dimension dimMesas = new Dimension(90,90);
		    Dimension dimPlano = new Dimension(this.cImgPlano.Allocation.Width-80, this.cImgPlano.Allocation.Height);
		    this.gestorDePlano = new GestorPlaning(new Marco(new Localizacion(0,0), dimPlano),
		                                                    dimMesas, ConstantesImg.losas, Rutas.Principal+"/"+Rutas.IMG_APP+"/");
		                                                    
		    
		    //Inicializamos el escenario principal
		    this.escPrincipal = new GestorEscenario (this.OnModificadoEsc,
		                                  new Dimension(this.cImgPlano.Allocation.Width,this.cImgPlano.Allocation.Height),Rutas.Principal+"/"+Rutas.IMG_APP+"/");
            
            this.escPrincipal.CrearEscHerra(new Localizacion(this.cImgPlano.Allocation.Width-80,0),
                                                   new Dimension(80,this.cImgPlano.Allocation.Height));
            
            this.RellenarZonas();
        }
        
         
        
        
        
         
         void OnModificadoEsc(Bitmap img){
             this.cImgPlano.Pixbuf = new Gdk.Pixbuf(UtilImagenes.DeBitmapABytes(img));
         }
		
		private void MostrarAceptar(bool mostrar){
		     this.btnAceptar.Visible = mostrar;
			 this.btnEditar.Visible = !mostrar;
		   	 this.btnLimpiar.Visible = mostrar;
		   	 this.btnRotarDibujo.Visible = mostrar;
			 this.lblEscalado.Visible = mostrar;
			 this.spinEscalado.Visible = mostrar;
			 this.btnAplicarEsc.Visible = mostrar;
	    }
      
        
        
        private void RellenarZonas(){
            ((Gtk.ListStore)this.cmbZonas.Model).Clear();
            if(tbZonas.Rows.Count>0){
        	foreach(DataRow dr in tbZonas.Rows){
				((Gtk.ListStore)this.cmbZonas.Model).AppendValues(dr["Nombre"].ToString(),dr);
			}
			this.cmbZonas.Active = 0;
		   }
          }
         
        
        
        protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				     if(modo == Modos.salir){
				          this.Destroy();
				          }else{
				          this.OnBtnAceptarClicked(null,null);
				          this.modo = Modos.salir;
				          this.MostrarAceptar(false);
				          this.lblInformacion.Text= "Modo no editable activado";
				          this.txtRef.Text = "";
				          this.txtNombre.Text = ""; 
				          }
				}

			

				protected virtual void OnBtnEditarClicked (object sender, System.EventArgs e)
				{
				     this.modo = Modos.Editar;
				       this.MostrarAceptar(true);
				             this.lblInformacion.Text = "Modo editar activado";   
				}

				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{   
				    if(this.modificado){
				        this.gestorDePlano.GuardarPlano(plano);
				         modificado =false;
				      }
				}

			   

				protected virtual void OnCmbZonasChanged (object sender, System.EventArgs e)
				{
			
				  if((this.cmbZonas.ActiveText != null)&&(this.gestorDePlano!=null)){
				       if(modo != Modos.salir){
				             if(modificado){
				               this.gestorDePlano.GuardarPlano(plano);
				               this.modificado = false;
				               }
				               }
				          Gtk.TreeIter iter;
				          this.cmbZonas.GetActiveIter(out iter);
			              DataRow rF = (DataRow)this.cmbZonas.Model.GetValue(iter,1);
			              int IDZonaEditada = (int)rF["IDZona"];
			              if(plano!=null)
			                      this.escPrincipal.QuitarImagen(plano);
				          
				          plano = this.gestorDePlano.CargarPlano(this.cmbZonas.ActiveText, IDZonaEditada.ToString());
				          this.escPrincipal.AgregarImg(plano);
				          this.spinEscalado.Text = plano.DimDeObjetos.Ancho.ToString();
				          
				   }
				   
				}

				protected virtual void OnBtnAñadirZonaClicked (object sender, System.EventArgs e)
				{
				         VenZonas ven = new VenZonas(gesLocal,tbZonas);
				         ven.salirZonas += this.RellenarZonas;
				              
				}

				protected virtual void OnAreaDeDibujoButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
				{
				   pulsado = true;
				   this.MesaSel = escPrincipal.mesSel;
				   this.ObjSel = (BotonesObj) escPrincipal.ControlSel;
				   this.datosSel = escPrincipal.datosSel;
				   if(modo!= Modos.salir){
				    //Esto es para saber si es una pulsacion continuada o solo un clic
				    //espera 50 milisegundos y des pues comprueba que este pulsado todavia
				     GLib.Timeout.Add(70,Mover);
				     
				     //Aqui cojemos las diferencia que hay entre cordenadas del monitor y las de la ventana
				     this.DeCorMonitorAVentana.X=Convert.ToInt32(args.Event.XRoot - args.Event.X);
				     this.DeCorMonitorAVentana.Y=Convert.ToInt32(args.Event.YRoot - args.Event.Y);
				      
				      if(ObjSel !=null){//para saber la diferencia de cordenadas desde que pinchas hasta la esquina del objeto
				          this.DePuenteroALocObj.X = Convert.ToInt32(args.Event.X) - (ObjSel.LocRelAlPadre.X);
				          this.DePuenteroALocObj.Y = Convert.ToInt32(args.Event.Y) - (ObjSel.LocRelAlPadre.Y);
				      
				       }else if(MesaSel !=null){//igual pero para las mesas
				              this.DePuenteroALocObj.X=Convert.ToInt32(args.Event.X)- (MesaSel.LocRelAlPadre.X);
				              this.DePuenteroALocObj.Y=Convert.ToInt32(args.Event.Y)-(MesaSel.LocRelAlPadre.Y);
				          }else if(datosSel !=null){//igual pero para los datos mesas
				              this.DePuenteroALocObj.X=Convert.ToInt32(args.Event.X)- (datosSel.LocRelAlPadre.X);
				              this.DePuenteroALocObj.Y=Convert.ToInt32(args.Event.Y)-(datosSel.LocRelAlPadre.Y);
				            }
				       
				          
				             
				    }
				    
				}
				
				
			    bool Mover(){
			       
			       if(pulsado){
			        if((ObjSel != null)&&(!ObjSel.IDObjeto.Equals("papelera"))){
			          flotante = new DibujoFlotante(new Marco(
				                      ObjSel.LocRelAlPadre.X+this.DeCorMonitorAVentana.X,ObjSel.LocRelAlPadre.Y+DeCorMonitorAVentana.Y,plano.DimDeObjetos.Ancho,
				                              plano.DimDeObjetos.Alto), new System.Drawing.Image[]{new Bitmap(Rutas.Principal+"/"+Rutas.IMG_APP+"/"+ConstantesImg.losas),ObjSel.ImgObj});
				               
			          }else if((MesaSel !=null)){
			      
			             MesaSel.Oculto = true;
			             flotante =   new DibujoFlotante(new Marco(MesaSel.LocRelAlPadre.X+DeCorMonitorAVentana.X,
				          MesaSel.LocRelAlPadre.Y+this.DeCorMonitorAVentana.Y,MesaSel.Dim.Ancho,MesaSel.Dim.Alto),
				                              new System.Drawing.Image[]{new Bitmap(Rutas.Principal+"/"+Rutas.IMG_APP+"/"+ConstantesImg.losas),MesaSel.ImgObj});
				              
				        }else if((datosSel !=null)){
			             datosSel.Oculto = true;
			              flotante =   new DibujoFlotante(new Marco(datosSel.LocRelAlPadre.X+DeCorMonitorAVentana.X,
				            datosSel.LocRelAlPadre.Y+this.DeCorMonitorAVentana.Y,datosSel.Dim.Ancho,datosSel.Dim.Alto),
				                              new System.Drawing.Image[]{new Bitmap(Rutas.Principal+"/"+Rutas.IMG_APP+"/"+ConstantesImg.losas),datosSel.ImgObj});
				              
				        }
				      }
				        
                    return false;			       
			    }

				protected virtual void OnAreaDeDibujoMotionNotifyEvent (object o, Gtk.MotionNotifyEventArgs args)
				{
				        this.escPrincipal.PunteroEnPosicion(new Localizacion(Convert.ToInt32(args.Event.X),Convert.ToInt32(args.Event.Y)));
			            if((modo!= Modos.salir)&&(flotante != null)){
			                   flotante.Move(Convert.ToInt32(args.Event.XRoot)-this.DePuenteroALocObj.X,
				                                         Convert.ToInt32(args.Event.YRoot)-this.DePuenteroALocObj.Y);
				               if(datosSel == null){                          
				                if((escPrincipal.ControlSel != null)&&(escPrincipal.ControlSel.IDObjeto.Equals("papelera"))){
				                    flotante.CambiarDim(plano.DimDeObjetos.Ancho/2,plano.DimDeObjetos.Alto/2);      
				                }else{
				                    flotante.CambiarDim(plano.DimDeObjetos.Ancho,plano.DimDeObjetos.Alto);  
				                }
				              }
				   
				 }
			   }
				
				protected virtual void OnAreaDeDibujoButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
				{
				  pulsado = false;
	              	 
				   if(flotante !=null){
				     flotante.Destroy(); flotante = null;
				     
				     if(datosSel!= null){
				        datosSel.Oculto = false;
				        this.modificado = true;
				        this.plano.MoverObj(datosSel, new Localizacion(Convert.ToInt32(args.Event.X)-this.DePuenteroALocObj.X,
				                   Convert.ToInt32(args.Event.Y)-this.DePuenteroALocObj.Y));
				        escPrincipal.Seleccionar(datosSel);
				        escPrincipal.datosSel = datosSel;
				                 
				     }
				     
				     if((MesaSel!=null)&&(this.escPrincipal.ControlSel==null)){
				           MesaSel.Oculto = false;
				           this.modificado = true;
				           this.plano.MoverObj(MesaSel, new Localizacion(Convert.ToInt32(args.Event.X)-this.DePuenteroALocObj.X,
				                   Convert.ToInt32(args.Event.Y)-this.DePuenteroALocObj.Y));
				            escPrincipal.Seleccionar(MesaSel);
				            escPrincipal.mesSel = MesaSel;
				     }
				     
				     if((MesaSel!=null)&&(this.escPrincipal.ControlSel!=null)&&(this.escPrincipal.ControlSel.IDObjeto.Equals("papelera"))){
				         this.plano.QuitarObj(MesaSel);
				         this.modificado = true;
				     }
				     
				     if((MesaSel==null)&&(ObjSel!=null)){
				                Gtk.TreeIter iter;
            	                this.cmbZonas.GetActiveIter(out iter);
            			        DataRow rF = (DataRow)this.cmbZonas.Model.GetValue(iter,1);
            			        int IDZonaEditada = (int)rF["IDZona"];
				                DataRow[] drs = tbPrincipal.Select("IDZona = "+IDZonaEditada.ToString());
				                InfMesa mesa;
				                
				        if(this.plano.NumMesasPlano < drs.Length){
				              this.drActivo = drs[this.plano.NumMesasPlano];
				              mesa = new InfMesa(ObjSel.NomImg, new Dimension(plano.DimDeObjetos.Ancho,plano.DimDeObjetos.Alto),
				                                           new Localizacion(Convert.ToInt32(args.Event.X)-this.DePuenteroALocObj.X,
				                   Convert.ToInt32(args.Event.Y)-this.DePuenteroALocObj.Y),drActivo["Nombre"].ToString(),drActivo["IDMesa"].ToString());
				                   mesa.Referencia = "";
				              
				           }else{
				             
				              mesa = new InfMesa(ObjSel.NomImg, new Dimension(plano.DimDeObjetos.Ancho,plano.DimDeObjetos.Alto),
				                                           new Localizacion(Convert.ToInt32(args.Event.X)-this.DePuenteroALocObj.X,
				                   Convert.ToInt32(args.Event.Y)-this.DePuenteroALocObj.Y),"Nueva "+ (this.plano.NumMesasPlano+1),"");
				             
				               this.drActivo = this.tbPrincipal.NewRow();
				               drActivo["Nombre"] = "Nueva "+ (this.plano.NumMesasPlano+1);
				               drActivo["IDZona"] = IDZonaEditada;
                               drActivo["Orden"] = this.plano.NumMesasPlano;
                               this.tbPrincipal.Rows.Add(drActivo);
                               mesa.IDObjeto = drActivo["IDMesa"].ToString();
                               mesa.NomMesa = drActivo["Nombre"].ToString();
            				   gesLocal.ActualizarSincronizar("Mesas","IDMesa ="+drActivo["IDMesa"].ToString(),
				                           AccionesConReg.Agregar);
				                  this.gesLocal.GuardarDatos(drActivo,"IDMesa ="+drActivo["IDMesa"].ToString(),
				                           AccionesConReg.Agregar);
				                           
				                        
				               }
				              plano.AgregarObj(mesa);  
                              if(mesa.escPadre!=null){
				                             this.MesaSel = mesa;
				                             escPrincipal.Seleccionar(mesa);
				                             escPrincipal.mesSel = mesa;
				                             }
                           }
				          this.modificado = true; 
				     }
				}

				protected virtual void OnTxtNombreActivated (object sender, System.EventArgs e)
				{
				     if(this.modo != Modos.salir){
				        this.drActivo["Nombre"] = this.txtNombre.Text;
				         MesaSel.NomMesa = this.txtNombre.Text;
				         MesaSel.Referencia = this.txtRef.Text;
				          gesLocal.ActualizarSincronizar("Mesas","IDMesa ="+drActivo["IDMesa"].ToString(),
				                           AccionesConReg.Modificar);
				                  this.gesLocal.GuardarDatos(drActivo,"IDMesa ="+drActivo["IDMesa"].ToString(),
				                           AccionesConReg.Modificar);
				               }            
				}

	            public override void Destroy(){
	               base.Destroy();
	            }

	            protected virtual void OnBtnLimpiarClicked (object sender, System.EventArgs e)
	            {
	                this.gestorDePlano.BorrarPlano(plano);
	                this.escPrincipal.QuitarImagen(plano);
	                Gtk.TreeIter iter;
	                this.cmbZonas.GetActiveIter(out iter);
			        DataRow rF = (DataRow)this.cmbZonas.Model.GetValue(iter,1);
			        int IDZonaEditada = (int)rF["IDZona"];
			        plano = this.gestorDePlano.CargarPlano(this.cmbZonas.ActiveText, IDZonaEditada.ToString());
			        this.escPrincipal.AgregarImg(plano);
			          
	            }

	            protected virtual void OnBtnRotarDibujoClicked (object sender, System.EventArgs e)
	            {
	               if(MesaSel != null){
	                  MesaSel.RotarImagen(RotateFlipType.Rotate90FlipXY);
				   }
	            }

	            
	            protected virtual void OnBtnTextoClicked (object sender, System.EventArgs e)
	            {
			       if(modo != Modos.salir){
			                  if(this.modificado){
				                   this.gestorDePlano.GuardarPlano(plano);
				                   this.modificado = false;
				                   }
				                   
				                 
				               }
				          
				          new VenMesas(this.gesLocal);
			              this.Destroy();
	            }

	            protected virtual void OnTxtRefActivated (object sender, System.EventArgs e)
	            {
	                this.OnTxtNombreActivated(sender,e);
			    }

	            protected virtual void OnButton351Clicked (object sender, System.EventArgs e)
	            {
	              if(this.spinEscalado.Text.Length>0){
	                 this.gestorDePlano.GuardarPlano(plano);
			         if(plano.EscalarObjetos(new Dimension(Int32.Parse(this.spinEscalado.Text),Int32.Parse(this.spinEscalado.Text))))
			                                              this.gestorDePlano.GuardarPlano(plano);
			                                              else{
			                                               this.escPrincipal.QuitarImagen(plano);
			                                               plano = this.gestorDePlano.CargarPlano(plano.nomZona,plano.IDObjeto);
			                                               this.escPrincipal.AgregarImg(plano);
			                                               }
			                                             
			        }
			       
	            }

	            protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
	            {
	                this.OnBtnAceptarClicked(null,null);
	            }

	           
	            
	           
	
	}
}
