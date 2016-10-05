using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Drawing;

using Valle.SqlGestion;
using Valle.SqlUtilidades;
using Valle.Utilidades;

namespace Valle.ToolsTpv
{
	public enum TratandoMesas
	{
		Juntando,
		Cambiando,
		invitando,
		no
	}
	
	public class InfTeclaMesa: Utilidades.IInfBoton{
		
		public String zona;
		public string mesa;
		public Color colorDeAtras;
		public int Tarifa;
		
		public InfTeclaMesa(string nomMesa, string nomZona, Color color)
		{
		    zona = nomZona;
	        mesa = nomMesa;
		    this.colorDeAtras = color;
			Tarifa = 1;
		}
		
		public InfTeclaMesa(){
			zona = "";
	        mesa = "Barra";
		    this.colorDeAtras = SystemColors.Control;
			Tarifa = 1;
		}
		
		#region IComparable implementation
		public int CompareTo (object obj)
		{
			return this.Orden.CompareTo((obj as ObjOrdenable).Orden);
		}
		#endregion
		
		#region IInfBoton implementation
		public object Datos {
			get {
				return this;
			}
			set {}
		}

		public Color ColorDeAtras {
			get {
				return colorDeAtras;
			}
			set {
			    this.colorDeAtras = value;
			}
		}

		public string Texto {
			get {
				return this.mesa ;
			}
			set {
				this.mesa = value;
			}
		}

		public Bitmap ImgFondo {
			get {
				return null;
			}
			set {
			}
		}
		Size tamaño = new Size(100,100);
		public Size Tamaño {
			get {
				return tamaño;
			}
			set {
				this.tamaño = value;
			}
		}
		#endregion

		#region ObjOrdenable implementation
		public int Orden {
			get {
				return 0;
			}
			set {
			}
		}
		#endregion
		#region IInfBoton implementation
		System.Drawing.Font font = new Font("sanz",12);
		public Font Font {
			get {
				return font;
			}
			set {
				font = value;
			}
		}
		
		#endregion
	}
	
	[Serializable()]
	public class LineaNula
	{
		public Articulo linea;
		public String camareroAnula;
		public DateTime HoraAnulado;
		public LineaNula(Articulo art, String camareroNul, DateTime HAnulado)
		{
			linea = art;
			HoraAnulado = HAnulado;
			camareroAnula = camareroNul;
		}
	}
	
	[Serializable()]
	public class Ronda
	{
		public Hashtable lineasArtActivos;
		public Hashtable lineasArtCobrados;
		public List<LineaNula> nulas;
		public DateTime horaServido;
		public String nomCamarero;
		public int numTicketPertenencia;
		public void AgregarLinea(Articulo art)
		{
			Articulo articuloB;
			if (lineasArtActivos.Contains(art.IDArticulo.ToString() + art.precio.ToString() + art.Descripcion)) {
				articuloB = (Articulo)lineasArtActivos[art.IDArticulo.ToString() + art.precio.ToString() + art.Descripcion];
				articuloB.TotalLinea += art.TotalLinea;
				articuloB.Cantidad += art.Cantidad;
			} else {
				lineasArtActivos.Add(art.IDArticulo.ToString() + art.precio.ToString() + art.Descripcion, art);
				art.CombidadPertenencia = this;
			}
		}
		public void QuitarLinea(Articulo art, String camarero)
		{
			if (lineasArtActivos.Contains(art.IDArticulo.ToString() + art.Precio.ToString() + art.Descripcion)) {
				Articulo articuloB = (Articulo)lineasArtActivos[art.IDArticulo.ToString() + art.Precio.ToString() + art.Descripcion];
				if (articuloB.Cantidad > art.Cantidad) {
					articuloB.TotalLinea -= art.TotalLinea;
					articuloB.Cantidad -= art.Cantidad;
				} else {
					lineasArtActivos.Remove(art.IDArticulo.ToString() + art.precio.ToString() + art.Descripcion);
				}
			}
			if (camarero != null) {
				nulas.Add(new LineaNula(art, camarero, DateTime.Now));
			}
		}
		
