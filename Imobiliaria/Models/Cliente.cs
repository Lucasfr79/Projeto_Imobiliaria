using System.ComponentModel.DataAnnotations;

namespace Imobiliaria.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string CPF { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string TipoCliente { get; set; } = "Cliente";
    public string TipoPessoa { get; set; } = "Fisica";
    public string CNPJ { get; set; } = string.Empty;
    public string DocumentoIdentidade { get; set; } = string.Empty;
    public string BancoNome { get; set; } = string.Empty;
    public string BancoAgencia { get; set; } = string.Empty;
    public string BancoConta { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
