using Entidades.Base;

using System.Linq.Expressions;

namespace Repositorios.Base
{
    public interface IRepositorio<T,K> where T : IBaseEntity<K> 
                                       where K : IEquatable<K>
    {
        T? Obtener(K id);
        IList<T> Obtener(Func<T, bool> consulta);
        IList<T> Obtener();
        K Guardar(T entidad);

    }
}
