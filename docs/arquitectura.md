# Arquitectura del sistema

## 1. Descripción general

TicketGO está desarrollado utilizando **ASP.NET Core MVC** y sigue una arquitectura por capas, buscando separar las responsabilidades de cada parte de la aplicación.

La aplicación utiliza el patrón **Modelo-Vista-Controlador (MVC)** como estructura principal y agrega una capa de servicios para separar la lógica de negocio de los controladores.

La comunicación general de la aplicación sigue el siguiente flujo:

```text
Usuario
   ↓
Vista
   ↓
Controller
   ↓
Service
   ↓
Data / ORM
   ↓
Base de datos
```

---

## 2. Estructura del proyecto

La estructura principal del proyecto es:

```text
TicketsGO/
├── Controllers/
├── Models/
├── Services/
├── Data/
├── Views/
├── wwwroot/
└── Program.cs
```

### Controllers

Contiene los controladores de ASP.NET Core MVC.

Los controladores reciben las solicitudes HTTP, obtienen los datos enviados por el usuario, llaman a los servicios correspondientes y determinan qué respuesta devolver.

Los controladores no deben concentrar la lógica de negocio principal.

Ejemplos:

- `UsuarioController`
- `EventoController`
- `TicketController`
- `CompraController`
- `AdminController`

---

### Models

Contiene las clases que representan las entidades principales del sistema.

Entre ellas:

- `Usuario`
- `Evento`
- `Ticket`
- `Compra`
- `Historial`
- `Transferencia`

Estas clases representan los datos y relaciones principales del dominio de TicketGO.

---

### Services

Contiene la lógica de negocio de la aplicación.

Los servicios son utilizados por los controladores para realizar operaciones que requieren reglas específicas del sistema.

Algunos ejemplos de responsabilidades:

- Registrar y gestionar usuarios.
- Crear y publicar eventos.
- Validar si un evento puede publicarse.
- Comprar tickets.
- Controlar la cantidad máxima de tickets por usuario.
- Procesar devoluciones.
- Transferir tickets.
- Actualizar el estado de los eventos.
- Registrar acciones en el historial.

De esta forma, los controladores se mantienen más simples y la lógica de negocio queda centralizada.

---

### Data

Contiene los componentes relacionados con el acceso a datos y el ORM.

Se utilizará **Entity Framework Core** como ORM para trabajar con la base de datos.

Dentro de esta capa se encontrará principalmente el `DbContext`, encargado de representar la conexión entre las entidades de la aplicación y las tablas de la base de datos.

También se configurarán aquí las relaciones entre las entidades y las restricciones necesarias.

---

### Views

Contiene las vistas Razor utilizadas por ASP.NET Core MVC.

Las vistas representan la interfaz que se presenta al usuario y reciben los datos enviados por los controladores.

Dentro de determinadas vistas se utilizará **Vue.js** para implementar operaciones AJAX, especialmente el CRUD requerido por la consigna.

Vue.js se utilizará como complemento de MVC y no como reemplazo de toda la arquitectura MVC.

---

### wwwroot

Contiene los archivos estáticos de la aplicación:

- CSS.
- JavaScript.
- Imágenes.
- Bootstrap.
- Otras librerías utilizadas por la interfaz.

---

### Program.cs

Contiene la configuración inicial de la aplicación.

Entre otras cosas, se configurarán:

- ASP.NET Core MVC.
- Entity Framework Core.
- Inyección de dependencias.
- Autenticación.
- Autorización.
- Roles de usuario.
- Rutas de la aplicación.
- Archivos estáticos.

---

## 3. Flujo de una solicitud

Una solicitud típica de TicketGO seguirá el siguiente flujo:

```text
Usuario
   │
   ▼
Vista / navegador
   │
   ▼
Controller
   │
   ▼
Service
   │
   ▼
Entity Framework Core
   │
   ▼
Base de datos
   │
   ▼
Service
   │
   ▼
Controller
   │
   ▼
Vista / respuesta HTTP
```

Por ejemplo, para realizar una compra:

1. El usuario selecciona los tickets desde la interfaz.
2. La solicitud llega al controlador correspondiente.
3. El controlador llama al servicio de compras.
4. El servicio verifica las reglas de negocio.
5. Entity Framework Core realiza las operaciones necesarias sobre la base de datos.
6. El servicio devuelve el resultado al controlador.
7. El controlador devuelve la respuesta correspondiente al usuario.

---

## 4. Autenticación y autorización

El sistema contará con autenticación para identificar a los usuarios y autorización basada en roles.

Los roles definidos para el sistema son:

- `Usuario`
- `Productor`
- `Administrador`

La autorización permitirá restringir determinadas funcionalidades según el rol del usuario.

Por ejemplo:

- Los usuarios autenticados pueden gestionar sus operaciones permitidas.
- Los productores cuentan con funcionalidades relacionadas con la gestión de eventos.
- Los administradores pueden gestionar usuarios y revisar eventos que requieren autorización.

Además, determinadas acciones estarán protegidas mediante las herramientas de autorización proporcionadas por ASP.NET Core.

---

## 5. Separación de responsabilidades

La arquitectura busca mantener separadas las responsabilidades principales:

| Componente  | Responsabilidad                              |
| ----------- | -------------------------------------------- |
| Controllers | Recibir solicitudes y coordinar la respuesta |
| Services    | Aplicar la lógica y reglas de negocio        |
| Models      | Representar las entidades del dominio        |
| Data        | Acceso a datos y configuración del ORM       |
| Views       | Presentar la información al usuario          |
| wwwroot     | Recursos estáticos                           |
| Program.cs  | Configuración de la aplicación               |

Esta separación facilita el mantenimiento del sistema y permite modificar una parte de la aplicación sin afectar directamente a las demás.

---

## 6. Tecnologías principales

El proyecto utiliza o utilizará las siguientes tecnologías:

- **C#**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **MySQL**
- **Razor Views**
- **Vue.js**
- **Bootstrap**
- **JavaScript**
- **HTML/CSS**

La aplicación se ejecutará sobre **.NET 8**.
