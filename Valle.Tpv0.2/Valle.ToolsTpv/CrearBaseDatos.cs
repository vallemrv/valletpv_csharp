   /*
 * Creado por SharpDevelop.
 * Usuario: valle
 * Fecha: 24/08/2009
 * Hora: 23:47
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Valle.SqlGestion;
using Valle.SqlUtilidades;

namespace Valle.ToolsTpv
{
	/// <summary>
	/// Description of CrearBaseDatos.
	/// </summary>
	public class CrearBaseDatos
	{
		 //toda las tablas de al base de datos ordenadas segun claves externas para la correcta inportacion y exportacion                    

	   public String[] listaTodasLasTablas = new string[]{"Familias","Colores","Controles","Favoritos",
                                    "Secciones","Articulos", "Teclas","VentaPorKilos","ArticuloNoVenta",
                  "DesgloseArt","TPVs","Camareros", "InstComision",  "Configuracion","Rutas", "Zonas","Mesas",
                  "ZonasTpv","FavoritosTpv","TeclasFav","SeccionesTpv","Ticket", "CierreDeCaja", "ResComision", "LineasTicket",
                  "Usuarios","Privilegios","SuperUsuario", "Almacen", "Inventarios",
                  "GestionMesas","Rondas", "LineasNulas", "LineasRonda","Sincronizados"};
		 
		string nomBaseDatos = "";
		public CrearBaseDatos(IGesSql gestorSql, string nomBase, string dirSquemas)
		{
			if(gestorSql is GesMySQL){
				nomBaseDatos = nomBase;
				GesMySQL gesL = (GesMySQL)gestorSql;
				gesL.CrearBaseDatos(nomBaseDatos);
				Console.WriteLine(dirSquemas);
				FileStream f = new FileStream(dirSquemas,FileMode.Open,FileAccess.Read);
				Console.WriteLine(dirSquemas);
				
				BinaryFormatter biForm = new BinaryFormatter();
				InfEsquemas infEsq = (InfEsquemas)biForm.Deserialize(f);
				f.Close();
				foreach(string tabla in listaTodasLasTablas){
					CrearTabla(tabla,infEsq.ExtraerExquema(tabla),gesL);
				}
				
			}
		}
		
		 void CrearTabla(string tabla, Esquema esq, GesMySQL gesLocal){

	                       foreach (ClaveExt cl in esq.clavesExt){
	                           cl.BaseDatos = nomBaseDatos;
	                        }
	                        string strCrearTabla = esq.StrCrearTabla().Replace("money","decimal");
	                        strCrearTabla = strCrearTabla.Replace("bit","TINYINT(1)");
	                        strCrearTabla = strCrearTabla.Replace(",,",",");
			                strCrearTabla = strCrearTabla + " TYPE = InnoDB";
			                Console.WriteLine(strCrearTabla);
			                gesLocal.EjConsultaNoSelect(tabla,strCrearTabla,nomBaseDatos);
                          }
	        
	}
}
