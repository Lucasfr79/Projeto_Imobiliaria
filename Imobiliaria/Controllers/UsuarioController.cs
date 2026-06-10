using Imobiliaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliaria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UserStore _userStore;

    public UsuarioController(UserStore userStore)
    {
        _userStore = userStore;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var usuarios = await _userStore.GetAllAsync();

        var resposta = usuarios.Select(usuario => new
        {
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.Telefone,
            usuario.Tipo,
            usuario.ImoveisFavoritos,
            usuario.CriadoEm
        });

        return Ok(resposta);
    }

    [HttpGet("{usuarioId:int}/favoritos")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFavoritos(int usuarioId)
    {
        var favoritos = await _userStore.GetFavoritosAsync(usuarioId);

        if (favoritos is null)
        {
            return NotFound(new { mensagem = "Usuario nao encontrado." });
        }

        return Ok(new { favoritos });
    }

    [HttpPost("{usuarioId:int}/favoritos/{imovelId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdicionarFavorito(int usuarioId, int imovelId)
    {
        var favoritos = await _userStore.AdicionarFavoritoAsync(usuarioId, imovelId);

        if (favoritos is null)
        {
            return NotFound(new { mensagem = "Usuario nao encontrado." });
        }

        return Ok(new { mensagem = "Imovel salvo nos favoritos.", favoritos });
    }

    [HttpDelete("{usuarioId:int}/favoritos/{imovelId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverFavorito(int usuarioId, int imovelId)
    {
        var favoritos = await _userStore.RemoverFavoritoAsync(usuarioId, imovelId);

        if (favoritos is null)
        {
            return NotFound(new { mensagem = "Usuario nao encontrado." });
        }

        return Ok(new { mensagem = "Imovel removido dos favoritos.", favoritos });
    }
}
