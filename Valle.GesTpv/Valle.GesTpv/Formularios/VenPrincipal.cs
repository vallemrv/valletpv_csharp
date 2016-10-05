// MainWindow.cs created with MonoDevelop
// User: valle at 22:54 11/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.IO;
using System.Collections.Generic;
using Gtk;
using System.Data;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;

using Valle.GtkUtilidades;
using Valle.Distribuido.SQLRemoting;

namespace Valle.GesTpv{
public partial class VenPrincipal: Gtk.Window
{	
    SQLClient gesRemoto;
    GesServidores gesServ;
	GesBaseLocal gesLocal;
	Splash sp;
		
		
	public VenPrincipal (Splash spl): base (Gtk.WindowType.Toplevel)
	{
		   Build ();
		   this.Hide();
		   
			
		  //Creamos los directorios necesarios para la aplicacion
		  new Rutas(); 
		  
		  //hilo para la creacion de clases auxiliares, y caraga de datos
		  sp = spl;
		  new Thread(new ThreadStart(this.hiloInicializacion)).Start();
	}
	
	private void hiloInicializacion(){
	       	sp.MostrarInformacion(new InfAMostrar("Cargando formulario principal",TipoInfMostrar.MensajeBarra));
    		sp.MostrarInformacion(new InfAMostrar(1.0/10.0,TipoInfMostrar.progresoBarr));
    	         
	       gesRemoto = new SQLClient(); 
		       
		   sp.MostrarInformacion(new InfAMostrar("Cargando tabla de servidores",TipoInfMostrar.MensajeBarra));
		   sp.MostrarInformacion(new InfAMostrar(2.0/10.0,TipoInfMostrar.progresoBarr));
		
		 	gesServ = new GesServidores();
		    
		    sp.MostrarInformacion(new InfAMostrar("Cargando conjunto de datos",TipoInfMostrar.MensajeBarra));
	    	sp.MostrarInformacion(new InfAMostrar(7.0/10.0,TipoInfMostrar.progresoBarr));
	      
	        gesLocal = new GesBaseLocal(gesServ);
			
	    	sp.MostrarInformacion(new InfAMostrar("Actualizando el aspecto del formulario",TipoInfMostrar.MensajeBarra));
		    sp.MostrarInformacion(new InfAMostrar(9.0/10.0,TipoInfMostrar.progresoBarr));
		    Thread.Sleep(1000);
		    
		    Application.Invoke (delegate {
		    inicializarMenu();
			inicializarComServ();
			this.lblInformacion.Text = "Esperando acciones";
		    	});
		   	
		   	sp.MostrarInformacion(new InfAMostrar("Finalizando .......",TipoInfMostrar.MensajeBarra));
		    sp.MostrarInformacion(new InfAMostrar(10.0/10.0,TipoInfMostrar.progresoBarr));
		    Thread.Sleep(1000);
		    Application.Invoke (delegate {this.Show();this.Maximize();});
			sp.Destroy();
	}
			
	private void inicializarComServ(){
	    ((ListStore)this.comboServidores.Model).Clear();
	    this.comboServidores.AppendText("Ningun servidor activo");
	    foreach(DataRow dr in gesServ.TbServidores.Rows){
				this.comboServidores.AppendText(dr["Nombre"].ToString());
			}
			
	 
		if(gesServ.ServidorActivo!=null)
		 this.comboServidores.Active = gesServ.TbServidores.Rows.IndexOf(gesServ.ServidorActivo)+1;
		  else
		  this.comboServidores.Active = gesServ.TbServidores.Rows.Count;
		  
		  
	}
	
	private void inicializarMenu(){
	     	this.actAlmacen.Sensitive = 
			(this.ArticulosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Articulos")&&
			 gesLocal.ContieneTabla("Inventarios")&& gesLocal.ContieneTabla("Almacen")&&
			 gesLocal.ContieneTabla("Familias") && gesLocal.ContieneTabla("VentaPorKilos") &&
			 gesLocal.ContieneTabla("ArticuloNoVenta") && gesLocal.ContieneTabla("DesgloseArt"));
			 
			 
			this.CamarerosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Camareros");
			this.actColores.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Colores");
			this.actFamiliasArticulos.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Familias");
			this.CamarerosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla ("Camareros");
			this.ConfigurarTpvSAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("TPVs");
			this.ZonasAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Zonas");
			this.MesasAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Mesas");
			this.SeccionesAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Secciones");
			this.TecladosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Teclas");
			this.UsuariosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Usuarios");
			this.PrivilegiosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Privilegios");
			this.SeguridadAction.Sensitive = gesServ.ServidorActivo!=null && this.UsuariosAction.Sensitive && this.PrivilegiosAction.Sensitive;
		    this.TeclasFavoritosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("TeclasFav");
		    this.CambioPreciosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Articulos");
			this.ElementosTpvAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.NumeroDeTablas()>10;
			this.GestionCamarerosAction.Sensitive = gesServ.ServidorActivo!=null && gesLocal.ContieneTabla("Camareros");
			this.btnConectar.Sensitive =  gesServ.ServidorActivo!=null &&
			           !this.btnDesconectar.Sensitive;
			   
		}
		
