/*
 * Creado por SharpDevelop.
 * Usuario: vallevm
 * Fecha: 17/07/2009
 * Hora: 13:23
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Windows.Forms;
using System.Threading;

namespace Valle.Utilidades
 {   
     /// <summary>
     ///  Ejecuta metodos asincronos. Necesita un formulario en agurmentos.
     ///  Puede modificar cualquier control desde otro hilo.
     /// </summary>
     
    public class MetodosAsync
    {
            Delegate del;
    		object[] arg;
    		Form frm;
    		public MetodosAsync(Delegate del, params object[] arg){
    			this.del=del;
    			this.arg=arg;
    		}
    		
    		public void EjFuncAsync(Form frm){
    			this.frm = frm;
    			Thread h = new Thread(new ThreadStart(HFuncAsync));
    			h.Start();
    		}
    
    		void HFuncAsync(){
    			Thread.Sleep(150);
    			frm.Invoke(del, arg);
    	   	}
        }
    
}
