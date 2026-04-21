using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using MPP;

namespace DAL
{
    public class UsuarioDAL
    {
        private string connectionString = "Data Source=DESKTOP-FEJ1OE8\\SQLEXPRESS;Initial Catalog=GestionUsuarios;Integrated Security=True";

        public Usuario ObtenerPorEmailYContraseña(string email, string contraseña)
        {
            Usuario usuario = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Email = @Email AND Contraseña = @Contraseña"; //consulta SQL para obtener un usuario que coincida con el email y la contraseña proporcionados. Utiliza parámetros para evitar inyecciones SQL.
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    usuario = UsuarioMPP.MapearAUsuario(reader);
                }
            }

            return usuario;
        }

        public bool Insertar(Usuario u)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Usuarios (Nombre, Email, Contraseña, DNI, FechaCreacion, Activo) VALUES (@Nombre, @Email, @Contraseña, @DNI, @FechaCreacion, @Activo)";
                SqlCommand cmd = new SqlCommand(query, con);
                UsuarioMPP.MapearParametros(cmd, u); // se encarga de asignar cada campo del objeto Usuario a su parámetro SQL

                con.Open();
                int filas = cmd.ExecuteNonQuery(); //ejecuta el INSERT y devuelve cuantas filas se insertaron
                return filas > 0;  //si se inserto al menos 1 fila, fue exitoso
            }
        }
    }
}
