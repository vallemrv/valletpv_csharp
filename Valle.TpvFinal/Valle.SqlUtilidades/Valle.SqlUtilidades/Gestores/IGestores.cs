// IGestores.cs created with MonoDevelop
// User: valle at 22:21 21/08/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;

using Valle.SqlUtilidades;

namespace Valle.SqlGestion
{
	
	
	public interface IGesSql
	{
		 void CargarTabla(string tabla, String columKey);
		 DataTable ExtraerTabla(string tabla, String columKey);
		 DataTable ExtraerTabla(string tabla, String columKey, string dirBaseDatos);
		 DataTable ExtraerTabla(string tabla);
		 bool ActualizarTabla(string tabla);
	     DataTable EjecutarSqlSelect(string nombre, string consulta, string dirBaseDatos);
		 DataTable EjecutarSqlSelect(string nombre, string consulta);
		 Object EjEscalar( string consulta, string dirBaseDatos);
		 Object EjEscalar(String consulta);
		int EjConsultaNoSelect(string tabla, String consulta);
		int EjConsultaNoSelect(string tabla, String consulta, string dirBaseDatos);
		int ModificarReg(Registro reg);
		int ModificarReg(Registro reg, string baseDatos);
		int NumRegEnTabla(string tabla);
		int NumRegEnTabla(string tabla, string baseDatos);
		void AgregarTabla(DataTable tb);
		void Desbloquear();
	}
}
