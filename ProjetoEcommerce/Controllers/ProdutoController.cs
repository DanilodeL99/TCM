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

        // LISTAGEM
        public IActionResult ListarProdutos()
        {
            return View(_produtoRepositorio.TodosProdutos());
        }


        // FORM CADASTRAR
        public IActionResult CadastrarProduto()
        {
            return View();
        }


        // SALVAR PRODUTO - POST
        [HttpPost]
        public IActionResult CadastrarProduto(Produto produto)
        {
            // pega do login
            var codUsuStr = HttpContext.Session.GetString("CodUsu");

            // valida sessão
            if (string.IsNullOrEmpty(codUsuStr))
            {
                TempData["MensagemErro"] = "Sessão expirada. Faça login novamente.";
                return RedirectToAction("Login", "Usuario");
            }

            // conversão segura
            if (!int.TryParse(codUsuStr, out int codUsu))
            {
                TempData["MensagemErro"] = "Erro ao recuperar o usuário da sessão.";
                return RedirectToAction("Login", "Usuario");
            }

            produto.CodUsu = codUsu;

            // salva no banco
            _produtoRepositorio.CadastrarProduto(produto);

            TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";
            return RedirectToAction("Menu", "Usuario");
        }


        // EDITAR PRODUTO (GET)
        public IActionResult EditarProduto(int id)
        {
            var produto = _produtoRepositorio.ObterProduto(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }


        // EDITAR PRODUTO (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarProduto([Bind("Id, Nome, Descricao, Preco, Qtd")] Produto produto)
        {
            if (!ModelState.IsValid)
                return View(produto);

            try
            {
                if (_produtoRepositorio.Atualizar(produto))
                {
                    TempData["MensagemSucesso"] = "Produto atualizado com sucesso!";
                    return RedirectToAction("ListarProdutos");
                }

                TempData["MensagemErro"] = "Erro ao atualizar o produto!";
            }
            catch
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao atualizar o produto.";
            }

            return View(produto);
        }


        // EXCLUI PRODUTO
        public IActionResult ExcluirProduto(int id)
        {
            _produtoRepositorio.Excluir(id);
            TempData["MensagemSucesso"] = "Produto excluído com sucesso!";

            return RedirectToAction("ListarProdutos");
        }
    }
}
