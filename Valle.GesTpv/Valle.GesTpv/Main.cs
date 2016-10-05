// Main.cs created with MonoDevelop
// User: valle at 22:54 11/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using System.Threading;
using Valle.GtkUtilidades;


namespace Valle.GesTpv
{
	class MainClass
	{
	  
		public static void Main (string[] args)
		{
		    Application.Init ();
		    Splash spl = new Splash(null,Rutas.Principal+"/"+Rutas.IMG_APP+"/ImgSpl.png",false);
		    spl.Show();
		    new VenPrincipal(spl);
		    Application.Run();
		}
		
		
		
	}
}