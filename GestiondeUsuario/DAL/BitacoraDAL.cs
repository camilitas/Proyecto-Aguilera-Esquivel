using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   
        public class BitacoraDAL
        {
            private string connectionString = "Data Source=DESKTOP-FEJ1OE8\\SQLEXPRESS;Initial Catalog=GestionUsuarios;Integrated Security=True";

            public void Guardar(string usuario, string accion)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Bitacora (Usuario, Accion, Fecha) VALUES (@Usuario, @Accion, @Fecha)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Accion", accion);
                    cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
}
