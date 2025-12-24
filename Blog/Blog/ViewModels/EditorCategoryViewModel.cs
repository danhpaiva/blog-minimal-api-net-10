using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "O nome deve ser passado obrigatoriamente.")]
    [StringLength(100, ErrorMessage = "Este campo deve ter no min 3 e no máximo 100 caracteres.", MinimumLength = 3)]
    public string Name { get; set; }
    [Required(ErrorMessage = "O slug deve ser passado obrigatoriamente.")]
    public string Slug { get; set; }
}
