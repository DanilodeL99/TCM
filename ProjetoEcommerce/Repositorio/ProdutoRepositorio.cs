using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio
    {
        private readonly string _conexaoMySQL;

        public ProdutoRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        // Cadastrar produto (retorna id)
        public int CadastrarProduto(Produto produto)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            string sql = @"
                INSERT INTO tbProduto (NomeProd, DescProd, PrecoProd, CodUsu)
                VALUES (@nome, @desc, @preco, @codusu);
            ";

            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nome", produto.NomeProd);
            cmd.Parameters.AddWithValue("@desc", produto.DescProd);
            cmd.Parameters.AddWithValue("@preco", produto.PrecoProd);
            cmd.Parameters.AddWithValue("@codusu", produto.CodUsu);

            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        public bool Atualizar(Produto produto)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            string sql = @"
                UPDATE tbProduto SET
                    NomeProd = @nome,
                    DescProd = @desc,
                    PrecoProd = @preco,
                    CodUsu = @codusu
                WHERE CodProd = @id;
            ";

            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@id", produto.CodProd);
            cmd.Parameters.AddWithValue("@nome", produto.NomeProd);
            cmd.Parameters.AddWithValue("@desc", produto.DescProd);
            cmd.Parameters.AddWithValue("@preco", produto.PrecoProd);
            cmd.Parameters.AddWithValue("@codusu", produto.CodUsu);

            return cmd.ExecuteNonQuery() > 0;
        }

        public IEnumerable<Produto> TodosProdutos()
        {
            var lista = new List<Produto>();
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("SELECT CodProd, NomeProd, DescProd, PrecoProd, CodUsu FROM tbProduto", conexao);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Produto
                {
                    CodProd = Convert.ToInt32(dr["CodProd"]),
                    NomeProd = dr["NomeProd"].ToString(),
                    DescProd = dr["DescProd"].ToString(),
                    PrecoProd = Convert.ToDecimal(dr["PrecoProd"]),
                    CodUsu = Convert.ToInt32(dr["CodUsu"])
                });
            }
            return lista;
        }

        public Produto ObterProduto(int id)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("SELECT CodProd, NomeProd, DescProd, PrecoProd, CodUsu FROM tbProduto WHERE CodProd = @id", conexao);
            cmd.Parameters.AddWithValue("@id", id);
            using var dr = cmd.ExecuteReader();
            if (!dr.Read()) return null;

            return new Produto
            {
                CodProd = Convert.ToInt32(dr["CodProd"]),
                NomeProd = dr["NomeProd"].ToString(),
                DescProd = dr["DescProd"].ToString(),
                PrecoProd = Convert.ToDecimal(dr["PrecoProd"]),
                CodUsu = Convert.ToInt32(dr["CodUsu"])
            };
        }

        public void Excluir(int id)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var cmd = new MySqlCommand("DELETE FROM tbProduto WHERE CodProd = @id", conexao);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
