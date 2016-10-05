using System;
using System.Threading;

namespace Valle.GtkUtilidades
{
	public delegate void MetodoSinArg();
	public class MetodosAsync
    {
            Delegate del;
    		object[] arg;
    		public MetodosAsync(Delegate del, params object[] arg){
    			this.del=del;
    			this.arg=arg;
    		}
    		
    		public void GtkFuncAsync(){
    			Thread h = new Thread(new ThreadStart(HFuncAsyncGtk));
    			h.Start();
    		}
		
		    public void FuncAsync(){
			  Thread h = new Thread(new ThreadStart(HFuncAsync));
    			h.Start();
		    }
		    
		    void HFuncAsync(){
			  del.DynamicInvoke(arg);
		    }
                
    		void HFuncAsyncGtk(){
    			Gtk.Application.Invoke(delegate{
				   del.DynamicInvoke(arg);
			   });
    	   	}
        }
    
}

