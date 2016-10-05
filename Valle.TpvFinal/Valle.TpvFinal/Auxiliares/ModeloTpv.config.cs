using System;
using System.Data;
using System.Drawing;
using Valle.TpvFinal.Tools;

using Valle.Distribuido;
using Valle.GtkUtilidades;
using Valle.Utilidades;
using Valle.SqlGestion;
using Valle.ToolsTpv;

namespace Valle.TpvFinal
{
	public partial class ModeloTpv
	{
		
		Distribuido.GesMensajes gesMen;
	  
		void CargarTpvConfig(){
	
			tbConfig.ReadXml(rutaPrincipal + "/" + RutasConfig.FICHERO_CONFIG);
			    dirBaseDatos = tbConfig.Rows[0][RutasConfig.COL_BASE_DATOS].ToString();
                rutaMesas = tbConfig.Rows[0][RutasConfig.COL_MESA].ToString();
                rutaFotos = tbConfig.Rows[0][RutasConfig.COL_FOTOS].ToString();
                rutaPlanos = tbConfig.Rows[0][RutasConfig.COL_PLANOS].ToString();
                rutaDatos = tbConfig.Rows[0][RutasConfig.COL_DATOS].ToString();
                RutasMod = (bool)tbConfig.Rows[0][RutasConfig.COL_MOD];
				esAuxiliar = (bool)tbConfig.Rows[0][RutasConfig.COL_AUXILIAR];
				sqlComunciacion.pass = tbConfig.Rows[0][RutasConfig.COL_SQL_PASS].ToString();
				sqlComunciacion.port =  tbConfig.Rows[0][RutasConfig.COL_SQL_PUERTO].ToString(); 
				sqlComunciacion.usr = tbConfig.Rows[0][RutasConfig.COL_SQL_USER].ToString();
                comunicacion.dir = tbConfig.Rows[0][RutasConfig.COL_IP].ToString();
                comunicacion.pDatos =tbConfig.Rows[0][RutasConfig.COL_PUERTO_DATOS].ToString();
                comunicacion.pMen=tbConfig.Rows[0][RutasConfig.COL_PUERTO_COMUN].ToString();
                comunicacion.protocolo = tbConfig.Rows[0][RutasConfig.COL_PROTOCOLO].ToString();
    	}
		
		void OnSalirConIni(){
		    
			this.CargarTpvConfig   ();
			
               gesMen = new  GesMensajes(Int32.Parse(comunicacion.pMen));
			   gesMen.mensajeRecibido += HandleGesMenmensajeRecibido;
			   
			   GesMySQL ges = new GesMySQL(sqlComunciacion.pass,sqlComunciacion.usr,sqlComunciacion.port);
			   //si no contiene la base de datos la creamos
			     
			    if(!ges.ContieneBaseDatos(dirBaseDatos)||ges.NumTablasBase(dirBaseDatos)<10){
			      Gtk.FileChooserDialog fc=
					new Gtk.FileChooserDialog("Fichero de esquema de datos",
					                            null,
					                            Gtk.FileChooserAction.Open,
					                            "Cancelar",Gtk.ResponseType.Cancel,
			                            "Abrir",Gtk.ResponseType.Accept);
				
    				    Gtk.FileFilter filter = new Gtk.FileFilter();
						filter.Name = "Esquema base tpv (*.esq)";
						filter.AddMimeType("binary/bin");
						filter.AddPattern("*.esq");
						fc.AddFilter(filter);
		
			
					if (fc.Run() == (int)Gtk.ResponseType.Accept) 
					{
					   new CrearBaseDatos(ges,dirBaseDatos,fc.Filename);
					    fc.Destroy();
					}
					
			    } 
                
			StartConfig();	
		}

