-- MySQL dump 10.13  Distrib 9.7.2, for Linux (x86_64)
--
-- Host: localhost    Database: TicketsGO
-- ------------------------------------------------------
-- Server version	9.7.2

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20261017152039_EsquemaInicial','8.0.0'),('20261027145609_AddAvatarUrlToUsuario','8.0.0'),('20261027155420_AddImagenUrlToEvento','8.0.0'),('20261031005122_AddHistorialFechaAndNotificaciones','8.0.0');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `compra`
--

DROP TABLE IF EXISTS `compra`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compra` (
  `id_compra` int NOT NULL AUTO_INCREMENT,
  `id_evento` int NOT NULL,
  `id_usuario` int NOT NULL,
  `total_tickets` int NOT NULL,
  `monto_total` decimal(10,2) NOT NULL,
  `estado` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`id_compra`),
  KEY `IX_compra_id_evento` (`id_evento`),
  KEY `IX_compra_id_usuario` (`id_usuario`),
  CONSTRAINT `FK_compra_evento_id_evento` FOREIGN KEY (`id_evento`) REFERENCES `evento` (`id_evento`) ON DELETE RESTRICT,
  CONSTRAINT `FK_compra_usuario_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compra`
--

LOCK TABLES `compra` WRITE;
/*!40000 ALTER TABLE `compra` DISABLE KEYS */;
INSERT INTO `compra` VALUES (1,1,2,1,20000.00,'Realizada'),(2,3,2,1,2310.00,'Cancelada'),(3,3,2,1,2310.00,'Cancelada'),(4,4,2,2,30000.00,'Realizada'),(5,4,104,1,15000.00,'Realizada');
/*!40000 ALTER TABLE `compra` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evento`
--

DROP TABLE IF EXISTS `evento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `evento` (
  `id_evento` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `nombre` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `descripcion` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha_inicio` datetime NOT NULL,
  `fecha_fin` datetime NOT NULL,
  `provincia` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ciudad` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `establecimiento` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `privado` tinyint(1) NOT NULL,
  `tickets_totales` int NOT NULL,
  `tickets_vendidos` int NOT NULL,
  `precio_ticket` decimal(10,2) NOT NULL,
  `estado` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `imagen_url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id_evento`),
  KEY `IX_evento_id_usuario` (`id_usuario`),
  CONSTRAINT `FK_evento_usuario_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evento`
--

LOCK TABLES `evento` WRITE;
/*!40000 ALTER TABLE `evento` DISABLE KEYS */;
INSERT INTO `evento` VALUES (1,103,'Evento prueba',' evento prueba evento prueba evento prueba evento prueba evento prueba','2026-10-28 12:00:00','2026-10-28 14:00:00','San Luis','Villa Mercedes','Complejo prueba',1,100,1,20000.00,'Finalizado','/uploads/eventos/8ae68e25-f154-4837-bf25-f29c42af527a.webp'),(2,103,'Evento prueab 2','evento pruebna 2 22222222222222','2026-10-29 18:55:00','2026-10-30 18:55:00','asd','asd','asdsa',1,5,0,12210.00,'Cancelado','/uploads/eventos/8f2cd909-070d-491a-a580-5afb65ca2ee0.jpeg'),(3,103,'asdsa','dasdasdsadsadsa','2026-11-07 19:14:00','2026-11-19 19:14:00','asdas','sad','dsad',1,1,0,2310.00,'Publicado','/uploads/eventos/2a2caefe-e797-429b-8f20-c61ea52c6126.webp'),(4,103,'Night Evento','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ','2026-11-28 20:15:00','2027-01-29 20:16:00','San Luis','Capital','Complejo X',1,4,3,15000.00,'Publicado','/uploads/eventos/d9dbc13e-8a6a-4e7b-8983-c14e0da1e406.jpg');
/*!40000 ALTER TABLE `evento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `historial`
--

