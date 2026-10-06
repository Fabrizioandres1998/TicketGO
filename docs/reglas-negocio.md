# Reglas de negocio

## 1. Usuarios

### RN-01 — Roles de usuario

El sistema contempla diferentes roles de usuario según sus permisos dentro de la plataforma.

Roles principales:

- **Administrador:** gestiona usuarios, productores y eventos que requieran intervención administrativa.
- **Productor:** puede crear y gestionar eventos.
- **Usuario:** puede comprar, devolver y transferir tickets.

### RN-02 — Productor verificado

Un usuario que tenga la condición de **productor verificado** puede publicar automáticamente los eventos que cree, siempre que estos cumplan con las validaciones correspondientes.

### RN-03 — Productor no verificado

Un productor que todavía no se encuentre verificado debe enviar sus eventos a revisión administrativa antes de que puedan ser publicados.

### RN-04 — Estado de los usuarios

Un usuario puede encontrarse activo o deshabilitado.

Un usuario deshabilitado no puede iniciar nuevas operaciones dentro de la plataforma que requieran autenticación.

---

## 2. Eventos

### RN-05 — Fecha del evento

La fecha y hora de un evento no pueden ser anteriores a la fecha y hora actual al momento de su creación.

### RN-06 — Estado del evento

Un evento posee un estado que determina su situación dentro de la plataforma.

Estados posibles:

- **EN_REVISION**
- **PUBLICADO**
- **CANCELADO**
- **FINALIZADO**

Solo los eventos en estado **PUBLICADO** son visibles para los usuarios y permiten la compra de tickets.

### RN-07 — Publicación de eventos

Un evento creado por un productor verificado puede ser publicado automáticamente.

Un evento creado por un productor no verificado debe permanecer en estado **EN_REVISION** hasta que un administrador lo apruebe.

### RN-08 — Capacidad del evento

El productor debe establecer la cantidad máxima de tickets disponibles para cada evento.

La cantidad de tickets vendidos nunca puede superar la capacidad establecida para el evento.

### RN-09 — Disponibilidad de tickets

La cantidad de tickets disponibles se obtiene a partir de la capacidad total del evento menos la cantidad de tickets vendidos.

Los tickets disponibles deben actualizarse automáticamente después de cada compra.

### RN-10 — Modificación del evento

Un evento puede ser modificado por su productor hasta **24 horas antes** de su fecha y hora de inicio.

Una vez alcanzado ese límite, el evento no puede ser modificado por el productor.

### RN-11 — Cancelación del evento

Un evento puede ser cancelado por su productor o por un administrador según los permisos correspondientes.

Un evento cancelado no permite nuevas compras.

### RN-12 — Eventos finalizados

Una vez alcanzada la fecha y hora de finalización del evento, este pasa a estado **FINALIZADO**.

Un evento finalizado no permite nuevas compras, devoluciones ni transferencias de tickets.

---

## 3. Compras

### RN-13 — Usuario autenticado

Solo los usuarios autenticados pueden realizar compras de tickets.

### RN-14 — Límite de compra

Un usuario puede comprar como máximo **5 tickets por evento**.

El límite se aplica considerando los tickets que el usuario tenga asociados para ese evento.

### RN-15 — Disponibilidad

No se puede completar una compra si la cantidad solicitada supera la cantidad de tickets disponibles.

### RN-16 — Compra de eventos publicados

Solo se pueden realizar compras de eventos que se encuentren en estado **PUBLICADO**.

### RN-17 — Registro de compra

Cada compra debe quedar registrada y asociada al usuario que realizó la operación.

La compra debe conservar información suficiente para conocer qué tickets fueron adquiridos, cuándo se realizó la compra y cuál fue el importe correspondiente.

---

## 4. Tickets

### RN-18 — Identificación única

Cada ticket debe poseer un identificador único dentro del sistema.

No pueden existir dos tickets con el mismo identificador.

### RN-19 — Asignación del ticket

Cada ticket pertenece inicialmente al usuario que realizó la compra.

### RN-20 — Devolución

Un usuario puede devolver un ticket hasta **48 horas antes** del comienzo del evento.

Una vez superado ese límite, el ticket no puede ser devuelto.

### RN-21 — Disponibilidad después de una devolución

Cuando un ticket es devuelto dentro del período permitido, vuelve a estar disponible para su venta.

La cantidad de tickets disponibles debe actualizarse automáticamente.

### RN-22 — Transferencia de tickets

Un usuario puede transferir un ticket a otro usuario registrado en la plataforma.

Una vez realizada la transferencia, el ticket deja de pertenecer al usuario original y pasa a pertenecer al nuevo usuario.

### RN-23 — Transferencia de tickets próximos al evento

Un ticket puede ser transferido hasta el momento que se establezca como límite para la transferencia.

Por defecto, se establece que las transferencias son posibles hasta **24 horas antes** del evento.

### RN-24 — Un único propietario

Un ticket puede pertenecer a un único usuario en un momento determinado.

Una transferencia reemplaza al propietario anterior por el nuevo propietario.

---

## 5. Historial

### RN-25 — Registro de operaciones

Las operaciones relevantes realizadas sobre tickets y compras deben quedar registradas en el historial.

Entre las operaciones que pueden registrarse se encuentran:

- Compra.
- Devolución.
- Transferencia.
- Cambio de estado.
- Cancelación.

### RN-26 — Trazabilidad

El historial debe permitir identificar qué operación se realizó, sobre qué ticket o compra, qué usuario la realizó y cuándo ocurrió.

Los registros del historial no deben eliminarse como consecuencia de una devolución o transferencia.

---

## 6. Reglas generales

### RN-27 — Integridad de los tickets

El sistema debe garantizar que un ticket no pueda venderse dos veces ni pertenecer simultáneamente a dos usuarios.

### RN-28 — Consistencia de disponibilidad

La cantidad de tickets disponibles debe mantenerse consistente con la capacidad del evento y las operaciones realizadas sobre los tickets.

### RN-29 — Operaciones sobre eventos no publicados

Los usuarios no pueden comprar tickets de eventos que se encuentren en estado BORRADOR, EN_REVISION, CANCELADO o FINALIZADO.

### RN-30 — Operaciones sobre eventos inexistentes

No se pueden realizar compras, devoluciones o transferencias sobre tickets o eventos inexistentes.

### RN-31 — Control de permisos

Las operaciones administrativas y de gestión de eventos deben estar restringidas según el rol y los permisos del usuario autenticado.
