using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                DNI = Convert.ToInt32(reader["DNI"]),
                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                Activo = Convert.ToBoolean(reader["Activo"])
            };
        }

             // Convierte un Usuario en parámetros SQL (del programa hacia la BD)
        public static void MapearParametros(SqlCommand cmd, Usuario u)
        {
            cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@Email", u.Email);
            cmd.Parameters.AddWithValue("@Contraseña", u.Contraseña);
            cmd.Parameters.AddWithValue("@DNI", u.DNI);
            cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.AddWithValue("@Activo", true);
        }
    }
}
