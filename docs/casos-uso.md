# Casos de uso — TicketGO

## Actor: Usuario

El usuario representa a una persona registrada en TicketGO. Dependiendo de su estado y rol, puede acceder a diferentes funcionalidades del sistema.

### CU-01 — Registrarse

**Actor:** Usuario

**Descripción:**  
El usuario crea una cuenta en TicketGO proporcionando los datos requeridos por el sistema.

**Resultado:**  
La cuenta del usuario queda registrada.

---

### CU-02 — Iniciar sesión

**Actor:** Usuario

**Descripción:**  
El usuario ingresa sus credenciales para acceder al sistema.

**Resultado:**  
El usuario queda autenticado y puede acceder a las funcionalidades correspondientes a su rol y estado.

---

### CU-03 — Crear evento

**Actor:** Usuario

**Descripción:**  
El usuario crea un nuevo evento proporcionando la información requerida, como nombre, descripción, fecha, ubicación, capacidad y precio.

**Resultado:**  
El evento queda registrado y sujeto al proceso correspondiente según el estado del usuario.

---

### CU-04 — Solicitar aprobación como productor

**Actor:** Usuario

**Descripción:**  
Un usuario solicita ser habilitado como productor para poder gestionar eventos sin necesidad de una revisión individual de cada evento.

**Resultado:**  
La solicitud queda registrada para su revisión por parte de un administrador.

---

### CU-05 — Comprar tickets

**Actor:** Usuario

**Descripción:**  
El usuario selecciona un evento disponible y realiza una compra de uno o más tickets.

**Resultado:**  
La compra queda registrada y los tickets adquiridos quedan asociados al usuario.

---

### CU-06 — Consultar mis tickets

**Actor:** Usuario

**Descripción:**  
El usuario consulta los tickets que posee actualmente.

**Resultado:**  
El sistema muestra los tickets asociados al usuario.

---

### CU-07 — Consultar perfil

**Actor:** Usuario

**Descripción:**  
El usuario accede a la información de su perfil.

**Resultado:**  
El sistema muestra los datos del usuario.

---

### CU-08 — Modificar perfil

**Actor:** Usuario

**Descripción:**  
El usuario modifica los datos permitidos de su perfil.

**Resultado:**  
La información del perfil queda actualizada.

---

### CU-09 — Agregar o modificar foto de perfil

**Actor:** Usuario

**Descripción:**  
El usuario agrega o modifica su foto de perfil.

**Resultado:**  
La nueva imagen queda asociada a su cuenta.

---

### CU-10 — Consultar historial de movimientos

**Actor:** Usuario

**Descripción:**  
El usuario consulta los movimientos registrados relacionados con sus operaciones en el sistema.

**Resultado:**  
El sistema muestra el historial correspondiente al usuario.

---

### CU-11 — Devolver tickets

**Actor:** Usuario

**Descripción:**  
El usuario solicita la devolución de uno o más tickets que posee, siempre que se cumplan las condiciones establecidas por las reglas de negocio.

**Resultado:**  
Los tickets son devueltos y se actualiza su disponibilidad cuando corresponda.

---

### CU-12 — Modificar evento

**Actor:** Usuario / Productor

**Descripción:**  
El usuario modifica uno de sus eventos, siempre que se encuentre dentro de las condiciones permitidas por las reglas de negocio.

**Resultado:**  
La información del evento queda actualizada.

---

### CU-13 — Cancelar evento

**Actor:** Usuario / Productor

**Descripción:**  
El usuario cancela uno de sus eventos, siempre que se cumplan las condiciones establecidas por el sistema.

**Resultado:**  
El evento queda cancelado y se ejecutan las acciones correspondientes sobre sus tickets y operaciones asociadas.

---

# Actor: Productor

El productor es un usuario que fue aprobado y habilitado para gestionar eventos.

### CU-14 — Crear evento como productor

**Actor:** Productor

**Descripción:**  
El productor crea un evento. Al encontrarse habilitado como productor, el evento no requiere una revisión adicional por parte de un administrador.

**Resultado:**  
El evento queda habilitado para continuar con su publicación según el estado correspondiente.

---

# Actor: Administrador

El administrador tiene permisos para gestionar usuarios y supervisar eventos y solicitudes dentro del sistema.

### CU-15 — Aprobar solicitud de productor

**Actor:** Administrador

**Descripción:**  
El administrador revisa una solicitud de un usuario que desea ser habilitado como productor y decide aprobarla o rechazarla.

**Resultado:**  
El usuario queda habilitado como productor o la solicitud queda rechazada.

---

### CU-16 — Aprobar o desaprobar evento

**Actor:** Administrador

**Descripción:**  
El administrador revisa eventos creados por usuarios que no se encuentran habilitados como productores y decide aprobarlos o desaprobarlos.

**Resultado:**  
El evento queda aprobado para su publicación o es rechazado.

---

### CU-17 — Eliminar evento

**Actor:** Administrador

**Descripción:**  
El administrador elimina un evento del sistema cuando corresponda.

**Resultado:**  
El evento queda eliminado y se realizan las acciones necesarias sobre la información asociada.

---

### CU-18 — Suspender usuario

**Actor:** Administrador

**Descripción:**  
El administrador suspende temporalmente a un usuario.

**Resultado:**  
El usuario queda suspendido y no puede utilizar las funcionalidades restringidas por su estado.

---

### CU-19 — Eliminar usuario

**Actor:** Administrador

**Descripción:**  
El administrador elimina un usuario del sistema cuando corresponda.

**Resultado:**  
El usuario queda eliminado, respetando las restricciones y relaciones existentes con sus operaciones y registros históricos.

---

## Resumen de actores

| Actor         | Principales funcionalidades                                                                         |
| ------------- | --------------------------------------------------------------------------------------------------- |
| Usuario       | Registro, autenticación, perfil, compra y devolución de tickets, gestión de sus eventos e historial |
| Productor     | Crear y gestionar eventos sin revisión individual previa                                            |
| Administrador | Aprobar productores, revisar eventos, eliminar eventos y gestionar usuarios                         |
