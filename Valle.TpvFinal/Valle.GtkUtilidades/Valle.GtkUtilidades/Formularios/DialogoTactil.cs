using System;
namespace Valle.GtkUtilidades
{
	public enum TipoRes { si_no, aceptar}
	public enum ResDialogTactil { si, no, aceptar, cancelar}
	public delegate void OnSalirDialogoTactil(ResDialogTactil res);
	
	public partial class DialogoTactil : Gtk.Window
	{
		
		protected virtual void OnBtnNoClicked (object sender, System.EventArgs e)
		{
			if(salirDialogoTactil != null) salirDialogoTactil(ResDialogTactil.no);
			this.Destroy();
		}
		
		public event OnSalirDialogoTactil salirDialogoTactil;
		
		public DialogoTactil (string tl, string men, TipoRes tipoRes ) : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.Title = tl; 
			
			this.lblInf.Texto =  men;
			this.lblInf.Font = new System.Drawing.Font("Arial",16,System.Drawing.FontStyle.Bold);
			this.lblInf.AlienamientoH = System.Drawing.StringAlignment.Near;
			this.lblInf.AlienamientoV= System.Drawing.StringAlignment.Center;
			this.lblInf.ColorDeFono = System.Drawing.SystemColors.ButtonShadow;
			
			if(tipoRes == TipoRes.si_no) 
				        this.btnAceptar.Visible = false;
			   else 
				    this.btnNo.Visible = this.btnSi.Visible = false;
		}
		 
		protected virtual void OnBtnAceptarClicked (object sender, System.EventArgs e)
		{
			if(salirDialogoTactil != null) salirDialogoTactil(ResDialogTactil.aceptar);
			this.Destroy();
		}
		
		protected virtual void OnBtnSiClicked (object sender, System.EventArgs e)
		{ 
			if(salirDialogoTactil != null) salirDialogoTactil(ResDialogTactil.si);
			this.Destroy();
		}
		
		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			if(salirDialogoTactil != null) salirDialogoTactil(ResDialogTactil.cancelar);
			//this.Destroy();
		}
		
		
		public static void MostrarMensaje(string titulo, string Mensaje, OnSalirDialogoTactil salir){
		   DialogoTactil td = new DialogoTactil(titulo,Mensaje,TipoRes.aceptar);
			td.salirDialogoTactil += salir;
		
		}
		
		
	}
}
