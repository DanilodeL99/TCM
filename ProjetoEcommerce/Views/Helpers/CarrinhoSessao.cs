using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProjetoEcommerce.Models;
using System.Collections.Generic;

namespace ProjetoEcommerce.Helpers
{
    public static class CarrinhoSessao
    {
        private const string KEY = "CARRINHO";

        // PEGAR O CARRINHO DA SESSÃO
        public static List<CarrinhoItem> Get(ISession session)
        {
            var data = session.GetString(KEY);
            if (string.IsNullOrEmpty(data))
                return new List<CarrinhoItem>();

            return JsonConvert.DeserializeObject<List<CarrinhoItem>>(data);
        }

        // SALVAR O CARRINHO NA SESSÃO
        public static void Save(ISession session, List<CarrinhoItem> carrinho)
        {
            session.SetString(KEY, JsonConvert.SerializeObject(carrinho));
        }

        // LIMPAR O CARRINHO
        public static void Clear(ISession session)
        {
            session.Remove(KEY);
        }
    }
}
