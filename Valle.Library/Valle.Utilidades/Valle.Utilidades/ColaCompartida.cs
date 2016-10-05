// MyClass.cs created with MonoDevelop
// User: valle at 20:54 04/03/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Valle.Utilidades
{
   public class ColaCompartida<T>
	{
	   
	    Queue<T> colaCompartida;
		AutoResetEvent hayPaquetes;
		AutoResetEvent colaVacia;
        
	    public int NumElementos{
		   get{
		     return colaCompartida.Count;
		   }
		}
		
		public ColaCompartida(AutoResetEvent colaVacia){
		   colaCompartida = new Queue<T>();
		   hayPaquetes = new AutoResetEvent(false);
		   this.colaVacia = colaVacia;
		}
		
		public ColaCompartida()
		{
		   colaCompartida = new Queue<T>();
		   hayPaquetes = new AutoResetEvent(false);
		}
		
		public void Encolar(T o)
	    {
		   lock(colaCompartida){
			  colaCompartida.Enqueue(o);
			  hayPaquetes.Set();
			  if(colaVacia!=null) colaVacia.Reset();
			}
        }
	
		public T Descolar()
		{
		  object pack = null;
		  bool hay = false;	
		   while(pack == null){	
			lock(colaCompartida){
			  if(colaCompartida.Count > 0){
					pack = colaCompartida.Dequeue();
					hay = true;
			   }
			}
				if(!hay){
				  if(colaVacia!=null) colaVacia.Set();
				  hayPaquetes.WaitOne();
				  }
		}
			return (T)pack;
		}                             
	
	    public void Baciar(){
	      lock(colaCompartida){
			this.colaCompartida.Clear();
			}
        }
		
	}
   
    
 
}