using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Gtk;

namespace Valle.GesTpv
{
    class GeneradorTeclados
    {
        PaginasArticulos pagArt;
        List<DatosTecla> Articulos;
        GesBaseLocal gesLocal;
        DataTable tbArticulos;
        DataTable tbColores;
        DataTable tbSecciones;
        DataTable tbTeclas;
        DataTable tbFavoritos;
        DataTable tbPrincipal;
        
        public Dictionary<string,PaginasArticulos> ListaDeTeclados = new Dictionary<string,PaginasArticulos>();
       
        public GeneradorTeclados(GesBaseLocal gesl,DataTable valTabla)
        {
          gesLocal = gesl;
          tbPrincipal =  valTabla; 
          tbColores = gesLocal.ExtraerLaTabla("Colores");
          tbSecciones = gesLocal.ExtraerLaTabla("Secciones");
          tbArticulos = gesLocal.ExtraerLaTabla("Articulos");
          //para generar los teclados de favoritos
           if(tbPrincipal.TableName.Equals("TeclasFav")){
                tbTeclas = gesLocal.ExtraerLaTabla("Teclas");
                tbFavoritos = gesLocal.ExtraerLaTabla("Favoritos");
                foreach(DataRow dr in tbFavoritos.Rows){
                   this.ListaDeTeclados.Add(dr["Nombre"].ToString(),this.AñadirTeclas(dr["IDFavoritos"].ToString()));
                  }
                }else{
                //para generar los teclados por secciones
                  foreach(DataRow dr in tbSecciones.Rows){
                     this.ListaDeTeclados.Add(dr["Nombre"].ToString(), this.AñadirTeclas(dr["IDSeccion"].ToString()));
                  }
                }
                 
           
        }
        
        public void QuitarTecla(DatosTecla dt, PaginasArticulos pagArt){
           pagArt.ListaTeclas.Remove(dt);
           pagArt.PaginarAriculos();
        }
        
        public void AñadirTeclas(DataRow dr,PaginasArticulos pagArt){
             if(tbPrincipal.TableName.Equals("Teclas")){
                  System.Drawing.Color miColor = System.Drawing.Color.Gray;
                  string idColor = tbSecciones.Select
                                 ("IDSeccion = "+dr["IDSeccion"].ToString())[0]["IDColor"].ToString();
                  if(idColor.Length>0){
                         DataRow colorDeAtras = tbColores.Select("IDColor = "+ idColor)[0];
              
                          miColor = System.Drawing.Color.FromArgb((int)colorDeAtras["Rojo"],
                              (int)colorDeAtras["Verde"],(int)colorDeAtras["Azul"]);
        		        
                      }
                        
                  pagArt.ListaTeclas.Add(this.crearDatosArt(miColor,dr));
             }else{
                  pagArt.ListaTeclas.Add(this.crearDatosArtFav(dr));
             }
        }
        
        public PaginasArticulos AñadirTeclas(string ID){
            if(tbPrincipal.TableName.Equals("Teclas")){
                  
                DataRow[] rsTeclas= tbPrincipal.Select("IDSeccion = "+ID,"orden");
                 Articulos = new List<DatosTecla>();
                       crearArticulos(tbSecciones.Select
                                 ("IDSeccion = "+ ID)[0]["IDColor"].ToString(), rsTeclas);
                          pagArt = new PaginasArticulos(32,Articulos);
                 return pagArt;
             }else{
               DataRow[] rsTeclas = tbPrincipal.Select("IDFavoritos = "+ID,"orden");
                       Articulos = new List<DatosTecla>();
                       crearArticulosFav(rsTeclas);
                       pagArt = new PaginasArticulos(32,Articulos);
                return pagArt;
             }
        }
        
        private void crearArticulosFav(DataRow[] dr)
        {
              
            for (int i = 0; i < dr.Length; i++)
            {
              Articulos.Add(this.crearDatosArtFav(dr[i]));
            }
        }
        
        private void crearArticulos(string idColor, DataRow[] dr)
        {
        
           System.Drawing.Color miColor = System.Drawing.Color.Gray;
            if(idColor.Length>0){
                   DataRow colorDeAtras = tbColores.Select("IDColor = "+idColor)[0];
              
                          miColor = System.Drawing.Color.FromArgb((int)colorDeAtras["Rojo"],
                              (int)colorDeAtras["Verde"],(int)colorDeAtras["Azul"]);
        		        
                      }
          
            for (int i = 0; i < dr.Length; i++)
            {
              Articulos.Add(this.crearDatosArt(miColor,dr[i]));
            }
        }
        
        DatosTecla crearDatosArtFav(DataRow dr){
                 
        
              DataRow drTecla = tbTeclas.Select("IDTecla = " +dr["IDTecla"].ToString())[0];
              DataRow drSeccion = tbSecciones.Select("IDSeccion = "+ drTecla["IDSeccion"].ToString())[0];
              System.Drawing.Color miColor = System.Drawing.Color.Gray;
              string idColor = tbSecciones.Select
                                 ("IDSeccion = "+drSeccion["IDSeccion"].ToString())[0]["IDColor"].ToString();
                  if(idColor.Length>0){
                         DataRow colorDeAtras = tbColores.Select("IDColor = "+ idColor)[0];
                          miColor = System.Drawing.Color.FromArgb((int)colorDeAtras["Rojo"],
                              (int)colorDeAtras["Verde"],(int)colorDeAtras["Azul"]);
        		        
                      }
                  
              
                DataRow drArticulo = tbArticulos.Select("IDArticulo = '" + drTecla["IDArticulo"].ToString()+"'")[0];
                DatosTecla dT  = new DatosTecla();
                dT.nombreCorto = drTecla["Nombre"].ToString(); 
                dT.nombreLargo = drArticulo["Nombre"].ToString();
                dT.IDArticulo = drArticulo["IDArticulo"].ToString();
                dT.IDTecla = (int)drTecla["IDTecla"];
                dT.nombreSeccion = drSeccion["Nombre"].ToString();
                dT.Orden = (int)dr["orden"];
                dT.colorDeAtras = miColor; 
		        dT.DR = dr;
		        return dT;
        }
        
        DatosTecla crearDatosArt(System.Drawing.Color colorDeAtras,DataRow dr){
            DataRow drArticulo = tbArticulos.Select("IDArticulo = '" + dr["IDArticulo"].ToString()+"'")[0];
                DatosTecla dT  = new DatosTecla();
                dT.nombreCorto = dr["Nombre"].ToString(); 
                dT.nombreLargo = drArticulo["Nombre"].ToString();
                dT.IDArticulo = drArticulo["IDArticulo"].ToString();
                dT.IDTecla = (int)dr["IDTecla"];
                dT.Orden = (int)dr["orden"];
                dT.colorDeAtras = colorDeAtras; 
		        dT.DR = dr;
		        return dT;
        }
        
       
    }
}
