using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;

namespace ProjetoEcommerce.Controllers
{

    // Criando a classe LoginController e herdando  da classe controller
    public class UsuarioController : Controller
    {
        /* Declara uma variável privada somente leitura do tipo UsuarioRepositorio chamada _usuarioRepositorio.
        O "readonly" indica que o valor desta variável só pode ser atribuído no construtor da classe.
        UsuarioRepositorio é uma classe do repositorio responsável por interagir com a camada de dados para gerenciar informações de usuários.*/
        private readonly UsuarioRepositorio _usuarioRepositorio;

        /*Define o construtor da classe LoginController. 
         Recebe uma instância de UsuarioRepositorio como parâmetro (injeção de dependência).*/
        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            // O construtor é chamado quando uma nova instância de LoginController é criada.
            _usuarioRepositorio = usuarioRepositorio;
        }


        /* Define uma action (método público) chamada Login que retorna um IActionResult.
         IActionResult é uma interface que representa o resultado de uma action.*/
        public IActionResult Login()
        {
            // retorna a página Login
            return View();
        }

        /* Define outra action chamada Login, mas desta vez ela responde a requisições HTTP POST ([HttpPost]).
        que recebe dois parâmetros do formulário enviado: email e senha (ambos do tipo string).*/

        public IActionResult Menu()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string nomeUsu, string senhaUsu)
        {
            var usuario = _usuarioRepositorio.ObterUsuario(nomeUsu);
            if (usuario != null && usuario.SenhaUsu.ToString() == senhaUsu)
            {
                return RedirectToAction("Menu", "Usuario");
            }
            ModelState.AddModelError("", "Nome ou senha inválidos.");
            return View();
        }



        //CHAMA O METODO CADASTRAR USUARIO
        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _usuarioRepositorio.Cadastrar(usuario);
                return RedirectToAction("Login");
            }

            return View(usuario);
        }
    }
}
