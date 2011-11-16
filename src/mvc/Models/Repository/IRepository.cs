using System.Collections.Generic;
using mvc.Models.Entities;
using mvc.Models.Mappers;

namespace mvc.Models.Repository
{
    public interface IRepository<K, V> where V : AbstractEntity<K> 
    {
        IEnumerable<V> GetAll();
        V GetById(K key);
        void Insert(IEnumerable<V> values);
        void Insert(V value);
        void Update(V value);
        void Add(IMapper<K, V> mapper);
    }
}