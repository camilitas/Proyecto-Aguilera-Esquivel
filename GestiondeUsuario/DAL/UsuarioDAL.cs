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
        private string connectionString = "Server=.;Database=GestionUsuarios;Trusted_Connection=True;";

        public Usuario ObtenerPorEmailYContraseña(string email, string contraseña)
        {
            Usuario usuario = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Email = @Email AND Contraseña = @Contraseña";
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
                string query = "INSERT INTO Usuarios (Nombre, Email, Contraseña) VALUES (@Nombre, @Email, @Contraseña)";
                SqlCommand cmd = new SqlCommand(query, con);
                UsuarioMPP.MapearParametros(cmd, u);

                con.Open();
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }
    }
}
