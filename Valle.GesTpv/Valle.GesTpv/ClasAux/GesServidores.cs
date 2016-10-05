using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Data;


namespace Valle.GesTpv
{
    public class GesServidores
    {
        
	   public string FICH_TB_SERVIDORES = Rutas.Ruta_Directa("/"+Rutas.DATOS+"/servidores.datos");
	   public string NOM_CONJUNTO_DATOS = "gesServidores";

	   public const string NOM_TB_SERVIDORES = "servidores";
	   public const string NOM_TB_ACTIVO = "activo";
	   
	   public const string IDSERVIDOR = "IDServidor";
	   public const string NOMBRE= "Nombre";
	   public const string IP="Ip";
	   public const string PUERTO="Puerto";
	   public const string PUERTO_SOCKT = "PortSockt";
	   public const string PROTOCOLO="Protocolo";
    	
       private DataSet datos;
	   
		
		
		public GesServidores(){
           try{
			datos = new DataSet(NOM_CONJUNTO_DATOS);
			 datos.ReadXml(FICH_TB_SERVIDORES);
		     }catch (FileNotFoundException){
				CrearTablasServidores();
     		 }
		}
 		
		private void CrearTablasServidores()
		{
			
			//Tabla principal con el nombre de servidor y su configuracion
		     DataTable tb = new DataTable(NOM_TB_SERVIDORES);
			 DataColumn clID = new DataColumn("IDServidor");
			 clID.DataType = Type.GetType("System.Int32");
			 clID.AutoIncrement = true;
			 DataColumn clNombre = new DataColumn("Nombre");
			 clNombre.DataType= Type.GetType("System.String");
			 DataColumn clIp = new DataColumn("Ip");
			 clIp.DataType = Type.GetType("System.String");
			 DataColumn clPuerto = new DataColumn("Puerto");
			 clPuerto.DataType = Type.GetType("System.Int32");
			 DataColumn clPuertoSock = new DataColumn("PortSockt");
			 clPuertoSock.DataType = Type.GetType("System.Int32");
			 
			 DataColumn clProtocolo = new DataColumn("Protocolo");
			 clProtocolo.DataType = Type.GetType("System.String");
			 DataColumn clExportar = new DataColumn("ExportarComun");
			 clExportar.DataType = Type.GetType("System.Boolean");
			 tb.Columns.AddRange(new DataColumn[]{clID,clNombre,clIp,clPuerto,clPuertoSock,clProtocolo,clExportar});
			 tb.Constraints.Add("KeyIdServidor",tb.Columns["IDServidor"],true);
		
			 //Tabla con el id del servidor activado
			 DataColumn clIDActivo = new DataColumn("IDServidor");
			 clIDActivo.DataType = Type.GetType("System.Int32");
			 DataTable tbAct = new DataTable(NOM_TB_ACTIVO);
     		 tbAct.Columns.Add(clIDActivo);
			 tbAct.Constraints.Add("servidorActivo",tb.Columns["IDServidor"],tbAct.Columns["IDServidor"]);
			 datos.Tables.AddRange(new DataTable[]{tb,tbAct});
             GuardarDatos();
			
		}
		
		public void SeHanExportadoComunes(){
		    foreach(DataRow dr in datos.Tables[NOM_TB_SERVIDORES].Rows){
        		      if(!dr.Equals(this.ServidorActivo)){
        		        dr["ExportarComun"] =  true;
        		      }
        		   }
        	this.GuardarDatos();
		
		}
		
		public DataTable TbServidores{
			 get{ return datos.Tables[NOM_TB_SERVIDORES];}
		}
		
		public DataRow RegistroNuevo(){
			return datos.Tables[NOM_TB_SERVIDORES].NewRow();
		}
		
		public void AñadirRegistro(DataRow regServidor){
			datos.Tables[NOM_TB_SERVIDORES].Rows.Add(regServidor);
			datos.AcceptChanges();
		    GuardarDatos();
		}

		public  DataRow ServidorActivo{
			get{
    			try{
    		    DataRow[] drs = TbServidores.Select("IDServidor ="+
    			                         datos.Tables[NOM_TB_ACTIVO].Rows[0][0]);
    			    if(drs.Length>0)                     
    		        		return drs[0];
    		        		else
    		        		return null;
    			}catch{
    				 return null;                                                                  
    			}
			}
		}
		
        public void GuardarDatos(){
		 datos.AcceptChanges();
     	 datos.WriteXml(FICH_TB_SERVIDORES,XmlWriteMode.WriteSchema);
		}
		
		public void ActivarServidor(int id){
          DataTable activo =  datos.Tables[NOM_TB_ACTIVO] ;
          if(id>=0){
			if(activo.Rows.Count>0){
		     	activo.Rows[0][GesServidores.IDSERVIDOR]= id;
			}else{
				DataRow dr = activo.NewRow();
				dr[GesServidores.IDSERVIDOR] = id;
				activo.Rows.Add(dr);
			}
		  }else{
		    activo.Rows.Clear();
		  }
		  
			GuardarDatos();
		}
		
       
    }
}
