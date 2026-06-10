using System.Text.Json;
using Imobiliaria.Models;

namespace Imobiliaria.Services;

public class ClienteStore
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public ClienteStore(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "clientes.json");
    }

    public async Task<IReadOnlyList<Cliente>> GetAllAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            if (!File.Exists(_filePath))
            {
                return new List<Cliente>();
            }

            await using var stream = File.OpenRead(_filePath);
            var clientes = await JsonSerializer.DeserializeAsync<List<Cliente>>(stream);
            return clientes ?? new List<Cliente>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Cliente> AddAsync(Cliente cliente)
    {
        await _semaphore.WaitAsync();

        try
        {
            List<Cliente> clientes;

            if (File.Exists(_filePath))
            {
                await using var readStream = File.OpenRead(_filePath);
                clientes = await JsonSerializer.DeserializeAsync<List<Cliente>>(readStream) ?? new List<Cliente>();
            }
            else
            {
                clientes = new List<Cliente>();
            }

            cliente.Id = clientes.Count == 0 ? 1 : clientes.Max(item => item.Id) + 1;
            cliente.CriadoEm = DateTime.UtcNow;
            clientes.Add(cliente);

            await using var writeStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(writeStream, clientes, _jsonOptions);

            return cliente;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
