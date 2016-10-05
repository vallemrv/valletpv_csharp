using System;
namespace Valle.TpvFinal
{
	public class VenSecciones : GtkUtilidades.VenBotones
	{
		public VenSecciones ()
		{
		}
		public VenSecciones( int numBonotnes): base(numBonotnes){
			
		}
		
		protected override void btnSalir_Click (object sender, EventArgs e)
		{
		    this.HandleBotonera4handleclickBoton(null,null);
			base.btnSalir_Click (sender, e);
		}
	}
}

