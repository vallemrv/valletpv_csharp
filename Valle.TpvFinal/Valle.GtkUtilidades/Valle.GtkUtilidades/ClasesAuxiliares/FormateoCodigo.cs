// FormateoCodigo.cs created with MonoDevelop
// User: valle at 22:33 19/12/2009
//
// Licencia GPL
//

using System;
using System.Collections.Generic;

namespace Valle.GtkUtilidades
{
    public class Intervalo{
        public int ini;
        public int fin;
    }
    
    public class FormateoCodigo
    {
            
        List<string> PalReservadasCSharp = new List<string>(("abstract,as,base,break,case,catch,checked,class,"+
                                           "const,continue,default,delegate,do,else,"+
                                           "enum,event,explicit,extern,false,finally,fixed,for,"+
                                           "foreach,goto,if,implicit,in,interface,internal,lock,"+
                                           "is,namespace,new,null,operator,out,override,params,private,"+
                                           "protected,public,readonly,ref,return,sealed,"+
                                           "sizeof,stackalloc,static, struct,switch,this,throw,true,"+
                                           "try,typeof,unchecked,unsafe,"+
                                           "using,virtual,void,while").Split(','));
                                           
        List<string> PalTyposComun = new List<string>( "char,bool,byte,int,double,ushor,string,long,float,object,decimal,sbyte,short,uint,ulong".Split(','));
        
        Gtk.TextBuffer buffer;
        
        private void OnDeleteRange(object o, Gtk.DeleteRangeArgs arg){
            int lineaMax = arg.Start.Line<=arg.End.Line?arg.End.Line:arg.Start.Line;
            int lineaMin = arg.Start.Line<=arg.End.Line?arg.Start.Line:arg.End.Line;
            Gtk.TextIter iterIni = buffer.GetIterAtLine(lineaMin);
            Gtk.TextIter iterFin = buffer.GetIterAtLine(lineaMax);
            iterFin.ForwardToLineEnd();
            buffer.RemoveAllTags(iterIni,iterFin);
            this.FormatearCSharp(lineaMin,lineaMax+1);
        }
        
        private void OnInsertText(object o, Gtk.InsertTextArgs arg){
            int numLineas = arg.Text.Split('\n').Length;
            Gtk.TextIter iter = buffer.GetIterAtLine(arg.Pos.Line);
            iter.BackwardLines(numLineas-1);
            this.FormatearCSharp(iter.Line,iter.Line+numLineas);    
        }
        
        int posComentario(string linea){
           if((linea.Contains("//") && !linea.Contains("\""))||
              ((linea.Contains("//") && linea.Contains("\"")) && 
              (linea.IndexOf("//") < linea.IndexOf("\"")))) return linea.IndexOf("//");
           if(linea.Contains("//") && linea.Contains("\"")){
               Intervalo[] intervalos = posEntrecomillas(linea);
                return linea.IndexOf("//",intervalos[intervalos.Length-1].fin+1);
           }
           return -1;
        }
        
        Intervalo[] posEntrecomillas(string linea){
          List<Intervalo> intervalos = new List<Intervalo>();
          int pos = 0;
            while(pos>=0){
                 Intervalo inter = new Intervalo();
                  inter.ini = pos = linea.IndexOf('"',pos+1);
                 if(pos>=0)
                  inter.fin = pos = linea.IndexOf('"',pos+1);
                 if(pos>=0)
                       intervalos.Add(inter);
               
            } 
            return intervalos.ToArray();
          }
        
