using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Entities
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
            _key = key;
        }

        [Display(Order = -1)]
        public K Key
        {
            get { return _key; }
        }
    }
}