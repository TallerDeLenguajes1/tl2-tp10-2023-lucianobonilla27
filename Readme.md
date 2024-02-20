# Kanban

## Descripción
Este proyecto es una aplicación de gestión de tareas que ayuda a los usuarios a organizar sus responsabilidades diarias. Ofrece una interfaz intuitiva para crear, editar y cambiar el estado de las tareas.

## Recursos Utilizados
- **Lenguaje de Programación:** C#
- **Framework:** ASP.NET
- **Base de Datos:** SQLite
- **Herramientas de Desarrollo:** VS Code

## Configuración y Uso
El proyecto tiene dos tipos de usuarios: operador y administrador. Por el momento solo un usuario administrador puede crear/editar/eliminar usuarios, la aplicacion ya trae creado un usuario administrador de prueba, se puede acceder con el nombre de usuario "admin" y contraseña "admin". Un administrador ademas puede ver todos los tableros y tareas del sistema, a diferencia de un usuario operador que solo puede observar sus tableros o tableros donde tenga tareas asignadas y sus tareas creadas o tareas asignadas. Un usuario operador solo puede editar/eliminar tareas o tableros solo si es propietario de ellos. Si no es propietario de un tablero no tendrá acciones para realizar y si no es propietario de una tarea, es decir que solo tiene asignada esa tarea, la unica accion que puede realizar es cambiar el estado de esa tarea, los estados son: ToDo, Doing, Review y Done.


## Estructura del Proyecto
El proyecto sigue una estructura organizativa que facilita el desarrollo, mantenimiento y comprensión del código. A continuación se presenta una descripción general de la estructura del proyecto:

1. Directorio Raíz:
/Controllers: Contiene controladores de ASP.NET MVC que gestionan las solicitudes HTTP y las respuestas.

/Models: Almacena los modelos de datos que representan las entidades del sistema.

/Views: Contiene las vistas de la aplicación. Se organiza siguiendo la estructura MVC (Modelo-Vista-Controlador).

/DB: Contiene archivos de acceso a datos, como contextos de bases de datos.

/Repository: Contiene archivos del manejo de datos de la aplicacion.

/Models: Contiene los modelos de la aplicación.

/ViewModels: Contiene los modelos que se mandan a la vista de la aplicacion.

/wwwroot: Incluye recursos estáticos accesibles públicamente, como archivos CSS, JavaScript e imágenes.


2. Configuración:
/appsettings.json: Archivo de configuración principal que almacena configuraciones de la aplicación.

/Program.cs: Configuración del inicio de la aplicación, servicios y middleware.





