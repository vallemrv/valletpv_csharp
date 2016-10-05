using System.Net;
using System;

namespace updates
{


	public class Update
	{
		System.Timers.Timer cm = new System.Timers.Timer(6000);
		
	    System.Threading.Thread h = null;
		string fileActualizacion = Valle.Utilidades.RutasArchivos.Ruta_Completa("actualizaciones.xml");		
			
		
		public Update ()
		{
			cm.Elapsed+= delegate {
			 	if((h==null)||(!h.IsAlive)){
				   h = new System.Threading.Thread(new System.Threading.ThreadStart(CompruebaAct));
				   h.Start();
				}	
			};
			
			cm.Start();
			
		}
		
		
		void CompruebaAct(){
			string id = this.getIdAct();
			WebClient request = new WebClient();
			string xml = request.DownloadString(new Uri("http://www.valleapp.com/updateTpv/gesupdates.ashx?accion=ultimaAct&Id="+id));
			if(hayAct(xml)) Actualizar(xml);                                    
		}
		
		bool hayAct(string xml){
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			doc.LoadXml(xml);
			return doc.DocumentElement.ChildNodes.Count > 0;
		}
		
		
		string getIdAct(){
			
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			System.Xml.XmlElement act = null;
			string id = "";
			
		    if(System.IO.File.Exists(fileActualizacion)){
						       doc.Load(fileActualizacion);	
				               if(doc.DocumentElement.ChildNodes.Count>0){
							        act = (System.Xml.XmlElement)doc.DocumentElement.FirstChild;
					                id = act.GetAttribute("id");
				               }
						}
			return id;
		}
		
		void Actualizar(string xml){
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			doc.LoadXml(xml);
			
			System.Xml.XmlElement act = (System.Xml.XmlElement) doc.DocumentElement.FirstChild;
			 String nombre = act.GetAttribute("nombre");
			
			WebClient request = new WebClient();
			Byte[] fichero = request.DownloadData(new Uri("http://www.valleapp.com/updateTpv/updates/"+nombre));
			Console.WriteLine(fichero.Length);
			 
		}
		
	}
}
