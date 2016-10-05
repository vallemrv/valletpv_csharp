// DibujoFlotante.cs created with MonoDevelop
// User: valle at 13:58 06/06/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

using Valle.GtkUtilidades;
using Valle.Utilidades.Imagenes;


namespace Valle.GesTpv
{
    
    
    public class DibujoFlotante
    {
        System.Drawing.Image[] fotos;
		Marco marco;
		ContenedorFlot flot;
		
		
	    public DibujoFlotante(Marco marco, System.Drawing.Image[] fs) 
		{
			fotos = fs;
			this.marco = marco;
			Gtk.Image controlImg = new Gtk.Image();
			flot = new ContenedorFlot(controlImg);
			flot.Resize(marco.dimension.Ancho,marco.dimension.Alto);
		    flot.Move(marco.localizacion.X, marco.localizacion.Y);
		    this.DibujarOjetos();
		    this.flot.ShowAll();
		}
		
		public void CambiarDim(int ancho, int alto){
		   marco.dimension = new Dimension(ancho,alto);
		   this.DibujarOjetos();
		   flot.Resize(marco.dimension.Ancho,marco.dimension.Alto);
       }
	
	    public void Move(int x, int y){
		  flot.Move(x,y);
		}
		
		public void Destroy(){
		  flot.Destroy();
		}

        
		protected virtual void DibujarOjetos()
	    {
	              Bitmap dib = new Bitmap(marco.Ancho,marco.Alto);
	              Graphics g = Graphics.FromImage(dib);
	              g.Clear(Color.White);
		        	for(int i = 0;i<fotos.Length;i++){
		               g.DrawImage(fotos[i],0,0,marco.Ancho,marco.Alto);
		               }
		            
		           (( Gtk.Image) flot.control).Pixbuf =  new Gdk.Pixbuf(Valle.Utilidades.Imagenes.UtilImagenes.DeBitmapABytes(dib)); 
	     }
   }
}
