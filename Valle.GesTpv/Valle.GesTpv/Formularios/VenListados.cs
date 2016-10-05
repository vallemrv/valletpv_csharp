// VenListados.cs created with MonoDevelop
// User: valle at 20:00 05/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Threading;
using Valle.Distribuido;
using Valle.Distribuido.SQLRemoting;

namespace Valle.GesTpv
{
	
	
	public partial class VenListados : Gtk.Window
	{
	    SQLClient gesRemoto;
	    
		public VenListados(SQLClient gesR) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			gesRemoto = gesR;
		}
		
	
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
				  this.gesRemoto.PararConsultasAsync();
				  this.Destroy();
				}

				

				protected virtual void OnBtnEjConsultaClicked (object sender, System.EventArgs e)
				{
				   this.btnEjConsulta.Sensitive = false;
			       if(this.txtConsulta.Buffer.Text.ToUpper().StartsWith("SELECT")){
				       gesRemoto.ConsultaDrAsync(this.txtConsulta.Buffer.Text, this.drEncontrado);
			          }else{ 
				       this.lblInformacion.Text = "Registros afectados "+gesRemoto.EjConsultaNoSelect("",txtConsulta.Buffer.Text).ToString();
				       this.btnEjConsulta.Sensitive = true;
			            }
	        	}
		
		       void drEncontrado(int progreso, int total, DataRow dr, string[] nomColumnas){
		          Gtk.Application.Invoke(delegate{
		                   if(progreso>=total) this.btnEjConsulta.Sensitive = true;
		                   if(total>0){
                              if(progreso<=1)
                                     this.arboldevista1.AgregarColumnas(nomColumnas);
                                     
		                     this.arboldevista1.AgregarRegistro(dr,nomColumnas);
		                     this.lblInformacion.Text = "Agregado "+progreso.ToString()+" de "+ total.ToString();
		                    }else
		                      this.lblInformacion.Text = "La consulta no ha obtenido resultados";
		                    
		                    });
		          
		       }

		       protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		       {
		          this.gesRemoto.PararConsultasAsync();
		       }

		       protected virtual void OnButton339Clicked (object sender, System.EventArgs e)
		       {
		         this.gesRemoto.PararConsultasAsync();
		       }
		       
		       

	}
}
