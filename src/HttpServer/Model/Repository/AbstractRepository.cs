using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer.Model.Entities;
using HttpServer.Model.Mappers;

namespace HttpServer.Model.Repository
{
    public abstract class AbstractRepository<K, V> : IRepository<K, V> where V : AbstractEntity<K>
    {
        private readonly Type _type = typeof (V);
        private readonly IDictionary<Type, IMapper<K, V>> _mappers = new Dictionary<Type, IMapper<K, V>>();

        public IEnumerable<V> GetAll()
        {
            return _mappers[_type].GetAll();
        }

        public V GetById(K key)
        {
            return _mappers[_type].GetById(key);
        }

        public void Insert(IEnumerable<V> values)
        {
            // Verificar se existe mapper para o tipo de dados do enumerável.
            IMapper<K, V> mapper;
            if (!_mappers.TryGetValue(typeof(V), out mapper))
                throw new InvalidOperationException(string.Format(
                    "Não existe nenhum mapper para o tipo indicado({0}).", typeof(V)));

            foreach (V value in values)
            {
                mapper.Insert(value);
            }
        }

        public void Insert(V value)
        {
            // Verificar se existe mapper para o tipo de dados do enumerável.
            IMapper<K, V> mapper;
            if (!_mappers.TryGetValue(typeof(V), out mapper))
                throw new InvalidOperationException(string.Format(
                    "Não existe nenhum mapper para o tipo indicado({0}).", typeof(V)));

            mapper.Insert(value);
        }

        public void Add(IMapper<K, V> mapper)
        {
            // Se já existir um Mapper para este tipo retorna excepção;
            IMapper<K, V> exists;
            if (_mappers.TryGetValue(_type, out exists))
                throw new InvalidOperationException(string.Format(
                    "Não é possível substituir o mapper existente para o tipo indicado({0}).", _type));

            _mappers[_type] = mapper;
        }
    }
}
