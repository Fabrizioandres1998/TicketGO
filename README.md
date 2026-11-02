# TicketGO

TicketGO es una aplicación web para publicar y descubrir eventos, comprar y administrar entradas, y transferir tickets entre usuarios. Reúne en un mismo sistema la gestión de productores y eventos, la venta de entradas y el seguimiento de las operaciones.

## Funcionalidades

- Registro, inicio y cierre de sesión; perfiles editables con avatar.
- Roles de **Usuario**, **Productor** y **Administrador**, con permisos según el rol y la verificación del productor.
- Publicación y revisión administrativa de eventos, con imágenes asociadas.
- Compra, consulta, devolución y transferencia de tickets, con control de disponibilidad e historial.
- Gestión de eventos mediante Vue.js y peticiones AJAX a una API JSON.
- Listados paginados en el servidor y búsquedas asíncronas de eventos y usuarios relacionados.
- API REST con autenticación JWT.

## Tecnologías

C#, ASP.NET Core MVC (.NET 8), Entity Framework Core, MySQL, Razor, Vue.js, JavaScript y Bootstrap.

## Requisitos previos

- .NET SDK 8.
- MySQL 8 o compatible.
- Una base de datos llamada `TicketsGO`.

## Puesta en marcha

1. Cloná el repositorio y abrí una terminal en `TicketsGO/`.
2. Creá la base `TicketsGO` e importá el dump `TicketsGO/Data/backup_ticketsgo.sql`. Elegí una opción:

   **Bash (Linux/macOS)**, desde la carpeta `TicketsGO/`:

   ```bash
   mysql -u USUARIO_MYSQL -p -e "CREATE DATABASE TicketsGO CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;"
   mysql -u USUARIO_MYSQL -p TicketsGO < Data/backup_ticketsgo.sql
   ```

   **PowerShell / Windows Terminal**, desde `TicketsGO/`:

   ```powershell
   mysql -u USUARIO_MYSQL -p -e "CREATE DATABASE TicketsGO CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;"
   cmd /c "mysql -u USUARIO_MYSQL -p TicketsGO < Data\backup_ticketsgo.sql"
   ```

   **Símbolo del sistema (CMD)**, desde `TicketsGO/`:

   ```bat
   mysql -u USUARIO_MYSQL -p -e "CREATE DATABASE TicketsGO CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;"
   mysql -u USUARIO_MYSQL -p TicketsGO < Data\backup_ticketsgo.sql
   ```

   Reemplazá `USUARIO_MYSQL` por tu usuario de MySQL e ingresá la contraseña cuando te la pida. Si no se reconoce el comando `mysql`, agregá la carpeta `bin` de MySQL al `PATH` o usá la ruta completa al cliente.

   **MySQL Workbench (entorno gráfico):**

   1. Conectate al servidor local de MySQL.
   2. En **Schemas**, creá un esquema llamado `TicketsGO` y dejalo como predeterminado.
   3. Abrí **Server > Data Import** y elegí **Import from Self-Contained File**.
   4. Seleccioná `TicketsGO/Data/backup_ticketsgo.sql` y establecé `TicketsGO` como **Default Target Schema**.
   5. Hacé clic en **Start Import**. Cuando termine, actualizá la lista de esquemas para ver las tablas.

   El dump incluye el esquema y datos de ejemplo; al importarlo se reemplazan las tablas con el mismo nombre que ya existan en `TicketsGO`. Como alternativa, se pueden crear las tablas con `dotnet ef database update`.

3. Configurá la conexión local a MySQL en `appsettings.json` con la variable de entorno `ConnectionStrings__DefaultConnection`:

   ```bash
   export ConnectionStrings__DefaultConnection='Server=localhost;Port=3306;Database=TicketsGO;User=USUARIO_MYSQL;Password=CONTRASEÑA'
   ```

   En PowerShell:

   ```powershell
   $env:ConnectionStrings__DefaultConnection = "Server=localhost;Port=3306;Database=TicketsGO;User=USUARIO_MYSQL;Password=CONTRASEÑA"
   ```

   En el entorno Development, si no se configura `Jwt__SigningKey`, la aplicación genera una clave aleatoria para ese proceso. Para otros entornos, configurar una clave secreta de al menos 32 bytes mediante `Jwt__SigningKey`.

4. Ejecutá la aplicación:

   ```bash
   dotnet restore
   dotnet run
   ```

   En el perfil HTTPS configurado, la aplicación escucha en `https://localhost:7015` y `http://localhost:5062`.

## Usuarios de prueba

Las cuentas usan la contraseña `123456`:

| Rol | Correo |
| --- | --- |
| Usuario | `usuarioprueba@ticketsgo.com` |
| Usuario | `usuariopruebados@ticketsgo.com` |
| Administrador | `admin@test.com` |
| Productor | `productorprueba@ticketsgo.com` |
| Productor | `productorpruebados@ticketsgo.com` |

Son cuentas para desarrollo y demostración.

> **Nota sobre el dump:** `TicketsGO/Data/backup_ticketsgo.sql` contiene datos de prueba.

## API REST y JWT

La aplicación web utiliza endpoints JSON para las operaciones de gestión de eventos y las búsquedas. Para probar el flujo Bearer, primero solicitá un token:

```http
POST /api/auth/login
Content-Type: application/json
```

```json
{
  "email": "admin@test.com",
  "password": "123456"
}
```

Enviá el valor `accessToken` devuelto en el encabezado `Authorization: Bearer <token>` a las rutas protegidas.

