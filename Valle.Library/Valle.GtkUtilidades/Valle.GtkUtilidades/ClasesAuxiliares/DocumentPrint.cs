
using System;
using System.IO;

namespace Valle.GtkUtilidades
{


	public class DocumentPrint : IDisposable
	{
		public enum Alineacion  {centro, derecha, izquierda};
		public enum Tamaño  {pequeña, normal, grande};
		
	   Byte[] agregarLineas = new Byte[]{0x1B, 0x64, 0x6};
	   byte[] tipoEuro = new Byte[]{0x1B, 0x74, 0x13,0x1B,0x52,0x07};	
	   Byte[] saltoDeLinea = new Byte[]{0x1B, 0x64, 0x1};
	   Byte[] iniciarImp = new Byte[]{0x1B, 0x40};	
	   byte[] resaltado = new byte[]{0x1B, 0x21, 0x08};
	   byte[] cortarPapel = new byte[]{	0x1D, 0x56, 0x1};
       byte[] alingCentrado = new byte[]{0x1B, 0x61, 0x31};
       byte[] alingDerecha = new byte[]{0x1B, 0x61, 0x30};	
	   byte[] alingIzquierda = new byte[]{0x1B, 0x61, 0x32};	
	   byte[] tamañoPequeño = new byte[]{0x1B, 0x4D, 0x30};
	   byte[] tamañoNormal = new byte[]{0x1B, 0x4D, 0x31};	
	   byte[] tamañoGranade = new byte[]{0x1B, 0x21, 10};	
	   byte[] tamañoGrandeYNegrita = new byte[]{0x1B, 0x21, 18};
	   byte[] abrirCajon = new byte[]{0x1B, 0x70, 0x00, 0x0D, 0x0A};
	   byte[] imprimeLogo = new byte[]{0x1C, 0x70, 0x01, 0x30}; 

	
	   FileStream document;
	   String fTmp = Valle.Utilidades.RutasArchivos.Ruta_Completa("/tmpPrint");
	   string nomImp;
	  
		#region IDisposable implementation
	   public void Dispose ()
	   {
		 document.Close();
	   	 if(File.Exists(fTmp)) File.Delete(fTmp);
	   }
	   
	   #endregion
	   	
		public void IniciarDoc(string nomImp){
		   this.nomImp = nomImp;
			document = new FileStream(fTmp,FileMode.Create);
			document.Write(iniciarImp,0,iniciarImp.Length);
		}
		
		
		void AddBytes(Byte[] b){
		    	document.Write(b,0,b.Length);
		}
		
		void AddALineamiento( Alineacion aling){
			AddBytes(tipoEuro);
			
			switch(aling){
			    case Alineacion.centro:	
				   AddBytes(alingCentrado);
				break;
				case Alineacion.izquierda:	
				   AddBytes(alingIzquierda);
				break;
				case Alineacion.derecha:	
				    AddBytes(alingDerecha);
				break;
			}
		}
		
		void AddNegrita(){
			AddBytes(resaltado);
		}
		
		void AddTamaño(Tamaño tamaño){
		     switch(tamaño){
			    case Tamaño.normal:	
				   AddBytes(tamañoNormal);
				break;
				case Tamaño.pequeña:	
				   AddBytes(tamañoPequeño);
				break;
				case Tamaño.grande:	
				    AddBytes(tamañoGranade);
				break;
			}	
		}
		
		public void ImprimirLogo(){
			AddBytes(imprimeLogo);
		}
		
		public void AbrirCajon(){
		   AddBytes(abrirCajon);
			document.Close();
			System.Diagnostics.Process lp = new System.Diagnostics.Process();
			lp.StartInfo.FileName = "lp";
			lp.StartInfo.Arguments = "-d "+ nomImp +" "+ fTmp;
			lp.Start();
		    lp.WaitForExit(3000);
		}
		
		public void AddLinea(){
			AddBytes(saltoDeLinea);
		}
		
		
		
		public void AddLinea(string str){
			str = str.Replace('€',Convert.ToChar(213));
			
			byte[] lineaByte = Valle.Utilidades.Convertir.StringAbytes(str);
		
			AddBytes(tipoEuro);
			AddBytes(lineaByte);
			
			AddBytes(saltoDeLinea);
		}
		
		
		public void AddLinea(string str, Alineacion aling){
			str = str.Replace('€', Convert.ToChar(213));
			
			AddBytes(tipoEuro);
			AddALineamiento(aling);
			AddBytes(Valle.Utilidades.Convertir.StringAbytes(str));
				AddBytes(saltoDeLinea);
				AddBytes(iniciarImp);
		}
		
		public void AddLinea(string str, Tamaño t){
			str = str.Replace('€',Convert.ToChar(213));
			 
			AddBytes(tipoEuro);
			AddTamaño(t);
			AddBytes(Valle.Utilidades.Convertir.StringAbytes(str));
				AddBytes(saltoDeLinea);
				AddBytes(iniciarImp);
		}
		
		public void AddLinea(string str, Alineacion aling, Tamaño t, bool negrita){
			str = str.Replace('€',Convert.ToChar(213));
			
			AddBytes(tipoEuro);
			AddALineamiento(aling);
			if(negrita && (t==Tamaño.grande)) AddBytes(tamañoGrandeYNegrita);
			else  if (negrita && (t!=Tamaño.grande)) AddNegrita();
		 	AddBytes(Valle.Utilidades.Convertir.StringAbytes(str));
				AddBytes(saltoDeLinea);
				AddBytes(iniciarImp);
		}
		
		public void AddLinea(string str, bool negrita){
			str = str.Replace('€',Convert.ToChar(213));
			
			AddBytes(tipoEuro);
			if (negrita) AddNegrita();
			AddBytes(Valle.Utilidades.Convertir.StringAbytes(str));
				AddBytes(saltoDeLinea);
				AddBytes(iniciarImp);
		}
		
		
		
		public void ImprimirDoc(){
			AddBytes(agregarLineas);
			AddBytes(cortarPapel);
			document.Close();
			System.Diagnostics.Process lp = new System.Diagnostics.Process();
			lp.StartInfo.FileName = "lp";
			lp.StartInfo.Arguments = "-d "+ nomImp +" "+ fTmp;
			lp.Start();
		    lp.WaitForExit(3000);
		}
		
	}
}
