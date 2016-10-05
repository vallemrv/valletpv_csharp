using System;
using System.Collections.Generic;
using Valle.Utilidades;

namespace Valle.GtkUtilidades
{
	
	
	public delegate void OnClickBtnBotonera(MiBoton boton, object args);
   // public enum PosicionBotonSalir { arribaDerecha, arribaIzquierda, abajoDerecha, abajoIzquierda }
    
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Botonera: Gtk.Bin
	{
		Gtk.Table tblBotonera ;
	    Valle.Utilidades.PaginasObj<IInfBoton> pagObj;

		public Botonera ()
		{
			this.Build (); 
		}
	
        public event OnClickBtnBotonera clickBoton;
		public event EventHandler paginacionClick;
        
		int botonesEnAlto = 5;
        int botonesEnAncho = 5;
		
		public void Redimensionar(int alto, int ancho){
			botonesEnAlto = alto; botonesEnAncho = ancho;
		}
		
       public bool MostrarSalir{
		    set{
			    this.btnSalir.Visible = value;	
			}
			get{
			    return this.btnSalir.Visible;	
			}
		}
		
		public bool MostrarMas{
		   set{
				btnAgregar.Visible = value;
			}
		   get{
			   return	btnAgregar.Visible;
			}
		}
		
		public bool Paginable{
			set{ 
				this.btnAbajo.Visible = value;
				this.btnArriba.Visible = value;
			}
			get{ return this.btnArriba.Visible;}
		}
       
	   
        public bool MostrarControles{
		   set{
				pnePaginacion.Visible = value;
			}
			get{
				return pnePaginacion.Visible;
			}
		}
		
		Dictionary<IInfBoton,MiBoton> botonesGtk = new Dictionary<IInfBoton, MiBoton>();
		public Dictionary<IInfBoton,MiBoton> BotonesGtk{
			get { return botonesGtk;}
		}
   
        public Botonera(OnClickBtnBotonera Accion, int altoEnBotones, int anchoEnBotones)
        {
            this.Build();
			clickBoton = Accion;
            botonesEnAncho = anchoEnBotones;
            botonesEnAlto = altoEnBotones;
			
	   }

        public void AgregarBotones(IInfBoton[] botonera)
        {
			if(tblBotonera!=null)
				tblBotonera.Destroy();
				
			botonesGtk.Clear();
			int row = botonera.Length == botonesEnAncho ? 1 : (botonera.Length/botonesEnAncho);
			int colTb = botonera.Length < botonesEnAncho ? botonera.Length : botonesEnAncho;
			tblBotonera = new Gtk.Table((uint)row, (uint)colTb, true);
			
			pneBotonera.Add(tblBotonera);
			
			this.Paginable = false;
			
            if ((botonera!=null)&&(botonera.Length > 0))
            {
                uint col = 0, fila = 0;
				
                foreach(IInfBoton b in botonera){
				    MiBoton boton = new MiBoton();
					boton.ClickBoton += HandleBotonEjAccion;
					boton.SetSizeRequest(b.Tamaño.Width,b.Tamaño.Height);
					boton.Datos = b.Datos;
					boton.ColorDeFono = b.ColorDeAtras;
					boton.ImgDeFondo = b.ImgFondo;
					boton .Font = b.Font;
					boton.Texto = b.Texto;
					this.tblBotonera.Attach(boton,col,col+1,fila,fila+1);
					if(col < colTb-1) col++;
					else {col = 0;fila++;}
					botonesGtk.Add(b,boton);
					
				}
				pneBotonera.ShowAll();
                
            }
          
         }

		public void AgregarBotones(Valle.Utilidades.PaginasObj<IInfBoton> pgs)
        {
			this.VaciarBotones();
			botonesGtk.Clear();
			this.Paginable = true;
			this.pagObj = pgs;
			this.RellenarElTecladoDeTeclas();
			this.RellenarTeclado(pagObj.Primera);
				     
		}

		private void RellenarElTecladoDeTeclas(){
		    for(uint i = (uint)botonesEnAlto;i>0;i--){
                for(uint j= (uint)botonesEnAncho;j>0;j--){
                  this.tblBotonera.Attach(new MiBoton(this.HandleBotonEjAccion),j-1,j,i-1,i);   
                }
		    }
		}
		
        void HandleBotonEjAccion (MiBoton.AccionesTecla accion, object obj)
		{
			if(accion == MiBoton.AccionesTecla.Clickado){
        	      if(clickBoton!=null) clickBoton((MiBoton)obj,((MiBoton)obj).Datos);
			}
        }
        
        public void VaciarBotones(){
          if(tblBotonera!= null){
			this.pneBotonera.Remove(tblBotonera);
		    tblBotonera.Destroy();
			}
			this.tblBotonera = new Gtk.Table((uint)botonesEnAncho,(uint)botonesEnAncho,true);
			this.pneBotonera.Add (this.tblBotonera);
			this.pneBotonera.ShowAll();
			
        }

     
      

       
	   public  void  btnSalir_Click(object sender, EventArgs e)
       {
              if(clickBoton!=null) clickBoton(null,null);
       }

	   protected virtual void OnBtnArribaClicked (object sender, System.EventArgs e)
	   {
			this.RellenarTeclado(this.pagObj.Atras);
			if(paginacionClick!=null) paginacionClick(sender,e);
			
	   }
		
	   protected virtual void OnBtnAbajoClicked (object sender, System.EventArgs e)
	   {
		    this.RellenarTeclado(this.pagObj.Sigiente);
			if(paginacionClick!=null) paginacionClick(sender,e);
	   }
		
		
	   void RellenarTeclado(IInfBoton[] infB){
	  
	    int pos = 0;
	     if(this.tblBotonera.Children.Length>0){
	           foreach (Gtk.Widget tecla in this.tblBotonera.Children){
	              if(tecla is Valle.GtkUtilidades.MiBoton){
	                  MiBoton estaTecla = ((MiBoton)tecla);
	                 
	                 if(pos<infB.Length){
					  
					   estaTecla.Datos = infB[pos].Datos;
					   estaTecla.SetSizeRequest(infB[pos].Tamaño.Width,infB[pos].Tamaño.Height);
					   estaTecla.ImgDeFondo = infB[pos].ImgFondo;
	                   estaTecla.Visible = true;
	                   estaTecla.ColorDeFono = infB[pos].ColorDeAtras;
	                   estaTecla.Texto = infB[pos].Texto;
					   estaTecla.Font = infB[pos].Font;
					    
						}else{
							
	                   estaTecla.Datos = null;
	                   estaTecla.Visible = false;
	                  }
	                  pos++;
	                }
	             }
	           }
	       
	      
	    }
		   
	   protected virtual void OnBtnAgregarClicked (object sender, System.EventArgs e)
	   {
		   if(clickBoton!=null) clickBoton(null,"mas");	
	   }
		
		
		
	}
}

