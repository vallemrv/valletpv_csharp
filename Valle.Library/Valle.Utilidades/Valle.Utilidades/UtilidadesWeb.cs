using System;
using System.Web;

namespace Valle.Utilidades
{
	public class UtilidadesWeb
	{
			public static string GetValueSetings(string key){
		      return System.Web.Configuration.WebConfigurationManager.AppSettings.Get(key);
     	   }
		
	}
}

