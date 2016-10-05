using System;
using Gtk;
using Valle.TpvFinal.Tools;

namespace Valle.TpvFinal
{
	
	
	public delegate void OnSalirConfIniTpv();
	
	public partial class ConfIniTpv : Gtk.Window
	{
		public event OnSalirConfIniTpv salirConfIniTpv;
		public DatosConfIni datosConfIni;
		
		
		
		
		public ConfIniTpv (DatosConfIni datosConfIni) : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.datosConfIni = datosConfIni;
			this.txtContraseña.Text = datosConfIni.sqlPass;
			string[] ip = datosConfIni.ipServidor.Split('.');
			this.txtIp1.Text = ip[0];
			this.txtIp2.Text = ip[1];
			this.txtIp4.Text = ip[2];
			this.txtIp5.Text = ip[3];
			this.txtNomUsuario.Text = datosConfIni.sqlUser;
			this.fchDatos.SetCurrentFolder(datosConfIni.PathDatos);
			this.fchFotos.SetCurrentFolder(datosConfIni.PathFotos);
			this.fchMesas.SetCurrentFolder(datosConfIni.PathMesas);
		    this.fchPlanos.SetCurrentFolder(datosConfIni.PathPlaning);
			this.txtPuertoComunicacion.Text = datosConfIni.puertoComunicacion;
			this.txtPuertoDatos.Text = datosConfIni.puertoDatos;
			this.txtPuetoSql.Text = datosConfIni.sqlPuerto;
			this.chkServidorAuxiliar.Active = 
				     this.pneIpServidor.Sensitive = datosConfIni.esAuxiliar; 
		}
		
		
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
			datosConfIni.esAuxiliar = this.chkServidorAuxiliar.Active;
			datosConfIni.ipServidor = this.txtIp1.Text+'.'+this.txtIp2.Text+'.'+this.txtIp4.Text+'.'+this.txtIp5.Text;
			datosConfIni.PathDatos = fchDatos.CurrentFolder;
			datosConfIni.PathFotos = fchFotos.CurrentFolder;
			datosConfIni.PathMesas = fchMesas.CurrentFolder;
			datosConfIni.PathPlaning = fchPlanos.CurrentFolder;
			datosConfIni.protocolo = "Tcp";
			datosConfIni.puertoComunicacion = this.txtPuertoComunicacion.Text;
			datosConfIni.puertoDatos = this.txtPuertoDatos.Text;
			datosConfIni.sqlPass = this.txtContraseña.Text;
			datosConfIni.sqlPuerto = this.txtPuetoSql.Text;
			datosConfIni.sqlUser = this.txtNomUsuario.Text;
			if(salirConfIniTpv!=null) salirConfIniTpv();
			this.Destroy();
		}
		
		protected virtual void OnChkServidorAuxiliarClicked (object sender, System.EventArgs e)
		{
			          this.pneIpServidor.Sensitive = this.chkServidorAuxiliar.Active;
		}
		
		
		
		
		
		
	}
}

