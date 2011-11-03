using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Tem que preencher o username")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tem que preencher a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}