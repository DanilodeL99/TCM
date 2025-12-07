using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace ProjetoEcommerce.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly string _conexaoMySQL;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        // LOGIN - retorna usuario se OK
        public Usuario Login(string nome, string senha)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var cmd = new MySqlCommand(@"
                SELECT CodUsu, NomeUsu, SenhaUsu
                FROM tbUsuario
                WHERE NomeUsu = @nome AND SenhaUsu = @senha
            ", conexao);

            cmd.Parameters.AddWithValue("@nome", nome);
            cmd.Parameters.AddWithValue("@senha", senha);

            using var dr = cmd.ExecuteReader();
            if (!dr.Read()) return null;

            return new Usuario
            {
                CodUsu = Convert.ToInt32(dr["CodUsu"]),
                NomeUsu = dr["NomeUsu"].ToString(),
                SenhaUsu = dr["SenhaUsu"].ToString()
            };
        }

        // Obter por nome (usado em outras partes)
        public Usuario ObterUsuario(string nome)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var cmd = new MySqlCommand("SELECT CodUsu, NomeUsu, SenhaUsu FROM tbUsuario WHERE NomeUsu = @nome", conexao);
            cmd.Parameters.AddWithValue("@nome", nome);

            using var dr = cmd.ExecuteReader();
            if (!dr.Read()) return null;

            return new Usuario
            {
                CodUsu = Convert.ToInt32(dr["CodUsu"]),
                NomeUsu = dr["NomeUsu"].ToString(),
                SenhaUsu = dr["SenhaUsu"].ToString()
            };
        }

        // Cadastrar usuário (retorna id gerado)
        public int Cadastrar(Usuario usuario)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO tbUsuario (NomeUsu, SenhaUsu) VALUES (@nome, @senha);
            ", conexao);

            cmd.Parameters.AddWithValue("@nome", usuario.NomeUsu);
            cmd.Parameters.AddWithValue("@senha", usuario.SenhaUsu);

            cmd.ExecuteNonQuery();
            // obter id gerado
            var id = (int)cmd.LastInsertedId;
            return id;
        }
    }
}
