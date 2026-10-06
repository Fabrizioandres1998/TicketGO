## Usuario

| Campo                  | Tipo         | PK  | FK  | Nulo |
| ---------------------- | ------------ | --- | --- | ---- |
| id_usuario             | int          | Sí  | —   | No   |
| nombre                 | varchar(100) | —   | —   | No   |
| apellido               | varchar(100) | —   | —   | No   |
| email                  | varchar(150) | —   | —   | No   |
| password               | varchar(255) | —   | —   | No   |
| rol                    | varchar(30)  | —   | —   | No   |
| estado                 | boolean      | —   | —   | No   |
| telefono               | varchar(30)  | —   | —   | Sí   |
| email_verificado       | boolean      | —   | —   | No   |
| verification_token     | varchar(255) | —   | —   | Sí   |
| reset_password_expires | datetime     | —   | —   | Sí   |

## Evento

| Campo               | Tipo         | PK  | FK  | Nulo |
| ------------------- | ------------ | --- | --- | ---- |
| id_evento           | int          | Sí  | —   | No   |
| id_usuario          | int          | —   | Sí  | No   |
| nombre              | varchar(150) | —   | —   | No   |
| descripcion         | text         | —   | —   | No   |
| fecha_inicio        | datetime     | —   | —   | No   |
| fecha_fin           | datetime     | —   | —   | No   |
| autorizado          | boolean      | —   | —   | No   |
| provincia           | varchar(100) | —   | —   | No   |
| ciudad              | varchar(100) | —   | —   | No   |
| establecimiento     | varchar(150) | —   | —   | No   |
| privado             | boolean      | —   | —   | No   |
| tickets_totales     | int          | —   | —   | No   |
| tickets_disponibles | int          | —   | —   | No   |
| tickets_vendidos    | int          | —   | —   | No   |
| estado              | varchar(30)  | —   | —   | No   |

## Ticket

| Campo      | Tipo         | PK  | FK  | Nulo |
| ---------- | ------------ | --- | --- | ---- |
| id_ticket  | int          | Sí  | —   | No   |
| id_evento  | int          | —   | Sí  | No   |
| id_usuario | int          | —   | Sí  | Sí   |
| estado     | varchar(30)  | —   | —   | No   |
| codigo     | varchar(100) | —   | —   | No   |

## Compra

| Campo         | Tipo          | PK  | FK  | Nulo |
| ------------- | ------------- | --- | --- | ---- |
| id_compra     | int           | Sí  | —   | No   |
| id_evento     | int           | —   | Sí  | No   |
| id_usuario    | int           | —   | Sí  | No   |
| total_tickets | int           | —   | —   | No   |
| monto_total   | decimal(10,2) | —   | —   | No   |
| estado        | varchar(30)   | —   | —   | No   |

## Historial

| Campo        | Tipo | PK  | FK  | Nulo |
| ------------ | ---- | --- | --- | ---- |
| id_historial | int  | Sí  | —   | No   |
| id_evento    | int  | —   | Sí  | No   |
| id_usuario   | int  | —   | Sí  | No   |
| descripcion  | text | —   | —   | No   |
