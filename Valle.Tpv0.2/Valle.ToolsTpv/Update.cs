using System.Net;
using System;

namespace Valle.ToolsTpv
{


	public class Update
	{
		System.Timers.Timer cm = new System.Timers.Timer(60000);
		
	    System.Threading.Thread h = null;
		string fileActualizacion = Valle.Utilidades.RutasArchivos.Ruta_Completa("/act.config");		
		string usr = "";
		string pass = "";
		string port = "";
		string dirAct = "";
		
		public Update (string pass, string port, string usr, string url)
		{
			
			this.usr = usr;
			this.pass = pass;
			this.port = port;
			this.dirAct = url;
			
			cm.Elapsed+= delegate {
			 	if((h==null)||(!h.IsAlive)){
				   h = new System.Threading.Thread(new System.Threading.ThreadStart(CompruebaAct));
				   h.Start();cm.Stop();
				}	
			};
			
			cm.Start();
			
		}
		
		
		void CompruebaAct(){ 
			try{
				string id = this.getIdAct();
				WebClient request = new WebClient();
				string xml = request.DownloadString(dirAct.EndsWith("/")?dirAct:dirAct+"/"+"gesupdates.ashx?accion=ultimaAct&Id="+id);
				if(hayAct(xml)) Actualizar(xml);  
				else cm.Start();
				
			}catch{
			 	cm.Start();
			}
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
					                id = act.GetAttribute("Id");
				               }
						}
			return id;
		}
		
		public void Parar(){
		 cm.Stop();	
		}
		
		public void Arrancar(){
		   cm.Start();	
		}
		
		void Actualizar(string xml){
			
			string ficheroAct = Valle.Utilidades.RutasArchivos.Ruta_Completa("/uptate.tar.gz");
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
			doc.LoadXml(xml);
			
			System.Xml.XmlElement act = (System.Xml.XmlElement) doc.DocumentElement.FirstChild;
			 String nombre = act.GetAttribute("fichero");
			 bool script =  Boolean.Parse(act.GetAttribute("script"));
			 bool sql = bool.Parse(act.GetAttribute("mysql"));
			
			try{
			
			WebClient request = new WebClient();
			string url = dirAct.EndsWith("/")?dirAct:dirAct+"/"+"Download.ashx?fichero="+nombre;
			request.DownloadFile(url,ficheroAct);
		    
		    System.Diagnostics.Process tar = new System.Diagnostics.Process();
			tar.StartInfo.FileName = "tar";
			tar.StartInfo.Arguments = "xzvf "+ ficheroAct;
			tar.Start();
			tar.WaitForExit();
			
			System.IO.File.Delete(ficheroAct);
			
			if(sql){
				
				string dirSql = Valle.Utilidades.RutasArchivos.Ruta_Completa("/sql");
				string[] files = System.IO.Directory.GetFiles(dirSql);
				 Valle.SqlGestion.GesMySQL ges = new Valle.SqlGestion.GesMySQL(pass,usr,
						                                                              port);
				foreach(string f in files){
				   if(f.Contains(".sql")) {
					    ges.EjConsultaNoSelect("",new System.IO.StreamReader(dirSql+"/"+f).ReadToEnd());
					}	
				}
				System.IO.Directory.Delete(dirSql);   
			}
				 
				                         
				
			if(script){
				 System.Diagnostics.Process s = new System.Diagnostics.Process();
				 s.StartInfo.FileName = "script";
				 s.Start();
				 s.WaitForExit();
				 System.IO.File.Delete("script");
            }
			}catch{
				
			}finally{
				doc.Save(fileActualizacion);
				cm.Start();
			}
			         
		}
		
	}
}
