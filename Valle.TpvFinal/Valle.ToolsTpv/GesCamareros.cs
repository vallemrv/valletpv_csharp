using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

using Valle.SqlUtilidades;
using Valle.SqlGestion;
using Valle.Utilidades;

namespace Valle.ToolsTpv
{
	public class Camarero: IInfBoton, ObjOrdenable{
	   
	   int orden = 0;	 
	   public string nombre;
		
		#region IComparable implementation
		public int CompareTo (object obj)
		{
			return this.Orden.CompareTo((obj as ObjOrdenable).Orden);
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

		#region IInfBoton implementation
		public object Datos {
			get {
				return this;
			}
			set {
				
			}
		}

		
		System.Drawing.Color _color = SystemColors.Control;
		public Color ColorDeAtras {
			get {
				return _color;
			}
			set {
				_color = value;
			}
		}

		public string Texto {
			get {
				return nombre;
			}
			set {
				nombre = value;
			}
		}
		
		System.Drawing.Bitmap b = null;
		public Bitmap ImgFondo {
			get {
				return b;
			}
			set {
				b= value;
			}
		}

		System.Drawing.Size _size = new System.Drawing.Size(100,100);
		public Size Tamaño {
			get {
				return _size;
			}
			set {
				_size = value;
			}
		}

		System.Drawing.Font  _font = new System.Drawing.Font("monoscapce",10f);
		
		public Font Font {
			get {
				return _font;
			}
			set {
				_font = value;
			}
		}
		#endregion

		

		
	}
	
    public class GesCamareros
    {
		public static Color COLOR_SELECCION = SystemColors.ActiveBorder;
		public static Color COLOR_DEFECTO = SystemColors.Control;
        
		
        public List<Camarero> camarerosActivos = new List<Camarero>();
		public PaginasObj<IInfBoton> pagCamareros;
		public String nomCamareroActivo;
        public bool masSensible = true;
        IGesSql gesBase;
        DataTable tb;
		int numCamPag = 0;
		
        public GesCamareros(IGesSql ges,int numCamPag)
        {
            gesBase = ges;
            tb = ges.ExtraerTabla("Camareros","IDCamarero");
			this.numCamPag = numCamPag;
			CargarTodosLosCamareros();
        }

        void CargarTodosLosCamareros()
        {
			camarerosActivos.Clear();
			 int pos = 0;
			 Camarero[] camarero = new Camarero[tb.Rows.Count];
			
              foreach (DataRow cam in tb.Rows)
                  {
                    camarero[pos] = new Camarero();
				    camarero[pos].Orden = pos;
				    camarero[pos].nombre = cam["Nombre"].ToString() + " " + cam["Apellidos"].ToString();
	               			
				     pos++;
				  }
			  pagCamareros = new PaginasObj<IInfBoton>(numCamPag,camarero);
			
             }
		
       
		
        public DataRow regCamareros()
        {
            return tb.NewRow();
        }
		
        public void AgregarCamarero(DataRow dr)
        {
            lock (tb)
            {
                tb.Rows.Add(dr); 
				gesBase.EjConsultaNoSelect("Camareros",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(dr,AccionesConReg.Agregar,
				                                                                "").Replace(@"\",@"\\"));
				CargarTodosLosCamareros();
            }
            new GesSincronizar(gesBase).ActualizarSincronizar("Camareros", "IDCamarero =" + dr["IDCamarero"].ToString(),
                 AccionesConReg.Agregar);
        }
		
      
    }
}