		public void CobrarLinea(Articulo art)
		{
			if (lineasArtCobrados.Contains(art.IDArticulo.ToString() + art.Precio.ToString() + art.Descripcion)) {
				Articulo articuloB = (Articulo)lineasArtCobrados[art.IDArticulo.ToString() + art.Precio.ToString() + art.Descripcion];
				articuloB.TotalLinea += art.TotalLinea;
				articuloB.Cantidad += art.Cantidad;
			} else {
				lineasArtCobrados.Add(art.IDArticulo.ToString() + art.Precio.ToString() + art.Descripcion, art);
				art.CombidadPertenencia = this;
			}
			this.QuitarLinea(art, null);
		}
		public Ronda()
		{
			lineasArtActivos = new Hashtable();
			lineasArtCobrados = new Hashtable();
			nulas = new List<LineaNula>();
		}
	}
	[Serializable()]
	public class Mesa
	{
		public List<Ronda> RondasActivas;
		public List<Ronda> RondasPagadas;
		public List<Articulo> LineasTemp;
		public Color colorDefecto;
		public String zona;
		public string mesa;
		public String camareroActivo;
		public string camareroAbreMesa;
		public DateTime HoraApertura;
		public int numCopiasTicket = 0;
		public int Tarifa = 1;
		
		public Mesa(string nomMesa, string nomZona, Color color)
		{
		    zona = nomZona;
	        mesa = nomMesa;
		    this.colorDefecto = color;
			RondasActivas = new List<Ronda>();
			RondasPagadas = new List<Ronda>();
			LineasTemp = new List<Articulo>();
		}
		public void RondaNueva(Ronda conv)
		{
			RondasActivas.Add(conv);
		}
		public Articulo[] ListadoArtActivos()
		{
			List<Articulo> artsList = new List<Articulo>();
			foreach (Ronda c in RondasActivas) {
				IEnumerator enumeracion = c.lineasArtActivos.Values.GetEnumerator();
				while (enumeracion.MoveNext()) {
					artsList.Add((Articulo)enumeracion.Current);
				}
			}
			foreach (Articulo art in LineasTemp) {
				artsList.Add(art);
			}
			return artsList.ToArray();
		}
		public bool EstaVacia {
			get { return RondasActivas.Count + RondasPagadas.Count + LineasTemp.Count <= 0; }
		}
		public bool LineasPendienteDeCobro {
			get { return RondasActivas.Count + LineasTemp.Count > 0; }
		}
	}
    
    [Serializable()]
	public class ZonaMesas : Valle.Utilidades.IInfBoton
	{
	    public string nomZona;
		public int botonesAncho;
		public int botonesAlto;
		public Color colorZona;
		
		#region IComparable implementation
		public int CompareTo (object obj)
		{
			return this.Orden.CompareTo((obj as ObjOrdenable).Orden);
		}
		#endregion
		
		#region IInfBoton implementation
		public object Datos {
			get {
				return this;
			}
			set {
				
			}
		}
		
		
		public Color ColorDeAtras {
			get {
				return colorZona;
			}
			set {
				colorZona = value;;
			}
		}
		
		
		public string Texto {
			get {
				return nomZona;
			}
			set {
				nomZona = value;
			}
		}
		
		Bitmap b = null;
		public Bitmap ImgFondo {
			get {
				return b;
			}
			set {
				b=value;
			}
		}
		
		Size t = new Size(100,100);
		public Size Tamaño {
			get {
				return t;
			}
			set {
				t = value;
			}
		}
		
		Font f = new Font("monospace",10f);
		public Font Font {
			get {
				return f;
			}
			set {
				f =  value;
			}
		}
		
		#endregion
		#region ObjOrdenable implementation
		int o = 0;
		public int Orden {
			get {
				return o;
			}
			set {
				o = value;
			}
		}
		
		#endregion
		public ZonaMesas(string nom, int alto, int ancho, Color cZona)
		{
			nomZona = nom;
			botonesAlto = alto;
			botonesAncho = ancho;
			colorZona = cZona;
		}
	}

	public class GesMesas  
	{
	   
	     
	    public IGesSql gesBase;
		public String Rut_mesas;
		public GuardarTicket guardarTicket;
		public TratandoMesas mesaEntratamiento = TratandoMesas.no;
		public Dictionary<string, InfTeclaMesa> TodosBtnMesas = new Dictionary<string, InfTeclaMesa>();
		public int tarifa = 1;

		public TratandoMesas MesaEntratamiento {
			get { return mesaEntratamiento; }
			set { mesaEntratamiento = value; }
		}
		
		private Mesa mesaActiva;
        
		public Mesa MesaActiva {
			get { return mesaActiva; }
			set {
				mesaActiva = value;
				nomMesaActiva = mesaActiva.mesa;
			}
		}
		public Mesa mesaAuxiliar;