| Método | Ruta | Acceso / uso |
| --- | --- | --- |
| `POST` | `/api/auth/login` | Inicia sesión y devuelve un JWT. |
| `GET` | `/api/auth/me` | Requiere autenticación; devuelve la identidad del usuario. |
| `GET` | `/api/eventos` | Requiere rol Productor o Administrador; admite `pagina` y `busqueda`. |
| `GET` | `/api/eventos/{id}` | Requiere rol Productor o Administrador; consulta un evento propio o, como admin, cualquier evento. |
| `POST` | `/api/eventos` | Crea un evento; recibe datos de formulario y una imagen opcional. |
| `PUT` | `/api/eventos/{id}` | Modifica un evento autorizado; recibe datos de formulario. |
| `DELETE` | `/api/eventos/{id}` | Elimina un evento autorizado. |
| `GET` | `/api/eventos/buscar?nombre=...&pagina=1` | Búsqueda pública de eventos publicados. |
| `GET` | `/api/usuarios/buscar?email=...` | Requiere autenticación; busca usuarios para transferir un ticket. |

## Matriz de evidencias de requisitos

| Requisito | Estado | Implementación / evidencia en el repositorio |
| --- | --- | --- |
| Modelo relacional con al menos 6 tablas y relaciones | ✅ Implementado: 7 entidades principales; relaciones 1:N y tickets asociados a evento y compra | [`AppDbContext`](TicketsGO/Data/AppDbContext.cs), [captura del DER](docs/DER.drawio.png), [modelo de base de datos](docs/base-datos.md) |
| Login, logout, roles y funcionalidad restringida por rol | ✅ Implementado | [`UsuarioController`](TicketsGO/Controllers/UsuarioController.cs), [`CompraController`](TicketsGO/Controllers/CompraController.cs), [`EventoController`](TicketsGO/Controllers/EventoController.cs) |
| Avatar personalizado | ✅ Implementado | [`UsuarioController`](TicketsGO/Controllers/UsuarioController.cs), [`FileService`](TicketsGO/Services/FileService.cs), [captura del perfil con avatar](docs/avatar.png) |
| Carga y almacenamiento de archivos adicionales al avatar | ✅ Implementado: imágenes de eventos en `wwwroot/uploads/eventos` | [`FileService`](TicketsGO/Services/FileService.cs), [`EventosApiController`](TicketsGO/Controllers/EventosApiController.cs), [![Video](https://img.shields.io/badge/▶_Ver_Video-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/_EkKq1TaqFI) |
| ABM/CRUD con Vue.js y AJAX/JSON | ✅ Implementado para la gestión de eventos (listado, alta, edición y baja) | [Vista Vue de gestión](TicketsGO/Views/Evento/GestionVue.cshtml), [`EventosApiController`](TicketsGO/Controllers/EventosApiController.cs), [![Video](https://img.shields.io/badge/▶_Ver_Video-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/YV8XckN_i_A) |
| Paginación procesada en el servidor | ✅ Implementado en listados principales | [`PaginatedResult`](TicketsGO/Services/PaginatedResult.cs), servicios de eventos, usuarios, compras y tickets; ejemplo: [`EventoService`](TicketsGO/Services/EventoService.cs), [![Video](https://img.shields.io/badge/▶_Ver_Video-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/eBdiQGq20g8) |
| Búsqueda asíncrona de entidades relacionadas | ✅ Implementado para eventos en compras y usuarios en transferencias | [formulario de compra](TicketsGO/Views/Compra/Create.cshtml), [`UsuariosApiController`](TicketsGO/Controllers/UsuariosApiController.cs), vistas de transferencia en `TicketsGO/Views/Ticket/`, [![Video](https://img.shields.io/badge/▶_Ver_Video-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/LhFW_2jdOaQ) |
| API REST autenticada con JWT | ✅ Implementado; login emite Bearer token y los endpoints protegidos validan autenticación y rol | [`AuthApiController`](TicketsGO/Controllers/AuthApiController.cs), [`EventosApiController`](TicketsGO/Controllers/EventosApiController.cs), [captura de Postman con Bearer](<docs/Bearer token - evidencia.png>), configuración en [`Program.cs`](TicketsGO/Program.cs) |
| `.gitignore` del proyecto | ✅ Incluido | [`.gitignore`](.gitignore) |
| Colección de Postman | ✅ Incluida | [`TicketsGO.postman_collection.json`](docs/TicketsGO.postman_collection.json) |

### Videos de demostración

- [Video 1](https://youtu.be/_EkKq1TaqFI)
- [Video 2](https://youtu.be/eBdiQGq20g8)
- [Video 3](https://youtu.be/LhFW_2jdOaQ)
- [Video 4](https://youtu.be/YV8XckN_i_A)

Las capturas disponibles están enlazadas en las filas correspondientes de la matriz. Para los demás requisitos, la matriz incluye enlaces directos a las vistas, controladores y servicios donde se puede revisar la implementación.

## Documentación

- [Requisitos funcionales y técnicos](docs/requisitos.md)
- [Reglas de negocio](docs/reglas-negocio.md)
- [Casos de uso](docs/casos-uso.md)
- [Arquitectura](docs/arquitectura.md)
- [Relaciones entre entidades](docs/relaciones-entidades.md)
- [Modelo de base de datos](docs/base-datos.md)
- [Diagrama DER](docs/DER.drawio)
- [Diagrama de clases](docs/Diagrama%20de%20clases.drawio)
- [Dump MySQL con datos de prueba](TicketsGO/Data/backup_ticketsgo.sql)