DROP TABLE IF EXISTS `historial`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `historial` (
  `id_historial` int NOT NULL AUTO_INCREMENT,
  `id_evento` int NOT NULL,
  `id_usuario` int NOT NULL,
  `descripcion` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_historial`),
  KEY `IX_historial_id_evento` (`id_evento`),
  KEY `IX_historial_id_usuario` (`id_usuario`),
  CONSTRAINT `FK_historial_evento_id_evento` FOREIGN KEY (`id_evento`) REFERENCES `evento` (`id_evento`) ON DELETE RESTRICT,
  CONSTRAINT `FK_historial_usuario_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `historial`
--

LOCK TABLES `historial` WRITE;
/*!40000 ALTER TABLE `historial` DISABLE KEYS */;
INSERT INTO `historial` VALUES (1,1,2,'Compra de 1 ticket(s) para el evento \'Evento prueba\'.','2026-10-30 21:54:30'),(2,3,2,'Compra de 1 ticket(s) para el evento \'asdsa\'.','2026-10-30 21:54:30'),(3,3,2,'Devolución de 1 ticket(s) de la compra #2.','2026-10-30 21:54:30'),(4,3,2,'Compra de 1 ticket(s) para el evento \'asdsa\'.','2026-10-30 21:54:30'),(5,4,2,'Compra de 2 ticket(s) para el evento \'Night Evento\'.','2026-10-30 21:54:30'),(6,4,2,'Transferencia del ticket 486ACEAF3D474919BBF53C1B0AC63598 a productorprueba@ticketsgo.com.','2026-10-30 21:54:30'),(7,4,103,'Ticket 486ACEAF3D474919BBF53C1B0AC63598 marcado como usado.','2026-10-30 21:54:30'),(8,4,104,'Compra de 1 ticket(s) para el evento \'Night Evento\'.','2026-10-30 21:55:58'),(9,4,104,'Transferencia del ticket D7472E91646C4E5EBCACEE357682DD34 a usuarioprueba@ticketsgo.com.','2026-10-30 21:56:13'),(10,4,2,'Recepción del ticket D7472E91646C4E5EBCACEE357682DD34 desde Night Evento.','2026-10-30 21:56:13'),(11,3,2,'Devolución de 1 ticket(s) de la compra #3.','2026-10-30 22:01:42'),(12,4,2,'Ticket 4A1977752361420C8C56D547A547E0CB marcado como usado.','2026-10-30 22:07:55'),(13,4,2,'Transferencia del ticket D7472E91646C4E5EBCACEE357682DD34 a usuariopruebados@ticketsgo.com.','2026-10-30 22:08:13'),(14,4,104,'Recepción del ticket D7472E91646C4E5EBCACEE357682DD34 desde Night Evento.','2026-10-30 22:08:13');
/*!40000 ALTER TABLE `historial` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notificacion`
--

DROP TABLE IF EXISTS `notificacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificacion` (
  `id_notificacion` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `titulo` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `mensaje` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `leida` tinyint(1) NOT NULL,
  PRIMARY KEY (`id_notificacion`),
  KEY `IX_notificacion_id_usuario` (`id_usuario`),
  CONSTRAINT `FK_notificacion_usuario_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificacion`
--

LOCK TABLES `notificacion` WRITE;
/*!40000 ALTER TABLE `notificacion` DISABLE KEYS */;
INSERT INTO `notificacion` VALUES (1,104,'Compra confirmada','Compraste 1 ticket(s) para \'Night Evento\'.','/Compra/MisCompras','2026-10-30 21:55:58',0),(2,103,'Nueva compra','Se compraron 1 ticket(s) para tu evento \'Night Evento\'.','/Evento/Details/4','2026-10-30 21:55:58',0),(3,104,'Ticket transferido','Transferiste un ticket de \'Night Evento\' a usuarioprueba@ticketsgo.com.','/Ticket/MisTickets','2026-10-30 21:56:13',0),(4,2,'Recibiste un ticket','Recibiste un ticket para \'Night Evento\'.','/Ticket/MisTickets','2026-10-30 21:56:13',1),(5,2,'Devolución confirmada','Se canceló la compra #3 de 1 ticket(s) para \'asdsa\'.','/Compra/MisCompras','2026-10-30 22:01:42',1),(6,103,'Tickets devueltos','Se devolvieron 1 ticket(s) de tu evento \'asdsa\'.','/Evento/Details/3','2026-10-30 22:01:42',0),(7,2,'Ticket transferido','Transferiste un ticket de \'Night Evento\' a usuariopruebados@ticketsgo.com.','/Ticket/MisTickets','2026-10-30 22:08:13',1),(8,104,'Recibiste un ticket','Recibiste un ticket para \'Night Evento\'.','/Ticket/MisTickets','2026-10-30 22:08:13',0);
/*!40000 ALTER TABLE `notificacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ticket`
--

DROP TABLE IF EXISTS `ticket`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ticket` (
  `id_ticket` int NOT NULL AUTO_INCREMENT,
  `id_evento` int NOT NULL,
  `id_compra` int NOT NULL,
  `id_usuario` int DEFAULT NULL,
  `estado` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `codigo` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`id_ticket`),
  KEY `IX_ticket_id_compra` (`id_compra`),
  KEY `IX_ticket_id_evento` (`id_evento`),
  KEY `IX_ticket_id_usuario` (`id_usuario`),
  CONSTRAINT `FK_ticket_compra_id_compra` FOREIGN KEY (`id_compra`) REFERENCES `compra` (`id_compra`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ticket_evento_id_evento` FOREIGN KEY (`id_evento`) REFERENCES `evento` (`id_evento`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ticket_usuario_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ticket`
--

LOCK TABLES `ticket` WRITE;
/*!40000 ALTER TABLE `ticket` DISABLE KEYS */;
INSERT INTO `ticket` VALUES (1,1,1,2,'NoUsado','4530F6698CE649E3B7686603C60DF54E'),(4,4,4,2,'Usado','4A1977752361420C8C56D547A547E0CB'),(5,4,4,103,'Usado','486ACEAF3D474919BBF53C1B0AC63598'),(6,4,5,104,'NoUsado','D7472E91646C4E5EBCACEE357682DD34');
/*!40000 ALTER TABLE `ticket` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transferencia`
--

DROP TABLE IF EXISTS `transferencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transferencia` (
  `id_transferencia` int NOT NULL AUTO_INCREMENT,
  `id_ticket` int NOT NULL,
  `id_usuario_origen` int NOT NULL,
  `id_usuario_destino` int NOT NULL,
  `fecha` datetime NOT NULL,
  PRIMARY KEY (`id_transferencia`),
  KEY `IX_transferencia_id_ticket` (`id_ticket`),
  KEY `IX_transferencia_id_usuario_destino` (`id_usuario_destino`),
  KEY `IX_transferencia_id_usuario_origen` (`id_usuario_origen`),
  CONSTRAINT `FK_transferencia_ticket_id_ticket` FOREIGN KEY (`id_ticket`) REFERENCES `ticket` (`id_ticket`) ON DELETE RESTRICT,
  CONSTRAINT `FK_transferencia_usuario_id_usuario_destino` FOREIGN KEY (`id_usuario_destino`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT,
  CONSTRAINT `FK_transferencia_usuario_id_usuario_origen` FOREIGN KEY (`id_usuario_origen`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transferencia`
--

LOCK TABLES `transferencia` WRITE;
/*!40000 ALTER TABLE `transferencia` DISABLE KEYS */;
INSERT INTO `transferencia` VALUES (1,5,2,103,'2026-10-30 21:40:02'),(2,6,104,2,'2026-10-30 21:56:13'),(3,6,2,104,'2026-10-30 22:08:13');
/*!40000 ALTER TABLE `transferencia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `apellido` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `email` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `telefono` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `rol` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `estado` tinyint(1) NOT NULL,
  `email_verificado` tinyint(1) NOT NULL,
  `verification_token` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `reset_password_expires` datetime DEFAULT NULL,
  `AvatarUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `IX_usuario_email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Admin','Test','admin@test.com','123456789','AQAAAAIAAYagAAAAEFdSBhQ4LeqUQXQ/UJ9hGkn95+EFtwLZMLaIJpU0p7dPTDiH4rdSAABrj8WfhgtSig==','Administrador',1,1,NULL,NULL,NULL),(2,'Usuario','Pruebaa','usuarioprueba@ticketsgo.com','1234556789','AQAAAAIAAYagAAAAEJoHN/g1v6e/w1Hg6RABdFK5DocDy3d4DrEm20/v9TaN8EUXFBl7kGntOIy6xZatIw==','Usuario',1,0,NULL,NULL,'/uploads/avatars/e407f421-4660-47bf-87bf-c9e4d9639006.webp'),(3,'Ana','Gómez','ana.gomez@test.com','111111111','x','Productor',1,1,NULL,NULL,NULL),(4,'Luis','Pérez','luis.perez@test.com','222222222','x','Usuario',1,1,NULL,NULL,NULL),(5,'María','Rodríguez','maria.rodriguez@test.com','333333333','x','Usuario',1,1,NULL,NULL,NULL),(6,'Carlos','Fernández','carlos.fernandez@test.com','444444444','x','Usuario',0,1,NULL,NULL,NULL),(7,'Lucía','Martínez','lucia.martinez@test.com','555555555','x','Usuario',0,1,NULL,NULL,NULL),(8,'Jorge','López','jorge.lopez@test.com','666666666','x','Usuario',1,0,NULL,NULL,NULL),(9,'Sofía','Díaz','sofia.diaz@test.com','777777777','x','Usuario',1,1,NULL,NULL,NULL),(10,'Pedro','Sánchez','pedro.sanchez@test.com','888888888','x','Administrador',0,1,NULL,NULL,NULL),(11,'Valentina','Romero','valentina.romero@test.com','999999999','x','Usuario',0,1,NULL,NULL,NULL),(12,'Diego','Torres','diego.torres@test.com','101010101','x','Usuario',1,1,NULL,NULL,NULL),(13,'Camila','Ruiz','camila.ruiz@test.com','121212121','x','Usuario',1,1,NULL,NULL,NULL),(14,'Martín','Acosta','martin.acosta@test.com','131313131','x','Usuario',1,0,NULL,NULL,NULL),(15,'Florencia','Medina','florencia.medina@test.com','141414141','x','Usuario',1,1,NULL,NULL,NULL),(16,'Nicolás','Herrera','nicolas.herrera@test.com','151515151','x','Usuario',0,1,NULL,NULL,NULL),(17,'Julieta','Silva','julieta.silva@test.com','161616161','x','Administrador',1,1,NULL,NULL,NULL),(68,'Andrés','Morales','andres.morales@test.com','110000016','x','Usuario',1,1,NULL,NULL,NULL),(69,'Paula','Ortiz','paula.ortiz@test.com','110000017','x','Usuario',1,1,NULL,NULL,NULL),(70,'Santiago','Castro','santiago.castro@test.com','110000018','x','Usuario',0,1,NULL,NULL,NULL),(71,'Micaela','Vargas','micaela.vargas@test.com','110000019','x','Usuario',1,1,NULL,NULL,NULL),(72,'Federico','Ríos','federico.rios@test.com','110000020','x','Usuario',1,0,NULL,NULL,NULL),(73,'Agustina','Blanco','agustina.blanco@test.com','110000021','x','Usuario',1,1,NULL,NULL,NULL),(74,'Tomás','Navarro','tomas.navarro@test.com','110000022','x','Usuario',1,1,NULL,NULL,NULL),(75,'Renata','Domínguez','renata.dominguez@test.com','110000023','x','Usuario',0,1,NULL,NULL,NULL),(76,'Bautista','Peralta','bautista.peralta@test.com','110000024','x','Usuario',1,1,NULL,NULL,NULL),(77,'Delfina','Suárez','delfina.suarez@test.com','110000025','x','Usuario',1,1,NULL,NULL,NULL),(78,'Ignacio','Molina','ignacio.molina@test.com','110000026','x','Administrador',1,1,NULL,NULL,NULL),(79,'Emilia','Fuentes','emilia.fuentes@test.com','110000027','x','Usuario',1,0,NULL,NULL,NULL),(80,'Thiago','Cabrera','thiago.cabrera@test.com','110000028','x','Usuario',0,1,NULL,NULL,NULL),(81,'Isabella','Reyes','isabella.reyes@test.com','110000029','x','Usuario',1,1,NULL,NULL,NULL),(82,'Facundo','Aguirre','facundo.aguirre@test.com','110000030','x','Usuario',1,1,NULL,NULL,NULL),(83,'Josefina','Ponce','josefina.ponce@test.com','110000031','x','Usuario',1,1,NULL,NULL,NULL),(84,'Lautaro','Campos','lautaro.campos@test.com','110000032','x','Usuario',0,1,NULL,NULL,NULL),(85,'Antonella','Vega','antonella.vega@test.com','110000033','x','Usuario',1,1,NULL,NULL,NULL),(86,'Benjamín','Luna','benjamin.luna@test.com','110000034','x','Usuario',1,0,NULL,NULL,NULL),(87,'Victoria','Juárez','victoria.juarez@test.com','110000035','x','Usuario',1,1,NULL,NULL,NULL),(88,'Mateo','Caballero','mateo.caballero@test.com','110000036','x','Usuario',1,1,NULL,NULL,NULL),(89,'Catalina','Rojas','catalina.rojas@test.com','110000037','x','Administrador',0,1,NULL,NULL,NULL),(90,'Simón','Miranda','simon.miranda@test.com','110000038','x','Usuario',1,1,NULL,NULL,NULL),(91,'Martina','Cortés','martina.cortes@test.com','110000039','x','Usuario',1,1,NULL,NULL,NULL),(92,'Joaquín','Núñez','joaquin.nunez@test.com','110000040','x','Usuario',0,1,NULL,NULL,NULL),(93,'Guadalupe','Salazar','guadalupe.salazar@test.com','110000041','x','Usuario',1,1,NULL,NULL,NULL),(94,'Ramiro','Orellana','ramiro.orellana@test.com','110000042','x','Usuario',1,0,NULL,NULL,NULL),(95,'Abril','Cáceres','abril.caceres@test.com','110000043','x','Usuario',1,1,NULL,NULL,NULL),(96,'Elías','Paredes','elias.paredes@test.com','110000044','x','Usuario',1,1,NULL,NULL,NULL),(97,'Regina','Arias','regina.arias@test.com','110000045','x','Usuario',0,1,NULL,NULL,NULL),(98,'Gonzalo','Ferrari','gonzalo.ferrari@test.com','110000046','x','Usuario',1,1,NULL,NULL,NULL),(99,'Amanda','Cordero','amanda.cordero@test.com','110000047','x','Usuario',1,1,NULL,NULL,NULL),(100,'Bruno','Villalba','bruno.villalba@test.com','110000048','x','Usuario',0,1,NULL,NULL,NULL),(101,'Lola','Benítez','lola.benitez@test.com','110000049','x','Usuario',1,1,NULL,NULL,NULL),(102,'Máximo','Quiroga','maximo.quiroga@test.com','110000050','x','Usuario',1,1,NULL,NULL,NULL),(103,'Productor','Prueba','productorprueba@ticketsgo.com','123456789','AQAAAAIAAYagAAAAECyrLOLZxKfM11uM2SB4dytk71bhsQOjpaQ30yHqhBWOxbyHFDsQ6kaEuS3XKvn8TQ==','Productor',1,0,NULL,NULL,NULL),(104,'Usuario','Prueba Dos','usuariopruebados@ticketsgo.com','123456789','AQAAAAIAAYagAAAAELSueSAOxKJhohZVHOE65JAI5OloLtBbhoLpShHoYq4sG+R7er3SK5JqzcZbBApq4A==','Usuario',1,0,NULL,NULL,NULL);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-10-31 13:32:11
