using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

using Valle.SqlGestion;
using Valle.SqlUtilidades;
using Valle.Utilidades;

namespace Valle.ToolsTpv
{
    public class Ticket{
       public Hashtable lineas = null;
       public DateTime FechaCobrado = DateTime.Now;
       public DateTime FechaInicio;
       public String Camarero ;
       public String Mesa;
       public int idTpv;
	   public bool CerrarMesa = true;
	   public decimal TotalTicket = 0m;
	   public decimal Cambio = 0m;
	 }
     
    public class GuardarTicket 
    {

        int numTicket=0;
        int idGesMesas=0;
        int idRondas = 0;
        int idNulas = 0;
        DataTable tbTicket;
        DataTable tbLineas ;
        DataTable tbGesMesas ;
        DataTable tbLineasRond;
        DataTable tbRondas;
        DataTable tbLineasNulas;
       
        ColaCompartida<Ticket> colaTicket;
        ColaCompartida<Mesa> colaMesas;
        Thread miHilo;
        AutoResetEvent exmut = new AutoResetEvent(false);
        string SELECT_TOP_LIMIT = "SELECT @TOP * FROM @TABLA ORDER BY @COLUMNKEY DESC @LIMIT";

        public enum IDTabla { Ticket, GesMesas, Rondas, nulas };
        public AutoResetEvent ColaVacia = new AutoResetEvent(true);
        public IGesSql gesBase;
        
        

        public int NumTicketEnCola{
        	get{
        		return this.colaTicket.NumElementos;
        	}
        }
        
        public int NumMesasEnCola{
        	get{
        		return this.colaMesas.NumElementos;
        	}
        }
        
        public void encolarTicket(Ticket ticket){
                colaTicket.Encolar(ticket);
           
        }

        public void encolarMesa(Mesa mesa){
                colaMesas.Encolar(mesa);
           
        }
        
        public Ticket DescolarTicket(){
  		   return colaTicket.Descolar();
        }
        
        public Mesa DescolarMesa(){
          	return colaMesas.Descolar();
        }
        public GuardarTicket(IGesSql ges)
        {
        	if(ges is GesMySQL){
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@TOP","");
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@LIMIT","LIMIT 1");
            }else{
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@LIMIT","");
            	SELECT_TOP_LIMIT = SELECT_TOP_LIMIT.Replace("@TOP","TOP 1");
            }
        	
            this.gesBase = ges;
            this.InicializarTablas();
            colaTicket = new ColaCompartida<Ticket>(ColaVacia);
            colaMesas = new ColaCompartida<Mesa>(ColaVacia);
            miHilo = new Thread(new ThreadStart(this.HiloGuardar));
            miHilo.Start();
        }



        public int NumIDCorresponde(IDTabla id)
        {
            exmut.WaitOne();
            int idR;
            switch (id)
            {
                case IDTabla.Ticket:
                    idR = numTicket;
                    numTicket++;
                    exmut.Set();
                    return idR;
                    
                case IDTabla.Rondas:
                     idR = idRondas;
                    idRondas++;
                    exmut.Set();
                    return idR;
                case IDTabla.nulas:
                     idR = idNulas;
                    idNulas++;
                    exmut.Set();
                    return idR;
                case IDTabla.GesMesas:
                     idR = idGesMesas;
                    idGesMesas++;
                    exmut.Set();
                    return idR;
             
            }

            return 0;
        }

