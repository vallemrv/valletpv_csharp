
// This file has been generated by the GUI designer. Do not modify.
namespace Valle.GesTpv
{
	public partial class VenFamilias
	{
		private global::Gtk.VBox vboxPrincipal;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Label label1;
		private global::Gtk.SpinButton txtIDFamilia;
		private global::Gtk.HBox hbox2;
		private global::Gtk.Label label2;
		private global::Gtk.Entry txtNombre;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TreeView lstTabla;
		private global::Gtk.HButtonBox grpModos;
		private global::Gtk.Button btnEditar;
		private global::Gtk.Button btnAñadir;
		private global::Gtk.Button btnBorrar;
		private global::Gtk.HButtonBox grpModos1;
		private global::Gtk.Button btnAceptar;
		private global::Gtk.Button btnSalir;
		private global::Gtk.Label lblInformacion;
        
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Valle.GesTpv.VenFamilias
			this.WidthRequest = 400;
			this.HeightRequest = 450;
			this.Name = "Valle.GesTpv.VenFamilias";
			this.Title = global::Mono.Unix.Catalog.GetString ("Familias de articulos");
			this.Icon = new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, "./Iconos/exec.gif"));
			this.WindowPosition = ((global::Gtk.WindowPosition)(3));
			this.Modal = true;
			this.Resizable = false;
			this.AllowGrow = false;
			// Container child Valle.GesTpv.VenFamilias.Gtk.Container+ContainerChild
			this.vboxPrincipal = new global::Gtk.VBox ();
			this.vboxPrincipal.Name = "vboxPrincipal";
			this.vboxPrincipal.Spacing = 6;
			this.vboxPrincipal.BorderWidth = ((uint)(12));
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("ID familia");
			this.hbox1.Add (this.label1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.txtIDFamilia = new global::Gtk.SpinButton (0, 1E+30, 1);
			this.txtIDFamilia.CanFocus = true;
			this.txtIDFamilia.Name = "txtIDFamilia";
			this.txtIDFamilia.Adjustment.PageIncrement = 10;
			this.txtIDFamilia.ClimbRate = 1;
			this.txtIDFamilia.Numeric = true;
			this.hbox1.Add (this.txtIDFamilia);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.txtIDFamilia]));
			w2.Position = 1;
			this.vboxPrincipal.Add (this.hbox1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.hbox1]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Nombre Familia");
			this.hbox2.Add (this.label2);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.label2]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.txtNombre = new global::Gtk.Entry ();
			this.txtNombre.CanFocus = true;
			this.txtNombre.Name = "txtNombre";
			this.txtNombre.IsEditable = true;
			this.txtNombre.InvisibleChar = '●';
			this.hbox2.Add (this.txtNombre);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.txtNombre]));
			w5.Position = 1;
			this.vboxPrincipal.Add (this.hbox2);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.hbox2]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.WidthRequest = 400;
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.lstTabla = new global::Gtk.TreeView ();
			this.lstTabla.Name = "lstTabla";
			this.GtkScrolledWindow.Add (this.lstTabla);
			this.vboxPrincipal.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.GtkScrolledWindow]));
			w8.Position = 2;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.grpModos = new global::Gtk.HButtonBox ();
			this.grpModos.Name = "grpModos";
			this.grpModos.Spacing = 14;
			this.grpModos.BorderWidth = ((uint)(6));
			this.grpModos.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnEditar = new global::Gtk.Button ();
			this.btnEditar.CanFocus = true;
			this.btnEditar.Name = "btnEditar";
			this.btnEditar.UseUnderline = true;
			// Container child btnEditar.Gtk.Container+ContainerChild
			global::Gtk.Alignment w9 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w10 = new global::Gtk.HBox ();
			w10.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w11 = new global::Gtk.Image ();
			w11.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "stock_effects", global::Gtk.IconSize.Menu);
			w10.Add (w11);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w13 = new global::Gtk.Label ();
			w13.LabelProp = global::Mono.Unix.Catalog.GetString ("Editar");
			w13.UseUnderline = true;
			w10.Add (w13);
			w9.Add (w10);
			this.btnEditar.Add (w9);
			this.grpModos.Add (this.btnEditar);
			global::Gtk.ButtonBox.ButtonBoxChild w17 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnEditar]));
			w17.Expand = false;
			w17.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnAñadir = new global::Gtk.Button ();
			this.btnAñadir.CanFocus = true;
			this.btnAñadir.Name = "btnAñadir";
			this.btnAñadir.UseUnderline = true;
			// Container child btnAñadir.Gtk.Container+ContainerChild
			global::Gtk.Alignment w18 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w19 = new global::Gtk.HBox ();
			w19.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w20 = new global::Gtk.Image ();
			w20.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w19.Add (w20);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w22 = new global::Gtk.Label ();
			w22.LabelProp = global::Mono.Unix.Catalog.GetString ("Añadir");
			w22.UseUnderline = true;
			w19.Add (w22);
			w18.Add (w19);
			this.btnAñadir.Add (w18);
			this.grpModos.Add (this.btnAñadir);
			global::Gtk.ButtonBox.ButtonBoxChild w26 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnAñadir]));
			w26.Position = 1;
			w26.Expand = false;
			w26.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnBorrar = new global::Gtk.Button ();
			this.btnBorrar.CanFocus = true;
			this.btnBorrar.Name = "btnBorrar";
			this.btnBorrar.UseUnderline = true;
			// Container child btnBorrar.Gtk.Container+ContainerChild
			global::Gtk.Alignment w27 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w28 = new global::Gtk.HBox ();
			w28.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w29 = new global::Gtk.Image ();
			w29.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-delete", global::Gtk.IconSize.Menu);
			w28.Add (w29);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w31 = new global::Gtk.Label ();
			w31.LabelProp = global::Mono.Unix.Catalog.GetString ("Borrar art");
			w31.UseUnderline = true;
			w28.Add (w31);
			w27.Add (w28);
			this.btnBorrar.Add (w27);
			this.grpModos.Add (this.btnBorrar);
			global::Gtk.ButtonBox.ButtonBoxChild w35 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnBorrar]));
			w35.Position = 2;
			w35.Expand = false;
			w35.Fill = false;
			this.vboxPrincipal.Add (this.grpModos);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.grpModos]));
			w36.Position = 3;
			w36.Expand = false;
			w36.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.grpModos1 = new global::Gtk.HButtonBox ();
			this.grpModos1.Name = "grpModos1";
			this.grpModos1.Spacing = 14;
			this.grpModos1.BorderWidth = ((uint)(6));
			this.grpModos1.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child grpModos1.Gtk.ButtonBox+ButtonBoxChild
			this.btnAceptar = new global::Gtk.Button ();
			this.btnAceptar.CanFocus = true;
			this.btnAceptar.Name = "btnAceptar";
			this.btnAceptar.UseStock = true;
			this.btnAceptar.UseUnderline = true;
			this.btnAceptar.Label = "gtk-ok";
			this.grpModos1.Add (this.btnAceptar);
			global::Gtk.ButtonBox.ButtonBoxChild w37 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos1 [this.btnAceptar]));
			w37.Expand = false;
			w37.Fill = false;
			// Container child grpModos1.Gtk.ButtonBox+ButtonBoxChild
			this.btnSalir = new global::Gtk.Button ();
			this.btnSalir.CanFocus = true;
			this.btnSalir.Name = "btnSalir";
			this.btnSalir.UseUnderline = true;
			// Container child btnSalir.Gtk.Container+ContainerChild
			global::Gtk.Alignment w38 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w39 = new global::Gtk.HBox ();
			w39.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w40 = new global::Gtk.Image ();
			w40.Pixbuf = new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, "./Iconos/EXIT00C.ICO"));
			w39.Add (w40);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w42 = new global::Gtk.Label ();
			w42.LabelProp = global::Mono.Unix.Catalog.GetString ("Salir");
			w42.UseUnderline = true;
			w39.Add (w42);
			w38.Add (w39);
			this.btnSalir.Add (w38);
			this.grpModos1.Add (this.btnSalir);
			global::Gtk.ButtonBox.ButtonBoxChild w46 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos1 [this.btnSalir]));
			w46.Position = 1;
			w46.Expand = false;
			w46.Fill = false;
			this.vboxPrincipal.Add (this.grpModos1);
			global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.grpModos1]));
			w47.Position = 4;
			w47.Expand = false;
			w47.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.lblInformacion = new global::Gtk.Label ();
			this.lblInformacion.Name = "lblInformacion";
			this.lblInformacion.Xalign = 0F;
			this.lblInformacion.LabelProp = global::Mono.Unix.Catalog.GetString ("Modo no editable activado");
			this.vboxPrincipal.Add (this.lblInformacion);
			global::Gtk.Box.BoxChild w48 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.lblInformacion]));
			w48.Position = 5;
			w48.Expand = false;
			w48.Fill = false;
			this.Add (this.vboxPrincipal);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 410;
			this.DefaultHeight = 479;
			this.btnAceptar.Hide ();
			this.Show ();
			this.lstTabla.CursorChanged += new global::System.EventHandler (this.OnLstTablaCursorChanged);
			this.btnEditar.Clicked += new global::System.EventHandler (this.OnBtnEditarClicked);
			this.btnAñadir.Clicked += new global::System.EventHandler (this.OnBtnAñadirClicked);
			this.btnBorrar.Clicked += new global::System.EventHandler (this.OnBtnBorrarClicked);
			this.btnAceptar.Clicked += new global::System.EventHandler (this.OnBtnAceptarClicked);
			this.btnSalir.Clicked += new global::System.EventHandler (this.OnBtnSalirClicked);
		}
	}
}