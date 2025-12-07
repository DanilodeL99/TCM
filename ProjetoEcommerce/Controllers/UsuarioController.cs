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

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
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

            // grava sessão
            HttpContext.Session.SetString("UsuarioLogado", usuarioBanco.NomeUsu);
            HttpContext.Session.SetString("CodUsu", usuarioBanco.CodUsu.ToString());

            // redireciona para menu do usuario (onde está o CRUD link "Minhas Configurações")
            return RedirectToAction("Menu", "Usuario");
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Menu do usuário (após login)
        public IActionResult Menu()
        {
            return View();
        }

        // GET: Cadastrar
        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        // POST: Cadastrar
        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            var novoId = _usuarioRepositorio.Cadastrar(usuario);

            // opcional: logar automático
            HttpContext.Session.SetString("UsuarioLogado", usuario.NomeUsu);
            HttpContext.Session.SetString("CodUsu", novoId.ToString());

            return RedirectToAction("Menu", "Usuario");
        }
    }
}
