using System;
using System.Diagnostics;


namespace restartTpv
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			System.Threading.Thread.Sleep(50);
			if(args.Length==2){
				  Process p = new Process();
					p.StartInfo.FileName = args[1] + "/ValleTpv.exe";
				   	int num = args.Length >0 ? Convert.ToInt32(args[0]) : 1;
					
				    while (Process.GetProcessesByName("mono").Length > num)
						                    System.Threading.Thread.Sleep(1000);
				
					p.Start();
			}		 
		}
	}
	
}

