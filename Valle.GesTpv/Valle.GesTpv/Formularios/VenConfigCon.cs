// VenConfiguracion.cs created with MonoDevelop
// User: valle at 10:34 12/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Xml;


namespace Valle.GesTpv
{
	public enum AccionesGesServ  {Salir, Borrar};
	public delegate void OnEjecutarAccionVenConf(AccionesGesServ accion, object obj);
	
	
	public partial class VenConfigCon : Gtk.Window
	{
		private GesServidores gesServ;
		private event OnEjecutarAccionVenConf EjAccion;
		
		public VenConfigCon(GesServidores ges, OnEjecutarAccionVenConf ejAcciones) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			gesServ = ges;
			EjAccion += new OnEjecutarAccionVenConf(ejAcciones);
			cargarListaServ();
		}
		
		private void cargarListaServ(){
		    ((Gtk.ListStore)cmbHistorial.Model).Clear();
			cmbHistorial.AppendText("Nuevo");
            foreach(DataRow dr in gesServ.TbServidores.Rows){
				cmbHistorial.AppendText(dr["Nombre"].ToString());
			}
		    cmbHistorial.Active = gesServ.TbServidores.Rows.Count >0 ? 
		        gesServ.TbServidores.Rows.IndexOf(gesServ.ServidorActivo)+1:0;
		}
		
		private bool datosValidos(){
			 return (this.txtNomServ.Text.Length>0)&& (this.cmbProtocolo.ActiveText.Length>0);
			
		}

		private void cargarReg(DataRow dr){
    		dr["Nombre"]= this.txtNomServ.Text;
	                  dr["Ip"]= String.Concat(this.txtIp1.Text,".",this.txtIp2.Text,"."
			                                         ,this.txtIp3.Text,".",this.txtIp4.Text);
	                  dr["Puerto"] = Int32.Parse(this.txtPuertoUno.Text);
	                  dr["Protocolo"] = this.cmbProtocolo.ActiveText;
			          dr["PortSockt"] = this.txtPuertoDos.Text;
	            }
		
		
		private void cargarDatos(DataRow dr){
			
			this.txtNomServ.Text=dr["Nombre"].ToString();
			String[] ipsPart =  dr["Ip"].ToString().Split('.');
	        txtIp1.Text= ipsPart[0];
			txtIp2.Text = ipsPart[1];
			this.txtIp3.Text = ipsPart[2];
			this.txtIp4.Text = ipsPart[3];
            this.txtPuertoUno.Text= dr["Puerto"].ToString();
			this.txtPuertoDos.Text = dr["PortSockt"].ToString();
			this.cmbProtocolo.Active = numItem(dr["Protocolo"].ToString()); 
			this.lblInformacion.Text = "Servidor: "+ dr["Nombre"].ToString()+". puede variar algun atributo o aceptar para activarlo";
		}
		
		private void BorrarDatos(){
			this.txtNomServ.Text="";
			txtIp1.Value = 0;
			txtIp2.Value = 0;
			this.txtIp3.Value = 0;
			this.txtIp4.Value = 0;
            this.txtPuertoUno.Value = 1050;
			this.txtPuertoDos.Value = 1050;
			this.cmbProtocolo.Active = 0; 
	     	this.lblInformacion.Text = "Servidor nuevo rellene todos los campos y pulse aceptar";
		}
		
		private int numItem(String nom){
           int num=0;
			switch(nom){
		
			case "http": num = 1;break;
			case "tcp": num = 0; break;
						
			}
			return num;
		}
		
		
				protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
				{
			           DataRow dr;
			          if(datosValidos()){
                 	         DataRow[] drs =  gesServ.TbServidores.Select("Nombre = '"+this.txtNomServ.Text+"'");
				               if(drs.Length>0){
					              dr = drs[0];
					               cargarReg(dr);
					            }else{
				                  DataSet ds = new DataSet(this.txtNomServ.Text);
				                  ds.WriteXml(this.gesServ.FICH_TB_SERVIDORES,XmlWriteMode.WriteSchema);
					              dr = gesServ.RegistroNuevo();
				                  cargarReg(dr);
				                  dr["ExportarComun"] = true;
					              gesServ.AñadirRegistro(dr);
				                } 
				            
				            gesServ.ActivarServidor((int)dr[GesServidores.IDSERVIDOR]);
				            EjAccion(AccionesGesServ.Salir, null);this.Destroy();
			            }else{
				          this.lblInformacion.Text = "Todos los campos son necesarios";
			           	}
				}

				

				protected virtual void OnCmbHistorialChanged (object sender, System.EventArgs e)
				{
				     if(this.cmbHistorial.ActiveText!=null){
			          DataRow[] drs =  gesServ.TbServidores.Select("Nombre = '"+this.cmbHistorial.ActiveText+"'");
				               if(drs.Length>0){
					             DataRow dr = drs[0];
					              cargarDatos(dr);
				                }else{
				                  BorrarDatos();
		                      	}
		                      	}
				}

				protected virtual void OnBtnCacelarClicked (object sender, System.EventArgs e)
				{
			                this.Destroy();
				}

				protected virtual void OnBtnBorrarClicked (object sender, System.EventArgs e)
				{
				       if(this.cmbHistorial.ActiveText!=null){
			             DataRow[] drs =  gesServ.TbServidores.Select("Nombre = '"+this.cmbHistorial.ActiveText+"'");
				               if(drs.Length>0){
					              this.EjAccion(AccionesGesServ.Borrar, drs[0]);
					              BorrarDatos();
					              this.cargarListaServ();
		                      	}
		                      	}
				}

			
				

				
	}
}
