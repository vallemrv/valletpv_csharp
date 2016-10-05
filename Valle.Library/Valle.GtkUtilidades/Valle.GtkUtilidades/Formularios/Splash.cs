// Splash.cs created with MonoDevelop
// User: valle at 21:26 30/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using System.Collections.Generic;


namespace Valle.GtkUtilidades
{
	public partial class Splash : Gtk.Window, Valle.Utilidades.ISplash
	{
		
		public Splash(string titulo, string dirFoto,bool animado) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Contruir();
			if(animado){
			   this.imagenSplash.PixbufAnimation = new Gdk.PixbufAnimation(dirFoto);
			}else{
			   this.imagenSplash.Pixbuf = new Gdk.Pixbuf(dirFoto);
			   }
			if(titulo == null) {
			    this.lblTitulo.Visible = false;
			    }else{
			     this.lblTitulo.Visible = true;
	             this.lblTitulo.Text = titulo;
			    }
	        this.lblInformacion.Visible = false;
	        this.KeepAbove = true;
	        this.Title = titulo;
	    }
		
		public String Titulo
		{
		   set{
		      this.lblTitulo.Visible = true;
	             this.lblTitulo.Text = value;
	             this.Title = Titulo;
	             }
	        get{ return this.lblTitulo.Text;}     
			  
		}
		
	    
	    public void MostrarInformacion(InfAMostrar inf){
	           switch(inf.tipo){
	               case TipoInfMostrar.MensajeBarra:
	                this.MostrarInformacionBar(inf.Informacion, inf.dirBarra);
	               break;
	               case TipoInfMostrar.MensajeLabel:
				    this.MostrarMensajes(inf.Mensaje);
			       break;
	               case TipoInfMostrar.progresoBarr:
	                this.MostrarProgreso(inf.Progreso, inf.dirBarra);
	               break;
			   }
           
	    }
	  
	    
	    void MostrarInformacionBar(string mensajesInf, DirBarra dir){
	            Gtk.Application.Invoke (delegate { 
				        switch(dir){
				           case DirBarra.BarUno:
                            this.barInformacion.Text = mensajesInf;
					        break;
					       case DirBarra.BarDos:
                            this.barInformacion2.Text = mensajesInf;
					       break;
				           
				             }
                            });
             
                       }
		
	   	void MostrarMensajes(string mensajesInf){
             Gtk.Application.Invoke (delegate {
                            this.lblInformacion.Visible = mensajesInf.Length >0;
                            this.lblInformacion.Text= mensajesInf;
                            });
             
                       }

        void MostrarProgreso(double progreso, DirBarra dir){
                        Gtk.Application.Invoke (delegate {
                         switch(dir){
                           case DirBarra.BarUno:
                            this.barInformacion.Fraction = progreso;
                            break;
                            case DirBarra.BarDos:
                             this.barInformacion2.Visible = progreso < 1;
                             this.barInformacion2.Fraction = progreso;
                            break;
                            }
                            });
                       }
     	
		#region ISplash implementation
		public void mostrarInformacion (string me)
		{
			 Gtk.Application.Invoke(delegate{	
			    	this.barInformacion.Text = me;
			   });
			
		}

	    public	void mostrarProgreso (int pro)
		{
		   Gtk.Application.Invoke(delegate{	
				this.barInformacion.Fraction = (double)pro/(double)maxProceso;
			});
			
		}

		public void guardar ()
		{
			this.Dispose();
		}
		
		int maxProceso = 100;
		public int MaxProceso {
			get {
			    
				return this.maxProceso;
				
			}
			set {
			 Gtk.Application.Invoke(delegate{	
			    	this.maxProceso = value;
				});
				
			}
		}

		public int Progreso {
			get {
				  
			 	  return Convert.ToInt32(this.barInformacion.Fraction*maxProceso);
			}
			set {
			  
			 Gtk.Application.Invoke(delegate{	
			 	this.barInformacion.Fraction = (double) value/(double)maxProceso;
				});
			}
		}
		#endregion
	}
}