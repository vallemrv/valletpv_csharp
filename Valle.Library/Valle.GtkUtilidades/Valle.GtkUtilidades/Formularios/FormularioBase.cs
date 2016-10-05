using System;
namespace Valle.GtkUtilidades
{
	public partial class FormularioBase : Gtk.Window
	{
		public FormularioBase () : base(Gtk.WindowType.Toplevel)
		{
			this.Contruir();
			TemporizadorDeCierre = new System.Timers.Timer();
			TemporizadorDeCierre.Elapsed+= this.TemporizadorDeCierre_Tick;
			Title = "ValleTPV El TPV de ValleSoft";
		}
		
		
		protected System.Timers.Timer TemporizadorDeCierre;
        
		public bool esTemporizado;
	    public bool SalirAlPulsar = true;
        public bool PulsadoRecientemente = false;
        public bool OcultarSolo = true;
	    bool timeOut = false;

		public bool TimeOut {
			get {
				return this.timeOut;
			}
		}		
		
        String titulo = "";
        public String Titulo
        {
            get { return titulo; }
            set { 
				titulo = value;
                if(this.lblTituloBase != null) this.lblTituloBase.Texto = titulo;
		   }
        }
        
		Valle.GtkUtilidades.MiLabel lblTituloBase;
		public Valle.GtkUtilidades.MiLabel LblTituloBase{
		     set{ 
				lblTituloBase = value;	
				lblTituloBase.Texto = titulo;
				lblTituloBase.AlienamientoH = System.Drawing.StringAlignment.Center;
			    lblTituloBase.AlienamientoV = System.Drawing.StringAlignment.Center;
			    lblTituloBase.ColorDeFono = System.Drawing.SystemColors.ActiveBorder;
				lblTituloBase.Font = new System.Drawing.Font(lblTituloBase.Font.FontFamily,11,System.Drawing.FontStyle.Bold);
			}
		}
		
		protected virtual void btnSalir_Click(object sender, EventArgs e){
		    CerrarFormulario();	
		}
		
        public   void  CerrarFormulario()
        {
		      this.TemporizadorDeCierre.Stop();
               if (OcultarSolo) { this.Hide(); } else { this.Destroy(); }
		}
        
        private void TemporizadorDeCierre_Tick(object sender, EventArgs e)
        {
               if (!PulsadoRecientemente)
                {
				    timeOut = true;
                    TemporizadorDeCierre.Stop();
                    Gtk.Application.Invoke(delegate{this.btnSalir_Click(sender,e);});
                }
                else
                {
				    timeOut = false;
                    PulsadoRecientemente = false;
                }
           
        }
        
	    public void EstablecerTemporizador(bool temporizado, int intervalo)
        {
            this.esTemporizado = temporizado; 
            if (temporizado)
            {
                TemporizadorDeCierre.Interval = intervalo;
                TemporizadorDeCierre.Start();
				timeOut = false;
            }
            else
            {
                TemporizadorDeCierre.Stop();
				timeOut = true;
            }
        }
		 
	   bool unavez = true;
     
	    protected virtual void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			if(unavez){
				    this.lblTituloBase.Texto = titulo;
			        args.RetVal = false;
				   unavez = false;
			}
	}
		
		
	}
}

