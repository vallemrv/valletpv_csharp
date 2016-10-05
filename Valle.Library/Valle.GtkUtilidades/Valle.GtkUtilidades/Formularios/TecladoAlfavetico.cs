using System;
namespace Valle.GtkUtilidades
{
    public enum AccionTeclado { AC_cancelar, AC_aceptar, AC_Buscar };
    public enum TipoDeTeclado { Tipo_Alfabetico, Tipo_Buscador }
	
    public delegate void OnAccionesTeclado(AccionTeclado accion, String cadena);

    public partial class TecladoAlfavetico : FormularioBase
    {
        String[] Letras = new String[]{"p","o","i","u","y","t",".",",","r","m","n",
                                                  "b","v","c","x","z","-","ñ","e","l","k",
                                                   "j","h","g","f","d","s","a","w","q"};

        String[] CharSpecial = new String[]{"\\","@","%","(",")","+","-","*","¡","!",
                                                  "&amp;","$",";",",",".",":","/","¿","?","=",
                                                   "","1/2","1/4","1.5","&gt;","",""};
        String[] tecladoActivo;
        TipoDeTeclado miTipo;
        AccionTeclado accion;


        public AccionTeclado Accion
        {
            get { return accion; }
        }



        public String Cadena
        {
            get { return this.txtCadena.Texto; }
            set { txtCadena.Texto = value; }
        }

        public event OnAccionesTeclado EjAccion = null;

        public TecladoAlfavetico(TipoDeTeclado tipo)
        {
            this.Init();
			txtCadena.TextoCambiado+= txtCadena_TextChanged;
			this.LblTituloBase = this.lbltitulo;
			this.tecladoActivo = Letras;
            inicializarTeclado();
            miTipo = tipo;
			txtCadena.ColorDeFono = System.Drawing.Color.White;
			txtCadena.Font = new System.Drawing.Font(txtCadena.Font.FontFamily,25f);
			if (tipo == TipoDeTeclado.Tipo_Buscador)
            {
                this.btnMayuFija.Sensitive = false;
                this.btnEcho.Sensitive = false;
                this.btnMayUnpos.Sensitive = false;
				this.SetSizeRequest(750,450);
				this.CambiarTamaño();
            }
        }
		
		private void CambiarTamaño()
        {
			Gtk.Widget[] cs = this.pneTeclado.Children;
			int pos = 0;
			for (int i= 0;i< cs.Length && pos < tecladoActivo.Length;i++)
            {
				if(cs[i].Name.Contains("btnA")){
					
					 Gtk.Button b = (Gtk.Button)cs[i];
		               b.SetSizeRequest(60,60);
                }
            }
			this.btnEspaciador.HeightRequest = 60;
			this.btnEcho.HeightRequest = 60;
			
			
         }


        private void inicializarTeclado()
        {
			Gtk.Widget[] cs = this.pneTeclado.Children;
			int pos = 0;
            for (int i= 0;i< cs.Length && pos < tecladoActivo.Length;i++)
            {
				if(cs[i].Name.Contains("btnA")){
					
					 Gtk.Button b = (Gtk.Button)cs[i];
						  string strTecla = ((btnMayuFija.Active || btnMayUnpos.Active) && !btnEspecial.Active) ?
	                        tecladoActivo[pos].ToUpper() :  tecladoActivo[pos];
							
					(b.Child as Gtk.Label).LabelProp= "<span size='xx-large'> "+ strTecla+"</span>";
					(b.Child as Gtk.Label).UseMarkup = true;pos++;//porque no todos los botones son teclas validas
                }
            }
         }

        private void Tecla_clik(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
		    Gtk.Label tecla = (Gtk.Label)((Gtk.Button)sender).Child;
            this.txtCadena.Texto += tecla.Text.Trim();
            if (btnMayUnpos.Active)
            {
                btnMayUnpos.Active = false;
                this.inicializarTeclado();
            }
        }



        private void btnMayuFija_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
             this.inicializarTeclado();
        }

        private void btnMayUnpos_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
			 this.inicializarTeclado();
        }

        private void btnEspaciador_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            this.txtCadena.Texto += " ";

        }



        private void btnAceptar_Click(object sender, EventArgs e)
        {
            accion = AccionTeclado.AC_aceptar;
            if (EjAccion != null) { EjAccion(accion, Cadena); }
            this.CerrarFormulario();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            accion = AccionTeclado.AC_cancelar;
            if (EjAccion != null) { EjAccion(accion, null); }
            this.CerrarFormulario();
        }

        

        private void btnBackSp_Click_1(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            if (Cadena.Length > 0)
            {
                Cadena = Cadena.Remove(Cadena.Length - 1);
            }
        }

        private void btnSpecial_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            if (!btnEspecial.Active)
            {
                this.tecladoActivo = CharSpecial;
                this.inicializarTeclado();
            }
            else
            {
                this.tecladoActivo = Letras;
                this.inicializarTeclado();
            }
        }

        private void txtCadena_TextChanged(object sender, EventArgs e)
        {
            if((miTipo == TipoDeTeclado.Tipo_Buscador)&&
                 (EjAccion!=null))
            {
                accion = AccionTeclado.AC_Buscar;
                EjAccion(accion, Cadena);
            }
        }
		
		

       
		protected override void btnSalir_Click(object sender, EventArgs e)
        {
            accion = AccionTeclado.AC_cancelar;
            if (EjAccion != null) { EjAccion(accion, null); }
            this.CerrarFormulario();
        }
    }
}