		 void HandleGesMenmensajeRecibido (string men)
	    {
			      string[] inst = men.Split(':');
		            switch (inst[0])
		            {
		                case "Reiniciar":
		                     Gtk.Application.Invoke(delegate {
					          //Todo lo necesario para reiniciar el tpv
					              Console.WriteLine("Reiniciando tpv");
				           });
		                    break;
		                case "MesaNueva":
		                   mesas.TodosBtnMesas[inst[1]].colorDeAtras = Color.Purple;
		                break;
		                 case "MesaCerrada":
		                  mesas.TodosBtnMesas[inst[1]].colorDeAtras =
		                                     Color.FromArgb(Int32.Parse(inst[2]));
		                  break;
		            }
    	    }
		
		void StartConfig(){
		    splashin = new Splash("",rutaPrincipal+"/iconos/ImgSpl.png",false);
			splashin.Show();
			splashin.MaxProceso = 100;
			System.Threading.Thread h = new System.Threading.Thread(new System.Threading.ThreadStart(ConfigurarTpv));
			h.Start();
		}
		
		void ConfigurarTpv(){
			
		    this.CargarTpvConfig();
			
			if(!esAuxiliar){
					gestorBase = new GesMySQL(dirBaseDatos,sqlComunciacion.pass,sqlComunciacion.usr,sqlComunciacion.port);
				new RegistrarServidor(gestorBase,Convert.ToInt32(comunicacion.pDatos),comunicacion.protocolo,this.rutaMesas);
				
				HayDatos = !((gestorBase.NumRegEnTabla("ZonasTpv")<=0)||(gestorBase.NumRegEnTabla("FavoritosTpv")<=0)||
                                 (gestorBase.NumRegEnTabla("SeccionesTpv")<=0)||(gestorBase.NumRegEnTabla("Teclas")<=0)||
                                 (gestorBase.NumRegEnTabla("TeclasFav")<=0));
		        if(!HayDatos){
           
	            	 Gtk.Application.Invoke(delegate{
						
						splashin.Destroy();
	          
	                      	new DialogoTactil("No hay datos en el tpv",
	            	                                          "No tiene datos necesarios para el arranque. Cree los datos desde la gestion del TPV, "+
	            	                                          "exporte estos datos y reinicie el TPV ",TipoRes.aceptar);
	            
	                     MostrarIconoBandeja();
					});
                  }else{
                    
					   splashin.mostrarInformacion("Comprobando base de datos..");
			           splashin.Progreso = 5;
				
					   
					   prepararBaseDatos();
					
					   splashin.mostrarInformacion("Generando controles y teclados ....");
					   int progreso = 20;
						                
					    genArt = new GeneradorDeArticulos(this.idTpv,this.numBotonesPag,progreso,this.gestorBase,splashin);
	                    
					    //inciciar ClaseAuxiliares y variables de configuracion
			            iniciarClasesAux(progreso);
			        
					   
			           splashin.mostrarInformacion("Iniciando tpv...");
					 
				       splashin.Progreso = 100;
					  
					   
					
					   Gtk.Application.Invoke (delegate {
						        System.Threading.Thread.Sleep(500);
						          splashin.Destroy();
						          this.IniciarTpv();
					     });
						
		                
                  }
			}
		}
		
