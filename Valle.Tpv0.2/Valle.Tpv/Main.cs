using System;
using Gtk;

namespace Valle.TpvFinal
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			 new ModeloTpv();  
			Application.Run ();
		}
	}
}

