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

        // GET: /Usuario/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Usuario/Login
        [HttpPost]
        public IActionResult Login(string nomeUsu, string senhaUsu)
        {
            var usuario = _usuarioRepositorio.ObterUsuario(nomeUsu);

            if (usuario != null && usuario.SenhaUsu == senhaUsu)
            {
                // salva na sessão o id e o nome (usamos ambos)
                HttpContext.Session.SetInt32("CodUsu", usuario.CodUsu);
                HttpContext.Session.SetString("UsuarioLogado", usuario.NomeUsu);

                return RedirectToAction("Menu", "Usuario");
            }

            ModelState.AddModelError("", "Nome ou senha inválidos.");
            return View();
        }

        // GET: /Usuario/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Usuario/Menu
        public IActionResult Menu()
        {
            return View();
        }

        // GET: /Usuario/CadastrarUsuario
        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        // POST: /Usuario/CadastrarUsuario
        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            _usuarioRepositorio.Cadastrar(usuario);

            // opcional: logar automaticamente após cadastro
            HttpContext.Session.SetInt32("CodUsu", usuario.CodUsu);
            HttpContext.Session.SetString("UsuarioLogado", usuario.NomeUsu);

            return RedirectToAction("Menu", "Usuario");
        }
    }
}
