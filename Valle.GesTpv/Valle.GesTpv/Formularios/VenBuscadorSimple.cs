// VenBuscadorSimple.cs created with MonoDevelop
// User: valle at 23:14 21/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;


namespace Valle.GesTpv
{
	public delegate void OnIdEncontrado(DataRow r);
	
	public partial class VenBuscadorSimple : Gtk.Window
	{
		

		private event OnIdEncontrado idEncontrado;
		private DataTable tbBusqueda;
		private String[] columVistas;
		private string columBusqueda;
		Valle.Utilidades.HBuscador hbus;
			
		public VenBuscadorSimple(OnIdEncontrado idEnc, DataTable tb, String[] columVista,
                            string columBus) :  base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			idEncontrado += idEnc;
			tbBusqueda = tb;			
			this.columVistas =columVista;
		    this.columBusqueda = columBus;
			arboldevista1.AgregarColumnas(columVista);
            hbus = new Valle.Utilidades.HBuscador(RellenaLista,tbBusqueda);
			
		}
		
	    private void RellenaLista(DataRow dr){
		 Gtk.Application.Invoke(delegate {
		  if(dr!=null)
	       arboldevista1.AgregarRegistro(dr,columVistas);	
			else
				arboldevista1.Modelo.Clear();
				});
		}
		
		public void BuscarReg(){
			      hbus.Buscar(this.columBusqueda +" like '"+this.txtPalBus.Text+"%'");
	     }
		
	   
		       

		        protected virtual void OnBtnCancelarClicked (object sender, System.EventArgs e)
			    {
			             this.hbus.Parar();
			             this.Destroy();
		        }

		        protected virtual void OnTxtPalBusChanged (object sender, System.EventArgs e)
		        {
		             hbus.Parar();  
			         arboldevista1.Modelo.Clear();
		             this.BuscarReg();
		        }

		        protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		        {
		          hbus.Parar();
		          this.Destroy();
		          args.RetVal = false;
		        }

		        protected virtual void OnArboldevista1SeleccionCambiada (object sender, System.EventArgs e)
		        {
			          this.idEncontrado(arboldevista1.ExtraerRegSelec());
                            this.hbus.Parar();
                            this.HideAll();
			                this.Dispose();
                      
		        }

				
	}
}
