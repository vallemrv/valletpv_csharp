
using System;

namespace Valle.TpvFinal
{
	public class DatosConfIni{
	   public string PathDatos ="";
	   public string PathFotos ="";
	   public string PathMesas ="";
	   public string PathPlaning ="";
	   public string ipServidor ="";
	   public bool esAuxiliar =  false;
	   public string horaActualizacion= "";	
	   public string puertoDatos ="8000";
	   public string puertoComunicacion ="8001";
	   public string sqlPuerto ="3306";
	   public string sqlUser ="";
	   public string sqlPass ="";
	   public string protocolo ="";
	   public string nom_imp = "Caja2";
	   public bool actualizar = false;
	   public bool sync_nube = false;
	   public string dir_servidor_sync = "";
	   public string dir_servidor_act = "";
		
	}
	
	public class EventArgsConfiguracion : EventArgs{
	      public bool estaCancelado = true;
		  public DatosConfIni datosConfiguracion ;
		  
		   public EventArgsConfiguracion(DatosConfIni datos){
			  estaCancelado = false;
			  this.datosConfiguracion = datos;
		     }
		
		   public EventArgsConfiguracion(){}
	}

	public partial class VenConfiguracion : GtkUtilidades.FormularioBase
	{
		
		DatosConfIni datosConfIni ;
		public event EventHandler<EventArgsConfiguracion> salirConfiguracion;
		
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
			datosConfIni.esAuxiliar = this.chkCliente.Active;
			datosConfIni.ipServidor = this.txtIp1.Text+'.'+this.txtIp2.Text+'.'+this.txtIp3.Text+'.'+this.txtIp4.Text;
			datosConfIni.PathDatos = fchDatos.CurrentFolder;
			datosConfIni.PathFotos = fchFotos.CurrentFolder;
			datosConfIni.PathMesas = fchMesas.CurrentFolder;
			datosConfIni.PathPlaning = fchPlanos.CurrentFolder;
			datosConfIni.protocolo = "Tcp";
			datosConfIni.puertoComunicacion = this.txtPortCom.Value.ToString();
			datosConfIni.puertoDatos = this.txtPortDatos.Value.ToString();
			datosConfIni.sqlPass = this.txtPass.Text;
			datosConfIni.sqlPuerto = this.txtPort.Value.ToString();
			datosConfIni.sqlUser = this.txtUsr.Text;
			datosConfIni.nom_imp = this.txtNomImp.Text;
			datosConfIni.sync_nube = this.chkSincronizar.Active;
			datosConfIni.actualizar = this.chkActualizar.Active;
			datosConfIni.dir_servidor_sync = this.txtDirSinc.Text;
			datosConfIni.dir_servidor_act = this.txtDirAtc.Text;
			
			if(salirConfiguracion!= null) this.salirConfiguracion(this,new EventArgsConfiguracion(datosConfIni));
			this.CerrarFormulario();
		}
		
		

		public VenConfiguracion (DatosConfIni datos) 
		{
			this.Build ();
			this.OcultarSolo = false;
			this.datosConfIni = datos;
			this.LblTituloBase = this.lblTittulo;
			this.Titulo = "Cofiguracion para el tpv";
			this.txtPass.Text = datosConfIni.sqlPass;
			string[] ip = datosConfIni.ipServidor.Split('.');
			this.txtIp1.Value = int.Parse(ip[0]);
			this.txtIp2.Value = int.Parse(ip[1]);
			this.txtIp3.Value = int.Parse(ip[2]);
			this.txtIp4.Value = int.Parse(ip[3]);
			this.txtUsr.Text = datosConfIni.sqlUser;
			this.fchDatos.SetCurrentFolder(datosConfIni.PathDatos);
			this.fchFotos.SetCurrentFolder(datosConfIni.PathFotos);
			this.fchMesas.SetCurrentFolder(datosConfIni.PathMesas);
		    this.fchPlanos.SetCurrentFolder(datosConfIni.PathPlaning);
			this.txtPortCom.Value = int.Parse(datosConfIni.puertoComunicacion);
			this.txtPortDatos.Value = int.Parse(datosConfIni.puertoDatos);
			this.txtPort.Value = int.Parse(datosConfIni.sqlPuerto);
			this.chkCliente.Active = 
				     this.pneIpsServidor.Sensitive = datosConfIni.esAuxiliar; 
			this.chkActualizar.Active =this.pneActualizar.Sensitive =
				datosConfIni.actualizar;
				
			this.chkSincronizar.Active = 
				     this.pneSincronizar.Sensitive = datosConfIni.sync_nube; 
			this.txtNomImp.Text = datosConfIni.nom_imp;
			this.txtDirSinc.Text = datosConfIni.dir_servidor_sync;
			this.txtDirAtc.Text = datosConfIni.dir_servidor_act;
			
		}
		
		protected virtual void OnBtnCancelarClicked (object sender, System.EventArgs e)
		{
			if(salirConfiguracion!= null) this.salirConfiguracion(this,new EventArgsConfiguracion());
			this.CerrarFormulario();
		}
		
		
		
		
		protected virtual void OnChkClienteClicked (object sender, System.EventArgs e)
		{
			 pneIpsServidor.Sensitive = chkCliente.Active;
		}
		
		protected virtual void OnChkSincronizarClicked (object sender, System.EventArgs e)
		{
			pneSincronizar.Sensitive = chkSincronizar.Active;
		}
		
		protected virtual void OnChkActualizarClicked (object sender, System.EventArgs e)
		{
			pneActualizar.Sensitive = chkActualizar.Active;
		}
		
		
		
		
		
		
		
	}
}
