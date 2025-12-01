namespace ProjetoEcommerce.Models
{
    public class Usuario
    {
        public int CodUsu { get; set; }
        public string NomeUsu { get; set; }   // para login
        public string SenhaUsu { get; set; }  // melhor usar string mesmo que seja numérico
    }

}

