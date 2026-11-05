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
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'fe5c5e23-b8fb-11f1-93d3-b63fbee26e6f:1-186';

--
-- Current Database: `TicketsGO`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `TicketsGO` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `TicketsGO`;

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compra`
--

LOCK TABLES `compra` WRITE;
/*!40000 ALTER TABLE `compra` DISABLE KEYS */;
INSERT INTO `compra` VALUES (1,1,2,1,20000.00,'Realizada'),(2,3,2,1,2310.00,'Cancelada'),(3,3,2,1,2310.00,'Cancelada'),(4,4,2,2,30000.00,'Realizada'),(5,4,104,1,15000.00,'Realizada'),(6,18,1,3,54000.00,'Realizada');
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evento`
--

LOCK TABLES `evento` WRITE;
/*!40000 ALTER TABLE `evento` DISABLE KEYS */;
INSERT INTO `evento` VALUES (1,103,'Evento prueba',' evento prueba evento prueba evento prueba evento prueba evento prueba','2026-10-28 12:00:00','2026-10-28 14:00:00','San Luis','Villa Mercedes','Complejo prueba',1,100,1,20000.00,'Finalizado','/uploads/eventos/8ae68e25-f154-4837-bf25-f29c42af527a.webp'),(2,103,'Evento prueab 2','evento pruebna 2 22222222222222','2026-10-29 18:55:00','2026-10-30 18:55:00','asd','asd','asdsa',1,5,0,12210.00,'Cancelado','/uploads/eventos/8f2cd909-070d-491a-a580-5afb65ca2ee0.jpeg'),(3,103,'asdsa','dasdasdsadsadsa','2026-11-07 19:14:00','2026-11-19 19:14:00','asdas','sad','dsad',1,1,0,2310.00,'Cancelado','/uploads/eventos/2a2caefe-e797-429b-8f20-c61ea52c6126.webp'),(4,103,'Night Evento','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ','2026-11-28 20:15:00','2027-01-29 20:16:00','San Luis','Capital','Complejo X',1,4,3,15000.00,'Publicado','/uploads/eventos/d9dbc13e-8a6a-4e7b-8983-c14e0da1e406.jpg'),(5,105,'Festival de música','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.\r\n','2026-12-12 12:35:00','2026-12-13 11:34:00','San Luis','San Luis','Establecimiento x',1,100,0,15000.00,'Publicado','/uploads/eventos/e7d81ec8-6ad2-4292-9018-45f3478c9f5e.jpg'),(6,105,'Coctel evento','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.\r\n','2026-12-23 11:43:00','2026-12-24 11:43:00','Provincia prueba','Ciudad prueba','Establecimiento prueba',1,200,0,10000.00,'Publicado','/uploads/eventos/542fadb5-81dc-4538-beb6-9b727695fa3b.webp'),(7,105,'CAMBIO DE NOMBRE DINAMICO','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.\r\n\r\nTres párrafos estándar\r\n\r\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur porta erat quis magna accumsan, elementum congue lorem varius. Sed sed efficitur sapien. Mauris interdum nisl ac nisl feugiat, non hendrerit dolor interdum. Duis rhoncus ipsum in dictum molestie. Proin ligula urna, finibus id nulla vel, venenatis molestie lectus.\r\nVestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Aliquam interdum pretium tellus, in convallis elit euismod vitae. Vivamus hendrerit ex pretium tortor feugiat congue. Ut facilisis elementum sapien, id sollicitudin ex lacinia eget. Phasellus sed est convallis, tristique enim eget, pulvinar lectus.\r\nDonec scelerisque tincidunt risus id suscipit. Cras varius dapibus elit, eget euismod purus elementum vitae. Sed dictum nisl eu magna dictum placerat. Mauris vel dictum purus, et sollicitudin nisl. Fusce volutpat ex nisl, sed finibus augue laoreet convallis. Praesent porttitor sapien ac neque pretium molestie.\r\n\r\nFormato de lista (Items)\r\n\r\n• Lorem ipsum dolor sit amet.\r\n• Consectetur adipiscing elit.\r\n• Sed do eiusmod tempor incididunt.\r\n• Ut labore et dolore magna aliqua.\r\nSi necesitas que tenga una extensión específica (como un número exacto de palabras o párrafos) o prefieres una alternativa en español, dime y lo adapto enseguida.\r\n','2027-01-02 11:49:00','2027-01-03 11:49:00','X','X','X',1,100,0,5000.00,'Cancelado','/uploads/eventos/105ad498-e01a-4e93-b96f-1e2ddccb6286.jpg'),(8,105,'edit dinamico ','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.\r\n\r\nTres párrafos estándar\r\n\r\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur porta erat quis magna accumsan, elementum congue lorem varius. Sed sed efficitur sapien. Mauris interdum nisl ac nisl feugiat, non hendrerit dolor interdum. Duis rhoncus ipsum in dictum molestie. Proin ligula urna, finibus id nulla vel, venenatis molestie lectus.\r\nVestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Aliquam interdum pretium tellus, in convallis elit euismod vitae. Vivamus hendrerit ex pretium tortor feugiat congue. Ut facilisis elementum sapien, id sollicitudin ex lacinia eget. Phasellus sed est convallis, tristique enim eget, pulvinar lectus.\r\nDonec scelerisque tincidunt risus id suscipit. Cras varius dapibus elit, eget euismod purus elementum vitae. Sed dictum nisl eu magna dictum placerat. Mauris vel dictum purus, et sollicitudin nisl. Fusce volutpat ex nisl, sed finibus augue laoreet convallis. Praesent porttitor sapien ac neque pretium molestie.\r\n\r\nFormato de lista (Items)\r\n\r\n• Lorem ipsum dolor sit amet.\r\n• Consectetur adipiscing elit.\r\n• Sed do eiusmod tempor incididunt.\r\n• Ut labore et dolore magna aliqua.\r\nSi necesitas que tenga una extensión específica (como un número exacto de palabras o párrafos) o prefieres una alternativa en español, dime y lo adapto enseguida.\r\n','2027-01-02 11:55:00','2027-01-03 11:55:00','X','X','X',1,111,0,1111.00,'Cancelado','/uploads/eventos/0ad6ebd9-fe68-4c49-9325-d12013b65e60.jpeg'),(9,4,'Recital de Rock Nacional','Una noche con las mejores bandas del rock argentino.','2026-12-15 21:00:00','2026-12-16 01:00:00','Buenos Aires','La Plata','Estadio Único',0,5000,0,15000.00,'Publicado',NULL),(10,4,'Obra de Teatro: El Fantasma','Clásica obra de teatro con un elenco de primer nivel.','2026-12-20 19:30:00','2026-12-20 22:00:00','Córdoba','Córdoba Capital','Teatro del Libertador',0,300,0,8000.00,'Publicado',NULL),(11,4,'Festival de Jazz','Tres días de jazz con artistas nacionales e internacionales.','2027-01-10 18:00:00','2027-01-12 23:00:00','Mendoza','Mendoza Capital','Parque Central',0,2000,0,12000.00,'Publicado',NULL),(12,4,'Stand Up Comedy Night','Los mejores comediantes en una noche imperdible.','2026-12-05 21:00:00','2026-12-05 23:30:00','Santa Fe','Rosario','Teatro Broadway',0,500,0,6000.00,'Publicado',NULL),(13,4,'Concierto Sinfónico','La orquesta filarmónica interpreta clásicos de Beethoven.','2027-02-14 20:00:00','2027-02-14 22:30:00','Buenos Aires','CABA','Teatro Colón',1,800,0,20000.00,'Publicado',NULL),(14,4,'Feria de Artesanías','Exposición y venta de artesanías regionales.','2026-12-01 10:00:00','2026-12-03 20:00:00','Salta','Salta Capital','Centro de Convenciones',0,1000,0,3000.00,'Publicado',NULL),(15,4,'Torneo de E-Sports','Competencia de videojuegos con premios en efectivo.','2027-01-25 14:00:00','2027-01-25 22:00:00','Buenos Aires','CABA','Predio Ferial',0,1500,0,5000.00,'Publicado',NULL),(16,4,'Cine al Aire Libre','Proyección de películas clásicas bajo las estrellas.','2026-12-28 20:30:00','2026-12-28 23:30:00','Neuquén','Neuquén Capital','Parque Central',0,600,0,4000.00,'Publicado',NULL),(17,4,'Festival Gastronómico','Degustación de platos típicos de toda la región.','2027-03-05 11:00:00','2027-03-07 22:00:00','Tucumán','San Miguel de Tucumán','Predio Ferial',0,3000,0,7000.00,'Publicado',NULL),(18,4,'Concierto de Música Electrónica','DJs internacionales en una noche inolvidable.','2027-02-28 23:00:00','2027-03-01 05:00:00','Buenos Aires','Mar del Plata','Playa Bristol',0,4000,3,18000.00,'Publicado',NULL),(19,1,'Recital de Prueba','Evento de prueba creado desde Postman','2027-05-15 21:00:00','2027-05-16 01:00:00','San Luis','Villa Mercedes','Teatro',0,100,0,5000.00,'Cancelado',NULL),(20,1,'Recital de Prueba','Evento de prueba creado desde Postman','2027-05-15 21:00:00','2027-05-16 01:00:00','San Luis','Villa Mercedes','Teatro',0,100,0,5000.00,'Publicado',NULL),(21,1,'Recital de Prueba','Evento de prueba creado desde Postman','2027-05-15 21:00:00','2027-05-16 01:00:00','San Luis','Villa Mercedes','Teatro',0,100,0,5000.00,'Publicado',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `historial`
--

LOCK TABLES `historial` WRITE;
/*!40000 ALTER TABLE `historial` DISABLE KEYS */;
INSERT INTO `historial` VALUES (1,1,2,'Compra de 1 ticket(s) para el evento \'Evento prueba\'.','2026-10-30 21:54:30'),(2,3,2,'Compra de 1 ticket(s) para el evento \'asdsa\'.','2026-10-30 21:54:30'),(3,3,2,'Devolución de 1 ticket(s) de la compra #2.','2026-10-30 21:54:30'),(4,3,2,'Compra de 1 ticket(s) para el evento \'asdsa\'.','2026-10-30 21:54:30'),(5,4,2,'Compra de 2 ticket(s) para el evento \'Night Evento\'.','2026-10-30 21:54:30'),(6,4,2,'Transferencia del ticket 486ACEAF3D474919BBF53C1B0AC63598 a productorprueba@ticketsgo.com.','2026-10-30 21:54:30'),(7,4,103,'Ticket 486ACEAF3D474919BBF53C1B0AC63598 marcado como usado.','2026-10-30 21:54:30'),(8,4,104,'Compra de 1 ticket(s) para el evento \'Night Evento\'.','2026-10-30 21:55:58'),(9,4,104,'Transferencia del ticket D7472E91646C4E5EBCACEE357682DD34 a usuarioprueba@ticketsgo.com.','2026-10-30 21:56:13'),(10,4,2,'Recepción del ticket D7472E91646C4E5EBCACEE357682DD34 desde Night Evento.','2026-10-30 21:56:13'),(11,3,2,'Devolución de 1 ticket(s) de la compra #3.','2026-10-30 22:01:42'),(12,4,2,'Ticket 4A1977752361420C8C56D547A547E0CB marcado como usado.','2026-10-30 22:07:55'),(13,4,2,'Transferencia del ticket D7472E91646C4E5EBCACEE357682DD34 a usuariopruebados@ticketsgo.com.','2026-10-30 22:08:13'),(14,4,104,'Recepción del ticket D7472E91646C4E5EBCACEE357682DD34 desde Night Evento.','2026-10-30 22:08:13'),(15,5,105,'Creación del evento \'Festival de música\'.','2026-11-02 11:35:28'),(16,5,105,'Modificación del evento \'Festival de música\'.','2026-11-02 11:36:18'),(17,5,105,'Modificación del evento \'Festival de música\'.','2026-11-02 11:36:22'),(18,5,105,'Modificación del evento \'Festival de música\'.','2026-11-02 11:38:19'),(19,5,105,'Modificación del evento \'Festival de música\'.','2026-11-02 11:38:32'),(20,6,105,'Creación del evento \'Coctel evento\'.','2026-11-02 11:44:42'),(21,7,105,'Creación del evento \'Evento prueba abm dinamico\'.','2026-11-02 11:49:50'),(22,7,105,'Modificación del evento \'CAMBIO DE NOMBRE DINAMICO\'.','2026-11-02 11:50:04'),(23,7,105,'Cancelación del evento \'CAMBIO DE NOMBRE DINAMICO\'.','2026-11-02 11:50:14'),(24,8,105,'Creación del evento \'Create abm dinamico\'.','2026-11-02 11:55:39'),(25,8,105,'Modificación del evento \'edit dinamico \'.','2026-11-02 11:55:52'),(26,8,105,'Cancelación del evento \'edit dinamico \'.','2026-11-02 11:56:00'),(27,3,1,'Cancelación del evento \'asdsa\'.','2026-11-02 12:12:30'),(28,18,1,'Compra de 3 ticket(s) para el evento \'Concierto de Música Electrónica\'.','2026-11-02 12:16:16'),(29,19,1,'Creación del evento \'Recital de Prueba\'.','2026-11-02 13:08:26'),(30,20,1,'Creación del evento \'Recital de Prueba\'.','2026-11-02 13:09:10'),(31,21,1,'Creación del evento \'Recital de Prueba\'.','2026-11-02 13:10:04'),(32,19,1,'Modificación del evento \'Recital de Prueba\'.','2026-11-02 13:22:48'),(33,19,1,'Modificación del evento \'Recital de Prueba\'.','2026-11-02 13:23:33'),(34,19,1,'Modificación del evento \'Recital de Prueba\'.','2026-11-02 13:23:58'),(35,19,1,'Cancelación del evento \'Recital de Prueba\'.','2026-11-02 13:24:26');
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificacion`
--

LOCK TABLES `notificacion` WRITE;
/*!40000 ALTER TABLE `notificacion` DISABLE KEYS */;
INSERT INTO `notificacion` VALUES (1,104,'Compra confirmada','Compraste 1 ticket(s) para \'Night Evento\'.','/Compra/MisCompras','2026-10-30 21:55:58',0),(2,103,'Nueva compra','Se compraron 1 ticket(s) para tu evento \'Night Evento\'.','/Evento/Details/4','2026-10-30 21:55:58',0),(3,104,'Ticket transferido','Transferiste un ticket de \'Night Evento\' a usuarioprueba@ticketsgo.com.','/Ticket/MisTickets','2026-10-30 21:56:13',0),(4,2,'Recibiste un ticket','Recibiste un ticket para \'Night Evento\'.','/Ticket/MisTickets','2026-10-30 21:56:13',1),(5,2,'Devolución confirmada','Se canceló la compra #3 de 1 ticket(s) para \'asdsa\'.','/Compra/MisCompras','2026-10-30 22:01:42',1),(6,103,'Tickets devueltos','Se devolvieron 1 ticket(s) de tu evento \'asdsa\'.','/Evento/Details/3','2026-10-30 22:01:42',0),(7,2,'Ticket transferido','Transferiste un ticket de \'Night Evento\' a usuariopruebados@ticketsgo.com.','/Ticket/MisTickets','2026-10-30 22:08:13',1),(8,104,'Recibiste un ticket','Recibiste un ticket para \'Night Evento\'.','/Ticket/MisTickets','2026-10-30 22:08:13',0),(9,105,'Evento creado','El evento \'Festival de música\' fue creado correctamente.','/Evento/Details/5','2026-11-02 11:35:28',0),(10,105,'Evento creado','El evento \'Coctel evento\' fue creado correctamente.','/Evento/Details/6','2026-11-02 11:44:42',0),(11,105,'Evento creado','El evento \'Evento prueba abm dinamico\' fue creado correctamente.','/Evento/Details/7','2026-11-02 11:49:50',0),(12,105,'Evento creado','El evento \'Create abm dinamico\' fue creado correctamente.','/Evento/Details/8','2026-11-02 11:55:39',0),(13,1,'Compra confirmada','Compraste 3 ticket(s) para \'Concierto de Música Electrónica\'.','/Compra/MisCompras','2026-11-02 12:16:16',0),(14,4,'Nueva compra','Se compraron 3 ticket(s) para tu evento \'Concierto de Música Electrónica\'.','/Evento/Details/18','2026-11-02 12:16:16',0),(15,1,'Evento creado','El evento \'Recital de Prueba\' fue creado correctamente.','/Evento/Details/19','2026-11-02 13:08:26',0),(16,1,'Evento creado','El evento \'Recital de Prueba\' fue creado correctamente.','/Evento/Details/20','2026-11-02 13:09:10',0),(17,1,'Evento creado','El evento \'Recital de Prueba\' fue creado correctamente.','/Evento/Details/21','2026-11-02 13:10:04',0);
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ticket`
--

LOCK TABLES `ticket` WRITE;
/*!40000 ALTER TABLE `ticket` DISABLE KEYS */;
INSERT INTO `ticket` VALUES (1,1,1,2,'NoUsado','4530F6698CE649E3B7686603C60DF54E'),(4,4,4,2,'Usado','4A1977752361420C8C56D547A547E0CB'),(5,4,4,103,'Usado','486ACEAF3D474919BBF53C1B0AC63598'),(6,4,5,104,'NoUsado','D7472E91646C4E5EBCACEE357682DD34'),(7,18,6,1,'NoUsado','DD0DF42DC84C447683D3D1307644717E'),(8,18,6,1,'NoUsado','B1129F70591B45CD80BFE29A5C165EC6'),(9,18,6,1,'NoUsado','2144ADCCAD0641F38CAC63EEB0DBB50C');
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
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Admin','Test','admin@test.com','123456789','AQAAAAIAAYagAAAAEFdSBhQ4LeqUQXQ/UJ9hGkn95+EFtwLZMLaIJpU0p7dPTDiH4rdSAABrj8WfhgtSig==','Administrador',1,1,NULL,NULL,NULL),(2,'Usuario','Pruebaa','usuarioprueba@ticketsgo.com','1234556789','AQAAAAIAAYagAAAAEJoHN/g1v6e/w1Hg6RABdFK5DocDy3d4DrEm20/v9TaN8EUXFBl7kGntOIy6xZatIw==','Usuario',1,0,NULL,NULL,'/uploads/avatars/e407f421-4660-47bf-87bf-c9e4d9639006.webp'),(3,'Ana','Gómez','ana.gomez@test.com','111111111','x','Productor',1,1,NULL,NULL,NULL),(4,'Luis','Pérez','luis.perez@test.com','222222222','x','Usuario',1,1,NULL,NULL,NULL),(5,'María','Rodríguez','maria.rodriguez@test.com','333333333','x','Usuario',1,1,NULL,NULL,NULL),(6,'Carlos','Fernández','carlos.fernandez@test.com','444444444','x','Usuario',0,1,NULL,NULL,NULL),(7,'Lucía','Martínez','lucia.martinez@test.com','555555555','x','Usuario',0,1,NULL,NULL,NULL),(8,'Jorge','López','jorge.lopez@test.com','666666666','x','Usuario',1,0,NULL,NULL,NULL),(9,'Sofía','Díaz','sofia.diaz@test.com','777777777','x','Usuario',1,1,NULL,NULL,NULL),(10,'Pedro','Sánchez','pedro.sanchez@test.com','888888888','x','Administrador',0,1,NULL,NULL,NULL),(11,'Valentina','Romero','valentina.romero@test.com','999999999','x','Usuario',0,1,NULL,NULL,NULL),(12,'Diego','Torres','diego.torres@test.com','101010101','x','Usuario',1,1,NULL,NULL,NULL),(13,'Camila','Ruiz','camila.ruiz@test.com','121212121','x','Usuario',1,1,NULL,NULL,NULL),(14,'Martín','Acosta','martin.acosta@test.com','131313131','x','Usuario',1,0,NULL,NULL,NULL),(15,'Florencia','Medina','florencia.medina@test.com','141414141','x','Usuario',1,1,NULL,NULL,NULL),(16,'Nicolás','Herrera','nicolas.herrera@test.com','151515151','x','Usuario',0,1,NULL,NULL,NULL),(17,'Julieta','Silva','julieta.silva@test.com','161616161','x','Administrador',1,1,NULL,NULL,NULL),(68,'Andrés','Morales','andres.morales@test.com','110000016','x','Usuario',1,1,NULL,NULL,NULL),(69,'Paula','Ortiz','paula.ortiz@test.com','110000017','x','Usuario',1,1,NULL,NULL,NULL),(70,'Santiago','Castro','santiago.castro@test.com','110000018','x','Usuario',0,1,NULL,NULL,NULL),(71,'Micaela','Vargas','micaela.vargas@test.com','110000019','x','Usuario',1,1,NULL,NULL,NULL),(72,'Federico','Ríos','federico.rios@test.com','110000020','x','Usuario',1,0,NULL,NULL,NULL),(73,'Agustina','Blanco','agustina.blanco@test.com','110000021','x','Usuario',1,1,NULL,NULL,NULL),(74,'Tomás','Navarro','tomas.navarro@test.com','110000022','x','Usuario',1,1,NULL,NULL,NULL),(75,'Renata','Domínguez','renata.dominguez@test.com','110000023','x','Usuario',0,1,NULL,NULL,NULL),(76,'Bautista','Peralta','bautista.peralta@test.com','110000024','x','Usuario',1,1,NULL,NULL,NULL),(77,'Delfina','Suárez','delfina.suarez@test.com','110000025','x','Usuario',1,1,NULL,NULL,NULL),(78,'Ignacio','Molina','ignacio.molina@test.com','110000026','x','Administrador',1,1,NULL,NULL,NULL),(79,'Emilia','Fuentes','emilia.fuentes@test.com','110000027','x','Usuario',1,0,NULL,NULL,NULL),(80,'Thiago','Cabrera','thiago.cabrera@test.com','110000028','x','Usuario',0,1,NULL,NULL,NULL),(81,'Isabella','Reyes','isabella.reyes@test.com','110000029','x','Usuario',1,1,NULL,NULL,NULL),(82,'Facundo','Aguirre','facundo.aguirre@test.com','110000030','x','Usuario',1,1,NULL,NULL,NULL),(83,'Josefina','Ponce','josefina.ponce@test.com','110000031','x','Usuario',1,1,NULL,NULL,NULL),(84,'Lautaro','Campos','lautaro.campos@test.com','110000032','x','Usuario',0,1,NULL,NULL,NULL),(85,'Antonella','Vega','antonella.vega@test.com','110000033','x','Usuario',1,1,NULL,NULL,NULL),(86,'Benjamín','Luna','benjamin.luna@test.com','110000034','x','Usuario',1,0,NULL,NULL,NULL),(87,'Victoria','Juárez','victoria.juarez@test.com','110000035','x','Usuario',1,1,NULL,NULL,NULL),(88,'Mateo','Caballero','mateo.caballero@test.com','110000036','x','Usuario',1,1,NULL,NULL,NULL),(89,'Catalina','Rojas','catalina.rojas@test.com','110000037','x','Administrador',0,1,NULL,NULL,NULL),(90,'Simón','Miranda','simon.miranda@test.com','110000038','x','Usuario',1,1,NULL,NULL,NULL),(91,'Martina','Cortés','martina.cortes@test.com','110000039','x','Usuario',1,1,NULL,NULL,NULL),(92,'Joaquín','Núñez','joaquin.nunez@test.com','110000040','x','Usuario',0,1,NULL,NULL,NULL),(93,'Guadalupe','Salazar','guadalupe.salazar@test.com','110000041','x','Usuario',1,1,NULL,NULL,NULL),(94,'Ramiro','Orellana','ramiro.orellana@test.com','110000042','x','Usuario',1,0,NULL,NULL,NULL),(95,'Abril','Cáceres','abril.caceres@test.com','110000043','x','Usuario',1,1,NULL,NULL,NULL),(96,'Elías','Paredes','elias.paredes@test.com','110000044','x','Usuario',1,1,NULL,NULL,NULL),(97,'Regina','Arias','regina.arias@test.com','110000045','x','Usuario',0,1,NULL,NULL,NULL),(98,'Gonzalo','Ferrari','gonzalo.ferrari@test.com','110000046','x','Usuario',1,1,NULL,NULL,NULL),(99,'Amanda','Cordero','amanda.cordero@test.com','110000047','x','Usuario',1,1,NULL,NULL,NULL),(100,'Bruno','Villalba','bruno.villalba@test.com','110000048','x','Usuario',0,1,NULL,NULL,NULL),(101,'Lola','Benítez','lola.benitez@test.com','110000049','x','Usuario',1,1,NULL,NULL,NULL),(102,'Máximo','Quiroga','maximo.quiroga@test.com','110000050','x','Usuario',1,1,NULL,NULL,NULL),(103,'Productor','Prueba','productorprueba@ticketsgo.com','123456789','AQAAAAIAAYagAAAAECyrLOLZxKfM11uM2SB4dytk71bhsQOjpaQ30yHqhBWOxbyHFDsQ6kaEuS3XKvn8TQ==','Productor',1,0,NULL,NULL,NULL),(104,'Usuario','Prueba Dos','usuariopruebados@ticketsgo.com','123456789','AQAAAAIAAYagAAAAELSueSAOxKJhohZVHOE65JAI5OloLtBbhoLpShHoYq4sG+R7er3SK5JqzcZbBApq4A==','Usuario',1,0,NULL,NULL,'/uploads/avatars/9bef3679-8714-40ba-b9bd-dbb22b07fe81.webp'),(105,'Productor ','Prueba Dos','productorpruebados@ticketsgo.com','123456','AQAAAAIAAYagAAAAEGhA0p0kJFmMzn5hZpxSzawmPhWRmPYCzYkkK1fhuUXYeVS4MlvFWivzyhprOKLaJA==','Productor',1,0,NULL,NULL,NULL);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-05 11:41:44
