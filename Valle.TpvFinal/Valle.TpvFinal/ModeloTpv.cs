		
using System;
using System.Collections.Generic;
using System.Timers;
using System.Data;

using Valle.TpvFinal.Tools;
using Valle.SqlGestion;
using Valle.GtkUtilidades;
using Valle.ToolsTpv;



namespace Valle.TpvFinal
{
	public partial class ModeloTpv
	{
		
		//Formularios 
		  Splash splashin;
		  Gtk.StatusIcon trayIcon;
	      Tpv tpv ;	
	      VenBotones secciones;
          SelectorDeMesas botoneraMesas;
          VenBotones botoneraCam;
          VenBotones elegirCam;
          VenBotones botoneraTpv;
          TecladoNumerico tclNumDecimal;
          TecladoAlfavetico tclAlf;
		  Herramientas herramientas;
          ImprimirTicket impTic;
          TicketCobrado infTicketCobrado;
          DesgloseTicket HerDesglose;
          DividirTiket HerDividir;
          HerRapidasArticulos separarArticulos;
          HerRapidasArticulos llenarIden;
          HerRapidasArticulos invitar;
          TecladoAlfavetico buscador;
          ListadoCierres listadoCierres;
		  ElegirMoneda elegirMonedas;
		 
		
		
		 //referente al TPV
        int numBotonesPag = 32;
		GeneradorDeArticulos genArt;
        GesCamareros camareros;
        GesMesas mesas;
       
		
       
        GuardarTicket guardarTicket;
		
		//Ficheros y rutas
        DataTable tbConfig;
        String rutaPrincipal = RutasConfig.getRutaPrincipal();
        String rutaMesas;
        String rutaFotos;
        string rutaPlanos;
        string rutaDatos;
		string nomImpresora = "caja";
        bool RutasMod = true;
        
        //Temporizadores y variables asociadas
        int tiempoFormAc = 0;
        int tiempoFormNoAc = 0;
        
        //Variables de configuracion
        bool esAuxiliar;
        bool Reiniciando = false;
        
        struct RemComunicacion{
           public string dir;
           public string pMen;
           public string pDatos;
           public string protocolo;
        } 
		
		struct SqlComunicacion{
		   public string usr;
		   public string pass;
		   public string port;
		}
        
        RemComunicacion comunicacion;
		SqlComunicacion sqlComunciacion;
		GesMySQL  gestorBase;
     
        Boolean puedoImprimir;
        bool MostrarVarios;
        bool MostrarVariosConNombre;
       
		bool esTemporizado;
        

       
        int idTpv;
        int numTicket;
		
		string dirBaseDatos; 
		
		Gtk.Window wSom;
		
		bool HayDatos = true;
         
       	public ModeloTpv ()
		{
	     	ComprobarPrimerUso();	
		}
		
