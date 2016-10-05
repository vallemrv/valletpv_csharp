using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using Valle.SqlGestion;
using Valle.SqlUtilidades;
using Valle.Utilidades;

namespace Valle.ToolsTpv
{
	
	
    public class TablaEnString{
      public static string VistaRondasServidas(IGesSql gesL, string consIntervaloTicket, string consIDTpv, string intervaloHoras, string tarifa, string nomCarareo){
       /* if( gesL.EjecutarSqlSelect("","SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES "+
              " WHERE (ROUTINE_NAME = 'CalculoPrecioUnidad') AND (ROUTINE_SCHEMA = 'dbo') AND (ROUTINE_TYPE = 'FUNCTION') ").Rows.Count<=0){
             gesL.EjConsultaNoSelect("","CREATE FUNCTION dbo.CalculoPrecioUnidad( "+
	                 "@numero integer, @nombre VARCHAR(50)) "+
	                 " RETURNS Decimal(5,2)  "+ 
	                 " AS  	BEGIN "+
                     " DECLARE @PrecioUnidad Decimal(5,2) "+
                     "   DECLARE CDatos CURSOR FOR	  "+
    	             "  Select Top 1 TotalLinea/Cantidad "+
	                 " From LineasTicket "+
	                 " Where numTicket = @numero AND nomArticulo = @nombre  "+
	                 " open CDatos  Fetch CDatos Into @PrecioUnidad "+
                     " RETURN @PrecioUnidad  END ");
                     }*/
                     
         consIntervaloTicket = consIntervaloTicket.Length>0 ? " AND ("+consIntervaloTicket.Replace("numTicket","Ticket.NumTicket")+")":"";
         intervaloHoras = intervaloHoras.Length>0 ? " AND ("+intervaloHoras.Replace("Fecha","Rondas.FechaServido + ' ' + Rondas.HoraServido")+")":"";
         tarifa = tarifa.Length>0 ? " AND ("+tarifa.Replace("Tarifa","LineasRonda.Tarifa")+")":"";
         consIDTpv = consIDTpv.Length>0? "("+consIDTpv.Replace("IDTpv","Ticket.IDTpv")+")":"";
         nomCarareo = nomCarareo.Length>0 ? " AND ("+nomCarareo.Replace("nomCamarero","Rondas.CamareroServido")+")":"";
        
         string consultaCompleta = "(SELECT  (FechaServido+' '+HoraServido) as Fecha, TotalLinea "+
                                   " FROM          LineasRonda INNER JOIN "+
                                   "           Rondas ON Rondas.IDVinculacion = LineasRonda.IDRonda INNER JOIN "+
                                   "           Ticket ON Ticket.NumTicket = LineasRonda.numTicket "+
                                   " WHERE  @IDTpv @Tarifa @NomCamarero @Intervalo_horas @Intervalo_ticket) AS VistaRondasSl";                   
                                   
           consultaCompleta = consultaCompleta.Replace("@Intervalo_ticket",consIntervaloTicket);
           consultaCompleta = consultaCompleta.Replace("@IDTpv",consIDTpv);
           consultaCompleta = consultaCompleta.Replace("@Intervalo_horas",intervaloHoras);
           consultaCompleta = consultaCompleta.Replace("@Tarifa",tarifa);
           consultaCompleta = consultaCompleta.Replace("@NomCamarero",nomCarareo);
           return consultaCompleta;
       }
    }
	public class DesgloseCierre{
		public string[] lienasArticulos;
		public decimal totalCierre =0;
		public string fechaCierre ="";
		public string HoraCierre = "";
		public int numTicket=0;
	}

    public class VentaCamarero
    {
        
        public decimal totalCobrado = 0;
        public decimal totalServido = 0;
        public string nomCamarero = "";
        public int numticketCobrados = 0;
        public int numRodasServidas = 0;
        public decimal VentaComision = 0;
        public decimal Comision = 0;
        public decimal MediaVentas
        {
            get { return totalCobrado / numticketCobrados; }
        }
        public decimal MediaServido
        {
            get { return totalServido / numRodasServidas;
			}
        }
        
        
        
