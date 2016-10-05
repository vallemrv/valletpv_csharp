using Gtk;
using System;
using System.Collections;
using System.Data;
using Valle.ToolsTpv;
using Valle.SqlGestion;

namespace Valle.TpvFinal
{
	public class TagNames{
		public static string maspeque = "maspequeña";
		public static string peque = "pepue";
		public static string mediana = "mediana";
		public static string grande = "grande";
		public static string rojo = "red_background";
		public static string azul = "blue_background";
		public static string centro = "center";
		public static string derecha = "right_justify";
		public static string izquierda = "left_justify" ;
		public static string monospace = "monospace";
		public static string italica = ("italic");
		public static string negrita = ("bold");
		
	}
	
	
	
	
	public class VisorDeInformes
	{
		
		 public static void CreateTags (TextBuffer buffer)
		{
			
			TextTag tag  = new TextTag (TagNames.grande);
			tag.Size = (int) Pango.Scale.PangoScale * 20;
			buffer.TagTable.Add (tag);

			tag  = new TextTag (TagNames.maspeque);
			tag.Size = (int) Pango.Scale.PangoScale * 7;
			buffer.TagTable.Add (tag);
			
			tag  = new TextTag (TagNames.peque);
			tag.Size = (int) Pango.Scale.PangoScale * 10;
			buffer.TagTable.Add (tag);
			
			tag  = new TextTag (TagNames.mediana);
			tag.Size = (int) Pango.Scale.PangoScale * 15;
			buffer.TagTable.Add (tag);
			
			tag  = new TextTag ("italic");
			tag.Style = Pango.Style.Italic;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("bold");
			tag.Weight = Pango.Weight.Bold;
			buffer.TagTable.Add (tag);

			
			tag  = new TextTag ("monospace");
			tag.Family = "monospace";
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("blue_foreground");
			tag.Foreground = "blue";
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("red_background");
			tag.Background = "red";
			buffer.TagTable.Add (tag);

			
			tag  = new TextTag ("center");
			tag.Justification = Justification.Center;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("right_justify");
			tag.Justification = Justification.Right;
			buffer.TagTable.Add (tag);

			tag  = new TextTag ("left_justify");
			tag.Justification = Justification.Left;
			buffer.TagTable.Add (tag);
			
		}

