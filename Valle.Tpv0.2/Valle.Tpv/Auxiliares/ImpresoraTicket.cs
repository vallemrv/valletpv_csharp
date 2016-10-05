using System;
using Valle.ToolsTpv;
using Valle.GtkUtilidades;

namespace Valle.TpvFinal
{
	public class ImpresoraTicket
	{
		
		public static void PreimprimirTicket (string rutFicheros, string nombreImpresora,
			         Articulo[] lineas, DateTime now, string mesa, int numCopiasTicket, decimal totalFactura)
		{		
			       Valle.GtkUtilidades.DocumentPrint docPrint = new Valle.GtkUtilidades.DocumentPrint();
			   docPrint.IniciarDoc(nombreImpresora); 
			
				    docPrint.AddLinea("PREIMPRESION DEL TICKET",
			                  DocumentPrint.Alineacion.centro,DocumentPrint.Tamaño.grande,true);
			        docPrint.AddLinea();
			        docPrint.AddLinea("Copia nº "+ numCopiasTicket);
				    docPrint.AddLinea(String.Format("Mesa: {0} ",mesa));
		            docPrint.AddLinea("");
		            docPrint.AddLinea("Can  Descripcion            Precio   Total");
		            docPrint.AddLinea("------------------------------------------");
		 
		            foreach(Articulo articulo in lineas)
		            {
						String precioImp = String.Format("{0:#0.00}", articulo.precio);
		                String totalImp = String.Format("{0:#0.00}", articulo.TotalLinea);
				        docPrint.AddLinea(String.Format("{0:#0.###} {1} {2} {3}",
		                   articulo.Cantidad.ToString().PadLeft(4)
		                   , Valle.Utilidades.CadenasTexto.EsctraerSubString(articulo.Descripcion,23)[0].Trim().PadRight(23),
		                    precioImp.PadLeft(6),
		                         totalImp.PadLeft(6)));
		            }
		
		           docPrint.AddLinea("");
				
				    
 
				     string monedaformat = String.Format("{0:c}",totalFactura);
				     docPrint.AddLinea(String.Format("Total Ticket :        {0}", monedaformat.PadLeft(7)),
			                  Valle.GtkUtilidades.DocumentPrint.Alineacion.centro,DocumentPrint.Tamaño.grande,true);
		    	    
				        docPrint.AddLinea(String.Format("{0:g}", now),DocumentPrint.Alineacion.centro);
			            docPrint.AddLinea("");
			            docPrint.AddLinea("");
					
			
			   docPrint.ImprimirDoc();
			   docPrint.Dispose();
			   
	
		}

		public static void ImprimeResumen(string rutFicheros, string nombreImpresora, string[] resumen, string nomRes){
		      
			       	Valle.GtkUtilidades.DocumentPrint docPrint = new Valle.GtkUtilidades.DocumentPrint();
			   docPrint.IniciarDoc(nombreImpresora); 
			   

		
		            docPrint.AddLinea(String.Format("Resumen  {0}",nomRes));
		            docPrint.AddLinea(String.Format("Fecha impresion: {0:g}", DateTime.Now));
		            docPrint.AddLinea("------------------------------------------");
		            docPrint.AddLinea("");
		            foreach (String linea in resumen)
		            {
		                docPrint.AddLinea(linea);
		            }
				
				    docPrint.ImprimirDoc();
			        docPrint.Dispose();
		}
		
		public static void ImprimirInforme(string rutficheros,
			string nombreImpresora, string[] resumen, string Titulo){
			
			Valle.GtkUtilidades.DocumentPrint docPrint = new Valle.GtkUtilidades.DocumentPrint();
			   docPrint.IniciarDoc(nombreImpresora); 
			   

            docPrint.AddLinea(String.Format(Titulo));
            docPrint.AddLinea("------------------------------------------");
            docPrint.AddLinea("");
            foreach (String linea in resumen)
            {
                docPrint.AddLinea(linea);
            }
				
				
			 docPrint.ImprimirDoc();
			   docPrint.Dispose();
		}
		
		public static void AbrirCajon(string rutFicheros, string nombreImpresora){
			Valle.GtkUtilidades.DocumentPrint docPrint = new Valle.GtkUtilidades.DocumentPrint();
			   docPrint.IniciarDoc(nombreImpresora); 
			   docPrint.AbrirCajon();
			
    	}
		
		
			
		
		public static void ImprimirTicket (string rutFicheros, string nomImpresora, System.Collections.IEnumerator  lineas,
            String camarero, String mesa, DateTime fechaTicket, int numTicket, decimal totUltima, decimal cambio)
			
			
		{
			
			Valle.GtkUtilidades.DocumentPrint docPrint = new Valle.GtkUtilidades.DocumentPrint();
			   docPrint.IniciarDoc(nomImpresora); 
			        
			        docPrint.ImprimirLogo();
			        docPrint.AddLinea();
			        docPrint.AddLinea();
				    docPrint.AddLinea(String.Format("Camarero: {0} ",camarero));
		            docPrint.AddLinea(String.Format("Mesa: {0} ",mesa));
		            docPrint.AddLinea("");
		            docPrint.AddLinea("Can  Descripcion            Precio   Total");
		            docPrint.AddLinea("------------------------------------------");
		 
		            while (lineas.MoveNext())
		            {
						Articulo articulo = (Articulo)((System.Collections.DictionaryEntry)lineas.Current).Value;
		                String precioImp = String.Format("{0:#0.00}", articulo.precio);
		                String totalImp = String.Format("{0:#0.00}", articulo.TotalLinea);
				        docPrint.AddLinea(String.Format("{0:#0.###} {1} {2} {3}",
		                   articulo.Cantidad.ToString().PadLeft(4)
		                   , Valle.Utilidades.CadenasTexto.EsctraerSubString(articulo.Descripcion,23)[0].Trim().PadRight(23),
		                    precioImp.PadLeft(6),
		                         totalImp.PadLeft(6)));
		            }
		
		           docPrint.AddLinea("");
				
				    
 
				     string monedaformat = String.Format("{0:c}",totUltima);
				     docPrint.AddLinea(String.Format("Total Ticket :        {0}", monedaformat.PadLeft(7)),
			                  Valle.GtkUtilidades.DocumentPrint.Alineacion.izquierda,DocumentPrint.Tamaño.grande,true);
		    	     monedaformat = String.Format("{0:c}",cambio+totUltima);
			        docPrint.AddLinea(
	                String.Format("Entrega :        {0}", monedaformat.PadLeft(7)),DocumentPrint.Alineacion.izquierda);
				      monedaformat = String.Format("{0:c}",cambio);
				    docPrint.AddLinea(
	                String.Format("Cambio  :        {0}", monedaformat.PadLeft(7)),DocumentPrint.Alineacion.izquierda);
				     
				     docPrint.AddLinea();
					
				        docPrint.AddLinea(String.Format("{0:g}", fechaTicket),DocumentPrint.Alineacion.centro);
			            docPrint.AddLinea(String.Format("Ticket Num: {0}", numTicket),DocumentPrint.Alineacion.centro);
			            docPrint.AddLinea("");
			            docPrint.AddLinea("");
						docPrint.AddLinea("Gracias por su visita",DocumentPrint.Alineacion.centro);
		            
			
			   docPrint.ImprimirDoc();
			   docPrint.Dispose();
			   
	
			  
		}
	
	        
      }
}