        public VentaCamarero(IGesSql gesL, string nomCam, string nomCamaSim, int IDCierre)
        {
            this.nomCamarero = nomCam;
            DataTable tbCierre = gesL.EjecutarSqlSelect("UnCierre", "SELECT * FROM CierreDeCaja WHERE IDCierre = " + IDCierre);
            
             object CalculoTotal = gesL.EjEscalar("SELECT  SUM(LineasTicket.TotalLinea) AS TotalLinea " +
                                  "FROM Ticket INNER JOIN LineasTicket ON Ticket.NumTicket = LineasTicket.numTicket " +
                                  "WHERE (Ticket.NumTicket >= " + tbCierre.Rows[0]["desdeTicket"].ToString() + ")"+
                                  " AND (Ticket.NumTicket <= " + tbCierre.Rows[0]["hastaTicket"].ToString() + ") AND (Ticket.Camarero = '" + nomCam + "') AND "+
                                  "(Ticket.IDTpv ="+tbCierre.Rows[0]["IDTpv"].ToString()+")");
                                  
            object CalculoNumTicket = gesL.EjEscalar("SELECT  COUNT(*) " +
                                  "FROM Ticket WHERE (Ticket.NumTicket >= " + tbCierre.Rows[0]["desdeTicket"].ToString() + ")"+
                                  " AND (Ticket.NumTicket <= " + tbCierre.Rows[0]["hastaTicket"].ToString() + ")"+
                                  " AND (Ticket.Camarero = '" + nomCam + "')"+
                                  " AND (Ticket.IDTpv ="+tbCierre.Rows[0]["IDTpv"].ToString()+")");
                               
            if ((!CalculoTotal.GetType().Name.Equals("DBNull")) && ((decimal)CalculoTotal > 0))
            {
			    totalCobrado = (decimal)CalculoTotal;
				 numticketCobrados =  Convert.ToInt32(CalculoNumTicket);
            }  
       
            
            object CalculoNumRondas = gesL.EjEscalar("SELECT COUNT(DISTINCT LineasRonda.IDRonda) AS Cuenta "+
                                                     "FROM  Rondas INNER JOIN "+
                                                     "LineasRonda ON Rondas.IDVinculacion = LineasRonda.IDRonda INNER JOIN "+
                                                     "Ticket ON LineasRonda.numTicket = Ticket.NumTicket "+
                                                     " WHERE (LineasRonda.numTicket >= " + tbCierre.Rows[0]["desdeTicket"].ToString() + ")"+
                                                     " AND (LineasRonda.numTicket <= " + tbCierre.Rows[0]["hastaTicket"].ToString() + ") AND (IDTpv = "+tbCierre.Rows[0]["IDTpv"].ToString()+")"+
                                                     " AND (CamareroServido = '"+nomCam+ "')");
                          
            object CalculoTotalServido = gesL.EjEscalar("SELECT SUM(TotalLinea) AS suma FROM "+ TablaEnString.VistaRondasServidas(gesL,
                         "(numTicket >= " + tbCierre.Rows[0]["desdeTicket"].ToString() + ") AND (numTicket <= " + tbCierre.Rows[0]["hastaTicket"].ToString() + ")",
                         "(IDTpv = "+tbCierre.Rows[0]["IDTpv"].ToString()+")","","", "(nomCamarero = '"+nomCam+ "') "));
           
            if ((!CalculoTotalServido.GetType().Name.Equals("DBNull")) && ((decimal)CalculoTotalServido > 0))
            {
                totalServido = (decimal)CalculoTotalServido;
			      numRodasServidas = Convert.ToInt32(CalculoNumRondas);
                this.calculoComision(gesL,nomCam,nomCamaSim,tbCierre.Rows[0]);
            }  
       

        }

