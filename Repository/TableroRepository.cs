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
        private string cadenaConexion = "Data Source=DB/kanban.bd;Cache=Shared";
        public void CrearTablero(Tablero t){
            var query = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) VALUES (@id, @nombre, @descripcion)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
            var query = @"UPDATE Tablero SET nombre = @nombre, descripcion = @descripcion WHERE id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre", tableroModificado.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descripcion", tableroModificado.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@id", id));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Tablero> ListarTableros()
        {
            var tableros = new List<Tablero>();
            var query = "SELECT * FROM Tablero";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };

                    connection.Close();
                    return tablero;
                }
            }

            return null;
        }

        public void EliminarTablero(int id)
        {
            var query = "DELETE FROM Tablero WHERE id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
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
    }
    
}