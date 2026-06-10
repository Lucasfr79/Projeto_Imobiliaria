using System.ComponentModel.DataAnnotations;

namespace Imobiliaria.Models;

public class RegisterRequest
{
    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(4)]
    public string Senha { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Tipo { get; set; } = "Cliente";
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Mensagem { get; set; } = string.Empty;
    public UsuarioPublico? Usuario { get; set; }
}

public record UsuarioPublico(int Id, string Nome, string Email, string Telefone, string Tipo);
