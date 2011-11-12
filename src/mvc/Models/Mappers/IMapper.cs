using System.Collections.Generic;
using mvc.Models.Entities;

namespace mvc.Models.Mappers
{
    public interface IMapper<K, V> where V : AbstractEntity<K>
    {
        IEnumerable<V> GetAll();
        V GetById(K key);

        void Insert(V value);
        void Update(V value);
    }
}