		 public static void MostrarInfTicket(Gtk.TextBuffer b, Mesa mesa){
            b.Clear();if(b.TagTable.Size==0) CreateTags(b);
		    TextIter i = b.GetIterAtOffset(0);
            AgregarLinea(b,ref i, string.Format("Abre mesa {0}",mesa.camareroAbreMesa));        
            AgregarLinea(b,ref i,string.Format("Mesa {0} ",mesa.mesa),TagNames.peque);
            AgregarLinea(b,ref i, string.Format("Fecha {0}-{1}",mesa.HoraApertura.ToShortDateString(),
                                   mesa.HoraApertura.ToShortTimeString(),TagNames.peque));
            int pos = 1;
            foreach(Ronda r in mesa.RondasActivas){ 
              AgregarLinea(b,ref i, "");
              AgregarLinea(b,ref i,string.Format("Ronda {1} {0}",r.nomCamarero,pos++),TagNames.peque);        
              AgregarLinea(b,ref i,string.Format("Hora {0}",r.horaServido.ToShortTimeString()),TagNames.peque);
              AgregarLinea(b,ref i,"",TagNames.peque);
              AgregarLinea(b,ref i,"Can  Descripcion            Precio   Total",TagNames.peque);
              AgregarLinea(b,ref i,"------------------------------------------",TagNames.peque);

              foreach(DictionaryEntry o in r.lineasArtActivos){
                  Articulo art = (Articulo)o.Value;
                  string preU = String .Format("{0:#0.00}",art.Precio);
                  string cantidad = String.Format("{0:#0.###}",art.Cantidad);
                  string TotalLinea = String.Format("{0:#0.00}",art.TotalLinea);
                  
                  AgregarLinea(b,ref i,String.Format("{0} {1} {2} {3}",
                       cantidad.PadLeft(4),
                         art.Descripcion.PadRight(22),
                               preU.PadLeft(6),
                                 TotalLinea.PadLeft(6)),TagNames.peque);
                }
               
               foreach(DictionaryEntry o in r.lineasArtCobrados){
                  Articulo art = (Articulo)o.Value;
                  
                  string preU = String .Format("{0:#0.00}",art.Precio);
                  string cantidad = String.Format("{0:#0.###}",art.Cantidad);
                  string TotalLinea = String.Format("{0:#0.00}",art.TotalLinea);
                  
                  AgregarLinea(b,ref i,String.Format("{0} {1} {2} {3}",
                       cantidad.PadLeft(4),
                         art.Descripcion.ToString().PadRight(22),
                               preU.PadLeft(6),
                                 TotalLinea.PadLeft(6)),TagNames.derecha,TagNames.peque,TagNames.azul);
                }
                
                foreach(LineaNula lnula in r.nulas){
                  AgregarLinea(b,ref i,"Anula la linea "+lnula.camareroAnula,TagNames.maspeque);
                  Articulo art = lnula.linea;
                  string preU = String .Format("{0:#0.00}",art.Precio);
                  string cantidad = String.Format("{0:#0.###}",art.Cantidad);
                  string TotalLinea = String.Format("{0:#0.00}",art.TotalLinea);
                  
                  AgregarLinea(b,ref i, String.Format("{0} {1} {2} {3}",
                       cantidad.PadLeft(4),
                         art.Descripcion.ToString().PadRight(22),
                               preU.PadLeft(6),
                                 TotalLinea.PadLeft(6)),TagNames.derecha,TagNames.peque,TagNames.rojo);
                }
              } 
             
             foreach(Ronda r in mesa.RondasPagadas){ 
              AgregarLinea(b,ref i, "");
              AgregarLinea(b,ref i,string.Format("Ronda {1} {0}",r.nomCamarero,pos++),TagNames.peque);        
              AgregarLinea(b,ref i,string.Format("Hora {0}",r.horaServido.ToShortTimeString()),TagNames.peque);
              AgregarLinea(b,ref i,"",TagNames.peque);
              AgregarLinea(b,ref i,"Can  Descripcion            Precio   Total",TagNames.peque);
              AgregarLinea(b,ref i,"------------------------------------------",TagNames.peque);

              foreach(DictionaryEntry o in r.lineasArtActivos){
                  Articulo art = (Articulo)o.Value;
                  string preU = String .Format("{0:#0.00}",art.Precio);
                  string cantidad = String.Format("{0:#0.###}",art.Cantidad);
                  string TotalLinea = String.Format("{0:#0.00}",art.TotalLinea);
                  
                  AgregarLinea(b,ref i,String.Format("{0} {1} {2} {3}",
                       cantidad.PadLeft(4),
                         art.Descripcion.ToString().PadRight(22),
                               preU.PadLeft(6),
                                 TotalLinea.PadLeft(6)),TagNames.derecha,TagNames.peque,TagNames.azul);
                }
               
               foreach(DictionaryEntry o in r.lineasArtCobrados){
                  Articulo art = (Articulo)o.Value;
                  string preU = String .Format("{0:#0.00}",art.Precio);
                  string cantidad = String.Format("{0:#0.###}",art.Cantidad);
                  string TotalLinea = String.Format("{0:#0.00}",art.TotalLinea);
                  
                  AgregarLinea(b,ref i,String.Format("{0} {1} {2} {3}",
                       cantidad.PadLeft(4),
                        art.Descripcion.ToString().PadRight(22),
                               preU.PadLeft(6),
                                 TotalLinea.PadLeft(6)),TagNames.derecha,TagNames.peque,TagNames.rojo);
                }
                
                foreach(LineaNula lnula in r.nulas){
                  AgregarLinea(b,ref i,"Anula la linea "+lnula.camareroAnula,TagNames.maspeque);
                  Articulo art = lnula.linea;
                  string preU = String .Format("{0:#0.00}",art.Precio);
                  string cantidad = String.Format("{0:#0.###}",art.Cantidad);
                  string TotalLinea = String.Format("{0:#0.00}",art.TotalLinea);
                  
                  AgregarLinea(b,ref i,String.Format("{0} {1} {2} {3}",
                       cantidad.PadLeft(4),
                         art.Descripcion.ToString().PadRight(22),
                               preU.PadLeft(6),
                                 TotalLinea.PadLeft(6)),TagNames.derecha,TagNames.peque,TagNames.rojo);
                }
              } 
         
        }
        