        void calculoComision(IGesSql gesL, string nomCam,string nomCamPila , DataRow Cierre)
        {
          
          DataRow Camarero = gesL.EjecutarSqlSelect("Camareros","SELECT * FROM Camareros WHERE Nombre ='"+nomCamPila+"'").Rows[0];
          DataTable InstCom = gesL.EjecutarSqlSelect("Inst","SELECT * FROM InstComision WHERE IDCamarero ="+ Camarero["IDCamarero"].ToString());
          if(InstCom.Rows.Count>0){
           string HCierre = Cierre["HoraCierre"].ToString(); 
           DataRow FechaTicketPrimero = gesL.EjecutarSqlSelect("TicketPrimero","Select * FROM Ticket WHERE NumTicket = "+
                                                              Cierre["desdeTicket"].ToString()).Rows[0];
                
           DataRow FechaTicketUltimo = gesL.EjecutarSqlSelect("TicketPrimero","Select * FROM Ticket WHERE NumTicket = "+
                                                            Cierre["hastaTicket"].ToString()).Rows[0];
          
           
           // DataTable ResComision = gesL.ExtraerTabla ("ResComision","IDVinculacion");
              foreach(DataRow r in InstCom.Rows){

             string consultaTarifa = !(r["Tarifa"].GetType().Name.Equals("DBNull")) ? "(Tarifa = "+r["Tarifa"].ToString() +")": "";
             string HFinal = !r["HoraFin"].GetType().Name.Equals("DBNull") ? FechaTicketUltimo["FechaCobrado"].ToString()+" "+r["HoraFin"].ToString() : 
             	                                                             FechaTicketUltimo["FechaCobrado"].ToString()+" "+HCierre;
             string intervaloH = CadenasParaSql.CrearConsultaIntervaloHora("Fecha", FechaTicketPrimero["FechaCobrado"].ToString()+" "+ r["HoraInicio"].ToString(), HFinal);
             object totalComision = gesL.EjEscalar("SELECT SUM(TotalLinea) AS suma FROM "+ TablaEnString.VistaRondasServidas(gesL,
                         "(numTicket >= " + Cierre["desdeTicket"].ToString() + ") AND (numTicket <= " + Cierre["hastaTicket"].ToString() + ")",
                         "(IDTpv = "+Cierre["IDTpv"].ToString()+")",intervaloH,
                         consultaTarifa,"(nomCamarero = '"+nomCam+ "')"));
                         
             if ((!totalComision.GetType().Name.Equals("DBNull")) && ((decimal)totalComision > 0))
             {
                 this.VentaComision += (decimal)totalComision;
                 this.Comision += this.VentaComision* (decimal)r["PorcientoCom"];
             }
           }
           
           

        }
       }
    }

    public class VentaHoras
    {
        public decimal totalVenta = 0;
        public string HInicio = "";
        public string HFin = "";
        public int numTicket = 0;
        public decimal Media
        {
            get { return totalVenta / numTicket; }
        }
        public VentaHoras(IGesSql gesL, string hIni, string hFin, int IDCierre)
        {
            DataTable tbCierre = gesL.EjecutarSqlSelect("UnCierre", "SELECT * FROM CierreDeCaja WHERE IDCierre = " + IDCierre);
            object CalculoTotal = gesL.EjEscalar("SELECT  SUM(LineasTicket.TotalLinea) "+
                                  "FROM Ticket INNER JOIN LineasTicket ON Ticket.NumTicket = LineasTicket.numTicket "+
                                  "WHERE    "+ CadenasParaSql.CrearConsultaIntervaloHora("Ticket.HoraCobrado",hIni,hFin) +
                                  " AND (Ticket.NumTicket >= " + tbCierre.Rows[0]["desdeTicket"].ToString() + ") AND (Ticket.NumTicket <= " + tbCierre.Rows[0]["hastaTicket"].ToString() + ")"+
                                  " AND Ticket.IDTpv = "+tbCierre.Rows[0]["IDTpv"].ToString());
                                  
            object CalculoNumTicket = gesL.EjEscalar("SELECT  COUNT(*) " +
                                  "FROM Ticket WHERE " + CadenasParaSql.CrearConsultaIntervaloHora("Ticket.HoraCobrado", hIni, hFin) +
                                  " AND (Ticket.NumTicket >= " + tbCierre.Rows[0]["desdeTicket"].ToString() + ") AND (Ticket.NumTicket <= " + tbCierre.Rows[0]["hastaTicket"].ToString() + ")"+
                                  " AND Ticket.IDTpv = "+tbCierre.Rows[0]["IDTpv"].ToString());

            HInicio = hIni;
            HFin = hFin;
            
            if ((!CalculoTotal.GetType().Name.Equals("DBNull")) && ((decimal)CalculoTotal > 0))
            {
                totalVenta = (decimal)CalculoTotal;
                numTicket = Convert.ToInt32(CalculoNumTicket);
            }
            
        }
    }

	
    public class GesVentas
    {
		
