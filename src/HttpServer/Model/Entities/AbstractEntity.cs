using System;

namespace HttpServer.Model.Entities
{
    public abstract class AbstractEntity<K>
    {
        public enum Status
        {
            Accepted = 1,
            Pending,
            Canceled
        } ;

        private readonly K _key;

        protected AbstractEntity(K key)
        {
            if (!(key is ValueType) && key == null)
                throw new ArgumentException("A chave não pode ter valor nulo.");

            _key = key;
        }

        public K Key
        {
            get { return _key; }
        }
    }
}