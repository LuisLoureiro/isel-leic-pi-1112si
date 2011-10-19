using System.Collections.Generic;
using HttpServer.Model.Entities;

namespace HttpServer.Model.Mappers
{
    public interface IMapper<K, V> where V : AbstractEntity<K>
    {
        IEnumerable<V> GetAll();
        V GetById(K key);

        void Insert(V value);
    }
}