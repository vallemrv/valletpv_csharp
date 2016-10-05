using Gtk;
using System;
using System.Collections.Generic;
using System.Data;
using Valle.GtkUtilidades;



namespace Valle.GesTpv
{
	
	
	public partial class VenPrecios : Gtk.Window
	{
		public delegate void OnSalirVenPrecios();
		public event OnSalirVenPrecios salirVenPricios;
		
		GesBaseLocal gesl;
		List<string> filtros = new List<string>();
		Dictionary<string, Modificado> ListaModificados = new Dictionary<string, Modificado>();
	    DataTable tbVista;

		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
	       if(ListaModificados.Count>0){
              Gtk.MessageDialog Dlg =   new Gtk.MessageDialog(this,Gtk.DialogFlags.Modal,Gtk.MessageType.Question,Gtk.ButtonsType.YesNo,
	               	                        "Hay precios modificados quiere guardarlos");
	        Dlg.Title = "Precios modificados";       	                        
	        ResponseType result = (ResponseType)Dlg.Run ();
            if (result == ResponseType.Yes)
                               this.OnBtnAplicarCmbClicked(null,null);
                    
              Dlg.Destroy();
             }	   
		
			if(salirVenPricios!=null) this.salirVenPricios();
		}		
		
		public VenPrecios(GesBaseLocal gesl) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.lblInfomacion.Text = "Cambio de precios masivos";
			this.gesl = gesl;
			this.arbolSeciones.AgregarColumnas(new ColumnArbol(typeof(bool),
			                                    "Mostrar",new Gtk.CellRendererToggle(),"active"),
			                                   new ColumnArbol(typeof(String),
			                                     "Seccion", new Gtk.CellRendererText(), "text"));
			
			Gtk.CellRendererText[] celdas = new Gtk.CellRendererText[]{new Gtk.CellRendererText(),
				                                                        new Gtk.CellRendererText(),
				                                                         new Gtk.CellRendererText()};
			  celdas[0].Editable = true;
			  celdas[0].Edited += Precio1Hanled;
			  celdas[0].Foreground = "red";
			  celdas[1].Editable = true;
			  celdas[1].Foreground = "red";
			  celdas[1].Edited += Precio2Hanled;
			  celdas[2].Editable = true;
			  celdas[2].Foreground = "red";
			  celdas[2].Edited += Precio3Hanled;
			
