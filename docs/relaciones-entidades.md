# Relaciones entre entidades

Este documento describe las relaciones entre las principales entidades del sistema TicketGO y su cardinalidad.

## Usuario - Evento

Un usuario puede tener uno o más eventos asociados.

La relación permite identificar al usuario que crea y administra cada evento.

**Cardinalidad:**

```text
Usuario 1 ─── N Evento
```

Cada evento pertenece a un usuario.

---

## Evento - Ticket

Un evento puede tener uno o más tickets disponibles.

Cada ticket pertenece a un único evento.

**Cardinalidad:**

```text
Evento 1 ─── N Ticket
```

La cantidad de tickets disponibles está determinada por la capacidad establecida para el evento y se actualiza a medida que se realizan compras o devoluciones.

---

## Usuario - Ticket

Un usuario puede tener uno o más tickets.

Cada ticket tiene un único usuario como propietario actual.

**Cardinalidad:**

```text
Usuario 1 ─── N Ticket
```

Esta relación permite identificar quién posee actualmente cada ticket y contempla también la transferencia de tickets entre usuarios.

---

## Usuario - Compra

Un usuario puede realizar una o más compras.

Cada compra pertenece a un único usuario.

**Cardinalidad:**

```text
Usuario 1 ─── N Compra
```

La relación permite identificar qué usuario realizó cada compra.

---

## Evento - Compra

Un evento puede tener una o más compras asociadas.

Cada compra corresponde a un evento determinado.

**Cardinalidad:**

```text
Evento 1 ─── N Compra
```

Esta relación permite determinar a qué evento corresponden los tickets adquiridos mediante una compra.

---

## Evento - Historial

Un evento puede tener muchos movimientos registrados en el historial.

Cada movimiento de historial relacionado con un evento permite registrar acciones o cambios importantes sobre este.

**Cardinalidad:**

```text
Evento 1 ─── N Historial
```

El historial permite mantener la trazabilidad de las operaciones realizadas sobre los eventos.

---

## Usuario - Historial

Un usuario puede tener muchos movimientos registrados en el historial.

Cada movimiento permite identificar al usuario relacionado con la operación registrada.

**Cardinalidad:**

```text
Usuario 1 ─── N Historial
```

Esta relación permite mantener la trazabilidad de las acciones realizadas por los usuarios.

---

## Resumen de relaciones

| Entidad origen | Entidad destino | Cardinalidad |
| -------------- | --------------- | ------------ |
| Usuario        | Evento          | 1:N          |
| Evento         | Ticket          | 1:N          |
| Usuario        | Ticket          | 1:N          |
| Usuario        | Compra          | 1:N          |
| Evento         | Compra          | 1:N          |
| Evento         | Historial       | 1:N          |
| Usuario        | Historial       | 1:N          |
