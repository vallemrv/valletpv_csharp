using System;
namespace Valle.GtkUtilidades
{
	public class BarraProgreso : Valle.Utilidades.IBarrProgres
	{
		Gtk.ProgressBar mibarra;
		int maxProgreso = 100;
		int progreso = 0;
		public BarraProgreso(Gtk.ProgressBar bar){
			mibarra = bar;
		}
		
		#region IBarrProgres implementation
		public int Progreso {
			get {
				return progreso;
			}
			set {
				progreso = value;
				mibarra.Fraction = progreso/maxProgreso;
			}
		}

		public int MaxProgreso {
			get {
				return maxProgreso;
			}
			set {
				maxProgreso = value;
			}
		}
		#endregion
	}
}

