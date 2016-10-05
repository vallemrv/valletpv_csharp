using System;

namespace Valle.SqlUtilidades.Tools
{
	public class ClausulaWhere{
	    string key;
		object valor;
		public ClausulaWhere(string key, object valor){
			this.key = key;
			this.valor = valor;
		}
		
		public string getClausula(){
		   return key +"="+  CadenasParaSql.EmpaquetarSegunTipo	(valor);
		}
	}
}