        void FormatearCSharp(int inicio, int final){
            
           for(int i= inicio;i<final;i++){
              Gtk.TextIter iterIni = buffer.GetIterAtLine(i);
              Gtk.TextIter iterFin = buffer.GetIterAtLine(i);
              iterFin.ForwardToLineEnd();
              string Strlinea = buffer.GetText(iterIni,iterFin,false);
              Strlinea = Strlinea.Replace("\\\""," ");
                
             //Busqueda de comentarios
              if(Strlinea.Contains("//")){
                 int posCom = posComentario(Strlinea);
                 if(posCom>=0){
                   iterIni.ForwardChars(posCom);
                   buffer.ApplyTag("//",iterIni,iterFin);
                   }
              }
          
              
              //busqueda de cadenas de texto string, char
              int renovados = 0;
              while(Strlinea.Contains("'")){
               int ini = Strlinea.IndexOf("'")+renovados;
                 Strlinea = Strlinea.Remove(ini-renovados,3);
                     renovados += 3;
                     iterIni = buffer.GetIterAtLine(i);
                     iterIni.ForwardChars(ini);
                     iterFin = buffer.GetIterAtLine(i);
                     iterFin.ForwardChars(ini+4);
                     buffer.ApplyTag("string",iterIni,iterFin);
              }
             if(Strlinea.Contains("\"")){
              renovados = 0;
              foreach(Intervalo inter in posEntrecomillas(Strlinea)){
                     Strlinea= Strlinea.Remove(inter.ini-renovados,inter.fin-inter.ini);
                     renovados = inter.fin-inter.ini;
                     iterIni = buffer.GetIterAtLine(i);
                     iterIni.ForwardChars(inter.ini);
                     iterFin = buffer.GetIterAtLine(i);
                     iterFin.ForwardChars(inter.fin+1);
                     buffer.ApplyTag("string",iterIni,iterFin);
              }
             }
             iterIni = buffer.GetIterAtLine(i);
             iterFin = buffer.GetIterAtLine(i);
             int conPal = 0;  
            
            string[] palabras = Strlinea.Trim().Split(' ','(',')',',',';','.','{','}','\t','=');
             foreach(string pal in palabras){
                if(PalReservadasCSharp.Contains(pal)){
                     int iniCon = Strlinea.IndexOf(pal);          
                     iterIni.ForwardChars(iniCon+conPal);
                     iterFin.ForwardChars(iniCon+pal.Length+conPal);
                     buffer.ApplyTag("pal_clave",iterIni,iterFin);
                     iterIni = buffer.GetIterAtLine(i);
                     iterFin = buffer.GetIterAtLine(i);
                     Strlinea = Strlinea.Remove(iniCon,pal.Length);
                     conPal += pal.Length;
                }
             }
          
             iterIni = buffer.GetIterAtLine(i);
             iterFin = buffer.GetIterAtLine(i);
               
             foreach(string pal in palabras){
              
                if(PalTyposComun.Contains(pal)){
                     int iniCon = Strlinea.IndexOf(pal);
                     iterIni.ForwardChars(iniCon+conPal);
                     iterFin.ForwardChars(iniCon+pal.Length+conPal);
                     buffer.ApplyTag("tipos",iterIni,iterFin);
                     iterIni = buffer.GetIterAtLine(i);
                     iterFin = buffer.GetIterAtLine(i);
                     Strlinea = Strlinea.Remove(iniCon,pal.Length);
                     conPal += pal.Length;
                     
                }
             }
          
          }
        }
       
       public FormateoCodigo(Gtk.TextBuffer buff){
            Gtk.TextTag tagComentarios =   new Gtk.TextTag("//");
            Gdk.Color color = Gdk.Color.Zero;
            Gdk.Color.Parse("blue", ref color);
            tagComentarios.ForegroundGdk = color;
            buff.TagTable.Add(tagComentarios);
            Gtk.TextTag tagpal_clave =   new Gtk.TextTag("pal_clave");
            Gdk.Color colorRed = Gdk.Color.Zero;
            Gdk.Color.Parse("dark red", ref colorRed);
            tagpal_clave.ForegroundGdk = colorRed;
            tagpal_clave.Weight = Pango.Weight.Bold;
            buff.TagTable.Add(tagpal_clave);
            Gtk.TextTag tagtipos =   new Gtk.TextTag("tipos");
            Gdk.Color colorGreen = Gdk.Color.Zero;
            Gdk.Color.Parse("dark green", ref colorGreen);
            tagtipos.ForegroundGdk = colorGreen;
            tagtipos.Weight = Pango.Weight.Bold;
            buff.TagTable.Add(tagtipos);
            Gtk.TextTag tagStr =   new Gtk.TextTag("string");
            Gdk.Color colorStr = Gdk.Color.Zero;
            Gdk.Color.Parse("cornflower blue", ref colorStr);
            tagStr.ForegroundGdk = colorStr;
            tagStr.Weight = Pango.Weight.Bold;
            buff.TagTable.Add(tagStr);
           
            this.buffer = buff; 
            this.buffer.DeleteRange += new  Gtk.DeleteRangeHandler(this.OnDeleteRange);
            this.buffer.InsertText += new Gtk.InsertTextHandler(this.OnInsertText);
            
            
       }
        
    }
}
