using Microsoft.AspNetCore.Mvc;

public class ClienteController : Controller
{
    public IActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Cadastrar(Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            return Content("Cliente cadastrado com sucesso!");
        }

        return View(cliente);
    }
}