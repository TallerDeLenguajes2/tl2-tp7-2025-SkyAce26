using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace MisProductos;

public class PresupuestoRepository
{
    string connectionString = "Data Source=DB/Tienda_final.db";

    public int Alta(Presupuesto presupuesto)
    {
        int nuevoId = 0;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@name, @date)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@name", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@date", presupuesto.FechaCreacion);

                command.ExecuteNonQuery();

                command.CommandText = "SELECT last_insert_rowid()";
                nuevoId = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return nuevoId;
    }

    public void AgregarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sqlDetalle = "INSERT INTO PresupuestosDetalle (idPresupuesto,idProducto, Cantidad) VALUES(@idPresu,@idProd, @cant)";

            using (var command = new SqliteCommand(sqlDetalle, connection))
            {
                command.Parameters.AddWithValue("@idPresu", idPresupuesto);
                command.Parameters.AddWithValue("@idProd", idProducto);
                command.Parameters.AddWithValue("@cant", cantidad);

                command.ExecuteNonQuery();
            }
        }
    }

    public Presupuesto Detalles(int id)
    {
        Presupuesto presupuesto = null;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Presupuestos p INNER JOIN PresupuestosDetalle pd ON p.idPresupuesto = pd.idPresupuesto INNER JOIN Productos pr ON pd.idProducto = pr.idProducto WHERE p.idPresupuesto = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        presupuesto = new Presupuesto
                        {
                            IdPresupuesto = reader.GetInt32(reader.GetOrdinal("IdPresupuesto")),
                            NombreDestinatario = reader.GetString(reader.GetOrdinal("NombreDestinatario")),
                            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            PresupuestoDetalle = new List<PresupuestoDetalle>()
                        };
                    }
                }
            }

            if (presupuesto != null)
            {
                string sqlDetalle = @"SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                                  FROM PresupuestosDetalle pd
                                  JOIN Productos p ON pd.idProducto = p.IdProducto
                                  WHERE pd.idPresupuesto = @id";
                using (var command = new SqliteCommand(sqlDetalle, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            presupuesto.PresupuestoDetalle.Add(new PresupuestoDetalle
                            {
                                Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                Producto = new Producto
                                {
                                    IdProducto = reader.GetInt32(reader.GetOrdinal("idProducto")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                    Precio = reader.GetDouble(reader.GetOrdinal("Precio"))
                                }
                            });
                        }
                    }
                }
            }

        }

        return presupuesto;
    }

    public List<Presupuesto> GetAll()
    {
        List<Presupuesto> listaPresupuesto = new List<Presupuesto>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT IdPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";

            var command = new SqliteCommand(sql, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaPresupuesto.Add(new Presupuesto
                    {
                        IdPresupuesto = reader.GetInt32(reader.GetOrdinal("IdPresupuesto")),
                        NombreDestinatario = reader.GetString(reader.GetOrdinal("NombreDestinatario")),
                        FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                    });
                }
            }
        }
        return listaPresupuesto;
    }

    /*
        public int Eliminar(int id)
        {
            int resultado;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Presupuestos WHERE IdPresupuesto = @id";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    resultado = command.ExecuteNonQuery();
                }
            }
            return resultado;
        }
    */

    public bool Eliminar(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using (var pragma = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
        {
            pragma.ExecuteNonQuery();
        }

        using var transaction = connection.BeginTransaction();
        try
        {
            string sqlDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
            using (var cmdDetalle = new SqliteCommand(sqlDetalle, connection, transaction))
            {
                cmdDetalle.Parameters.AddWithValue("@id", id);
                cmdDetalle.ExecuteNonQuery();
            }

            string sqlCabecera = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            using (var cmdCabecera = new SqliteCommand(sqlCabecera, connection, transaction))
            {
                cmdCabecera.Parameters.AddWithValue("@id", id);
                int filasAfectadas = cmdCabecera.ExecuteNonQuery();

                transaction.Commit();

                return filasAfectadas == 1;
            }
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

}