
// This file has been generated by the GUI designer. Do not modify.
namespace Valle.GesTpv
{
	public partial class VenColores
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Label label4;
		private global::Gtk.Entry txtNombre;
		private global::Gtk.HBox hbox2;
		private global::Gtk.ColorButton btnColor;
		private global::Valle.GtkUtilidades.CImagen imgColor;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TreeView lstTabla;
		private global::Gtk.HButtonBox grpModos;
		private global::Gtk.Button btnAceptar;
		private global::Gtk.Button btnEditar;
		private global::Gtk.Button btnAñadir;
		private global::Gtk.Button btnBorrar;
		private global::Gtk.Button btnSalir;
		private global::Gtk.Label lblInformacion;
        
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Valle.GesTpv.VenColores
			this.HeightRequest = 450;
			this.Name = "Valle.GesTpv.VenColores";
			this.Title = global::Mono.Unix.Catalog.GetString ("Colores de controles");
			this.Icon = new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, "./Iconos/exec.gif"));
			this.WindowPosition = ((global::Gtk.WindowPosition)(3));
			this.Modal = true;
			this.Resizable = false;
			this.AllowGrow = false;
			// Container child Valle.GesTpv.VenColores.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			this.vbox1.BorderWidth = ((uint)(9));
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Nombre color");
			this.hbox1.Add (this.label4);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label4]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.txtNombre = new global::Gtk.Entry ();
			this.txtNombre.CanFocus = true;
			this.txtNombre.Name = "txtNombre";
			this.txtNombre.Text = global::Mono.Unix.Catalog.GetString ("o");
			this.txtNombre.IsEditable = true;
			this.txtNombre.InvisibleChar = '●';
			this.hbox1.Add (this.txtNombre);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.txtNombre]));
			w2.Position = 1;
			// Container child hbox1.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.btnColor = new global::Gtk.ColorButton ();
			this.btnColor.WidthRequest = 56;
			this.btnColor.HeightRequest = 56;
			this.btnColor.CanFocus = true;
			this.btnColor.Events = ((global::Gdk.EventMask)(784));
			this.btnColor.Name = "btnColor";
			this.hbox2.Add (this.btnColor);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.btnColor]));
			w3.Position = 0;
			w3.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.imgColor = new global::Valle.GtkUtilidades.CImagen ();
			this.imgColor.WidthRequest = 50;
			this.imgColor.HeightRequest = 50;
			this.imgColor.Events = ((global::Gdk.EventMask)(256));
			this.imgColor.Name = "imgColor";
			this.hbox2.Add (this.imgColor);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.imgColor]));
			w4.Position = 1;
			this.hbox1.Add (this.hbox2);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.hbox2]));
			w5.Position = 2;
			w5.Expand = false;
			this.vbox1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
			w6.Position = 0;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.lstTabla = new global::Gtk.TreeView ();
			this.lstTabla.CanFocus = true;
			this.lstTabla.Name = "lstTabla";
			this.GtkScrolledWindow.Add (this.lstTabla);
			this.vbox1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.GtkScrolledWindow]));
			w8.Position = 1;
			// Container child vbox1.Gtk.Box+BoxChild
			this.grpModos = new global::Gtk.HButtonBox ();
			this.grpModos.Name = "grpModos";
			this.grpModos.Spacing = 14;
			this.grpModos.BorderWidth = ((uint)(6));
			this.grpModos.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnAceptar = new global::Gtk.Button ();
			this.btnAceptar.CanFocus = true;
			this.btnAceptar.Name = "btnAceptar";
			this.btnAceptar.UseStock = true;
			this.btnAceptar.UseUnderline = true;
			this.btnAceptar.Label = "gtk-ok";
			this.grpModos.Add (this.btnAceptar);
			global::Gtk.ButtonBox.ButtonBoxChild w9 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnAceptar]));
			w9.Expand = false;
			w9.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnEditar = new global::Gtk.Button ();
			this.btnEditar.CanFocus = true;
			this.btnEditar.Name = "btnEditar";
			this.btnEditar.UseUnderline = true;
			// Container child btnEditar.Gtk.Container+ContainerChild
			global::Gtk.Alignment w10 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w11 = new global::Gtk.HBox ();
			w11.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w12 = new global::Gtk.Image ();
			w12.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "stock_effects", global::Gtk.IconSize.Menu);
			w11.Add (w12);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w14 = new global::Gtk.Label ();
			w14.LabelProp = global::Mono.Unix.Catalog.GetString ("Editar");
			w14.UseUnderline = true;
			w11.Add (w14);
			w10.Add (w11);
			this.btnEditar.Add (w10);
			this.grpModos.Add (this.btnEditar);
			global::Gtk.ButtonBox.ButtonBoxChild w18 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnEditar]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnAñadir = new global::Gtk.Button ();
			this.btnAñadir.CanFocus = true;
			this.btnAñadir.Name = "btnAñadir";
			this.btnAñadir.UseUnderline = true;
			// Container child btnAñadir.Gtk.Container+ContainerChild
			global::Gtk.Alignment w19 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w20 = new global::Gtk.HBox ();
			w20.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w21 = new global::Gtk.Image ();
			w21.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w20.Add (w21);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w23 = new global::Gtk.Label ();
			w23.LabelProp = global::Mono.Unix.Catalog.GetString ("Añadir");
			w23.UseUnderline = true;
			w20.Add (w23);
			w19.Add (w20);
			this.btnAñadir.Add (w19);
			this.grpModos.Add (this.btnAñadir);
			global::Gtk.ButtonBox.ButtonBoxChild w27 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnAñadir]));
			w27.Position = 2;
			w27.Expand = false;
			w27.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnBorrar = new global::Gtk.Button ();
			this.btnBorrar.CanFocus = true;
			this.btnBorrar.Name = "btnBorrar";
			this.btnBorrar.UseUnderline = true;
			// Container child btnBorrar.Gtk.Container+ContainerChild
			global::Gtk.Alignment w28 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w29 = new global::Gtk.HBox ();
			w29.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w30 = new global::Gtk.Image ();
			w30.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-delete", global::Gtk.IconSize.Menu);
			w29.Add (w30);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w32 = new global::Gtk.Label ();
			w32.LabelProp = global::Mono.Unix.Catalog.GetString ("Borrar art");
			w32.UseUnderline = true;
			w29.Add (w32);
			w28.Add (w29);
			this.btnBorrar.Add (w28);
			this.grpModos.Add (this.btnBorrar);
			global::Gtk.ButtonBox.ButtonBoxChild w36 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnBorrar]));
			w36.Position = 3;
			w36.Expand = false;
			w36.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnSalir = new global::Gtk.Button ();
			this.btnSalir.CanFocus = true;
			this.btnSalir.Name = "btnSalir";
			this.btnSalir.UseUnderline = true;
			// Container child btnSalir.Gtk.Container+ContainerChild
			global::Gtk.Alignment w37 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w38 = new global::Gtk.HBox ();
			w38.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w39 = new global::Gtk.Image ();
			w39.Pixbuf = new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, "./Iconos/EXIT00C.ICO"));
			w38.Add (w39);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w41 = new global::Gtk.Label ();
			w41.LabelProp = global::Mono.Unix.Catalog.GetString ("Salir");
			w41.UseUnderline = true;
			w38.Add (w41);
			w37.Add (w38);
			this.btnSalir.Add (w37);
			this.grpModos.Add (this.btnSalir);
			global::Gtk.ButtonBox.ButtonBoxChild w45 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnSalir]));
			w45.Position = 4;
			w45.Expand = false;
			w45.Fill = false;
			this.vbox1.Add (this.grpModos);
			global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.grpModos]));
			w46.Position = 2;
			w46.Expand = false;
			w46.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.lblInformacion = new global::Gtk.Label ();
			this.lblInformacion.Name = "lblInformacion";
			this.lblInformacion.Xalign = 0F;
			this.lblInformacion.LabelProp = global::Mono.Unix.Catalog.GetString ("Modo no editable activado");
			this.vbox1.Add (this.lblInformacion);
			global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.lblInformacion]));
			w47.Position = 3;
			w47.Expand = false;
			w47.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 594;
			this.DefaultHeight = 674;
			this.btnAceptar.Hide ();
			this.Show ();
			this.txtNombre.Activated += new global::System.EventHandler (this.OnTxtNombreActivated);
			this.lstTabla.CursorChanged += new global::System.EventHandler (this.OnLstTablaCursorChanged);
			this.btnAceptar.Clicked += new global::System.EventHandler (this.OnBtnAceptarClicked);
			this.btnEditar.Clicked += new global::System.EventHandler (this.OnBtnEditarClicked);
			this.btnAñadir.Clicked += new global::System.EventHandler (this.OnBtnAñadirClicked);
			this.btnBorrar.Clicked += new global::System.EventHandler (this.OnBtnBorrarClicked);
			this.btnSalir.Clicked += new global::System.EventHandler (this.OnBtnSalirClicked);
		}
	}
}