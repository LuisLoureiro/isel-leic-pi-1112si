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

        protected AbstractEntity(K key)
        {
            Key = key;
        }

        [Display(Order = -1)]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório")]
        public K Key { get; set; }
    }
}