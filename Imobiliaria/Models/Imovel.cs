namespace Imobiliaria.Models;

public class Imovel
{
    public int Id { get; set; }
    public int? LocadorId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string ImagemUrl { get; set; } = string.Empty;
}

public class ImovelRequest
{
    public int? LocadorId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string ImagemUrl { get; set; } = string.Empty;
}
