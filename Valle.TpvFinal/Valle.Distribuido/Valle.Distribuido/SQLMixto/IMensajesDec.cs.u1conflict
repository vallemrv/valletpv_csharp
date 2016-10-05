using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Lifetime;
using System.Data;

namespace Valle.Distribuido.Servidores
{
    [Serializable]
    public delegate void OnMensajeDecc(string accion, object o);

    public class MensajesDec: MarshalByRefObject
    {
        OnMensajeDecc EjMen;

        public MensajesDec(OnMensajeDecc func)
        {
            EjMen += new OnMensajeDecc(func);
        }

        public void EnviarMen(string men,object o)
        {
            EjMen(men, o);
        }
    
        public override Object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();

            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.Zero;
            }
            return lease;
        }
       
       /* public void EjecutarDecc(MensajesDec men, string[] intervalo, string valor)
        {

            string strDiario = "SELECT Ticket.FechaCobrado, SUM(LineasTicket.TotalLinea) AS diario " +
                               "FROM Ticket INNER JOIN LineasTicket ON Ticket.NumTicket = LineasTicket.numTicket " +
                               "GROUP BY Ticket.FechaCobrado " +
                               "HAVING (Ticket.FechaCobrado >= '" + intervalo[0] + "' AND Ticket.FechaCobrado <= '" + intervalo[1] + "')" +
                               "ORDER BY Ticket.FechaCobrado";



            DataTable diario = this.ejecutarConsultaSelect(strDiario, "Diario");
            DataTable TotalIntervalo = this.ejecutarConsultaSelect(
                "SELECT SUM(LineasTicket.TotalLinea) AS Total FROM Ticket INNER JOIN " +
                "LineasTicket ON Ticket.NumTicket = LineasTicket.numTicket WHERE (Ticket.FechaCobrado >= '" + intervalo[0] + "' AND Ticket.FechaCobrado <= '" + intervalo[1] + "')", "TotalIntervalo");
            DataTable Ticket = this.cargarTabla("Ticket");
            DataTable LineasTicket = this.cargarTabla("LineasTicket");
            DataTable LineasRondas = this.cargarTabla("LineasRonda");
            DataTable NumLineasTicket = this.ejecutarConsultaSelect(
                "SELECT numTicket, COUNT(numTicket) AS lineas " +
                "FROM   LineasTicket " +
                "GROUP BY numTicket " +
                "ORDER BY numTicket ", "NumLineas");

            decimal total = (decimal)TotalIntervalo.Rows[0][0];
            decimal totalBus = Decimal.Parse(valor);
            if (total > totalBus)
            {
                decimal promedio = (((total - totalBus) * 100) / total) / 100;
                int dia = 0;
                while (total > totalBus)
                {
                    decimal impDiario = (decimal)diario.Rows[dia]["diario"];
                    string fechaDia = diario.Rows[dia]["FechaCobrado"].ToString();
                    decimal difDiario = impDiario * promedio;
                    decimal difCons = 0;
                    dia++;
                    DataView dw = new DataView(Ticket, "FechaCobrado ='" + fechaDia + "'", "NumTicket", DataViewRowState.CurrentRows);
                    int pos = 0;
                    int maxBus = 15;
                    while (difCons < difDiario)
                    {
                        DataRow[] rs = NumLineasTicket.Select("numTicket = " + dw[pos]["NumTicket"].ToString());
                        if ((rs.Length > 0) && ((int)rs[0]["Lineas"] >= maxBus))
                        {
                            DataView dwLineas = new DataView(LineasTicket, "numTicket = " + dw[pos]["NumTicket"].ToString(), "totalLinea", DataViewRowState.CurrentRows);
                            int reg = this.ejecutarConsultaNoSelect("DELETE FROM LineasRonda WHERE numTicket = " +
                                dw[pos]["NumTicket"].ToString() + " AND  nomArticulo = '" + dwLineas[dwLineas.Count - 1]["nomArticulo"].ToString() + "'");

                            reg = this.ejecutarConsultaNoSelect("DELETE FROM LineasTicket WHERE numTicket = " +
                              dw[pos]["NumTicket"].ToString() + " AND  nomArticulo = '" + dwLineas[dwLineas.Count - 1]["nomArticulo"].ToString() + "'");

                            if (reg > 0) { difCons += (decimal)dwLineas[dwLineas.Count - 1]["totalLinea"]; }

                        }
                        pos++;
                        if (pos >= dw.Count) { pos = 0; maxBus--; }
                        if (maxBus < 2) { maxBus = 15; }
                    }
                    total -= difCons;
                }
            }
        }*/

    }
}
