using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Valle.GesTpv
{
    class Pagina
    {
        List<DatosTecla> art;
        public Pagina()
        {
            art = new List<DatosTecla>();           
        }
        public List<DatosTecla> ListaDatosEstaPagina
        {
            get { return art; }
        }
    
    }
    
    class PaginasArticulos
    {
        List<Pagina> paginas;
        int maxOrden = 0;
        List<DatosTecla> listaTeclas;
        int numArtPorPagina = 0;
        
        
        
        int puntPagina=0;
        public int PuntPagina {
    		get { 
    		   if(puntPagina >= this.paginas.Count)
    		                puntPagina = this.paginas.Count-1;
    		   return puntPagina; 
    		 }
            set
            {
                if ((value < paginas.Count)&&(value>=0))
                {
                    puntPagina = value;
                }
            }
      	}
      	
        
        public List<DatosTecla> Pagina
        {
            get { return paginas[PuntPagina].ListaDatosEstaPagina; }
        }
        
        public int NumPaginas {
	    	get { return this.paginas.Count; }
     	 }

        public int MaxOrden {
        	get {
        		return maxOrden;
        	}
        	set {
        		maxOrden = value;
        	}
        }

        public List<DatosTecla> ListaTeclas {
        	get {
        		return listaTeclas;
        	}
        	set {
        		listaTeclas = value;
        	}
        }
        
        public PaginasArticulos(int numArtMax, List<DatosTecla> articulos){
      	  //actualizamos las propiedaes internas
      	  this.numArtPorPagina = numArtMax;
      	  this.MaxOrden = articulos.Count > 0 ? articulos[articulos.Count-1].Orden : 0;
          //creamos las pagias e insertamos los articulos
          paginas = new List<Pagina>();
          listaTeclas = articulos;
          this.PaginarAriculos();
        }
             
        public void PaginarAriculos(){
           paginas.Clear();
           Pagina pagina = new Pagina();
             
          foreach(DatosTecla dT in listaTeclas)
          {
             if(pagina.ListaDatosEstaPagina.Count  >= this.numArtPorPagina){
                 paginas.Add(pagina);
                 pagina = new Pagina();
                 }
                 pagina.ListaDatosEstaPagina.Add(dT);
          }
          
          if((!paginas.Contains(pagina))&&(pagina.ListaDatosEstaPagina.Count>0))
                                                                          paginas.Add(pagina);
        }

      
 }
}
