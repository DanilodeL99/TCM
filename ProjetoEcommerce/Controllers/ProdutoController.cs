using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;

namespace ProjetoEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult ListarProdutos()
        {
            return View(_produtoRepositorio.TodosProdutos());
        }

        public IActionResult CadastrarProduto()
        {
            // só exibe o formulário
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProduto([Bind("NomeProd,DescProd,PrecoProd")] Produto produto)
        {
            // valida sessão
            var codUsuStr = HttpContext.Session.GetString("CodUsu");
            if (string.IsNullOrEmpty(codUsuStr))
            {
                TempData["MensagemErro"] = "Sessão expirada. Faça login para cadastrar produtos.";
                return RedirectToAction("Login", "Usuario");
            }

            produto.CodUsu = int.Parse(codUsuStr);

            // cadastra e obtém id
            var novoId = _produtoRepositorio.CadastrarProduto(produto);
            TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";
            return RedirectToAction("ListarProdutos", "Produto");
        }

        public IActionResult EditarProduto(int id)
        {
            var produto = _produtoRepositorio.ObterProduto(id);
            if (produto == null) return NotFound();
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarProduto([Bind("CodProd,NomeProd,DescProd,PrecoProd")] Produto produto)
        {
            var codUsuStr = HttpContext.Session.GetString("CodUsu");
            if (!string.IsNullOrEmpty(codUsuStr))
                produto.CodUsu = int.Parse(codUsuStr);

            if (_produtoRepositorio.Atualizar(produto))
            {
                TempData["MensagemSucesso"] = "Produto atualizado!";
                return RedirectToAction("ListarProdutos");
            }

            TempData["MensagemErro"] = "Erro ao atualizar.";
            return View(produto);
        }

        public IActionResult ExcluirProduto(int id)
        {
            _produtoRepositorio.Excluir(id);
            return RedirectToAction("ListarProdutos");
        }
    }
}