	protected void OpTeminadaImp(){
	   if((bool)gesServ.ServidorActivo["ExportarComun"])
		 {
		 ImportarExportar impEx = new ImportarExportar(gesLocal,gesRemoto,this.OpTerminadaExp);
	     
		    impEx.Exportar(gesLocal.listaTablasExportables);
		    gesServ.ServidorActivo["ExportarComun"] = false;
		    gesServ.GuardarDatos();
		 }else{
		   this.OpTerminadaExp();
		 }
	}
	
	protected void OpTerminadaExp(){
	                  this.inicializarMenu();
				      this.conexionRealizada();
				}
	
	private void importarTablas(){
	  ImportarExportar impEx = new ImportarExportar(gesLocal,gesRemoto,this.OpTeminadaImp);
	  bool exportarComunes = false;
	  
	  if(gesLocal.NumeroDeTablas() < gesLocal.listaTodasLasTablas.Length){
	       List<string> listTablas = new List<string>(); 
	         foreach(string tabla in gesLocal.listaTablasLocales){
	            if(!gesLocal.ContieneTabla(tabla)){
	               listTablas.Add(tabla);
	                if(gesLocal.PertenezeA(gesLocal.listaTablasComunes,tabla)){ 
        		                           exportarComunes = true;
        		                           gesServ.ServidorActivo["ExportarComun"] = false;
        		                    }
        		         }
        		   }      
        		   
        	 impEx.Importar(listTablas.ToArray());
        	 
             if(exportarComunes){
        	  this.gesServ.SeHanExportadoComunes();
        	}
           		
			 
		 }else 
		    if((bool)gesServ.ServidorActivo["ExportarComun"])
		     this.OpTeminadaImp();
    		 else
	    	   this.OpTerminadaExp();
		 
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
	    this.gesLocal.Terminar();
		Application.Quit ();
		a.RetVal = true;
		
	}

	protected virtual void OnConectarActionActivated (object sender, System.EventArgs e)
	{
				
		    gesRemoto.Conectar((int)gesServ.ServidorActivo[GesServidores.PUERTO],gesServ.ServidorActivo[GesServidores.IP].ToString(),
		                                                                        gesServ.ServidorActivo[GesServidores.PROTOCOLO].ToString());
    
				if (gesRemoto.EstaConectado){
				   Console.WriteLine("Estoy aki");
				    //new GesPlantillasTb(gesRemoto).CrearPlantilla("EsqValleTpv");	
			        this.lblInformacion.Text = "Conexion realizada con exito ";
                    this.lblEstado.Text = "Conectado";
			    	importarTablas();// importar las tablas si es necesario
                 }else{
             	  this.lblInformacion.Text =  "Error en la conexion puede que no tega el servidor encedido";
				  this.lblEstado.Text = "No conectado";
				  gesRemoto.Desconectar();
				 }
				
		
	}
	
	private void conexionRealizada(){
	          	this.lblInformacion.Text = ("Conexion realizada con exito");
                this.lblEstado.Text = "Conectato";
                this.btnConectar.Sensitive = false;
				this.btnDesconectar.Sensitive = true;
				this.HacerCopiasSeguridadAction.Sensitive = true;
				this.mImportar.Sensitive = true;
				this.ExportarAction.Sensitive = true;
			    this.VentasAction.Sensitive = true;
				this.ListadosAction.Sensitive = true;
				this.ReiniciarTpvAction.Sensitive = true;
		        this.VerBotonSinc();
	 }
	
	private void VerBotonSinc(){
	
	    		            
              this.btnSincronizar.Sensitive = gesLocal.ExtraerLaTabla(GesBaseLocal.NOM_TB_SINCRONIZADOS)
	   			            .Select(GesBaseLocal.IDSERV+" = "+ gesServ.ServidorActivo["IDServidor"].ToString()).Length > 0
				                          || gesRemoto.NumRegEnTabla(GesBaseLocal.NOM_TB_SINCRONIZADOS)> 0;
				                          
	}

