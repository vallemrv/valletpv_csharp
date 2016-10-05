// VenSincronizar.cs created with MonoDevelop
// User: valle at 20:01 08/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

using Valle.Distribuido.SQLRemoting;
using Valle.SqlGestion;

namespace Valle.GesTpv
{
	public delegate void OnSalirSincronizar();
	
	public partial class VenSincronizar : Gtk.Window
	{
	
	    private Gtk.ListStore stRemoto;
		private Gtk.ListStore stLocal;
		private ImportarExportar Sincronizar;
		private GesBaseLocal gesLocal;
		private SQLClient gesRemoto;
		
		public event OnSalirSincronizar SalirSincronizar;
	
		
		public VenSincronizar(GesBaseLocal gesL, SQLClient gesR) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			gesLocal = gesL;gesRemoto = gesR; 
		    Sincronizar = new ImportarExportar(gesL,gesR,this.OpTerminada);
			stRemoto = new Gtk.ListStore(typeof(String),typeof(string));
			stLocal = new Gtk.ListStore(typeof(String),typeof(string));
			
			lstRemoto.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			lstRemoto.AppendColumn("Registros",new Gtk.CellRendererText(),"text",1);
			
			lstRemoto.Model = stRemoto;
			lstLocal.AppendColumn("Nombre",new Gtk.CellRendererText(),"text",0);
			lstLocal.AppendColumn("Registros",new Gtk.CellRendererText(),"text",1);
			
			lstLocal.Model = stLocal;
			rellenarListas();
		}
		
         private void rellenarListas(){
			 DataTable tbLocal = gesLocal.EjConsultaSelect("Sincronizados", 
                                     "SELECT TablaPertenencia, COUNT(TablaPertenencia) AS NumReg FROM Sincronizados WHERE "+
                                            GesBaseLocal.IDSERV+" = "+gesLocal.gesServ.ServidorActivo["IDServidor"].ToString()+
                                            " GROUP BY (TablaPertenencia)");
             DataTable tbRemoto = gesRemoto.EjConsultaSelect("Sincronizados", 
             "SELECT TablaPertenencia, COUNT(TablaPertenencia) AS NumReg FROM Sincronizados GROUP BY (TablaPertenencia)");
             
               stLocal.Clear();
			   stRemoto.Clear();
			   
			   foreach(DataRow rl in tbLocal.Rows){
			        stLocal.AppendValues(rl[0].ToString(),rl[1].ToString());
                    }
                    
               if(tbRemoto!=null){      
                   foreach(DataRow rR in tbRemoto.Rows){
    			        stRemoto.AppendValues(rR[0].ToString(),rR[1].ToString());
                        }
               }
			
		}
		
		
		        protected void OpTerminada(){
		              this.rellenarListas();
		              this.btnSalir.Sensitive = true;
		              this.lblInforamcion.Text = "TPV Sincronizado, operacion realizada con exito";
			    }
		
				protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
				{
			        this.Destroy();
				    if(SalirSincronizar != null) {SalirSincronizar();}
				}
                
				protected virtual void OnBtnExportarClicked (object sender, System.EventArgs e)
				{
				      this.btnSalir.Sensitive = false;
			          this.btnExportar.Sensitive = false;
			          this.Sincronizar.Sincronizar();
			    }
			    
			    
                   
				 

	}
}
