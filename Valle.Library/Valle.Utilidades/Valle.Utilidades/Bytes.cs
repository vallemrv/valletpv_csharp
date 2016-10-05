// UtilByte.cs created with MonoDevelop
// User: valle at 14:23 27/04/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace Valle.Utilidades
{
    
    
    public class Bytes
    {
        // Devuelve el indice del byte buscado 
        public static int BuscarHEX(byte[] fuente,int ini, string hex){
        	int index = -1; 
        	for(int i= ini; i<fuente.Length;i++){
        		if(Convertir.DeBytesAStringHEX(fuente[i]).Equals(hex)){
        			return i;
        		}
        	}
        	return index;
        }
       
        // Devuelve el indice del byte buscado
        public static int BuscarHEX(byte[] fuente, int ini, int fin, string hex){
        	int index = -1; 
        	
        	if((fuente.Length-1)<fin)
        		          fin = fuente.Length-1;
        	
        	for(int i= ini; i<fin+1;i++){
        		if(Convertir.DeBytesAStringHEX(fuente[i]).Equals(hex)){
        			return i;
        		}
        	}
        	return index;
        }
       
        
      //copia un sub array de bytes
      //devuelve el numero de bytes copiados
       public static int CopiarBytes(byte[] des, byte[] fuente, int ini)
        {
           int proc = 0;
        	for(int i = 0;i<des.Length;i++){
        		if((i+ini)>=fuente.Length){break;}
        		   des[i] = fuente[i+ini]; proc++;
        	}
           return proc;
        }
        
        //copia la cantidad de bytes dese inicio hasta el numreo especificado en can
        //devuelve el array de bytes compiados
        public static byte[] CopiarBytes(Byte[] fuente,int ini, int can){
        	List<byte> des = new List<byte>();
        	for(int i = ini;i<ini+can;i++){
        		des.Add(fuente[i]);
        	}
        	return des.ToArray();
        }

        public static byte[] ExtraerBytes(List<byte> buff, int can){
        	byte[] des = new byte[can];
        	  for(int i = 0; i<can;i++){
        		if(buff.Count>0){
        	     	des[i] = buff[0];
        		    buff.RemoveAt(0);
        		}
        		   else
        		   	des[i] = 0x00;
        		
        	  }
        	 return des;
        
        }
        
        
    }
}
