using Imobiliaria.Models;
using Imobiliaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliaria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImovelController : ControllerBase
{
    private readonly ImovelStore _imovelStore;

    public ImovelController(ImovelStore imovelStore)
    {
        _imovelStore = imovelStore;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Listar()
    {
        return Ok(_imovelStore.Listar());
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Cadastrar([FromBody] ImovelRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Titulo) ||
            string.IsNullOrWhiteSpace(request.Localizacao) ||
            string.IsNullOrWhiteSpace(request.Descricao) ||
            request.Preco <= 0)
        {
            return BadRequest(new { mensagem = "Preencha titulo, localizacao, descricao e preco valido." });
        }

        var imovel = _imovelStore.Adicionar(request);

        return Ok(new
        {
            mensagem = "Imovel cadastrado com sucesso!",
            imovel
        });
    }
}
