using System.ComponentModel.DataAnnotations;

namespace WebsupplyHHemo.API.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login é obrigatório")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        public string? Senha { get; set; }
    }
}
