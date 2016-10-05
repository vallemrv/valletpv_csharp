using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Valle.GtkUtilidades
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class MiLabel : Gtk.Bin
	{
		public event EventHandler TextoCambiado;
		
	   Color cLetras = Color.Black; 
	   public Color ColorLetras{
	      set{
	         cLetras = value;
             this.DibujarControl();	        
	      }
	      get{
	        return cLetras;
	      }
	   
	   }
		
		Color cFondo = SystemColors.Control; 
	   public Color ColorDeFono{
	      set{
	         cFondo = value;
             this.DibujarControl();	        
	      }
	      get{
	        return cFondo;
	      }
	   
	   }
		
		string texto;	   
	   public string Texto{
	     get{
	      return texto;
	     }
	     set{
	       texto = value;
			if(TextoCambiado!=null) TextoCambiado(this,new EventArgs());
			this.DibujarControl();	
	     }
	    }
		
		 public Font Font {
	    	get {
	    		return _font;
	    	}
	    	set {
	    		_font = value;
				this.DibujarControl();
	     	}
	    }
	   
		Font _font = new Font("sans",9);
		StringFormat _formato;
	    
		public StringAlignment AlienamientoV{
		    set{ _formato.LineAlignment = value;}	
		}
		
		public StringAlignment AlienamientoH{
		    set{ _formato.Alignment = value;}	
		}
		
		public MiLabel ()
		{
			this.Contruir ();
			 _formato = new StringFormat(StringFormatFlags.LineLimit);
		     _formato.Alignment = StringAlignment.Near;
		     _formato.LineAlignment = StringAlignment.Center;
		}
		
		protected void DibujarControl()
     	{
		  Bitmap f = new Bitmap(this.Allocation.Width,this.Allocation.Height);
			
	         Graphics g = Graphics.FromImage(f);
			  if(!this.ColorDeFono.IsEmpty)
	               g.FillRectangle(new SolidBrush(cFondo),0,0,f.Width,f.Height);
			     else
				   g.FillRectangle(new SolidBrush(Color.LightGray),0,0,f.Width,f.Height);
			
			 SolidBrush brocha = new SolidBrush(cLetras);	                
	         g.DrawString(this.texto,this._font,brocha,new Rectangle(5,5,f.Width-10,f.Height-10),_formato);
			
     	    System.IO.MemoryStream ms =new System.IO.MemoryStream();
	          f.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
	          this.imgVista.Pixbuf = new Gdk.Pixbuf(ms.ToArray());ms.Close();
			this.imgVista.QueueDraw();
	     
         
     	}
       
	   bool unavez = true;
       protected virtual void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
       {
			if(unavez){
			 this.DibujarControl();
		     unavez = false;
			}
			  else
				  unavez = true;
			 
        }

       
		
		
	}
}

