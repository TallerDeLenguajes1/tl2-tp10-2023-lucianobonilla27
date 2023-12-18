using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace tl2_tp09_2023_lucianobonilla27.Models
{
    public class UsuarioRepository : IUsuarioRepository
    {
       readonly string CadenaDeConexion;

       public UsuarioRepository(string CadenaDeConexion){
         this.CadenaDeConexion = CadenaDeConexion;
       }

    public void CrearUsuario(Usuario usuario)
    {
         var query = @"INSERT INTO usuario (nombre_de_usuario,rol,contrasenia) VALUES (@nombre_de_usuario,@rol,@contrasenia)";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@rol", usuario.RolUsuario));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", usuario.Contrasenia));

                command.ExecuteNonQuery();

                connection.Close();   
            }
    }

    public void ModificarUsuario(int id, Usuario usuarioModificado)
    {
       var query = $"UPDATE Usuario SET nombre_de_usuario = @nombre_de_usuario,rol = @rol,contrasenia = @contrasenia  WHERE id=@idUsuario";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuarioModificado.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@rol", (int)usuarioModificado.RolUsuario));
                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuarioModificado.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", usuarioModificado.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@idUsuario", id));

                command.ExecuteNonQuery();

                connection.Close();   
            }

    }

    public List<Usuario> ListarUsuarios()
    {
       var queryString = @"SELECT * FROM usuario;";
            List<Usuario> usuarios = new List<Usuario>();
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
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
                        usuario.RolUsuario = Enum.Parse<Usuario.Rol>(reader["rol"].ToString());

                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
        return usuarios;
    }

    public Usuario ObtenerUsuarioPorId(int? id)
    {
        if (id == null)
        {
            return null;
        }
        SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion);
        var usuario = new Usuario();
        SQLiteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM usuario WHERE id = @idUsuario";
        command.Parameters.Add(new SQLiteParameter("@idUsuario", id));
        connection.Open();
        using (SQLiteDataReader reader = command.ExecuteReader())
        {
            // Si no hay filas, lanzar una excepción
            if (!reader.HasRows)
            {
                connection.Close();
                throw new Exception("No se encontró el usuario");
            }

            while (reader.Read())
            {
                usuario.Id = Convert.ToInt32(reader["id"]);
                usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                usuario.Contrasenia = reader["contrasenia"].ToString();
                usuario.RolUsuario = Enum.Parse<Usuario.Rol>(reader["rol"].ToString());
            }
        }
        connection.Close();

        return usuario;
    }


    public Usuario ObtenerUsuarioPorNombre(string nombre)
    {
        SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion);
        var usuario = new Usuario();
        SQLiteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM usuario WHERE nombre_de_usuario = @nombre";
        command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
        connection.Open();
        using (SQLiteDataReader reader = command.ExecuteReader())
        {
            // Si no hay filas, lanzar una excepción
            if (!reader.HasRows)
            {
                connection.Close();
                throw new Exception("No se encontró el usuario");
            }

            while (reader.Read())
            {
                usuario.Id = Convert.ToInt32(reader["id"]);
                usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                usuario.Contrasenia = reader["contrasenia"].ToString();
                usuario.RolUsuario = Enum.Parse<Usuario.Rol>(reader["rol"].ToString());
            }
        }
        connection.Close();

        return usuario;
    }


    public void EliminarUsuario(int id)
    {
        var query = $"DELETE FROM usuario WHERE id = @id;";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
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
