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

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Usuario usuario = SessionManager.Instancia.ObtenerUsuarioActivo();
            lblBienvenida.Text = "Bienvenido, " + usuario.Nombre + "!";

            // Composite: permisos según perfil
            menuAdmin.Enabled = PerfilBLL.Instancia.TienePermiso(usuario.Rol, "GestionUsuarios");
            menuMaestro.Enabled = PerfilBLL.Instancia.TienePermiso(usuario.Rol, "GestionUsuarios");
            menuVenta.Enabled = PerfilBLL.Instancia.TienePermiso(usuario.Rol, "GestionUsuarios");
            menuCompras.Enabled = PerfilBLL.Instancia.TienePermiso(usuario.Rol, "GestionUsuarios");
            menuReporte.Enabled = PerfilBLL.Instancia.TienePermiso(usuario.Rol, "VerBitacora");
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
            DialogResult confirm = MessageBox.Show(
               "¿Estás seguro que querés cerrar sesión?",
               "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                UsuarioBLL.Instancia.Logout(); // usa el método que registra en bitácora
                new Form1().Show();
                this.Close();
            }
        }

        private void gestionDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormGU().Show();
            this.Hide();
        }

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bitacoraEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormBitacora().Show();
            this.Hide();
        }
    }
}
