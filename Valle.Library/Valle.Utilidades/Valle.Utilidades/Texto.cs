using System;
using System.Collections.Generic;
using System.Text;

namespace Valle.Utilidades
{
    public class CadenasTexto
    {
    	public static string[] SplitADosPuntos(string inst){
              	return inst.Split(':');
    	}
    	
        public static string InvertirCadena(string s)
        {
            char[] tex = s.ToCharArray();
            StringBuilder sOut = new StringBuilder();
            for (int i = tex.Length - 1; i >= 0; i--)
            {
                sOut.Append(tex[i]);
            }
            return sOut.ToString();
        }
        public static string RotarFecha(string fecha)
        {
            string[] dat = fecha.Split('/');
            StringBuilder sOut = new StringBuilder();
                for (int i = dat.Length - 1; i >= 0; i--)
                {
                    sOut.Append(dat[i]);
                    if (i != 0) { sOut.Append('/'); }
                }
                return sOut.ToString();
        }
        
        public static string[] EsctraerSubString(string str, int longitud){
            List<string> cadenas = new List<string>();
            int procesados = 0;
            string[] palabras = str.Split(' ');
            StringBuilder sb = new StringBuilder();
             foreach(string pal in palabras){
              if((pal.Length+procesados)>longitud){
                 cadenas.Add(sb.ToString().TrimEnd());
                 sb = new StringBuilder();
                 sb.Append(pal+" ");
                 procesados = 0;
              }else{
                sb.Append(pal+" ");
                procesados+=pal.Length;
              }
           } 
           if(sb.Length>0) cadenas.Add(sb.ToString().TrimEnd());
           
           return cadenas.ToArray();
        }
        
        public static string LimpiarSignos(string s){
          string resultado = s.Replace(',',' ');
          resultado = resultado.Replace('(',' ');
          resultado = resultado.Replace(')',' ');
          resultado = resultado.Replace('.',' ');
          resultado = resultado.Replace('/',' ');
          resultado = resultado.Replace('-',' ');
		  	
           return resultado;
        }
        
        public static string LimpiarNumeros(string s){
          string resultado = s.Replace('0',' ');
          resultado = resultado.Replace('1',' ');
          resultado = resultado.Replace('2',' ');
          resultado = resultado.Replace('3',' ');
           resultado = resultado.Replace('4',' ');
          resultado = resultado.Replace('5',' ');
          resultado = resultado.Replace('6',' ');
          resultado = resultado.Replace('7',' ');
          resultado = resultado.Replace('8',' ');
          resultado = resultado.Replace('9',' ');
          return resultado;
        }
        
        public static string LimpiarSignos(string s, char remplazo){
          string resultado = s.Replace(',',remplazo);
          resultado = resultado.Replace('(',remplazo);
          resultado = resultado.Replace(')',remplazo);
          resultado = resultado.Replace('.',remplazo);
          resultado = resultado.Replace('/',remplazo);
          resultado = resultado.Replace('-',remplazo);
		   return resultado;
        }
        
        public static string LimpiarNumeros(string s, char remplazo){
          string resultado = s.Replace('0',remplazo);
          resultado = resultado.Replace('1',remplazo);
          resultado = resultado.Replace('2',remplazo);
          resultado = resultado.Replace('3',remplazo);
          resultado = resultado.Replace('4',remplazo);
          resultado = resultado.Replace('5',remplazo);
          resultado = resultado.Replace('6',remplazo);
          resultado = resultado.Replace('7',remplazo);
          resultado = resultado.Replace('8',remplazo);
          resultado = resultado.Replace('9',remplazo);
          return resultado;
        }

        
    }
}
