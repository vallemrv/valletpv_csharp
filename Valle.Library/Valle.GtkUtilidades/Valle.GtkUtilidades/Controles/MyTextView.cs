// MyTextView.cs created with MonoDevelop
// User: valle at 14:13 19/12/2009
//
// Licencia GPL
//

using System;

namespace Valle.GtkUtilidades
{
 
	[System.ComponentModel.Category("Valle.GtkUtilidades")]
    [System.ComponentModel.ToolboxItem(true)]
    public class MyTextView: Gtk.TextView
    {
        public delegate void OnCursorMovido();
        public event OnCursorMovido cursorMovido;
        
        int numLinea = 0;
        public int NumLineaCursor{
              get{
                return numLinea;
              }
        }
        
        int numColum = 0;
        public int NumColum{
              get{
                return numColum;
              }
        }
        
        int numChar = 0;
        public int NumChar{
              get{
                return numChar;
              }
        }
       
        
        public MyTextView()
        {
        }
        
        protected override bool OnButtonPressEvent(Gdk.EventButton e)
        {
           base.OnButtonPressEvent(e);
           Gtk.TextIter iter = this.Buffer.GetIterAtOffset(this.Buffer.CursorPosition);
           Gtk.TextIter iterAux = this.Buffer.GetIterAtOffset(this.Buffer.CursorPosition);
           numLinea = iter.Line;
           numColum = iter.VisibleLineIndex;
           iterAux.BackwardChars(numColum);
           numChar = iterAux.GetText(iter).TrimStart().Length;
           if(cursorMovido!=null)cursorMovido();   
           return true;
        }
        
        protected override void OnMoveCursor (Gtk.MovementStep step, int count, bool extend_selection)
        {
            base.OnMoveCursor (step, count, extend_selection);
            Gtk.TextIter iter = this.Buffer.GetIterAtOffset(this.Buffer.CursorPosition);
            Gtk.TextIter iterAux = this.Buffer.GetIterAtOffset(this.Buffer.CursorPosition);
            numLinea = iter.Line;
            numColum = iter.VisibleLineIndex;
            iterAux.BackwardChars(numColum);
            numChar = iterAux.GetText(iter).TrimStart().Length;
            if(cursorMovido!=null)cursorMovido();
        }


    }
}
