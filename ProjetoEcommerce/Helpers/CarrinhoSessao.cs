using Microsoft.AspNetCore.Http;
using System.Text.Json;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Helpers
{
    public static class CarrinhoSessao
    {
        private const string KEY = "CARRINHO";

        public static List<CarrinhoItem> Get(ISession session)
        {
            var s = session.GetString(KEY);
            if (string.IsNullOrEmpty(s)) return new List<CarrinhoItem>();
            return JsonSerializer.Deserialize<List<CarrinhoItem>>(s) ?? new List<CarrinhoItem>();
        }

        public static void Save(ISession session, List<CarrinhoItem> itens)
        {
            var s = JsonSerializer.Serialize(itens);
            session.SetString(KEY, s);
        }
    }
}
