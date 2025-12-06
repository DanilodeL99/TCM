using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProjetoEcommerce.Models;
using System.Collections.Generic;

namespace ProjetoEcommerce.Helpers
{
    public static class CarrinhoSessao
    {
        private const string CHAVE = "CARRINHO";

        public static List<CarrinhoItem> Get(ISession session)
        {
            var dado = session.GetString(CHAVE);

            if (string.IsNullOrEmpty(dado))
                return new List<CarrinhoItem>();

            return JsonConvert.DeserializeObject<List<CarrinhoItem>>(dado);
        }

        public static void Save(ISession session, List<CarrinhoItem> carrinho)
        {
            var json = JsonConvert.SerializeObject(carrinho);
            session.SetString(CHAVE, json);
        }

        public static void Clear(ISession session)
        {
            session.Remove(CHAVE);
        }
    }
}