		public Mesa MesaAuxiliar {
			get { return mesaAuxiliar; }
			set { mesaAuxiliar = value; }
		}
		public String nomMesaActiva;

		public string NomMesaActiva {
			get { return nomMesaActiva; }
			set { nomMesaActiva = value; }
		}
		public List<ZonaMesas> zonas = new List<ZonaMesas>();

		public List<ZonaMesas> Zonas {
			get { return zonas; }
			set { zonas = value; }
		}
		public Hashtable mesas_botones = new Hashtable();

		public Hashtable Mesas_botones {
			get { return mesas_botones; }
			set { mesas_botones = value; }
		}
		
    
        public GesMesas(IGesSql gesBase,string port, string protocolo, GuardarTicket gesTicket, string rutaMesas,Valle.Distribuido.GesMensajes gm){
            
			this.gesBase = gesBase;
			this.Rut_mesas = rutaMesas;
			this.guardarTicket = gesTicket;
	      }
        
     
		

		public bool EstaLaMesaAbierta(string nomMesa)
		{
         	FileInfo mesa = new FileInfo(Rut_mesas + Path.DirectorySeparatorChar + nomMesa);
			return mesa.Exists;
			}
		
		public void MesaNueva(InfTeclaMesa btnMesa, String camarero)
		{
			mesaActiva = new Mesa(btnMesa.mesa,btnMesa.zona,btnMesa.colorDeAtras);
			mesaActiva.camareroAbreMesa = camarero;
			mesaActiva.camareroActivo = camarero;
			mesaActiva.HoraApertura = DateTime.Now;
			mesaActiva.Tarifa = btnMesa.Tarifa;
			btnMesa.colorDeAtras = Color.Purple;
			
			
		}
		
		Mesa Deserializar(string nomMesa){
		   BinaryFormatter formateAdorBinario = new BinaryFormatter();
           FileInfo mesa = new FileInfo(Rut_mesas + Path.DirectorySeparatorChar + nomMesa);
		   FileStream f = mesa.OpenRead();
		   Mesa m = (Mesa)formateAdorBinario.Deserialize(f); f.Close();
		   return m;
		   
		}
		
		private void CargarMesa(string nomMesa){
		 
		     mesaActiva = Deserializar(nomMesa);
			}

		public bool AbrirMesa(InfTeclaMesa btnMesa, String camarero)
		{
			 this.CerrarConvida();
		    	nomMesaActiva = btnMesa.mesa;
					if (this.EstaLaMesaAbierta(nomMesaActiva)) {
						try {
							this.CargarMesa(nomMesaActiva);
							mesaActiva.camareroActivo = camarero;
						} catch {
							MesaNueva(btnMesa, camarero);
						}
					} else {
						MesaNueva(btnMesa, camarero);
					}
					return true;
		}
		
		public void CerrarConvida()
		{
			if ((mesaActiva != null) && (mesaActiva.LineasTemp.Count > 0)) {
				LlenarRonda(mesaActiva.LineasTemp);
				mesaActiva.LineasTemp.Clear();
			}

			GuardarMesa();
		}
		
		public void CerrarMesa()
		{
		   
			if (mesaActiva != null) {
			  	FileInfo f = new FileInfo(Rut_mesas + Path.DirectorySeparatorChar + nomMesaActiva);
				f.Delete();
			
				if(TodosBtnMesas.ContainsKey(mesaActiva.mesa+MesaActiva.zona))
					TodosBtnMesas[mesaActiva.mesa+MesaActiva.zona].colorDeAtras = mesaActiva.colorDefecto;
					
				
				
				mesaActiva = null;
				GC.Collect();
				
			}

		}
		public void GuardarMesa()
		{
  
			if (mesaActiva != null) {
			 	FileStream f = new FileStream(Rut_mesas + Path.DirectorySeparatorChar + nomMesaActiva, FileMode.OpenOrCreate, FileAccess.Write);
				BinaryFormatter b = new BinaryFormatter();
				b.Serialize(f, mesaActiva);
				f.Close();
			}
		}
		public void CobrarMesaEnBase(object mesa)
		{
			guardarTicket.guardarMesas((Mesa)mesa, 0);
		}
		
		public Articulo[] ListaDeArticulos()
		{
			return mesaActiva.ListadoArtActivos();
		}
		
