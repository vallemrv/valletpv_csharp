using System;

namespace Valle.Utilidades
{
	public class UtilidadErrores
	{
		public static void EscribirEnFicheroErr(string nomFichero, string err, string fecha, string funProduceErr){
                System.IO.FileStream s = new System.IO.FileStream(nomFichero, System.IO.FileMode.Append);
        		System.IO.StreamWriter sw = new System.IO.StreamWriter(s);
        		sw.WriteLine(fecha+": Error>> "+err+": Funcion de la excepcion>> "+funProduceErr);
        		sw.Close();s.Close(); 
         }
		
		
	}
}

