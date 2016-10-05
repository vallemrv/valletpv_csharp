using System;
using System.Collections.Generic;
using System.Text;

namespace Valle.Utilidades
{
	public interface ObjOrdenable : IComparable
	{
	     int Orden { set;get;}	
	}
	
	
	
    class Pagina<T>
    {
        List<T> listaDeObjetos;
		int objPorPagina ;
        public Pagina(int numMaxObj)
        {
			objPorPagina = numMaxObj;
            listaDeObjetos = new List<T>();           
        }
		
        public T[] Objs
        {
            get { return listaDeObjetos.ToArray(); }
        }
       
		public int NumObjs{
		   get{ return listaDeObjetos.Count;}	
		}
       
		public bool insertarObj(T obj){
			if(listaDeObjetos.Count < objPorPagina){
		            listaDeObjetos.Add(obj);	return true;
			}else{
			    return false;	
			}
		}
			
	}
	
	
    public class PaginasObj<T> 
		
	  {
        int numObjMaxPag;
        

		public int NumObjMaxPag {
			get {
				return this.numObjMaxPag;
			}
		}
		
		Pagina<T>[] paginas;
        int puntPagina=0;
  
		public int PuntPagina {
			get {
				return this.puntPagina;
			}	
		}
		
		int numPaginas;
        public int NumPaginas {
	    	get { return numPaginas; }
     	 }
        
        public PaginasObj(int numObjMax, ObjOrdenable[] objs){
      	
			//actualizamos las propiedes internas
			
		    numPaginas = objs.Length==numObjMax? 1 : (objs.Length/(numObjMax))+1;
			numObjMaxPag=numObjMax;

            //Ordenamos los articulos segun el numero de orden
            Array.Sort((ObjOrdenable[])objs);
            
            //creamos las pagias e insertamos los articulos
            paginas = new Pagina<T>[numPaginas];
			paginas[puntPagina] = new Pagina<T>(numObjMax);
             foreach(T item in objs){
			   if(!paginas[puntPagina].insertarObj(item)){
					puntPagina++;
				    paginas[puntPagina] = new Pagina<T>(numObjMax);
					paginas[puntPagina].insertarObj(item);
				}
			 }
      }

	    public T[] Sigiente{
			get{
				if(puntPagina < (paginas.Length-1)) puntPagina++;
				return paginas[puntPagina].Objs;
			}
			
		}
		
    	public T[] Atras{
			get{
				if(puntPagina>0) puntPagina--;
				return paginas[puntPagina].Objs;
			}
			
		}
		
	    public T[] Primera{
			
			get{
			 	puntPagina = 0;
				return paginas[puntPagina].Objs;
			}
		}
		
		public T[] Ultima{
			get{ 
				puntPagina = paginas.Length-1;
				return paginas[puntPagina].Objs;}
		}	

		
		
     
	
		
    }
}