	protected virtual void OnConfiguracionActionActivated (object sender, System.EventArgs e)
	{
	        OnBtnDesconectarActivated(null, null);
	        this.lblInformacion.Text = "Configurando servidores";
		    VenConfigCon conf = new VenConfigCon(gesServ, AccionVenConfig);
			conf.Show();
	}
	
    private void AccionVenConfig(AccionesGesServ accion, object o){
          switch(accion){
            case AccionesGesServ.Salir:
              this.lblInformacion.Text = ("Esperando acciones");
              this.inicializarComServ();
              this.gesLocal.CambiarServidor(gesServ.ServidorActivo["Nombre"].ToString());
			  this.inicializarMenu();
			break;
			case AccionesGesServ.Borrar:
			 this.gesLocal.EliminarDatosServ(((DataRow)o)["Nombre"].ToString());
			 gesServ.TbServidores.Rows.Remove(((DataRow)o));
	         this.gesServ.GuardarDatos();
	         this.inicializarComServ();
					              
			break;
			}
    }

	protected virtual void OnBtnDesconectarActivated (object sender, System.EventArgs e)
	{
	        if(this.btnDesconectar.Sensitive){
     			this.btnConectar.Sensitive = this.gesServ.ServidorActivo!=null;
				this.btnDesconectar.Sensitive = false;
				this.HacerCopiasSeguridadAction.Sensitive = false;
				this.mImportar.Sensitive = false;
				this.ExportarAction.Sensitive = false;
			    this.VentasAction.Sensitive = false;
				this.ListadosAction.Sensitive = false;
				this.ReiniciarTpvAction.Sensitive = false;
				this.btnSincronizar.Sensitive = false;
				this.lblInformacion.Text = "Ha finalizado la conexion";
				this.lblEstado.Text = "Desconectado";
				this.gesRemoto.Desconectar();
			    }
			    
	}

    protected virtual void OnMImportarActivated (object sender, System.EventArgs e)
    {
              this.lblInformacion.Text =("Inportacion de tablas al servidor");
              VenExportImport venImp = new VenExportImport(gesLocal,gesRemoto,ModVen.Importar);
			  venImp.SalirImportar += new OnSalirImporExpor(this.VerBotonSinc);
			  venImp.Show();
    }

    protected virtual void OnActArticulosActivated (object sender, System.EventArgs e)
    {
            this.lblInformacion.Text = ("Modificar articulos ......");
            OnBtnDesconectarActivated(null,null);
			VenArticulos venArt = new VenArticulos(gesLocal);
			venArt.Show();
    }

    protected virtual void OnActColoresActivated (object sender, System.EventArgs e)
    {
        OnBtnDesconectarActivated(null,null);
        VenColores venCol = new VenColores(gesLocal);
           venCol.Show();
			
    }

    protected virtual void OnActFamiliasArticulosActivated (object sender, System.EventArgs e)
    {
         OnBtnDesconectarActivated(null,null);
         VenFamilias venFam = new VenFamilias(gesLocal);
           venFam.Show();
    }

    protected virtual void OnSeccionesActionActivated (object sender, System.EventArgs e)
    {
        OnBtnDesconectarActivated(null,null); 
        VenSecciones venSec = new VenSecciones(gesLocal);
            venSec.Show();
    }

    protected virtual void OnTpvSActionActivated (object sender, System.EventArgs e)
    {
            OnBtnDesconectarActivated(null,null);
           Valle.GesTpv.VenConfigTpv ven = new Valle.GesTpv.VenConfigTpv(gesLocal);
             ven.Show();
         
    }

    protected virtual void OnTecladosActionActivated (object sender, System.EventArgs e)
    {
             OnBtnDesconectarActivated(null,null);
            VenTeclados ven = new VenTeclados(gesLocal);
               ven.Show();
            
    }

    protected virtual void OnCamarerosActionActivated (object sender, System.EventArgs e)
    {
           OnBtnDesconectarActivated(null,null);
           VenCamareros ven = new VenCamareros(gesLocal);
              ven.Show();
    }

    protected virtual void OnMesasActionActivated (object sender, System.EventArgs e)
    {
             OnBtnDesconectarActivated(null,null);
              VenPlanig ven = new VenPlanig(gesLocal);
                ven.Show();
    }

    protected virtual void OnZonasActionActivated (object sender, System.EventArgs e)
    {
                OnBtnDesconectarActivated(null,null);
                VenZonas ven = new VenZonas(gesLocal);
                  ven.Show();
    }

    protected virtual void OnCambioPreciosActionActivated (object sender, System.EventArgs e)
    {
		  OnBtnDesconectarActivated(null,null);
            new  VenPrecios(gesLocal);
    }