        private void InicializarTablas()
        {
        	string consulta = SELECT_TOP_LIMIT.Replace("@TABLA","Ticket");
        	consulta = consulta.Replace("@COLUMNKEY","NumTicket");
        	tbTicket = gesBase.EjecutarSqlSelect("Ticket", consulta);
        	
        	consulta = SELECT_TOP_LIMIT.Replace("@TABLA","LineasTicket");
        	consulta = consulta.Replace("@COLUMNKEY","numTicket");
            tbLineas = gesBase.EjecutarSqlSelect("LineasTicket", consulta);
            this.numTicket = tbTicket.Rows.Count > 0 ? (int)tbTicket.Rows[0]["NumTicket"]+1 : 1;

            #region Cargar tablas de mesas
            //Cargar gestor de mesas
            
            consulta = SELECT_TOP_LIMIT.Replace("@TABLA","GestionMesas");
        	consulta = consulta.Replace("@COLUMNKEY","IDVinculacion");
            tbGesMesas = gesBase.EjecutarSqlSelect("GestionMesas", consulta);
            this.idGesMesas = tbGesMesas.Rows.Count > 0 ? (int)tbGesMesas.Rows[0]["IDVinculacion"]+1 : 1;

            consulta = SELECT_TOP_LIMIT.Replace("@TABLA","LineasRonda");
        	consulta = consulta.Replace("@COLUMNKEY","numTicket");
            tbLineasRond = gesBase.EjecutarSqlSelect("LineasRonda", consulta);
            
            consulta = SELECT_TOP_LIMIT.Replace("@TABLA","Rondas");
        	consulta = consulta.Replace("@COLUMNKEY","IDVinculacion");
            tbRondas = gesBase.EjecutarSqlSelect("Rondas", consulta);
            this.idRondas = tbRondas.Rows.Count > 0 ? (int)tbRondas.Rows[0]["IDVinculacion"]+1 : 1;

            consulta = SELECT_TOP_LIMIT.Replace("@TABLA","LineasNulas");
        	consulta = consulta.Replace("@COLUMNKEY","IDVinculacion");
            tbLineasNulas = gesBase.EjecutarSqlSelect("LineasNulas", consulta);
            this.idNulas = tbLineasNulas.Rows.Count > 0 ? (int)tbLineasNulas.Rows[0]["IDVinculacion"]+1 : 1;
            exmut.Set();


            #endregion


        }
        private void HiloGuardar()
        {
            
            while (true)
            {
                 int numEstTicket = this.guardarTicket(colaTicket.Descolar());
                 this.guardarMesas(colaMesas.Descolar(), numEstTicket);
            }
         }

