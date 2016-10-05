using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using System.Drawing;

using Valle.SqlGestion;
using Valle.Utilidades;

namespace Valle.ToolsTpv
{
    public class GeneradorDeArticulos
    {
       
        DateTime[] horaInicioFav; 
		int numArtPag = 0;
		string[] idsFavs =null;
        Dictionary<string,Articulo> todosLosArt = new Dictionary<string, Articulo>();
		Dictionary<string,PaginasObj<Articulo>> pagsArt = new Dictionary<string, PaginasObj<Articulo>>();
		List<InfBoton> secciones = new List<InfBoton>();
		DataTable tbTodasLasTeclas;

        
        public GeneradorDeArticulos(int idTpv, int numTeclasPagina, int prog,
                            IGesSql gesLocal, ISplash splash)
        {
        	
             numArtPag = numTeclasPagina;
			
            #region cargar tablas teclados tpv
            //cargamos teclas del tpv
           splash.mostrarProgreso(prog+=5);
			
			tbTodasLasTeclas = gesLocal.EjecutarSqlSelect("Teclas", "SELECT  "+
               "Teclas.Nombre,  Articulos.IDArticulo, Teclas.Orden" +
               " FROM Articulos INNER JOIN" +
               " Teclas ON Articulos.IDArticulo = Teclas.IDArticulo" +
               " INNER JOIN SeccionesTpv ON Teclas.IDSeccion = SeccionesTpv.IDSeccion "+ 
               " WHERE (SeccionesTpv.IDTpv = "+ idTpv.ToString()+") ORDER BY Teclas.Orden");
          
           DataSet vDs = new DataSet("TecladosTpv");
			
           DataTable tb = gesLocal.EjecutarSqlSelect("Teclas", "SELECT  Articulos.Nombre AS NombreArt," +
               "Teclas.Nombre, Articulos.Precio1, Articulos.Precio2, Articulos.Precio3, Articulos.IDArticulo," +
               " Teclas.IDSeccion, Teclas.IDTecla, Teclas.Orden FROM Articulos INNER JOIN" +
               " Teclas ON Articulos.IDArticulo = Teclas.IDArticulo" +
               " INNER JOIN SeccionesTpv ON Teclas.IDSeccion = SeccionesTpv.IDSeccion "+ 
               " WHERE (SeccionesTpv.IDTpv = "+ idTpv.ToString()+") ORDER BY Teclas.Orden");
			
           vDs.Tables.Add(tb);

            //cargamos las secciones del tpv
            splash.mostrarProgreso(prog+=5);
          
            tb = gesLocal.EjecutarSqlSelect("SeccionesTpv","SELECT Secciones.IDSeccion, Secciones.Nombre, Colores.Rojo, Colores.Verde, Colores.Azul " +
                                        " FROM  Secciones LEFT OUTER JOIN Colores ON Secciones.IDColor = Colores.IDColor "+
                                        " INNER JOIN SeccionesTpv ON Secciones.IDSeccion = SeccionesTpv.IDSeccion " +
                                        " WHERE (SeccionesTpv.IDTpv = "+ idTpv.ToString() +")");
           vDs.Tables.Add(tb);

            //Cargamos los teclas de los favoritos del tpv
              splash.mostrarProgreso(prog+=5);
          
              tb = gesLocal.EjecutarSqlSelect( "TeclasFav", "SELECT  Articulos.Nombre AS NombreArt," +
              "Teclas.Nombre, Articulos.Precio1, Articulos.Precio2, Articulos.Precio3, Articulos.IDArticulo," +
              " Teclas.IDSeccion,TeclasFav.IDFavoritos, TeclasFav.IDTecla, TeclasFav.Orden FROM Articulos INNER JOIN" +
              " Teclas ON Articulos.IDArticulo = Teclas.IDArticulo" +
              " INNER JOIN TeclasFav ON Teclas.IDTecla = TeclasFav.IDTecla INNER JOIN FavoritosTpv "+
              " ON TeclasFav.IDFavoritos = FavoritosTpv.IDFavoritos " +
              " WHERE (FavoritosTpv.IDTpv = " + idTpv.ToString() + ") ORDER BY TeclasFav.Orden");
         
            vDs.Tables.Add(tb);
            //cargamos las Favoritos del tpv
            splash.mostrarProgreso(prog+=5);
          
            tb = gesLocal.EjecutarSqlSelect("FavoritosTpv","SELECT FavoritosTpv.IDFavoritos, FavoritosTpv.HoraInicioFav "+
                                        " FROM  FavoritosTpv "+
                                        " WHERE (FavoritosTpv.IDTpv = " + idTpv.ToString() + ")");
            vDs.Tables.Add(tb);
            tb= gesLocal.EjecutarSqlSelect("VentaPorKilos", "SELECT * FROM VentaPorKilos");
            vDs.Tables.Add(tb);
            #endregion cargar tablas teclados tpv

            DataTable tbSeccion = vDs.Tables["SeccionesTpv"];
            DataTable tbTeclas = vDs.Tables["Teclas"];
            DataTable tbTeclasFav = vDs.Tables["TeclasFav"];
            DataTable tbFavoritosTpv = vDs.Tables["FavoritosTpv"];
            
            
          
            #region crear articulos clases
			
		    
            //Crear los distitos favoritos
           
            idsFavs = new string[tbFavoritosTpv.Rows.Count];
            horaInicioFav = new DateTime[tbFavoritosTpv.Rows.Count];
            int  posID = 0;
            foreach (DataRow rFav in tbFavoritosTpv.Rows){
              DataRow[] drsFav =tbTeclasFav.Select("IDFavoritos ="+ rFav["IDFavoritos"].ToString());
                if (drsFav.Length > 0)
                   {
                      Articulo[] articulos = new Articulo[drsFav.Length];
                      crearArticulosFav(articulos, tbSeccion,tbTeclas, drsFav,vDs.Tables["VentaPorKilos"]);
                      
					  
                       pagsArt.Add(rFav["IDFavoritos"].ToString(), new PaginasObj<Articulo>(numTeclasPagina, articulos));
                       horaInicioFav[posID] = DateTime.Parse(rFav["HoraInicioFav"].ToString());
                       idsFavs[posID] = rFav["IDFavoritos"].ToString();posID++;
                   }
            }
			
             
			splash.mostrarProgreso(prog+=5);
          
           
            //creamos las clase de articulos con las secciones
           
            foreach (DataRow r in tbSeccion.Rows)
            {
              DataRow[]   drsTeclas = tbTeclas.Select("IDSeccion = " + r["IDSeccion"].ToString());
                if (drsTeclas.Length > 0)
                   {
                       //para recoger el color de los botones
                  
                        Color cl = Color.FromArgb((!r["Rojo"].GetType().Name.Equals("DBNull"))?
                                                 (int)r["Rojo"] : Convert.ToInt32(Color.Gray.R), (!r["Verde"].GetType().Name.Equals("DBNull")) ?
                                                 (int)r["Verde"] : Convert.ToInt32(Color.Gray.G), (!r["Azul"].GetType().Name.Equals("DBNull")) ?
                                                 (int)r["Azul"] : Convert.ToInt32(Color.Gray.B));
                        
              			InfBoton b = new InfBoton();
					     b.ColorDeAtras = cl;
					     b.Texto = r["Nombre"].ToString();
					     b.Orden = tbSeccion.Rows.IndexOf(r);
					     b.Tamaño = new Size(100,100);
                       secciones.Add(b);
		
                       Articulo[] articulos = new Articulo[drsTeclas.Length];
                       crearArticulos(articulos, cl, drsTeclas,vDs.Tables["VentaPorKilos"]);

                       
                        pagsArt.Add(r["Nombre"].ToString(), new PaginasObj<Articulo>(numTeclasPagina, articulos));
                      
                   }
                   
               }
               splash.mostrarProgreso(prog+=5);

            #endregion 
              vDs.Dispose(); vDs = null;
			 
           }
       

       
        private void crearArticulos(Articulo[] articulos, Color colorDeAtras,  DataRow[] dr,DataTable tbVentaK)
        {
            for (int i = 0; i < dr.Length; i++)
            {
                articulos[i] = new Articulo();
                articulos[i].Descripcion = dr[i]["NombreArt"].ToString().Trim();
                articulos[i].NombreCorto = dr[i]["Nombre"].ToString().Trim();
                articulos[i].Precio1 = (decimal)dr[i]["Precio1"];
                articulos[i].Precio2 = (decimal)dr[i]["Precio2"];
                articulos[i].Precio3 = (decimal)dr[i]["Precio3"];
                articulos[i].IDArticulo = dr[i]["IDArticulo"].ToString();
                articulos[i].Orden = (int)dr[i]["Orden"];
                articulos[i].MiColor = colorDeAtras;
                articulos[i].VentaPorKilos = tbVentaK.Select(
                                "IDArticulo = " + dr[i]["IDArticulo"].ToString()).Length > 0;
                
                if (!todosLosArt.ContainsKey(articulos[i].IDArticulo)) 
                {
                    todosLosArt.Add(articulos[i].IDArticulo, articulos[i]);
                }
            }
        }
		