		public void CobrarLinea(Articulo art, int numTicket)
		{
			art.NumTicketPertenencia = numTicket;
			art.CombidadPertenencia.CobrarLinea(art);
			if (art.CombidadPertenencia.lineasArtActivos.Count <= 0) {
				CobrarRonda(art, numTicket);
			}
			GuardarMesa();
		}
		
		public void AnularLinea(Articulo art)
		{
			if (art.CombidadPertenencia != null) {
				art.CombidadPertenencia.QuitarLinea(art, mesaActiva.camareroActivo);
				if (art.CombidadPertenencia.lineasArtActivos.Count <= 0) {
					mesaActiva.RondasActivas.Remove(art.CombidadPertenencia);
					mesaActiva.RondasPagadas.Add(art.CombidadPertenencia);
				}
			} else {
				mesaActiva.LineasTemp.Remove(art);
			}
			GuardarMesa();
		}
		
		public void CobrarRonda(Articulo artPrimerLista, int numTicket)
		{
			artPrimerLista.CombidadPertenencia.numTicketPertenencia = numTicket;
			mesaActiva.RondasActivas.Remove(artPrimerLista.CombidadPertenencia);
			mesaActiva.RondasPagadas.Add(artPrimerLista.CombidadPertenencia);
			GuardarMesa();
		}
		
		public void LLenarIden(Ronda conv)
		{
			Ronda convAux = new Ronda();

			IEnumerator enumeracion = conv.lineasArtActivos.GetEnumerator();
			while (enumeracion.MoveNext()) {
				Articulo art = (Articulo)enumeracion.Current;
				convAux.lineasArtActivos.Add(art.IDArticulo.ToString() + art.Precio.ToString() + art.Descripcion, art);
			}
			convAux.horaServido = DateTime.Now;
			convAux.nomCamarero = mesaActiva.camareroActivo;
			mesaActiva.RondaNueva(convAux);
			GuardarMesa();
		}
		
		public void LlenarRonda(List<Articulo> arts)
		{
			Ronda convAux = new Ronda();
			foreach (Articulo art in arts) {
				convAux.AgregarLinea(art);
			}
			convAux.horaServido = DateTime.Now;
			convAux.nomCamarero = mesaActiva.camareroActivo;
			mesaActiva.RondaNueva(convAux);
			GuardarMesa();
		}
		
		public void Llenarlinea(Articulo art)
		{
			if (mesaActiva != null) {
				mesaActiva.LineasTemp.Add(art);
				GuardarMesa();
			}
		}
		
		public void CrearBotonera(int idTpv)
		{
			DataTable tb = gesBase.EjecutarSqlSelect("ZonasTpv", "SELECT Zonas.IDZona, Zonas.Nombre,Zonas.Alto, Zonas.Ancho, Zonas.tarifa, " + "Colores.Rojo, Colores.Verde, Colores.Azul FROM Zonas LEFT OUTER JOIN Colores ON " + "Zonas.IDColor = Colores.IDColor INNER JOIN ZonasTpv ON Zonas.IDZona = ZonasTpv.IDZona " + "WHERE ZonasTpv.IDTpv = " + idTpv.ToString());
			DataTable tbMesas = gesBase.EjecutarSqlSelect("Mesas", "SELECT * FROM Mesas");


			for (int i = 0; i < tb.Rows.Count; i++) {
				Color cl = Color.FromArgb((!tb.Rows[i]["Rojo"].GetType().Name.Equals("DBNull")) ? (int)tb.Rows[i]["Rojo"] : Convert.ToInt32(Color.Gray.R), (!tb.Rows[i]["Verde"].GetType().Name.Equals("DBNull")) ? (int)tb.Rows[i]["Verde"] : Convert.ToInt32(Color.Gray.G), (!tb.Rows[i]["Azul"].GetType().Name.Equals("DBNull")) ? (int)tb.Rows[i]["Azul"] : Convert.ToInt32(Color.Gray.B));
				DataRow[] drs = tbMesas.Select("IDZona = " + tb.Rows[i]["IDZona"].ToString());
				InfTeclaMesa[] btn = new InfTeclaMesa[drs.Length];
				crearBontones(btn, drs, cl, (int)tb.Rows[i]["tarifa"], tb.Rows[i]["Nombre"].ToString());
				this.zonas.Add(new ZonaMesas(tb.Rows[i]["Nombre"].ToString(), (int)tb.Rows[i]["Alto"], (int)tb.Rows[i]["Ancho"], cl));
				this.mesas_botones.Add(tb.Rows[i]["Nombre"].ToString(), btn);

			}

		}
		