		void IniciarTpv(){
			tpv.Show();
			if(esTemporizado){
			   elegirCam.Show();
				tpv.esMesaUnica = false; 
			}else{
				this.AbrirMesaUnica();
			    tpv.esMesaUnica = true;
			}
			
		}
		
		
		
		
	    void HandleTpveventTpv (object sender, EventTpv e)
        {
				tpv.PararTiempo(true);// para el temporizador de tpv
			    if(infTicketCobrado.Visible ) infTicketCobrado.CerrarFormulario();
			
			switch(e.accion){
			case AccionesTpv.abriCajon:
				ImpresoraTicket.AbrirCajon(rutaDatos,nomImpresora);  
				tpv.VolverTpv();
				break;
				
			case AccionesTpv.tiempoEspirado:
			  Gtk.Application.Invoke(delegate{
				this.MostrarCamareros();
				});
				break;
			case  AccionesTpv.opciones:	
					herramientas.puedoImprimir = puedoImprimir;
					herramientas.Show();
					break;
				case AccionesTpv.salir:
				  	this.OnSalirAplicacion();
					break;
				case AccionesTpv.varios:
				    this.OnMostrarVenVarios();
				break;
			case AccionesTpv.llenarLinea:
				    this.mesas.Llenarlinea((Articulo)e.dato);
				break;
			case AccionesTpv.variosnombre:
				   this.OnMostrarVenVariosNombre();
				break;
			case AccionesTpv.BorrarLinea:
				   this.mesas.AnularLinea((Articulo)e.dato);
				   tpv.VolverTpv();
				break;
			case AccionesTpv.mesas:
				    mesas.GestionarMesa(tpv.NumLinesaNotas);  
		            botoneraMesas.EstablecerTemporizador(esTemporizado, this.tiempoFormAc);
		            botoneraMesas.NomCamarero = camareros.nomCamareroActivo;
		            botoneraMesas.Show();
				break;
			case AccionesTpv.herramietas:
				if(tpv.NumLinesaNotas>0){
				   HerDesglose.EstablecerTemporizador(esTemporizado, this.tiempoFormAc);
				    HerDesglose.Show();
				}
				break;
			case AccionesTpv.favoritos:
				 tpv.MostrarTeclado(genArt.PagFav);
               break;	
				
			case AccionesTpv.cobrar:
			  if(tpv.NumLinesaNotas>0){
			  	elegirMonedas.Importe = tpv.TotalFactura;
				Ticket ticket = new Ticket();
                ticket.lineas = (System.Collections.Hashtable)e.dato;
                ticket.Camarero = camareros.nomCamareroActivo;
                ticket.Mesa = mesas.NomMesaActiva;
                ticket.FechaInicio = mesas.MesaActiva.HoraApertura;
                ticket.idTpv = idTpv;
				elegirMonedas.ticket = ticket;
				elegirMonedas.Show();
				}
				break;
				
			case AccionesTpv.secciones:
			 secciones.EstablecerTemporizador(esTemporizado, this.tiempoFormAc);
                secciones.Show();
               break;
				
			case AccionesTpv.imprimir:
		      if (tpv.NumLinesaNotas>0)
	            {
	                 mesas.MesaActiva.numCopiasTicket++;
	                 ImpresoraTicket.PreimprimirTicket (rutaDatos,nomImpresora, mesas.ListaDeArticulos(), DateTime.Now,
	                 mesas.MesaActiva.mesa, mesas.MesaActiva.numCopiasTicket, tpv.TotalFactura);
					 tpv.VolverTpv();
	            }
	            else
	            {
	               // impTic.EstablecerTemporizador(esTemporizado, this.tiempoFormAc );
				    impTic.Show();
	            }
			break;
				
			case AccionesTpv.camareros:
				 this.MostrarCamareros();
				break;
				
			case AccionesTpv.buscador:
				 if (!buscador.Visible)
		            {
		                if ((infTicketCobrado != null)&&(infTicketCobrado.Visible)) { infTicketCobrado.CerrarFormulario(); }
					    tpv.PararTiempo(true);
		                buscador.Titulo = "Buscador de articulos";
		                buscador.Cadena = "";
		                buscador.EstablecerTemporizador(esTemporizado, this.tiempoFormAc);
					    buscador.SetSizeRequest(700,400);
					    buscador.Move(buscador.Screen.Width-buscador.WidthRequest-10, 10);
		                buscador.Show();
		            }
		            else
		            {
					     this.tpv.MostrarTeclado(genArt.PagFav);
                	    buscador.CerrarFormulario();
					    this.tpv.VolverTpv();
			        }
				
		 	break;
			default: 
				  tpv.VolverTpv();
			break;
			}	
			
			
        }
				                 
		void HandleElegirMonedasSalirElegirMonedas (object sender, EventoSalirElegirMonedas e)
        {
				if(!e.cancelado){
        	    
				ImpresoraTicket.AbrirCajon(rutaDatos,nomImpresora);
				elegirMonedas.ticket.Cambio = e.Cambio;
			
                
                //Actualiza la gestion de mesas y guradar el ticket
                
                if (puedoImprimir)
                {
                    ImpresoraTicket.ImprimirTicket(rutaDatos, nomImpresora, elegirMonedas.ticket.lineas.GetEnumerator(),
                        camareros.nomCamareroActivo, mesas.NomMesaActiva,
                        elegirMonedas. ticket.FechaCobrado,  numTicket, e.Importe, e.Cambio);
                }
              
				if(elegirMonedas.SepararArticulos){
				    foreach (Articulo art in (List<Articulo>)elegirMonedas.Articulos){
					    mesas.CobrarLinea(art, numTicket);	
					}
				}else			
			      	mesas.CerrarConvida();
				
				
				if(elegirMonedas.ticket.CerrarMesa){
						//actualizar apariencia de tpv
					  Mesa m = mesas.MesaActiva;
					  mesas.CerrarMesa();
					
				      if (esTemporizado)
		                {
							guardarTicket.encolarTicket(elegirMonedas.ticket);
		                    guardarTicket.encolarMesa(m);
						  	tpv.BorrarLineasNotas();
			             	botoneraMesas.EstablecerTemporizador(esTemporizado,this.tiempoFormAc);
				            botoneraMesas.Show();
				
		                 }
		                 else
		                 {
							guardarTicket.guardarTicket(elegirMonedas.ticket);
		                    this.AbrirMesaUnica();
					     }		
					   
					    
				}else{
				   guardarTicket.guardarTicket(elegirMonedas.ticket);	  
				   separarArticulos.Rondas = mesas.MesaActiva.RondasActivas;
				   separarArticulos.Show();
				   separarArticulos.EstablecerTemporizador(esTemporizado, this.tiempoFormAc+5000);			
				   infTicketCobrado.TransientFor = separarArticulos;
				   tpv.MostrarMesa(mesas.ListaDeArticulos());	
				}

			     infTicketCobrado.EstablecerInformacion(numTicket.ToString(), camareros.nomCamareroActivo,
                                              String.Format("{0:c}", e.Importe), String.Format("{0:c}",e.Cambio));
				
                  infTicketCobrado.EstablecerTemporizador(true, this.tiempoFormNoAc+60000);
				
            	infTicketCobrado.Show();
				numTicket++;//Incrementar en uno los ticket
				
            }else
				 wSom.Hide();
           
		   	
        }
		                 
		
		void HandleSeccionesclickBoton (MiBoton boton, object args)
        {
			if (boton != null)
               tpv.MostrarTeclado(genArt.getPag(boton.Texto));
			   else
			   tpv.MostrarTeclado(genArt.PagFav);
            
	    }
       
