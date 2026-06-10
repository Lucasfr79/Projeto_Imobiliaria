using Imobiliaria.Models;
using Imobiliaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliaria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ClienteStore _clienteStore;

    public ClienteController(ClienteStore clienteStore)
    {
        _clienteStore = clienteStore;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var clientes = await _clienteStore.GetAllAsync();
        return Ok(clientes);
    }

    [HttpPost("Cadastrar")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cadastrar([FromBody] Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var clienteSalvo = await _clienteStore.AddAsync(cliente);

        return Ok(new
        {
            mensagem = "Cliente cadastrado com sucesso!",
            cliente = clienteSalvo
        });
    }
}
