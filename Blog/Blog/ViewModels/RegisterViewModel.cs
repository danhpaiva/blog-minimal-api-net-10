using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O campo name e obrigatorio")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O campo name e obrigatorio")]
    [EmailAddress(ErrorMessage = "O campo email esta em um formato invalido")]
    public string Email { get; set; }
}
