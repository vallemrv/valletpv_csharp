CREATE TABLE `ArqueoCaja` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `IDVinculacion` int(11) NOT NULL,
  `Resultado` decimal(10,0) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_cierre_arqueo` (`IDVinculacion`),
  CONSTRAINT `fk_cierre_arqueo` FOREIGN KEY (`IDVinculacion`) REFERENCES `CierreDeCaja` (`IDCierre`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin2