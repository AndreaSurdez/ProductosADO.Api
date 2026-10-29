using MySqlConnector;
using ProductosADO.Api.Models;

namespace ProductosADO.Api.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string _connectionString;

        // Este es el patrón de Inyección de Dependencias. 
        // No hardcodeamos la conexión, la leemos de Program.cs / appsettings.json
        public ProductoRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        // CONEXION + CONSULTA GENERAL
        // Aquí se aplica el flujo de ADO.NET: Abrir conexión -> Crear comando -> Ejecutar Reader -> Mapear -> Cerrar conexión
        public List<Producto> ObtenerTodos()
        {
            var lista = new List<Producto>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Productos";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapearProducto(reader));
                    }
                }
            }
            return lista;
        }

        public Producto? ObtenerPorId(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Productos WHERE Id = @Id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Siempre usar parámetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return MapearProducto(reader);
                    }
                }
            }
            return null;
        }

        public List<Producto> Buscar(string termino)
        {
            var lista = new List<Producto>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Productos WHERE Nombre LIKE @term OR Descripcion LIKE @term";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@term", $"%{termino}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) lista.Add(MapearProducto(reader));
                    }
                }
            }
            return lista;
        }

        // INSERT - Usamos ExecuteScalar para obtener el ID generado
        public int Insertar(Producto p)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = @"INSERT INTO Productos (Nombre, Descripcion, Precio, Stock) 
                              VALUES (@n, @d, @p, @s); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", p.Nombre);
                    cmd.Parameters.AddWithValue("@d", p.Descripcion);
                    cmd.Parameters.AddWithValue("@p", p.Precio);
                    cmd.Parameters.AddWithValue("@s", p.Stock);
                    var result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        // ACTUALIZAR - ExecuteNonQuery retorna filas afectadas
        public bool Actualizar(Producto p)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "UPDATE Productos SET Nombre=@n, Descripcion=@d, Precio=@p, Stock=@s WHERE Id=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", p.Nombre);
                    cmd.Parameters.AddWithValue("@d", p.Descripcion);
                    cmd.Parameters.AddWithValue("@p", p.Precio);
                    cmd.Parameters.AddWithValue("@s", p.Stock);
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ELIMINAR
        public bool Eliminar(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "DELETE FROM Productos WHERE Id=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Helper para no repetir código de mapeo
        private Producto MapearProducto(MySqlDataReader r)
        {
            return new Producto
            {
                Id = r.GetInt32("Id"),
                Nombre = r.GetString("Nombre"),
                Descripcion = r.IsDBNull(r.GetOrdinal("Descripcion")) ? null : r.GetString("Descripcion"),
                Precio = r.GetDecimal("Precio"),
                Stock = r.GetInt32("Stock"),
                FechaCreacion = r.GetDateTime("FechaCreacion")
            };
        }
    }
}