		void HandleLlenarIdenEjAccion (AccionesSepararArticulos accion, List<Articulo> articulos, decimal totalDesglose)
        {
        	  if(accion == AccionesSepararArticulos.llenar){
                 foreach (Articulo articulo in articulos)
                    {
					     Articulo art = articulo.Clone();
					      tpv.AgregarLinea(art);
                    }
                    
                  
           	}

        }

        void HandleInvitarEjAccion (AccionesSepararArticulos accion,List<Articulo> articulos, decimal totalDesglose)
        {
            
		       this.botoneraMesas.OperandoConMesas = false;
                this.botoneraMesas.SalirAlPulsar = true;
                this.botoneraMesas.EstablecerTemporizador(esTemporizado, tiempoFormAc);
				this.botoneraMesas.Informacion = "Herramientas extras";
                mesas.mesaEntratamiento = TratandoMesas.no;
			
        }

        void HandleSepararArticulosEjAccion (AccionesSepararArticulos accion, List<Articulo> articulos, decimal totalDesglose)
        {
					      
        	if(accion == AccionesSepararArticulos.Caja){
                        separarArticulos.CerrarFormulario();
                        ImpresoraTicket.AbrirCajon(rutaDatos, nomImpresora);
         
                       System.Collections. Hashtable nota = new System.Collections.Hashtable();
                        foreach (Articulo art in articulos)
                        {
					        
                            Articulo articuloA = art;
                            if (nota.Contains(articuloA.IDArticulo))
                            {
                                Articulo articuloB = (Articulo)nota[articuloA.IDArticulo];
                                articuloB.TotalLinea += articuloA.TotalLinea;
                                articuloB.Cantidad += articuloA.Cantidad;
                            }
                            else
                            {
                                nota.Add(articuloA.IDArticulo, articuloA.Clone());
                            }
                        }

                            Ticket ticket = new Ticket();
                            ticket.lineas = nota;
                            ticket.Camarero = camareros.nomCamareroActivo;
                            ticket.Mesa = mesas.nomMesaActiva;
                            ticket.FechaInicio = mesas.MesaActiva.HoraApertura;
                            ticket.idTpv = idTpv;
				            ticket.CerrarMesa =  totalDesglose >= tpv.TotalFactura;
                            ticket.TotalTicket = totalDesglose;
				
				       elegirMonedas.Articulos = articulos;
				       elegirMonedas.Importe = ticket .TotalTicket;
					   elegirMonedas.ticket = ticket;
				       elegirMonedas.Show();
			     }
			         else if(accion == AccionesSepararArticulos.Salir){
				         	tpv.BorrarLineasNotas();
			             	botoneraMesas.EstablecerTemporizador(esTemporizado,this.tiempoFormAc);
				            botoneraMesas.Show();         
			     }   
			         else if(accion == AccionesSepararArticulos.llenar){
				          mesas.LlenarRonda(articulos);
				          tpv.MostrarMesa(mesas.ListaDeArticulos());
			      }
	    }

        void HandleHerDividirEjAccion (AccionesDividitTicket accion, List<string> informe)
        {
        	if(accion == AccionesDividitTicket.Caja){
                    HerDividir.CerrarFormulario();
                    ImpresoraTicket.ImprimirInforme(rutaDatos,nomImpresora, 
					      informe.ToArray(),String.Format("Division del ticket {0}",numTicket));
            }
        }

