// GesPlantillasTb.cs created with MonoDevelop
// User: valle at 21:41 24/08/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Valle.Distribuido.SQLRemoting;
using Valle.SqlUtilidades;
using Valle.GtkUtilidades;

namespace Valle.GesTpv
{
	
	
	public class GesPlantillasTb
	{
		SQLClient gesRemoto;
		Splash sp; 
		string nomPlantilla = "";
	
		public GesPlantillasTb(SQLClient gesR)
		{
			this.gesRemoto = gesR;
		}
		
		
		public void CrearPlantilla(string nomPlan){
			nomPlantilla = nomPlan;
			sp = new Splash("Creando Plantilla "+nomPlan, Rutas.Ruta_Directa(Rutas.IMG_APP+Path.DirectorySeparatorChar+"comunicacion.gif"),true);
			sp.Show();
	        Thread h = new Thread(new ThreadStart(this.HCrearPlantilla));
			h.Start();

		}
		
	    private void HCrearPlantilla(){
			InfEsquemas infEsq = new InfEsquemas();
			string[] ListaTablas = gesRemoto.NombreTablas;
		    double procesados = 0;
			double total = ListaTablas.Length;
			 foreach (string tabla in ListaTablas){
				sp.MostrarInformacion(new InfAMostrar(procesados++/total, TipoInfMostrar.progresoBarr));
                sp.MostrarInformacion(new InfAMostrar("Recopilando infomramcion ...  ",TipoInfMostrar.MensajeBarra));
             	infEsq.AgregarEsquema(gesRemoto.ExtraerSqlCreateTable(tabla));
			  }
			BinaryFormatter BiForm = new BinaryFormatter();
			FileStream f = new FileStream(Rutas.Ruta_Directa(Rutas.DATOS+Path.DirectorySeparatorChar+
			                               this.nomPlantilla+".esq"),FileMode.OpenOrCreate,FileAccess.Write);
			BiForm.Serialize(f,infEsq);
			f.Close();
			sp.Destroy();
		}
		
		/*public void CargarPlantilla(string nomPlant){
			nomPlantilla = nomPlan;
			sp = new Splash("Creando Plantilla "+nomPlan, Rutas.Ruta_Directa(Rutas.IMG_APP+Path.DirectorySeparatorChar+"comunicacion.gif"),true);
			sp.Show();
	        Thread h = new Thread(new ThreadStart(this.HCargarPlantilla));
			h.Start();

		}
	    
     	private void HCargarPlantilla(){
			BinaryFormatter BiForm = new BinaryFormatter();
			FileStream f = new FileStream(Rutas.Ruta_Directa(Rutas.DATOS+Path.DirectorySeparatorChar+
			                               this.nomPlantilla+".esq"),FileMode.OpenOrCreate,FileAccess.Write);
			
			InfEsquemas infEsq = (InfEsquemas)BiForm.Deserialize(f);
			f.Close();
			
			double procesados = 0;
			double total = ListaTablas.Length;
			
			foreach (Esquema sq in infEsq.ListaTablas){
				sp.MostrarInformacion(new InfAMostrar(procesados++/total, TipoInfMostrar.progresoBarr));
                sp.MostrarInformacion(new InfAMostrar("Recopilando infomramcion ...  ",TipoInfMostrar.MensajeBarra));
             	infEsq.AgregarEsquema(gesRemoto.ExtraerSqlCreateTable(tabla));
            }
			
			sp.Destroy();
		}*/

		    
		
	}
}
