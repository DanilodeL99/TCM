namespace ProjetoEcommerce.Models.DTO
{
    public class AtualizarQuantidadeDTO
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }

    public class RemoverItemDTO
    {
        public int ProdutoId { get; set; }
    }
}
