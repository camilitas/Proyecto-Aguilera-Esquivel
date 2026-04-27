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
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SessionManager.Instancia.CerrarSesion(); // Limpiamos la sesion activa

            //volvemos al login
            new Form1().Show();
            this.Close();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Usuario usuario = SessionManager.Instancia.ObtenerUsuarioActivo(); // Obtenemos el usuario activo desde el SessionManager 
            lblBienvenida.Text = "Bienvenida, " + usuario.Nombre + "!"; //mostramos su nombre en el label de bienvenida

            bool esAdmin = usuario.Rol == "Admin";
            menuAdmin.Enabled = esAdmin;

            menuMaestro.Enabled = esAdmin;
            menuVenta.Enabled = esAdmin;
            menuCompras.Enabled = esAdmin;
            menuReporte.Enabled = esAdmin;
        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SessionManager.Instancia.HaySesionActiva())
                MessageBox.Show("Ya hay una sesión activa: " +
                    SessionManager.Instancia.ObtenerUsuarioActivo().NombreUsuario,
                    "Sesión activa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormRecuperar().Show();
            this.Hide();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager.Instancia.CerrarSesion();
            new Form1().Show();
            this.Close();
        }

        private void gestionDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormGU().Show();
            this.Hide();
        }
    }
}
