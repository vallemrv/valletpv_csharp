// TxtHoras.cs created with MonoDevelop
// User: valle at 0:35 15/07/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Valle.GtkUtilidades
{
    
    [System.ComponentModel.Category("Valle.GtkUtilidades")]
    [System.ComponentModel.ToolboxItem(true)]
    public partial class TxtHoras : Gtk.Bin
    {
        
        public string Etiqueta{
           get{
             return this.lblHoras.Text;
           }
           set{
             this.lblHoras.Text = value;
           }
        }
        
        public string Hora{
           get{
             if((this.txtHoras.Text.Length>0)&&(this.txtMinutos.Text.Length>0))
             return this.txtHoras.Text.PadLeft(2,'0')+":"+this.txtMinutos.Text.PadLeft(2,'0');
             else
             return "";
           }
           set{
              if(value.Length>0){
                string[] hpartida = value.Split(':');
                this.txtHoras.Text = hpartida[0].StartsWith("0")&& hpartida[0].Length>2? hpartida[0].Remove(0,1) : hpartida[0];
                if(hpartida.Length>1)
                  this.txtMinutos.Text = hpartida[1].StartsWith("0") && hpartida[1].Length>2 ? hpartida[1].Remove(0,1) : hpartida[1];
                    else
                     this.txtMinutos.Text = "0";
              }else{
                this.txtMinutos.Text = "";
                this.txtHoras.Text = "";
              }
             
           }
           
        }
        
        public TxtHoras()
        {
            this.Build();
            this.Hora = "";
        }
        
        
    }
}
