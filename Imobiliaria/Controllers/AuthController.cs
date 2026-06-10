using Imobiliaria.Models;
using Imobiliaria.Services;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliaria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserStore _userStore;

    public AuthController(UserStore userStore)
    {
        _userStore = userStore;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var novoUsuario = await _userStore.RegisterAsync(request);

        if (novoUsuario is null)
        {
            return BadRequest(new { mensagem = "Ja existe um usuario cadastrado com esse e-mail." });
        }

        return Ok(new AuthResponse
        {
            Mensagem = "Cadastro realizado com sucesso!",
            Usuario = new UsuarioPublico(novoUsuario.Id, novoUsuario.Nome, novoUsuario.Email, novoUsuario.Telefone, novoUsuario.Tipo)
        });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var usuario = await _userStore.AuthenticateAsync(request.Email, request.Senha);

        if (usuario is null)
        {
            return Unauthorized(new { mensagem = "E-mail ou senha invalidos." });
        }

        return Ok(new AuthResponse
        {
            Mensagem = "Login realizado com sucesso!",
            Usuario = new UsuarioPublico(usuario.Id, usuario.Nome, usuario.Email, usuario.Telefone, usuario.Tipo)
        });
    }
}