        public void guardarMesas(Mesa mesaActiva, int numEsteTicket)
        {
            Registro reg = null;
            string nomMesaActiva = mesaActiva.mesa;

            DataRow rM;
            rM = tbGesMesas.NewRow();
            rM["Mesa"] = nomMesaActiva;
            rM["CamareroAbreMesa"] = mesaActiva.camareroAbreMesa;
            rM["FechaInicio"] = Valle.Utilidades.CadenasTexto.RotarFecha(mesaActiva.HoraApertura.ToShortDateString());
            rM["FechaFin"] = Valle.Utilidades.CadenasTexto.RotarFecha(DateTime.Now.ToShortDateString());
            rM["HoraInicio"] = mesaActiva.HoraApertura.ToShortTimeString().PadLeft(5, '0');
            rM["HoraFin"] = DateTime.Now.ToShortTimeString().PadLeft(5, '0');
            rM["IDVinculacion"] = this.NumIDCorresponde(IDTabla.GesMesas);
            reg = UtilidadesReg.DeDataRowARegistro(tbGesMesas.Columns, rM);
            reg.AccionReg = AccionesConReg.Agregar;
            gesBase.ModificarReg(reg);
           

            #region Rondas activas
            DataRow rRonda;
            foreach (Ronda ronda in mesaActiva.RondasActivas)
            {
                rRonda = tbRondas.NewRow();
                rRonda["IDMesa"] = rM["IDVinculacion"];
                rRonda["CamareroServido"] = ronda.nomCamarero;
                rRonda["FechaServido"] = Valle.Utilidades.CadenasTexto.RotarFecha(ronda.horaServido.ToShortDateString());
                rRonda["horaServido"] = ronda.horaServido.ToShortTimeString().PadLeft(5, '0');
                rRonda["IDVinculacion"] = this.NumIDCorresponde(IDTabla.Rondas);
                reg = UtilidadesReg.DeDataRowARegistro(tbRondas.Columns, rRonda);
                reg.AccionReg = AccionesConReg.Agregar;
                gesBase.ModificarReg(reg);

              
                IEnumerator rondActivas = ronda.lineasArtActivos.Values.GetEnumerator();

                while (rondActivas.MoveNext())
                {
                    DataRow rActivas = tbLineasRond.NewRow();
                    rActivas["numTicket"] = numEsteTicket;
                    rActivas["Cantidad"] = ((Articulo)rondActivas.Current).Cantidad;
                    rActivas["nomArticulo"] = ((Articulo)rondActivas.Current).Descripcion;
                    rActivas["Tarifa"] = ((Articulo)rondActivas.Current).Tarifa;
                    rActivas["IDRonda"] = rRonda["IDVinculacion"];
                    rActivas["TotalLinea"] = ((Articulo)rondActivas.Current).TotalLinea;
                    reg = UtilidadesReg.DeDataRowARegistro(tbLineasRond.Columns, rActivas);
                    reg.AccionReg = AccionesConReg.Agregar;
                    gesBase.ModificarReg(reg);

                
                }

                IEnumerator rondCobradas = ronda.lineasArtCobrados.Values.GetEnumerator();
                while (rondCobradas.MoveNext())
                {
                    DataRow rCobradas = tbLineasRond.NewRow();
                    rCobradas["numTicket"] = ((Articulo)rondCobradas.Current).NumTicketPertenencia;
                    rCobradas["nomArticulo"] = ((Articulo)rondCobradas.Current).Descripcion;
                    rCobradas["Cantidad"] = ((Articulo)rondCobradas.Current).Cantidad;
                    rCobradas["Tarifa"] = ((Articulo)rondCobradas.Current).Tarifa;
                    rCobradas["IDRonda"] = rRonda["IDVinculacion"];
                    rCobradas["TotalLinea"] = ((Articulo)rondCobradas.Current).TotalLinea;
                    reg = UtilidadesReg.DeDataRowARegistro(tbLineasRond.Columns, rCobradas);
                    reg.AccionReg = AccionesConReg.Agregar;
                    gesBase.ModificarReg(reg);
                
                }

                foreach (LineaNula nulo in ronda.nulas)
                {
                    DataRow rNulas = tbLineasNulas.NewRow();
                    rNulas["camareroAnula"] = nulo.camareroAnula;
                    rNulas["IDRonda"] = rRonda["IDVinculacion"];
                    rNulas["NombreArticulo"] = nulo.linea.Descripcion;
                    rNulas["Cantidad"] = nulo.linea.Cantidad;
                    rNulas["totalLinea"] = nulo.linea.TotalLinea;
                    rNulas["FechaAnulada"] = Valle.Utilidades.CadenasTexto.RotarFecha(nulo.HoraAnulado.ToShortDateString());
                    rNulas["HoraAnulada"] = nulo.HoraAnulado.ToShortTimeString().PadLeft(5, '0');
                    rNulas["IDVinculacion"] = this.NumIDCorresponde(IDTabla.nulas);
                    reg = UtilidadesReg.DeDataRowARegistro(tbLineasNulas.Columns, rNulas);
                    reg.AccionReg = AccionesConReg.Agregar;
                    gesBase.ModificarReg(reg);
                

                }
            }
            #endregion rondas activas

            #region Rondas Cobradas

            foreach (Ronda ronda in mesaActiva.RondasPagadas)
            {
                rRonda = tbRondas.NewRow();
                rRonda["IDMesa"] = rM["IDVinculacion"];
                rRonda["CamareroServido"] = ronda.nomCamarero;
                rRonda["FechaServido"] = Valle.Utilidades.CadenasTexto.RotarFecha(ronda.horaServido.ToShortDateString());
                rRonda["horaServido"] = ronda.horaServido.ToShortTimeString().PadLeft(5, '0');
                rRonda["IDVinculacion"] = this.NumIDCorresponde(IDTabla.Rondas);
                reg = UtilidadesReg.DeDataRowARegistro(tbRondas.Columns, rRonda);
                reg.AccionReg = AccionesConReg.Agregar;
                gesBase.ModificarReg(reg);
                    
            
                IEnumerator rondActivas = ronda.lineasArtActivos.Values.GetEnumerator();
                while (rondActivas.MoveNext())
                {
                    DataRow rActivas = tbLineasRond.NewRow();
                    rActivas["numTicket"] = ronda.numTicketPertenencia;
                    rActivas["Cantidad"] = ((Articulo)rondActivas.Current).Cantidad;
                    rActivas["nomArticulo"] = ((Articulo)rondActivas.Current).Descripcion;
                    rActivas["Tarifa"] = ((Articulo)rondActivas.Current).Tarifa;
                    rActivas["IDRonda"] = rRonda["IDVinculacion"];
                    rActivas["TotalLineas"] = ((Articulo)rondActivas.Current).TotalLinea;
                    reg = UtilidadesReg.DeDataRowARegistro(tbLineasRond.Columns, rActivas);
                    reg.AccionReg = AccionesConReg.Agregar;
                    gesBase.ModificarReg(reg);
                
                }

                IEnumerator rondCobradas = ronda.lineasArtCobrados.Values.GetEnumerator();
                while (rondCobradas.MoveNext())
                {
                    DataRow rCobradas = tbLineasRond.NewRow();
                    rCobradas["numTicket"] = ronda.numTicketPertenencia;
                    rCobradas["nomArticulo"] = ((Articulo)rondCobradas.Current).Descripcion;
                    rCobradas["Cantidad"] = ((Articulo)rondCobradas.Current).Cantidad;
                    rCobradas["Tarifa"] = ((Articulo)rondCobradas.Current).Tarifa;
                    rCobradas["IDRonda"] = rRonda["IDVinculacion"];
                    rCobradas["TotalLinea"] = ((Articulo)rondCobradas.Current).TotalLinea;
                    reg = UtilidadesReg.DeDataRowARegistro(tbLineasRond.Columns, rCobradas);
                    reg.AccionReg = AccionesConReg.Agregar;
                    gesBase.ModificarReg(reg);
                   
                }

                foreach (LineaNula nulo in ronda.nulas)
                {
                    DataRow rNulas = tbLineasNulas.NewRow();
                    rNulas["camareroAnula"] = nulo.camareroAnula;
                    rNulas["IDRonda"] = rRonda["IDVinculacion"];
                    rNulas["NombreArticulo"] = nulo.linea.Descripcion;
                    rNulas["Cantidad"] = nulo.linea.Cantidad;
                    rNulas["totalLinea"] = nulo.linea.TotalLinea;
                    rNulas["FechaAnulada"] = Valle.Utilidades.CadenasTexto.RotarFecha(nulo.HoraAnulado.ToShortDateString());
                    rNulas["HoraAnulada"] = nulo.HoraAnulado.ToShortTimeString().PadLeft(5, '0');
                    rNulas["IDVinculacion"] = this.NumIDCorresponde(IDTabla.nulas);
                    reg = UtilidadesReg.DeDataRowARegistro(tbLineasNulas.Columns, rNulas);
                    reg.AccionReg = AccionesConReg.Agregar;
                    gesBase.ModificarReg(reg);
                    
                  
                }

            }
            #endregion rondas pagadas

         
        }
      
