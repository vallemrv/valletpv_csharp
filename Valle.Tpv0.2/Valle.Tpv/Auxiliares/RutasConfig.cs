using System;
using System .IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;

using Valle.Utilidades;

namespace Valle.TpvFinal.Tools
{
	
   public delegate void OnSalirCrearConf();
   public class CrearConfiguracionIni{
		
		event OnSalirCrearConf salirConfig;
		DatosConfIni datosConfIni = new DatosConfIni();
		
	   public  CrearConfiguracionIni(OnSalirCrearConf salirConfig)
        {
			
	
			this.salirConfig = salirConfig;
			
			datosConfIni.PathMesas = RutasArchivos.Ruta_Completa(RutasConfig.NOM_DIR_MESAS);
            datosConfIni.PathFotos = RutasArchivos.Ruta_Completa(RutasConfig.NOM_DIR_FOTOS);
            datosConfIni.PathPlaning = RutasArchivos.Ruta_Completa(RutasConfig.NOM_DIR_PLANOS);
            datosConfIni.PathDatos = RutasArchivos.Ruta_Completa(RutasConfig.NOM_DIR_DATOS);
            datosConfIni.ipServidor = System.Net.IPAddress.Any.ToString();
            datosConfIni.puertoComunicacion = "8000";
            datosConfIni.puertoDatos = "8001";
            datosConfIni.protocolo = "TCP";
            datosConfIni.esAuxiliar = false;
		
			    if (!Directory.Exists(datosConfIni.PathMesas))  Directory.CreateDirectory(datosConfIni.PathMesas); 
                if (!Directory.Exists(datosConfIni.PathFotos))  Directory.CreateDirectory(datosConfIni.PathFotos); 
                if (!Directory.Exists(datosConfIni.PathPlaning))  Directory.CreateDirectory(datosConfIni.PathPlaning); 
                if (!Directory.Exists(datosConfIni.PathDatos))  Directory.CreateDirectory(datosConfIni.PathDatos); 
			
                
			VenConfiguracion confTpv = new VenConfiguracion(datosConfIni);
			confTpv.salirConfiguracion += OnSalirconfIniTpv;
			
            
           
        }
       