        private void iniciarClasesAux(int progreso)
        {
		  
		    splashin.mostrarProgreso(75);	
			splashin.mostrarInformacion("Cargando el tpv...");
			 
			Gtk.Application.Invoke(delegate{
			
    		  guardarTicket = new GuardarTicket(gestorBase);
			  mesas = new GesMesas(gestorBase,comunicacion.pDatos,comunicacion.protocolo,guardarTicket,rutaMesas,gesMen);
			  camareros = new GesCamareros(gestorBase,16);
			  
			  
			  tpv = new Tpv(esTemporizado,tiempoFormAc);
			  tpv.Title = "ValleTPV El TPV de ValleSoft";
			  tpv.eventTpv += HandleTpveventTpv;
			  tpv.MostrarTeclado(genArt.PagFav);
			  tpv.mostrarVarios(this.MostrarVarios);
			  tpv.mostrarVariosConNombre(this.MostrarVariosConNombre);
			 
			  	
			  	  
				mesas.CrearBotonera(idTpv);
				
				wSom =	new Gtk.Window(Gtk.WindowType.Toplevel);
				wSom.AppPaintable = true;
				wSom.WindowPosition = Gtk.WindowPosition.CenterAlways;
			    wSom.SkipTaskbarHint = true;
				wSom.AcceptFocus = false;
				wSom.Visible=false;
				wSom.TransientFor = tpv;
				wSom.Decorated = false;
				wSom.ExposeEvent+= delegate {
					int width=0,height=0;
					wSom.GetSize(out width,out height); 
					  Cairo.Context c = Gdk.CairoHelper.Create(wSom.GdkWindow);
					   c.Color = new Cairo.Color(0.3,0.3,0.8,0.6);
					   c.Rectangle(0,0,width,height);
					   c.PaintWithAlpha(0.6);
					
					((IDisposable)c).Dispose();
				};
				
				wSom.Maximize();
					
			});
				
		    System.Threading.Thread.Sleep(500);
		    splashin.mostrarInformacion("Creando controles auxiliares");
            splashin.mostrarProgreso(85);

            Gtk.Application.Invoke(delegate{
			
			#region controles auxiliares
            // el listado de cierres
            listadoCierres = new ListadoCierres(gestorBase, idTpv,nomImpresora,rutaDatos);
            listadoCierres.salirListadoCierres += HandleListadoCierressalirListadoCierres; 
			listadoCierres.TransientFor = wSom;
			listadoCierres.Modal = true;
				listadoCierres.Shown += delegate {
					  	lock (wSom) {wSom.Show();}
				       };
					 listadoCierres.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
				
			});
		
		   
			System.Threading.Thread.Sleep(500);
		    splashin.mostrarInformacion("Finalizando ....");
            splashin.mostrarProgreso(95);
          
		Gtk.Application.Invoke(delegate{
			//Teclados para elegir camarero
			    elegirCam = new VenBotones();
				elegirCam.clickBoton += HandleElegirCamclickBoton;
	            elegirCam.Titulo = "Añadir camareros";
			    elegirCam.SalirAlPulsar = false;
				elegirCam.MostrarMas = true;
				elegirCam.Redimensionar(4,4);
			    elegirCam.PaginasBtn = camareros.pagCamareros;
				elegirCam.OcultarSolo = true;
				elegirCam.TransientFor = wSom;
				elegirCam.Modal = true;
				elegirCam.Shown += delegate {
						lock (wSom) {wSom.Show();}
				};
			   
				//botonera de camareros
			    	botoneraCam = new VenBotones();
		            botoneraCam.Titulo = "Elegir camarero";
					botoneraCam.clickBoton+= HandleBotoneraCamclickBoton;
				    botoneraCam.SalirAlPulsar = true;
				    botoneraCam.MostrarSalir = false;
				    botoneraCam.OcultarSolo = true;
				    botoneraCam.TransientFor = wSom;
				    botoneraCam.Modal = true;
				    botoneraCam.Shown += delegate {
					     wSom.Show();
				       };
				   
				
			//Botonera de selector de mesas	
				botoneraMesas = new  SelectorDeMesas(mesas);
                botoneraMesas.Titulo = "Elegir mesa";
			    botoneraMesas.selectorClick+= HandleBotoneraMesasselectorClick;
				botoneraMesas.OcultarSolo = true;
				botoneraMesas.SalirAlPulsar = true;
				botoneraMesas.TransientFor = wSom;
				botoneraMesas.Shown += delegate {
					lock (wSom) {wSom.Show();}
				     };
				botoneraMesas.Hidden +=  delegate {
					   if(!botoneraMesas.TimeOut && !botoneraMesas.Saliendo) wSom.Hide();
					};
				   
				
		   //Botonera de secciones 
			  secciones = new VenSecciones(4);
			  secciones.clickBoton += HandleSeccionesclickBoton;
			  secciones.MostrarMas = false;	
			  secciones.PaginasBtn = genArt.BtnsSecciones;	
			  secciones.TransientFor = wSom;
			  secciones.Modal = true;
				secciones.Shown += delegate {
					 	lock (wSom) {wSom.Show();}
				       };
					 secciones.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
				
				
			//teclados numericos y alfabeticos
        	//teclado alfavetico
            tclAlf = new TecladoAlfavetico(TipoDeTeclado.Tipo_Alfabetico);
			tclAlf.EjAccion += HandleTclAlfEjAccion;
			tclAlf.TransientFor = wSom;
			tclAlf.Modal = true;
			tclAlf.Shown += delegate {
					 	lock (wSom) {wSom.Show();}
				       };
             tclAlf.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
					
				
          
			//teclado para buscar articulos
            buscador = new TecladoAlfavetico(TipoDeTeclado.Tipo_Buscador);
            buscador.EjAccion +=  HandleBuscadorEjAccion;
            buscador.SalirAlPulsar = false;
				
            
			//Teclados numericos decimal y entero
            tclNumDecimal = new TecladoNumerico(FormatosNumericos.F_Decimal, "");
			tclNumDecimal.salirNumerico+= HandleTclNumDecimalsalirNumerico;
		    tclNumDecimal.TransientFor = wSom;
			tclNumDecimal.Modal = true;
			tclNumDecimal.Shown += delegate {
					 	lock (wSom) {wSom.Show();}
				       };
					 tclNumDecimal.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
					
          
			//crear herramientas
            herramientas = new Herramientas(puedoImprimir);
			herramientas.EjAccion += HandleHerramientasEjAccion;
		    herramientas.TransientFor = wSom;
				herramientas.Shown += delegate {
					 	lock (wSom) {wSom.Show();}
				       };
					 herramientas.Hidden +=  delegate {
					   if(herramientas.acion != AccionesHerramientas.CambiarTpv)	lock (wSom) {wSom.Hide();}
					};
				
				
           
			//herramientas de desglose
            HerDesglose = new DesgloseTicket();
            HerDesglose.EjAccion += HandleHerDesgloseEjAccion;
            HerDesglose.SalirAlPulsar = true;
			HerDesglose.TransientFor = wSom;
				HerDesglose.Shown+= delegate {
						wSom.Show();
					};
				HerDesglose.Hidden+= delegate {
						wSom.Hide();
					};	
            
			HerDividir = new DividirTiket();
            HerDividir.EjAccion += HandleHerDividirEjAccion;
			HerDividir.TransientFor = wSom;
			   HerDividir.Shown += delegate {
					  	lock (wSom) {wSom.Show();}
				       };
					 HerDividir.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
					
			
           
			//Separar ticket
            separarArticulos = new HerRapidasArticulos(TipoControlHerrmientaRapida.Separar);
            separarArticulos.EjAccion +=  HandleSepararArticulosEjAccion;
			separarArticulos.TransientFor = wSom;
				separarArticulos.Shown += delegate {
					  	lock (wSom) {wSom.Show();}
				       };
					
				
           //botonera Tpv
		    this.botoneraTpv = new VenBotones();
			this.botoneraTpv.MostrarMas = false;
			this.botoneraTpv.clickBoton += HandleBotoneraTpvclickBoton;
				botoneraTpv.TransientFor = wSom;
				botoneraTpv.Shown += delegate {
						  if(esTemporizado) lock (wSom) {wSom.Show();}
				       };
					 botoneraTpv.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
				
				
			//llenar iden
            llenarIden = new HerRapidasArticulos(TipoControlHerrmientaRapida.llenadoIden);
			llenarIden.TransientFor = wSom;
            llenarIden.EjAccion += HandleLlenarIdenEjAccion;
			    llenarIden.Shown += delegate {
					  if(esTemporizado)	lock (wSom) {wSom.Show();}
				       };
					 llenarIden.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
				
             
			//Invitar una mesa a otra
            invitar = new HerRapidasArticulos(TipoControlHerrmientaRapida.Invitacion);
		    invitar.TransientFor = wSom;
            invitar.EjAccion += HandleInvitarEjAccion;
				invitar.Shown += delegate {
					  if(esTemporizado)	lock (wSom) {wSom.Show();}
				       };
					 invitar.Hidden +=  delegate {
					   	lock (wSom) {wSom.Hide();}
					};
				
             
			});
			splashin.mostrarProgreso(100);
			System.Threading.Thread.Sleep(500);
            
			
		Gtk.Application.Invoke(delegate{	
			
				//crear dialogo de impresion
				
		    impTic = new ImprimirTicket(gestorBase, idTpv,rutaDatos,nomImpresora);
			impTic.TransientFor = wSom;
			impTic.Shown += delegate {
					if(esTemporizado)	lock (wSom) {wSom.Show();}
				       };
					 impTic.Hidden +=  delegate {
					   tpv.VolverTpv();
					   	lock (wSom) {wSom.Hide();}
					};
					
			  	
			
			
				//Ticket 
            infTicketCobrado = new TicketCobrado(4, 4);
            infTicketCobrado.OcultarSolo = true; 
		    infTicketCobrado.TransientFor = wSom;
			infTicketCobrado.KeepAbove = true;
		
				//ElegirMonedas
		    elegirMonedas = new ElegirMoneda();
			elegirMonedas.SalirElegirMonedas+= HandleElegirMonedasSalirElegirMonedas;
				elegirMonedas.TransientFor = wSom;
				elegirMonedas.Modal = true;
				elegirMonedas.Shown += delegate {
						lock (wSom) {wSom.Show();
					      }
				       };
				elegirMonedas.Hidden += delegate {
						  if (!esTemporizado) lock (wSom) {wSom.Hide();
					      }
				       };
			});
			
			
            #endregion
		}

        
         
		
		private void prepararBaseDatos()
        {
            
            //cargar tpvs 
             splashin.Progreso=10;
				
           
            #region cargar Configuracion
            //cargar configuracion instantanea
            DataTable tbConfiguracion = gestorBase.ExtraerTabla("Configuracion");
            DataRow[] tpvActivo = tbConfiguracion.Select("Activo = True");
            DataRow rA;
              if (tpvActivo.Length > 0)
                {
                    this.idTpv = (int)tpvActivo[0]["IDtpv"];
                    tpvActivo[0]["Activo"] = false;
                    gestorBase.EjConsultaNoSelect("Configuracion",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(tpvActivo[0],Valle.SqlUtilidades.AccionesConReg.Modificar,
				                                                                "IdVinculacion = "+tpvActivo[0]["IdVinculacion"].ToString()).Replace(@"\",@"\\"));
			 
                    rA=tpvActivo[0];
                }
                else
                {
                   this.idTpv = new CargarIDPrometedor(tbConfiguracion).IDPrometedor;
                   rA = tbConfiguracion.Select("IDTpv = " + idTpv)[0];
                }
           
            this.tiempoFormAc = ((int)rA["TiempoFormAc"])*1000;
            this.tiempoFormNoAc = ((int)rA["TiempoFormNoAc"])*1000;
            this.puedoImprimir = (bool)rA["ImprimirAutomatico"];
            this.esTemporizado = (bool)rA["IdentificacionPrimero"];
            this.MostrarVarios = (bool)rA["mostrarVarios"];
            this.MostrarVariosConNombre = (bool)rA["mostrarVariosConNombre"];
            //this.esBloqueado = rA["Bloqueado"].ToString().Length > 0 ?
            //Valle.Seguridad.Encriptar.DescriptarCadena(rA["Bloqueado"].ToString()) : "";
           
            #endregion cargar Configuracion
           
			splashin.Progreso=15;
			
           if(!esAuxiliar){
            #region para las rutas
            if(this.RutasMod){
             gestorBase.EjConsultaNoSelect("Rutas", "DELETE FROM Rutas");
                DataTable rutas = gestorBase.ExtraerTabla("Rutas");
                DataRow r = rutas.NewRow ();
                r["identificacion"] = RutasConfig.NOM_DIR_DATOS;
                r["Ruta"] = tbConfig.Rows[0][RutasConfig.COL_BASE_DATOS].ToString();
                rutas.Rows.Add(r);
                 r = rutas.NewRow ();
                r["identificacion"] = RutasConfig.NOM_DIR_DATOS;
                r["Ruta"] = this.rutaDatos;
                rutas.Rows.Add(r);
                r = rutas.NewRow ();
                r["identificacion"] = RutasConfig.NOM_DIR_FOTOS;
                r["Ruta"] = this.rutaFotos;
                rutas.Rows.Add(r);
                r = rutas.NewRow ();
                r["identificacion"] = RutasConfig.NOM_DIR_PLANOS;
                r["Ruta"] = this.rutaPlanos;
                rutas.Rows.Add(r);
                r = rutas.NewRow ();
                r["identificacion"] = RutasConfig.NOM_DIR_MESAS;
                r["Ruta"] = this.rutaMesas;
                rutas.Rows.Add(r);
				gestorBase.ModificarReg(r,Valle.SqlUtilidades.AccionesConReg.Agregar,"");
				tbConfig.Rows[0][RutasConfig.COL_MOD] = this.RutasMod = false;
                tbConfig.WriteXml(rutaPrincipal + "/" + RutasConfig.FICHERO_CONFIG,XmlWriteMode.WriteSchema); 
                
            }
		       
            #endregion
              
				splashin.Progreso=20;
				
            //para saber el numero de ticket por el que va
            object numObTicket = gestorBase.EjEscalar("SELECT MAX(NumTicket) FROM Ticket");
            if(!numObTicket.GetType().Name.Equals("DBNull"))
                numTicket = (int)numObTicket+1;
                else
                 numTicket = 0;
          }
        }
				
		
		
		void ComprobarPrimerUso(){
		  tbConfig = new DataTable(RutasConfig.NOMBRE_TB_CONFIG);
             
            try {

                tbConfig.ReadXml(rutaPrincipal + "/" + RutasConfig.FICHERO_CONFIG);
				StartConfig();
            }
            catch 
            {
                new CrearConfiguracionIni(OnSalirConIni);
             }
            
		}

	    
				

		
		#region Icono de bandeja 
		void MostrarIconoBandeja(){
			    // Creation of the Icon
			trayIcon = new Gtk.StatusIcon(new Gdk.Pixbuf (Valle.Utilidades.RutasArchivos.Ruta_Completa("iconos/tpv.ico")));
			trayIcon.Visible = true;
	 
			// Show a pop up menu when the icon has been right clicked.
			trayIcon.PopupMenu += OnTrayIconPopup;
	 
			// A Tooltip for the Icon
			trayIcon.Tooltip = "ValleTpv esperando datos";
	
		}
		
		// Create the popup menu, on right click.
	   void OnTrayIconPopup (object o, EventArgs args) {
			Gtk.Menu popupMenu = new Gtk.Menu();
			Gtk.ImageMenuItem menuItem = new Gtk.ImageMenuItem ("Salir");
			Gtk.Image appimg = new Gtk.Image(Gtk.Stock.Quit, Gtk.IconSize.Menu);
			menuItem.Image = appimg;
			popupMenu.Add(menuItem);
			// Quit the application when quit has been clicked.
			menuItem.Activated += delegate { Gtk.Application.Quit(); };
			menuItem = new Gtk.ImageMenuItem("Restarurar aplicacion");
			appimg = new Gtk.Image(Gtk.Stock.Refresh, Gtk.IconSize.Menu);
			menuItem.Image = appimg;
			popupMenu.Add(menuItem);
			menuItem.Activated += delegate { 
				if(tpv!=null) {
					trayIcon.Dispose();
					tpv.Show();
					tpv.VolverTpv();
				}
			};
			
			menuItem.Sensitive = HayDatos;
			
			popupMenu.ShowAll();
			popupMenu.Popup();
	     }
     
		#endregion	
	}
}

