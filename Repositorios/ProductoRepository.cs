using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace MisProductos;
public class ProductoRepository
{
    string cadenaConexion = "Data Source = DB/Tienda.db";
    public void Alta(Producto producto)
    {
        using (var conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();

            string sql = "INSERT INTO Producto (Descripcion, Precio) VALUES (@desc, @prec)";

            using var comando = new SqliteCommand(sql, conexion);

            comando.Parameters.Add(new SqliteParameter("@desc", producto.Descripcion));
            comando.Parameters.Add(new SqliteParameter("@prec", producto.Precio));
        }


        comando.ExecuteNonQuery();
    }

    public List<Producto> GetAll()
    {
        List<Producto> listaProductos = new List<Producto>();
        using (var connection = new SqliteConnection(cadenaConexion))
        {
            connection.Open();

            string sql = "SELECT idProducto, Descripcion, Precion FROM Productos";
            var command = new SqliteCommand(sql, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaProductos.Add(new Producto
                    (
                        IdProducto = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio"))
                    ));
                }
            }
        }
        return listaProductos;
    }


}