    protected virtual void OnUsuariosActionActivated (object sender, System.EventArgs e)
    {
             OnBtnDesconectarActivated(null,null);
             VenUsuarios ven = new VenUsuarios(gesLocal);
                ven.Show();
    }

    protected virtual void OnPrivilegiosActionActivated (object sender, System.EventArgs e)
    {
               OnBtnDesconectarActivated(null,null);
               VenPrivilegios ven = new VenPrivilegios(gesLocal,null);
				            ven.Show();
    }

    protected virtual void OnTeclasFavoritosActionActivated (object sender, System.EventArgs e)
    {
         OnBtnDesconectarActivated(null,null);
         VenTecladosFav ven = new VenTecladosFav(gesLocal);
              ven.Show();
    }

    protected virtual void OnExportarActionActivated (object sender, System.EventArgs e)
    {
          VenExportImport ven = new VenExportImport(gesLocal,gesRemoto,ModVen.Exportar);
            ven.SalirImportar += new OnSalirImporExpor(this.VerBotonSinc);
			  ven.Show();
    }

    protected virtual void OnBtnSincronizarActivated (object sender, System.EventArgs e)
    {
              VenSincronizar ven = new VenSincronizar(gesLocal,gesRemoto);
                ven.SalirSincronizar += new OnSalirSincronizar(this.VerBotonSinc);
                  ven.Show();
    }
		
    protected virtual void OnReiniciarTpvActionActivated (object sender, System.EventArgs e)
    {
      try{
       new Valle.Distribuido.GesMensajes(this.gesServ.ServidorActivo["IP"].ToString(),
				                                  (int)this.gesServ.ServidorActivo["PortSockt"]).EnviarMensaje("Reiniciar");
       
       }catch (Exception ex){
         this.lblInformacion.Text = (ex.Message);
        }
       
    }
		
    protected virtual void OnComboServidoresChanged (object sender, System.EventArgs e)
    {
		   this.OnBtnDesconectarActivated(null,null);
        	
           if ((comboServidores.ActiveText == null)||
               (this.comboServidores.ActiveText.Equals("Ningun servidor activo"))){
				
				 this.gesServ.ActivarServidor(-1);
                 this.inicializarMenu();
				
			}else if ((gesServ.ServidorActivo!=null) &&
	       		(!this.gesServ.ServidorActivo["Nombre"].Equals(this.comboServidores.ActiveText))){
				
                     gesServ.ActivarServidor((int)gesServ.TbServidores.Rows[comboServidores.Active-1]["IDServidor"]);
                     this.gesLocal.CambiarServidor(gesServ.ServidorActivo["Nombre"].ToString());
                     this.inicializarMenu();
				
            }else if ((gesServ.ServidorActivo==null)&&
			          (!this.comboServidores.ActiveText.Equals("Ningun servidor activo"))){
				
				    gesServ.ActivarServidor((int)gesServ.TbServidores.Rows[comboServidores.Active-1]["IDServidor"]);
			        this.gesLocal.CambiarServidor(gesServ.ServidorActivo["Nombre"].ToString());
                     this.inicializarMenu();	
			}
			
    }
    
    private int aumentar = 0;
    protected virtual void OnPnePrincipalSizeAllocated (object o, Gtk.SizeAllocatedArgs args)
    {
        if(aumentar == 0){
          Gdk.Pixbuf imagenScalada = this.imgPortada.Pixbuf.ScaleSimple
                (this.imgPortada.Allocation.Width,this.imgPortada.Allocation.Height,Gdk.InterpType.Tiles);
			this.imgPortada.Pixbuf = imagenScalada; aumentar++;
	        }else{
	          aumentar = 0;
	        }
	        args.RetVal = false;
	}
  
    protected virtual void OnGestionComisionesActionActivated (object sender, System.EventArgs e)
    {
              OnBtnDesconectarActivated(null,null);
             	new VenInstComision(gesLocal);
    }

    protected virtual void OnKeyPressEvent (object o, Gtk.KeyPressEventArgs args)
    {
	   Gdk.ModifierType mod = Gdk.ModifierType.ControlMask | Gdk.ModifierType.Mod1Mask;
		if((args.Event.State == mod)&&(args.Event.Key == Gdk.Key.a)&&
			   (this.btnDesconectar.Sensitive)){
				 VenListados ven = new VenListados(gesRemoto);
                 ven.Show();
			}
    }

    protected virtual void OnGestionAlmacenActionActivated (object sender, System.EventArgs e)
    {
			  OnBtnDesconectarActivated(null,null);
             	new VenAlmacenes(gesLocal);
    }

  
}
}