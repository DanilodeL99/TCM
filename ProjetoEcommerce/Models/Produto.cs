using MySqlX.XDevAPI;

namespace ProjetoEcommerce.Models
{
    public class Produto
    {
        public int CodProd { get; set; }
        public string NomeProd { get; set; }
        public string DescProd { get; set; }
        public decimal PrecoProd { get; set; }
        public int CodUsu { get; set; }
    }
}


