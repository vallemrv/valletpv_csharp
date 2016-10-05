// Planing.cs created with MonoDevelop
// User: valle at 16:29 27/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Data;

using Valle.Utilidades.Imagenes;

namespace Valle.GesTpv
{
     public class ConstantesImg{
        public static string mesa = "Mesa.png";
		public static string mesaAlta = "mesaalta.png";
		public static string BarraRect = "BarraRect.png";
		public static string BarraCurba = "barracurba.png";
		public static string losas = "losas.png";
		public static string papelera = "papelera.png";
		public static string sombra = "Sombra.png";
     
     }
     
    [Serializable]
    public class LosDatosMesa: ObjInteractivo{
      public InfMesa mesa;
      
      
      public LosDatosMesa(Dimension dim, Localizacion loc, string id, InfMesa mesa):
       base(dim,loc,id)
       {
           this.mesa = mesa;
           this.DibujarObjeto();
           
       }
       
      public override void DibujarObjeto(){
            this.imgObj = new Bitmap(Dim.Ancho,Dim.Alto);                
            Graphics gInf = Graphics.FromImage(this.imgObj);
            gInf.Clear(Color.Transparent);
            //Escribe el nombre de la mesa
		    gInf.DrawString(mesa.NomMesa,  new Font("Times",12,FontStyle.Bold), new SolidBrush(Color.Black),5,0);
 			//Escribe una refencia si la hay
			gInf.DrawString(mesa.Referencia,  new Font("Times",12,FontStyle.Bold),new SolidBrush(Color.Black),5,15);
			base.DibujarObjeto();
	  }
      
      public override void CargarFoto(){
        this.DibujarObjeto();
      }
      
      
    }
    
    [Serializable]
	public class InfMesa: ObjInteractivo{
	  
	   protected string nomMesa="";
	   protected string referencia="";
	  
	   protected LosDatosMesa miImfor;
	   [NonSerialized]
	   protected Bitmap imgOriginal;
	   
	   
	   protected List<RotateFlipType> instrucRot = new List<RotateFlipType>();
	   protected bool ocupada = false;
	   
      
       public override void DibujarObjeto ()
       {
            
            if(this.imgOriginal==null){
                            this.imgOriginal = this.imgObj;
                            foreach(RotateFlipType tipo in this.instrucRot){
                            this.imgOriginal.RotateFlip(tipo);
                             }
                            }
                            
           //Crea un objeto asociado para que contenga la informacion de la mesa
           if(this.miImfor == null){
                         Marco  ubicacionInf = new Marco(new Localizacion(this.Loc.X,this.Loc.Y-10),new Dimension(90,40));
                           miImfor = new LosDatosMesa(ubicacionInf.dimension,ubicacionInf.localizacion,"inf",this);
                    }
          
            this.imgObj = new Bitmap(Dim.Ancho,Dim.Alto);
            Graphics g = Graphics.FromImage(this.imgObj);
            g.DrawImage(imgOriginal,0,0,this.Dim.Ancho,this.Dim.Alto);
                
		    //color del cuadradito que indica si la mesa esta ocupada
		    Color c ;
		    if(!ocupada) 
		       c= Color.Blue; else c = Color.Red;
		    // Dibuja el cuadradito  
			g.FillRectangle(new SolidBrush(c), new Rectangle(this.Dim.Ancho-18,5,15,15));
			
           base.DibujarObjeto ();
       }
 
       

       public bool Ocupada {
           get {
               return ocupada;
           }
           set {
               ocupada = value;
               this.DibujarObjeto();
           }
       }

       public string NomMesa {
           get {
               return nomMesa;
           }
           set {
               nomMesa = value;
               if(miImfor!=null)
                      this.miImfor.DibujarObjeto();
           }
       }

       public string Referencia {
           get {
               return referencia;
           }
           set {
               referencia = value;
               if(miImfor!=null)
                   this.miImfor.DibujarObjeto();
           }
       }

       public LosDatosMesa MiImfor {
           get {
               return miImfor;
           }
           set {
               miImfor = value;
           }
       }
       
       public InfMesa(string nomImg, Dimension dim, Localizacion loc, string nomMesa, string id):  
              base(nomImg, dim, loc, id)
       {
          this.nomMesa = nomMesa;
       }
       
       public override void RotarImagen(RotateFlipType tipo){
            imgOriginal.RotateFlip(tipo);
            this.instrucRot.Add(tipo);
            base.RotarImagen(tipo);
       }
           
