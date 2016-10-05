
// This file has been generated by the GUI designer. Do not modify.
namespace Valle.GesTpv
{
	public partial class VenArticulosSimple
	{
		private global::Gtk.VBox vboxPrincipal;
		private global::Gtk.Table pneDatosArt;
		private global::Gtk.VBox vbox1;
		private global::Gtk.CheckButton chkPorKilos;
		private global::Gtk.CheckButton chkNoEnVenta;
		private global::Gtk.VBox vbox2;
		private global::Gtk.Label label5;
		private global::Gtk.SpinButton txtPrecioDos;
		private global::Gtk.VBox vbox3;
		private global::Gtk.Label label6;
		private global::Gtk.SpinButton txtPrecioTres;
		private global::Gtk.VBox vbox4;
		private global::Gtk.Label label4;
		private global::Gtk.SpinButton txtPrecioUno;
		private global::Gtk.VBox vboxFamilias;
		private global::Gtk.Label label7;
		private global::Gtk.HBox hbox1;
		private global::Gtk.ComboBox cmbFamilias;
		private global::Gtk.Button btnAñadirFam;
		private global::Gtk.VBox vboxIDArt;
		private global::Gtk.Label label2;
		private global::Gtk.SpinButton txtID;
		private global::Gtk.VBox vboxNombre;
		private global::Gtk.Label label3;
		private global::Gtk.Entry txtNombre;
		private global::Gtk.HButtonBox grpModos;
		private global::Gtk.Button btnBuscar;
		private global::Gtk.Button btnSalir;
		private global::Gtk.Label lblInformacion;
        
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Valle.GesTpv.VenArticulosSimple
			this.Name = "Valle.GesTpv.VenArticulosSimple";
			this.Title = global::Mono.Unix.Catalog.GetString ("Alta rapida de articulo");
			this.Icon = new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, "./Iconos/exec.gif"));
			this.WindowPosition = ((global::Gtk.WindowPosition)(3));
			this.Modal = true;
			this.Resizable = false;
			this.AllowGrow = false;
			// Container child Valle.GesTpv.VenArticulosSimple.Gtk.Container+ContainerChild
			this.vboxPrincipal = new global::Gtk.VBox ();
			this.vboxPrincipal.Name = "vboxPrincipal";
			this.vboxPrincipal.Spacing = 6;
			this.vboxPrincipal.BorderWidth = ((uint)(9));
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.pneDatosArt = new global::Gtk.Table (((uint)(3)), ((uint)(3)), false);
			this.pneDatosArt.Name = "pneDatosArt";
			this.pneDatosArt.RowSpacing = ((uint)(4));
			this.pneDatosArt.ColumnSpacing = ((uint)(14));
			this.pneDatosArt.BorderWidth = ((uint)(24));
			// Container child pneDatosArt.Gtk.Table+TableChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.chkPorKilos = new global::Gtk.CheckButton ();
			this.chkPorKilos.CanFocus = true;
			this.chkPorKilos.Name = "chkPorKilos";
			this.chkPorKilos.Label = global::Mono.Unix.Catalog.GetString ("Se vende por kilos");
			this.chkPorKilos.DrawIndicator = true;
			this.chkPorKilos.UseUnderline = true;
			this.vbox1.Add (this.chkPorKilos);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.chkPorKilos]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.chkNoEnVenta = new global::Gtk.CheckButton ();
			this.chkNoEnVenta.CanFocus = true;
			this.chkNoEnVenta.Name = "chkNoEnVenta";
			this.chkNoEnVenta.Label = global::Mono.Unix.Catalog.GetString ("Producto no en venta");
			this.chkNoEnVenta.DrawIndicator = true;
			this.chkNoEnVenta.UseUnderline = true;
			this.vbox1.Add (this.chkNoEnVenta);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.chkNoEnVenta]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			this.pneDatosArt.Add (this.vbox1);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.pneDatosArt [this.vbox1]));
			w3.LeftAttach = ((uint)(2));
			w3.RightAttach = ((uint)(3));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child pneDatosArt.Gtk.Table+TableChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 0F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Tarifa 2");
			this.vbox2.Add (this.label5);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.label5]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.txtPrecioDos = new global::Gtk.SpinButton (0, 99999, 1);
			this.txtPrecioDos.CanFocus = true;
			this.txtPrecioDos.Name = "txtPrecioDos";
			this.txtPrecioDos.Adjustment.PageIncrement = 10;
			this.txtPrecioDos.ClimbRate = 1;
			this.txtPrecioDos.Digits = ((uint)(2));
			this.txtPrecioDos.Numeric = true;
			this.vbox2.Add (this.txtPrecioDos);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.txtPrecioDos]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			this.pneDatosArt.Add (this.vbox2);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.pneDatosArt [this.vbox2]));
			w6.TopAttach = ((uint)(2));
			w6.BottomAttach = ((uint)(3));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child pneDatosArt.Gtk.Table+TableChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 0F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Tarifa 3");
			this.vbox3.Add (this.label6);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.label6]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.txtPrecioTres = new global::Gtk.SpinButton (0, 99999, 1);
			this.txtPrecioTres.CanFocus = true;
			this.txtPrecioTres.Name = "txtPrecioTres";
			this.txtPrecioTres.Adjustment.PageIncrement = 10;
			this.txtPrecioTres.ClimbRate = 1;
			this.txtPrecioTres.Digits = ((uint)(2));
			this.txtPrecioTres.Numeric = true;
			this.vbox3.Add (this.txtPrecioTres);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.txtPrecioTres]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			this.pneDatosArt.Add (this.vbox3);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.pneDatosArt [this.vbox3]));
			w9.TopAttach = ((uint)(2));
			w9.BottomAttach = ((uint)(3));
			w9.LeftAttach = ((uint)(2));
			w9.RightAttach = ((uint)(3));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child pneDatosArt.Gtk.Table+TableChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 0F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Tarifa 1");
			this.vbox4.Add (this.label4);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.label4]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.txtPrecioUno = new global::Gtk.SpinButton (0, 99999, 1);
			this.txtPrecioUno.CanFocus = true;
			this.txtPrecioUno.Name = "txtPrecioUno";
			this.txtPrecioUno.Adjustment.PageIncrement = 10;
			this.txtPrecioUno.ClimbRate = 1;
			this.txtPrecioUno.Digits = ((uint)(2));
			this.txtPrecioUno.Numeric = true;
			this.vbox4.Add (this.txtPrecioUno);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.txtPrecioUno]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			this.pneDatosArt.Add (this.vbox4);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.pneDatosArt [this.vbox4]));
			w12.TopAttach = ((uint)(2));
			w12.BottomAttach = ((uint)(3));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child pneDatosArt.Gtk.Table+TableChild
			this.vboxFamilias = new global::Gtk.VBox ();
			this.vboxFamilias.Name = "vboxFamilias";
			this.vboxFamilias.Spacing = 6;
			// Container child vboxFamilias.Gtk.Box+BoxChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 0F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Familia");
			this.vboxFamilias.Add (this.label7);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vboxFamilias [this.label7]));
			w13.Position = 0;
			w13.Expand = false;
			w13.Fill = false;
			// Container child vboxFamilias.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.cmbFamilias = global::Gtk.ComboBox.NewText ();
			this.cmbFamilias.WidthRequest = 200;
			this.cmbFamilias.Name = "cmbFamilias";
			this.hbox1.Add (this.cmbFamilias);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.cmbFamilias]));
			w14.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.btnAñadirFam = new global::Gtk.Button ();
			this.btnAñadirFam.CanFocus = true;
			this.btnAñadirFam.Name = "btnAñadirFam";
			this.btnAñadirFam.UseUnderline = true;
			// Container child btnAñadirFam.Gtk.Container+ContainerChild
			global::Gtk.Alignment w15 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w16 = new global::Gtk.HBox ();
			w16.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w17 = new global::Gtk.Image ();
			w17.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w16.Add (w17);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w19 = new global::Gtk.Label ();
			w16.Add (w19);
			w15.Add (w16);
			this.btnAñadirFam.Add (w15);
			this.hbox1.Add (this.btnAñadirFam);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.btnAñadirFam]));
			w23.Position = 1;
			w23.Expand = false;
			w23.Fill = false;
			this.vboxFamilias.Add (this.hbox1);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vboxFamilias [this.hbox1]));
			w24.Position = 1;
			w24.Expand = false;
			w24.Fill = false;
			this.pneDatosArt.Add (this.vboxFamilias);
			global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.pneDatosArt [this.vboxFamilias]));
			w25.XOptions = ((global::Gtk.AttachOptions)(4));
			w25.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child pneDatosArt.Gtk.Table+TableChild
			this.vboxIDArt = new global::Gtk.VBox ();
			this.vboxIDArt.Name = "vboxIDArt";
			this.vboxIDArt.Spacing = 6;
			// Container child vboxIDArt.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Ref Articulo");
			this.vboxIDArt.Add (this.label2);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vboxIDArt [this.label2]));
			w26.Position = 0;
			w26.Expand = false;
			w26.Fill = false;
			// Container child vboxIDArt.Gtk.Box+BoxChild
			this.txtID = new global::Gtk.SpinButton (0, 1E+25, 1);
			this.txtID.CanFocus = true;
			this.txtID.Name = "txtID";
			this.txtID.Adjustment.PageIncrement = 10;
			this.txtID.ClimbRate = 1;
			this.txtID.Numeric = true;
			this.vboxIDArt.Add (this.txtID);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vboxIDArt [this.txtID]));
			w27.Position = 1;
			w27.Expand = false;
			w27.Fill = false;
			this.pneDatosArt.Add (this.vboxIDArt);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.pneDatosArt [this.vboxIDArt]));
			w28.TopAttach = ((uint)(1));
			w28.BottomAttach = ((uint)(2));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child pneDatosArt.Gtk.Table+TableChild
			this.vboxNombre = new global::Gtk.VBox ();
			this.vboxNombre.Name = "vboxNombre";
			this.vboxNombre.Spacing = 6;
			// Container child vboxNombre.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 0F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Nombre");
			this.vboxNombre.Add (this.label3);
			global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.vboxNombre [this.label3]));
			w29.Position = 0;
			w29.Expand = false;
			w29.Fill = false;
			// Container child vboxNombre.Gtk.Box+BoxChild
			this.txtNombre = new global::Gtk.Entry ();
			this.txtNombre.CanFocus = true;
			this.txtNombre.Name = "txtNombre";
			this.txtNombre.IsEditable = true;
			this.txtNombre.InvisibleChar = '●';
			this.vboxNombre.Add (this.txtNombre);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.vboxNombre [this.txtNombre]));
			w30.Position = 1;
			w30.Expand = false;
			w30.Fill = false;
			this.pneDatosArt.Add (this.vboxNombre);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.pneDatosArt [this.vboxNombre]));
			w31.TopAttach = ((uint)(1));
			w31.BottomAttach = ((uint)(2));
			w31.LeftAttach = ((uint)(1));
			w31.RightAttach = ((uint)(2));
			w31.XOptions = ((global::Gtk.AttachOptions)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vboxPrincipal.Add (this.pneDatosArt);
			global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.pneDatosArt]));
			w32.Position = 0;
			w32.Expand = false;
			w32.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.grpModos = new global::Gtk.HButtonBox ();
			this.grpModos.Name = "grpModos";
			this.grpModos.Spacing = 14;
			this.grpModos.BorderWidth = ((uint)(6));
			this.grpModos.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnBuscar = new global::Gtk.Button ();
			this.btnBuscar.CanFocus = true;
			this.btnBuscar.Name = "btnBuscar";
			this.btnBuscar.UseUnderline = true;
			// Container child btnBuscar.Gtk.Container+ContainerChild
			global::Gtk.Alignment w33 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w34 = new global::Gtk.HBox ();
			w34.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w35 = new global::Gtk.Image ();
			w35.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-find", global::Gtk.IconSize.Menu);
			w34.Add (w35);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w37 = new global::Gtk.Label ();
			w37.LabelProp = global::Mono.Unix.Catalog.GetString ("Buscar");
			w37.UseUnderline = true;
			w34.Add (w37);
			w33.Add (w34);
			this.btnBuscar.Add (w33);
			this.grpModos.Add (this.btnBuscar);
			global::Gtk.ButtonBox.ButtonBoxChild w41 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnBuscar]));
			w41.Expand = false;
			w41.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnSalir = new global::Gtk.Button ();
			this.btnSalir.CanFocus = true;
			this.btnSalir.Name = "btnSalir";
			this.btnSalir.UseStock = true;
			this.btnSalir.UseUnderline = true;
			this.btnSalir.Label = "gtk-ok";
			this.grpModos.Add (this.btnSalir);
			global::Gtk.ButtonBox.ButtonBoxChild w42 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnSalir]));
			w42.Position = 1;
			w42.Expand = false;
			w42.Fill = false;
			this.vboxPrincipal.Add (this.grpModos);
			global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.grpModos]));
			w43.Position = 1;
			w43.Expand = false;
			w43.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.lblInformacion = new global::Gtk.Label ();
			this.lblInformacion.Name = "lblInformacion";
			this.lblInformacion.Xalign = 0F;
			this.lblInformacion.LabelProp = global::Mono.Unix.Catalog.GetString ("Intruduzca los datos y pulse salir");
			this.vboxPrincipal.Add (this.lblInformacion);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.lblInformacion]));
			w44.Position = 2;
			w44.Expand = false;
			w44.Fill = false;
			this.Add (this.vboxPrincipal);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 659;
			this.DefaultHeight = 336;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.txtNombre.Changed += new global::System.EventHandler (this.OnTxtNombreChanged);
			this.txtNombre.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnTxtNombreFocusOutEvent);
			this.txtID.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnTxtIDFocusOutEvent);
			this.txtID.Changed += new global::System.EventHandler (this.OnTxtIDChanged);
			this.cmbFamilias.Changed += new global::System.EventHandler (this.OnCmbFamiliasChanged);
			this.btnAñadirFam.Clicked += new global::System.EventHandler (this.OnBtnAñadirFamClicked);
			this.btnBuscar.Clicked += new global::System.EventHandler (this.OnBtnBuscarClicked);
			this.btnSalir.Clicked += new global::System.EventHandler (this.OnBtnSalirClicked);
		}
	}
}
