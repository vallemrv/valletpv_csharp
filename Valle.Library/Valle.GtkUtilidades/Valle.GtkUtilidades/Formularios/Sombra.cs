
using System;

namespace Valle.GtkUtilidades
{


	public class Sombra : Gtk.Window
	{

		public Sombra () : base(Gtk.WindowType.Toplevel)
		{
			this.AppPaintable = true;
			this.Colormap = this.Screen.RgbaColormap;
			
				this.WindowPosition = Gtk.WindowPosition.CenterAlways;
			    this.SkipTaskbarHint = true;
				this.AcceptFocus = false;
				this.Visible=false;
				this.Decorated = false;
			
				this.ExposeEvent+= delegate {
					Cairo.Context c = Gdk.CairoHelper.Create(this.GdkWindow);
					  
					c.SetSourceRGBA(0.0,0.0,0.0,0.65);
                    c.Operator = Cairo.Operator.Source;
                    c.Paint();
				
				     ((IDisposable)c).Dispose();
				};
		}
		
		
	}
}
