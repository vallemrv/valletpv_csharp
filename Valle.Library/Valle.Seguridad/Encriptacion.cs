using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Valle.Seguridad
{
    public class Encriptar
    {
    
    static UnicodeEncoding uniEncoding = new UnicodeEncoding();
    static Byte[] key = new byte[]{0xED,0xAC,0x81,0x10,0x12,0xAF,0xE8,0x8C,0x8E,0xE3,0xAF,0x94,0x0A,0xA5,0x0A,
                                       0xED,0x44,0xAA,0xED,0x67,0x66,0x78,0xCC,0x82};
    static Byte[] IV = new byte[]{0xED,0xAC,0x81,0xED,0xA1,0xAF,0xE8,0x8C,0x8E,0xE3,0xAF,0x94,0x0A,0xA5,0x0A};

    public static String EncriptarCadena(String Dato)
    {
		 //guardamos el dato en la memoria temporal
             MemoryStream ms = new MemoryStream();

             //creamos un descriptador asociandolo a la memoria tmp		
         CryptoStream cStream = new CryptoStream
					(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key,IV), CryptoStreamMode.Write);


        		//lo asocio a un escritor de secuecias encadenadas
            StreamWriter sWriter = new StreamWriter(cStream);
			 

             //escribimos el dato encriptandolo
            sWriter.WriteLine(Dato);
			sWriter.Close();
            cStream.Close();
            
             
		     byte[] datoEnByte =  ms.ToArray();
			 string val = uniEncoding.GetString(datoEnByte);
		
			  ms.Close();
			  return val;        
			
    }

        public static string DescriptarCadena(string dato)
    {

            //guardamos el dato en la memoria temporal
            MemoryStream ms = new MemoryStream(uniEncoding.GetBytes(dato));

            //creamos un descriptador asociandolo a la memoria tmp		
            CryptoStream cStream = new CryptoStream(ms,
                new TripleDESCryptoServiceProvider().CreateDecryptor(key, IV),
                CryptoStreamMode.Read);

            //lo asocio a un lector de secuecias encadenadas
            StreamReader sReader = new StreamReader(cStream);


            string val = sReader.ReadLine();

            //cerramos todos los flujos de datos

            sReader.Close();
            cStream.Close();
            ms.Close();

            // Return the string. 
            return val;
        }
    }
        
    
    
}
