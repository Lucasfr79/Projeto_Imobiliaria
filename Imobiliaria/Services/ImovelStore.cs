using System.Text.Json;
using Imobiliaria.Models;

namespace Imobiliaria.Services;

public class ImovelStore
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly List<Imovel> _imoveis =
    [
        new Imovel
        {
            Id = 1,
            Titulo = "Casa Moderna",
            Localizacao = "Zona 07 - Maringa/PR",
            Descricao = "3 quartos, 2 banheiros, garagem dupla.",
            Preco = 550000m,
            ImagemUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 2,
            Titulo = "Apartamento Central",
            Localizacao = "Centro - Maringa/PR",
            Descricao = "2 quartos, sacada gourmet e vaga coberta.",
            Preco = 320000m,
            ImagemUrl = "https://images.unsplash.com/photo-1494526585095-c41746248156?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 3,
            Titulo = "Terreno Espacoso",
            Localizacao = "Jardim Alvorada - Maringa/PR",
            Descricao = "500m2 em bairro tranquilo e com facil acesso.",
            Preco = 210000m,
            ImagemUrl = "https://images.unsplash.com/photo-1600566753376-12c8ab7fb75b?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 4,
            Titulo = "Sobrado Premium",
            Localizacao = "Zona 02 - Maringa/PR",
            Descricao = "4 quartos, area gourmet e piscina.",
            Preco = 890000m,
            ImagemUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 5,
            Titulo = "Casa com Jardim",
            Localizacao = "Jardim Alvorada - Maringa/PR",
            Descricao = "3 quartos, quintal amplo e varanda.",
            Preco = 470000m,
            ImagemUrl = "https://images.unsplash.com/photo-1600585154526-990dced4db0d?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 6,
            Titulo = "Apartamento Iluminado",
            Localizacao = "Novo Centro - Maringa/PR",
            Descricao = "2 quartos, sala integrada e sacada.",
            Preco = 345000m,
            ImagemUrl = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 7,
            Titulo = "Residencia Contemporanea",
            Localizacao = "Zona 05 - Maringa/PR",
            Descricao = "4 quartos, suite e acabamento premium.",
            Preco = 760000m,
            ImagemUrl = "https://images.unsplash.com/photo-1600566753376-12c8ab7fb75b?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 8,
            Titulo = "Casa com Area Gourmet",
            Localizacao = "Jardim Italia - Maringa/PR",
            Descricao = "3 quartos, churrasqueira e espaco externo.",
            Preco = 590000m,
            ImagemUrl = "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 9,
            Titulo = "Cozinha Planejada",
            Localizacao = "Centro - Maringa/PR",
            Descricao = "Apartamento com projeto moderno e funcional.",
            Preco = 410000m,
            ImagemUrl = "https://images.unsplash.com/photo-1600573472591-ee6b68d14c68?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 10,
            Titulo = "Fachada Elegante",
            Localizacao = "Zona 08 - Maringa/PR",
            Descricao = "Imovel com varanda, jardim e garagem.",
            Preco = 640000m,
            ImagemUrl = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 11,
            Titulo = "Suite Confortavel",
            Localizacao = "Jardim Paulista - Maringa/PR",
            Descricao = "Dormitorios aconchegantes e excelente iluminacao.",
            Preco = 380000m,
            ImagemUrl = "https://images.unsplash.com/photo-1605146769289-440113cc3d00?auto=format&fit=crop&w=1200&q=80"
        },
        new Imovel
        {
            Id = 12,
            Titulo = "Casa Familiar",
            Localizacao = "Zona 03 - Maringa/PR",
            Descricao = "Amplo espaco interno para toda a familia.",
            Preco = 525000m,
            ImagemUrl = "https://images.unsplash.com/photo-1560185007-cde436f6a4d0?auto=format&fit=crop&w=1200&q=80"
        }
    ];

    public ImovelStore(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "imoveis.json");

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var imoveisSalvos = JsonSerializer.Deserialize<List<Imovel>>(json);

            if (imoveisSalvos is not null && imoveisSalvos.Count > 0)
            {
                _imoveis.Clear();
                _imoveis.AddRange(imoveisSalvos);
            }
        }
        else
        {
            Salvar();
        }
    }

    public IReadOnlyList<Imovel> Listar()
    {
        lock (_lock)
        {
            return _imoveis.ToList();
        }
    }

    public Imovel Adicionar(ImovelRequest request)
    {
        lock (_lock)
        {
            var imovel = new Imovel
            {
                Id = _imoveis.Count == 0 ? 1 : _imoveis.Max(item => item.Id) + 1,
                LocadorId = request.LocadorId,
                Titulo = request.Titulo.Trim(),
                Localizacao = request.Localizacao.Trim(),
                Descricao = request.Descricao.Trim(),
                Preco = request.Preco,
                ImagemUrl = string.IsNullOrWhiteSpace(request.ImagemUrl)
                    ? "https://images.unsplash.com/photo-1560518883-ce09059eeffa?auto=format&fit=crop&w=1200&q=80"
                    : request.ImagemUrl.Trim()
            };

            _imoveis.Add(imovel);
            Salvar();

            return imovel;
        }
    }

    private void Salvar()
    {
        var json = JsonSerializer.Serialize(_imoveis, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
