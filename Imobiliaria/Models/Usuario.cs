namespace Imobiliaria.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Tipo { get; set; } = "Cliente";
    public List<int> ImoveisFavoritos { get; set; } = new();
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
