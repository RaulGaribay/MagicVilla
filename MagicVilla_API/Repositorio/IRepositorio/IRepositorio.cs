using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    //La T significa que podemos recibir cualquier tipo de entidad (es genérico)
    public interface IRepositorio<T> where T : class
    {
        Task Crear(T entidad);

        //Si no se envía un filtro, simplemente nos devolverá toda la lista.
        //Si sí se le envía, esta lista se filtrará según la expresión que le enviemos.
        //La expresion es una ExpressionLinq
        Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null);

        Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, string? incluirPropiedades = null);

        Task Remover(T entidad);

        Task Grabar();
    }
}
