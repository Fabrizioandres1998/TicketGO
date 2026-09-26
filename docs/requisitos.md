# Requisitos del sistema — TicketGO

## 1. Descripción

TicketGO es una plataforma web para la publicación de eventos y la compra, gestión y transferencia de tickets.

El sistema contempla diferentes tipos de usuarios y permisos según su rol.

---

# 2. Requisitos funcionales

## RF-01 — Registro de usuarios

El sistema debe permitir que una persona se registre proporcionando los datos necesarios para crear una cuenta.

## RF-02 — Inicio de sesión

El sistema debe permitir a los usuarios registrados iniciar y cerrar sesión.

## RF-03 — Roles de usuario

El sistema debe manejar diferentes roles de usuario y restringir determinadas funcionalidades según el rol.

Los roles iniciales serán:

- Administrador
- Productor
- Cliente

## RF-04 — Perfil de usuario

El usuario debe poder consultar y modificar determinados datos de su perfil.

El usuario debe poder cargar un avatar.

## RF-05 — Gestión de productores

El sistema debe permitir determinar si un productor se encuentra verificado.

Los productores verificados tendrán permisos diferentes respecto de los productores no verificados.

## RF-06 — Creación de eventos

Los productores autorizados deben poder crear eventos indicando, como mínimo:

- Nombre
- Descripción
- Fecha y hora
- Lugar
- Capacidad
- Precio
- Imagen u otros archivos relacionados
- Estado

## RF-07 — Gestión de eventos

Los productores autorizados deben poder modificar y gestionar sus eventos de acuerdo con las reglas de negocio.

Los administradores deben poder intervenir en los eventos cuando corresponda.

## RF-08 — Estados de eventos

El sistema debe permitir gestionar el estado de un evento.

Los estados contemplados inicialmente son:

- BORRADOR
- EN_REVISION
- PUBLICADO
- CANCELADO
- FINALIZADO

## RF-09 — Publicación de eventos

Los eventos publicados deben ser visibles para los usuarios.

Los eventos que no se encuentren publicados no deben aparecer en el listado público de eventos.

## RF-10 — Revisión de eventos

Los eventos creados por productores no verificados deben poder ser revisados por un administrador antes de su publicación.

## RF-11 — Listado de eventos

El sistema debe permitir consultar los eventos publicados.

El listado debe contar con paginación.

## RF-12 — Búsqueda de eventos

El sistema debe permitir buscar eventos mediante diferentes criterios, como nombre, fecha, lugar o categoría, según corresponda.

## RF-13 — Compra de tickets

Los usuarios autenticados deben poder comprar tickets para eventos publicados que tengan disponibilidad.

## RF-14 — Control de disponibilidad

El sistema debe controlar automáticamente la cantidad de tickets disponibles de cada evento.

La cantidad disponible debe actualizarse después de las compras y devoluciones.

## RF-15 — Límite de compra

El sistema debe impedir que un usuario compre más de 5 tickets para un mismo evento.

## RF-16 — Identificación de tickets

Cada ticket generado por el sistema debe poseer un identificador único.

## RF-17 — Consulta de tickets

Los usuarios deben poder consultar los tickets que poseen.

## RF-18 — Devolución de tickets

Los usuarios deben poder devolver tickets dentro del período establecido por las reglas de negocio.

Los tickets devueltos deben volver a estar disponibles para la venta cuando corresponda.

## RF-19 — Transferencia de tickets

Los usuarios deben poder transferir tickets a otros usuarios registrados.

El sistema debe actualizar el propietario del ticket luego de una transferencia.

## RF-20 — Historial

El sistema debe registrar las operaciones relevantes realizadas sobre tickets y compras.

Entre ellas:

- Compras
- Devoluciones
- Transferencias
- Cambios de estado
- Cancelaciones

## RF-21 — Gestión administrativa

Los administradores deben poder gestionar los recursos que requieran intervención administrativa.

Entre ellos pueden encontrarse:

- Usuarios
- Productores
- Eventos
- Revisiones de eventos

## RF-22 — Archivos

El sistema debe permitir almacenar archivos asociados a los usuarios y eventos.

El avatar del usuario constituye uno de los archivos gestionados por el sistema.

---

# 3. Requisitos de seguridad

## RS-01 — Autenticación

Las funcionalidades que requieran una cuenta deben estar disponibles únicamente para usuarios autenticados.

## RS-02 — Autorización

El acceso a las funcionalidades debe estar restringido según el rol del usuario.

## RS-03 — Protección de información

Las contraseñas no deben almacenarse en texto plano.

## RS-04 — API segura

La API del sistema debe utilizar autenticación mediante JWT para las operaciones que requieran autorización.

---

# 4. Requisitos técnicos

## RT-01 — Framework

La aplicación debe desarrollarse utilizando ASP.NET Core MVC.

## RT-02 — Base de datos

El sistema debe utilizar una base de datos relacional.

## RT-03 — ORM

El acceso a la base de datos se realizará mediante Entity Framework Core.

## RT-04 — Frontend dinámico

Al menos un ABM/CRUD debe implementarse utilizando Vue.js y peticiones AJAX.

## RT-05 — Paginación

Los listados que puedan crecer con el tiempo deben utilizar paginación del lado del servidor.

No se deben recuperar todos los registros para realizar la paginación en el cliente.

## RT-06 — Búsquedas relacionadas

La selección de entidades relacionadas debe realizarse mediante búsquedas AJAX cuando exista una cantidad potencialmente grande de registros.

## RT-07 — API

El sistema debe disponer de una API que pueda ser probada mediante una herramienta como Postman.

## RT-08 — Control de versiones

El código fuente debe mantenerse en un repositorio Git.

El repositorio debe contener el archivo `.gitignore` correspondiente al proyecto.

---

# 5. Documentación

El proyecto debe incluir:

- Diagrama entidad-relación.
- Diagrama de clases.
- Documentación de reglas de negocio.
- README con la descripción del proyecto.
- Documentación de la API.
- Colección de Postman para probar la API.
