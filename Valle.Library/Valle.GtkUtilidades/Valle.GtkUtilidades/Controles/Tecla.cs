// Tecla.cs created with MonoDevelop
// User: valle at 15:20 11/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Valle.GtkUtilidades
{
	[System.ComponentModel.Category("Valle.GtkUtilidades")]
    [System.ComponentModel.ToolboxItem(true)]
    public partial class MiBoton: Gtk.Bin
	{
	    public enum AccionesTecla {IniciarMover, Mover, Clickado}
        public delegate void OnEjAccionTecla(AccionesTecla accion, object dato);
	
	    
	    public event OnEjAccionTecla ClickBoton;
	    object datos;
		StringFormat _formato;
	    
	    public object Datos {
	    	get {
	    		return datos;
	    	}
	    	set {
	    		datos = value;
			}
	    }
	    
	   Color cFondo; 
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
			this.DibujarControl();	
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
	   
	    public MiBoton(){
		   this.Contruir();
		 _formato = new StringFormat(StringFormatFlags.LineLimit);
		 _formato.Alignment = StringAlignment.Center;
		 _formato.LineAlignment = StringAlignment.Center;
		
		}
	    
		public MiBoton(OnEjAccionTecla funcEj)
		{
	     this.Contruir();
		 // Targets
		 this.ClickBoton += new OnEjAccionTecla(funcEj);
		 Gtk.TargetEntry[] target = new Gtk.TargetEntry[] {new Gtk.TargetEntry ("STRING",Gtk.TargetFlags.App,0)};
	     Gtk.Drag.SourceSet (this.btnTecla, Gdk.ModifierType.Button1Mask, target, Gdk.DragAction.Move);
		 Gtk.Drag.DestSet(this.btnTecla, Gtk.DestDefaults.All, target, Gdk.DragAction.Move);
		 _formato = new StringFormat(StringFormatFlags.LineLimit);
		 _formato.Alignment = StringAlignment.Center;
		 _formato.LineAlignment = StringAlignment.Center;
		
		}

        
     	protected virtual void OnBtnTeclaDragDrop (object o, Gtk.DragDropArgs args)
     	{
			if(ClickBoton!=null)
     	     ClickBoton(AccionesTecla.Mover,this);  
     	}

     	protected virtual void OnBtnTeclaDragBegin (object o, Gtk.DragBeginArgs args)
     	{
			Gtk.Drag.SetIconPixbuf(args.Context,this.imgFondo.Pixbuf,0,0);
     	     if(ClickBoton!=null) ClickBoton(AccionesTecla.IniciarMover,this);
     	}

     	protected virtual void OnBtnTeclaClicked (object sender, System.EventArgs e)
     	{
     	    if(ClickBoton!=null) ClickBoton(AccionesTecla.Clickado,this);
     	}

     	
       protected void DibujarControl()
     	{
           Bitmap f = new Bitmap(this.imgFondo.Allocation.Width,this.imgFondo.Allocation.Height);
			
	         Graphics g = Graphics.FromImage(f);
			  if(!this.ColorDeFono.IsEmpty)
	               g.FillRectangle(new SolidBrush(cFondo),0,0,f.Width,f.Height);
			     else
				   g.FillRectangle(new SolidBrush(Color.LightGray),0,0,f.Width,f.Height);
			
			  if(this.ImgDeFondo!=null)
				 g.DrawImage(this.ImgDeFondo,3,3,f.Width-6,f.Height-6);
				                
	         g.DrawString(this.texto,this._font,Brushes.Black,new Rectangle(5,5,f.Width-10,f.Height-10),_formato);
			
     	    System.IO.MemoryStream ms =new System.IO.MemoryStream();
	          f.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
	          this.imgFondo.Pixbuf = new Gdk.Pixbuf(ms.ToArray());
			this.imgFondo.QueueDraw();
	     
         
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
