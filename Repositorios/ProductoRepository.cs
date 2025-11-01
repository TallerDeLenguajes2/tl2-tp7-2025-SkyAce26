    using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace MisProductos;
public class ProductoRepository
{
    string connectionString = "Data Source=DB/Tienda_final.db";
    public int Alta(Producto producto)
    {
        int nuevoId = 0;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@desc, @prec)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@desc", producto.Descripcion);
                command.Parameters.AddWithValue("@prec", producto.Precio);

                command.ExecuteNonQuery();

                command.CommandText = "SELECT last_insert_rowid()";
                nuevoId = Convert.ToInt32(command.ExecuteScalar());
            }

        }

        return nuevoId;
    }

    public List<Producto> GetAll()
    {
        List<Producto> listaProductos = new List<Producto>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT idProducto, Descripcion, Precio FROM Productos";

            var command = new SqliteCommand(sql, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaProductos.Add(new Producto
                    {
                        IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Precio = reader.GetDouble(reader.GetOrdinal("Precio")),
                    });
                }
            }
        }
        return listaProductos;
    }

    public int Baja(int id)
    {
        int resultado;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "DELETE FROM Productos WHERE idProducto = @identificador";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@identificador", id);
                resultado = command.ExecuteNonQuery();
            }
        }
        return resultado;
    }

    public int ModificarProducto(int id, Producto producto)
    {
        int resultado;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "UPDATE productos SET Descripcion = @desc, Precio = @prec WHERE idProducto = @identificador";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@identificador", id);
                command.Parameters.AddWithValue("@desc", producto.Descripcion);
                command.Parameters.AddWithValue("@prec", producto.Precio);
                resultado = command.ExecuteNonQuery();
            }
        }
        return resultado;
    }

    public Producto Detalles(int id)
    {
        Producto producto = new Producto();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM productos WHERE idProducto = @identificador";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@identificador", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto.Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"));
                        producto.Precio = reader.GetDouble(reader.GetOrdinal("Precio"));
                    }
                }
            }

            return producto;
        }
    }

}