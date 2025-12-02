using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;
using Microsoft.AspNetCore.Http;

namespace ProjetoEcommerce.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        // -----------------------------
        // EXIBE A TELA DE LOGIN
        // -----------------------------
        public IActionResult Login()
        {
            return View();
        }

        // -----------------------------
        // PROCESSA LOGIN
        // -----------------------------
        [HttpPost]
        public IActionResult Login(string nomeUsu, string senhaUsu)
        {
            var usuario = _usuarioRepositorio.ObterUsuario(nomeUsu);

            if (usuario != null && usuario.SenhaUsu == senhaUsu)
            {
                // 🔥 SALVA O ID DO USUÁRIO NA SESSÃO
                HttpContext.Session.SetString("CodUsu", usuario.CodUsu.ToString());

                return RedirectToAction("Menu", "Usuario");
            }

            ModelState.AddModelError("", "Nome ou senha inválidos.");
            return View();
        }

        // -----------------------------
        // MENU DO USUÁRIO LOGADO
        // -----------------------------
        public IActionResult Menu()
        {
            return View();
        }

        // -----------------------------
        // CADASTRAR USUÁRIO (GET)
        // -----------------------------
        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        // -----------------------------
        // CADASTRAR USUÁRIO (POST)
        // -----------------------------
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
