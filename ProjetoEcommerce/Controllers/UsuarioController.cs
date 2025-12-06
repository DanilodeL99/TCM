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

        // =====================================================
        // GET /Usuario/Login
        // =====================================================
        public IActionResult Login()
        {
            return View();
        }

        // =====================================================
        // POST /Usuario/Login
        // =====================================================
        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            var usuarioBanco = _usuarioRepositorio.Login(usuario.NomeUsu, usuario.SenhaUsu);

            if (usuarioBanco == null)
            {
                TempData["MensagemErro"] = "Usuário ou senha inválidos.";
                return View(usuario);
            }

            // SALVA SESSÃO
            HttpContext.Session.SetInt32("CodUsu", usuarioBanco.CodUsu);
            HttpContext.Session.SetString("NomeUsu", usuarioBanco.NomeUsu);

            return RedirectToAction("Index", "Home");
        }

        // =====================================================
        // LOGOUT
        // =====================================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // MENU DO USUÁRIO
       
        public IActionResult Menu()
        {
            if (HttpContext.Session.GetInt32("CodUsu") == null)
                return RedirectToAction("Login", "Usuario");

            return View();
        }

     
        // TELA DE CADASTRO DE USUÁRIO
       
        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        // =====================================================
        // POST /Usuario/CadastrarUsuario
        // =====================================================
        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            bool criado = _usuarioRepositorio.Cadastrar(usuario);

            if (!criado)
            {
                TempData["MensagemErro"] = "Erro ao cadastrar usuário.";
                return View(usuario);
            }

            // Login automático
            HttpContext.Session.SetInt32("CodUsu", usuario.CodUsu);
            HttpContext.Session.SetString("NomeUsu", usuario.NomeUsu);

            return RedirectToAction("Menu", "Usuario");
        }
    }
}
