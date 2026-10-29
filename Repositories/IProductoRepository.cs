using ProductosADO.Api.Models;

namespace ProductosADO.Api.Repositories
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto? ObtenerPorId(int id);
        List<Producto> Buscar(string termino);
        int Insertar(Producto producto);
        bool Actualizar(Producto producto);
        bool Eliminar(int id);
    }
}