        void HandleHerDesgloseEjAccion (AccionesDesglose accion)
        {
			 switch (accion)
                {
                    case AccionesDesglose.dividir:
                        HerDividir.ImporteADividir = tpv.TotalFactura;
                        HerDividir.EstablecerTemporizador(esTemporizado, tiempoFormAc);
                        HerDividir.Show();
                        break;
                    case AccionesDesglose.llenar:
                        mesas.CerrarConvida();
                        llenarIden.Rondas = mesas.MesaActiva.RondasActivas;
                        llenarIden.EstablecerTemporizador(esTemporizado, tiempoFormAc);
                        llenarIden.Show();
                        break;
                    case AccionesDesglose.separar:
                        mesas.CerrarConvida();
                        separarArticulos.Rondas = mesas.MesaActiva.RondasActivas;
                        separarArticulos.EstablecerTemporizador(esTemporizado, this.tiempoFormAc+5000);
                        separarArticulos.Show();
                        break;
                  
                }
             
        }

		void HandleBotoneraMesasselectorClick (object sender, EventArgMesas e)
        {
        	
                   
             if (e.Tipo  ==  EventArgMesas.TIPO_HERRAMIENTAS)
             {
                this.EjAccionesControlMesas(e.Datos.ToString());  
             }else{
				InfTeclaMesa btn = (InfTeclaMesa)e.Datos;
                if (((mesas.mesaEntratamiento != TratandoMesas.no) && (mesas.mesaEntratamiento != TratandoMesas.Cambiando) && (!mesas.EstaLaMesaAbierta(btn.mesa)))||
                     ((mesas.mesaEntratamiento == TratandoMesas.Cambiando && mesas.mesaAuxiliar==null && (!mesas.EstaLaMesaAbierta(btn.mesa)))))
                    {
              
					
				DialogoTactil.MostrarMensaje("Mesa cerrada","La mesa no esta abierta operacion cancelada",new OnSalirDialogoTactil(delegate(ResDialogTactil res){
						
                  this.botoneraMesas.OperandoConMesas = false;
                  this.botoneraMesas.SalirAlPulsar = true;
                  this.botoneraMesas.EstablecerTemporizador(esTemporizado, this.tiempoFormAc);
                  mesas.mesaEntratamiento = TratandoMesas.no;
                  this.botoneraMesas.Informacion = "Herramientas extras";
                  mesas.mesaAuxiliar = null;
					}));
					
              }else{                   
               switch(mesas.mesaEntratamiento){
                 case TratandoMesas.invitando:
                        this.invitarMesas(btn);
                     break;
                 case TratandoMesas.Cambiando:
                    this.CambiarMesas(btn);
                    break;
                 case TratandoMesas.Juntando:
                    this.JuntarMesas(btn);
                    break;

                 case TratandoMesas.no:
	                 if ((infTicketCobrado != null)&&(infTicketCobrado.Visible)) {     infTicketCobrado.CerrarFormulario();  }
	                 #region Eleccion de una mesa
	                  tpv.BorrarLineasNotas();
	                  if(mesas.AbrirMesa(btn, camareros.nomCamareroActivo)){
					      tpv.MostrarMesa(mesas.ListaDeArticulos(),mesas.nomMesaActiva,camareros.nomCamareroActivo,mesas.MesaActiva.Tarifa);
	                  }else{
	                     this.HandleTpveventTpv(this,new EventTpv(AccionesTpv.mesas));
	                  }
	                   #endregion
	                break;
	              }
               }
             }
        }

        
       
		
		
		
		
		
        void HandleHerramientasEjAccion (AccionesHerramientas accion, object datos)
        {
		 
			switch (accion)
            {
            
                case AccionesHerramientas.ListadoCierres:
                    ListadoCierres();
                  break;
                case AccionesHerramientas.CajaDia:
            		 this.CalcularDiario();
                    break;
                case AccionesHerramientas.CambiarModoImp:
                    puedoImprimir = (bool)datos;
                    DataRow r = gestorBase.ExtraerTabla("Configuracion").Select("IDTpv =" + idTpv)[0];
				    r["ImprimirAutomatico"] = puedoImprimir;
                    gestorBase.ModificarReg(r,Valle.SqlUtilidades.AccionesConReg.Modificar, "IDTpv =" + idTpv);
                    
                    new GesSincronizar(gestorBase).ActualizarSincronizar("Configuracion", "IDTpv =" + idTpv,
                                       Valle.SqlUtilidades. AccionesConReg.Modificar);
                    break;
                    
                case AccionesHerramientas.ReiniciarTpv:
                    this.ReiniciarTpv();
                    break;
                    
                case AccionesHerramientas.CambiarTpv:
                     this.CambiarTpv();
                    break;
                    
                   case AccionesHerramientas.Minimizar:
				       this.tpv.PararTiempo(true);
				       this.tpv.Hide();
				       MostrarIconoBandeja();
				    break;
                default:
                    tpv.VolverTpv();
                    break;

            }
            	
        }
		
