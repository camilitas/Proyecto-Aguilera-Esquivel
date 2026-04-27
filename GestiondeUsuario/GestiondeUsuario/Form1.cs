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
            if (string.IsNullOrEmpty(txtNombreUsuario.Text) || string.IsNullOrEmpty(txtContraseña.Text))
            {
                MessageBox.Show("Completá todos los campos.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {

                bool acceso = UsuarioBLL.Instancia.Login(txtNombreUsuario.Text, txtContraseña.Text);

                if (acceso)
                {
                    Usuario usuarioActivo = SessionManager.Instancia.ObtenerUsuarioActivo();
                    if (usuarioActivo.PrimerIngreso)
                    {
                        MessageBox.Show("Bienvenido! Por ser tu primer ingreso debés cambiar tu contraseña.",
                            "Primer ingreso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new FormRecuperar().Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("¡Bienvenido!", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new FormPrincipal().Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nConsulte con el administrador.", "Cuenta bloqueada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            
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

        private void btnHash_Click(object sender, EventArgs e)
        {
            
        }
    }
}
