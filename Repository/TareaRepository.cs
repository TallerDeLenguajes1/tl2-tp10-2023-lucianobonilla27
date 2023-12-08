using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using tl2_tp09_2023_lucianobonilla27.Repository;



namespace tl2_tp09_2023_lucianobonilla27.Models
{
    public class TareaRepository : ITareaRepository
    {
       readonly string CadenaDeConexion;

       public TareaRepository(string CadenaDeConexion){
         this.CadenaDeConexion = CadenaDeConexion;
       }

        public Tarea CrearTareaEnTablero(int idTablero,Tarea tarea)
        {


        var query = @"INSERT INTO Tarea (Id_Tablero, Nombre, Estado, Descripcion, Color, Id_Usuario_Asignado) VALUES (@idTablero, @nombre, @estado, @descripcion, @color, @idUsuarioAsignado)";
        using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);

            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tarea.EstadoT.ToString()));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tarea.IdUsuarioAsignado));

            command.ExecuteNonQuery();

            connection.Close();
        }

        
        return tarea;
       }

       public List<Tarea> ListarTareas(){
             var tareas = new List<Tarea>();
            var query = "SELECT * FROM Tarea";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        IdTablero = Convert.ToInt32(reader["Id_Tablero"]),
                        Nombre = reader["Nombre"].ToString(),
                        EstadoT = Enum.Parse<Tarea.Estado>(reader["Estado"].ToString()),
                        Descripcion = reader["Descripcion"].ToString(),
                        Color = reader["Color"].ToString(),
                        IdUsuarioAsignado = Convert.ToInt32(reader["Id_Usuario_Asignado"])
                    };
                    tareas.Add(tarea);
                }

                connection.Close();
            }

            return tareas;
       }

        
        public void ModificarNombreTarea(int id, string nombreNuevo)
        {
            var query = @"UPDATE Tarea SET Nombre = @nombre WHERE id=@id";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre", nombreNuevo));
                command.Parameters.Add(new SQLiteParameter("@id", id));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void ModificarTarea(int id, Tarea tareaModificada)
        {
            var query = @"UPDATE Tarea 
                        SET Id_Tablero = @idTablero,
                            Nombre = @nombre,
                            Estado = @estado,
                            Descripcion = @descripcion,
                            Color = @color,
                            Id_Usuario_Asignado = @idUsuarioAsignado
                        WHERE Id = @id";

            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@idTablero", tareaModificada.IdTablero));
                command.Parameters.Add(new SQLiteParameter("@nombre", tareaModificada.Nombre));
                command.Parameters.Add(new SQLiteParameter("@estado", (int)tareaModificada.EstadoT));
                command.Parameters.Add(new SQLiteParameter("@descripcion", tareaModificada.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@color", tareaModificada.Color));
                command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tareaModificada.IdUsuarioAsignado));
                command.Parameters.Add(new SQLiteParameter("@id", id));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

         public void ModificarEstadoTarea(int id, int estadoNuevo)
        {
            var query = @"UPDATE Tarea SET estado = @estado WHERE id=@id";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@estado", estadoNuevo));
                command.Parameters.Add(new SQLiteParameter("@id", id));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public Tarea ObtenerTareaPorId(int id)
        {
            var tarea = new Tarea();
            var query = "SELECT * FROM Tarea WHERE Id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
                var reader = command.ExecuteReader();

                // Si no hay filas, lanzar una excepción
                if (!reader.HasRows)
                {
                    connection.Close();
                    throw new Exception("No se encontró la tarea");
                }

                while (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["Id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["Id_Tablero"]);
                    tarea.Nombre = reader["Nombre"].ToString();
                    tarea.EstadoT = Enum.Parse<Tarea.Estado>(reader["Estado"].ToString());
                    tarea.Descripcion = reader["Descripcion"].ToString();
                    tarea.Color = reader["Color"].ToString();
                    tarea.IdUsuarioAsignado = Convert.ToInt32(reader["Id_Usuario_Asignado"]);
                }

                connection.Close();
            }

            return tarea;
        }


       public List<Tarea> ListarPorUsuario(int idUsuario)
        {
            var tareas = new List<Tarea>();
            var query = "SELECT * FROM Tarea WHERE Id_Usuario_Asignado = @idUsuario";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                var reader = command.ExecuteReader();

                // Si no hay filas, lanzar una excepción
                // if (!reader.HasRows)
                // {
                //     connection.Close();
                //     throw new Exception("No se encontraron tareas para el usuario especificado");
                // }

                while (reader.Read())
                {
                    var tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        IdTablero = Convert.ToInt32(reader["Id_Tablero"]),
                        Nombre = reader["Nombre"].ToString(),
                        EstadoT = Enum.Parse<Tarea.Estado>(reader["Estado"].ToString()),
                        Descripcion = reader["Descripcion"].ToString(),
                        Color = reader["Color"].ToString(),
                        IdUsuarioAsignado = Convert.ToInt32(reader["Id_Usuario_Asignado"])
                    };
                    tareas.Add(tarea);
                }

                connection.Close();
            }

            return tareas;
        }


        public List<Tarea> ListarPorTablero(int id){
            var tareas = new List<Tarea>();
            var query = "SELECT * FROM Tarea WHERE Id_tablero = @idTablero";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTablero", id));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        IdTablero = Convert.ToInt32(reader["Id_Tablero"]),
                        Nombre = reader["Nombre"].ToString(),
                        EstadoT = Enum.Parse<Tarea.Estado>(reader["Estado"].ToString()),
                        Descripcion = reader["Descripcion"].ToString(),
                        Color = reader["Color"].ToString(),
                        IdUsuarioAsignado = Convert.ToInt32(reader["Id_Usuario_Asignado"])
                    };
                    tareas.Add(tarea);
                }

                connection.Close();
            }

            return tareas;
        }

        public List<Tarea> ListarTareasPorTableros(List<int> idsTableros)
        {
            var tareas = new List<Tarea>();

            if (idsTableros == null || !idsTableros.Any())
            {
                throw new ArgumentException("La lista de IDs de tableros no puede ser nula o vacía");
            }

            // Construir la cadena IN dinámicamente
            var inClause = string.Join(",", idsTableros);

            // Consulta SQL con la cadena IN construida
            var query = $"SELECT * FROM Tarea WHERE Id_Tablero IN ({inClause})";

            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Si no hay filas, lanzar una excepción
                    // if (!reader.HasRows)
                    // {
                    //     throw new Exception("No se encontraron tareas para los tableros especificados");
                    // }

                    while (reader.Read())
                    {
                        var tarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            IdTablero = Convert.ToInt32(reader["Id_Tablero"]),
                            Nombre = reader["Nombre"].ToString(),
                            EstadoT = Enum.Parse<Tarea.Estado>(reader["Estado"].ToString()),
                            Descripcion = reader["Descripcion"].ToString(),
                            Color = reader["Color"].ToString(),
                            IdUsuarioAsignado = Convert.ToInt32(reader["Id_Usuario_Asignado"])
                        };
                        tareas.Add(tarea);
                    }
                        connection.Close();

                }
            }

            return tareas;
        }




        public void EliminarTarea(int id)
        {
            var query = "DELETE FROM Tarea WHERE Id = @id";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void AsignarTareaAUsuario(int idTarea, int idUsuario)
        {
            var query = "UPDATE Tarea SET Id_Usuario_Asignado = @idUsuario WHERE Id = @idTarea";
            using (SQLiteConnection connection = new SQLiteConnection(CadenaDeConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}