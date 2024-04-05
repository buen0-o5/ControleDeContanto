using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeContator.Models
{
    public class RedefinirSenhaModel
    {
        [Required(ErrorMessage = "Informe o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe seu e-mail")]
        public string Email { get; set; }
    }
}
