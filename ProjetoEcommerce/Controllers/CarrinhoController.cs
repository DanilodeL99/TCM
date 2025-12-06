using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Helpers;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Models.DTO;
using System.Linq;

namespace ProjetoEcommerce.Controllers
{
    public class CarrinhoController : Controller
    {
        [HttpGet]
        public IActionResult Listar()
        {
            var carrinho = CarrinhoSessao.Get(HttpContext.Session);
            return Json(carrinho);
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] CarrinhoItem item)
        {
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

            return Ok(new
            {
                sucesso = true,
                total = carrinho.Sum(x => x.Preco * x.Quantidade)
            });
        }

        [HttpPost]
        public IActionResult AtualizarQuantidade([FromBody] AtualizarQuantidadeDTO dto)
        {
            var carrinho = CarrinhoSessao.Get(HttpContext.Session);
            var item = carrinho.FirstOrDefault(x => x.ProdutoId == dto.ProdutoId);

            if (item != null)
            {
                item.Quantidade = dto.Quantidade;

                if (item.Quantidade <= 0)
                    carrinho.Remove(item);

                CarrinhoSessao.Save(HttpContext.Session, carrinho);
            }

            return Ok(new { sucesso = true });
        }

        [HttpPost]
        public IActionResult Remover([FromBody] RemoverItemDTO dto)
        {
            var carrinho = CarrinhoSessao.Get(HttpContext.Session);

            carrinho.RemoveAll(x => x.ProdutoId == dto.ProdutoId);

            CarrinhoSessao.Save(HttpContext.Session, carrinho);

            return Ok(new { sucesso = true });
        }
    }
}
