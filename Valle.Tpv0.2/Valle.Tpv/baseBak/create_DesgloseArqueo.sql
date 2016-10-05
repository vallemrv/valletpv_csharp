CREATE TABLE `DesgloseArqueo` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `IDVinculacion` int(11) NOT NULL,
  `Clave` text NOT NULL,
  `Valor` text NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_arqueo_desglose` (`IDVinculacion`),
  CONSTRAINT `fk_arqueo_desglose` FOREIGN KEY (`IDVinculacion`) REFERENCES `ArqueoCaja` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8