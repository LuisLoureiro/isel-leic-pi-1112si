using System.Collections.Generic;

namespace HttpServer.Model
{
    interface IRepository<K, T>
    {
        IEnumerable<T> GetAll();
        T GetById(K key);
    }
}
