using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;

namespace HttpServer.Model.Mappers
{
    public class AbstractMapper<K, V> : IMapper<K, V> where V : AbstractEntity<K>
    {
        private readonly IDictionary<K, V> _props = new Dictionary<K, V>();

        public IEnumerable<V> GetAll()
        {
            return _props.Values;
        }

        public V GetById(K key)
        {
            return _props[key];
        }

        public void Insert(V value)
        {
            // Se já existir um valor para o mesmo identificador envia excepção.
            if (_props.ContainsKey(value.Key))
                throw new InvalidOperationException(string.Format(
                    "Já existe uma proposta com o mesmo identificador({0}).", value.Key));

            _props[value.Key] = value;
        }
    }
}
