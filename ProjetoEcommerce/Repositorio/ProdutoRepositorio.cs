using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;

namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio
    {
        private readonly string _conexaoMySQL;

        public ProdutoRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        // =============================
        // CADASTRAR PRODUTO
        // =============================
        public void CadastrarProduto(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                string sql = @"INSERT INTO tbProduto 
                               (CodProd, NomeProd, DescProd, PrecoProd, CodUsu)
                               VALUES (@codprod, @nome, @desc, @preco, @codusu)";

                MySqlCommand cmd = new MySqlCommand(sql, conexao);

                cmd.Parameters.AddWithValue("@codprod", produto.CodProd);
                cmd.Parameters.AddWithValue("@nome", produto.NomeProd);
                cmd.Parameters.AddWithValue("@desc", produto.DescProd);
                cmd.Parameters.AddWithValue("@preco", produto.PrecoProd);
                cmd.Parameters.AddWithValue("@codusu", produto.CodUsu);

                cmd.ExecuteNonQuery();
            }
        }

        // =============================
        // ATUALIZAR PRODUTO
        // =============================
        public bool Atualizar(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                string sql = @"UPDATE tbProduto 
                               SET NomeProd = @nome,
                                   DescProd = @desc,
                                   PrecoProd = @preco,
                                   CodUsu = @codusu
                               WHERE CodProd = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conexao);

                cmd.Parameters.AddWithValue("@id", produto.CodProd);
                cmd.Parameters.AddWithValue("@nome", produto.NomeProd);
                cmd.Parameters.AddWithValue("@desc", produto.DescProd);
                cmd.Parameters.AddWithValue("@preco", produto.PrecoProd);
                cmd.Parameters.AddWithValue("@codusu", produto.CodUsu);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // =============================
        // LISTAR TODOS OS PRODUTOS
        // =============================
        public IEnumerable<Produto> TodosProdutos()
        {
            var lista = new List<Produto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tbProduto", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
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
            }

            return lista;
        }

        // =============================
        // OBTER PRODUTO ESPECÍFICO
        // =============================
        public Produto ObterProduto(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM tbProduto WHERE CodProd = @id", conexao);

                cmd.Parameters.AddWithValue("@id", id);

                using (var dr = cmd.ExecuteReader())
                {
                    Produto produto = null;

                    if (dr.Read())
                    {
                        produto = new Produto
                        {
                            CodProd = Convert.ToInt32(dr["CodProd"]),
                            NomeProd = dr["NomeProd"].ToString(),
                            DescProd = dr["DescProd"].ToString(),
                            PrecoProd = Convert.ToDecimal(dr["PrecoProd"]),
                            CodUsu = Convert.ToInt32(dr["CodUsu"])
                        };
                    }

                    return produto;
                }
            }
        }

        // =============================
        // EXCLUIR PRODUTO
        // =============================
        public void Excluir(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "DELETE FROM tbProduto WHERE CodProd = @id", conexao);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
