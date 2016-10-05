using System;
using System.Collections.Generic;
using Valle.GtkUtilidades;


namespace Valle.TpvFinal
{
	public enum AccionesDividitTicket { Caja, salir }
    public delegate void OnAccionDividirTicket(AccionesDividitTicket accion, List<string> informe);
   
	public partial class DividirTiket : FormularioBase
	{
		public DividirTiket () 
		{
		    this.Init ();
			this.WindowPosition = Gtk.WindowPosition.CenterAlways;
			this.KeepAbove = true; this.Modal = true;
			this.LblTituloBase = this.lblTitu;
			this.Titulo = "Dividir Ticket";
			
			this.lblInfTicket.Font = new System.Drawing.Font(lblInfTicket.Font.FontFamily,20f);
		    this.lblInfTicket.ColorDeFono = System.Drawing.SystemColors.Control;
			this.lblInfTicket.AlienamientoH = System.Drawing.StringAlignment.Center;
		    
			this.txtNumPersonas.Font = new System.Drawing.Font(lblInfTicket.Font.FontFamily,30f);
		    this.txtNumPersonas.ColorDeFono = System.Drawing.Color.White;
			this.txtNumPersonas.AlienamientoH = System.Drawing.StringAlignment.Far;
		    
			
		}
	
        public event OnAccionDividirTicket EjAccion;
        public AccionesDividitTicket accion;
        decimal importeADividir;

        public decimal ImporteADividir
        {
            get { return importeADividir; }
            set { importeADividir = value;
                 numPersonas = 1; primera = true;
            }
        }
        decimal division;
        decimal resto;
        int numPersonas;
        List<string> informe = new List<string>();

      

        
        private void btnDividir_Click(object sender, EventArgs e)
        {
            PulsadoRecientemente = true;
            string numDecimal = String.Format("{0:####0.00}",importeADividir/numPersonas);
            int numUltimo = Int32.Parse(numDecimal.Substring(numDecimal.Length-1));
            numDecimal = numDecimal.Remove(numDecimal.Length-1);
            if (numUltimo > 5) { numDecimal = String.Concat(numDecimal + "5"); } else { numDecimal = String.Concat(numDecimal + "0"); }
            division = Decimal.Parse(numDecimal);
            decimal comparacion = division * numPersonas;
            resto = importeADividir - comparacion;
            informe.Clear();
            
            lblInfTicket.Texto = String.Format("Importe {0:c}", importeADividir);
            informe.Add(String.Format("Importe {0:c}", importeADividir));
            
            if (resto > 0)
            {
                lblInfTicket.Texto += "\n" + String.Format("{0} Ticket a {1:c}", numPersonas - 1, division);
                informe.Add(String.Format("{0} Ticket a {1:c}", numPersonas - 1, division));
                lblInfTicket.Texto += "\n" + String.Format("1 Ticket a {0:c}",  division+resto);
                informe.Add(String.Format("1 Ticket a {0:c}",  division+resto));
                lblInfTicket.Texto += "\n" + String.Format("Total  {0:c}", (division*numPersonas) + resto);
                informe.Add(String.Format("Total  {0:c}", (division*numPersonas) + resto));
          
            }
            else
            {
                lblInfTicket.Texto += "\n" + String.Format("{0} Ticket a {1:c}", numPersonas, division);
                informe.Add(String.Format("{0} Ticket a {1:c}", numPersonas, division));
                lblInfTicket.Texto += "\n" + String.Format("Total  {0:c}", (division * numPersonas));
                informe.Add(String.Format("Total  {0:c}", (division * numPersonas)));
            }
           
        }

        private void btnCaja_Click(object sender, EventArgs e)
        {
           
            accion = AccionesDividitTicket.Caja;
            if (EjAccion != null) { EjAccion(AccionesDividitTicket.Caja, informe); }
            base.btnSalir_Click(sender, e);
         
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            numPersonas++;
            txtNumPersonas.Texto = numPersonas.ToString();
            PulsadoRecientemente = true;
        }

        private void btnBaj_Click(object sender, EventArgs e)
        {
            if (numPersonas > 1) { numPersonas--; }
            txtNumPersonas.Texto = numPersonas.ToString();
            PulsadoRecientemente = true;
        }

       
        
		protected override void btnSalir_Click(object sender, EventArgs e)
        {
            accion = AccionesDividitTicket.salir;
            if (EjAccion != null) { EjAccion(AccionesDividitTicket.salir, null); }
            base.btnSalir_Click(sender, e);
        }
		
		bool primera = true;
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			if(primera){
			  lblInfTicket.Texto = String.Format("Importe {0:c}", importeADividir);
			  txtNumPersonas.Texto = numPersonas.ToString();
			  primera  = false;
			}	
			return base.OnExposeEvent (evnt);
		}
	}
}

