using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Valle.SqlGestion;

namespace Valle.SqlUtilidades
{
    public class LeerEsquemas
    {
     
        const string sqlRule = "SELECT INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME AS TABLA, " +
                               "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME AS COL, " +
                               "INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME AS TABLA_PADRE, " +
                               "INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME AS COL_REF, " +
                               "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UPDATE_RULE, INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.DELETE_RULE " +
                               "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS INNER JOIN " +
                               "INFORMATION_SCHEMA.KEY_COLUMN_USAGE ON " +
                               "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME = INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME " +
                               "INNER JOIN  INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ON INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME = " +
                               "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME  ";

        const string sqlRestricciones = "SELECT INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME, " +
                                       "INFORMATION_SCHEMA.CHECK_CONSTRAINTS.CHECK_CLAUSE " +
                                       "FROM         INFORMATION_SCHEMA.CHECK_CONSTRAINTS INNER JOIN  " +
                                       "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ON  " +
                                       "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME = " +
                                       " INFORMATION_SCHEMA.CHECK_CONSTRAINTS.CONSTRAINT_NAME ";

        const string sqlColumnas = "SELECT TABLE_NAME, COLUMN_NAME,  COLUMN_DEFAULT,  IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, " +
                                     " NUMERIC_SCALE " +
                                     " FROM INFORMATION_SCHEMA.COLUMNS ";
  
        const String sqlLLaves = "SELECT     INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME, INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME, "+
                                " INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_TYPE "+
                                " FROM         INFORMATION_SCHEMA.KEY_COLUMN_USAGE INNER JOIN "+
                                " INFORMATION_SCHEMA.TABLE_CONSTRAINTS ON "+
                                " INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_NAME = INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME "+
                                " WHERE     (INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_TYPE = 'PRIMARY KEY')";

        DataTable rule; 
        DataTable llaves; 
        DataTable columnas;
        DataTable restricciones; 
        IGesSql gestion;

        public LeerEsquemas(IGesSql gestion)
        {
            this.gestion = gestion;
			if(gestion is GesMSSQL){
	            llaves = gestion.EjecutarSqlSelect("LLaves",sqlLLaves);
	            rule = gestion.EjecutarSqlSelect("rule",sqlRule);
	            columnas = gestion.EjecutarSqlSelect("columnas",sqlColumnas);
	            restricciones = gestion .EjecutarSqlSelect("Restriciones",sqlRestricciones);
				}
        }
       
        public DataTable ListaDeTablas
        {
          get{
             return gestion.EjecutarSqlSelect("ListaTablas", "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME <> 'sysdiagrams'");
          }
        }

        public Esquema ExtraerEsquema(string tabla)
        {
		  if(gestion is GesMSSQL){
            InfColumna infCol;
            ClaveExt ClExt;
          
            Esquema esq = new Esquema();
            esq.nomTabla = tabla;

            foreach (DataRow r in columnas.Select("TABLE_NAME = '" + tabla + "' "))
            {
                infCol = new InfColumna();
                infCol.nomColumna = r["COLUMN_NAME"].ToString();
                infCol.tipo  = r["DATA_TYPE"].ToString();
                if (r["CHARACTER_MAXIMUM_LENGTH"].ToString().Length > 0)
                {
                    infCol.numChar = String.Format("({0})", r["CHARACTER_MAXIMUM_LENGTH"].ToString());
                }
                if ((r["NUMERIC_PRECISION"].ToString().Length > 0) && (!infCol.tipo.Equals("int")))
                {
                    infCol.precision = String.Format("({0},{1})", r["NUMERIC_PRECISION"].ToString(), r["NUMERIC_SCALE"].ToString());
                }
                if (r["COLUMN_DEFAULT"].ToString().Length > 0)
                {
                    infCol.Default = r["COLUMN_DEFAULT"].ToString();
                }
                if (r["IS_NULLABLE"].ToString().Equals("NO"))
                {
                    infCol.EsNula = "NOT NULL";
                }
               
				DataRow[] check = restricciones.Select("TABLE_NAME = '" + r["TABLE_NAME"].ToString() +
                                                            "' AND COLUMN_NAME ='" + r["COLUMN_NAME"].ToString() + "'");
                if (check.Length > 0)
                {
                     infCol.Check = check[0]["CHECK_CLAUSE"].ToString();
                }
                esq.infColumnaN.Add(infCol); // a�adimos los valores de la columnaN

                DataRow[] k = llaves.Select("TABLE_NAME = '" + r["TABLE_NAME"].ToString() +
                                                            "' AND COLUMN_NAME ='" + r["COLUMN_NAME"].ToString() + "'");
                if (k.Length > 0)
                {
                    esq.clavesPrim.Add(k[0]["COLUMN_NAME"].ToString());//a�adimos las claves primarias
                }

                DataRow[] f = this.rule.Select("TABLA = '" + r["TABLE_NAME"].ToString() +
                                                            "' AND COL ='" + r["COLUMN_NAME"].ToString() + "'");
                if (f.Length > 0)
                {
                    ClExt = new ClaveExt();
                    ClExt.nombreCol = (f[0]["COL"].ToString());
                    ClExt.nomColPadre = (f[0]["COL_REF"].ToString());
                    ClExt.nomTablaPadre = (f[0]["TABLA_PADRE"].ToString());
                    ClExt.ruleUp = (f[0]["UPDATE_RULE"].ToString());
                    ClExt.ruleDel = (f[0]["DELETE_RULE"].ToString());
                    esq.clavesExt.Add(ClExt);
                }

               
            }
            return esq;
		 }else
				return null;
			
			
        }

    
    }
}
