
using System;
using System.Collections.Generic;
using System.Data;

using Valle.SqlGestion;
using Valle.SqlUtilidades;

namespace Valle.TpvFinal
{
	
	


	public class DegloseArqueo
	{
		
		
		
		Dictionary<string,decimal> arqueo;
		string[] arrayClaves = {"0,20","0,50","1","2","5","10","20","50","100","200","500"};	
		Decimal totalCierre = 0;
		Decimal CajaReal = 0;
		Decimal Cambio = 0;
		Decimal Gastos = 0;
		decimal totalEfectivo =0;
		int numTicket =0;
		public bool Permitido{
		   get{ return numTicket >0;}	
		}
		
		Dictionary<string,decimal> desgloseEfectivo = new Dictionary<string,decimal>();

		public DegloseArqueo (Dictionary<string,decimal> arqueo,int idTpv, GesMySQL ges )
		{
			this.arqueo = arqueo;
		    TotalizarDesblose();
			DataTable tbCierreCaja = ges.ExtraerTabla("CierreDeCaja", "IDCierre");
		    DataTable tbArqueoCaja = ges.ExtraerTabla("ArqueoCaja", "ID");
			DataTable tbDesgloseArqueo = ges.ExtraerTabla("DesgloseArqueo","ID");
            DataTable tbSumaCierre ;
            DataTable tbNumTicket;
            
            DataView dwCierre = new DataView(tbCierreCaja, "IDTpv =" + idTpv, "hastaTicket", DataViewRowState.CurrentRows);
             int numComienzo = dwCierre.Count > 0 ?
                (int)dwCierre[dwCierre.Count - 1]["hastaTicket"] : 0;
   
            tbSumaCierre = ges.EjecutarSqlSelect("SumaCierre","SELECT SUM(TotalLinea) AS total FROM "+
                                    "(SELECT  LineasTicket.numTicket, LineasTicket.TotalLinea FROM "+
                                      "LineasTicket INNER JOIN Ticket ON LineasTicket.numTicket = Ticket.NumTicket "+
                                      "WHERE (Ticket.IDTpv = "+ idTpv+ ")) AS LineasTpv WHERE numTicket > "+numComienzo);

            tbNumTicket = ges.EjecutarSqlSelect("NumTicket", ("SELECT MIN(numTicket) AS minimo, MAX(numTicket) AS maximo FROM " +
                                      "(SELECT  LineasTicket.numTicket FROM " +
                                      "LineasTicket INNER JOIN Ticket ON LineasTicket.numTicket = Ticket.NumTicket " +
                                      "WHERE (Ticket.IDTpv = " + idTpv + ")) AS LineasTpv WHERE numTicket > " + numComienzo));
           
		  numTicket = tbNumTicket.Rows[0]["minimo"].GetType().ToString().Equals("System.DBNull") ? 0 :  (int)tbNumTicket.Rows[0]["maximo"] - (int)tbNumTicket.Rows[0]["minimo"];
			
			
			if(this.Permitido){
			
			  totalCierre = (decimal)tbSumaCierre.Rows[0][0];
            
			   DataRow drCierre = tbCierreCaja.NewRow();
                drCierre["desdeTicket"] = (int)tbNumTicket.Rows[0]["minimo"];
                drCierre["hastaTicket"] = (int)tbNumTicket.Rows[0]["maximo"];
                drCierre["fechaCierre"] = Utilidades.CadenasTexto.RotarFecha(DateTime.Now.ToShortDateString());
                drCierre["HoraCierre"] = DateTime.Now.ToShortTimeString().PadLeft(5,'0');
                drCierre["IDTpv"] = idTpv;
                tbCierreCaja.Rows.Add(drCierre);
				tbCierreCaja.AcceptChanges();
                ges.EjConsultaNoSelect("CierreDeCaja",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(drCierre,AccionesConReg.Agregar,
				                                                                                                        "").Replace(@"\",@"\\"));
				 DataRow drArque =  tbArqueoCaja.NewRow();
				        drArque["Resultado"] = totalCierre-CajaReal;
				        drArque["IDVinculacion"] = drCierre["IDCierre"];
				   tbArqueoCaja.Rows.Add(drArque);tbArqueoCaja.AcceptChanges();
			    
				    ges.EjConsultaNoSelect("ArqueoCaja",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(drArque,AccionesConReg.Agregar,
				                                                                                                        "").Replace(@"\",@"\\"));   
                   foreach(string key in arqueo.Keys){
				      DataRow rDesglose = tbDesgloseArqueo.NewRow();
					     rDesglose["IDVinculacion"] = drArque["ID"];
					     rDesglose["Clave"] = key;
					     rDesglose["Valor"] = arqueo[key];
					   tbDesgloseArqueo.Rows.Add(rDesglose); tbDesgloseArqueo.AcceptChanges();
					
				    	ges.EjConsultaNoSelect("DesgloseArqueo",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(rDesglose,AccionesConReg.Agregar,
				                                                                                                        "").Replace(@"\",@"\\"));   
					
				    }
				   this.PartirDesglose();
				
			    }
		}
		
		void TotalizarDesblose(){
		    foreach(string key in arqueo.Keys){
			      if(key.Contains("cambio")){
					    CajaReal -= arqueo[key];
					     Cambio = arqueo[key];
				  }
				  if(Char.IsNumber(key,0)){
					   Decimal inc = Decimal.Parse(key) * arqueo[key];
					   totalEfectivo += inc;
					   CajaReal += inc;
				  }
				  if(key.Contains("varios")||key.Contains("targeta"))
					                  CajaReal+=arqueo[key];
				
				  if(key.Contains("gastos")){
					Gastos = arqueo[key];
					CajaReal+=arqueo[key];
				  }
			}
			
		}
		
		
		void PartirDesglose(){
			
			if(totalEfectivo>Cambio){
		
			decimal cambioParcial = totalEfectivo;
			int punt = arrayClaves.Length-1;
			
			while ((cambioParcial > Cambio)&&(punt>-1)){
			 
			  decimal moneda = decimal.Parse(arrayClaves[punt]);
			  string key =	arrayClaves[punt];
			 
			  if(arqueo.ContainsKey(key)){
			 
			  decimal num = arqueo[key];
			  decimal cal = arqueo[key] * moneda;	
					
		      if((cambioParcial-cal)>Cambio){
					arqueo[key] = 0;	
					desgloseEfectivo.Add(key,num);
					cambioParcial -= cal;
					  
				}else{
			      decimal dif =cambioParcial-Cambio;
				  if(dif>moneda){
     				  num =   Math.Floor(dif/ moneda);
					  arqueo[key]-=num;
					  desgloseEfectivo.Add(key,num);			
	      			  cal =  num * moneda;
		    		  cambioParcial -= cal;
			    	}			
			     }
			   }	
				
				punt--;
			}
			
		 }
		}
		
		public string[] InformeCambio(){
		   
		 List<string> informe = new List<string>();
			
			informe.Add ("Fecha : "+DateTime.Now.ToShortDateString());
			informe.Add( "Hora : " + DateTime.Now.ToShortTimeString());             
			informe.Add("");
	     	foreach(string k in arrayClaves){
			  if(arqueo.ContainsKey(k) && arqueo[k]>0){
					informe.Add( arqueo[k].ToString().PadLeft(4)+" "+Descripcion(k).PadRight(9)+"  de "+
					                      k.PadLeft(5)+"  =  "+
					                    (decimal.Parse(k)*arqueo[k]).ToString().PadLeft(5));
				}
			}
			  
			 if(arqueo.ContainsKey("varios")) informe.Add("Mas "+arqueo["varios"]+" en moneditas sueltas");
			  informe.Add("");
			  informe.Add("");
			  informe.Add("Total cambio "+Cambio);
			
			return informe.ToArray();
		}
		
		
		public string[] InformeReintegro(){
			
			List<string> informe = new List<string>();
			informe.Add ("Fecha : "+DateTime.Now.ToShortDateString());
			informe.Add( "Hora : " + DateTime.Now.ToShortTimeString());
			informe.Add("");
			foreach(string k in arrayClaves){
			  if(desgloseEfectivo.ContainsKey(k) && desgloseEfectivo[k]>0){
					informe.Add( desgloseEfectivo[k].ToString().PadLeft (4)+" "+Descripcion(k).PadRight(9)+"  de "+
					                      k.PadLeft(5)+"  =  "+
					                    (decimal.Parse(k)*desgloseEfectivo[k]).ToString().PadLeft(5));
				}
			}
			  
			  informe.Add("");
			  informe.Add("");
			  informe.Add("Total efectivo  "+(totalEfectivo-Cambio).ToString());
			  if(arqueo.ContainsKey("targeta"))  informe.Add("Total cobros con targeta  "+arqueo["targeta"]);
			  if(Gastos>0) informe.Add("Total pagos por caja " + Gastos);
			
			return informe.ToArray();
			
		}
		
		string Descripcion(string moneda){
		   switch(moneda){
			case "0,20":
			case "0,50":
			case "1":
			case "2":
				return "monedas";
				
			default:
				return "Billetes";
				
			}
			
		}
		
		 
	}
}
