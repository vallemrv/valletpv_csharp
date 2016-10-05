using System;
namespace Valle.Utilidades
{
	public interface IInfBoton: Valle.Utilidades.ObjOrdenable
	{
		object Datos{set;get;}
		System.Drawing.Color ColorDeAtras{get;set;}
		string Texto{get;set;}
		System.Drawing.Bitmap ImgFondo{set;get;}
		System.Drawing.Size Tamaño{set;get;}
		System.Drawing.Font Font {set;get;}
		
	}
	
	public class InfBoton : IInfBoton{
	    
		object datos = null;
		System.Drawing.Color colorDeAtras = System.Drawing.SystemColors.Control;
		string texto = "";
		System.Drawing.Bitmap imgFondo = null;
		System.Drawing.Size tamaño = new System.Drawing.Size(100,100);
		int orden = 0;
		System.Drawing.Font font = new System.Drawing.Font("sans",10f);
		                                                   
		public System.Drawing.Font Font {
			get {
				return this.font;
			}
			set {
				font = value;
			}
		}

		#region IComparable implementation
		public int CompareTo (object obj)
		{
			return this.Orden.CompareTo((obj as ObjOrdenable).Orden);
		}
		#endregion

		#region IInfBoton implementation
		public object Datos {
			get {
				return datos;
			}
			set {
				datos = value;
			}
		}

		public System.Drawing.Color ColorDeAtras {
			get {
			    return	colorDeAtras ;
			}
			set {
				colorDeAtras = value;
			}
		}

		public string Texto {
			get {
				return texto ;
			}
			set {
				texto = value;
			}
		}

		public System.Drawing.Bitmap ImgFondo {
			get {
				return imgFondo;
			}
			set {
				imgFondo = value;
			}
		}

		public System.Drawing.Size Tamaño {
			get {
				return tamaño;
			}
			set {
				tamaño = value;
			}
		}
		#endregion

		#region ObjOrdenable implementation
		public int Orden {
			get {
				return orden;
			}
			set {
				orden = value;
			}
		}
		#endregion		
	}
}

