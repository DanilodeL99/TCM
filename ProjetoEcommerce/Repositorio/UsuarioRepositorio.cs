using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System;
using System.Data;

namespace ProjetoEcommerce.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly string _conexaoMySQL;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        // =============================
        // OBTER USUÁRIO (pelo código ou nome)
        // =============================

        public Usuario ObterUsuario(string nome)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var cmd = new MySqlCommand("SELECT * FROM tbUsuario WHERE NomeUsu = @nome", conexao);
            cmd.Parameters.AddWithValue("@nome", nome);

            using var dr = cmd.ExecuteReader();
            Usuario usuario = null;
            if (dr.Read())
            {
                usuario = new Usuario
                {
                    CodUsu = Convert.ToInt32(dr["CodUsu"]),
                    NomeUsu = dr["NomeUsu"].ToString(),
                    SenhaUsu = dr["SenhaUsu"].ToString()
                };
            }
            return usuario;
        }


        // =============================
        // CADASTRAR USUÁRIO
        // =============================

        public void Cadastrar(Usuario usuario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO tbUsuario (CodUsu, NomeUsu, SenhaUsu) VALUES (@cod, @nome, @senha)", conexao);

                cmd.Parameters.AddWithValue("@cod", usuario.CodUsu);
                cmd.Parameters.AddWithValue("@nome", usuario.NomeUsu);
                cmd.Parameters.AddWithValue("@senha", usuario.SenhaUsu);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
