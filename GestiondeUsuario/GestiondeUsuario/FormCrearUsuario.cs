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


namespace GestiondeUsuario
{
    public partial class FormCrearUsuario : Form
    {
        public FormCrearUsuario()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtDNI.Text)  ||
                string.IsNullOrEmpty(txtContraseña.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Atencion!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            if(txtContraseña.Text != txtConfirmarContraseña.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtDNI.Text, out int dni))
            {
                MessageBox.Show("El DNI debe ser un numero.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!EncriptadorBLL.ContraseñaSegura(txtContraseña.Text))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas y números.", "Contraseña insegura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // creamos el objeto Usuario con los datos del formulario
            Usuario nuevo = new Usuario
            {
                Nombre = txtNombre.Text,
                Email = txtEmail.Text,
                DNI = dni,
                Contraseña = txtContraseña.Text
                // FechaCreacion y Activo los asigna MPP automaticamente
            };

            bool resultado = UsuarioServicio.Instancia.CrearUsuario(nuevo);  // le pedimos a la BLL que procese la creacion (valida + encripta + guarda)

            if (resultado)
            {
                MessageBox.Show("Usuario creado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al crear el usuario. Verifique los datos ingresados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtEmail.Text = "";
            txtDNI.Text = "";
            txtContraseña.Text = "";
            txtConfirmarContraseña.Text = "";
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Close();
        }
    }
}