        public static void MostrarInfTicket(Gtk.TextBuffer b, int numTicket, IGesSql gesL, string camareroCobraTicket){
           DataTable tbLineasRon = gesL.EjecutarSqlSelect("InfLineasRon","Select Mesa, CamareroAbreMesa, HoraInicio, FechaInicio," +
             "HoraFin, FechaFin, CamareroServido, FechaServido, HoraServido, IDRonda, numTicket,nomArticulo, Cantidad,(TotalLinea/Cantidad)AS PrecioU, TotalLinea" +
             "  From LineasRonda INNER JOIN Rondas ON "+
                                                 " LineasRonda.IDRonda = Rondas.IDVinculacion Inner Join GestionMesas On "+
                                                 " Rondas.IDMesa = GestionMesas.IDVinculacion WHERE LineasRonda.numTicket ="+numTicket+
                                                 " ORDER BY LineasRonda.IDRonda" );
         
         int idRonda = 0;
         if(tbLineasRon.Rows.Count>0){
            b.Clear();if(b.TagTable.Size==0) CreateTags(b);
			TextIter i = b.GetIterAtOffset(0);	
            AgregarLinea(b,ref i, string.Format("Abre mesa {0}",tbLineasRon.Rows[0]["CamareroAbreMesa"]));        
            AgregarLinea(b,ref i,string.Format("Mesa {0} ",tbLineasRon.Rows[0]["Mesa"]),TagNames.peque);
            AgregarLinea(b,ref i,string.Format("Fecha {0}-{1}",Utilidades.CadenasTexto.RotarFecha(tbLineasRon.Rows[0]["FechaInicio"].ToString()),
                                    tbLineasRon.Rows[0]["HoraInicio"]),TagNames.peque);
                                    
            idRonda = (int)tbLineasRon.Rows[0]["IDRonda"] ;    
            AgregarLinea(b,ref i,"");
            AgregarLinea(b,ref i,string.Format("Una ronda {0}",tbLineasRon.Rows[0]["CamareroServido"]),TagNames.peque);        
            AgregarLinea(b,ref i,string.Format("Hora {0}",tbLineasRon.Rows[0]["HoraServido"]),TagNames.peque);
            AgregarLinea(b,ref i,"",TagNames.peque);
            AgregarLinea(b,ref i,"Can  Descripcion            Precio   Total",TagNames.peque);
            AgregarLinea(b,ref i,"------------------------------------------",TagNames.peque);

              for(int j = 0;j<tbLineasRon.Rows.Count;j++){
                 if(idRonda != (int)tbLineasRon.Rows[j]["IDRonda"]){
                   idRonda = (int)tbLineasRon.Rows[j]["IDRonda"] ; 
                   AgregarLinea(b,ref i,"",TagNames.peque);
                   AgregarLinea(b,ref i, string.Format("Otra ronda {0}",tbLineasRon.Rows[j]["CamareroServido"]));        
                   AgregarLinea(b,ref i,string.Format("Hora {0}",tbLineasRon.Rows[j]["HoraServido"]),TagNames.peque);
                   AgregarLinea(b,ref i,"",TagNames.peque);   
                   AgregarLinea(b,ref i,"Can  Descripcion            Precio   Total",TagNames.peque);
                   AgregarLinea(b,ref i,"------------------------------------------",TagNames.peque);
   
                  }
                  
                  string preU = String .Format("{0:#0.00}",tbLineasRon.Rows[j]["PrecioU"]);
                  string cantidad = String.Format("{0:#0.###}",tbLineasRon.Rows[j]["Cantidad"]);
                  string TotalLinea = String.Format("{0:#0.00}",tbLineasRon.Rows[j]["TotalLinea"]);
                  
                  AgregarLinea(b,ref i,String.Format("{0} {1} {2} {3}",
                       cantidad.PadLeft(4),
                         tbLineasRon.Rows[j]["nomArticulo"].ToString().PadRight(22),
                               preU.PadLeft(6),
                                 TotalLinea.PadLeft(6)),TagNames.peque);
                }
               AgregarLinea(b,ref i,"",TagNames.mediana);
               AgregarLinea(b,ref i,"Combra mesa "+camareroCobraTicket,TagNames.peque);
               AgregarLinea(b,ref i,string.Format("Fecha {0}-{1}",Utilidades.CadenasTexto.RotarFecha(tbLineasRon.Rows[0]["FechaFin"].ToString()),
                                    tbLineasRon.Rows[0]["HoraFin"]),TagNames.peque);
            
         }
         
        }
        
