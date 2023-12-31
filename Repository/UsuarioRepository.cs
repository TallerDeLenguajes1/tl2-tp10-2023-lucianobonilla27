using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace tl2_tp09_2023_lucianobonilla27.Models
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.bd;Cache=Shared";



    public void CrearUsuario(Usuario usuario)
    {
         var query = @"INSERT INTO usuario (nombre_de_usuario) VALUES (@nombre_de_usuario)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
                command.ExecuteNonQuery();

                connection.Close();   
            }
    }

    public void ModificarUsuario(int id, Usuario usuarioModificado)
    {
       var query = $"UPDATE Usuario SET nombre_de_usuario = (@nombre_de_usuario) WHERE id=@idUsuario";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuarioModificado.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@idUsuario", id));

                command.ExecuteNonQuery();

                connection.Close();   
            }

    }

    public List<Usuario> ListarUsuarios()
    {
       var queryString = @"SELECT * FROM usuario;";
            List<Usuario> usuarios = new List<Usuario>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario();
                        usuario.Id = Convert.ToInt32(reader["id"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
        return usuarios;
    }

    public Usuario ObtenerUsuarioPorId(int id)
    {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            var usuario = new Usuario();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM usuario WHERE id = @idUsuario";
            command.Parameters.Add(new SQLiteParameter("@idUsuario", id));
            connection.Open();
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                }
            }
            connection.Close();

            return (usuario);
    }

    public void EliminarUsuario(int id)
    {
        var query = $"DELETE FROM usuario WHERE id = @id;";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@id", id));

                command.ExecuteNonQuery();

                connection.Close();   
            }
    }

    }

   
}
