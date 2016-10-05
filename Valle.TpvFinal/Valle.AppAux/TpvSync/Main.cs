using System;

namespace TpvSync
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Gtk.Application.Init();
			    new Sync();
			Gtk.Application.Run();
		}
	}
}