        public static void MostrarTicket(Gtk.TextBuffer b, IEnumerator lineas,
            String camarero, String mesa, DateTime fechaTicket, int numTicket){
            b.Clear();
			if(b.TagTable.Size==0) CreateTags(b);
			TextIter i =b.GetIterAtOffset(0);
            AgregarLinea(b,ref i, String.Format("Camarero: {0} ",camarero));
            AgregarLinea(b,ref i, "");
            AgregarLinea(b,ref i, String.Format("Mesa: {0} ",mesa));
            AgregarLinea(b,ref i, string.Format("Fecha {0} ",fechaTicket));
            AgregarLinea(b,ref i, string.Format("Num. Ticket {0} ", numTicket ));
            AgregarLinea(b,ref i, "");
            AgregarLinea(b,ref i,"Can  Descripcion            Precio   Total",TagNames.peque);
            AgregarLinea(b,ref i,"------------------------------------------",TagNames.peque);

            while (lineas.MoveNext())
            {
                Articulo articulo = (Articulo)lineas.Current;
                String precioImp = String.Format("{0:#0.00}", articulo.precio);
                String totalImp = String.Format("{0:#0.00}", articulo.TotalLinea);
                AgregarLinea(b,ref i,String.Format("{0:#0.###} {1} {2} {3}",
                   articulo.Cantidad.ToString().PadLeft(4)
                   , articulo.Descripcion.Trim().PadRight(22),
                    precioImp.PadLeft(6),
                         totalImp.PadLeft(6)),TagNames.peque);
            }
        }
        
        public static void mostrarInforme(Gtk.TextBuffer b, string[] informe){
            b.Clear();
			if(b.TagTable.Size==0) CreateTags(b);
			TextIter i = b.StartIter;
            foreach(string s in informe){
               AgregarLinea(b,ref i, s, TagNames.peque);
            }
        }
		
       public static void AgregarLinea(Gtk.TextBuffer b,ref TextIter iterInser, string texto, string aling, string dim, string color){
             b.InsertWithTagsByName(ref iterInser,texto+'\n',new String[]{TagNames.monospace,aling,dim,color});
	    }
       
	   
       public static void AgregarLinea(Gtk.TextBuffer b,ref TextIter iter, string strTexto){
           b.InsertWithTagsByName(ref iter, strTexto+"\n",new String[]{TagNames.monospace});
	   }
       
       public static void AgregarLinea(Gtk.TextBuffer b,ref TextIter iterInser, string texto,string dim){
            b.InsertWithTagsByName(ref iterInser,texto+'\n',new String[]{TagNames.monospace,dim});
	 }
	}
}

