using Imobiliaria.Models;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliaria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinanciamentoController : ControllerBase
{
    [HttpPost("simular")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Simular([FromBody] FinanciamentoRequest request)
    {
        var cpf = ApenasNumeros(request.Cpf);

        if (cpf.Length != 11 || request.ValorImovel <= 0 || request.RendaMensal <= 0)
        {
            return BadRequest(new { mensagem = "Informe CPF com 11 digitos, valor do imovel e renda mensal." });
        }

        var parcelas = request.Parcelas <= 0 ? 360 : request.Parcelas;
        var entrada = Math.Max(0, request.Entrada);
        var valorFinanciado = Math.Max(0, request.ValorImovel - entrada);
        var jurosMensal = 0.009m;
        var fator = (decimal)Math.Pow((double)(1 + jurosMensal), parcelas);
        var parcela = valorFinanciado * (jurosMensal * fator) / (fator - 1);
        var rendaMinima = parcela / 0.35m;
        var entradaSuficiente = entrada >= request.ValorImovel * 0.1m;
        var cpfScoreSimulado = int.Parse(cpf[^2..]) >= 25;
        var rendaSuficiente = request.RendaMensal >= rendaMinima;
        var aprovado = entradaSuficiente && cpfScoreSimulado && rendaSuficiente;

        return Ok(new FinanciamentoResponse
        {
            Aprovado = aprovado,
            Mensagem = aprovado
                ? "Financiamento pre-aprovado na simulacao."
                : "Financiamento nao aprovado na simulacao. Revise entrada, renda ou CPF informado.",
            ValorFinanciado = Math.Round(valorFinanciado, 2),
            ParcelaEstimada = Math.Round(parcela, 2),
            RendaMinima = Math.Round(rendaMinima, 2)
        });
    }

    private static string ApenasNumeros(string texto)
    {
        return new string((texto ?? string.Empty).Where(char.IsDigit).ToArray());
    }
}
