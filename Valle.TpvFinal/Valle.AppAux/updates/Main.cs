using System;

namespace updates
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Gtk.Application.Init();
			   new Update();
			Gtk.Application.Run();
		}
	}
}
