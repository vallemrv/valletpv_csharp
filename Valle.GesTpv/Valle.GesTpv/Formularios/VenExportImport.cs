// VenImportar.cs created with MonoDevelop
// User: valle at 20:12 19/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Threading;
using Valle.Distribuido.SQLRemoting;

namespace Valle.GesTpv
{
	
	public delegate void OnSalirImporExpor();
	public enum ModVen {Importar, Exportar};
	
	public partial class VenExportImport : Gtk.Window
	{
		private GesBaseLocal gesLocal;
		private SQLClient gesRemoto;
		private Gtk.ListStore stRemoto;
		private ImportarExportar imporExpor;
		private ModVen modo;
		private string[] listaTablas;
		private string[] listaIndividual;
	                               
		
		public event OnSalirImporExpor SalirImportar;
		
		public VenExportImport(GesBaseLocal gesL, SQLClient gesR,ModVen modo) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			gesLocal = gesL;
			gesRemoto = gesR;
			imporExpor = new ImportarExportar(gesLocal,gesRemoto,this.OpTeminada);
			this.modo  = modo;
			
			stRemoto = new Gtk.ListStore(typeof(String));
			
			lstRemoto.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			lstRemoto.Model = stRemoto;
			
			switch(modo){
			   case ModVen.Exportar:
			    this.label3.Text = "Lista de tablas a exportar";
			    this.listaIndividual = gesLocal.listaTablasIndividual;
			    this.Title = "Exportar datos al tpv";
			    this.btnImportar.Label = "Ejecutar la exportacion";
				this.btnSalir.Label = "Salir exportar";
			    this.lblInforamcion.Text ="ATENCION: Los datos se envian al Tpv";
			    this.radioIndividual.Label = "Exportar solo los datos individuales";
			    this.radioTodas.Label= "Exportar todos los datos (Requiere sincronizar todos los tpv)";
			   break;
			   case ModVen.Importar:
			     this.label3.Text = "Lista de tablas a importar";
			     this.Title = "Importar datos al tpv";
			     this.listaIndividual = gesLocal.listaTablasIndividual;
			     this.btnImportar.Label = "Ejecutar la importacion";
				 this.btnSalir.Label = "Salir importar";
			     this.lblInforamcion.Text ="ATENCION: Los datos se traeran del Tpv";
			     this.radioIndividual.Label = "Importar solo los datos individuales";
			     this.radioTodas.Label= "Importar todos los datos";
			  
			   break;
			}
			
			
          	
          	rellenarListas();
		}
		
         private void rellenarListas(){
              
              listaTablas = this.radioIndividual.Active ? this.listaIndividual : gesLocal.listaTablasLocales;
          	
			  stRemoto.Clear();
			  foreach(String s in this.listaTablas){
				         stRemoto.AppendValues(s);
          		   }
	        }
		
		
		
		
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
			        this.OnDeleteEvent(null,null);
				}
                
				protected virtual void OnBtnImportarClicked (object sender, System.EventArgs e)
				{
			          this.btnSalir.Sensitive = false;
			          this.btnImportar.Sensitive = false;
			          if(modo== ModVen.Importar)
			                this.imporExpor.Importar(this.listaTablas);
			                else
			                 this.imporExpor.Exportar(this.listaTablas);
			          
				}

             
          
              protected virtual void OnRadioTodasToggled (object sender, System.EventArgs e)
              {
                 rellenarListas();
              }

              
              protected void OpTeminada(){
              
                     if((this.modo.Equals(ModVen.Exportar))&&(this.radioTodas.Active))
                                     gesLocal.gesServ.SeHanExportadoComunes();
                                     
				      this.btnSalir.Sensitive = true;
			          this.lblInforamcion.Text = "Operacion terminada con exito";
				}

              protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
              {
                    this.Destroy();
			        if(SalirImportar != null){SalirImportar();}
		      }

              
	}
}
