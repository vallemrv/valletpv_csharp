
// This file has been generated by the GUI designer. Do not modify.
namespace Valle.GtkUtilidades
{
	public partial class Botonera
	{
		private global::Gtk.HBox hbox1;

		private global::Gtk.Alignment pneBotonera;

		private global::Gtk.VButtonBox pnePaginacion;

		private global::Gtk.Button btnArriba;

		private global::Gtk.Button btnAbajo;

		private global::Gtk.Button btnAgregar;

		private global::Gtk.Button btnSalir;

		protected virtual void Contruir ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Valle.GtkUtilidades.Botonera
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Valle.GtkUtilidades.Botonera";
			// Container child Valle.GtkUtilidades.Botonera.Gtk.Container+ContainerChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.CanFocus = true;
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.pneBotonera = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.pneBotonera.Name = "pneBotonera";
			this.hbox1.Add (this.pneBotonera);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.pneBotonera]));
			w1.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.pnePaginacion = new global::Gtk.VButtonBox ();
			this.pnePaginacion.Name = "pnePaginacion";
			this.pnePaginacion.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(3));
			// Container child pnePaginacion.Gtk.ButtonBox+ButtonBoxChild
			this.btnArriba = new global::Gtk.Button ();
			this.btnArriba.CanFocus = true;
			this.btnArriba.Name = "btnArriba";
			this.btnArriba.UseUnderline = true;
			// Container child btnArriba.Gtk.Container+ContainerChild
			global::Gtk.Alignment w2 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w3 = new global::Gtk.HBox ();
			w3.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w4 = new global::Gtk.Image ();
			w4.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-go-up", global::Gtk.IconSize.Dialog);
			w3.Add (w4);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w6 = new global::Gtk.Label ();
			w3.Add (w6);
			w2.Add (w3);
			this.btnArriba.Add (w2);
			this.pnePaginacion.Add (this.btnArriba);
			global::Gtk.ButtonBox.ButtonBoxChild w10 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.pnePaginacion[this.btnArriba]));
			w10.Expand = false;
			w10.Fill = false;
			// Container child pnePaginacion.Gtk.ButtonBox+ButtonBoxChild
			this.btnAbajo = new global::Gtk.Button ();
			this.btnAbajo.WidthRequest = 100;
			this.btnAbajo.HeightRequest = 100;
			this.btnAbajo.CanFocus = true;
			this.btnAbajo.Name = "btnAbajo";
			this.btnAbajo.UseUnderline = true;
			// Container child btnAbajo.Gtk.Container+ContainerChild
			global::Gtk.Alignment w11 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w12 = new global::Gtk.HBox ();
			w12.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w13 = new global::Gtk.Image ();
			w13.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-go-down", global::Gtk.IconSize.Dialog);
			w12.Add (w13);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w15 = new global::Gtk.Label ();
			w12.Add (w15);
			w11.Add (w12);
			this.btnAbajo.Add (w11);
			this.pnePaginacion.Add (this.btnAbajo);
			global::Gtk.ButtonBox.ButtonBoxChild w19 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.pnePaginacion[this.btnAbajo]));
			w19.Position = 1;
			w19.Expand = false;
			w19.Fill = false;
			// Container child pnePaginacion.Gtk.ButtonBox+ButtonBoxChild
			this.btnAgregar = new global::Gtk.Button ();
			this.btnAgregar.WidthRequest = 100;
			this.btnAgregar.HeightRequest = 100;
			this.btnAgregar.CanFocus = true;
			this.btnAgregar.Name = "btnAgregar";
			this.btnAgregar.UseUnderline = true;
			// Container child btnAgregar.Gtk.Container+ContainerChild
			global::Gtk.Alignment w20 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w21 = new global::Gtk.HBox ();
			w21.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w22 = new global::Gtk.Image ();
			w22.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Dialog);
			w21.Add (w22);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w24 = new global::Gtk.Label ();
			w21.Add (w24);
			w20.Add (w21);
			this.btnAgregar.Add (w20);
			this.pnePaginacion.Add (this.btnAgregar);
			global::Gtk.ButtonBox.ButtonBoxChild w28 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.pnePaginacion[this.btnAgregar]));
			w28.Position = 2;
			w28.Expand = false;
			w28.Fill = false;
			// Container child pnePaginacion.Gtk.ButtonBox+ButtonBoxChild
			this.btnSalir = new global::Gtk.Button ();
			this.btnSalir.WidthRequest = 100;
			this.btnSalir.HeightRequest = 100;
			this.btnSalir.CanFocus = true;
			this.btnSalir.Name = "btnSalir";
			this.btnSalir.UseUnderline = true;
			// Container child btnSalir.Gtk.Container+ContainerChild
			global::Gtk.Alignment w29 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w30 = new global::Gtk.HBox ();
			w30.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w31 = new global::Gtk.Image ();
			w31.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Valle.GtkUtilidades.iconos.~APP21MB.ICO");
			w30.Add (w31);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w33 = new global::Gtk.Label ();
			w30.Add (w33);
			w29.Add (w30);
			this.btnSalir.Add (w29);
			this.pnePaginacion.Add (this.btnSalir);
			global::Gtk.ButtonBox.ButtonBoxChild w37 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.pnePaginacion[this.btnSalir]));
			w37.Position = 3;
			w37.Expand = false;
			w37.Fill = false;
			this.hbox1.Add (this.pnePaginacion);
			global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.pnePaginacion]));
			w38.Position = 1;
			w38.Expand = false;
			this.Add (this.hbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.btnAgregar.Hide ();
			this.Hide ();
			this.btnArriba.Clicked += new global::System.EventHandler (this.OnBtnArribaClicked);
			this.btnAbajo.Clicked += new global::System.EventHandler (this.OnBtnAbajoClicked);
			this.btnAgregar.Clicked += new global::System.EventHandler (this.OnBtnAgregarClicked);
			this.btnSalir.Clicked += new global::System.EventHandler (this.btnSalir_Click);
		}
	}
}
