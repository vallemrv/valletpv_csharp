using System;

namespace Valle.Utilidades
{
	public class WebConfig
	{
		
			public static string GetValueSetings(string key){
		      return System.Web.Configuration.WebConfigurationManager.AppSettings.Get(key);
     	   }
		
	}
}

