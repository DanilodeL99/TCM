using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Helpers;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Controllers
{
    public class CarrinhoController : Controller
    {
        // GET: /Carrinho/Listar
        [HttpGet]
        public IActionResult Listar()
        {
            var carrinho = CarrinhoSessao.Get(HttpContext.Session);
            return Json(carrinho);
        }

        // POST: /Carrinho/Adicionar
        [HttpPost]
        public IActionResult Adicionar([FromBody] CarrinhoItem item)
        {
            // se não logado, retornar 401 (cliente JS deve redirecionar para login)
            var nome = HttpContext.Session.GetString("UsuarioLogado");
            if (string.IsNullOrEmpty(nome))
                return Unauthorized(new { mensagem = "Usuário não autenticado." });

            var carrinho = CarrinhoSessao.Get(HttpContext.Session);
            var existente = carrinho.FirstOrDefault(x => x.ProdutoId == item.ProdutoId);
            if (existente == null)
            {
                item.Quantidade = item.Quantidade <= 0 ? 1 : item.Quantidade;
                carrinho.Add(item);
            }
            else
            {
                existente.Quantidade += item.Quantidade <= 0 ? 1 : item.Quantidade;
            }

            CarrinhoSessao.Save(HttpContext.Session, carrinho);
            return Ok(new { sucesso = true, total = carrinho.Sum(x => x.Preco * x.Quantidade) });
        }

        [HttpPost]
        public IActionResult AtualizarQuantidade([FromBody] CarrinhoItem dto)
        {
            var carrinho = CarrinhoSessao.Get(HttpContext.Session);
            var item = carrinho.FirstOrDefault(x => x.ProdutoId == dto.ProdutoId);
            if (item != null)
            {
                item.Quantidade = dto.Quantidade;
                if (item.Quantidade <= 0) carrinho.Remove(item);
                CarrinhoSessao.Save(HttpContext.Session, carrinho);
            }
            return Ok(new { sucesso = true });
        }

        [HttpPost]
        public IActionResult Remover([FromBody] CarrinhoItem dto)
        {
            var carrinho = CarrinhoSessao.Get(HttpContext.Session);
            carrinho.RemoveAll(x => x.ProdutoId == dto.ProdutoId);
            CarrinhoSessao.Save(HttpContext.Session, carrinho);
            return Ok(new { sucesso = true });
        }
    }
}
