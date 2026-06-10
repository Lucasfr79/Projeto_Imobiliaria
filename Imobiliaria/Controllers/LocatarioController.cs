using Microsoft.AspNetCore.Mvc;
using Imobiliaria.Models;
using System.Linq;

public class LocatarioController : Controller
{
    // GET: /Locatario/Cadastrar
    public IActionResult Cadastrar()
    {
        // Renderiza a view com um modelo vazio
        var model = new Cliente();
        model.TipoCliente = "Locador"; // marca como locador
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(Cliente locatario)
    {
        // Regras adicionais para locatário
        if (string.IsNullOrEmpty(locatario.TipoPessoa))
        {
            ModelState.AddModelError(nameof(locatario.TipoPessoa), "Tipo de pessoa é obrigatório.");
        }

        // Se for jurídica, CNPJ é obrigatório
        if (locatario.TipoPessoa == "Juridica")
        {
            if (string.IsNullOrWhiteSpace(locatario.CNPJ))
            {
                ModelState.AddModelError(nameof(locatario.CNPJ), "CNPJ é obrigatório para pessoa jurídica.");
            }
        }
        else
        {
            // pessoa física: DocumentoIdentidade (RG) pode ser obrigatório dependendo da regra
            if (string.IsNullOrWhiteSpace(locatario.DocumentoIdentidade))
            {
                ModelState.AddModelError(nameof(locatario.DocumentoIdentidade), "Documento (RG) é recomendado para pessoa física.");
            }
        }

        // Mantém requisitos básicos do modelo (Nome/CPF)
        if (ModelState.IsValid)
        {
            // TODO: salvar no banco de dados
            TempData["SuccessMessage"] = "Locatário cadastrado com sucesso!";
            return RedirectToAction(nameof(Cadastrar));
        }

        // Se houver erros, retorna a view com mensagens para correção
        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
        TempData["ErrorMessage"] = string.Join(" | ", errors);
        return View(locatario);
    }
}
