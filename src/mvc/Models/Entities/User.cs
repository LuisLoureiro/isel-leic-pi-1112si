using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Entities
{
    public class User
    {
        [Required(ErrorMessage = "O número de docente é obrigatório")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Tem que preencher o username")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tem que preencher a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}