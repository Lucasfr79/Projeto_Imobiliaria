using System.Text.Json;
using Imobiliaria.Models;

namespace Imobiliaria.Services;

public class UserStore
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public UserStore(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "usuarios.json");
    }

    public async Task<IReadOnlyList<Usuario>> GetAllAsync()
    {
        var usuarios = await ReadAsync();
        return usuarios.OrderBy(usuario => usuario.Nome).ToList();
    }

    public async Task<Usuario?> RegisterAsync(RegisterRequest request)
    {
        await _semaphore.WaitAsync();

        try
        {
            var email = request.Email.Trim();
            var senha = request.Senha.Trim();
            var tipo = NormalizarTipo(request.Tipo);
            var usuarios = await ReadWithoutLockAsync();

            if (usuarios.Any(usuario => usuario.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var usuario = new Usuario
            {
                Id = usuarios.Count == 0 ? 1 : usuarios.Max(usuario => usuario.Id) + 1,
                Nome = request.Nome.Trim(),
                Email = email,
                Senha = senha,
                Telefone = request.Telefone?.Trim() ?? string.Empty,
                Tipo = tipo,
                CriadoEm = DateTime.UtcNow
            };

            usuarios.Add(usuario);
            await WriteWithoutLockAsync(usuarios);

            return usuario;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Usuario?> AuthenticateAsync(string email, string senha)
    {
        var usuarios = await ReadAsync();
        var normalizedEmail = email.Trim();
        var normalizedSenha = senha.Trim();

        return usuarios.FirstOrDefault(usuario =>
            usuario.Email.Trim().Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase) &&
            usuario.Senha.Trim() == normalizedSenha);
    }

    public async Task<IReadOnlyList<int>?> GetFavoritosAsync(int usuarioId)
    {
        var usuarios = await ReadAsync();
        var usuario = usuarios.FirstOrDefault(item => item.Id == usuarioId);
        return usuario?.ImoveisFavoritos.OrderBy(id => id).ToList();
    }

    public async Task<IReadOnlyList<int>?> AdicionarFavoritoAsync(int usuarioId, int imovelId)
    {
        await _semaphore.WaitAsync();

        try
        {
            var usuarios = await ReadWithoutLockAsync();
            var usuario = usuarios.FirstOrDefault(item => item.Id == usuarioId);

            if (usuario is null)
            {
                return null;
            }

            if (!usuario.ImoveisFavoritos.Contains(imovelId))
            {
                usuario.ImoveisFavoritos.Add(imovelId);
                await WriteWithoutLockAsync(usuarios);
            }

            return usuario.ImoveisFavoritos.OrderBy(id => id).ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IReadOnlyList<int>?> RemoverFavoritoAsync(int usuarioId, int imovelId)
    {
        await _semaphore.WaitAsync();

        try
        {
            var usuarios = await ReadWithoutLockAsync();
            var usuario = usuarios.FirstOrDefault(item => item.Id == usuarioId);

            if (usuario is null)
            {
                return null;
            }

            if (usuario.ImoveisFavoritos.Remove(imovelId))
            {
                await WriteWithoutLockAsync(usuarios);
            }

            return usuario.ImoveisFavoritos.OrderBy(id => id).ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static string NormalizarTipo(string? tipo)
    {
        return tipo?.Trim().Equals("Locador", StringComparison.OrdinalIgnoreCase) == true
            ? "Locador"
            : "Cliente";
    }

    private async Task<List<Usuario>> ReadAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            return await ReadWithoutLockAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<List<Usuario>> ReadWithoutLockAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Usuario>();
        }

        await using var stream = File.OpenRead(_filePath);
        var usuarios = await JsonSerializer.DeserializeAsync<List<Usuario>>(stream);
        return usuarios ?? new List<Usuario>();
    }

    private async Task WriteWithoutLockAsync(List<Usuario> usuarios)
    {
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, usuarios, _jsonOptions);
    }
}
