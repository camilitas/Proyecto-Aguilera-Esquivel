using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace MPP
{
    public class UsuarioMPP
    {
        public static Usuario MapearAUsuario(SqlDataReader reader) // convierte filas de la base de datos en objetos Usuario (de la BD hacia el programa)
        {
            return new Usuario()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString(),
                Email = reader["Email"].ToString(),
                Contraseña = reader["Contraseña"].ToString(),
                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
            };
        }

             // Convierte un Usuario en parámetros SQL (del programa hacia la BD)
        public static void MapearParametros(SqlCommand cmd, Usuario u)
        {
            cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@Email", u.Email);
            cmd.Parameters.AddWithValue("@Contraseña", u.Contraseña);
            cmd.Parameters.AddWithValue("@FechaCreacion", u.FechaCreacion);

        }
    }
}