         void OnSalirconfIniTpv(Object o, EventArgsConfiguracion args){
			
			if(!args.estaCancelado) datosConfIni = args.datosConfiguracion;
			
			DataTable tbConfig = new DataTable(RutasConfig.NOMBRE_TB_CONFIG);
            DataColumn dirBaseDatos = new DataColumn(RutasConfig.COL_BASE_DATOS);
            dirBaseDatos.DataType = Type.GetType("System.String");
            DataColumn RutaFotos = new DataColumn(RutasConfig.COL_FOTOS);
            RutaFotos.DataType = Type.GetType("System.String");
            DataColumn RutaMesa = new DataColumn(RutasConfig.COL_MESA);
            RutaMesa.DataType = Type.GetType("System.String");
            DataColumn RutaPlano = new DataColumn(RutasConfig.COL_PLANOS);
            RutaPlano.DataType = Type.GetType("System.String");
            DataColumn RutaDatos = new DataColumn(RutasConfig.COL_DATOS);
            RutaDatos.DataType = Type.GetType("System.String");
            DataColumn Modificado = new DataColumn(RutasConfig.COL_MOD);
            Modificado.DataType = Type.GetType("System.Boolean");
            DataColumn Auxiliar = new DataColumn(RutasConfig.COL_AUXILIAR);
            Auxiliar.DataType = Type.GetType("System.Boolean");
            DataColumn IP = new DataColumn(RutasConfig.COL_IP);
            IP.DataType = Type.GetType("System.String");
            DataColumn puertoD = new DataColumn(RutasConfig.COL_PUERTO_DATOS);
            puertoD.DataType = Type.GetType("System.String");
            DataColumn puetoC = new DataColumn(RutasConfig.COL_PUERTO_COMUN);
            puetoC.DataType = Type.GetType("System.String");
            DataColumn protocolo = new DataColumn(RutasConfig.COL_PROTOCOLO);
            protocolo.DataType = Type.GetType("System.String");
            DataColumn pass = new DataColumn(RutasConfig.COL_SQL_PASS);
            pass.DataType = Type.GetType("System.String");
			DataColumn sqlport = new DataColumn(RutasConfig.COL_SQL_PUERTO);
            sqlport.DataType = Type.GetType("System.String");
			DataColumn sqluser = new DataColumn(RutasConfig.COL_SQL_USER);
            sqluser.DataType = Type.GetType("System.String");
			DataColumn nomImp = new DataColumn(RutasConfig.COL_NOM_IMP);
            nomImp.DataType = Type.GetType("System.String");
            DataColumn act = new DataColumn(RutasConfig.COL_ACT);
            act.DataType = Type.GetType("System.Boolean");
            DataColumn sycNube = new DataColumn(RutasConfig.COL_SYNC);
            sycNube.DataType = Type.GetType("System.Boolean");
            DataColumn nom_servidor_sync = new DataColumn(RutasConfig.COL_NOM_SERVIDOR_SYNC);
            nom_servidor_sync.DataType = Type.GetType("System.String");
            DataColumn nom_servidor_act = new DataColumn(RutasConfig.COL_NOM_SERVIDOR_ACT);
            nom_servidor_act.DataType = Type.GetType("System.String");
           
            
            
            tbConfig.Columns.Add(dirBaseDatos);
            tbConfig.Columns.Add(RutaFotos);
            tbConfig.Columns.Add(RutaMesa);
            tbConfig.Columns.Add(RutaPlano);
            tbConfig.Columns.Add(RutaDatos);
            tbConfig.Columns.Add(Modificado);
            tbConfig.Columns.Add(Auxiliar);
            tbConfig.Columns.Add(IP);
            tbConfig.Columns.Add(puertoD);
            tbConfig.Columns.Add(puetoC);
            tbConfig.Columns.Add(protocolo);
			tbConfig.Columns.Add(sqluser);
			tbConfig.Columns.Add(sqlport);
			tbConfig.Columns.Add(pass);
			tbConfig.Columns.Add(nomImp);
			tbConfig.Columns.Add(act);
			tbConfig.Columns.Add(sycNube);
			tbConfig.Columns.Add(nom_servidor_sync);
			tbConfig.Columns.Add(nom_servidor_act);
			
			 DataRow dr = tbConfig.NewRow();
            dr[RutasConfig.COL_BASE_DATOS] = "BaseTPV";
            dr[RutasConfig.COL_MESA] = datosConfIni.PathMesas;
            dr[RutasConfig.COL_FOTOS] = datosConfIni.PathFotos;
            dr[RutasConfig.COL_PLANOS] = datosConfIni.PathPlaning;
            dr[RutasConfig.COL_DATOS] = datosConfIni.PathDatos;
            dr[RutasConfig.COL_IP]= datosConfIni.ipServidor;
            dr[RutasConfig.COL_PUERTO_COMUN] = datosConfIni.puertoComunicacion;
            dr[RutasConfig.COL_PUERTO_DATOS] = datosConfIni.puertoDatos;
            dr[RutasConfig.COL_PROTOCOLO] = datosConfIni.protocolo;
            dr[RutasConfig.COL_MOD] = true;
            dr[RutasConfig.COL_AUXILIAR] = datosConfIni.esAuxiliar;
			dr[RutasConfig.COL_SQL_PASS] = datosConfIni.sqlPass;
			dr[RutasConfig.COL_SQL_PUERTO] = datosConfIni.sqlPuerto;
			dr[RutasConfig.COL_SQL_USER] = datosConfIni.sqlUser;
			dr[RutasConfig.COL_NOM_IMP] = datosConfIni.nom_imp;
			dr[RutasConfig.COL_ACT] = datosConfIni.actualizar;
			dr[RutasConfig.COL_SYNC] = datosConfIni.sync_nube;
			dr[RutasConfig.COL_NOM_SERVIDOR_SYNC] = datosConfIni.dir_servidor_sync;
			dr[RutasConfig.COL_NOM_SERVIDOR_ACT] = datosConfIni.dir_servidor_act;
			
            tbConfig.Rows.Add(dr);
            tbConfig.WriteXml(RutasConfig.getRutaPrincipal()+System.IO.Path.DirectorySeparatorChar+RutasConfig.FICHERO_CONFIG,XmlWriteMode.WriteSchema);
			
			    if (!Directory.Exists(datosConfIni.PathMesas))  Directory.CreateDirectory(datosConfIni.PathMesas); 
                if (!Directory.Exists(datosConfIni.PathFotos))  Directory.CreateDirectory(datosConfIni.PathFotos); 
                if (!Directory.Exists(datosConfIni.PathPlaning))  Directory.CreateDirectory(datosConfIni.PathPlaning); 
                if (!Directory.Exists(datosConfIni.PathDatos))  Directory.CreateDirectory(datosConfIni.PathDatos); 
			
            this.salirConfig();
			
		}	
		
	}
	
   public class RutasConfig
    {
        public const String NOMBRE_TB_CONFIG = "Config";
        public const String COL_PRINCIPAL = "RutaPrincipal";
        public const String COL_BASE_DATOS = "dirBaseDatos";
        public const string COL_DATOS = "RutDatos";
        public const String COL_FOTOS = "RutaFotos";
        public const String COL_MESA = "RutaMesas";
        public const string COL_PLANOS = "RutaPlano";
        public const string COL_MOD = "Modificado";
        public const string COL_AUXILIAR = "EsAuxiliar";
        public const string COL_IP = "IP";
        public const string COL_PUERTO_DATOS = "puerto_datos";
        public const string COL_PUERTO_COMUN = "puerto_comunicacion";
        public const string COL_PROTOCOLO = "protocolo";
		public const string COL_SQL_PUERTO = "sql_puerto";
        public const string COL_SQL_PASS = "sql_pass";
        public const string COL_SQL_USER = "sql_user";
		public const string COL_NOM_IMP  = "nom_Impresora";
		public const string COL_ACT  = "actualizar";
		public const string COL_SYNC  = "sync_nube";
		public const string COL_NOM_SERVIDOR_SYNC  = "nom_servidor_sync";
        public const string COL_NOM_SERVIDOR_ACT  = "nom_servidor_atc";
		
        public const String FICHERO_CONFIG = "ValleTpv.config";
        
        public const String NOM_DIR_DATOS = "Datos";
        public const String NOM_DIR_MESAS = "Mesas";
        public const String NOM_DIR_FOTOS = "Fotos";
        public const string NOM_DIR_PLANOS = "Planos";
       
			
        public static String getRutaPrincipal()
        {
       	     return  Valle.Utilidades.RutasArchivos.Ruta_Principal;
        }

       
    }
}
