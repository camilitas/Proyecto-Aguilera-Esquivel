using BE;
using BLL;
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
            SessionManagerBLL.Instancia.CerrarSesion(); // Limpiamos la sesion activa

            //volvemos al login
            new Form1().Show();
            this.Close();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Usuario usuario = SessionManagerBLL.Instancia.ObtenerUsuarioActivo(); // Obtenemos el usuario activo desde el SessionManager 
            lblBienvenida.Text = "Bienvenida, " + usuario.Nombre + "!"; //mostramos su nombre en el label de bienvenida
        }
    }
}