       public void Refrescar(){
          this.DibujarObjeto();
       }
       
           
    }
    [Serializable]
    public class BotonesObj: ObjInteractivo<string>
    {
       public string RutCompImg{
          get{
            return this.rutaImgs+this.nomImg;
          }
       }
       public BotonesObj(string nomImg, Dimension dim, Localizacion loc, string id):  
              base(nomImg, dim, loc, id){
                   this.nomImg = nomImg;
              }
              
        public override void DibujarObjeto ()
        { 
            Bitmap imgAux = new Bitmap(this.Datos);
            Graphics gAux =  Graphics.FromImage(imgAux);
            gAux.DrawRectangle(new Pen(Color.Black),0,0, imgAux.Width-1, imgAux.Height-1);
            this.imgObj = imgAux;
            base.DibujarObjeto ();
        }
      
        public override void CargarFoto ()
        {
          if(escPadre!=null){
             this.Datos = (escPadre as Escenario).RutaImgs+this.nomImg;
             this.DibujarObjeto();
             }
        }
      
    }
    
	
	[Serializable]
	public class Planing : Escenario, ISerializable
	{
	    public string nomZona;
	    public Dimension DimDeObjetos;
	    
	          public Planing(SerializationInfo info, StreamingContext context):
               base(info.GetString("id"), (Localizacion)info.GetValue("localizacion",typeof(Localizacion)) ,(Dimension)info.GetValue("dimension",typeof(Dimension)))
                {
                    this.nomZona = info.GetString("nomZona");
                    this.DimDeObjetos = (Dimension)info.GetValue("dimObj",typeof(Dimension));
                    this.inicializarVariables();
                    int numObj = info.GetInt32("numObj");
                           for(int i= 0; i<numObj;i++){
                                this.conjObjs.Add((ObjBase)info.GetValue("obj"+i, typeof(ObjBase)));
                                if(!(this.conjObjs[i] is ObjEstatico))
                                     this.PonerEnTablero(this.conjObjs[i]);
                                }
                }
              #region Miembros de ISerializable

              void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
              {
                    info.AddValue("dimObj",this.DimDeObjetos, typeof(Dimension));
                    info.AddValue("nomZona",this.nomZona);
                    info.AddValue("dimension", dimension, typeof(Dimension));
                    info.AddValue("localizacion", localizacion, typeof(Localizacion));
                    info.AddValue("id", this.IDObjeto);
                   ObjBase[] objs = this.conjObjs.ToArray();
                    info.AddValue("numObj",objs.Length);
                    for(int i= 0; i<objs.Length;i++)
                        info.AddValue("obj"+i, objs[i], typeof(ObjBase));
                  
                  
              }
              #endregion
              
		public Planing(string nom, string ID,  Marco plano, Dimension dimDeObj,string rutaImgs, string nomImgFondo):
		 base(ID,plano.localizacion,plano.dimension)
		{ 
		   this.IDObjeto = ID; this.nomZona = nom;
		   this.DimDeObjetos = dimDeObj;
		   this.rutaImgs = rutaImgs;
		   this.AgregarObj(new ObjEstatico(nomImgFondo,this.dimension,new Localizacion(0,0),"fondo"));
		}
		
		public override void DibujarObjeto ()
		{ 
	         if(escPadre!=null){
		   	  
		       Graphics g = Graphics.FromImage(this.imgObj);
               g.Clear(Color.Wheat);
              
               foreach (ObjBase obj in conjObjs){
                 if((!obj.Oculto)){
                       g.DrawImage(obj.ImgObj,obj.Loc.X,obj.Loc.Y,obj.Dim.Ancho,obj.Dim.Alto);
                       if(obj is InfMesa){
            		        InfMesa mesa = (obj as InfMesa);
            		        LosDatosMesa datos = mesa.MiImfor;
            		        if(!datos.Oculto)
            		           g.DrawLine(new Pen(Color.Black),mesa.Loc.X +(mesa.Dim.Ancho/2),mesa.Loc.Y +(mesa.Dim.Alto/2),
            		                                                datos.Loc.X + (datos.Dim.Ancho/2), datos.Loc.Y + (datos.Dim.Alto/2));
		                  }
		         }       
		    }
		     this.escPadre.DibujarObjeto();
		     this.OnImgMod();
		    }
		}
		
		public override void AgregarObj (ObjBase obj)
		{
		    base.AgregarObj (obj);
		    if(obj.escPadre!=null){ 
		    if(obj is InfMesa){
		      InfMesa mesa = (obj as InfMesa);
		      LosDatosMesa datos = mesa.MiImfor;
		         if(this.PuedoDibujar(new Marco(datos.Loc, datos.Dim))){
		            base.AgregarObj(datos);
		           }else{
		            datos.Loc = new Localizacion(datos.Loc.X , datos.Loc.Y * mesa.Loc.Y+mesa.Dim.Alto);
		            base.AgregarObj(datos);
		          }
		        }
		       }
		}

