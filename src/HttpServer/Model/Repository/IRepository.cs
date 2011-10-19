using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;
using HttpServer.Model.Mappers;

namespace HttpServer.Model.Repository
{
    public interface IRepository
    {
        void Add<K, V>(Type type, IMapper<K, V> mapper) where V : AbstractEntity<K>;
    }

    public interface IRepository<K, V> : IRepository where V : AbstractEntity<K>
    {
        IEnumerable<V> GetAll(Type type);
        V GetById(Type type, K key);
        void Insert(IEnumerable<V> values);
        void Insert(V value);
        void Add(Type type, IMapper<K, V> mapper);
    }
}