/*
 * Creado por SharpDevelop.
 * Usuario: vallevm
 * Fecha: 14/08/2008
 * Hora: 22:56
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */

using System;
using System.Drawing;
using System.Collections.Generic;

using Valle.Utilidades;

namespace Valle.ToolsTpv
{
	/// <summary>
	/// Description of Ticket.
	/// </summary>
    [Serializable]
	public class Articulo : ObjOrdenable
	{
        private int numTicketPertenencia;

        public int NumTicketPertenencia
        {
            get { return numTicketPertenencia; }
            set { numTicketPertenencia = value; }
        }

        private Ronda combidadPertenencia;

        internal Ronda CombidadPertenencia
        {
            get { return combidadPertenencia; }
            set { combidadPertenencia = value; }
        }

       
        private List<Articulo> desglose = new List<Articulo>();

        public List<Articulo> Desglose
        {
            get { return desglose; }
            set { desglose = value; }
        }

        private int orden;

        public int Orden
        {
            get { return orden; }
            set { orden = value; }
        }
        private Color miColor;

        public Color MiColor
        {
            get { return miColor; }
            set { miColor = value; }
        }

        private string idArticulo;

        public string IDArticulo
        {
            get { return idArticulo; }
            set { idArticulo = value; }
        }

		private int tarifa;
		
		public int Tarifa {
			get { return tarifa; }
			set {
				if (( value >3)||(value<1)){
					tarifa = 1;
				}else{
				  tarifa = value;
				}
				
			}
		}
        private bool ventaPorKilos;

        public bool VentaPorKilos
        {
            get { return ventaPorKilos; }
            set { ventaPorKilos = value; }
        }

		private decimal precio2;
		
		public decimal Precio2 {
			get { return precio2; }
			set { precio2 = value; }
		}
		private decimal precio3;
		
		public decimal Precio3 {
			get { return precio3; }
			set { precio3 = value; }
		}
		private decimal precio1;
		
		public decimal Precio1 {
			get { return precio1; }
			set { precio1 = value; }
		}
		public decimal precio;
		public decimal Precio {
		
			get { 
				
				return precio; 
			}
		
		}
		private decimal cantidad;
		
		public decimal Cantidad {
			get { return cantidad; }
			set {
				switch(tarifa){
					case 1:
					cantidad = value;
					precio = precio1;
				    totalLinea = precio1*cantidad;
				    break;
				   case 2: 
				    cantidad = value;
				    precio = precio2;
				    totalLinea = precio2*cantidad;
				    
				    break;
				   case 3:
				    cantidad = value;
				    precio = precio3;
				    totalLinea = precio3*cantidad;
				    
				    break;
				   default:
				    cantidad = value;
				    precio = precio1;
				    totalLinea = precio1*cantidad;
				    break;
				}
			}
		}
		private decimal totalLinea;
		
		public decimal TotalLinea {
			get { return totalLinea; }
			set { totalLinea = value; }
		}
		private String descripcion;
	
		public string Descripcion {
			get { return descripcion; }
			set { descripcion = value; }
		}
		private String nombreCorto;
		
		public string NombreCorto {
			get { return nombreCorto; }
			set { nombreCorto = value; }

		}
        private String impresora;

        public String Impresora
        {
            get { return impresora; }
            set { impresora = value; }
        }

        public Articulo Clone()
        {
            Articulo aux = new Articulo();
            aux.Cantidad=this.cantidad;
            aux.Descripcion=this.descripcion;
            aux.IDArticulo= this.idArticulo;
            aux.NombreCorto=this.nombreCorto;
            aux.precio=this.precio;
            aux.Precio1=this.precio1;
            aux.Precio3=this.precio3;
            aux.Precio2= this.precio2;
            aux.Tarifa=this.tarifa;
            aux.MiColor = this.miColor;
            aux.TotalLinea=this.totalLinea;
            aux.Desglose = this.Desglose;
            aux.VentaPorKilos = this.VentaPorKilos;
            aux.Impresora = this.Impresora;
            aux.CombidadPertenencia = this.combidadPertenencia;
            aux.NumTicketPertenencia = this.numTicketPertenencia;
            return aux;
            
        }
		
		#region IComparable implementation
		public int CompareTo (object obj)
		{
			return this.Orden.CompareTo((obj as ObjOrdenable).Orden);
		}
		#endregion
	}
}
