using Imobiliaria.Models;
using Imobiliaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliaria.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class LocatarioApiController : ControllerBase
{
    private readonly ClienteStore _clienteStore;

    public LocatarioApiController(ClienteStore clienteStore)
    {
        _clienteStore = clienteStore;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _clienteStore.GetAllAsync();
        var locatarios = clientes.Where(c => c.TipoCliente == "Locador");
        return Ok(locatarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var clientes = await _clienteStore.GetAllAsync();
        var item = clientes.FirstOrDefault(c => c.Id == id && c.TipoCliente == "Locador");
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost("Cadastrar")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cadastrar([FromBody] Cliente locatario)
    {
        if (locatario == null)
            return BadRequest(new { mensagem = "Payload inválido." });

        if (string.IsNullOrWhiteSpace(locatario.TipoPessoa))
            ModelState.AddModelError(nameof(locatario.TipoPessoa), "Tipo de pessoa é obrigatório.");

        if (locatario.TipoPessoa == "Juridica")
        {
            if (string.IsNullOrWhiteSpace(locatario.CNPJ))
                ModelState.AddModelError(nameof(locatario.CNPJ), "CNPJ é obrigatório para pessoa jurídica.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(locatario.DocumentoIdentidade))
                ModelState.AddModelError(nameof(locatario.DocumentoIdentidade), "Documento (RG) é recomendado para pessoa física.");
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        locatario.TipoCliente = "Locador";
        var salvo = await _clienteStore.AddAsync(locatario);

        return CreatedAtAction(nameof(GetById), new { id = salvo.Id }, salvo);
    }
}
