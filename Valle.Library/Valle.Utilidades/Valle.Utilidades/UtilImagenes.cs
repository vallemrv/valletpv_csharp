// UtilImagenes.cs created with MonoDevelop
// User: valle at 14:46 15/06/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace Valle.Utilidades.Imagenes
{

         public delegate void OnEscenarioEncontrado(Escenario esc, Localizacion l);
         public delegate void OnObjInteractivoEncontrado(ObjBase objIn, Localizacion l);
         public delegate void OnImgModificada(Bitmap imagen);
         public delegate void OnObjNoEnc(Localizacion l);
         public enum TipoObjeto {Estatico, Interactivo, Escenario}
        
        
         
        [Serializable]
        public class Localizacion{
            public int X;
            public int Y;
            public Localizacion(int x,int y){
               X=x;Y=y;
            }
            public Localizacion(){}
        }
        
        [Serializable]
        public class Dimension{
            public int Alto;
            public int Ancho;
            public Dimension(int ancho, int alto){
               Alto=alto;Ancho=ancho;
            }
            public Dimension(){}
        }
        
        [Serializable]
        public class Marco{
           public Localizacion localizacion;
           public Dimension dimension;
           public int Alto{
              get{
                return dimension.Alto;
              }
              set{
                dimension.Alto = value;
              }
           }
           
           public int Ancho{
              get{
                return dimension.Ancho;
              }
              set{
                dimension.Ancho = value;
              }
           }
           public int X{
              get{
                return localizacion.X;
              }
              set{
                localizacion.X= value;
              }
           }
           public int Y{
              get{
                return localizacion.Y;
              }
              set{
                localizacion.Y = value;
              }
           }
           
           
           public Marco(Localizacion localizacion, Dimension dimension){
                this.localizacion = localizacion;this.dimension = dimension;
           }
           
           public Marco(int x,int y,int ancho,int alto){
             this.localizacion = new Localizacion(x,y);
             this.dimension = new Dimension(ancho,alto);
           }
        }
        
        
        [Serializable]
        public class ObjBase{
           
           public string IDObjeto;
           
           
           
           public ObjBase escPadre
           {
              get{
                return padre;
                }
              set{
                padre = value;
                this.CargarFoto();
              }
           }
           
           
           protected ObjBase padre;
           protected string rutaImgs="";
           protected string nomImg="";
           protected bool ocultar= false;
           protected Dimension dimension;
           protected TipoObjeto tipo;
           
           [NonSerialized] protected Bitmap imgObj;
           protected Localizacion localizacion;
          
            public Localizacion LocRelAlPadre{
             get{
               Localizacion l = new Localizacion(Loc.X,Loc.Y);
               if(this.escPadre != null) {
                  l.X +=  escPadre.LocRelAlPadre.X;
                  l.Y +=  escPadre.LocRelAlPadre.Y;
               }
               return l;
             }
           }
           
           public Bitmap ImgObj{
             get{
               return this.imgObj;
             }
           }
           
           public string NomImg
           {
              get{
                return nomImg;
              }
              set{
                 this.nomImg = value;
              }
           }
           
           
           public TipoObjeto Tipo{
             get{
               return tipo;
               }
           }

           public Dimension Dim {
               get {
                   return dimension;
               }
               set {
                  dimension = new Dimension(value.Ancho,value.Alto);
               }
           }

           public Localizacion Loc {
               get {
                   return localizacion;
               }
               set {
                   localizacion = new Localizacion(value.X,value.Y);
               }
           }

           public bool Oculto {
               get {
                   return ocultar;
               }
               set {
                  if(ocultar != value){
                        ocultar = value;
                        this.DibujarObjeto();
                        }
               }
           }
           
           public ObjBase(TipoObjeto tipo){
              this.tipo = tipo;
           }
          
           public virtual void DibujarObjeto(){
              if(escPadre!=null) this.escPadre.DibujarObjeto();
           }
            
          public virtual void RotarImagen(RotateFlipType tipo){
            this.imgObj.RotateFlip(tipo);
            this.DibujarObjeto();
           }
           
          public virtual void CargarFoto(){
            if(escPadre!=null){
               if((escPadre.rutaImgs.Length>0)&&(this.nomImg.Length >0))
                    this.imgObj = new Bitmap(escPadre.rutaImgs+this.nomImg);
               this.DibujarObjeto();
               }
          }
      
        }       
        
        
        [Serializable]
        public class ObjEstatico : ObjBase{
           
           public ObjEstatico(string nomImg, Dimension dim, Localizacion loc, string id):
                      base(TipoObjeto.Estatico)
             {
               this.nomImg = nomImg; this.dimension = dim; this.localizacion = loc;
               this.IDObjeto = id;
            }
        }
        
        [Serializable]
        public class ObjInteractivo<T> : ObjBase{
           public T Datos;
           public ObjInteractivo(string nomImg, Dimension dim, Localizacion loc, string ID): 
                   base(TipoObjeto.Interactivo)
           {
               this.nomImg =  nomImg; this.dimension = dim; this.localizacion = loc; this.IDObjeto = ID;
           }
           public ObjInteractivo(Dimension dim, Localizacion loc, string ID): 
                   base(TipoObjeto.Interactivo)
           {
               this.dimension = dim; this.localizacion= loc; this.IDObjeto = ID;
           }
        }
        
        [Serializable]
        public class ObjInteractivo : ObjInteractivo<object>{
           public ObjInteractivo(string nomImg, Dimension dim, Localizacion loc, string ID): 
                   base(nomImg,dim,loc,ID)
           {
               this.nomImg = nomImg; this.dimension = dim; this.localizacion= loc; this.IDObjeto = ID;
           }
           public ObjInteractivo(Dimension dim, Localizacion loc, string ID): 
                   base(dim,loc,ID)
           {
               this.dimension = dim; this.localizacion= loc; this.IDObjeto = ID;
           }
        }
        
        [Serializable]
        public class Escenario : ObjBase, ISerializable{
     
          
           public event OnObjNoEnc noEncontrado;
           public event OnEscenarioEncontrado encontradoEsc;
           public event OnObjInteractivoEncontrado encontradoObjIn;
           public event OnImgModificada imgModificada;
           
             public bool esMaxNivel = false;
           
             public string RutaImgs{
                set{
                  this.rutaImgs = value;
                  foreach(ObjBase obj in this.conjObjs){
                    obj.CargarFoto();
                  }
                }
                get{
                  return this.rutaImgs;
                }
             }
           
         
            //Referencias para poder dibujar el contenido en el escenario
            protected List<ObjBase> conjObjs;
            
            //utilizo un array de pilas para saber si el objeto esta encima de otro
            //Guardo una referencia de un objeto en toda la dimension de este
            protected List<ObjBase>[,] tableroCordenadas;
            
             public Escenario(SerializationInfo info, StreamingContext context):
               base(TipoObjeto.Escenario)
                {
                    this.dimension = (Dimension)info.GetValue("dimension",typeof(Dimension));
                    this.localizacion = (Localizacion)info.GetValue("localizacion",typeof(Localizacion));
                    this.IDObjeto = info.GetString("id");
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
                    info.AddValue("dimension", dimension, typeof(Dimension));
                    info.AddValue("localizacion", localizacion, typeof(Localizacion));
                    info.AddValue("id", this.IDObjeto);
                    ObjBase[] objs = this.conjObjs.ToArray();
                    info.AddValue("numObj",objs.Length);
                    for(int i= 0; i<objs.Length;i++)
                        info.AddValue("obj"+i, objs[i], typeof(ObjBase));
                  
                 }
              #endregion
              
                
            public Escenario(string IDEscenario, Localizacion loc, Dimension dim, bool esMaxNivel):
              base(TipoObjeto.Escenario)
            {
                 this.IDObjeto = IDEscenario; 
                 this.localizacion = loc; 
                 this.dimension = dim;
                 this.esMaxNivel = esMaxNivel;
                 this.inicializarVariables();
            }
            
            public Escenario(string IDEscenario, Localizacion loc, Dimension dim):
              base(TipoObjeto.Escenario)
            {
                 this.IDObjeto = IDEscenario; 
                 this.localizacion = loc; 
                 this.dimension = dim;
                 this.inicializarVariables();
            }
            
            
            protected void inicializarVariables(){
              this.conjObjs = new List<ObjBase>();
              this.imgObj = new Bitmap(Dim.Ancho, Dim.Alto);
              Graphics g = Graphics.FromImage(this.imgObj);
              g.Clear(Color.Wheat);
              this.tableroCordenadas = new List<ObjBase>[Dim.Ancho, Dim.Alto];
            }
            
            protected virtual void OnObjNoEncontrado(Localizacion l){
              if(this.noEncontrado!=null) this.noEncontrado(l);  
            }
            
            protected virtual void OnObjIntEnc(ObjBase obj, Localizacion l){
              if(this.encontradoObjIn!=null) this.encontradoObjIn(obj,l);
            }
            
            protected virtual void OnEscenarioEnc(ObjBase obj, Localizacion l){
              if(this.encontradoEsc!=null) this.encontradoEsc((Escenario) obj,l);
            }
            
            protected virtual void OnImgMod(){
               if(this.imgModificada!=null) this.imgModificada(this.ImgObj);
            }
            
            public override void DibujarObjeto ()
            {
              if((escPadre!=null)||(this.esMaxNivel)){
               Graphics g = Graphics.FromImage(this.imgObj);
               g.Clear(Color.Wheat);
              
               foreach (ObjBase obj in conjObjs){
                 if(!obj.Oculto)
                     g.DrawImage(obj.ImgObj,obj.Loc.X,obj.Loc.Y,obj.Dim.Ancho,obj.Dim.Alto);
                 }
               
                this.OnImgMod();
                base.DibujarObjeto ();
              }
             }
            
            

             protected virtual bool PuedoDibujar(Marco marco){
              if((marco.localizacion.X<0)||(marco.localizacion.Y < 0)
                  ||((marco.localizacion.X+marco.dimension.Ancho)>this.Dim.Ancho)||
                    ((marco.localizacion.Y + marco.dimension.Alto) > this.Dim.Alto))
                        return false;
                        else
                        return true;
             }
           
           protected virtual void PonerEnTablero(ObjBase obj){
              lock(this.tableroCordenadas){ 
                 for(int i= obj.Loc.X; i< obj.Loc.X + obj.Dim.Ancho;i++){
	                for(int j= obj.Loc.Y; j< obj.Loc.Y + obj.Dim.Alto;j++){
	                     if(this.tableroCordenadas[i,j]==null)
	                            this.tableroCordenadas[i,j] = new  List<ObjBase>();
	                     
	                       this.tableroCordenadas[i,j].Add(obj);
	          	}
	           }
	         }
	        }
           
           protected virtual void QuitarEnTablero(ObjBase obj){
               lock(this.tableroCordenadas){
                 for(int i= obj.Loc.X; i< obj.Loc.X + obj.Dim.Ancho;i++)
	               for(int j= obj.Loc.Y; j< obj.Loc.Y + obj.Dim.Alto;j++)
	                                  this.tableroCordenadas[i,j].Remove(obj);
	            }                                    
           }
             
           
           public virtual void AgregarObj(ObjBase obj){
            
            if(!this.conjObjs.Contains(obj)){
                     
               if(PuedoDibujar(new Marco(obj.Loc,obj.Dim))){
                 this.conjObjs.Add(obj);
              
                 if((obj.Tipo== TipoObjeto.Escenario)||
                                 (obj.Tipo== TipoObjeto.Interactivo)){
                             this.PonerEnTablero(obj);
                            
                         }
                  
                  obj.escPadre = this;
	              this.DibujarObjeto();
               }
             }
           }
           
           public virtual void QuitarObj(ObjBase obj){
              if(this.conjObjs.Remove(obj)){
                   if((obj.Tipo== TipoObjeto.Escenario)||
                                 (obj.Tipo== TipoObjeto.Interactivo)){  
                                                        this.QuitarEnTablero(obj);
                                                        obj.escPadre = null;
                                                        }
                     this.DibujarObjeto();                                   
                  } 
           
           }
          
          
          public virtual void MoverObj(ObjBase obj, Localizacion locNueva){
                if(this.PuedoDibujar(new Marco(locNueva, obj.Dim))){
                     this.QuitarEnTablero(obj);
                     obj.Loc = locNueva;
                     this.PonerEnTablero(obj);
                     this.DibujarObjeto();
                }
          }
        
          //Modifico las cordenada del puntero
          //a cordenadas del objeto segun su localizacion
          //si el puntero esta encima de un objeto este genera su evento correspondiente
          public virtual void PunteroEnPosicion(Localizacion l){
            int xReal = l.X-this.Loc.X;
            int yReal = l.Y-this.Loc.Y;
            lock(this.tableroCordenadas){
                    if((xReal<0)||(xReal>Dim.Ancho)||(yReal<0)||(yReal>Dim.Alto)||
                                     (this.tableroCordenadas[xReal,yReal] == null)||
                                        (this.tableroCordenadas[xReal,yReal].Count <= 0))
                                                                       this.OnObjNoEncontrado(l);
                     else{
                       ObjBase obj =  this.tableroCordenadas[xReal,yReal]
                                           [this.tableroCordenadas[xReal,yReal].Count-1];
                          if(obj.Tipo== TipoObjeto.Escenario){
                                        this.OnEscenarioEnc(obj,l);
                                  }else
                                      this.OnObjIntEnc(obj,l);
                     
               }    
           }
        }
     }   
    
    
    
    public class UtilImagenes
    {
    
        public static Bitmap Escalar(Bitmap img, Dimension dimNueva){
           Bitmap b = new Bitmap(dimNueva.Ancho,dimNueva.Alto);
           Graphics g = Graphics.FromImage(b);
           g.DrawImage(img,0,0,dimNueva.Ancho,dimNueva.Ancho);
           img.Dispose(); GC.Collect();
           return b;
        }
        
        public static Bitmap ClonarImg(Bitmap img){
            return img.Clone(new Rectangle(0,0,img.Width,img.Height),img.PixelFormat);
        }
        
        public static byte[] DeBitmapABytes(Bitmap img){
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms,ImageFormat.Jpeg);
                return ms.ToArray();
        }
        
    }
}
