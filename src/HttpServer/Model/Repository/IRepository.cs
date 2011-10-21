using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;
using HttpServer.Model.Mappers;

namespace HttpServer.Model.Repository
{
    public interface IRepository<K, V> where V : AbstractEntity<K> 
    {
        IEnumerable<V> GetAll();
        V GetById(K key);
        void Insert(IEnumerable<V> values);
        void Insert(V value);
        void Add(IMapper<K, V> mapper);
    }
}