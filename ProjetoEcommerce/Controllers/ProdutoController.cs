using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;

namespace ProjetoEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        /* Declara uma variável privada somente leitura do tipo ProdutoRepositorio chamada _produtoRepositorio.
         O "readonly" indica que o valor desta variável só pode ser atribuído no construtor da classe.
         ProdutoRepositorio é uma classe do repositorio responsável por interagir com a camada de dados para gerenciar informações de usuários.*/
        private readonly ProdutoRepositorio _produtoRepositorio;

        /* Define o construtor da classe LoginController.
        Recebe uma instância de UsuarioRepositorio como parâmetro (injeção de dependência)*/
        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            /* O construtor é chamado quando uma nova instância de LoginController é criada.*/
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult ListarProdutos()
        {
            /* Retorna a View padrão associada a esta Action,
             passando como modelo a lista de todos os produtos obtida do repositório.*/
            return View(_produtoRepositorio.TodosProdutos());
        }


        /* Action para exibir o formulário de cadastro de produto (via Requisição GET)*/
        public IActionResult CadastrarProduto()
        {
            //retorna a Página
            return View();
        }

        // Action que recebe e processa os dados que serão enviados pelo formulário de cadastro de produtos (via Requisição POST)
        [HttpPost]
        public IActionResult CadastrarProduto(Produto produto)
        {

            /* O parâmetro 'produto' recebe os dados enviados pelo formulário,
             que são automaticamente mapeados para as propriedades da classe Produto.
             Chama o método no repositório para cadastrar o novo produto no sistema.*/
            _produtoRepositorio.CadastrarProduto(produto);

            //redireciona para pagina Index 'Menu/Usuario' garante que o nome da Action seja usado corretamente,
            return RedirectToAction("Menu", "Usuario");
        }

        /* Action para exibir o formulário de edição de um produto específico (via Requisição GET)
         Este método recebe o 'id' do produto a ser editado como parâmetro.*/
        public IActionResult EditarProduto(int id)
        {
            // Obtém o produto específico do repositório usando o ID fornecido.
            var produto = _produtoRepositorio.ObterProduto(id);

            // Verifica se o produto foi encontrado. É uma boa prática tratar casos onde o ID é inválido.
            if (produto == null)
            {
                // Você pode retornar um NotFound (código de status 404) ou outra resposta apropriada.
                return NotFound();
            }

            // Retorna a View associada a esta Action (EditarProduto.cshtml),
            return View(produto);
        }


        // Carrega a lista de Produtos que envia a alteração(post)

        [HttpPost]
        [ValidateAntiForgeryToken] // Essencial para segurança contra ataques CSRF
        /*[Bind] para especificar explicitamente quais propriedades do objeto Produto podem ser vinculadas a partir dos dados do formulário.
        Isso é uma boa prática de segurança para evitar o overposting (onde um usuário malicioso pode enviar dados para propriedades
        que você não pretendia que fossem alteradas)*/
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
                else
                {
                    TempData["MensagemErro"] = "Erro ao atualizar o produto!";
                }
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao atualizar.";
            }

            return View(produto);
        }


        public IActionResult ExcluirProduto(int id)
        {
            // Obtém o produto específico do repositório usando o Codigo fornecido.
            _produtoRepositorio.Excluir(id);
            // Retorna a View de confirmação de exclusão, passando o produto como modelo.
            return RedirectToAction("ListarProdutos", "Produto");
        }
    }
}
