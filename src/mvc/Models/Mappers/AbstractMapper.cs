using System;
using System.Collections.Generic;
using mvc.Models.Entities;

namespace mvc.Models.Mappers
{
    public class AbstractMapper<K, V> : IMapper<K, V> where V : AbstractEntity<K>
    {
        private readonly IDictionary<K, V> _entities = new Dictionary<K, V>();

        public IEnumerable<V> GetAll()
        {
            return _entities.Values;
        }

        public V GetById(K key)
        {
            V value;
            return !_entities.TryGetValue(key, out value) ? null : value;
        }

        public void Insert(V value)
        {
            // Se já existir um valor para o mesmo identificador envia excepção.
            if (_entities.ContainsKey(value.Key))
                throw new InvalidOperationException(string.Format(
                    "Já existe uma entidade com o mesmo identificador({0}).", value.Key));

            _entities[value.Key] = value;
        }

        public void Update(V value)
        {
            // Se não existir um valor para o identificador indicado envia excepção
            if (!_entities.ContainsKey(value.Key))
                throw new InvalidOperationException(string.Format(
                    "Não existe uma entidade com o identificador indicado({0}).", value.Key));

            _entities[value.Key] = value;
        }
    }
}