		void ReiniciarTpv(){
			this.Reiniciando = true;
			
			tpv.BorrarLineasNotas();
			                if ((infTicketCobrado != null)&&(infTicketCobrado.Visible)) { infTicketCobrado.CerrarFormulario(); }
					        guardarTicket.Terminar();
					        Gtk.Application.Invoke(delegate{  Gtk.Application.Quit();});
			
			System.Diagnostics.Process restart = new System.Diagnostics.Process();
			      restart.StartInfo.FileName = Valle.Utilidades.RutasArchivos.Ruta_Principal+System.IO.Path.DirectorySeparatorChar+"app/Restart.exe";
			      restart.StartInfo.Arguments = System.Diagnostics.Process.GetProcessesByName("mono").Length.ToString() +" "+
				            Valle.Utilidades.RutasArchivos.Ruta_Principal ;
			restart.Start();
	  	}
		
		
		void HandleListadoCierressalirListadoCierres (object sender, EventArgs e)
        {
			tpv.VolverTpv();
        }

		
        void HandleBuscadorEjAccion (AccionTeclado accion, string cadena)
        {
			 switch (accion)
            {
                case AccionTeclado.AC_Buscar:
				     Valle.Utilidades.PaginasObj<Articulo> pg = genArt.BuscarArt(cadena);
				   if(pg!=null)
				        tpv.MostrarTeclado(pg);
			       break;
                case AccionTeclado.AC_cancelar:
				     this.tpv.VolverTpv();
                     this.tpv.MostrarTeclado(genArt.PagFav);
                    break;
                 
            }
         	
        }

        void HandleElegirCamclickBoton (MiBoton boton, object args)
        {
			
          MiBoton camareroElegido = boton;
		   if((camareroElegido != null)&&(!camareroElegido.Texto.Equals("")))
           {
              if (camareroElegido.ColorDeFono.Equals(GesCamareros.COLOR_DEFECTO))
              {
				  ((Camarero)args).ColorDeAtras =	GesCamareros.COLOR_SELECCION;
                  camareros.camarerosActivos.Add((Camarero)args);
                  camareroElegido.ColorDeFono = GesCamareros.COLOR_SELECCION;
              }
              else
              {
				  ((Camarero)args).ColorDeAtras =	GesCamareros.COLOR_DEFECTO;
                 	
                  camareros.camarerosActivos.Remove((Camarero)args);
                  camareroElegido.ColorDeFono = GesCamareros.COLOR_DEFECTO;
              }
				
          }else if((camareroElegido == null)&&(args!=null)){
				     elegirCam.CerrarFormulario();
		          	this.AñadirCamareroABase();
          }else{
				if(camareros.camarerosActivos.Count==0)
					elegirCam.Show();
			  else{
				  elegirCam.CerrarFormulario();
				  botoneraCam.Botones = camareros.camarerosActivos.ToArray();
				  botoneraCam.Show();
				}
          }
        }

        void HandleBotoneraCamclickBoton (MiBoton boton, object args)
        {
			MiBoton pulsado = boton;
		    if ((pulsado!=null)&&(pulsado.Texto != ""))
            {
                camareros.nomCamareroActivo = pulsado.Texto;
				mesas.GestionarMesa(tpv.NumLinesaNotas);  
		        botoneraMesas.EstablecerTemporizador(esTemporizado, this.tiempoFormAc);
		        botoneraMesas.NomCamarero = camareros.nomCamareroActivo;
		        botoneraMesas.Show();
				
		    }else
                elegirCam.Show();
        }