    	public DesgloseCierre CalcularCierre(IGesSql ges, int idTpv, int ticketCom, int ticketFin)
        {
    		DesgloseCierre desglose = new DesgloseCierre();
             DataTable tbSumaCierre ;
             DataTable tbResumen;
             DataTable tbNumTicket;
            
            int numComienzo = ticketCom;
   
            tbSumaCierre = ges.EjecutarSqlSelect("SumaCierre","SELECT SUM(TotalLinea) AS total FROM "+
                                    "(SELECT  LineasTicket.numTicket, LineasTicket.TotalLinea FROM "+
                                      "LineasTicket INNER JOIN Ticket ON LineasTicket.numTicket = Ticket.NumTicket "+
                                      "WHERE (Ticket.IDTpv = "+ idTpv+ ")) AS LineasTpv WHERE numTicket >= "+numComienzo
                                                                                                      +" AND  numTicket <= "+ ticketFin);

            tbNumTicket = ges.EjecutarSqlSelect("NumTicket", ("SELECT MIN(numTicket) AS minimo, MAX(numTicket) AS maximo FROM " +
                                      "(SELECT  LineasTicket.numTicket FROM " +
                                      "LineasTicket INNER JOIN Ticket ON LineasTicket.numTicket = Ticket.NumTicket " +
                                      "WHERE (Ticket.IDTpv = " + idTpv + ")) AS LineasTpv WHERE numTicket >= "+numComienzo
                                      +" AND  numTicket <= "+ ticketFin));
                                                                                                      
            tbResumen = ges.EjecutarSqlSelect("TbResumen","SELECT nomArticulo, SUM(Cantidad) AS Cantidad " +
                                        "FROM (SELECT  LineasTicket.numTicket, LineasTicket.nomArticulo, LineasTicket.Cantidad FROM "+
                                         "LineasTicket INNER JOIN Ticket ON LineasTicket.numTicket = Ticket.NumTicket "+
                                         "WHERE (Ticket.IDTpv = "+ idTpv+ ")) AS LineasTpv  WHERE (numTicket >= "+numComienzo
                                                                                                      +" AND  numTicket <= "+ ticketFin+") " +
                                         "GROUP BY nomArticulo");
             
            if (tbResumen.Rows.Count > 0)
                {
                
                int logArray = tbResumen.Rows.Count;
                desglose.lienasArticulos = new String[logArray];
                desglose.totalCierre = (decimal)tbSumaCierre.Rows[0][0];
                desglose.numTicket = (int)tbNumTicket.Rows[0]["maximo"] - (int)tbNumTicket.Rows[0]["minimo"]+1;
                int punt = 0;
                foreach (DataRow dr in tbResumen.Rows)
                {
                    String cantidad = String.Format("{0:#0.###}", dr["Cantidad"]);
                    
                    if(cantidad.Contains(",")){
                      string[] cantidades = cantidad.Split(',');
                      cantidad = cantidades[0].PadLeft(4)+','+cantidades[1];
                    }else{
                      cantidad = cantidad.PadLeft(4);
                    }
                 
                    desglose.lienasArticulos[punt] = dr["nomArticulo"].ToString().PadRight(22) + cantidad;
                    punt++;
                }
                 
                return desglose;
            }
            return null;
        }
        
        public List<string> CalcularVentaHoras(IGesSql ges, int IDCierre){
           string[] Intervalos = new string[] { "07:00-09:00", "09:01-11:00", "11:01-13:00", "13:01-15:00", "15:01-17:00","17:01-19:00",
                                                  "19:01-21:00","21:01-23:00","23:01-00:00",
                                                  "00:01-02:00","02:01-03:00","03:01-05:00",
                                                  "05:01-06:59"};
            List<string>  Listado = new List<string>();                                    
            VentaHoras infVentaHoras;
            Listado.Add("Estadistica de ventas por hora");
            Listado.Add("");
            foreach (string intervalo in Intervalos)
            {
                string[] sepInt = intervalo.Split('-');
                infVentaHoras = new VentaHoras(ges, sepInt[0], sepInt[1], IDCierre);
                if(infVentaHoras.totalVenta>0){
                    Listado.Add("En el intervalo de "+ intervalo);
                     Listado.Add(String.Format("Total vendido = {0:c}",infVentaHoras.totalVenta));
                      Listado.Add("Num de ticket "+ infVentaHoras.numTicket);
                      Listado.Add(String.Format("Media por ticket = {0:c}",infVentaHoras.Media));
                      Listado.Add("");
                }
                
            }
            
            return Listado;
        }
        
