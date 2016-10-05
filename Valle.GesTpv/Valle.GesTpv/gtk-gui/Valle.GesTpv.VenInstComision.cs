
// This file has been generated by the GUI designer. Do not modify.
namespace Valle.GesTpv
{
	public partial class VenInstComision
	{
		private global::Gtk.VBox vboxPrincipal;
		private global::Gtk.HBox hbox1;
		private global::Gtk.VBox vbox2;
		private global::Gtk.Label label2;
		private global::Gtk.ComboBox cmbCamareros;
		private global::Gtk.VBox vbox3;
		private global::Gtk.Label lblDesde;
		private global::Gtk.SpinButton txtPorcentaje;
		private global::Gtk.VBox vbox4;
		private global::Gtk.Label label5;
		private global::Gtk.SpinButton txtTarifa;
		private global::Gtk.HBox hbox2;
		private global::Valle.GtkUtilidades.TxtHoras txthorasInicio;
		private global::Valle.GtkUtilidades.TxtHoras txthorasFin;
		private global::Valle.GtkUtilidades.ArbolDeVista ListaCamareros;
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
			// Widget Valle.GesTpv.VenInstComision
			this.Name = "Valle.GesTpv.VenInstComision";
			this.Title = global::Mono.Unix.Catalog.GetString ("Gestion de comisiones");
			this.WindowPosition = ((global::Gtk.WindowPosition)(3));
			this.Modal = true;
			// Container child Valle.GesTpv.VenInstComision.Gtk.Container+ContainerChild
			this.vboxPrincipal = new global::Gtk.VBox ();
			this.vboxPrincipal.Name = "vboxPrincipal";
			this.vboxPrincipal.Spacing = 6;
			this.vboxPrincipal.BorderWidth = ((uint)(9));
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.vbox2.BorderWidth = ((uint)(3));
			// Container child vbox2.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 0F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Nombre camarero");
			this.vbox2.Add (this.label2);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.label2]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.cmbCamareros = global::Gtk.ComboBox.NewText ();
			this.cmbCamareros.Name = "cmbCamareros";
			this.vbox2.Add (this.cmbCamareros);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.cmbCamareros]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			this.hbox1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
			w3.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			this.vbox3.BorderWidth = ((uint)(3));
			// Container child vbox3.Gtk.Box+BoxChild
			this.lblDesde = new global::Gtk.Label ();
			this.lblDesde.Name = "lblDesde";
			this.lblDesde.Xalign = 0F;
			this.lblDesde.LabelProp = global::Mono.Unix.Catalog.GetString ("Porcentaje comision");
			this.vbox3.Add (this.lblDesde);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.lblDesde]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.txtPorcentaje = new global::Gtk.SpinButton (0, 100, 1);
			this.txtPorcentaje.CanFocus = true;
			this.txtPorcentaje.Name = "txtPorcentaje";
			this.txtPorcentaje.Adjustment.PageIncrement = 10;
			this.txtPorcentaje.ClimbRate = 1;
			this.txtPorcentaje.Numeric = true;
			this.vbox3.Add (this.txtPorcentaje);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.txtPorcentaje]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			this.hbox1.Add (this.vbox3);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox3]));
			w6.Position = 1;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			this.vbox4.BorderWidth = ((uint)(3));
			// Container child vbox4.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 0F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Tarifa");
			this.vbox4.Add (this.label5);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.label5]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.txtTarifa = new global::Gtk.SpinButton (0, 3, 1);
			this.txtTarifa.CanFocus = true;
			this.txtTarifa.Name = "txtTarifa";
			this.txtTarifa.Adjustment.PageIncrement = 10;
			this.txtTarifa.ClimbRate = 1;
			this.txtTarifa.Numeric = true;
			this.vbox4.Add (this.txtTarifa);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.txtTarifa]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			this.hbox1.Add (this.vbox4);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox4]));
			w9.Position = 2;
			this.vboxPrincipal.Add (this.hbox1);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.hbox1]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.txthorasInicio = new global::Valle.GtkUtilidades.TxtHoras ();
			this.txthorasInicio.Events = ((global::Gdk.EventMask)(256));
			this.txthorasInicio.Name = "txthorasInicio";
			this.txthorasInicio.Etiqueta = "Hora inicio";
			this.hbox2.Add (this.txthorasInicio);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.txthorasInicio]));
			w11.Position = 0;
			w11.Expand = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.txthorasFin = new global::Valle.GtkUtilidades.TxtHoras ();
			this.txthorasFin.Events = ((global::Gdk.EventMask)(256));
			this.txthorasFin.Name = "txthorasFin";
			this.txthorasFin.Etiqueta = "Hora finalizacion";
			this.hbox2.Add (this.txthorasFin);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.txthorasFin]));
			w12.Position = 1;
			w12.Expand = false;
			this.vboxPrincipal.Add (this.hbox2);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.hbox2]));
			w13.Position = 1;
			w13.Expand = false;
			w13.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.ListaCamareros = new global::Valle.GtkUtilidades.ArbolDeVista ();
			this.ListaCamareros.HeightRequest = 250;
			this.ListaCamareros.Events = ((global::Gdk.EventMask)(256));
			this.ListaCamareros.Name = "ListaCamareros";
			this.vboxPrincipal.Add (this.ListaCamareros);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.ListaCamareros]));
			w14.Position = 2;
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
			global::Gtk.Alignment w15 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w16 = new global::Gtk.HBox ();
			w16.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w17 = new global::Gtk.Image ();
			w17.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "stock_effects", global::Gtk.IconSize.Menu);
			w16.Add (w17);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w19 = new global::Gtk.Label ();
			w19.LabelProp = global::Mono.Unix.Catalog.GetString ("Editar");
			w19.UseUnderline = true;
			w16.Add (w19);
			w15.Add (w16);
			this.btnEditar.Add (w15);
			this.grpModos.Add (this.btnEditar);
			global::Gtk.ButtonBox.ButtonBoxChild w23 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnEditar]));
			w23.Expand = false;
			w23.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnAñadir = new global::Gtk.Button ();
			this.btnAñadir.CanFocus = true;
			this.btnAñadir.Name = "btnAñadir";
			this.btnAñadir.UseUnderline = true;
			// Container child btnAñadir.Gtk.Container+ContainerChild
			global::Gtk.Alignment w24 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w25 = new global::Gtk.HBox ();
			w25.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w26 = new global::Gtk.Image ();
			w26.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w25.Add (w26);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w28 = new global::Gtk.Label ();
			w28.LabelProp = global::Mono.Unix.Catalog.GetString ("Añadir");
			w28.UseUnderline = true;
			w25.Add (w28);
			w24.Add (w25);
			this.btnAñadir.Add (w24);
			this.grpModos.Add (this.btnAñadir);
			global::Gtk.ButtonBox.ButtonBoxChild w32 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnAñadir]));
			w32.Position = 1;
			w32.Expand = false;
			w32.Fill = false;
			// Container child grpModos.Gtk.ButtonBox+ButtonBoxChild
			this.btnBorrar = new global::Gtk.Button ();
			this.btnBorrar.CanFocus = true;
			this.btnBorrar.Name = "btnBorrar";
			this.btnBorrar.UseUnderline = true;
			// Container child btnBorrar.Gtk.Container+ContainerChild
			global::Gtk.Alignment w33 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w34 = new global::Gtk.HBox ();
			w34.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w35 = new global::Gtk.Image ();
			w35.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-delete", global::Gtk.IconSize.Menu);
			w34.Add (w35);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w37 = new global::Gtk.Label ();
			w37.LabelProp = global::Mono.Unix.Catalog.GetString ("Borrar art");
			w37.UseUnderline = true;
			w34.Add (w37);
			w33.Add (w34);
			this.btnBorrar.Add (w33);
			this.grpModos.Add (this.btnBorrar);
			global::Gtk.ButtonBox.ButtonBoxChild w41 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos [this.btnBorrar]));
			w41.Position = 2;
			w41.Expand = false;
			w41.Fill = false;
			this.vboxPrincipal.Add (this.grpModos);
			global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.grpModos]));
			w42.Position = 3;
			w42.Expand = false;
			w42.Fill = false;
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
			global::Gtk.ButtonBox.ButtonBoxChild w43 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos1 [this.btnAceptar]));
			w43.Expand = false;
			w43.Fill = false;
			// Container child grpModos1.Gtk.ButtonBox+ButtonBoxChild
			this.btnSalir = new global::Gtk.Button ();
			this.btnSalir.CanFocus = true;
			this.btnSalir.Name = "btnSalir";
			this.btnSalir.UseUnderline = true;
			// Container child btnSalir.Gtk.Container+ContainerChild
			global::Gtk.Alignment w44 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w45 = new global::Gtk.HBox ();
			w45.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w46 = new global::Gtk.Image ();
			w46.Pixbuf = new global::Gdk.Pixbuf (global::System.IO.Path.Combine (global::System.AppDomain.CurrentDomain.BaseDirectory, "./Iconos/EXIT00C.ICO"));
			w45.Add (w46);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w48 = new global::Gtk.Label ();
			w48.LabelProp = global::Mono.Unix.Catalog.GetString ("Salir");
			w48.UseUnderline = true;
			w45.Add (w48);
			w44.Add (w45);
			this.btnSalir.Add (w44);
			this.grpModos1.Add (this.btnSalir);
			global::Gtk.ButtonBox.ButtonBoxChild w52 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.grpModos1 [this.btnSalir]));
			w52.Position = 1;
			w52.Expand = false;
			w52.Fill = false;
			this.vboxPrincipal.Add (this.grpModos1);
			global::Gtk.Box.BoxChild w53 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.grpModos1]));
			w53.Position = 4;
			w53.Expand = false;
			w53.Fill = false;
			// Container child vboxPrincipal.Gtk.Box+BoxChild
			this.lblInformacion = new global::Gtk.Label ();
			this.lblInformacion.Name = "lblInformacion";
			this.lblInformacion.Xalign = 0F;
			this.lblInformacion.LabelProp = global::Mono.Unix.Catalog.GetString ("Modo no editable activado");
			this.vboxPrincipal.Add (this.lblInformacion);
			global::Gtk.Box.BoxChild w54 = ((global::Gtk.Box.BoxChild)(this.vboxPrincipal [this.lblInformacion]));
			w54.Position = 5;
			w54.Expand = false;
			w54.Fill = false;
			this.Add (this.vboxPrincipal);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 551;
			this.DefaultHeight = 581;
			this.btnAceptar.Hide ();
			this.Show ();
			this.ListaCamareros.SeleccionCambiada += new global::System.EventHandler (this.OnLstTablaCursorChanged);
			this.btnEditar.Clicked += new global::System.EventHandler (this.OnBtnEditarClicked);
			this.btnAñadir.Clicked += new global::System.EventHandler (this.OnBtnAñadirClicked);
			this.btnBorrar.Clicked += new global::System.EventHandler (this.OnBtnBorrarClicked);
			this.btnAceptar.Clicked += new global::System.EventHandler (this.OnBtnAceptarClicked);
			this.btnSalir.Clicked += new global::System.EventHandler (this.OnBtnSalirClicked);
		}
	}
}