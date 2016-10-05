using System;
namespace Valle.GtkUtilidades
{
	public enum FormatosNumericos { F_entero, F_Decimal, F_IP };
    public enum AccionesNumerico { AC_Anular, AC_Aceptar, AC_BtnPulsado };
    public delegate void OnSalirNumerico(AccionesNumerico accion, String numero);

	public partial class TecladoNumerico : FormularioBase
	{
		public TecladoNumerico (FormatosNumericos formatoTeclado, String titulo) 
		{
			this.init ();
			LblTituloBase = lblTitulo;
			Titulo = titulo;
			this.lblDisplay.Font = new  System.Drawing.Font(this.lblDisplay.Font.FontFamily,20f);
			this.lblDisplay.ColorDeFono = System.Drawing.Color.White;
            salirNumerico = null;
            formato = formatoTeclado;
            if (formato== FormatosNumericos.F_entero)
                         this.btnComa.Sensitive = false;
           
		}
		
		public String Numero
        {
            set { this.lblDisplay.Texto = value; }
            get { return lblDisplay.Texto; }
        }

        
        private AccionesNumerico accion;

        public AccionesNumerico Accion
        {
            get { return accion; }
        }
        
		private FormatosNumericos formato;
        public FormatosNumericos Formato
        {
            set { formato = value;
            this.btnComa.Sensitive = false;
            }
        }
        
        public event OnSalirNumerico salirNumerico;
        
        private void BtnNumerico_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            
            string tecla = (((Gtk.Button)sender).Child as Gtk.Label).Text;
             switch(tecla){
                case ",":
                  if(formato!= FormatosNumericos.F_IP){
                      if(!this.Numero.Contains(tecla)){
                         Numero += tecla;
                      }
                    }else{
                       Numero+="."; 
                    }
                 break;
                default:
                    Numero += tecla;
                break;
             }
           
        }

       

        private void btnAceptar_Click_1(object sender, EventArgs e)
        {
            accion = AccionesNumerico.AC_Aceptar;
            if (salirNumerico != null) { salirNumerico(accion, this.lblDisplay.Texto); }
            base.btnSalir_Click(sender,e);
        }

        private void OnBtnSalir(object sender, EventArgs e)
        {
            accion = AccionesNumerico.AC_Anular;
            if (salirNumerico != null) { salirNumerico(accion, null); }
            base.btnSalir_Click(sender, e);
        }

        private void btnBackSp_Click_1(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            if (this.Numero.Length > 1)
               Numero = Numero.Remove(Numero.Length - 1);
                else
                  Numero = "";
            
        }

        protected virtual void OnBtnBorrar (object sender, System.EventArgs e)
		{
			Numero = "";
		}
		
		
    }
}

