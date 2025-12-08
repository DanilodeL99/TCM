using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;

namespace ProjetoEcommerce.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string NomeUsu, string SenhaUsu)
        {
            if (string.IsNullOrWhiteSpace(NomeUsu) || string.IsNullOrWhiteSpace(SenhaUsu))
            {
                TempData["MensagemErro"] = "Preencha usuário e senha.";
                return View();
            }

            var usuarioBanco = _usuarioRepositorio.Login(NomeUsu, SenhaUsu);
            if (usuarioBanco == null)
            {
                TempData["MensagemErro"] = "Usuário ou senha inválidos.";
                return View();
            }

            HttpContext.Session.SetString("UsuarioLogado", usuarioBanco.NomeUsu);
            HttpContext.Session.SetString("CodUsu", usuarioBanco.CodUsu.ToString());

            // redirecionar ao Home conforme pedido
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Menu() => View();

        public IActionResult CadastrarUsuario() => View();

        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            var novoId = _usuarioRepositorio.Cadastrar(usuario);

            HttpContext.Session.SetString("UsuarioLogado", usuario.NomeUsu);
            HttpContext.Session.SetString("CodUsu", novoId.ToString());

            // após cadastro, voltar ao Home (ou ao menu se preferir)
            return RedirectToAction("Index", "Home");
        }
    }
}
