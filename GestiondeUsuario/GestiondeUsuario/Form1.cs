using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Servicios;

namespace GestiondeUsuario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtContraseña.Text))
            {
                MessageBox.Show("Completa todos los campos.", "Atencion",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {

                bool acceso = UsuarioServicio.Instancia.Login(txtEmail.Text, txtContraseña.Text); // Verifica credenciales en BD y retorna true si usuario existe, false si no

                if (acceso)
                {
                    MessageBox.Show("¡Bienvenido!", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    new FormPrincipal().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Email o contraseña incorrectos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\nConsulte con el administrador.", "Cuenta bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            new FormCrearUsuario().Show();
            this.Hide();
        }

        private void txtContraseña_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblRecuperar_Click(object sender, EventArgs e)
        {
            new FormRecuperar().Show();
            this.Hide();
        }
    }
}