        public override void QuitarObj (ObjBase obj)
        {
           if(obj is InfMesa)
               base.QuitarObj((obj as InfMesa).MiImfor);
               
            base.QuitarObj (obj);
            
        }
        
        public override void MoverObj(ObjBase obj, Localizacion locNueva){
                base.MoverObj(obj,locNueva);
        }
		
		
		public bool EscalarObjetos(Dimension dimObj){
		  this.DimDeObjetos = dimObj;
		  bool sePuede = false;
		  foreach(ObjBase mesa in this.conjObjs){
		    if(mesa is InfMesa){
		        if(this.PuedoDibujar(new Marco(mesa.Loc,dimObj))){
		            this.QuitarEnTablero(mesa);
		            mesa.Dim = this.DimDeObjetos;
		            this.PonerEnTablero(mesa);
		            sePuede = true;
		          }else{
		            sePuede = false;
		            break;
		          }
		       }
		  }
		  if(sePuede)
		     this.DibujarObjeto();
		     
		  return sePuede;
		}
		
		public int NumMesasPlano{
		   get{
        		   int contador = 0;
        		  foreach(ObjBase obj in this.conjObjs){
        		      if(obj is InfMesa)
        		                    contador++;
        		  }
        		  return contador;
        		  }
		}
		
		
	}
	
	
	public class GestorEscenario{
	   
	   
	   public ObjBase ControlSel;
	   public InfMesa mesSel;
	   public LosDatosMesa datosSel;
	   
	   
	   
	   ObjBase objSel;
	   Escenario herr;
	   ObjEstatico sombra;
	   Escenario escVista;
	   string rutsImgs;
	   
	    
	   
	   public GestorEscenario(OnImgModificada OnModificada, Dimension dim, string rutaImgs){
	                     this.rutsImgs = rutaImgs;              
	                     this.escVista = new Escenario("Principal",new Localizacion(0,0), dim, true);
	                     this.escVista.RutaImgs = rutaImgs;
	                     this.escVista.encontradoEsc+=OnEscEncontrado;
	                     this.escVista.encontradoObjIn+=OnMesaEnc;
	                     this.escVista.imgModificada += OnModificada;
	                     sombra = new ObjEstatico(ConstantesImg.sombra, new Dimension(), new Localizacion(),"Sombra");
	                  }
	                  
	      
         void OnMesaEnc(ObjBase obj, Localizacion l){
            
            //Comprueba se hubiera una selecion previa
            // y la deseleccion
            if((this.objSel!=null)&&(!this.objSel.Equals(obj)))
                                               this.DeSeleccionar();
             
             
            if(!obj.Oculto)//No seleccionar si el objeto esta oculto    
                     this.Seleccionar(obj);
                     else
                     //Si estubiese seleccinado antes de ocultar la deselecciono
                     this.DeSeleccionar();
            
            this.objSel = obj;
               
            if(obj is InfMesa){                          
               this.mesSel = (InfMesa)obj;
               this.datosSel = null;
               }else{
               this.datosSel = (LosDatosMesa)obj;
               this.mesSel = null;
              }
         }
                      
	                  
	    void OnEscEncontrado(Escenario esc, Localizacion l){
	          esc.PunteroEnPosicion(new Localizacion(l.X,l.Y));  
        }              
	                  
	    void OnObjControl(ObjBase obj, Localizacion l){
	       //Rutinas para la seleccion y deseleccion de obj
           if((this.ControlSel!=null)&&(!this.ControlSel.Equals(obj)))
                                                this.DeSeleccionar();
            
	        if(!obj.Oculto)
                     this.Seleccionar(obj);
                 else
                     this.DeSeleccionar();
                     
               this.ControlSel = obj;
        }  
        
