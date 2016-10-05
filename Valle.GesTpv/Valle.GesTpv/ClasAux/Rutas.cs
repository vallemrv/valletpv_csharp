// Rutas.cs created with MonoDevelop
// User: valle at 16:50 07/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Valle.GesTpv
{
	
	
	public class Rutas
	{
	       public const string IMG_APP = "Iconos";
		   public const string PLANING = "Planing";
		   public const string DATOS = "Datos";
		 
		  public static string Principal{
		     get{
	    	    return  Utilidades.RutasArchivos.Ruta_Principal;
		      }
		    }
		    
		 public static string Ruta_Directa(string rutaRelativa){
		     return Utilidades.RutasArchivos.Ruta_Completa(rutaRelativa);
		 }
		  
		  public Rutas(){
		          System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Principal +System.IO.Path.DirectorySeparatorChar+IMG_APP);
		            if(!dir.Exists){dir.Create();}
		              dir = new System.IO.DirectoryInfo(Principal +System.IO.Path.DirectorySeparatorChar+PLANING);
		            if(!dir.Exists){dir.Create();}
		                dir = new System.IO.DirectoryInfo(Principal +System.IO.Path.DirectorySeparatorChar+DATOS);
		            if(!dir.Exists){dir.Create();}
		   }
	    
		
	}
}