        void HandleBotoneraTpvclickBoton (MiBoton boton, object args)
        { 
			
			MiBoton pulsado = boton;
            if ((pulsado != null) && (!pulsado.Texto.Equals("")))
            {
                DataRow dr =  gestorBase.ExtraerTabla("Configuracion").Select("IDTpv = " + args.ToString())[0];
			    dr["Activo"] = true;
			    gestorBase.EjConsultaNoSelect("Configuracion",Valle.SqlUtilidades.UtilidadesReg.ExConsultaNoSelet(dr,Valle.SqlUtilidades.AccionesConReg.Modificar,
				                                                                "IdVinculacion = "+dr["IdVinculacion"].ToString()).Replace(@"\",@"\\"));
			     this.ReiniciarTpv();
            }
            else
                tpv.VolverTpv();
        	
        }
				
				
				
		
				
	   void EjAccionesControlMesas( string accion)
        {
            SelectorDeMesas bot = this.botoneraMesas;

                if(accion.Equals("cancelar")){
                    bot.SalirAlPulsar = true;
                    mesas.mesaEntratamiento = TratandoMesas.no;
                    bot.Informacion = "Herramientas extras";
			   }
                   
               
				
                if(accion.Equals("cambiar")){
                    bot.SalirAlPulsar = false;
                    mesas.mesaEntratamiento = TratandoMesas.Cambiando;
                    bot.Informacion = "Pulse la mesa que desea cambiar";
		    	}
				
                if(accion.Equals("juntar")){
                    bot.SalirAlPulsar = false;
                    mesas.mesaEntratamiento = TratandoMesas.Juntando;
                    bot.Informacion = "Pulse la mesa que desea juntar";
			     }
				
                if(accion.Equals("invitar")){
                    bot.SalirAlPulsar = false;
                    mesas.mesaEntratamiento = TratandoMesas.invitando;
                    bot.Informacion = "Pulse la mesa invitadora";
		     	}
			
			    if(accion.Equals("AbrirCajon")){
				    this.MostrarCamareros();
				    ImpresoraTicket.AbrirCajon(rutaDatos,nomImpresora);
			     }

                if(accion.Equals("salir")){
				     this.MostrarCamareros();
				 }

            
        }
				
		
		Articulo artVarios = new Articulo();
		
		
		void OnMostrarVenVariosNombre(){
			 if ((infTicketCobrado != null)&&(infTicketCobrado.Visible)) { infTicketCobrado.CerrarFormulario();  }
             artVarios = new Articulo();
			 artVarios.IDArticulo = "0";
             artVarios.Descripcion = "Varios";
             artVarios.NombreCorto = "Varios";
			tclAlf.WindowPosition = Gtk.WindowPosition.CenterAlways;
			
            tclAlf.Titulo = "Insertar Descripcion del articulo";
            tclAlf.Cadena = "";
		    tclAlf.Show();   
		}
		
		void OnMostrarVenVarios(){
			if ((infTicketCobrado != null)&&(infTicketCobrado.Visible)) { infTicketCobrado.CerrarFormulario();  }
           
            tclNumDecimal.Titulo = "Insertar precio";
            tclNumDecimal.Numero = "";
			tclNumDecimal.WindowPosition = Gtk.WindowPosition.CenterAlways;
			
			artVarios = new Articulo();
			 artVarios.IDArticulo = "0";
             artVarios.Descripcion = "Varios";
             artVarios.NombreCorto = "Varios";
			tclNumDecimal.Show();
			
	    }

			 void HandleTclAlfEjAccion (AccionTeclado accion, string cadena)
			 {
			    if (accion  == AccionTeclado.AC_aceptar)
                {
                   Articulo artV = artVarios;
			       artV.Descripcion = tclAlf.Cadena.Length>0 ? tclAlf.Cadena : "Varios";
				}
						
		            tclNumDecimal.Titulo = "Insertar precio";
		            tclNumDecimal.Numero = "";
			        tclNumDecimal.Modal = true;
				    tclNumDecimal.Show();
			 }

			 void HandleTclNumDecimalsalirNumerico (AccionesNumerico accion, string numero)
			 {
			      if (accion == AccionesNumerico.AC_Aceptar)
			            {
			                Articulo artV = artVarios;
				            artV.Precio1 = Decimal.Parse(numero.Length > 0 ? numero.Replace(".",",") : "0");
			                artV.Precio2 = Decimal.Parse(numero.Length > 0 ? numero.Replace(".",",") : "0");
			                artV.Precio3 = Decimal.Parse(numero.Length > 0 ? numero.Replace(".",",") : "0");
				            tpv.AgregarLinea(artV);
			            }else 
				      tpv.VolverTpv();
			 }
		
		
		
		
        
		void OnSalirAplicacion(){
			
			mesas.GestionarMesa(tpv.NumLinesaNotas);
					      
		   
			System.IO.DirectoryInfo dirMesas = new System.IO.DirectoryInfo(Valle.Utilidades.RutasArchivos.Ruta_Completa(RutasConfig.NOM_DIR_MESAS));
            System.IO.FileInfo[] ficMesas = dirMesas.GetFiles();
            System.Text.StringBuilder bl = new System.Text.StringBuilder();
			
             foreach(System.IO.FileInfo f in ficMesas){
			         bl.Append("Abierta la mesa  '"+f.Name.Replace("."+f.Extension,"")+"'  ");
             }
			
             GtkUtilidades.DialogoTactil mensaje;
			
             if(ficMesas.Length>0){
               bl.AppendLine();
                mensaje = new GtkUtilidades.DialogoTactil("Salir de la aplicacion",
                           bl.ToString()+"¿Realmente desea salir de la aplicacion?",TipoRes.si_no);
				    }else{
				
                    mensaje = new GtkUtilidades.DialogoTactil("Salir de la aplicacion",
                          "¿Realmente desea salir de la aplicacion?",TipoRes.si_no);
		        }
				
		    	mensaje.salirDialogoTactil += delegate(ResDialogTactil res) {
					
			       if (res == ResDialogTactil.si)
			        {
			                tpv.BorrarLineasNotas();
			                if ((infTicketCobrado != null)&&(infTicketCobrado.Visible)) { infTicketCobrado.CerrarFormulario(); }
					        guardarTicket.Terminar();
					        Gtk.Application.Invoke(delegate{  Gtk.Application.Quit();});
			        } else
				       tpv.VolverTpv();
				    };
		    } 
		
		
		
		void MostrarCamareros(){
			
		  if (!Reiniciando)
           {
             mesas.GestionarMesa(tpv.NumLinesaNotas);
			 tpv.MostrarTeclado(genArt.PagFav);
             
             if (camareros.camarerosActivos.Count <= 0)
             {
                 this.elegirCam.PaginasBtn = camareros.pagCamareros;
					this.elegirCam.Show();
			 }
             else
             {
                 this.botoneraCam.Botones = camareros.camarerosActivos.ToArray();
				  this.botoneraCam.Show();
	         } 
         }	
			
		}
		
       void ListadoCierres(){
		    listadoCierres.MostrarCierres(this.idTpv);
        }

        void CambiarTpv(){
          DataTable tbTpv = gestorBase.ExtraerTabla("TPVs");
                    List<Valle.Utilidades.InfBoton> btnsTpv = new List<Valle.Utilidades.InfBoton>();
                    foreach (DataRow r in tbTpv.Rows)
                    {
                        Valle.Utilidades.InfBoton b = new Valle.Utilidades.InfBoton();
                        b.Texto = r["Nombre"].ToString();
                        b.Font = new System.Drawing.Font(b.Font, System.Drawing.FontStyle.Bold);
                        b.Font = new System.Drawing.Font(b.Font.Name, 11,
                        b.Font.Style);
                        b.Tamaño = new System.Drawing.Size(100, 90);
                        b.Datos =  r["IDTpv"].ToString();
                        btnsTpv.Add(b);
                    }
                    botoneraTpv.Botones = btnsTpv.ToArray();
                    botoneraTpv.Show();
        }
        
        void invitarMesas(object mesaPulsada)
        {
           InfTeclaMesa btn = (InfTeclaMesa)mesaPulsada;
            mesas.AbrirMesa(btn, camareros.nomCamareroActivo);
            
            if (mesas.mesaAuxiliar == null)
            {
               this.botoneraMesas.Informacion  = (String.Format("La mesa {0} Invita a la mesa: seleccione otra", mesas.nomMesaActiva));
                mesas.mesaAuxiliar = mesas.MesaActiva;
            }
            else
            {
                this.botoneraMesas.Informacion = (String.Format("La mesa {0} Invita a la mesa: {1}",mesas.mesaAuxiliar.mesa, mesas.nomMesaActiva));
                invitar.Rondas = mesas.MesaActiva.RondasActivas;
                invitar.Show();
               
                
            }
        }
		
        void JuntarMesas(object mesaPulsada)
        {
            InfTeclaMesa btn = (InfTeclaMesa)mesaPulsada;
            mesas.AbrirMesa(btn, camareros.nomCamareroActivo);
			
            if (mesas.mesaAuxiliar == null)
            {
                this.botoneraMesas.Informacion = (String.Format("Juntando mesa {0} con: seleccione otra", mesas.nomMesaActiva));
                mesas.mesaAuxiliar = mesas.MesaActiva;
            }
            else
            {
                mesas.JuntarMesa();
                this.botoneraMesas.OperandoConMesas = false;
                this.botoneraMesas.SalirAlPulsar = true;
                this.botoneraMesas.EstablecerTemporizador(esTemporizado, tiempoFormAc);
				this.botoneraMesas.Informacion = "Herramientas extras";
                mesas.mesaEntratamiento = TratandoMesas.no;
            }
        }
        
		void CambiarMesas(object mesaPulsada)
        {
            InfTeclaMesa btn = (InfTeclaMesa)mesaPulsada;
            mesas.AbrirMesa(btn, camareros.nomCamareroActivo);

            if (mesas.mesaAuxiliar == null)
            {
                this.botoneraMesas.Informacion = (String.Format("Cambiando mesa {0} por: seleccione otra", mesas.nomMesaActiva));
                mesas.mesaAuxiliar = mesas.MesaActiva;
            }
            else
            {
                mesas.CambiarMesa();
                this.botoneraMesas.OperandoConMesas = false;
                this.botoneraMesas.SalirAlPulsar = true;
                this.botoneraMesas.EstablecerTemporizador(esTemporizado, tiempoFormAc);
				this.botoneraMesas.Informacion = "Herramientas extras";
                mesas.mesaEntratamiento = TratandoMesas.no;
                
            }
          
        }
       
		
		
        
        void AñadirCamareroABase()
        {
            DataRow dr = camareros.regCamareros();
			TecladoAlfavetico tAlf = new TecladoAlfavetico(TipoDeTeclado.Tipo_Alfabetico);
			tAlf.Titulo  = "Introduce nombre camarero";
			tAlf.WindowPosition = Gtk.WindowPosition.CenterAlways;
			tAlf.Show();
			//Procesar Nombre camarero
            tAlf.EjAccion+= delegate(AccionTeclado accion, string cadena) {
				tAlf.Destroy();
				if((accion != AccionTeclado.AC_aceptar)||(cadena.Length<=0)){
				       elegirCam.PaginasBtn = camareros.pagCamareros;
					   elegirCam.Show();
				}
				else
				{
					dr["Nombre"] = tclAlf.Cadena;
	                tAlf = new TecladoAlfavetico(TipoDeTeclado.Tipo_Alfabetico);
	                tAlf.Titulo = "Introduce Apellidos camarero";
					tAlf.WindowPosition = Gtk.WindowPosition.CenterAlways;
					tAlf.Show();
				   		dr["Nombre"] = cadena;
					  //Procesar Apellidos 
					    tAlf.EjAccion  += delegate(AccionTeclado accionA, string cadenaA) {
					       tAlf.Destroy();
							if((accionA != AccionTeclado.AC_aceptar)||(cadena.Length<=0)){
							      elegirCam.PaginasBtn = camareros.pagCamareros;
							      elegirCam.Show();
						    }
							else
							{           dr["Apellidos"] = cadenaA;
                 				       camareros.AgregarCamarero(dr);
								       elegirCam.PaginasBtn = camareros.pagCamareros;
							           elegirCam.Show();
								  	
							}
				      	};
					 }	
				};
					    
	     }
		
		InfTeclaMesa mesaUnica = new InfTeclaMesa("Barra","Contado",System.Drawing.SystemColors.Control);
        void AbrirMesaUnica()
        {
			camareros.nomCamareroActivo = "Cajero";
			mesaUnica.Tarifa = 1;
            mesas.AbrirMesa(mesaUnica, camareros.nomCamareroActivo);
            tpv.MostrarMesa(mesas.ListaDeArticulos(),mesas.nomMesaActiva,camareros.nomCamareroActivo,mesas.MesaActiva.Tarifa);
        }
		
		void CalcularDiario(){
        	String[] lineas = new GesVentas().CalcularCierre(gestorBase, idTpv);
                    if (lineas != null)
                       ImpresoraTicket.ImprimeResumen(rutaDatos,nomImpresora, lineas, "Cierre de Caja");
                    else
                       DialogoTactil.MostrarMensaje("Aviso en caja diaria", "No hay ticket nuevos desde el ultimo cierre." +
                            " No se puede hacer un cierre de caja",new OnSalirDialogoTactil(delegate{ tpv.VolverTpv();}));
            }
		
         public void Dispose(){
		  
		 if(splashin!=null) splashin.Destroy();
		 if(trayIcon!=null) trayIcon.Dispose();
	     if(tpv != null) tpv.Destroy();	
	     if(secciones != null) secciones.Destroy();
         if(botoneraMesas != null) botoneraMesas.Destroy();
         if(botoneraCam != null) botoneraCam.Destroy();
         Gtk.Widget g = elegirCam; 	if(g!=null ) g.Destroy();
			 g = botoneraTpv; if(g!=null ) g.Destroy();
			 g = tclNumDecimal; if(g!=null ) g.Destroy();
			 g = tclAlf; if(g!=null ) g.Destroy();
	         g = herramientas; if(g!=null ) g.Destroy();
	         g = impTic; if(g!=null ) g.Destroy();
	         g =  infTicketCobrado; if(g!=null ) g.Destroy();
	         g =  HerDesglose; if(g!=null ) g.Destroy();
	         g =  HerDividir; if(g!=null ) g.Destroy();
             g = separarArticulos;	if(g!=null ) g.Destroy();
             g =  llenarIden; if(g!=null ) g.Destroy();
             g = invitar; if(g!=null ) g.Destroy();
             g = buscador;  if(g!=null ) g.Destroy();
             g =listadoCierres;  if(g!=null ) g.Destroy();
             g = elegirMonedas; if(g!=null ) g.Destroy();
          
		
         guardarTicket.Terminar();
		
		
		 g =  wSom; if( g!=null ) g.Destroy();
         
		}
		
		
	}

	
	
			
}

