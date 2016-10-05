/*
 * Creado por SharpDevelop.
 * Usuario: vallevm
 * Fecha: 30/03/2009
 * Hora: 1:50
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Text;

namespace Valle.Utilidades
{
	/// <summary>
	/// Description of Convertir.
	/// </summary>
	public class Convertir
	{
		public Convertir()
		{}
		
			
	       public static string BytesAString(Byte[] Cad, int ini, int fin){
			 return Encoding.UTF32.GetString(Cad,ini,fin);
			}

		  public static byte[] StringAbytes(string str){
             return Encoding.UTF32.GetBytes(str);
		  }
        
          public static byte[] StringHexABytes(string hex){
        	byte[] hexBit =  new byte[hex.ToCharArray().Length/2];
        	for(int i= 0 ;i<hexBit.Length;i++){
        		string byteStr = hex.Substring(i*2,2);
        		hexBit[i] = Convert.ToByte(byteStr,16);
        		 
        	}
        	return hexBit;
          }
        
          public static string DeBytesAStringHEX(Byte b)
          {
            return b.ToString("X2");
          }

          public static string DeBytesAStringHEX(Byte[] inst){
        	StringBuilder hexString = new StringBuilder(inst.Length);
			for (int i=0; i < inst.Length; i++)
			{
				hexString.Append(inst[i].ToString("X2"));
			}
			return hexString.ToString();
           }
 
		
	}
}