        public int guardarTicket(Ticket ticket)
        {
            Registro reg;
              DataRow drTicket;
                     drTicket = tbTicket.NewRow();
                     drTicket["FechaCobrado"] = Valle.Utilidades.CadenasTexto.RotarFecha(ticket.FechaCobrado.ToShortDateString());
                     drTicket["HoraCobrado"] = ticket.FechaCobrado.ToShortTimeString().PadLeft(5, '0');
                     drTicket["Camarero"] = ticket.Camarero;
                     drTicket["Mesa"] = ticket.Mesa;
                     drTicket["IDTpv"] = ticket.idTpv;
                     drTicket["NumTicket"] = this.NumIDCorresponde(IDTabla.Ticket);
			         drTicket["Cambio"] = ticket.Cambio;
                     reg = UtilidadesReg.DeDataRowARegistro(tbTicket.Columns, drTicket);
                     reg.AccionReg = AccionesConReg.Agregar;
                     gesBase.ModificarReg(reg);
                  
                IEnumerator lineas = ticket.lineas.Values.GetEnumerator();
                    while (lineas.MoveNext())
                    {
                        DataRow linea = tbLineas.NewRow();
                        Articulo articulo = (Articulo)lineas.Current;
                        linea["numTicket"] = drTicket["NumTicket"];
                        linea["Cantidad"] = articulo.Cantidad;
                        linea["nomArticulo"] = articulo.Descripcion;
                        linea["TotalLinea"] = articulo.TotalLinea;
                        reg = UtilidadesReg.DeDataRowARegistro(tbLineas.Columns, linea);
                        reg.AccionReg = AccionesConReg.Agregar;
                        gesBase.ModificarReg(reg);
                      }

                      return (int)drTicket["NumTicket"];
        }

         public void Terminar()
         {
         	while ((NumMesasEnCola + NumTicketEnCola) > 0)
            {
                 ColaVacia.WaitOne();
            }
             if (miHilo.IsAlive) { miHilo.Abort(); }
         }

     
     }
}
