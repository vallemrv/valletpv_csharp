
CREATE DATABASE IF NOT EXISTS BaseTPV;
USE BaseTPV;

DROP TABLE IF EXISTS BaseTPV.TPVs;
CREATE TABLE  BaseTPV.TPVs (
  IDTpv int(11) NOT NULL,
  Nombre varchar(50) NOT NULL,
  PRIMARY KEY (IDTpv)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO BaseTPV.TPVs VALUES  (1,'Sencillo'),
 (2,'Completo');


DROP TABLE IF EXISTS BaseTPV.Almacen;
CREATE TABLE  BaseTPV.Almacen (
  IDVinculacion int(11) NOT NULL,
  NombreAlmacen varchar(50) NOT NULL,
  Descripcion varchar(50) DEFAULT NULL,
  PRIMARY KEY (IDVinculacion)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS BaseTPV.Familias;
CREATE TABLE  BaseTPV.Familias (
  IDFamilia varchar(50) NOT NULL,
  Nombre varchar(50) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDFamilia)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO BaseTPV.Familias VALUES  ('10000','Varios',14),
 ('11000','Cafes e infusiones                  ',1),
 ('12000','Bolleria  y tostadas          ',2),
 ('13000','Pasteles                      ',3),
 ('21000','Cervezas                      ',4),
 ('22000','Vinos                         ',5),
 ('23000','Whiskys y Ginebras                    ',6),
 ('24000','Licores                       ',7),
 ('25000','Bebida para llevar',12),
 ('30000','Refrescos                     ',8),
 ('40000','Raciones',9),
 ('41000','Bocadillos',10),
 ('42000','Venta por kilos',11);


DROP TABLE IF EXISTS BaseTPV.Articulos;
CREATE TABLE  BaseTPV.Articulos (
  IDArticulo varchar(50) NOT NULL,
  Nombre varchar(50) NOT NULL,
  Precio1 decimal(5,2) DEFAULT NULL,
  Precio2 decimal(5,2) DEFAULT NULL,
  Precio3 decimal(5,2) DEFAULT NULL,
  IDFamilia varchar(50) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDArticulo),
  KEY IDFamilia (IDFamilia),
  CONSTRAINT Articulos_ibfk_1 FOREIGN KEY (IDFamilia) REFERENCES Familias (IDFamilia) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS BaseTPV.ArticuloNoVenta;
CREATE TABLE  BaseTPV.ArticuloNoVenta (
  IDVinculacion int(11) NOT NULL,
  IDArticulo varchar(50) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDArticulo (IDArticulo),
  CONSTRAINT ArticuloNoVenta_ibfk_1 FOREIGN KEY (IDArticulo) REFERENCES Articulos (IDArticulo) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;




INSERT INTO BaseTPV.Articulos VALUES  ('100001','Chicles','1.00','1.00','1.00','10000',214),
 ('100002','Bolsas golosa','1.00','1.00','1.00','10000',215),
 ('100003','Bolsa patatas','1.00','1.00','1.00','10000',216),
 ('1001','Cafe e infusiones             ','1.20','1.30','1.40','11000',1),
 ('1002','Chocolate y colacao           ','1.40','1.50','1.60','11000',2),
 ('1003','1/2 tostada o churros         ','0.80','0.80','0.80','12000',3),
 ('1004','Tostada o churros             ','1.10','1.10','1.10','12000',4),
 ('1005','Carrajillo                    ','1.80','1.90','2.00','11000',5),
 ('1006','Tostada especial              ','2.70','2.70','2.70','12000',6),
 ('1007','1/2 Tostada especial          ','2.00','2.00','2.00','12000',7),
 ('1008','Bolleria                      ','1.30','1.30','1.30','12000',8),
 ('1009','Guarroman                     ','1.60','1.60','1.60','13000',9),
 ('1010','Tartas                        ','3.00','3.00','3.00','13000',10),
 ('1011','Pasteles                      ','1.50','1.50','1.50','13000',11),
 ('1012','Carajillo baileys             ','2.00','2.20','2.40','11000',12),
 ('1013','Batidos                       ','1.80','2.00','2.20','30000',13),
 ('1014','Zumos                         ','2.00','2.20','2.40','30000',14),
 ('1015','Z. Naranja grande             ','2.20','2.40','2.60','30000',15),
 ('1016','Z. Naranja pequeño            ','2.00','2.20','2.40','30000',16),
 ('110001','Cafe para llevar','1.00','1.00','1.00','11000',211),
 ('110002','Cafe bombon','1.60','1.60','1.60','11000',213),
 ('1101','1/2 Tost jamon york           ','1.50','1.50','1.70','12000',19),
 ('1102','Maritoñi                      ','1.20','1.20','1.20','13000',20),
 ('1103','Suizo                         ','1.20','1.20','1.20','12000',21),
 ('1104','Croissant                     ','1.00','1.00','1.00','12000',22),
 ('1105','Bolleria especial             ','2.10','2.30','2.40','12000',23),
 ('1119','Tostada jamon york            ','2.30','2.30','2.50','12000',24),
 ('120001','Bizcocho','2.00','2.00','2.00','12000',224),
 ('20001','Cerveza pequeña               ','1.80','2.00','2.20','21000',25),
 ('20002','Cerveza Grande                ','2.00','2.20','2.30','21000',26),
 ('20003','1925                          ','2.50','2.70','2.90','21000',27),
 ('20004','Jarra Cerveza                 ','9.00','9.00','9.00','21000',28),
 ('20005','Cerveza especial              ','2.20','2.40','2.60','21000',29),
 ('20006','Vino del lugar                ','1.50','1.80','2.00','22000',30),
 ('20007','Tinto Vno. grande             ','2.00','2.20','2.40','22000',31),
 ('20008','Tinto Vno. pequeño            ','1.80','2.00','2.20','22000',32),
 ('20011','Comb Importacion              ','3.60','3.80','4.00','23000',35),
 ('20012','Comb nacional                 ','3.00','3.10','3.20','23000',36),
 ('20013','Copas de coñac                ','2.00','2.20','2.40','24000',37),
 ('20014','Licores sin                   ','2.00','2.40','2.60','24000',38),
 ('20015','Magno                         ','2.40','2.60','2.80','24000',39),
 ('20016','Whisky solo nac               ','2.50','2.70','2.90','23000',40),
 ('20017','Whisky solo import            ','3.10','3.30','3.50','23000',41),
 ('20018','Copa de anis                  ','2.00','2.20','2.40','24000',42),
 ('210001','Mezquita','2.10','2.30','2.50','21000',203),
 ('2100012','Jarra 0.5L','2.80','2.80','2.80','21000',221),
 ('220001','Yllera','3.00','3.20','3.40','22000',175),
 ('2200010','Barbadillo','2.00','2.20','2.40','22000',184),
 ('2200011','Coto','2.50','2.70','2.90','22000',185),
 ('2200012','Crianza','2.20','2.40','2.60','22000',186),
 ('2200013','Cosecha','2.00','2.20','2.40','22000',187),
 ('2200014','B. Marques azaleta','9.50','9.50','9.50','22000',190),
 ('2200016','B. Navajas','9.50','9.50','9.50','22000',191),
 ('2200017','B. Cosecha','9.50','9.50','9.50','22000',192),
 ('2200018','B. lambrusco','8.50','8.50','8.50','22000',193),
 ('2200019','B. Marques caceres','14.10','14.10','14.10','22000',194),
 ('220002','Lagunilla','2.50','2.80','3.00','22000',176),
 ('2200020','B. Barbadillo','9.10','9.10','9.10','22000',195),
 ('2200021','B. Manzanilla','12.10','12.10','12.10','22000',196),
 ('2200022','B. Crianza','11.00','11.00','11.00','22000',197),
 ('2200023','B. Ribera del duero','12.00','12.00','12.00','22000',198),
 ('2200024','B. Blanco seco','9.10','9.10','9.10','22000',200),
 ('2200025','Heineken','2.30','2.30','2.30','22000',212),
 ('2200026','Protos','3.00','3.20','3.40','22000',222),
 ('220003','Ribera duero','2.50','2.80','3.00','22000',177),
 ('220004','Maques caceres','3.00','3.20','3.40','22000',178),
 ('220005','Cortesia','2.00','2.20','2.40','22000',179),
 ('220006','Vino dulce','2.20','2.40','2.60','22000',180),
 ('220007','Manzanilla','2.20','2.40','2.50','22000',181),
 ('220008','Lambrusco','2.00','2.20','2.40','22000',182),
 ('2200088','Albariño','3.00','3.20','3.40','22000',226),
 ('220009','Rueda','2.00','2.20','2.40','22000',183),
 ('240001','Pacharan','2.00','2.20','2.40','24000',209),
 ('240002','Vermú','2.00','2.00','2.00','24000',218),
 ('240003','Licores varios','2.00','2.00','2.00','24000',219),
 ('240004','Baileys','2.50','2.50','2.50','24000',220),
 ('250001','Zumos','1.30','1.30','1.30','25000',160),
 ('250002','Latas refresco','1.30','1.30','1.30','25000',161),
 ('250003','Biofrutas','1.50','1.50','1.50','25000',163),
 ('250004','Latas cerveza','1.30','1.30','1.30','25000',165),
 ('250005','1925 para llevar','1.60','1.60','1.60','25000',166),
 ('250006','Agua grande llevar','1.50','1.50','1.50','25000',167),
 ('250007','Agua pequeña llevar','1.00','1.00','1.00','25000',168),
 ('250008','Litro leche','1.50','1.50','1.50','25000',169),
 ('250009','Barra pan','1.00','1.00','1.00','25000',170),
 ('30000','Agua con tapa','2.00','2.20','2.40','30000',201),
 ('300001','Vino sin grande','2.00','2.20','2.40','30000',188),
 ('300002','Vino sin peq','1.80','2.00','2.20','30000',189),
 ('300003','Agua grande tapa','2.20','2.40','2.50','30000',202),
 ('300004','Refresco','2.00','2.20','2.40','30000',206),
 ('300005','Latas + tapa','2.20','2.40','2.60','30000',225),
 ('400001','Tomate Aliñado','3.00','3.00','3.00','40000',48),
 ('4000010','Alpujarreño','6.00','6.00','6.00','40000',129),
 ('4000011','Combinado nº2','5.50','5.50','5.50','40000',130),
 ('4000012','Combinado nº3','5.50','5.50','5.50','40000',131),
 ('4000013','Combinado nº4','7.50','7.50','7.50','40000',132),
 ('4000014','Combinado nº5','6.50','6.50','6.50','40000',133),
 ('4000015','Combinado nº6','6.50','6.50','6.50','40000',134),
 ('4000016','Combinado nº7','5.50','5.50','5.50','40000',135),
 ('4000017','Combinado nº8','5.50','5.50','5.50','40000',136),
 ('4000018','Combinado nº9','5.00','5.00','5.00','40000',137),
 ('4000019','Combinado nº10','5.00','5.00','5.00','40000',138),
 ('400002','Ensalada de Pimientos','4.00','4.00','4.00','40000',49),
 ('4000020','Combinado nº11','5.50','5.50','5.50','40000',139),
 ('4000021','Combinado nº12','5.50','5.50','5.50','40000',140),
 ('4000022','Combinado nº13','5.50','5.50','5.50','40000',141),
 ('4000023','Combinado nº14','7.00','7.00','7.00','40000',142),
 ('4000024','Combinado nº15','7.00','7.00','7.00','40000',143),
 ('4000025','Combinado nº16','4.50','4.50','4.50','40000',144),
 ('4000026','Combinado nº17','4.50','4.50','4.50','40000',145),
 ('4000027','Combinado nº18','6.50','6.50','6.50','40000',146),
 ('4000028','Combinado nº19','4.50','4.50','4.50','40000',147),
 ('4000029','Combinado nº20','6.50','6.50','6.50','40000',148),
 ('400003','Ensalda mixta','5.00','5.00','5.00','40000',50),
 ('4000030','Racion de pan','0.60','0.60','6.00','40000',208),
 ('400004','Cogollos con ajos','4.00','4.00','4.00','40000',51),
 ('400005','Revuelto esparragos','5.70','5.70','5.70','40000',52),
 ('400006','Esparragos mahonesa','5.00','5.00','5.00','40000',53),
 ('400007','Judias con Jamón','5.70','5.70','5.70','40000',54),
 ('400008','1/2 Judias con Jamón','4.00','4.00','4.00','40000',56),
 ('400009','Habas con jamón','7.00','7.00','7.00','40000',57),
 ('400010','1/2 Habas con jamón','4.00','4.00','4.00','40000',58),
 ('400011','Paella','5.00','5.00','5.00','40000',59),
 ('400012','1/2 Paella','4.00','4.00','4.00','40000',60),
 ('400013','1/2 Queso añaejo','5.00','5.00','5.00','40000',61),
 ('400014','Queso añejo','9.00','9.00','9.00','40000',62),
 ('400015','Lomo de orza','8.00','8.00','8.00','40000',63),
 ('400016','1/2 Lomo de orza','5.00','5.00','5.00','40000',64),
 ('400017','Surtido iberico','12.00','12.00','12.00','40000',65),
 ('400018','1/2 Surtido iberico','8.00','8.00','8.00','40000',66),
 ('400019','Jamón serrano','8.60','8.60','8.60','40000',67),
 ('400020','1/2 Jamón serrano','5.00','5.00','5.00','40000',68),
 ('400021','Chorizo','8.00','8.00','8.00','42000',45),
 ('400022','Patatas a lo pobre','5.00','5.00','5.00','40000',69),
 ('400023','1/2 Patatas pobre','3.00','3.00','3.00','40000',70),
 ('400024','Racion chorizo casero','9.00','9.00','9.00','40000',71),
 ('400025','1/2 Chorizo casero','5.00','5.00','5.00','40000',72),
 ('400026','Morcilla casera','7.50','7.50','7.50','40000',73),
 ('400027','1/2 Morcilla casera','3.80','3.80','3.80','40000',74),
 ('400028','Mussaka','4.00','4.00','4.00','40000',75),
 ('400029','Croquetas','5.50','5.50','5.50','40000',76),
 ('400030','1/2 Croquetas','3.00','3.00','3.00','40000',77),
 ('400031','Ensaladilla rusa','7.00','7.00','7.00','40000',78),
 ('400032','1/2 Ensaladilla rusa','4.00','4.00','4.00','40000',79),
 ('400033','Bombas','6.50','6.50','6.50','40000',80),
 ('400034','1/2 Bombas','4.00','4.00','4.00','40000',81),
 ('400035','Solomillo plancha','6.00','6.00','6.00','40000',82),
 ('400036','Solomillo salsa','8.50','8.50','8.50','40000',83),
 ('400037','Lomo frito con ajos','7.00','7.00','7.00','40000',84),
 ('400038','Lomo plancha','6.00','6.00','6.00','40000',85),
 ('400039','Lomo en salsa','7.00','7.00','7.00','40000',86),
 ('400040','Carne en salsa','7.00','7.00','7.00','40000',87),
 ('400041','1/2 Carne salsa','4.00','4.00','4.00','40000',88),
 ('400042','Chuletas cordero','9.50','9.50','9.50','40000',89),
 ('400043','Pechuga pollo','6.00','6.00','6.00','40000',90),
 ('400044','Lomo empanado','6.00','6.00','6.00','40000',91),
 ('400045','Pollo empanado','6.00','6.00','6.00','40000',92),
 ('400046','Calamares plancha','8.00','8.00','8.00','40000',93),
 ('400047','Calamares fritos','8.60','8.60','8.60','40000',94),
 ('400048','Rape frito','8.50','8.50','8.50','40000',95),
 ('400049','Bacalao con tomate','9.00','9.00','9.00','40000',96),
 ('400050','Gambas fritas','9.00','9.00','9.00','40000',97),
 ('400051','Gambas Fritas con ajos','9.50','9.50','9.50','40000',98),
 ('400052','Almejas a la marinera','5.00','5.00','5.00','40000',99),
 ('400053','Puntas de calamar','7.00','7.00','7.00','40000',100),
 ('41000','Bocadillo nº9','2.50','2.50','2.50','41000',116),
 ('410001','Rosca de jamón','5.50','5.50','5.50','41000',101),
 ('4100010','Bocadillo Vegetal','3.00','3.00','3.00','41000',111),
 ('4100011','Bocadillo nº5','2.50','2.50','2.50','41000',112),
 ('4100012','Bocadillo nº6','2.50','2.50','2.50','41000',113),
 ('4100013','Bocadillo nº7','2.50','2.50','2.50','41000',114),
 ('4100017','Bocadillo nº10','2.50','2.50','2.50','41000',117),
 ('4100018','Bocadillo nº8','2.50','2.50','2.50','41000',115),
 ('4100019','Bocadillo nº11','2.50','2.50','2.50','41000',118),
 ('410002','1/2 Rosca jamón','3.00','3.00','3.00','41000',102),
 ('4100020','Bocadillo nº12','2.50','2.50','2.50','41000',119),
 ('4100021','Bocadillo nº13','2.50','2.50','2.50','41000',120),
 ('4100022','Bocadillo nº14','2.00','2.00','2.00','41000',122),
 ('4100023','Bocadillo nº15','3.00','3.00','3.00','41000',123),
 ('4100024','Bocadillo nº16','2.80','2.80','2.80','41000',124),
 ('4100025','Bocadillo nº17','2.80','2.80','2.80','41000',125),
 ('4100026','Bocadillo nº18','2.80','2.80','2.80','41000',126),
 ('4100027','Bocadillo nº19','4.00','4.00','4.00','41000',127),
 ('4100028','Bocadillo nº20','2.50','2.50','2.50','41000',128),
 ('4100029','Sandwich mixto','2.00','2.00','2.00','41000',149),
 ('410003','Rosca de lomo','5.50','5.50','5.50','41000',103),
 ('4100030','Sandwich vegetal','2.50','2.50','2.50','41000',150),
 ('4100031','Sandwich de la casa','3.00','3.00','3.00','41000',151),
 ('4100032','Triquini','4.00','4.00','4.00','41000',152),
 ('4100033','Sandwich de pollo','3.50','3.50','3.50','41000',153),
 ('4100034','Hamburguesa Sola','2.00','2.00','2.00','41000',155),
 ('4100035','Hamburgesa con queso','2.10','2.10','2.10','41000',156),
 ('4100036','Hamburguesa de la casa','3.00','3.00','3.00','41000',157),
 ('4100037','Zapatilla','2.75','2.75','2.75','41000',158),
 ('4100038','Pan pizza','2.50','2.50','2.50','41000',159),
 ('410004','1/2 Rosca lomo','3.00','3.00','3.00','41000',104),
 ('4100045','1/2 Bocadillo normal','2.00','2.00','2.00','41000',204),
 ('4100046','Bocadillo normal','2.50','2.50','2.50','41000',207),
 ('4100047','Tapa extra','1.00','1.00','1.00','41000',210),
 ('4100048','Tapa extra Hamburguesa','1.50','1.50','1.50','41000',223),
 ('410005','1/2 Rosca de la casa','3.70','3.70','3.70','41000',106),
 ('410006','Rosca de la casa','6.50','6.50','6.50','41000',107),
 ('410007','Serranito','3.50','3.50','3.50','41000',108),
 ('410008','Serranita','3.50','3.50','3.50','41000',109),
 ('410009','Bocadillo de la casa','3.50','3.50','3.50','41000',110),
 ('420001','Morcilla kilo','7.00','7.00','7.00','42000',171),
 ('420002','Queso en aceite','15.00','15.00','15.00','42000',172),
 ('420003','Lomo de orza kilo','21.00','21.00','21.00','42000',173),
 ('420004','Lomo iberico kilo','30.00','30.00','30.00','42000',174);

DROP TABLE IF EXISTS BaseTPV.Camareros;
CREATE TABLE  BaseTPV.Camareros (
  IDCamarero int(11) NOT NULL,
  Nombre varchar(50) NOT NULL,
  Apellidos varchar(50) DEFAULT NULL,
  Direccion varchar(50) DEFAULT NULL,
  Telefono char(9) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (IDCamarero)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO BaseTPV.Camareros VALUES  (2,'Manuel','Rodriguez','',''),
 (13,'Piedad','Sanchez','','');

DROP TABLE IF EXISTS BaseTPV.CierreDeCaja;
CREATE TABLE  BaseTPV.CierreDeCaja (
  IDCierre int(11) NOT NULL,
  desdeTicket int(11) NOT NULL,
  hastaTicket int(11) NOT NULL,
  fechaCierre varchar(50) NOT NULL,
  HoraCierre varchar(6) NOT NULL DEFAULT '0:00',
  IDTpv int(11) NOT NULL,
  PRIMARY KEY (IDCierre),
  KEY IDTpv (IDTpv),
  CONSTRAINT CierreDeCaja_ibfk_1 FOREIGN KEY (IDTpv) REFERENCES TPVs (IDTpv) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS BaseTPV.Colores;
CREATE TABLE  BaseTPV.Colores (
  Nombre varchar(50) NOT NULL,
  IdColor int(11) NOT NULL,
  rojo int(11) NOT NULL,
  verde int(11) NOT NULL,
  azul int(11) NOT NULL,
  PRIMARY KEY (IdColor)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.Colores VALUES  ('Amarillo  ',1,255,255,0),
 ('Azul      ',2,0,0,255),
 ('Verde     ',3,0,255,0),
 ('Rojo      ',4,255,0,0),
 ('Blanco    ',5,255,255,255),
 ('Gris      ',7,190,190,190),
 ('Morado claro',8,188,153,213),
 ('Salmon    ',9,255,165,79),
 ('Naranja   ',10,255,165,0),
 ('Rosa',11,255,182,182),
 ('Rojo oscuro',12,157,15,15),
 ('Verde Claro',14,152,218,138),
 ('Azul Clarito',16,170,232,232);


DROP TABLE IF EXISTS BaseTPV.Configuracion;
CREATE TABLE  BaseTPV.Configuracion (
  IDTpv int(11) NOT NULL,
  ImprimirAutomatico tinyint(1) NOT NULL DEFAULT '1',
  HoraInicioTpv varchar(50) NOT NULL,
  IdentificacionPrimero tinyint(1) NOT NULL DEFAULT '0',
  Bloqueado varchar(100) NOT NULL,
  activo tinyint(1) NOT NULL,
  mostrarVariosConNombre tinyint(1) NOT NULL,
  mostrarVarios tinyint(1) NOT NULL DEFAULT '1',
  IDVinculacion int(11) NOT NULL,
  TiempoFormAc int(11) NOT NULL DEFAULT '5',
  TiempoFormNoAc int(11) NOT NULL DEFAULT '10',
  PRIMARY KEY (IDVinculacion),
  KEY IDTpv (IDTpv),
  CONSTRAINT Configuracion_ibfk_1 FOREIGN KEY (IDTpv) REFERENCES TPVs (IDTpv) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.Configuracion VALUES  (1,0,'06:00',0,'',0,0,1,1,5,10),
 (2,1,'13:00',1,'',0,1,1,2,5,10);

DROP TABLE IF EXISTS BaseTPV.Controles;
CREATE TABLE  BaseTPV.Controles (
  NombreForm varchar(50) NOT NULL,
  Descripcion varchar(50) NOT NULL,
  IDBotonControl varchar(50) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDBotonControl)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS BaseTPV.DesgloseArt;
CREATE TABLE  BaseTPV.DesgloseArt (
  IDArtPrimario varchar(50) NOT NULL,
  IDArtDesglose varchar(50) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  Incremento decimal(3,2) DEFAULT NULL,
  Grupo int(11) DEFAULT NULL,
  CanArtGenera int(11) DEFAULT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDArtPrimario (IDArtPrimario),
  KEY IDArtDesglose (IDArtDesglose),
  CONSTRAINT DesgloseArt_ibfk_1 FOREIGN KEY (IDArtPrimario) REFERENCES Articulos (IDArticulo) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT DesgloseArt_ibfk_2 FOREIGN KEY (IDArtDesglose) REFERENCES Articulos (IDArticulo) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;







DROP TABLE IF EXISTS BaseTPV.Favoritos;
CREATE TABLE  BaseTPV.Favoritos (
  IDFavoritos int(11) NOT NULL,
  Nombre varchar(50) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (IDFavoritos)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.Favoritos VALUES  (1,'Desayunos'),
 (2,'Comidas'),
 (3,'Meriendas'),
 (4,'Tapeo');


DROP TABLE IF EXISTS BaseTPV.FavoritosTpv;
CREATE TABLE  BaseTPV.FavoritosTpv (
  IDFavoritos int(11) NOT NULL,
  IDTpv int(11) NOT NULL,
  HoraInicioFav varchar(50) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDFavoritos (IDFavoritos),
  KEY IDTpv (IDTpv),
  CONSTRAINT FavoritosTpv_ibfk_1 FOREIGN KEY (IDFavoritos) REFERENCES Favoritos (IDFavoritos) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT FavoritosTpv_ibfk_2 FOREIGN KEY (IDTpv) REFERENCES TPVs (IDTpv) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.FavoritosTpv VALUES  (1,1,'06:00',1),
 (2,1,'13:00',2),
 (2,2,'13:00',3),
 (3,2,'17:00',4),
 (4,2,'20:00',5);


DROP TABLE IF EXISTS BaseTPV.GestionMesas;
CREATE TABLE  BaseTPV.GestionMesas (
  IDVinculacion int(11) NOT NULL,
  Mesa varchar(50) NOT NULL,
  CamareroAbreMesa varchar(50) NOT NULL,
  FechaInicio varchar(50) NOT NULL,
  HoraInicio varchar(6) NOT NULL DEFAULT '0:00',
  FechaFin varchar(50) NOT NULL,
  HoraFin varchar(6) NOT NULL DEFAULT '0:00',
  PRIMARY KEY (IDVinculacion)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



DROP TABLE IF EXISTS BaseTPV.InstComision;
CREATE TABLE  BaseTPV.InstComision (
  IDVinculacion int(11) NOT NULL,
  IDCamarero int(11) NOT NULL,
  PorcientoCom decimal(3,2) NOT NULL,
  HoraInicio varchar(5) NOT NULL,
  HoraFin varchar(5) DEFAULT NULL,
  tarifa int(11) DEFAULT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDCamarero (IDCamarero),
  CONSTRAINT InstComision_ibfk_1 FOREIGN KEY (IDCamarero) REFERENCES Camareros (IDCamarero) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS BaseTPV.Inventarios;
CREATE TABLE  BaseTPV.Inventarios (
  IDVinculacion int(11) NOT NULL,
  IDArt varchar(50) NOT NULL,
  MaxStock int(11) DEFAULT NULL,
  MinStock int(11) DEFAULT NULL,
  Stock int(11) DEFAULT NULL,
  IDAlm int(11) NOT NULL,
  Nivel int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDArt (IDArt),
  KEY IDAlm (IDAlm),
  CONSTRAINT Inventarios_ibfk_1 FOREIGN KEY (IDArt) REFERENCES Articulos (IDArticulo) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT Inventarios_ibfk_2 FOREIGN KEY (IDAlm) REFERENCES Almacen (IDVinculacion) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



DROP TABLE IF EXISTS BaseTPV.Rondas;
CREATE TABLE  BaseTPV.Rondas (
  IDVinculacion int(11) NOT NULL,
  IDMesa int(11) NOT NULL,
  CamareroServido varchar(50) NOT NULL,
  FechaServido varchar(50) NOT NULL,
  HoraServido varchar(6) NOT NULL DEFAULT '0:00',
  PRIMARY KEY (IDVinculacion),
  KEY IDMesa (IDMesa),
  CONSTRAINT Rondas_ibfk_1 FOREIGN KEY (IDMesa) REFERENCES GestionMesas (IDVinculacion) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS BaseTPV.LineasNulas;
CREATE TABLE  BaseTPV.LineasNulas (
  IDVinculacion int(11) NOT NULL,
  camareroAnula varchar(50) NOT NULL,
  IDRonda int(11) NOT NULL,
  NombreArticulo varchar(50) NOT NULL,
  Cantidad decimal(8,3) NOT NULL,
  totalLinea decimal(19,4) NOT NULL,
  FechaAnulada varchar(50) NOT NULL,
  HoraAnulada varchar(6) NOT NULL DEFAULT '0:00',
  PRIMARY KEY (IDVinculacion),
  KEY IDRonda (IDRonda),
  CONSTRAINT LineasNulas_ibfk_1 FOREIGN KEY (IDRonda) REFERENCES Rondas (IDVinculacion) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS BaseTPV.LineasRonda;
CREATE TABLE  BaseTPV.LineasRonda (
  numTicket int(11) NOT NULL,
  Cantidad decimal(8,3) NOT NULL,
  nomArticulo varchar(50) NOT NULL,
  IDRonda int(11) NOT NULL,
  Tarifa int(11) NOT NULL DEFAULT '1',
  TotalLinea decimal(8,2) DEFAULT NULL,
  KEY IDRonda (IDRonda),
  CONSTRAINT LineasRonda_ibfk_1 FOREIGN KEY (IDRonda) REFERENCES Rondas (IDVinculacion) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS BaseTPV.Ticket;
CREATE TABLE  BaseTPV.Ticket (
  NumTicket int(11) NOT NULL,
  FechaCobrado varchar(50) NOT NULL,
  HoraCobrado varchar(6) NOT NULL DEFAULT '0:00',
  Camarero varchar(50) NOT NULL,
  Mesa varchar(50) NOT NULL,
  IDTpv int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (NumTicket)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS BaseTPV.LineasTicket;
CREATE TABLE  BaseTPV.LineasTicket (
  numTicket int(11) NOT NULL,
  Cantidad decimal(8,3) NOT NULL,
  nomArticulo varchar(50) NOT NULL,
  TotalLinea decimal(19,4) NOT NULL,
  KEY numTicket (numTicket),
  CONSTRAINT LineasTicket_ibfk_1 FOREIGN KEY (numTicket) REFERENCES Ticket (NumTicket) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS BaseTPV.Zonas;
CREATE TABLE  BaseTPV.Zonas (
  IDZona int(11) NOT NULL,
  Nombre varchar(50) NOT NULL,
  tarifa int(11) NOT NULL DEFAULT '1',
  IDColor int(11) DEFAULT '0',
  Planing varchar(100) DEFAULT NULL,
  Alto int(11) NOT NULL DEFAULT '3',
  Ancho int(11) NOT NULL DEFAULT '6',
  PRIMARY KEY (IDZona),
  KEY IDColor (IDColor),
  CONSTRAINT Zonas_ibfk_1 FOREIGN KEY (IDColor) REFERENCES Colores (IdColor) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.Zonas VALUES  (1,'Barra',1,1,'',3,6),
 (2,'Salon 1',1,2,'',3,5),
 (3,'Salon 2',1,11,'',3,6),
 (4,'Terraza central',2,16,'',3,6),
 (5,'Terraza izq',2,4,'',3,6),
 (6,'Terraza der',2,3,'',3,6);



DROP TABLE IF EXISTS BaseTPV.Mesas;
CREATE TABLE  BaseTPV.Mesas (
  IDMesa int(11) NOT NULL,
  Nombre varchar(50) NOT NULL,
  IDZona int(11) NOT NULL,
  Orden int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (IDMesa),
  KEY IDZona (IDZona),
  CONSTRAINT Mesas_ibfk_1 FOREIGN KEY (IDZona) REFERENCES Zonas (IDZona) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO BaseTPV.Mesas VALUES  (1,'Zona 1',1,0),
 (2,'Zona 2',1,1),
 (3,'Zona 3',1,2),
 (4,'Zona 4',1,3),
 (5,'Zona 5',1,4),
 (6,'Zona 6',1,5),
 (7,'Mesa 1',2,0),
 (8,'Mesa 2',2,1),
 (9,'Mesa 3',2,2),
 (10,'Mesa 4',2,3),
 (11,'Mesa 5',2,4),
 (12,'Mesa 6',2,5),
 (13,'Mesa 7',2,6),
 (14,'Mesa 8',2,7),
 (15,'Mesa 9',2,8),
 (16,'Mesa 10',2,9),
 (17,'Mesa 1 s2',3,0),
 (18,'Mesa 2 s2',3,1),
 (19,'Mesa 3 s2',3,2),
 (20,'Mesa 4 s2',3,3),
 (21,'Mesa 5 s2',3,4),
 (22,'Mesa 6 s2',3,5),
 (23,'Mesa 7 s2',3,6),
 (24,'mesa 11',4,0),
 (25,'mesa 12',4,1),
 (26,'mesa 13',4,2),
 (27,'mesa 14',4,3),
 (28,'mesa 15',4,4),
 (29,'mesa 16',4,5),
 (30,'mesa 17',4,6),
 (31,'mesa 18',4,7),
 (32,'mesa 21',4,8),
 (33,'mesa 22',4,9),
 (34,'mesa 23',4,10),
 (35,'mesa 24',4,11),
 (36,'mesa 25',4,12),
 (37,'mesa 26',4,13),
 (38,'mesa 27',4,14),
 (39,'mesa 28',4,15),
 (41,'mesa 8 s2',3,7),
 (42,'mesa 9 s2',3,8),
 (43,'mesa 10 s2',3,9),
 (44,'mesa 11 s2',3,10),
 (45,'mesa 12 s2',3,11),
 (46,'Mesa 1 terrIzq',5,0),
 (47,'Mesa 2 terrIzq',5,1),
 (50,'Mesa 3 TerrIzq',5,2),
 (51,'Mesa 4 TerrIzq',5,3),
 (52,'Mesa 5 TerrIzq',5,4),
 (53,'Mesa 6 TerrIzq',5,5),
 (54,'Mesa 7 TerrIzq',5,6),
 (55,'Mesa 8 TerrIzq',5,7),
 (56,'Mesa 9 TerrIzq',5,8),
 (57,'Mesa 10 TerrIzq',5,9),
 (58,'Mesa 11 TerrIzq',5,10),
 (59,'Mesa 12 TerrIzq',5,11),
 (60,'Mesa 13 TerrIzq',5,12),
 (61,'Mesa 14 TerrIzq',5,13),
 (62,'Mesa 15 TerrIzq',5,14),
 (63,'Mesa 16 TerrIzq',5,15),
 (64,'Mesa 17 TerrIzq',5,16),
 (65,'Mesa 18 TerrIzq',5,17),
 (68,'Mesa 1 TerrDer',6,0),
 (69,'Mesa 2 TerrDer',6,1),
 (70,'Mesa 3 TerrDer',6,2),
 (71,'Mesa 4 TerrDer',6,3),
 (72,'Mesa 5 TerrDer',6,4),
 (73,'Mesa 6 TerrDer',6,5),
 (74,'Mesa 7 TerrDer',6,6),
 (75,'Mesa 8 TerrDer',6,7),
 (76,'Mesa 9 TerrDer',6,8),
 (77,'Mesa 10 TerrDer',6,9),
 (78,'Mesa 11 TerrDer',6,10),
 (79,'Mesa 12 TerrDer',6,11),
 (80,'Mesa 13 TerrDer',6,12),
 (81,'Mesa 14 TerrDer',6,13),
 (82,'Mesa 15 TerrDer',6,14),
 (83,'Mesa 16 TerrDer',6,15),
 (84,'Mesa 17 TerrDer',6,16),
 (85,'Mesa 18 TerrDer',6,17);

DROP TABLE IF EXISTS BaseTPV.Privilegios;
CREATE TABLE  BaseTPV.Privilegios (
  IDUsuario int(11) NOT NULL,
  IDBotonControl varchar(50) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDBotonControl (IDBotonControl),
  CONSTRAINT Privilegios_ibfk_1 FOREIGN KEY (IDBotonControl) REFERENCES Controles (IDBotonControl) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS BaseTPV.ResComision;
CREATE TABLE  BaseTPV.ResComision (
  IDVinculacion int(11) NOT NULL,
  IDCamarero int(11) NOT NULL,
  IDCierre int(11) NOT NULL,
  TotalVendido decimal(8,2) NOT NULL,
  TotalComision decimal(8,2) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDCamarero (IDCamarero),
  KEY IDCierre (IDCierre),
  CONSTRAINT ResComision_ibfk_1 FOREIGN KEY (IDCamarero) REFERENCES Camareros (IDCamarero) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT ResComision_ibfk_2 FOREIGN KEY (IDCierre) REFERENCES CierreDeCaja (IDCierre) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;




DROP TABLE IF EXISTS BaseTPV.Rutas;
CREATE TABLE  BaseTPV.Rutas (
  IDVinculacion int(11) NOT NULL,
  Identificacion varchar(50) NOT NULL,
  Ruta varchar(100) NOT NULL,
  PRIMARY KEY (IDVinculacion)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



DROP TABLE IF EXISTS BaseTPV.Secciones;
CREATE TABLE  BaseTPV.Secciones (
  IDSeccion int(11) NOT NULL,
  Nombre varchar(50) NOT NULL,
  IDColor int(11) DEFAULT '0',
  PRIMARY KEY (IDSeccion),
  KEY IDColor (IDColor),
  CONSTRAINT Secciones_ibfk_1 FOREIGN KEY (IDColor) REFERENCES Colores (IdColor) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO BaseTPV.Secciones VALUES  (3,'Desayunos',1),
 (5,'Cervezas',2),
 (6,'Vinos',3),
 (7,'Refrescos',4),
 (8,'Copas',5),
 (11,'Bolleria y tartas',7),
 (12,'Bocadillos',8),
 (13,'Raciones',16),
 (14,'Venta por kilos',9),
 (15,'Botellas',11),
 (16,'Productos para llevar',10);

DROP TABLE IF EXISTS BaseTPV.SeccionesTpv;
CREATE TABLE  BaseTPV.SeccionesTpv (
  IDVinculacion int(11) NOT NULL,
  IDTpv int(11) NOT NULL,
  IDSeccion int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDTpv (IDTpv),
  KEY IDSeccion (IDSeccion),
  CONSTRAINT SeccionesTpv_ibfk_1 FOREIGN KEY (IDTpv) REFERENCES TPVs (IDTpv) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT SeccionesTpv_ibfk_2 FOREIGN KEY (IDSeccion) REFERENCES Secciones (IDSeccion) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.SeccionesTpv VALUES  (1,1,3),
 (2,1,11),
 (3,1,8),
 (4,1,7),
 (5,1,12),
 (6,1,13),
 (7,1,5),
 (8,1,6),
 (9,1,14),
 (10,2,3),
 (11,2,5),
 (12,2,6),
 (13,2,7),
 (14,2,8),
 (15,2,11),
 (16,2,12),
 (17,2,13),
 (18,2,14),
 (19,2,15),
 (20,2,16),
 (21,1,16),
 (22,1,15);


DROP TABLE IF EXISTS BaseTPV.Sincronizados;
CREATE TABLE  BaseTPV.Sincronizados (
  TablaPertenencia varchar(50) NOT NULL,
  CadenaSelect varchar(100) NOT NULL,
  Accion int(11) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;






DROP TABLE IF EXISTS BaseTPV.Teclas;
CREATE TABLE  BaseTPV.Teclas (
  IDTecla int(11) NOT NULL,
  IDArticulo varchar(50) NOT NULL,
  Nombre varchar(50) NOT NULL,
  IDSeccion int(11) NOT NULL,
  Orden int(11) NOT NULL DEFAULT '1',
  Foto varchar(100) DEFAULT NULL,
  PRIMARY KEY (IDTecla),
  KEY IDArticulo (IDArticulo),
  KEY IDSeccion (IDSeccion),
  CONSTRAINT Teclas_ibfk_1 FOREIGN KEY (IDArticulo) REFERENCES Articulos (IDArticulo) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT Teclas_ibfk_2 FOREIGN KEY (IDSeccion) REFERENCES Secciones (IDSeccion) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.Teclas VALUES  (36,'20001','Caña',5,0,''),
 (37,'20002','Tubo',5,1,''),
 (38,'20003','Reserva 1925',5,2,''),
 (39,'20005','Cervez especial',5,3,''),
 (40,'20006','Lugar peq',6,0,''),
 (41,'20007','Verano gran',6,1,''),
 (42,'20008','Verano peq',6,2,''),
 (45,'20011','Combina import',8,1,''),
 (46,'20012','Combina nacional',8,1,''),
 (47,'20013','Copa coñac',8,1,''),
 (48,'20014','Licores sin',8,1,''),
 (49,'20015','Copa magno',8,1,''),
 (50,'20016','W. solo nacional',8,1,''),
 (51,'20017','W. solo import',8,1,''),
 (52,'20018','Copa de anis',8,1,''),
 (54,'1119','Tost Jamon york',11,1,''),
 (55,'1101','1/2 Tost Jamon york',11,1,''),
 (56,'1102','Maritoñi',11,1,''),
 (57,'1103','Suizo',11,1,''),
 (58,'1104','Croissant',11,1,''),
 (59,'1105','Bolleria especial',11,1,''),
 (61,'1001','Cafe e infusiones             ',3,0,''),
 (62,'1002','Chocolate y colacao           ',3,1,''),
 (63,'1003','1/2 tostada o churros         ',3,2,''),
 (64,'1004','Tostada o churros             ',3,3,''),
 (65,'1005','Carrajillo                    ',3,8,''),
 (66,'1006','Tostada especial              ',3,11,''),
 (67,'1007','1/2 Tostada especial          ',3,10,''),
 (68,'1008','Bolleria                      ',3,7,''),
 (69,'1009','Guarroman                     ',3,16,''),
 (70,'1010','Tartas                        ',3,17,''),
 (71,'1011','Pasteles                      ',3,18,''),
 (72,'1012','Carajillo baileys             ',3,9,''),
 (73,'1013','Batidos                       ',3,5,''),
 (74,'1014','Zumos                         ',3,6,''),
 (75,'1015','Z. Naranja grande             ',3,14,''),
 (76,'1016','Z. Naranja pequeño            ',3,15,''),
 (79,'1101','1/2 Tost jamon york           ',3,12,''),
 (80,'1102','Maritoñi                      ',3,24,''),
 (81,'1103','Suizo                         ',3,25,''),
 (82,'1104','Croissant                     ',3,26,''),
 (83,'1105','Bolleria especial             ',3,19,''),
 (84,'1119','Tostada jamon york            ',3,13,''),
 (85,'20013','Copas de coñac                ',3,20,''),
 (90,'20018','Copa de anis                  ',3,21,''),
 (92,'1013','Batidos                       ',7,1,''),
 (93,'1014','Zumos                         ',7,2,''),
 (94,'1015','Z. Naranja grande             ',7,5,''),
 (95,'1016','Z. Naranja pequeño            ',7,6,''),
 (98,'400001','Tomate Aliñado',13,1,''),
 (99,'400002','Ensalada de Pimientos ',13,2,''),
 (100,'400003','Ensalda mixta',13,3,''),
 (101,'400004','Cogollos con ajos',13,4,''),
 (102,'400005','Revuelto de esparragos',13,5,''),
 (104,'400021','Chorizo',14,0,''),
 (106,'400006','Esparragos mahonesa',13,6,''),
 (108,'20004','Jarra Cerveza                 ',5,6,''),
 (109,'210001','Mezquita',5,5,''),
 (111,'220001','Yllera',6,4,''),
 (112,'220002','Lagunilla',6,5,''),
 (113,'220003','Ribera duero',6,6,''),
 (114,'220004','Maques caceres',6,7,''),
 (115,'220005','Cortesia',6,9,''),
 (116,'220006','Vino dulce',6,10,''),
 (117,'220007','Manzanilla',6,11,''),
 (118,'220008','Lambrusco',6,12,''),
 (119,'220009','Rueda',6,13,''),
 (120,'2200010','Barbadillo',6,14,''),
 (121,'2200011','Coto',6,8,''),
 (122,'2200012','Crianza',6,15,''),
 (123,'2200013','Cosecha',6,16,''),
 (125,'30000','Agua con tapa',7,9,''),
 (126,'300001','Vino sin grande',7,3,''),
 (127,'300002','Vino sin peq',7,4,''),
 (130,'1003','1/2 tostada o churros         ',11,6,''),
 (131,'1004','Tostada o churros             ',11,7,''),
 (132,'1006','Tostada especial              ',11,8,''),
 (133,'1007','1/2 Tostada especial          ',11,9,''),
 (134,'1008','Bolleria                      ',11,10,''),
 (135,'1009','Guarroman                     ',11,11,''),
 (136,'1010','Tartas                        ',11,12,''),
 (137,'1011','Pasteles                      ',11,13,''),
 (138,'41000','Bocadillo nº9',12,0,''),
 (139,'410001','Rosca de jamón',12,1,''),
 (140,'410002','1/2 Rosca jamón',12,2,''),
 (141,'410003','Rosca de lomo',12,3,''),
 (142,'410004','1/2 Rosca lomo',12,4,''),
 (143,'410005','1/2 Rosca de la casa',12,5,''),
 (144,'410006','Rosca de la casa',12,6,''),
 (145,'410007','Serranito',12,7,''),
 (146,'410008','Serranita',12,8,''),
 (147,'410009','Bocadillo de la casa',12,9,''),
 (148,'4100010','Bocadillo Vegetal',12,10,''),
 (149,'4100011','Bocadillo nº5',12,11,''),
 (150,'4100012','Bocadillo nº6',12,12,''),
 (151,'4100013','Bocadillo nº7',12,13,''),
 (152,'4100017','Bocadillo nº10',12,14,''),
 (153,'4100018','Bocadillo nº8',12,15,''),
 (154,'4100019','Bocadillo nº11',12,16,''),
 (155,'4100020','Bocadillo nº12',12,17,''),
 (156,'4100021','Bocadillo nº13',12,18,''),
 (157,'4100022','Bocadillo nº14',12,19,''),
 (158,'4100023','Bocadillo nº15',12,20,''),
 (159,'4100024','Bocadillo nº16',12,21,''),
 (160,'4100025','Bocadillo nº17',12,22,''),
 (161,'4100026','Bocadillo nº18',12,23,''),
 (162,'4100027','Bocadillo nº19',12,24,''),
 (163,'4100028','Bocadillo nº20',12,25,''),
 (164,'4100029','Sandwich mixto',12,26,''),
 (165,'4100030','Sandwich vegetal',12,27,''),
 (166,'4100031','Sandwich de la casa',12,28,''),
 (167,'4100032','Triquini',12,29,''),
 (168,'4100033','Sandwich de pollo',12,30,''),
 (169,'4100034','Hamburguesa Sola',12,31,''),
 (170,'4100035','Hamburgesa con queso',12,32,''),
 (171,'4100036','Hamburguesa de la casa',12,33,''),
 (172,'4100037','Zapatilla',12,34,''),
 (173,'4100038','Pan pizza',12,35,''),
 (174,'4100045','1/2 Bocadillo normal',12,36,''),
 (175,'400007','Judias con Jamón',13,7,''),
 (176,'400008','1/2 Judias con Jamón',13,8,''),
 (177,'400009','Habas con jamón',13,9,''),
 (178,'400010','1/2 Habas con jamón',13,10,''),
 (179,'400011','Paella',13,11,''),
 (180,'400012','1/2 Paella',13,12,''),
 (181,'400013','1/2 Queso añaejo',13,13,''),
 (182,'400014','Queso añejo',13,14,''),
 (183,'400015','Lomo de orza',13,15,''),
 (184,'400016','1/2 Lomo de orza',13,16,''),
 (185,'400017','Surtido iberico',13,17,''),
 (186,'400018','1/2 Surtido iberico',13,18,''),
 (187,'400019','Jamón serrano',13,19,''),
 (188,'400020','1/2 Jamón serrano',13,20,''),
 (189,'400022','Patatas a lo pobre',13,21,''),
 (190,'400023','1/2 Patatas pobre',13,22,''),
 (191,'400024','Racion chorizo casero',13,23,''),
 (192,'400025','1/2 Chorizo casero',13,24,''),
 (193,'400026','Morcilla casera',13,25,''),
 (194,'400027','1/2 Morcilla casera',13,26,''),
 (195,'400028','Mussaka',13,27,''),
 (196,'400029','Croquetas',13,28,''),
 (197,'400030','1/2 Croquetas',13,29,''),
 (198,'400031','Ensaladilla rusa',13,30,''),
 (199,'400032','1/2 Ensaladilla rusa',13,31,''),
 (200,'400033','Bombas',13,32,''),
 (201,'400034','1/2 Bombas',13,33,''),
 (202,'400035','Solomillo plancha',13,34,''),
 (203,'400036','Solomillo salsa',13,35,''),
 (204,'400037','Lomo frito con ajos',13,36,''),
 (205,'400038','Lomo plancha',13,37,''),
 (206,'400039','Lomo en salsa',13,38,''),
 (207,'400040','Carne en salsa',13,39,''),
 (208,'400041','1/2 Carne salsa',13,40,''),
 (209,'400042','Chuletas cordero',13,41,''),
 (210,'400043','Pechuga pollo',13,42,''),
 (211,'400044','Lomo empanado',13,43,''),
 (212,'400045','Pollo empanado',13,44,''),
 (213,'400046','Calamares plancha',13,45,''),
 (214,'400047','Calamares fritos',13,46,''),
 (215,'400048','Rape frito',13,47,''),
 (216,'400049','Bacalao con tomate',13,48,''),
 (217,'400050','Gambas fritas',13,49,''),
 (218,'400051','Gambas Fritas con ajos',13,50,''),
 (219,'400052','Almejas a la marinera',13,51,''),
 (220,'400053','Puntas de calamar',13,52,''),
 (221,'4000010','Alpujarreño',13,53,''),
 (222,'4000011','Combinado nº2',13,54,''),
 (223,'4000012','Combinado nº3',13,55,''),
 (224,'4000013','Combinado nº4',13,56,''),
 (225,'4000014','Combinado nº5',13,57,''),
 (226,'4000015','Combinado nº6',13,58,''),
 (227,'4000016','Combinado nº7',13,59,''),
 (228,'4000017','Combinado nº8',13,60,''),
 (229,'4000018','Combinado nº9',13,61,''),
 (230,'4000019','Combinado nº10',13,62,''),
 (231,'4000020','Combinado nº11',13,63,''),
 (232,'4000021','Combinado nº12',13,64,''),
 (233,'4000022','Combinado nº13',13,65,''),
 (234,'4000023','Combinado nº14',13,66,''),
 (235,'4000024','Combinado nº15',13,67,''),
 (236,'4000025','Combinado nº16',13,68,''),
 (237,'4000026','Combinado nº17',13,69,''),
 (238,'4000027','Combinado nº18',13,70,''),
 (239,'4000028','Combinado nº19',13,71,''),
 (240,'4000029','Combinado nº20',13,72,''),
 (241,'420001','Morcilla kilo',14,1,''),
 (242,'420002','Queso en aceite',14,2,''),
 (243,'420003','Lomo de orza kilo',14,3,''),
 (244,'420004','Lomo iberico kilo',14,4,''),
 (245,'2200014','B. Marques azaleta',15,0,''),
 (246,'2200016','B. Navajas',15,1,''),
 (247,'2200017','B. Cosecha',15,2,''),
 (248,'2200018','B. lambrusco',15,3,''),
 (249,'2200019','B. Marques caceres',15,4,''),
 (250,'2200020','B. Barbadillo',15,5,''),
 (251,'2200021','B. Manzanilla',15,6,''),
 (252,'2200022','B. Crianza',15,7,''),
 (253,'2200023','B. Ribera del duero',15,8,''),
 (254,'2200024','B. Blanco seco',15,9,''),
 (256,'300004','Refresco',7,0,''),
 (258,'4100046','Bocadillo normal',12,37,''),
 (259,'250001','Zumos',16,0,''),
 (260,'250002','Latas refresco',16,1,''),
 (261,'250003','Biofrutas',16,2,''),
 (262,'250004','Latas cerveza',16,3,''),
 (263,'250005','1925 para llevar',16,4,''),
 (264,'250006','Agua grande llevar',16,5,''),
 (265,'250007','Agua pequeña llevar',16,6,''),
 (266,'250008','Litro leche',16,7,''),
 (267,'250009','Barra pan',16,8,''),
 (269,'240001','Pacharan',8,8,''),
 (270,'4100047','Tapa extra',12,38,''),
 (272,'4000030','Racion de pan',13,0,''),
 (274,'110001','Cafe para llevar',16,9,''),
 (276,'100001','Chicles',16,10,''),
 (277,'100002','Bolsas golosa',16,11,''),
 (278,'100003','Bolsa patatas',16,12,''),
 (279,'2200025','Heineken',5,4,''),
 (280,'110002','Cafe bombon',3,4,''),
 (285,'240002','Vermú',8,9,''),
 (286,'240003','Licores varios',8,10,''),
 (287,'240004','Baileys',8,11,''),
 (288,'2100012','Jarra 0.5L',5,7,NULL),
 (289,'2200026','Protos',6,3,NULL),
 (290,'120001','Bizcocho',11,14,NULL),
 (291,'2200088','Albariño',6,17,NULL),
 (292,'300005','Latas + tapa',7,10,NULL);


DROP TABLE IF EXISTS BaseTPV.TeclasFav;
CREATE TABLE  BaseTPV.TeclasFav (
  IDVinculacion int(11) NOT NULL,
  IDFavoritos int(11) NOT NULL,
  IDTecla int(11) NOT NULL,
  Orden int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDFavoritos (IDFavoritos),
  KEY IDTecla (IDTecla),
  CONSTRAINT TeclasFav_ibfk_1 FOREIGN KEY (IDFavoritos) REFERENCES Favoritos (IDFavoritos) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT TeclasFav_ibfk_2 FOREIGN KEY (IDTecla) REFERENCES Teclas (IDTecla) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.TeclasFav VALUES  (1,1,61,0),
 (2,1,62,1),
 (3,1,63,2),
 (4,1,64,3),
 (7,1,54,8),
 (8,1,55,9),
 (9,1,56,38),
 (10,1,57,37),
 (11,1,58,39),
 (12,1,59,10),
 (13,1,45,18),
 (14,1,46,19),
 (15,1,47,17),
 (16,1,48,20),
 (17,1,49,21),
 (18,1,50,22),
 (19,1,51,23),
 (20,1,52,40),
 (21,1,132,11),
 (22,1,133,12),
 (23,1,134,5),
 (24,1,135,13),
 (25,1,136,14),
 (26,1,137,15),
 (27,2,256,1),
 (28,2,36,2),
 (29,2,37,3),
 (30,2,38,6),
 (31,2,39,7),
 (32,2,40,8),
 (33,2,41,9),
 (34,2,42,10),
 (37,2,108,14),
 (39,2,111,16),
 (40,2,112,17),
 (41,2,113,18),
 (42,2,114,19),
 (43,2,115,20),
 (44,2,116,21),
 (45,2,117,22),
 (46,2,118,23),
 (47,2,119,24),
 (48,2,120,25),
 (49,2,121,26),
 (50,2,122,5),
 (51,2,123,4),
 (52,2,100,33),
 (53,2,177,34),
 (54,2,179,35),
 (55,2,180,36),
 (56,2,221,37),
 (57,3,130,2),
 (58,3,131,3),
 (59,3,65,5),
 (83,1,174,24),
 (84,1,258,25),
 (85,1,104,26),
 (86,1,241,27),
 (87,1,36,28),
 (88,1,37,29),
 (89,1,256,30),
 (90,1,94,6),
 (91,1,95,7),
 (92,1,126,32),
 (93,1,127,33),
 (94,1,122,34),
 (95,1,123,35),
 (96,2,98,38),
 (97,2,260,39),
 (98,2,262,40),
 (99,3,61,0),
 (100,3,62,1),
 (103,3,66,6),
 (104,3,67,7),
 (105,3,54,8),
 (106,3,55,9),
 (107,3,56,34),
 (108,3,57,35),
 (109,3,58,36),
 (110,3,59,13),
 (111,3,132,14),
 (112,3,133,15),
 (113,3,134,4),
 (114,3,135,16),
 (115,3,136,17),
 (116,3,137,18),
 (117,3,45,19),
 (118,3,46,20),
 (119,3,47,21),
 (120,3,48,24),
 (121,3,49,23),
 (122,3,50,25),
 (123,3,51,26),
 (124,3,52,27),
 (125,3,174,32),
 (126,3,258,33),
 (127,3,36,28),
 (128,3,37,29),
 (129,4,36,2),
 (130,4,37,3),
 (131,4,38,6),
 (132,4,39,7),
 (133,4,40,17),
 (134,4,41,10),
 (135,4,42,11),
 (138,4,111,18),
 (139,4,112,19),
 (140,4,113,13),
 (141,4,114,20),
 (142,4,115,22),
 (143,4,116,23),
 (144,4,117,24),
 (145,4,118,25),
 (146,4,119,16),
 (147,4,120,26),
 (148,4,121,27),
 (149,4,122,5),
 (150,4,123,4),
 (151,4,93,28),
 (152,4,126,8),
 (153,4,127,9),
 (154,4,256,1),
 (155,4,139,35),
 (156,4,141,36),
 (157,4,144,39),
 (158,4,145,40),
 (159,4,146,41),
 (160,4,147,42),
 (161,4,174,43),
 (162,4,258,44),
 (163,2,93,12),
 (165,2,125,13),
 (166,3,92,30),
 (167,3,93,31),
 (168,3,94,10),
 (169,3,95,11),
 (171,3,126,37),
 (172,3,127,38),
 (173,3,256,12),
 (174,4,92,33),
 (175,4,94,29),
 (176,4,95,32),
 (178,4,125,12),
 (180,4,270,0),
 (181,2,270,0),
 (182,4,276,47),
 (183,4,277,48),
 (184,4,278,49),
 (185,4,61,50),
 (186,4,45,46),
 (187,4,46,45),
 (188,2,61,43),
 (189,2,65,44),
 (190,2,69,45),
 (191,2,70,46),
 (192,2,71,47),
 (193,2,75,29),
 (194,2,76,32),
 (195,1,38,36),
 (196,1,65,4),
 (197,2,126,27),
 (198,2,127,28),
 (200,2,174,41),
 (201,2,258,42),
 (202,2,269,48),
 (203,2,285,49),
 (204,2,286,50),
 (205,2,47,51),
 (206,3,269,39),
 (207,4,140,34),
 (208,4,142,37),
 (209,4,143,38),
 (210,3,286,40),
 (211,2,289,11),
 (212,4,289,21),
 (213,4,108,14),
 (214,4,288,15),
 (215,1,92,16),
 (216,1,290,31),
 (217,3,290,22),
 (218,2,291,30),
 (219,2,292,31),
 (220,4,292,31),
 (221,4,291,30);




DROP TABLE IF EXISTS BaseTPV.VentaPorKilos;
CREATE TABLE  BaseTPV.VentaPorKilos (
  IDArticulo varchar(50) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDArticulo (IDArticulo),
  CONSTRAINT VentaPorKilos_ibfk_1 FOREIGN KEY (IDArticulo) REFERENCES Articulos (IDArticulo) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.VentaPorKilos VALUES  ('400021',1),
 ('420001',4),
 ('420002',2),
 ('420003',3),
 ('420004',5);




DROP TABLE IF EXISTS BaseTPV.ZonasTpv;
CREATE TABLE  BaseTPV.ZonasTpv (
  IDZona int(11) NOT NULL,
  IDTpv int(11) NOT NULL,
  IDVinculacion int(11) NOT NULL,
  PRIMARY KEY (IDVinculacion),
  KEY IDZona (IDZona),
  KEY IDTpv (IDTpv),
  CONSTRAINT ZonasTpv_ibfk_1 FOREIGN KEY (IDZona) REFERENCES Zonas (IDZona) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT ZonasTpv_ibfk_2 FOREIGN KEY (IDTpv) REFERENCES TPVs (IDTpv) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


INSERT INTO BaseTPV.ZonasTpv VALUES  (1,1,1),
 (2,1,2),
 (3,1,3),
 (1,2,5),
 (2,2,6),
 (3,2,7),
 (5,2,9),
 (6,2,10),
 (4,2,11);


CREATE TABLE BaseTPV.ArqueoCaja (
  ID int(11) NOT NULL AUTO_INCREMENT,
  IDVinculacion int(11) NOT NULL,
  Resultado decimal(10,0) NOT NULL,
  PRIMARY KEY (ID),
  KEY fk_cierre_arqueo (IDVinculacion),
  CONSTRAINT fk_cierre_arqueo FOREIGN KEY (IDVinculacion) REFERENCES CierreDeCaja (IDCierre) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin2;

CREATE TABLE BaseTPV.DesgloseArqueo (
  ID int(11) NOT NULL AUTO_INCREMENT,
  IDVinculacion int(11) NOT NULL,
  Clave text NOT NULL,
  Valor text NOT NULL,
  PRIMARY KEY (ID),
  KEY fk_arqueo_desglose (IDVinculacion),
  CONSTRAINT fk_arqueo_desglose FOREIGN KEY (IDVinculacion) REFERENCES ArqueoCaja (ID) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

