using BE;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GestiondeUsuario
{
    public partial class FormRecuperar : Form
    {
        public FormRecuperar()
        {
            InitializeComponent();
        }

        private void FormRecuperar_Load(object sender, EventArgs e)
        {

        }

        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            // Validación: campos vacíos
            if (string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtNuevaPass.Text) ||
                string.IsNullOrEmpty(txtConfirmarPass.Text))
            {
                MessageBox.Show("Completá todos los campos.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación: contraseñas iguales
            if (txtNuevaPass.Text != txtConfirmarPass.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!EncriptadorBLL.ContraseñaSegura(txtNuevaPass.Text))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas y números.", "Contraseña insegura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Llamada a BLL
            bool ok = UsuarioBLL.Instancia.RecuperarContraseña(
                txtEmail.Text,
                txtContraseñaActual.Text,
                txtNuevaPass.Text
            );

            if (ok)
            {
                MessageBox.Show("Contraseña actualizada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Volver al login
                new Form1().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Email o contraseña actual incorrectos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Close();
        }
    }
}
