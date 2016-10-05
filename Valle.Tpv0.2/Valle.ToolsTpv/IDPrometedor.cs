using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Valle.ToolsTpv
{
    public class CargarIDPrometedor
    {

        public int IDPrometedor;

        public CargarIDPrometedor(DataTable tb)
        {
            DataView dw;
            int ahora = DateTime.Now.Hour;
            int HMejorSeajusta;
            int hMayor;
            int IndexIDHmayor = 0;
            bool encontrado = false;
            bool primeraVez = true;
                 
                    dw = new DataView(tb, "", "HoraInicioTpv", DataViewRowState.CurrentRows);
                    HMejorSeajusta = DateTime.Parse(dw[0]["HoraInicioTpv"].ToString()).Hour;
                    hMayor = HMejorSeajusta;
                    IDPrometedor = (int)dw[0]["IDTpv"];
                    for(int i= 0;i<dw.Count;i++)
                    {
                        int horaInicio = DateTime.Parse(dw[i]["HoraInicioTpv"].ToString()).Hour;
                        if (horaInicio >= hMayor)
                        { IndexIDHmayor = i; hMayor = horaInicio; }
                   
                        if (horaInicio <= ahora)
                        {
                            if ((primeraVez) || (horaInicio >= HMejorSeajusta))
                            {
                                primeraVez = false;
                                IDPrometedor = (int)dw[i]["IDTpv"];
                                HMejorSeajusta = DateTime.Parse(dw[i]["HoraInicioTpv"].ToString()).Hour;
                                encontrado = true;
                            }
                        }
                        
                    }
                   if (!encontrado) { IDPrometedor = Int32.Parse(dw[IndexIDHmayor]["IDTpv"].ToString()); }
         
        }
		
		public static int CargarIDFav(string[] idsFav, DateTime[] horasInicioFav)
        {
            if (idsFav.Length > 0)
            {
                DateTime ahora = DateTime.Now;
                int ID = Int32.Parse(idsFav[0]);
                DateTime hMejorSeAjusta = horasInicioFav[0];
                DateTime hMayor = hMejorSeAjusta;
                int IndexIDHmayor = 0;
                bool encontrado = false;
                bool primeraVez = true;
           

                for (int i = 0; i < horasInicioFav.Length; i++)
                {
                    if (horasInicioFav[i] >= hMayor)
                              { IndexIDHmayor = i; hMayor = horasInicioFav[i]; }
                    if (horasInicioFav[i] <= ahora)
                    {
                        if ((primeraVez) || (horasInicioFav[i] >= hMejorSeAjusta))
                        {
                            primeraVez = false;
                            ID = Int32.Parse(idsFav[i]);
                            hMejorSeAjusta = horasInicioFav[i];
                            encontrado = true;
                        }
                    }
                   
                }
                if (!encontrado) { ID = Int32.Parse(idsFav[IndexIDHmayor]); }
				return ID;
            }else
			
			return -1;
		}

        public  CargarIDPrometedor(String[] idsFav, int[] horasInicioFav)
        {
            if (idsFav.Length > 0)
            {
                int ahora = DateTime.Now.Hour;
                IDPrometedor = Int32.Parse(idsFav[0]);
                int hMejorSeAjusta = horasInicioFav[0];
                int hMayor = hMejorSeAjusta;
                int IndexIDHmayor = 0;
                bool encontrado = false;
                bool primeraVez = true;
           

                for (int i = 0; i < horasInicioFav.Length; i++)
                {
                    if (horasInicioFav[i] >= hMayor)
                              { IndexIDHmayor = i; hMayor = horasInicioFav[i]; }
                    if (horasInicioFav[i] <= ahora)
                    {
                        if ((primeraVez) || (horasInicioFav[i] >= hMejorSeAjusta))
                        {
                            primeraVez = false;
                            IDPrometedor = Int32.Parse(idsFav[i]);
                            hMejorSeAjusta = horasInicioFav[i];
                            encontrado = true;
                        }
                    }
                   
                }
                if (!encontrado) { IDPrometedor = Int32.Parse(idsFav[IndexIDHmayor]); }
            }
            
        }
    }
}
