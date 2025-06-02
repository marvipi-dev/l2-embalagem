using System.ComponentModel.DataAnnotations;

namespace Embalagem.Api.Views;

public class Usuario
{
    [MaxLength(50, ErrorMessage = "O e-mail deve conter no máximo 50 caracteres.")]
    public required string Email { get; set; }

    [MaxLength(128, ErrorMessage = "A senha deve conter no máximo 128 caracteres")]
    public required string Senha { get; set; }
}