			this.arbolArticulos.AgregarColumnas(new ColumnArbol(typeof(String),
			                                     "  Nombre articulo     ",  new Gtk.CellRendererText(), "text"),
			                                    new ColumnArbol(typeof(String),
			                                     "  Precio uno  ",  new Gtk.CellRendererText(), "text"),
			                                    new ColumnArbol(typeof(String),
			                                     "  Nuevo  ",    celdas[0], "text"),
			                                    new ColumnArbol(typeof(String),
			                                     "  Precio tres  ", new Gtk.CellRendererText(), "text"),
			                                    new ColumnArbol(typeof(String),
			                                     "  Nuevo  ",  celdas[1], "text"),
			                                    new ColumnArbol(typeof(String),
			                                     "  Precio dos  ",   new Gtk.CellRendererText(), "text"),
			                                    new ColumnArbol(typeof(String),
			                                     "  Nuevo  ",  celdas[2], "text"));
			                                    
			    
		  this.RellenarFamilias();
		
		}

		void Precio1Hanled(object o, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
		    this.arbolArticulos.Modelo.GetIter(out iter, new Gtk.TreePath(args.Path));
			if(args.NewText.Length>0){
			   DataRow r = this.arbolArticulos.ExtraerRegSelec();
			   if (!this.ListaModificados.ContainsKey(args.Path)) {
	              Modificado modNuevo = new Modificado();
				  modNuevo.id = r["IDArticulo"].ToString();
				  ListaModificados.Add(args.Path,modNuevo);
               }
				Modificado mod = ListaModificados[args.Path];
				mod.precio1 = Decimal.Parse(args.NewText);
				this.arbolArticulos.Modelo.SetValue(iter,2,String.Format("{0:#0.00}",Decimal.Parse(args.NewText)));
				mod.reg = r;
				this.lblInfomacion .Text = "Se han modificado los precios y no estan guardados";
			}else{
				if (this.ListaModificados.ContainsKey(args.Path)) {
				   Modificado mod = ListaModificados[args.Path];
				   mod.precio1 =-1;
				   if((mod.precio1<0)&&(mod.precio2<0)&&(mod.precio3<0))
						   ListaModificados.Remove(args.Path);
				 	this.arbolArticulos.Modelo.SetValue(iter,2,args.NewText);	
				}
			}
		 	
		}
		
		void Precio2Hanled(object o, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
		    this.arbolArticulos.Modelo.GetIter(out iter, new Gtk.TreePath(args.Path));
			if(args.NewText.Length>0){
			   DataRow r = this.arbolArticulos.ExtraerRegSelec();
			   if (!this.ListaModificados.ContainsKey(args.Path)) {
	              Modificado modNuevo = new Modificado();
				  modNuevo.id = r["IDArticulo"].ToString();
				  ListaModificados.Add(args.Path,modNuevo);
               }
				Modificado mod = ListaModificados[args.Path];
				mod.precio2 = Decimal.Parse(args.NewText);
				mod.reg = r;
				 	this.arbolArticulos.Modelo.SetValue(iter,4,String.Format("{0:#0.00}",Decimal.Parse(args.NewText)));
				this.lblInfomacion .Text = "Se han modificado los precios y no estan guardados";
			}else{
				if (this.ListaModificados.ContainsKey(args.Path)) {
				   Modificado mod = ListaModificados[args.Path];
				   mod.precio2 =-1;
				   if((mod.precio1<0)&&(mod.precio2<0)&&(mod.precio3<0))
						   ListaModificados.Remove(args.Path);
				 	this.arbolArticulos.Modelo.SetValue(iter,4,args.NewText);	
				}
			}
		 	
		
			
		}
		
		void Precio3Hanled(object o, Gtk.EditedArgs args)
		{
			Gtk.TreeIter iter;
		    this.arbolArticulos.Modelo.GetIter(out iter, new Gtk.TreePath(args.Path));
			if(args.NewText.Length>0){
			   DataRow r = this.arbolArticulos.ExtraerRegSelec();
			   if (!this.ListaModificados.ContainsKey(args.Path)) {
	              Modificado modNuevo = new Modificado();
				  modNuevo.id = r["IDArticulo"].ToString();
				  ListaModificados.Add(args.Path,modNuevo);
               }
				Modificado mod = ListaModificados[args.Path];
				mod.precio3 = Decimal.Parse(args.NewText);
				this.arbolArticulos.Modelo.SetValue(iter,6,String.Format("{0:#0.00}",Decimal.Parse(args.NewText)));
				mod.reg = r;
				this.lblInfomacion .Text = "Se han modificado los precios y no estan guardados";
			}else{
				if (this.ListaModificados.ContainsKey(args.Path)) {
				   Modificado mod = ListaModificados[args.Path];
				   mod.precio3 =-1;
				   if((mod.precio1<0)&&(mod.precio2<0)&&(mod.precio3<0))
						   ListaModificados.Remove(args.Path);
				this.arbolArticulos.Modelo.SetValue(iter,6,args.NewText);
				}
			}
		 	
		 	
		}
		
		void RellenarFamilias(){
		  	DataTable tbFamilias = gesl.ExtraerLaTabla("Familias");
			 foreach(DataRow r in tbFamilias.Rows){
				 this.arbolSeciones.Modelo.AppendValues(false,r["Nombre"].ToString(),r);	
			}
			
		}
		
		void RellenarArt(){
		  this.arbolArticulos.Modelo.Clear();
			
			foreach(DataRow r in tbVista.Rows)
		    	this.arbolArticulos.Modelo.AppendValues(r["Nombre"].ToString(),r["Precio1"].ToString(),
				                                  "",r["Precio2"].ToString(),"",
				                                     r["Precio3"].ToString(),"",r);
				
		}
		
		void Filtrar(){
		  string filtro = "";
		  if(filtros.Count>0) filtro = " WHERE ";
		  foreach(string sec in filtros){
			  filtro += " Familias.Nombre ='"+sec+"' ";
			  if(filtros.IndexOf(sec)< filtros.Count-1)
					filtro += " OR ";
			}
			
			if(tbVista != null) tbVista.Dispose();
	        tbVista =  gesl.EjConsultaSelect("RegistroMuestra","SELECT Articulos.IDVinculacion as IDArticulo, Articulos.Nombre, Articulos.Precio1, "+
                                 "Articulos.Precio2, Articulos.Precio3 FROM Articulos INNER JOIN Familias ON "+
			                     "Familias.IDFamilia = Articulos.IDFamilia "+ filtro,"Familias","Articulos");
			
			this.RellenarArt();
			
		}
			        
	  

		protected virtual void OnArbolSecionesFilaActivada (object o, Gtk.RowActivatedArgs args)
		{
			if(ListaModificados.Count>0){
              Gtk.MessageDialog Dlg =   new Gtk.MessageDialog(this,Gtk.DialogFlags.Modal,Gtk.MessageType.Question,Gtk.ButtonsType.YesNo,
	               	                        "Hay precios modificados quiere guardarlos");
			        Dlg.Title = "Precios modificados";       	                        
			        ResponseType result = (ResponseType)Dlg.Run ();
		            if (result == ResponseType.Yes)
		                               this.OnBtnAplicarCmbClicked(null,null);
				          else
					         this.ListaModificados.Clear();
		                    
		              Dlg.Destroy();
		       }	   
		
			Gtk.TreeIter iter;
			this.arbolSeciones.Modelo.GetIter(out iter,args.Path);
			bool activo = (bool)this.arbolSeciones.Modelo.GetValue(iter,0);
			string seccion = this.arbolSeciones.Modelo.GetValue(iter,1).ToString();
			if(!activo) filtros.Add(seccion);
			 else filtros.Remove(seccion);
			this.arbolSeciones.Modelo.SetValue(iter,0,!activo);
			Filtrar();
		}

		protected virtual void OnBtnRevertirEurosClicked (object sender, System.EventArgs e)
		{
			this.RellenarArt();
			this.btnRevertirEuros.Sensitive = false;
			this.ListaModificados.Clear();
		}

		protected virtual void OnBtnActualizarEuroClicked (object sender, System.EventArgs e)
		{
			if(ListaModificados.Count>0){
              Gtk.MessageDialog Dlg =   new Gtk.MessageDialog(this,Gtk.DialogFlags.Modal,Gtk.MessageType.Question,Gtk.ButtonsType.YesNo,
	               	                        "Hay precios modificados quiere guardarlos");
		        Dlg.Title = "Precios modificados";       	                        
		        ResponseType result = (ResponseType)Dlg.Run ();
	            if (result == ResponseType.Yes)
	                               this.OnBtnAplicarCmbClicked(null,null);
				else
					ListaModificados.Clear();
                    
              	Dlg.Destroy();
             }	  
			
			this.btnRevertirEuros.Sensitive = true;
			Gtk.TreeIter iter;
			this.arbolArticulos.Modelo.GetIterFirst(out iter);
			decimal incremento = Decimal.Parse(this.txtEuros.Text);
			decimal precio1 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,1).ToString());
			decimal precio2 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,3).ToString());
			decimal precio3 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,5).ToString());                                                                
			System.Console.WriteLine(precio1);
			this.arbolArticulos.Modelo.SetValue(iter,2,(precio1+incremento).ToString());
			this.arbolArticulos.Modelo.SetValue(iter,4,(precio2+incremento).ToString());
			this.arbolArticulos.Modelo.SetValue(iter,6,(precio3+incremento).ToString());
	
			DataRow r = (DataRow)this.arbolArticulos.Modelo.GetValue(iter,7);
			Gtk.TreePath path =
			  this.arbolArticulos.Modelo.GetPath(iter);
			   if (!this.ListaModificados.ContainsKey(path.ToString())) {
	              Modificado modNuevo = new Modificado();
				  modNuevo.id = r["IDArticulo"].ToString();
				  ListaModificados.Add(path.ToString(),modNuevo);
               }
				Modificado mod = ListaModificados[path.ToString()];
			    mod.precio1 = precio1+incremento;
			    mod.precio2 = precio2+incremento;
				mod.precio3 = precio3+incremento;
			    mod.reg = r;
				
			  while(this.arbolArticulos.Modelo.IterNext(ref iter)){
			    
				precio1 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,1).ToString());
			    precio2 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,3).ToString());
			    precio3 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,5).ToString());                                                                
			
				this.arbolArticulos.Modelo.SetValue(iter,2,(precio1+incremento).ToString());
				this.arbolArticulos.Modelo.SetValue(iter,4,(precio2+incremento).ToString());
				this.arbolArticulos.Modelo.SetValue(iter,6,(precio3+incremento).ToString());
				     	
				    r = (DataRow)this.arbolArticulos.Modelo.GetValue(iter,7);
					path = this.arbolArticulos.Modelo.GetPath(iter);
					   if (!this.ListaModificados.ContainsKey(path.ToString())) {
			              Modificado modNuevo = new Modificado();
						  modNuevo.id = r["IDArticulo"].ToString();
						  ListaModificados.Add(path.ToString(),modNuevo);
		               }
						mod = ListaModificados[path.ToString()];
					    mod.precio1 = precio1+incremento;
					    mod.precio2 = precio2+incremento;
						mod.precio3 = precio3+incremento;
			            mod.reg = r;
			}
			this.lblInfomacion .Text = "Se han modificado los precios y no estan guardados";
			
		}

		

		protected virtual void OnBtnRevertirPorcClicked (object sender, System.EventArgs e)
		{
			this.RellenarArt();
			this.btnRevertirPorc.Sensitive = false;
			this.ListaModificados.Clear();
		}

		protected virtual void OnBtnActualizarPorcClicked (object sender, System.EventArgs e)
		{
			if(ListaModificados.Count>0){
              Gtk.MessageDialog Dlg =   new Gtk.MessageDialog(this,Gtk.DialogFlags.Modal,Gtk.MessageType.Question,Gtk.ButtonsType.YesNo,
	               	                        "Hay precios modificados quiere guardarlos");
		        Dlg.Title = "Precios modificados";       	                        
		        ResponseType result = (ResponseType)Dlg.Run ();
	            if (result == ResponseType.Yes)
	                               this.OnBtnAplicarCmbClicked(null,null);
				   else
					 this.ListaModificados.Clear();
                    
              	Dlg.Destroy();
             }	  
			
			this.btnRevertirPorc.Sensitive = true;
			Gtk.TreeIter iter;
			this.arbolArticulos.Modelo.GetIterFirst(out iter);
			decimal incremento = Decimal.Parse(this.txtPorcentaje.Text);
			decimal precio1 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,1).ToString());
			decimal precio2 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,3).ToString());
			decimal precio3 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,5).ToString());                                                                
			
			this.arbolArticulos.Modelo.SetValue(iter,2,String.Format(
			                                 "{0:#0.00}",(precio1*((incremento/100)+1))));
			this.arbolArticulos.Modelo.SetValue(iter,4,String.Format(
			                                 "{0:#0.00}",(precio2*((incremento/100)+1))));
			this.arbolArticulos.Modelo.SetValue(iter,6,String.Format(
			                                 "{0:#0.00}",(precio3*((incremento/100)+1))));
	       
			DataRow r = (DataRow)this.arbolArticulos.Modelo.GetValue(iter,7);
			Gtk.TreePath path =
			this.arbolArticulos.Modelo.GetPath(iter);
			   if (!this.ListaModificados.ContainsKey(path.ToString())) {
	              Modificado modNuevo = new Modificado();
				  modNuevo.id = r["IDArticulo"].ToString();
				  ListaModificados.Add(path.ToString(),modNuevo);
               }
				Modificado mod = ListaModificados[path.ToString()];
			    mod.precio1 = (precio1*((incremento/100)+1));
			    mod.precio2 = (precio2*((incremento/100)+1));
				mod.precio3 = (precio3*((incremento/100)+1));
			    mod.reg = r;
				
			  while(this.arbolArticulos.Modelo.IterNext(ref iter)){
			    
				precio1 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,1).ToString());
			    precio2 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,3).ToString());
			    precio3 = Decimal.Parse(this.arbolArticulos.Modelo.GetValue(iter,5).ToString());                                                                
			
				this.arbolArticulos.Modelo.SetValue(iter,2,String.Format(
			                                 "{0:#0.00}",(precio1*((incremento/100)+1))));
			    this.arbolArticulos.Modelo.SetValue(iter,4,String.Format(
			                                 "{0:#0.00}",(precio2*((incremento/100)+1))));
			    this.arbolArticulos.Modelo.SetValue(iter,6,String.Format(
			                                 "{0:#0.00}",(precio3*((incremento/100)+1))));
	     	
				r = (DataRow)this.arbolArticulos.Modelo.GetValue(iter,7);
				path = this.arbolArticulos.Modelo.GetPath(iter);
					   if (!this.ListaModificados.ContainsKey(path.ToString())) {
			              Modificado modNuevo = new Modificado();
						  modNuevo.id = r["IDArticulo"].ToString();
						  ListaModificados.Add(path.ToString(),modNuevo);
		               }
						mod = ListaModificados[path.ToString()];
					    mod.precio1 = (precio1*((incremento/100)+1));
					    mod.precio2 = (precio2*((incremento/100)+1));
						mod.precio3 = (precio3*((incremento/100)+1));
				        mod.reg = r;
			
			}
			this.lblInfomacion .Text = "Se han modificado los precios y no estan guardados";
			
		}

		

		protected virtual void OnBtnAplicarCmbClicked (object sender, System.EventArgs e)
	    {
			foreach(Modificado mod in ListaModificados.Values){
			   Valle.SqlUtilidades.Registro reg = mod.ExtraerReg();
			   gesl.GuardarDatos(reg);
			   gesl.ActualizarSincronizar(reg.NomTabla,reg.CadenaSelect,reg.AccionReg);
			   mod.Validar();
			}
			ListaModificados.Clear();
			this.btnRevertirEuros.Sensitive = this.btnRevertirPorc.Sensitive = false;
			this.lblInfomacion.Text = "Los precios han sido modificados y guardados correctamente";
			this.RellenarArt();
		}	
	}
	  
	public class Modificado
    {
	     public string id;
		 public decimal precio1 = -1;
		 public decimal precio2 = -1;
		 public decimal precio3 = -1;
		 public DataRow reg;
		 public Valle.SqlUtilidades.Registro ExtraerReg(){
				Valle.SqlUtilidades.Registro reg = new Valle.SqlUtilidades.Registro();
					reg.AccionReg = Valle.SqlUtilidades.AccionesConReg.Modificar;
					reg.CadenaSelect = "IDVinculacion = "+id;
				    reg.NomTabla = "Articulos";
					Valle.SqlUtilidades.Columna cl;
					if(precio1>=0){
				        cl = new Valle.SqlUtilidades.Columna();
						cl.nombreColumna = "Precio1";
						cl.valor = precio1;
						reg.Columnas.Add(cl);
					}
					if(precio2>=0){
				        cl = new Valle.SqlUtilidades.Columna();
						cl.nombreColumna = "Precio2";
						cl.valor = precio2;
						reg.Columnas.Add(cl);
					}
					if(precio3>=0){
				        cl = new Valle.SqlUtilidades.Columna();
						cl.nombreColumna = "Precio3";
						cl.valor = precio3;
						reg.Columnas.Add(cl);
					}
					return reg;
				}
		
		    public void Validar(){
		         reg["precio1"] = precio1;	       
			     reg["precio2"] = precio2;
			     reg["precio3"] = precio3;
			     reg.Table.AcceptChanges();
		    }
       }
}

			
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			