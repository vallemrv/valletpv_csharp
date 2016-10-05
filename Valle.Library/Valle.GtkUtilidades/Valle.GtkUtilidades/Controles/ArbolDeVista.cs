// ArbolDeVista.cs created with MonoDevelop
// User: valle at 0:50 10/06/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using Gtk;

namespace Valle.GtkUtilidades
{
    
    
    [System.ComponentModel.Category("Valle.GtkUtilidades")]
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ArbolDeVista : Gtk.Bin
    {
        public event Gtk.RowActivatedHandler FilaActivada;
        public event System.EventHandler SeleccionCambiada;
		public event System.EventHandler<Gtk.ScrollChildArgs> ScrollModificado;
        
        public ArbolDeVista()
        {
            this.Contruir();
        }
	
		public Gtk.Adjustment VScroll{
		  get {
			  return this.treeUnico.Vadjustment	;
			}
		}
		
		public Gtk.Adjustment HScroll{
		  get {
			  return this.treeUnico.Hadjustment	;
			}
		}
		
	    public void AgregarColumnas(params string[] NomColumnasArbol){
		  int pos = 0;
		  Type[] tipos = new Type[NomColumnasArbol.Length+1];
		  this.borrarColumnas();//Vaciamos todos los datos que pudiese haber;
		 
			 foreach(string cl in NomColumnasArbol){
			    tipos[pos] = typeof(String);
			    this.treeUnico.AppendColumn(cl,new Gtk.CellRendererText(),"text",pos);
				pos++;
		     }
		     
		     tipos[tipos.Length-1] = typeof(DataRow);//tipo que contendra el datarow mostrado
	         Modelo = new Gtk.ListStore(tipos);
		     
		}
		
		
		//Agrega columnas con tipos y renderizados diferentes
		 public void AgregarColumnas(params ColumnArbol[] columnas){
		  int pos = 0;
		  Type[] tipos = new Type[columnas.Length+1];
		  this.borrarColumnas();//Vaciamos todos los datos que pudiese haber;
		 
			 foreach(ColumnArbol cl in columnas){
			    tipos[pos] = cl.tipoDato;
			    this.treeUnico.AppendColumn(cl.nomColumn,cl.renderizacion,cl.Propiedad,pos);
				pos++;
		     }
		     
		     tipos[tipos.Length-1] = typeof(DataRow);//tipo que contendra el datarow mostrado
	         Modelo = new Gtk.ListStore(tipos);
		}
       
        
        [System.Obsolete("Usar AgregarColumnas", true)]
        public void AgregarColumna(string s, Gtk.CellRenderer cell, string propiedad, int index){
             this.treeUnico.AppendColumn(s,cell,propiedad,index);
	    }
        
        public void borrarColumnas(){
            foreach(Gtk.TreeViewColumn c in this.treeUnico.Columns){
               this.treeUnico.RemoveColumn(c);
            }
        }
        
        
        public Gtk.ListStore Modelo{
            set{
              this.treeUnico.Model = value;
            }
            get{
              return (Gtk.ListStore)this.treeUnico.Model;
            }
        }
        
        public void Size(int ancho, int alto){
            this.treeUnico.SetSizeRequest(ancho,alto);
        }
        
        
        public DataRow ExtraerRegSelec(){
             Gtk.TreeModel model;
             Gtk.TreeIter iter;
                if (this.treeUnico.Selection.GetSelected(out model, out iter)) 
                      return (DataRow)Modelo.GetValue(iter,Modelo.NColumns-1);
                      else
                      return null;
        }
		
		
		//Agrega un gesistro solo. El modelo tiene que estar creado previamente
		public void AgregarRegistro(DataRow dr,params string[] nomColumTbAMostrar){
		 	object[] reg = new object[Modelo.NColumns] ;
			
			for(int i=0;i<nomColumTbAMostrar.Length ;i++)
	            reg[i] = dr[nomColumTbAMostrar[i]].ToString();
	          
		      reg[reg.Length-1] = dr;
		      Modelo.AppendValues(reg);  
		}
		
		//Agrega un registros de forma manual. Como se haria en un modelo normal
		public void AgregarRegistro(DataRow r, params object[] col){
			object[] reg = new object[Modelo.NColumns] ;
			
			for(int i=0;i<col.Length ;i++){
	            reg[i] = col;
			    System.Console.WriteLine(col.ToString());
			}
	          
		      reg[reg.Length-1] = r;
			
		      Modelo.AppendValues(reg);  
		}	
        
		//Rellena el conjuto de registros. Podemos elegir que columnas mostrar y cuales seran sus nombres
        public void rellenarLista(DataRow[] rws, string[] nomColumTbAMostrar, string[] NomColumnaArbol){
		  int pos = 0;
		  Type[] tipos = new Type[nomColumTbAMostrar.Length+1];
		  object[] reg = new object[nomColumTbAMostrar.Length+1];
		 
		 this.borrarColumnas();//Vaciamos todos los datos que pudiese haber;
		 
		 foreach(string cl in NomColumnaArbol){
		    tipos[pos] = typeof(String);
		    this.treeUnico.AppendColumn(cl,new Gtk.CellRendererText(),"text",pos);
		    pos++;
	     }
	     
	     tipos[tipos.Length-1] = typeof(DataRow);//tipo que contendra el datarow mostrado
           
	     Modelo = new Gtk.ListStore(tipos);
	     
	     foreach(DataRow dr in rws){
	      for(int i=0;i< nomColumTbAMostrar.Length;i++){
	          reg[i] = dr[nomColumTbAMostrar[i]].ToString();
		      }
		      reg[reg.Length-1] = dr;
		      Modelo.AppendValues(reg);  
            }
		
		}
        
		//Muestra todos los datos de la tabla. Con los nombres que tenga la base de datos
        public void rellenarLista(DataTable tb){
		
		int pos = 0;
		Type[] tipos = new Type[tb.Columns.Count+1];
		object[] reg = new object[tb.Columns.Count+1];
				       
	    this.borrarColumnas();//Vaciamos todos los datos que pudiese haber;
	
		foreach(DataColumn cl in tb.Columns){
		       tipos[pos] = typeof(String);
		    this.treeUnico.AppendColumn(cl.ColumnName,new Gtk.CellRendererText(),"text",pos);
				pos++;
	    }
	    
				       
		tipos[tipos.Length-1] = typeof(DataRow);//tipo que contendra el datarow mostrado
           
	     Modelo = new Gtk.ListStore(tipos);
	   
    			       	     
		    foreach(DataRow dr in tb.Rows){
		      for(int i=0;i< tb.Columns.Count;i++){
		          reg[i] = dr[tb.Columns[i].ColumnName].ToString();
		      }
		         reg[reg.Length-1] = dr;
		         Modelo.AppendValues(reg);  
            }
				       
		}
        
		//Rellena todos los datos. Puedes elegir las columnas a mostrar y los nombres que muestra la vista
        public void rellenarLista(DataTable tb, string[] nomColumTbAMostrar, string[] NomColumnaArbol){
		  int pos = 0;
		  Type[] tipos = new Type[nomColumTbAMostrar.Length+1];
		  object[] reg = new object[nomColumTbAMostrar.Length+1];
		 
		 this.borrarColumnas();//Vaciamos todos los datos que pudiese haber;
		 
		 foreach(string cl in NomColumnaArbol){
		    tipos[pos] = typeof(String);
		    this.treeUnico.AppendColumn(cl,new Gtk.CellRendererText(),"text",pos);
		    pos++;
	     }
	     
	     tipos[tipos.Length-1] = typeof(DataRow);//tipo que contendra el datarow mostrado
           
	     Modelo = new Gtk.ListStore(tipos);
	   
	     
	     foreach(DataRow dr in tb.Rows){
	      for(int i=0;i< nomColumTbAMostrar.Length;i++){
	          reg[i] = dr[nomColumTbAMostrar[i]].ToString();
		      }
		      reg[reg.Length-1] = dr;
		      Modelo.AppendValues(reg);  
            }

		}

        protected virtual void OnTreeUnicoRowActivated (object o, Gtk.RowActivatedArgs args)
        {
		   if(FilaActivada != null) FilaActivada(this,args);
        }

        protected virtual void OnTreeUnicoCursorChanged (object sender, System.EventArgs e)
        {
           if(SeleccionCambiada!=null) SeleccionCambiada(this,e);
        }
		
        protected virtual void OnGtkScrolledWindowScrollChild (object o, Gtk.ScrollChildArgs args)
    	{
			if(ScrollModificado!=null) ScrollModificado(o,args);
    	}
    	
    	
    }
	
	public class ColumnArbol{
		public Type tipoDato;
		public string nomColumn;
		public Gtk.CellRenderer renderizacion;
		public string Propiedad;
		public ColumnArbol(Type tipo,string nomColumn,Gtk.CellRenderer rederizacion,string propiedad){
			this.tipoDato = tipo;
			this.nomColumn = nomColumn;
			this.renderizacion = rederizacion;
			this.Propiedad = propiedad;
		}
		public ColumnArbol(){}
		
	}
}
