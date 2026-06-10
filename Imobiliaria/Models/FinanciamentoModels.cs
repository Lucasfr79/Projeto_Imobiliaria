namespace Imobiliaria.Models;

public class FinanciamentoRequest
{
    public string Cpf { get; set; } = string.Empty;
    public decimal ValorImovel { get; set; }
    public decimal Entrada { get; set; }
    public decimal RendaMensal { get; set; }
    public int Parcelas { get; set; } = 360;
}

public class FinanciamentoResponse
{
    public bool Aprovado { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public decimal ValorFinanciado { get; set; }
    public decimal ParcelaEstimada { get; set; }
    public decimal RendaMinima { get; set; }
}
