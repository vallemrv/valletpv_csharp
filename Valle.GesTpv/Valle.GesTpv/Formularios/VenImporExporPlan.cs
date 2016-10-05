// VenImporExporPlan.cs created with MonoDevelop
// User: valle at 15:41 23/06/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Data;

using Valle.Distribuido;
using Valle.GtkUtilidades;

namespace Valle.GesTpv
{
    
    
    public partial class VenImporExporPlan : Gtk.Window
    {
        GesRemFicheros gesFicheros;
        DataTable tb;
        Valle.Distribuido.InfFichero[] listFich;
        Splash sp;
	 
        
        public VenImporExporPlan(GesServidores gesServ, GesBaseLocal gesLocal) : 
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            gesFicheros = new GesRemFicheros(gesServ.ServidorActivo[GesServidores.IP].ToString(),
                                                            (int)gesServ.ServidorActivo[GesServidores.PUERTO]);
            
            gesFicheros.listaRecibida += this.OnListaRecibida;
            gesFicheros.CanDatosEnviados += this.OnCantidadDatosEnv;
            gesFicheros.CanDatosRecibidos += this.OnCanDatosRecibidos;
            gesFicheros.ficheroRecibido += this.OnFicheroRecibido;
            
            tb = gesLocal.ExtraerLaTabla("Rutas");
            
            Gtk.ListStore list = new Gtk.ListStore(typeof(bool),typeof(string),typeof(string));
            this.treeArchivos.AppendColumn("",new Gtk.CellRendererToggle(),"active",0);
            this.treeArchivos.AppendColumn("Nombre plano",new Gtk.CellRendererText(),"text",1);
            this.treeArchivos.AppendColumn("Fecha modificado", new Gtk.CellRendererText(), "text", 2);
            this.treeArchivos.Model = list;
            
           
        }
        
        
        void RellenarListaFich(){
             ((Gtk.ListStore)this.treeArchivos.Model).Clear();
             foreach(InfFichero inf in this.listFich){
                ((Gtk.ListStore)this.treeArchivos.Model).AppendValues(false,inf.Nom_Fichero,inf.FechaModificacion);
             }
        
        }
        
        void OnListaRecibida(InfFichero[] inf){
          this.listFich = inf;
        }
        
        void OnCantidadDatosEnv(int can, int total){
          sp = new Splash("Importando tablas", Rutas.Ruta_Directa("/"+Rutas.IMG_APP+"/comunicacion.gif"),true);
        }
        
        void OnCanDatosRecibidos(int can){
         sp = new Splash("Importando tablas", Rutas.Ruta_Directa("/"+Rutas.IMG_APP+"/comunicacion.gif"),true);
        }
        
        void OnFicheroRecibido(string path){
        
        }

        protected virtual void OnRdoCambiados (object sender, System.EventArgs e)
        {
            if(this.rdoPlanosLoc.Active){
               this.listFich = 
                        this.gesFicheros.CrearListaFicheros(
                                   this.gesFicheros.ExListaDeFicherosLoc(Rutas.Ruta_Directa(Rutas.PLANING)));
                 this.btnImportar.Label = "Subir Ficheros";
            }else{
               this.gesFicheros.ListaDeFicherosRem(tb.Select("identificacion = planos")[0]["Ruta"].ToString());
                this.btnImportar.Label = "Descargar Fichros";
            }
        }

        protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
        {
             this.gesFicheros.Dispose();
        }

        protected virtual void OnBtnSalirClicked (object sender, System.EventArgs e)
        {
        }

        protected virtual void OnBtnImportarClicked (object sender, System.EventArgs e)
        {
           if(this.rdoPlanosLoc.Active){
              sp = new Splash("Enviando ficheros", Rutas.Ruta_Directa("/"+Rutas.IMG_APP+"/comunicacion.gif"),true);
              }else{
              sp = new Splash("Reciviendo ficheros", Rutas.Ruta_Directa("/"+Rutas.IMG_APP+"/comunicacion.gif"),true);
              }
        }

        
        
    }
}
