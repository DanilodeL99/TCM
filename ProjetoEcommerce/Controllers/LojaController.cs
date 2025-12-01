using Microsoft.AspNetCore.Mvc;

namespace ProjetoEcommerce.Controllers
{
    public class LojaController : Controller
    {
        public IActionResult CatalogoProdutos()
        {
            return View();
        }
    }
}
