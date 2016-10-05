// RutasArchivos.cs created with MonoDevelop
// User: valle at 22:51 07/05/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Text;
using System.Threading;

namespace Valle.Utilidades
{
    
    
    public class RutasArchivos
    {
    	 public static string Ruta_Principal{
		     get{
		        string ruta =  System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Remove(0,5);
		        if(ruta[2].Equals(':'))
		            ruta = ruta.Remove(0,1);
	    	    return  ruta;
		      }
		    }
         public static string Ruta_Completa(string rutaRelativa){
             return (Ruta_Principal+System.IO.Path.DirectorySeparatorChar+rutaRelativa); 
         }
         
         public static void EscribirEnFicheroErr(string nomFichero, string err, string fecha, string funProduceErr){
                System.IO.FileStream s = new System.IO.FileStream(nomFichero, System.IO.FileMode.Append);
        		System.IO.StreamWriter sw = new System.IO.StreamWriter(s);
        		sw.WriteLine(fecha+": Error>> "+err+": Funcion de la excepcion>> "+funProduceErr);
        		sw.Close();s.Close(); 
         }
       
    }
}
