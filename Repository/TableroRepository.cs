using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using tl2_tp09_2023_lucianobonilla27.Repository;



namespace tl2_tp09_2023_lucianobonilla27.Models
{
    public class TableroRepository : ITableroRepository
    {
       readonly string CadenaDeConexion;

       public TableroRepository(string CadenaDeConexion){
         this.CadenaDeConexion = CadenaDeConexion;
       }   
        public void CrearTablero(Tablero t){
            var query = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) VALUES (@id, @nombre, @descripcion)";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@id", t.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombre", t.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descripcion", t.Descripcion));

                command.ExecuteNonQuery();

                connection.Close();   
            }
        }
        public void ModificarTablero(int id, Tablero tableroModificado)
        {
            var query = @"UPDATE Tablero SET nombre = @nombre, descripcion = @descripcion, id_usuario_propietario = @idPropietario WHERE id = @id;";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre", tableroModificado.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descripcion", tableroModificado.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@idPropietario", tableroModificado.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@id", id));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Tablero> ListarTableros()
        {
            var tableros = new List<Tablero>();
            var query = "SELECT * FROM Tablero";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero);
                }

                connection.Close();
            }

            return tableros;
        }

        public Tablero ObtenerTableroPorId(int id)
        {
            var query = "SELECT * FROM Tablero WHERE id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var tablero = new Tablero();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                }

                connection.Close();

                // Lanzar una excepción si no se encontró ningún tablero
                if (tablero.Id == 0)
                {
                    throw new Exception("Tablero no encontrado");
                }

                return tablero;
            }
        }
        
        public Tablero ObtenerTableroPorNombre(string nombre)
        {
            var query = "SELECT * FROM Tablero WHERE nombre = @nombre";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var tablero = new Tablero();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                }
                connection.Close();
                    // Lanzar una excepción si no se encontró ningún tablero
                if (tablero.Id == 0)
                {
                    throw new Exception("Tablero no encontrado");
                }
                return tablero;
            }

        
        }

        public List<Tablero> ListarTablerosConTareasAsignadas(int idUsuario)
        {
            var tablerosConTareasAsignadas = new List<Tablero>();

            var query = @"
                SELECT DISTINCT Tablero.*
                FROM Tablero
                JOIN Tarea ON Tablero.Id = Tarea.Id_Tablero
                WHERE Tarea.Id_Usuario_Asignado = @idUsuario
            ";

            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();

                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            IdUsuarioPropietario = Convert.ToInt32(reader["Id_Usuario_Propietario"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString()
                        };

                        tablerosConTareasAsignadas.Add(tablero);
                    }
                }

                connection.Close();
            }

            return tablerosConTareasAsignadas;
        }

        public int ObtenerIdTableroPorUsuario(int idUsuarioPropietario)
        {
            var query = "SELECT id FROM Tablero WHERE id_usuario_propietario = @idUsuarioPropietario";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUsuarioPropietario", idUsuarioPropietario));

                // Intentamos ejecutar el comando y obtener el resultado
                var resultado = command.ExecuteScalar();

                // Cerramos la conexión antes de retornar el resultado
                connection.Close();

                // Si el resultado no es nulo, convertimos a entero y lo retornamos
                if (resultado != null)
                {
                    return Convert.ToInt32(resultado);
                }

                // Si no hay resultado, retornamos un valor predeterminado (por ejemplo, -1)
                return -1;
            }
        }



        public void EliminarTablero(int id)
        {
            var query = "DELETE FROM Tablero WHERE id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Tablero> ListarTableroPorUsuario(int id)
        {
            var tableros = new List<Tablero>();
            var query = "SELECT * FROM Tablero WHERE id_usuario_propietario = @id";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
                var reader = command.ExecuteReader();

                // Si no hay tableros, lanzar una excepción
                // if (!reader.HasRows)
                // {
                //     connection.Close();
                //     throw new Exception("No se encontraron tableros para el usuario");
                // }

                while (reader.Read())
                {
                    var tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero);
                }

                connection.Close();
            }

            return tableros;
        }

    }
    
}