        private void crearArticulosFav(Articulo[] articulos, DataTable tbSecc,DataTable tbTeclas, DataRow[] dr,DataTable tbVentaK)
        {

            for (int i = 0; i < dr.Length; i++)
            {
                articulos[i] = new Articulo();
                //para recoger el color de los botones
                DataRow rSecc = tbSecc.Select("IDSeccion = " + tbTeclas.Select("IDTecla =" +
                                         dr[i]["IDTecla"].ToString())[0]["IDSeccion"].ToString())[0];
                Color colorDeAtras = Color.FromArgb((!rSecc["Rojo"].GetType().Name.Equals("DBNull")) ?
            (int)rSecc["Rojo"] : Convert.ToInt32(Color.Gray.R), (!rSecc["Verde"].GetType().Name.Equals("DBNull")) ?
            (int)rSecc["Verde"] : Convert.ToInt32(Color.Gray.G), (!rSecc["Azul"].GetType().Name.Equals("DBNull")) ?
            (int)rSecc["Azul"] : Convert.ToInt32(Color.Gray.B));
                

                articulos[i].Descripcion = dr[i]["NombreArt"].ToString().Trim();
                articulos[i].NombreCorto = dr[i]["Nombre"].ToString().Trim();
                articulos[i].Precio1 = (decimal)dr[i]["Precio1"];
                articulos[i].Precio2 = (decimal)dr[i]["Precio2"];
                articulos[i].Precio3 = (decimal)dr[i]["Precio3"];
                articulos[i].IDArticulo = dr[i]["IDArticulo"].ToString();
                articulos[i].Orden = (int)dr[i]["Orden"];
                articulos[i].MiColor = colorDeAtras;
                articulos[i].VentaPorKilos = tbVentaK.Select(
                              "IDArticulo = " + dr[i]["IDArticulo"].ToString()).Length > 0;
              

            }
        }
       
       public PaginasObj<Articulo> getPag(string key){
			 return pagsArt[key];	
		}
		
	   public PaginasObj<IInfBoton> BtnsSecciones{
			get{
				return new PaginasObj<IInfBoton>(11,secciones.ToArray());
	       }
		}
	
		
		
		public PaginasObj<Articulo> PagFav{
			 get{
			  return pagsArt[CargarIDPrometedor.CargarIDFav(idsFavs,this.horaInicioFav).ToString()];	
			}
		}
		
		public PaginasObj<Articulo> BuscarArt(string cadenaBus){
		        List<Articulo> arts = new List<Articulo>();
                DataView dv = new DataView(this.tbTodasLasTeclas,"Nombre LIKE '" + cadenaBus + "*'","Orden", DataViewRowState.CurrentRows);
                    foreach (DataRowView rArt in dv)
                    {
                    	arts.Add(this.todosLosArt[rArt["IDArticulo"].ToString()]);
                    }
                    
			        if (arts.Count > 0)
                    {
                         return new PaginasObj<Articulo>(numArtPag, arts.ToArray());
                    }                                              
		                                              
		      return null;                                        
		}
    }
}
