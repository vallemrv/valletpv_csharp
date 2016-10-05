// DibujoFlotante.cs created with MonoDevelop
// User: valle at 16:23 26/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Valle.GtkUtilidades
{
	
	[System.ComponentModel.Category("Valle.GtkUtilidades")]
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ContenedorFlot : Gtk.Window
	{
	   public Gtk.Widget control;
	   
		public ContenedorFlot(Gtk.Widget control):
						base(Gtk.WindowType.Toplevel)
        {
           this.TypeHint = Gdk.WindowTypeHint.Dock;
		   this.control = control;
		   this.Add(control);
		    
		}
		
		

				
	}
}
