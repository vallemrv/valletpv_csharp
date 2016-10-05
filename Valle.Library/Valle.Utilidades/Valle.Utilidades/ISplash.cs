using System;
namespace Valle.Utilidades
{
	public interface ISplash
    {
		int MaxProceso{get;set;}
		int Progreso{get;set;}
	    void mostrarInformacion(string me);
        void mostrarProgreso(int pro);
        void guardar();
    }
}

