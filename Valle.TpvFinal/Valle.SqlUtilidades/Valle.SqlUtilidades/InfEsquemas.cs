using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Valle.SqlUtilidades
{

    [Serializable]
    public class InfColumna
    {
        public string nomColumna ="";
        public string tipo = "";
        public string numChar = "";
        public string precision = "";
        public string escala = "";
        public string EsNula = "";
        
        string cDefault = "";
       
        string check = "";
       
        public string Check {
            get {
                return check;
            }
            set {
                check = value;
               check = check.Replace("[","");
               check = check.Replace("]","");
               check = check.Replace("(","");
               check = check.Replace(")","");
            }
        }

        public string Default {
            get {
                return cDefault;
            }
            set {
                cDefault = value;
                cDefault = cDefault.Replace("((","");
                cDefault = cDefault.Replace("))","");
                cDefault = cDefault.Replace("(","");
                cDefault = cDefault.Replace(")","");
            }
        }

    }

    [Serializable]
    public class ClaveExt
    {
        public string nombreCol ="";
        public string nomColPadre = "";
        public string ruleUp = "";
        public string ruleDel = "";
        
        string tablaPadre = "";
        public string nomTablaPadre {
        	set{tablaPadre = value;}
        	get {return tablaPadre;}
        }
        string baseDatos = "";
        
		public string BaseDatos {
			set { baseDatos = value; }
		}
        
        public string StrParaCrearTabla{
        	get{ return baseDatos.Length>0? baseDatos+"."+tablaPadre: tablaPadre;}
        }

       
    }


    [Serializable]
    public class Esquema
    {
        enum rules {delete, update};
        public string nomTabla = "";
        public List<InfColumna> infColumnaN = new List<InfColumna>();
        public List<string> clavesPrim = new List<string>();
        public List<ClaveExt> clavesExt =new List<ClaveExt>();
        public string StrCrearTabla()
        {
            StringBuilder sb = new StringBuilder();
                sb.Append("CREATE TABLE ");
                sb.AppendLine(this.nomTabla);
                sb.Append("(");
                foreach (InfColumna col in this.infColumnaN)
                {
                
                    sb.Append(col.nomColumna+" "+col.tipo+col.numChar+col.precision + " " + col.EsNula);
                    if(col.Default.Length>0)
                        sb.Append(" DEFAULT " + col.Default);
                        
                    if(col.Check.Length>0)
                      sb.AppendFormat(" CHECK ({0})",col.Check);
                    
                    sb.AppendLine(",");
                }
                sb.Remove(sb.Length - 2, 2);
               if(this.clavesPrim.Count >0){
                   sb.AppendLine(", ");
                   sb.Append("PRIMARY KEY (");
                   foreach (string s in this.clavesPrim)
                   {
                       sb.Append(s + ",");
                   }
                   sb.Replace(",", ")", sb.Length - 1, 1);
                }
                
                   string externa = "FOREIGN KEY ({0}) REFERENCES {1}({2})";
                    foreach (ClaveExt clExt in this.clavesExt)
                    {
                        sb.AppendLine(", ");
                        sb.AppendFormat(externa, clExt.nombreCol, clExt.StrParaCrearTabla, clExt.nomColPadre);
                        sb.AppendFormat("{0} {1}", crearRule(clExt.ruleUp, rules.update), crearRule(clExt.ruleDel, rules.delete ));
                    }
               
            
                sb.AppendLine(")");
                return sb.ToString();
        }

        private string crearRule(string p, rules r)
        {
            string s = "";
            switch (r)
            {
                case rules.delete:
                    s = " ON DELETE " + p;
                    break;
                case rules.update:
                    s = " ON UPDATE " + p;
                    break;
            }
            return s;
        }

    }

    [Serializable]
    public class InfEsquemas
    {
        Dictionary<string, Esquema> Esquemas = new Dictionary<string, Esquema>();

		public string[] ListaTablas{
			get{
				string[] lista = new string[Esquemas.Keys.Count];
				 Esquemas.Keys.CopyTo(lista,0);
				return lista;
			}
		}
		
        public void AgregarEsquema( Esquema esq){
            if (Esquemas.ContainsKey(esq.nomTabla))
            {

                Esquemas[esq.nomTabla] = esq;
            }
            else
            {
                Esquemas.Add(esq.nomTabla, esq);
            }

        }
        
        public Esquema ExtraerExquema(string tabla)
        {
            if(Esquemas.ContainsKey(tabla)){
            return Esquemas[tabla];
            }else{
                return null;
            }
        }
    }
}
