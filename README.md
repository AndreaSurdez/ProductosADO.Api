# Práctica ADO.NET - Módulo de Productos

**Alumna:** Andrea Mariana Surdez Espeleta
**Materia:** Programación Móvil - Marco Ulises Padilla Galván
**Tecnología:** .NET 8 Web API + MySQL Workbench + ADO.NET + Swagger

### Flujo del Proyecto - Conceptos de Teoría

El proyecto está dividido en capas, que es parte de la Arquitectura por Capas:

1.  **Capa de Conexión (appsettings.json / Program.cs):** Aquí está la cadena de conexión. Es el punto central. Usamos Inyección de Dependencias para no hardcodear la conexión en todos lados. En Program.cs registramos `AddScoped<IProductoRepository, ProductoRepository>` que es el principio de Inversión de Dependencias (SOLID).

2.  **Capa de Modelo (Models/Producto.cs):** Es la entidad que representa la tabla. Es el DTO que viaja entre capas.

3.  **Capa de Contrato (IProductoRepository):** Es la Interface. Define QUÉ debe hacer el módulo pero no CÓMO. Esto permite cambiar de SQL a MySQL sin romper el Controller. Es Abstracción.

4.  **Capa de Datos / Repository (ProductoRepository.cs):** Aquí implementamos ADO.NET puro. Usamos `MySqlConnection`, `MySqlCommand` y `MySqlDataReader`. 
   - `using` asegura que la conexión se cierre siempre.
   - `Parameters.AddWithValue` evita SQL Injection.
   - `ExecuteReader` para Consultas, `ExecuteScalar` para Insert y obtener ID, `ExecuteNonQuery` para Actualizar y Eliminar.
   - Esto es el Patrón Repository, centraliza todo el acceso a datos.

5.  **Capa de Presentación (ProductosController.cs):** Recibe el Request de Swagger y solo le habla a la Interface, nunca a la BD directamente. Es el principio de Responsabilidad Única.

### Puntos de la Práctica Resueltos

- **Conexión:** MySqlConnector + GetConnection() desde IConfiguration
- **Consultas:** ObtenerTodos() y ObtenerPorId() con SELECT *
- **Insert:** Insertar() con INSERT + LAST_INSERT_ID()
- **Actualizar:** Actualizar() con UPDATE
- **Eliminar:** Eliminar() con DELETE
- **Buscar:** Buscar(termino) con LIKE %termino% para búsqueda por nombre

### Prueba con Swagger
Correr `dotnet run` y abrir https://localhost:XXXX/swagger para probar los 6 endpoints.