		void crearBontones(InfTeclaMesa[] btnMesa, DataRow[] rs, Color cl, int tarifa, string zona)
		{

			for (int i = 0; i < rs.Length; i++) {

				btnMesa[i] = new InfTeclaMesa();
				btnMesa[i].Tarifa = tarifa;
				btnMesa[i].zona = zona;
				
				FileInfo mesa = new FileInfo(Rut_mesas + Path.DirectorySeparatorChar + rs[i]["Nombre"].ToString());
				if (mesa.Exists) {
					btnMesa[i].colorDeAtras = Color.Purple;
				} else {
					btnMesa[i].colorDeAtras = cl;
				}
				btnMesa[i].mesa = rs[i]["Nombre"].ToString();
				string clave = btnMesa[i].mesa+btnMesa[i].zona;
				if(!TodosBtnMesas.ContainsKey(clave)) 
				        TodosBtnMesas.Add(clave,btnMesa[i]);
			}
		}
		
		public bool estaVacia()
		{
			return mesaActiva.EstaVacia;
		}
		
		public void CambiarMesa()
		{
			Color cAux = mesaActiva.colorDefecto;
			String nomMesa = mesaActiva.mesa;
			String zona = mesaActiva.zona;
		
			if (mesaActiva.EstaVacia) {
				MesaActiva = mesaAuxiliar;
				CerrarMesa();
				MesaActiva = mesaAuxiliar;
				mesaActiva.mesa = nomMesa;
				mesaActiva.colorDefecto = cAux;
				mesaActiva.zona = zona;
				this.nomMesaActiva = nomMesa;
				GuardarMesa();
				mesaAuxiliar = null;
			} else {
				mesaActiva.mesa = mesaAuxiliar.mesa;
				mesaActiva.colorDefecto = mesaAuxiliar.colorDefecto;
				mesaActiva.zona = mesaAuxiliar.zona;
				nomMesaActiva = mesaAuxiliar.mesa;
				GuardarMesa();
				mesaActiva = mesaAuxiliar;
				mesaActiva.mesa = nomMesa;
				mesaActiva.colorDefecto = cAux;
				mesaActiva.zona = zona;
				nomMesaActiva = nomMesa;
				GuardarMesa();
				mesaAuxiliar = null;

			}

		}
		
		public  void JuntarMesa()
		{
			foreach (Articulo art in mesaAuxiliar.LineasTemp) {
				mesaActiva.LineasTemp.Add(art);
			}
			mesaAuxiliar.LineasTemp.Clear();
			foreach (Ronda tbCobradas in mesaAuxiliar.RondasPagadas) {
				mesaActiva.RondasPagadas.Add(tbCobradas);
			}
			mesaAuxiliar.RondasPagadas.Clear();
			foreach (Ronda tbActiva in mesaAuxiliar.RondasActivas) {
				mesaActiva.RondasActivas.Add(tbActiva);
			}
			mesaAuxiliar.RondasActivas.Clear();
			Mesa aux = mesaActiva;
			mesaActiva = mesaAuxiliar;
			nomMesaActiva = mesaAuxiliar.mesa;
			CerrarMesa();
			mesaActiva = aux;
			nomMesaActiva = aux.mesa;
			GuardarMesa();
			mesaAuxiliar = null;
		}
		
		public void GestionarMesa(int numLineas)
		{
			if (MesaActiva != null) {
				if (estaVacia()) {
					CerrarMesa();
				} else {
					if (mesaActiva.LineasPendienteDeCobro) {
						CerrarConvida();
					} else {
						System.Threading.Thread cobroH = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.CobrarMesaEnBase));
						cobroH.Start(mesaActiva);
						CerrarMesa();
					}

				}
			}
			mesaActiva = null;
		}
	
	    public void InvitarMesas(List<Articulo> articulos)
		{
			foreach (Articulo art in articulos) {
				art.CombidadPertenencia.QuitarLinea(art, null);
				if (art.CombidadPertenencia.lineasArtActivos.Count <= 0) {
					mesaActiva.RondasActivas.Remove(art.CombidadPertenencia);
				}
			}
			GuardarMesa();
			MesaActiva = mesaAuxiliar;
			LlenarRonda(articulos);
		}
		 
		
	   
	}
}