        public List<string> CalcularVentaCamareros(IGesSql ges, int IDCierre, IBarrProgres miBarra)
        {
            
            List<string>  Listado = new List<string>();                                    
            Listado.Add("Estadistica de camareros");
            Listado.Add("");
            
            VentaCamarero infVentaCamarero;
            DataTable tbCamareros = ges.ExtraerTabla("Camareros",null);
            miBarra.MaxProgreso = tbCamareros.Rows.Count;
            foreach (DataRow rC in tbCamareros.Rows)
            {
                miBarra.Progreso ++;
                
                infVentaCamarero = new VentaCamarero(ges,rC["Nombre"].ToString()+" "+rC["Apellidos"].ToString(),rC["Nombre"].ToString(), IDCierre);
                    if(infVentaCamarero.totalCobrado + infVentaCamarero.totalServido + infVentaCamarero.VentaComision >0)
                                     Listado.Add("Camarero = "+rC["Nombre"].ToString()+" "+rC["Apellidos"].ToString());
                    
                    if(infVentaCamarero.totalCobrado>0){
                          Listado.Add(String.Format("Total cobrado = {0:c}", infVentaCamarero.totalCobrado));
                          Listado.Add("Num ticket cobrados = "+ infVentaCamarero.numticketCobrados);
                          Listado.Add(String.Format("Media de ticket cobrados = {0:c}",infVentaCamarero.MediaVentas));
                          Listado.Add("");
                          }
                  
                    if(infVentaCamarero.totalServido>0){
                         Listado.Add(String.Format("Total servido = {0:c}",infVentaCamarero.totalServido));
                         Listado.Add("Num rondas servidas = "+ infVentaCamarero.numRodasServidas);
                         Listado.Add(String.Format("Media servido = {0:c}", infVentaCamarero.MediaServido));
                         Listado.Add("");
                    }
                    
                    if(infVentaCamarero.VentaComision>0){
                                        
                        Listado.Add(String.Format("Total venta comision = {0:c}", infVentaCamarero.VentaComision));
                        Listado.Add(String.Format("Comision dia = {0:c}",infVentaCamarero.Comision));
                        Listado.Add("");    
                    }
                
            }

               return Listado;
        }

        public string[] CalcularCierre(IGesSql ges, int idTpv)
        {
             DataTable tbCierreCaja = ges.ExtraerTabla("CierreDeCaja", "IDCierre");
             DataTable tbSumaCierre ;
             DataTable tbResumen;
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
           
            tbResumen = ges.EjecutarSqlSelect("TbResumen","SELECT nomArticulo, SUM(Cantidad) AS Cantidad " +
                                        "FROM (SELECT  LineasTicket.numTicket, LineasTicket.nomArticulo, LineasTicket.Cantidad FROM "+
                                         "LineasTicket INNER JOIN Ticket ON LineasTicket.numTicket = Ticket.NumTicket "+
                                         "WHERE (Ticket.IDTpv = "+ idTpv+ ")) AS LineasTpv  WHERE (numTicket >  "+numComienzo+") " +
                                         "GROUP BY nomArticulo");
             
            if (tbResumen.Rows.Count > 0)
                {
                
                int logArray = 4 + tbResumen.Rows.Count;
                String[] linea = new String[logArray];
                decimal totalDia = (decimal)tbSumaCierre.Rows[0][0];
                linea[0] = String.Format("Total del dia: {0:#0.00}", totalDia);
                int numTicket = (int)tbNumTicket.Rows[0]["maximo"] - (int)tbNumTicket.Rows[0]["minimo"]+1;
                linea[1] = String.Format("Numero de ticket: {0}", numTicket);
                linea[2] = String.Format("Media por ticket: {0:#0.00}", totalDia / numTicket);
                linea[3] = "";
                int punt = 4;
                foreach (DataRow dr in tbResumen.Rows)
                {
                    String cantidad = String.Format("{0:#0.###}", dr["Cantidad"]);
                    if(cantidad.Contains(",")){
                      string[] cantidades = cantidad.Split(',');
                      cantidad = cantidades[0].PadLeft(4)+','+cantidades[1];
                    }else{
                      cantidad = cantidad.PadLeft(4);
                    }
                    linea[punt] = dr["nomArticulo"].ToString().PadRight(30) + cantidad;
                    punt++;
                }

                DataRow drCierre = tbCierreCaja.NewRow();
                drCierre["desdeTicket"] = (int)tbNumTicket.Rows[0]["minimo"];
                drCierre["hastaTicket"] = (int)tbNumTicket.Rows[0]["maximo"];
                drCierre["fechaCierre"] = Utilidades.CadenasTexto.RotarFecha(DateTime.Now.ToShortDateString());
                drCierre["HoraCierre"] = DateTime.Now.ToShortTimeString().PadLeft(5,'0');
                drCierre["IDTpv"] = idTpv;
                tbCierreCaja.Rows.Add(drCierre);
                ges.EjConsultaNoSelect("CierreDeCaja",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(drCierre,AccionesConReg.Agregar,
				                                                                "").Replace(@"\",@"\\"));
				
                return linea;
            }
            return null;
        }
    }
}
