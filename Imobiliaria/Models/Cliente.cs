using System.ComponentModel.DataAnnotations;

public class Cliente
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; }

    [Required]
    public string CPF { get; set; }

    public string Telefone { get; set; }

    public string Email { get; set; }

    // Endereço completo (rua, número, bairro, cidade, estado, CEP)
    public string Endereco { get; set; }

    // Tipo do cadastro: "Cliente" ou "Locador"
    public string TipoCliente { get; set; }

    // Tipo de pessoa: "Física" ou "Jurídica" (usado principalmente para locador)
    public string TipoPessoa { get; set; }

    // Caso a pessoa seja jurídica, CNPJ pode ser informado
    public string CNPJ { get; set; }

    // Documento de identidade (RG) quando aplicável
    public string DocumentoIdentidade { get; set; }

    // Dados bancários (opcionais) — podem ser úteis para repasses ao locador
    public string BancoNome { get; set; }
    public string BancoAgencia { get; set; }
    public string BancoConta { get; set; }
}