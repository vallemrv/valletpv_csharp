// MiImagen.cs created with MonoDevelop
// User: valle at 16:44 14/06/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Valle.GtkUtilidades
{
    
    [System.ComponentModel.Category("Valle.GtkUtilidades")]
    [System.ComponentModel.ToolboxItem(true)]
    public partial class CImagen : Gtk.Bin
    {
         object datos;
	    
	    public object Datos {
	    	get {
	    		return datos;
	    	}
	    	set {
	    		datos = value;
	    	}
	    }
	    
	   Color cFondo; 
	   public Color ColorDeFondo{
	      set{
	         cFondo = value;
	         this.DibujarControl();
	      }
	      get{
	        return cFondo;
	      }
	   
	   
	   }
	   
	   Bitmap fotoFondo;
	    public Bitmap ImgDeFondo{
	       set{
	          fotoFondo = value;
	         this.DibujarControl();
	       }
	       
	       get{
	         return fotoFondo;
	       }
	    }
	   
        public CImagen()
        {
            this.Build();
        }
        
		
		protected void DibujarControl()
     	{
           Bitmap f = new Bitmap(this.imgFondo.Allocation.Width,this.imgFondo.Allocation.Height);
			
	         Graphics g = Graphics.FromImage(f);
			  if(!this.ColorDeFondo.IsEmpty)
	               g.FillRectangle(new SolidBrush(cFondo),0,0,f.Width,f.Height);
			     else
				   g.FillRectangle(new SolidBrush(Color.LightGray),0,0,f.Width,f.Height);
			
			  if(this.ImgDeFondo!=null)
				 g.DrawImage(this.ImgDeFondo,3,3,f.Width-6,f.Height-6);
				                
	     	
     	    System.IO.MemoryStream ms =new System.IO.MemoryStream();
	          f.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
	          this.imgFondo.Pixbuf = new Gdk.Pixbuf(ms.ToArray());
			this.imgFondo.QueueDraw();
	     
         
     	}
       
		bool primeravez = true;
     	protected virtual void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
     	{
     	  if(primeravez){
     	     this.DibujarControl();
     	     this.primeravez = false;
          }else{
     	     primeravez = true;
     	  }
     	}

    }
}
