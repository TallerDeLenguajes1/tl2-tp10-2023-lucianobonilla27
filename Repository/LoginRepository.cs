using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using tl2_tp09_2023_lucianobonilla27.Models;

namespace tl2_tp10_2023_lucianobonilla27.Repository
{

    public interface ILoginRepository
    {
        bool ValidarCredenciales(string nombreDeUsuario,string contrasenia);
        string ObtenerRol(string nombreDeUsuario);
        Usuario ObtenerUsuario(string nombreDeUsuario, string contrasenia);

    }
    public class LoginRepository : ILoginRepository
    {
    readonly string CadenaDeConexion;

     public LoginRepository(string CadenaDeConexion){
         this.CadenaDeConexion = CadenaDeConexion;
       }   
    public bool ValidarCredenciales(string nombreDeUsuario, string contrasenia)
    {
        string query = "SELECT COUNT(*) FROM Usuario WHERE Nombre_De_Usuario = @nombreDeUsuario AND Contrasenia = @contrasenia";

        using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombreDeUsuario", nombreDeUsuario);
                command.Parameters.AddWithValue("@contrasenia", contrasenia);

                int count = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return count > 0;
            }
        }
    }

    public string ObtenerRol(string nombreDeUsuario)
    {
        string query = "SELECT Rol FROM Usuario WHERE Nombre_De_Usuario = @nombreDeUsuario";

        using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombreDeUsuario", nombreDeUsuario);

                object result = command.ExecuteScalar();

                connection.Close();

                return result?.ToString();
            }
        }
    }
    public Usuario ObtenerUsuario(string nombreDeUsuario, string contrasenia)
    {
        string query = "SELECT * FROM Usuario WHERE Nombre_De_Usuario = @nombreDeUsuario AND Contrasenia = @contrasenia";

        using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombreDeUsuario", nombreDeUsuario);
                command.Parameters.AddWithValue("@contrasenia", contrasenia);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Usuario usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            NombreDeUsuario = reader["Nombre_De_Usuario"].ToString(),
                            Contrasenia = reader["Contrasenia"].ToString(),
                            RolUsuario = (Usuario.Rol)Enum.Parse(typeof(Usuario.Rol), reader["Rol"].ToString())
                        };

                        connection.Close();
                        return usuario;
                    }
                }
            }

            connection.Close();
            return null;
        }
    }
    }
}