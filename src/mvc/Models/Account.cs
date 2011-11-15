using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace mvc.Models
{
    public class InternalUser : AccountUser
    {
        [HiddenInput(DisplayValue = false)]
        public bool IsActivated { get; set; }
    }

    public class AccountUser : RegisterUser
    {
        [Display(Name = "Fotografia")]
        public Image Foto { get; set; }

        [HiddenInput(DisplayValue = true)]
        public override int Number { get; set; }
    }
    
    public class RegisterUser : DefaultUser
    {   
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação", Order = int.MaxValue)]
        [Compare("Password", ErrorMessage = "As passwords não são iguais.")]
        public string ConfirmPassword { get; set; }

        public void ChangePassword(string newPassword)
        {
            ConfirmPassword = Password = newPassword;
        }
    }

    public class DefaultUser
    {
        [Required(ErrorMessage = "O número de docente é obrigatório.")]
        [Display(Name = "Número Docente")]
        [Range(1, 99999, ErrorMessage = "O número de docente tem no máximo cinco algarismos.")]
        public virtual int Number { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome")]
        [StringLength(100, ErrorMessage = "Nome demasiado extenso. Dimensão máxima de 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [Display(Name = "Endereço de correio electrónico")]
        [RegularExpression(@"[\w\.-]*[a-zA-Z0-9_]@[\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]", ErrorMessage = "O email indicado não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tem que preencher a password.")]
        [StringLength(50, ErrorMessage = "A password tem de ter no minimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class LogOn
    {
        [Required(ErrorMessage = "Tem que preencher o nome de utilizador")]
        [Display(Name = "Utilizador")]
        public int Username { get; set; }

        [Required(ErrorMessage = "Tem que introduzir a password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Display(Name = "Lembrar-me?")]
        //public bool RememberMe { get; set; }
    }
}