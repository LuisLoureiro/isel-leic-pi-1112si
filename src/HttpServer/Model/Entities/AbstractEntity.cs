namespace HttpServer.Model.Entities
{
    public abstract class AbstractEntity<K>
    {
        private readonly K _key;

        protected AbstractEntity(K key)
        {
            _key = key;
        }

        public K Key
        {
            get { return _key; }
        }
    }
}