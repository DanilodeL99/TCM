using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;

namespace ProjetoEcommerce.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly string _conexaoMySQL;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        // =====================================================
        // LOGIN
        // =====================================================
        public Usuario Login(string nome, string senha)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var sql = @"SELECT * FROM tbUsuario 
                        WHERE NomeUsu = @nome AND SenhaUsu = @senha";

            var cmd = new MySqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@nome", nome);
            cmd.Parameters.AddWithValue("@senha", senha);

            using var dr = cmd.ExecuteReader();

            if (!dr.Read())
                return null;

            return new Usuario
            {
                CodUsu = Convert.ToInt32(dr["CodUsu"]),
                NomeUsu = dr["NomeUsu"].ToString(),
                SenhaUsu = dr["SenhaUsu"].ToString()
            };
        }

        // =====================================================
        // CADASTRAR USUÁRIO
        // =====================================================
        public bool Cadastrar(Usuario usuario)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var sql = @"INSERT INTO tbUsuario (CodUsu, NomeUsu, SenhaUsu) 
                        VALUES (@cod, @nome, @senha)";

            var cmd = new MySqlCommand(sql, conexao);

            cmd.Parameters.AddWithValue("@cod", usuario.CodUsu);
            cmd.Parameters.AddWithValue("@nome", usuario.NomeUsu);
            cmd.Parameters.AddWithValue("@senha", usuario.SenhaUsu);

            return cmd.ExecuteNonQuery() > 0;
        }

        // =====================================================
        // OBTER USUÁRIO
        // =====================================================
        public Usuario ObterUsuario(string nome)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var cmd = new MySqlCommand("SELECT * FROM tbUsuario WHERE NomeUsu = @nome", conexao);
            cmd.Parameters.AddWithValue("@nome", nome);

            using var dr = cmd.ExecuteReader();

            if (!dr.Read())
                return null;

            return new Usuario
            {
                CodUsu = Convert.ToInt32(dr["CodUsu"]),
                NomeUsu = dr["NomeUsu"].ToString(),
                SenhaUsu = dr["SenhaUsu"].ToString()
            };
        }
    }
}
