using System;
using System.Threading;

namespace Valle.GtkUtilidades
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ScrollTactil : Gtk.Bin
	{
		
		
		public event EventHandler moviendoCursor;
		public Gtk.ScrolledWindow wScroll;
		Thread hMoverScroll;
		
		public ScrollTactil ()
		{  
	
			this.Contruir ();
		}
		
		void MoverScrollUp(){
			 while(wScroll.Vadjustment.Value > wScroll.Vadjustment.Lower){
				Thread.Sleep(50);
				 Gtk.Application.Invoke(delegate {
				     wScroll.Vadjustment.Value -= wScroll.Vadjustment.StepIncrement;
					if(moviendoCursor!=null) moviendoCursor(this, new EventArgs());
				});
			}
		}
		
		void MoverScrollDown(){
			
			 while(wScroll.Vadjustment.Value < wScroll.Vadjustment.Upper-wScroll.VScrollbar.Allocation.Height){
			     Thread.Sleep(50);
				 Gtk.Application.Invoke(delegate {
				     wScroll.Vadjustment.Value += wScroll.Vadjustment.StepIncrement;	
					 if(moviendoCursor!=null) moviendoCursor(this, new EventArgs());
				});
			}
			
		}
		
		
		
		
	    protected virtual void OnBtnSubirOrgPressed (object sender, System.EventArgs e)
		{
			if(wScroll!=null){
			   if((hMoverScroll!=null)&&(hMoverScroll.IsAlive)) hMoverScroll.Abort();
			    hMoverScroll = new Thread(new ThreadStart(MoverScrollUp));
				hMoverScroll.Start();
				                          
			}
		}
		
		protected virtual void OnBtnSubirOrgReleased (object sender, System.EventArgs e)
		{
			if(wScroll!=null)
			   if((hMoverScroll!=null)&&(hMoverScroll.IsAlive)) hMoverScroll.Abort();
			
		}
		
		protected virtual void OnBtnBajarOrgPressed (object sender, System.EventArgs e)
		{
			if(wScroll!=null){
			   if((hMoverScroll!=null)&&(hMoverScroll.IsAlive)) hMoverScroll.Abort();
			    hMoverScroll = new Thread(new ThreadStart(MoverScrollDown));
				hMoverScroll.Start();
			}
		}
		
		protected virtual void OnBtnBajarOrgReleased (object sender, System.EventArgs e)
		{
			 if(wScroll!=null)
			   if((hMoverScroll!=null)&&(hMoverScroll.IsAlive)) hMoverScroll.Abort();
		}
		
		
		
		
		
	}
}