        void OnObjNoEnc(Localizacion l){
              this.DeSeleccionar();
              this.ControlSel = null;
              this.mesSel = null;
              this.datosSel = null;
         }
         
             
         
	   
	   //Dibuja el control sin el plano
        public void CrearEscHerra(Localizacion loc, Dimension dim){
            herr = new Escenario("Herramientas",loc, dim);
            herr.RutaImgs = rutsImgs;
            herr.encontradoObjIn += OnObjControl;
            herr.noEncontrado += this.OnObjNoEnc;
            Dimension dimObj = new Dimension(60,60);
            Localizacion locObj = new Localizacion(10,5);
            
                   //dibujar la papelera
                   BotonesObj papelera = new BotonesObj(ConstantesImg.papelera,new Dimension(dimObj.Ancho, dimObj.Alto),
                                                                           new Localizacion(locObj.X,locObj.Y),"papelera");
                   herr.AgregarObj(papelera);         
             
                   //dibujar la mesa salon
                   BotonesObj mesa_comedor = new BotonesObj(ConstantesImg.mesa,new Dimension(dimObj.Ancho, dimObj.Alto),
                                                                           new Localizacion(locObj.X,(locObj.Y+dimObj.Alto+13)),"mesa_comedor");
                   herr.AgregarObj(mesa_comedor);         
             
                   
                   //dibujar la mesa Alta
                   BotonesObj mesa_alta = new BotonesObj(ConstantesImg.mesaAlta, new Dimension(dimObj.Ancho, dimObj.Alto),
                                                                           new Localizacion(locObj.X,(locObj.Y+dimObj.Alto+13)*2),"mesa_alta");
                   herr.AgregarObj(mesa_alta);         
             
                   //dibujar la Barra curba
                   BotonesObj barra_curba = new BotonesObj(ConstantesImg.BarraCurba,new Dimension(dimObj.Ancho, dimObj.Alto),
                                                                           new Localizacion(locObj.X,(locObj.Y+dimObj.Alto+13)*3),"barra_curba");
                   herr.AgregarObj(barra_curba);         
             
                   
                   //dibujar la Barra larga
                   BotonesObj barra_larga = new BotonesObj(ConstantesImg.BarraRect,new Dimension(dimObj.Ancho, dimObj.Alto),
                                                                           new Localizacion(locObj.X,(locObj.Y+dimObj.Alto+13)*4),"barra_larga");
                  
                   herr.AgregarObj(barra_larga);         
                   
                   this.escVista.AgregarObj(herr); 
             
             }
             
             
             
             public void PunteroEnPosicion(Localizacion l){
               this.escVista.PunteroEnPosicion(l);
             }
           
             public void QuitarImagen(ObjBase obj){
               this.escVista.QuitarObj(obj);
             }
             
             public void AgregarImg(ObjBase obj){
                if( obj is Planing){
                       (obj as Planing).encontradoObjIn += this.OnMesaEnc;
                       (obj as Planing).noEncontrado += this.OnObjNoEnc;
                }
                        this.escVista.AgregarObj(obj);
             }
             
            public void Seleccionar(ObjBase obj){
                  sombra.Dim = obj.Dim;
                  sombra.Loc = obj.LocRelAlPadre ;
                  this.escVista.AgregarObj(sombra);
             }

	         public void DeSeleccionar(){
	            this.escVista.QuitarObj(sombra);
	         }
	    
	}
	
	
	public class GestorPlaning
	{
	  
	   Marco recPlano;
	   Dimension dimDeObj;
	   string imgSuelo;
	   string rutasImgs;
	   
	   public GestorPlaning(Marco Plano, Dimension dimObj, string imgSuelo, string rutasImgs){	
	     this.dimDeObj = dimObj;
	     this.recPlano = Plano;
	     this.imgSuelo = imgSuelo;
	     this.rutasImgs = rutasImgs;
	   }
	   
	   
	  
	     public void GuardarPlano(Planing plano){
	            FileStream f = new FileStream(Rutas.Ruta_Directa(Rutas.PLANING+"/"+plano.nomZona +".plng"), FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(f, plano);
                f.Close();
	     }
	     
	     public void BorrarPlano(Planing plano){
	          FileInfo f = new FileInfo(Rutas.Ruta_Directa(Rutas.PLANING+"/"+plano.nomZona +".plng"));
	            if(f.Exists){
                 f.Delete();
	         }
	     }
	     
	     public Planing CargarPlano(string nomZona, string id){
	       FileInfo f = new FileInfo(Rutas.Ruta_Directa(Rutas.PLANING+"/"+nomZona +".plng"));
	        if(f.Exists){
	            BinaryFormatter formateAdorBinario = new BinaryFormatter();
                FileStream fReal = new FileStream(f.FullName, FileMode.Open);
                Planing plano = (Planing)formateAdorBinario.Deserialize(fReal);
                plano.RutaImgs = this.rutasImgs;
                fReal.Close();
                return plano;
            }else{
              Planing plano = new Planing(nomZona,id.ToString(),this.recPlano,this.dimDeObj,this.rutasImgs, this.imgSuelo);
              return plano;
	        }
	     }
	     
	    
    }
}
